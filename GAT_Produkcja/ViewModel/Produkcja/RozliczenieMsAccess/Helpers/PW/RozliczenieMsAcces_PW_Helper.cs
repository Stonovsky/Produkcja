using GAT_Produkcja.db;
using GAT_Produkcja.db.EntitesInterfaces;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.dbMsAccess.EntitesInterfaces;
using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.dbMsAccess.Models.Adapters;
using GAT_Produkcja.dbMsAccess.UnitOfWork;
using GAT_Produkcja.GUSServiceReference;
using GAT_Produkcja.ViewModel.Produkcja.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.PW.Adapters;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.Strategy;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.Strategy.Factory;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Modele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.PW
{
    public class RozliczenieMsAcces_PW_Helper : IRozliczenieMsAcces_PW_Helper
    {
        private NazwaTowaruSubiektHelper nazwaTowaruSubiekt;
        private IEnumerable<tblProdukcjaRozliczenie_CenyTransferowe> listaCenTransferowychGTEX;
        private readonly IUnitOfWork unitOfWork;
        private readonly IUnitOfWorkMsAccess unitOfWorkMsAccess;

        public RozliczenieMsAcces_PW_Helper(IUnitOfWork unitOfWork,
                                            IUnitOfWorkMsAccess unitOfWorkMsAccess)
        {
            this.unitOfWork = unitOfWork;
            this.unitOfWorkMsAccess = unitOfWorkMsAccess;

            nazwaTowaruSubiekt = new NazwaTowaruSubiektHelper();
        }

        public async Task LoadAsync()
        {
            listaCenTransferowychGTEX = await unitOfWork.tblProdukcjaRozliczenie_CenyTransferowe.WhereAsync(c => c.CzyAktualna == true);
        }

        #region RozliczeniePW

        public IEnumerable<tblProdukcjaRozliczenie_PW> GenerujRozliczeniePW(IEnumerable<IGniazdoProdukcyjne> listaPozycjiKonfekcji, decimal cenaMieszanki)
        {
            if (listaPozycjiKonfekcji is null ||
                !listaPozycjiKonfekcji.Any())
                throw new ArgumentNullException($"Brak argumentów dla metody {nameof(GenerujRozliczeniePW)}");


            var listaPW = new List<tblProdukcjaRozliczenie_PW>();

            foreach (var pozycja in listaPozycjiKonfekcji)
            {
                if (pozycja.Szerokosc == 0) continue;
                if (pozycja.Dlugosc == 0) continue;

                var nazwaTowaru = nazwaTowaruSubiekt.GenerujNazweTowaru(pozycja);
                var pozycjaPW = new tblProdukcjaRozliczenie_PW
                {
                    NrZlecenia = pozycja.Zlecenie,
                    IDZlecenie = pozycja.ZlecenieID,
                    NazwaTowaruSubiekt = nazwaTowaru,
                    SymbolTowaruSubiekt = nazwaTowaruSubiekt.GenerujSymbolTowaru(pozycja),
                    Ilosc = pozycja.IloscM2,
                    Ilosc_kg = pozycja.Waga,
                    Odpad_kg = pozycja.WagaOdpadu,
                    IDJm = (int)JmEnum.m2,
                    Jm = "m2",
                    CenaProduktuBezNarzutow_kg = cenaMieszanki,
                    CenaJednostkowa = cenaMieszanki,
                    Wartosc = (pozycja.Waga + pozycja.WagaOdpadu) * cenaMieszanki,
                    Szerokosc_m = pozycja.Szerokosc,
                    Dlugosc_m = pozycja.Dlugosc,
                    NazwaTowaru = pozycja.Artykul,

                };

                listaPW.Add(pozycjaPW);
            }
            return listaPW;
        }

        public IEnumerable<tblProdukcjaRozliczenie_PW> GenerujRozliczeniePW(IEnumerable<IProdukcjaRuchTowar> listaPozycjiKonfekcji, decimal cenaMieszanki)
        {
            if (listaPozycjiKonfekcji is null ||
                !listaPozycjiKonfekcji.Any())
                throw new ArgumentNullException($"Brak argumentów dla metody {nameof(GenerujRozliczeniePW)}");

            var listaPW = new List<tblProdukcjaRozliczenie_PW>();


            foreach (var pozycja in listaPozycjiKonfekcji)
            {
                if (pozycja.Szerokosc_m == 0) continue;
                if (pozycja.Dlugosc_m == 0) continue;

                tblProdukcjaRozliczenie_PW pozycjaPW = GenerujEncjeRozliczeniaPW(pozycja, cenaMieszanki);

                listaPW.Add(pozycjaPW);
            }

            DodajNazwyRolekBazowychDoListy(listaPW);
            return listaPW;
        }

        private tblProdukcjaRozliczenie_PW GenerujEncjeRozliczeniaPW(IProdukcjaRuchTowar pozycja, decimal cenaMieszanki)
        {
            return new tblProdukcjaRozliczenie_PW
            {
                NrZlecenia = pozycja.NrZleceniaPodstawowego.ToString(),
                IDZlecenie = pozycja.IDZleceniePodstawowe.GetValueOrDefault(),
                NazwaTowaruSubiekt = pozycja.TowarNazwaSubiekt,
                SymbolTowaruSubiekt = pozycja.TowarSymbolSubiekt,
                Ilosc = pozycja.Ilosc_m2,
                Ilosc_kg = pozycja.Waga_kg,
                Odpad_kg = pozycja.WagaOdpad_kg,
                IDJm = (int)JmEnum.m2,
                Jm = "m2",
                CenaProduktuBezNarzutow_kg = cenaMieszanki,
                CenaProduktuBezNarzutow_m2 = GenerujCeneM2(pozycja, cenaMieszanki),
                CenaSprzedazyGtex_m2 = PobierzCeneTransferowa(pozycja.TowarNazwaSubiekt),
                CenaHurtowaAGG_m2 = PobierzCeneHurtowa(pozycja.TowarNazwaSubiekt),
                CenaJednostkowa = cenaMieszanki,
                Wartosc = (pozycja.Waga_kg + pozycja.WagaOdpad_kg) * cenaMieszanki,
                Szerokosc_m = pozycja.Szerokosc_m,
                Dlugosc_m = pozycja.Dlugosc_m,
                Przychod = pozycja.KierunekPrzychodu,
                NrRolkiBazowej = pozycja.NrRolkiBazowej,
                NrRolki = pozycja.NrRolkiPelny,
                IDMsAccess = pozycja.IDMsAccess,
                NrWz = pozycja.NrDokumentu,
            };
        }

        private decimal GenerujCeneM2(IProdukcjaRuchTowar pozycja, decimal cenaMieszanki)
        {
            if (pozycja.Ilosc_m2 == 0) return 0;

            return (pozycja.Waga_kg / pozycja.Ilosc_m2) * cenaMieszanki;
        }

        public void DodajNazwyRolekBazowychDoListy(List<tblProdukcjaRozliczenie_PW> listaPW)
        {
            if (listaPW is null || !listaPW.Any())
                throw new ArgumentException("Brak listy dla ktorej nalezy pobrac symbol rolki bazowej");

            var listaNrSztuk = PobierzListeNrSztuk(listaPW);
            if (listaNrSztuk is null) return;


            var listaPozycjiKonfekcji = unitOfWorkMsAccess.Konfekcja.GetByNrSztuki(listaNrSztuk);
            foreach (var pozycja in listaPW)
            {
                Konfekcja rolka = listaPozycjiKonfekcji.Where(e => e.NrSztuki == pozycja.NrRolkiBazowej
                                                                && e.Przychody.ToLower().Contains("linia"))
                                                       .FirstOrDefault();
                if (rolka is null) continue;
                var rolkaBazowa = new KonfekcjaAdapter(rolka);
                pozycja.NazwaRolkiBazowej = NazwaTowaru.GenerujNazweTowaru(rolkaBazowa);
                pozycja.SymbolRolkiBazowej = NazwaTowaru.GenerujSymbolTowaru(rolkaBazowa);
            }
        }

        /// <summary>
        /// Zwarca liste numerow <see cref="string"/> sztuk usuwajac litere "m" z nr sztuki
        /// </summary>
        /// <param name="listaPW"></param>
        /// <returns></returns>
        private IEnumerable<string> PobierzListeNrSztuk(List<tblProdukcjaRozliczenie_PW> listaPW)
        {
            IEnumerable<string> listaNrSztuk = listaPW.Select(s => s.NrRolkiBazowej).Distinct().ToList();
            listaNrSztuk = listaNrSztuk.Where(s => s != null).ToList();

            if (listaNrSztuk is null) return null;

            listaNrSztuk.Select(nr => nr.Replace("m", "")).ToList();

            return listaNrSztuk;
        }

        public decimal PobierzCeneTransferowa(string nazwaTowaruSubiekt)
        {
            if (nazwaTowaruSubiekt is null ||
                nazwaTowaruSubiekt == string.Empty)
                throw new ArgumentException("Brak nazwy towaru dla pobrania ceny z bazy Subiekt GTEX");

            foreach (var pozycja in listaCenTransferowychGTEX)
            {
                if (nazwaTowaruSubiekt.Contains(pozycja.TowarNazwa))
                    return pozycja.CenaTransferowa;
            }
            return 0;
        }
        public decimal PobierzCeneHurtowa(string nazwaTowaruSubiekt)
        {
            if (nazwaTowaruSubiekt is null ||
                nazwaTowaruSubiekt == string.Empty)
                throw new ArgumentException("Brak nazwy towaru dla pobrania ceny z bazy Subiekt GTEX");

            foreach (var pozycja in listaCenTransferowychGTEX)
            {
                if (nazwaTowaruSubiekt.Contains(pozycja.TowarNazwa))
                    return pozycja.CenaHurtowa;
            }
            return 0;
        }

        #endregion        

        #region Odpad
        public tblProdukcjaRozliczenie_PW GenerujOdpadDlaPW(IEnumerable<tblProdukcjaRozliczenie_PW> listaPW)
        {

            if (listaPW is null || !listaPW.Any())
            {
                throw new ArgumentException("Brak listy do wyliczenia odpadu");
            }

            var pozycja = listaPW.First();
            var odpad = new tblProdukcjaRozliczenie_PW
            {
                SymbolTowaruSubiekt = PobierzSymbolINazweZSubiektDlaOdpadu(listaPW.First().NazwaTowaruSubiekt).symbol,
                NazwaTowaruSubiekt = PobierzSymbolINazweZSubiektDlaOdpadu(listaPW.First().NazwaTowaruSubiekt).nazwa,
                IDZlecenie = pozycja.IDZlecenie,
                NrZlecenia = pozycja.NrZlecenia,
                NrWz = pozycja.NrWz,
                Ilosc_kg = listaPW.Sum(s => s.Odpad_kg),
                CenaProduktuBezNarzutow_kg = pozycja.CenaProduktuBezNarzutow_kg,
                Wartosc = listaPW.Sum(s => s.Odpad_kg) * pozycja.CenaProduktuBezNarzutow_kg,
                Ilosc = listaPW.Sum(s => s.Odpad_kg),
                IDJm = (int)JmEnum.kg,
                Jm = "kg",
                CenaJednostkowa = pozycja.CenaProduktuBezNarzutow_kg,
            };

            return odpad;
        }

        public async Task<tblProdukcjaRozliczenie_PW> GenerujOdpadDlaPW(int idZlecenie)
        {

            if (idZlecenie == 0) throw new ArgumentException("Brak id zlecenia do wyliczenia odpadu");

            var rolkiRWRozchodowane = await unitOfWork.tblProdukcjaRuchTowar
                                                       .WhereAsync(r => r.IDProdukcjaRuchTowarStatus != (int)ProdukcjaRuchTowarStatusEnum.Rozchodowano
                                                                     && r.tblProdukcjaZlcecenieProdukcyjne.IDProdukcjaZlecenie == idZlecenie);

            if (!rolkiRWRozchodowane.Any()) throw new ArgumentException("Brak rolek rozchodowanych dla danego zlecenia"); ;

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
                Ilosc = rolkiRWRozchodowane.Sum(s=>s.WagaOdpad_kg),
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

        #endregion

        #region Podsumowanie
        public PwPodsumowanieModel PodsumujPW(IEnumerable<tblProdukcjaRozliczenie_PW> listaPW)
        {
            if (listaPW is null || !listaPW.Any())
                throw new ArgumentException("Brak listy Rw do podsumowania");

            return new PwPodsumowanieModel
            {
                IloscPozycji = listaPW.Count(),
                WagaKg = listaPW.Sum(s => s.Ilosc_kg),
                OdpadKg = listaPW.Sum(s => s.Odpad_kg),
                IloscM2 = listaPW.Sum(s => s.Ilosc),
                Wartosc = listaPW.Sum(s => s.Wartosc)
            };
        }


        public IEnumerable<tblProdukcjaRozliczenie_PW> PodsumujPWPodzialTowar(IEnumerable<tblProdukcjaRozliczenie_PW> listaPW)
        {
            if (listaPW is null || !listaPW.Any())
                throw new ArgumentException("Brak listy Rw do podsumowania");

            IEnumerable<tblProdukcjaRozliczenie_PW> zestawienie = new PodsumujPWPodzialTowarAdapter().Grupuj(listaPW);

            return zestawienie;
        }

        public PwPodsumowanieModel PodsumujListe(IEnumerable<Konfekcja> lista)
        {
            if (lista is null || !lista.Any())
                throw new ArgumentException("Brak listy Rw do podsumowania");

            return new PwPodsumowanieModel
            {
                NrZlecen = PobierzZlecenia(lista),
                IloscPozycji = lista.Count(),
                WagaKg = lista.Sum(s => s.Waga),
                OdpadKg = lista.Sum(s => s.WagaOdpadu),
                IloscM2 = lista.Sum(s => s.IloscM2),
            };

        }
        public PwPodsumowanieModel PodsumujListe(IEnumerable<tblProdukcjaRozliczenie_PW> lista)
        {
            if (lista is null || !lista.Any())
                throw new ArgumentException("Brak listy Rw do podsumowania");

            return new PwPodsumowanieModel
            {
                IloscPozycji = lista.Count(),
                WagaKg = lista.Sum(s => s.Ilosc_kg),
                OdpadKg = lista.Sum(s => s.Odpad_kg),
                IloscM2 = lista.Sum(s => s.Ilosc),
            };
        }

        private string PobierzZlecenia(IEnumerable<Konfekcja> lista)
        {
            lista = lista.OrderBy(s => s.ZlecenieID).ToList();
            var zlecenia = lista.Select(s => s.Zlecenie).Distinct();

            return string.Join(",", zlecenia);
        }

        public string GenerujRozliczoneTowary(IEnumerable<tblProdukcjaRozliczenie_PW> listaPodsumowanPWTowar)
        {
            StringBuilder towar = new StringBuilder();
            foreach (var pozycja in listaPodsumowanPWTowar)
            {
                towar.Append($"{pozycja.NazwaTowaruSubiekt} = {Math.Round(pozycja.Ilosc, 2)}m2; ");
            }
            return towar.ToString();
        }

        #endregion

    }
}
