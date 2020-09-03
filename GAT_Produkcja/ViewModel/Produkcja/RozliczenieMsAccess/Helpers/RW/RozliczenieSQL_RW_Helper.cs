using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Migrations;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.Strategy;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.Strategy.Factory;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Modele;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.RW
{
    public class RozliczenieSQL_RW_Helper : IRozliczenieSQL_RW_Helper
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ISurowiecSubiektStrategy surowiecSubiektStrategy;
        private readonly NazwaTowaruSubiektHelper nazwaTowaruSubiekt;
        private IEnumerable<tblProdukcjaRuchTowar> rolkiRWZOdpademDlaZlecenia;

        #region CTOR
        public RozliczenieSQL_RW_Helper(IUnitOfWork unitOfWork,
                                      ISurowiecSubiektStrategyFactory surowiecSubiektStrategyFactory)
        {
            this.unitOfWork = unitOfWork;
            surowiecSubiektStrategy = surowiecSubiektStrategyFactory.PobierzStrategie(SurowiecSubiektFactoryEnum.ZNazwy);
            nazwaTowaruSubiekt = new NazwaTowaruSubiektHelper();
        }
        #endregion

        #region LoadAsync
        public async Task LoadAsync()
        {
        }
        #endregion

        #region Uzupelnienie RW o IloscKG oraz wartosc

        public async Task DodajIlosciKgIWartoscDoRW(ObservableCollection<tblProdukcjaRozliczenie_RW> listaRWSurowca,
                                              ObservableCollection<tblProdukcjaRozliczenie_PW> listaPwWyrobuGotowego)
        {
            if (listaRWSurowca is null || !listaRWSurowca.Any()) throw new ArgumentException("Brak listy surowca do analizy");
            if (listaPwWyrobuGotowego is null || !listaPwWyrobuGotowego.Any()) throw new ArgumentException("Brak listy surowca do analizy");

            var sumaIlosciKgDlaPW = listaPwWyrobuGotowego.Sum(s => s.Ilosc_kg) + listaPwWyrobuGotowego.Sum(s => s.Odpad_kg);
            var wagaOdpadu = await PobierzWageOdpaduDlaZlecenia(listaPwWyrobuGotowego.First().IDZlecenie);
            var sumarycznaWaga = sumaIlosciKgDlaPW + wagaOdpadu;

            foreach (var poz in listaRWSurowca)
            {
                poz.Ilosc = sumarycznaWaga * poz.Udzial;
                poz.Wartosc = poz.Ilosc * poz.CenaJednostkowa;
            }
        }

        public async Task<decimal> PobierzWageOdpaduDlaZlecenia(int idZlecenieProdukcyjne)
        {
            rolkiRWZOdpademDlaZlecenia = await PobierzListeRolekRozchodowanychINiezaksiegowanychDlaZlecenia(idZlecenieProdukcyjne);

            if (rolkiRWZOdpademDlaZlecenia is null
                || !rolkiRWZOdpademDlaZlecenia.Any()) return 0;

            return rolkiRWZOdpademDlaZlecenia.Sum(s => s.WagaOdpad_kg);
        }

        /// <summary>
        /// Pobiera wszystkie rolki, ktore zostaly rozchodowane a nie zaksiegowane dla danego bazowego zlecenia produkcyjnego niezaleznie od gniazda
        /// </summary>
        /// <param name="idZlecenie"></param>
        /// <returns></returns>
        private async Task<IEnumerable<tblProdukcjaRuchTowar>> PobierzListeRolekRozchodowanychINiezaksiegowanychDlaZlecenia(int idZlecenie)
        {
            return await unitOfWork.tblProdukcjaRuchTowar
                        .WhereAsync(r => r.IDZleceniePodstawowe == idZlecenie
                                        && r.WagaOdpad_kg > 0
                                        && r.IDProdukcjaRozliczenieStatus != (int)ProdukcjaRozliczenieStatusEnum.Rozliczono);
        }

        #endregion

        #region GenerowaniCenyMieszanki

        public async Task<decimal> GenerujCeneMieszanki(int idZlecenieProdukcyjne)
        {
            if (idZlecenieProdukcyjne == 0) throw new ArgumentException(nameof(idZlecenieProdukcyjne));

            var mieszanka = await unitOfWork.tblProdukcjaZlecenieProdukcyjne_Mieszanka.WhereAsync(m => m.IDProdukcjaZlecenieProdukcyjne == idZlecenieProdukcyjne);

            if (!mieszanka.Any())
                throw new InvalidOperationException($"Brak mieszanki dla zlecenia: {idZlecenieProdukcyjne}");

            if (mieszanka.First().CenaMieszanki_kg > 0)
                return mieszanka.First().CenaMieszanki_kg;

            return await ObliczCeneMieszankiZeSkladowych(mieszanka);
        }

        private async Task<decimal> ObliczCeneMieszankiZeSkladowych(IEnumerable<tblProdukcjaZlecenieProdukcyjne_Mieszanka> mieszanka)
        {
            decimal cena = 0;
            foreach (var surowiec in mieszanka)
            {
                decimal cenaJednostkowa = 0;
                if (surowiec.Cena_kg > 0)
                    cenaJednostkowa = surowiec.Cena_kg;
                else
                {
                    var towarRuch = await unitOfWork.vwMagazynRuchGTX.WhereAsync(t => t.IdTowar == surowiec.IDTowar
                                                                                   && t.Pozostalo > 0
                                                                                   && t.Cena > 0);
                    cenaJednostkowa = towarRuch.First().Cena;
                }

                cena += surowiec.ZawartoscProcentowa * cenaJednostkowa;
            }

            return cena;
        }

        #endregion

        public async Task<IEnumerable<tblProdukcjaRozliczenie_RW>> GenerujRozliczenieRWAsync(tblProdukcjaRozliczenie_PW wybranaPozycjaKonfekcjiDoRozliczenia)
        {
            if (wybranaPozycjaKonfekcjiDoRozliczenia is null)
                throw new ArgumentNullException($"Brak zlecenia jako argumentu funkcji {nameof(GenerujRozliczenieRWAsync)}.");

            // pobierz mieszanke dla zlecenia
            var mieszanka = await unitOfWork.tblProdukcjaZlecenieProdukcyjne_Mieszanka
                                    .WhereAsync(e => e.IDProdukcjaZlecenieProdukcyjne == wybranaPozycjaKonfekcjiDoRozliczenia.IDZlecenie);

            if (!mieszanka.Any())
                throw new ArgumentOutOfRangeException($"Brak mieszanki dla zlecenia: {wybranaPozycjaKonfekcjiDoRozliczenia.IDZlecenie}");

            //dla kazdej pozycji wygeneruj rozliczenie wg udziału, ceny i ilosci
            var listaRW = new List<tblProdukcjaRozliczenie_RW>();

            foreach (var surowiec in mieszanka)
            {
                vwMagazynRuchGTX surowiecDostepny = await PobierzSurowiecDostepny(surowiec);
                tblProdukcjaRozliczenie_RW surowiecRozliczenie = GenerujEncjeRozliczenia(surowiec, surowiecDostepny);

                listaRW.Add(surowiecRozliczenie);
            }
            return listaRW;
        }

        private tblProdukcjaRozliczenie_RW GenerujEncjeRozliczenia(tblProdukcjaZlecenieProdukcyjne_Mieszanka surowiec, vwMagazynRuchGTX surowiecDostepny)
        {
            return new tblProdukcjaRozliczenie_RW
            {

                IDNormaZuzyciaMsAccess = surowiec.IDMsAccess.GetValueOrDefault(),
                IDSurowiecSubiekt = surowiecDostepny.IdTowar,
                CenaJednostkowa = surowiecDostepny.Cena,
                NazwaTowaruSubiekt = surowiecDostepny.TowarNazwa,
                SymbolTowaruSubiekt = surowiecDostepny.TowarSymbol,
                IDZlecenie = surowiec.IDProdukcjaZlecenieProdukcyjne.GetValueOrDefault(),
                Udzial = surowiec.ZawartoscProcentowa,
                DataDodania = DateTime.Now,
                IDJm = (int)JmEnum.kg,
                Jm = "kg"
            };
        }
        private async Task<vwMagazynRuchGTX> PobierzSurowiecDostepny(tblProdukcjaZlecenieProdukcyjne_Mieszanka surowiec)
        {
            var towar = await unitOfWork.vwTowarGTX.SingleOrDefaultAsync(e => e.IdTowar == surowiec.IDTowar);

            if (surowiec.Cena_kg > 0)
            {
                var surowiecDostepnyNaMagazynie = await unitOfWork.vwMagazynRuchGTX.WhereAsync(e => e.IdTowar == surowiec.IDTowar
                                                                                                 && e.Cena == surowiec.Cena_kg);
                return surowiecDostepnyNaMagazynie.First();
            }
            else
            {
                var surowiecDostepnyNaMagazynie = await unitOfWork.vwMagazynRuchGTX.WhereAsync(e => e.IdTowar == surowiec.IDTowar
                                                                                                 && e.Pozostalo > 0);
                if (surowiecDostepnyNaMagazynie.Any())
                    return surowiecDostepnyNaMagazynie.FirstOrDefault();

                surowiecDostepnyNaMagazynie = await unitOfWork.vwMagazynRuchGTX.WhereAsync(e => e.IdTowar == surowiec.IDTowar);
                return surowiecDostepnyNaMagazynie.FirstOrDefault();
            }
        }


        public RwPodsumowanieModel PodsumujRW(IEnumerable<tblProdukcjaRozliczenie_RW> listaRW)
        {
            if (listaRW is null || !listaRW.Any())
                throw new ArgumentException("Brak listy Rw do podsumowania");

            return new RwPodsumowanieModel
            {
                IloscPozycji = listaRW.Count(),
                IloscKg = listaRW.Sum(s => s.Ilosc),
                Koszt = listaRW.Sum(s => s.Wartosc),
                UdzialSurowca = listaRW.Sum(s => s.Udzial),
            };
        }


        public async Task ZaksiegujOdpadJakoRozliczony()
        {
            if (rolkiRWZOdpademDlaZlecenia is null
                || !rolkiRWZOdpademDlaZlecenia.Any()) return;

            rolkiRWZOdpademDlaZlecenia.ToList().ForEach(r => r.IDProdukcjaRuchTowarStatus = (int)ProdukcjaRuchTowarStatusEnum.Rozchodowano);

            await unitOfWork.SaveAsync();
        }


        public async Task<tblProdukcjaRozliczenie_PW> GenerujOdpadDlaPW(int idZlecenie)
        {

            if (idZlecenie == 0) throw new ArgumentException("Brak id zlecenia do wyliczenia odpadu");

            var rolkiRWRozchodowane = await PobierzListeRolekRozchodowanychINiezaksiegowanychDlaZlecenia(idZlecenie);

            if (!rolkiRWRozchodowane.Any()) return null;

            //var pozycja = listaPW.First();
            var odpad = new tblProdukcjaRozliczenie_PW
            {
                SymbolTowaruSubiekt = PobierzSymbolINazweZSubiektDlaOdpadu(rolkiRWRozchodowane.First().TowarNazwaSubiekt).symbol,
                NazwaTowaruSubiekt = PobierzSymbolINazweZSubiektDlaOdpadu(rolkiRWRozchodowane.First().TowarNazwaSubiekt).nazwa,
                IDZlecenie = rolkiRWRozchodowane.First().tblProdukcjaZlcecenieProdukcyjne.IDProdukcjaZlecenie,
                NrZlecenia = rolkiRWRozchodowane.First().tblProdukcjaZlcecenieProdukcyjne.NrZlecenia.ToString(),
                NrWz = rolkiRWRozchodowane.First().NrPalety.ToString(),
                Ilosc_kg = rolkiRWRozchodowane.Sum(s => s.WagaOdpad_kg),
                CenaProduktuBezNarzutow_kg = rolkiRWRozchodowane.First().Cena_kg,
                Wartosc = rolkiRWRozchodowane.First().Wartosc,
                Ilosc = rolkiRWRozchodowane.Sum(s => s.WagaOdpad_kg),
                IDJm = (int)JmEnum.kg,
                Jm = "kg",
                CenaJednostkowa = rolkiRWRozchodowane.First().Cena_kg,
            };

            return odpad;
        }

        /// <summary>
        /// Zwraca Liste rozliczen PW
        /// </summary>
        /// <param name="listaPozycjiKonfekcji">pozycje konfekcji z MsAccess</param>
        /// <param name="cenaKgMieszanki">cena mnieszanki za kg</param>
        /// <returns></returns>
        private (string symbol, string nazwa) PobierzSymbolINazweZSubiektDlaOdpadu(string nazwaTowaru)
        {
            var surowiec = nazwaTowaruSubiekt.PobierzDaneZNazwyTowaru(nazwaTowaru).surowiec;

            if (surowiec.Contains("PP"))
            {
                return ("TASMY_PP", "Surowiec PP taśmy");
            }
            else
            {
                return ("TASMY_PES", "Surowiec PES taśmy");
            }
        }

        public decimal GenerujCeneMieszanki(IEnumerable<tblProdukcjaRozliczenie_RW> listaRW)
        {
            decimal cenaMieszanki = 0;

            foreach (var surowiec in listaRW)
            {
                cenaMieszanki += surowiec.Udzial * surowiec.CenaJednostkowa;
            }

            return cenaMieszanki;
        }
    }
}
