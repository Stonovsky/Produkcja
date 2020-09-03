using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.dbMsAccess.UnitOfWork;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.Helpers.WeryfikacjaTolerancji;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.Adapters;
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
    public class RozliczenieMsAcces_RW_Helper : IRozliczenieMsAcces_RW_Helper
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ISurowiecSubiektStrategyFactory surowiecSubiektStrategyFactory;
        private readonly IUnitOfWorkMsAccess unitOfWorkMsAccess;
        private readonly ISurowiecSubiektStrategy surowiecStrategy;
        private IEnumerable<Surowiec> listaSurowcow;
        private IEnumerable<NormyZuzycia> listNormZuzycia;

        public RozliczenieMsAcces_RW_Helper(IUnitOfWork unitOfWork,
                                            IUnitOfWorkMsAccess unitOfWorkMsAccess,
                                            ISurowiecSubiektStrategyFactory surowiecSubiektStrategyFactory

            )
        {
            this.unitOfWork = unitOfWork;
            this.surowiecSubiektStrategyFactory = surowiecSubiektStrategyFactory;
            this.unitOfWorkMsAccess = unitOfWorkMsAccess;


            surowiecStrategy = this.surowiecSubiektStrategyFactory.PobierzStrategie(SurowiecSubiektFactoryEnum.ZNazwy);
        }

        public async Task LoadAsync()
        {
            listNormZuzycia = await unitOfWorkMsAccess.NormyZuzycia.GetAllAsync();
            listaSurowcow = await unitOfWorkMsAccess.Surowiec.GetAllAsync();
        }
        #region RW

        /// <summary>
        /// Tworzy rozliczenie RW dla pozycji mieszanki wraz z idTowaru z Subiekt oraz udzialami
        /// </summary>
        /// <param name="mieszanka">encje NormZuzycia dla mieszanki</param>
        /// <returns></returns>
        public async Task<IEnumerable<tblProdukcjaRozliczenie_RW>> GenerujRozliczenieRWAsync(tblProdukcjaRozliczenie_PW wybranaPozycjaKonfekcjiDoRozliczenia)
        {
            if (wybranaPozycjaKonfekcjiDoRozliczenia is null)
                throw new ArgumentNullException($"Brak zlecenia jako argumentu funkcji {nameof(GenerujRozliczenieRWAsync)}.");

            var mieszanka = await PobierzMieszankeDlaZleceniaAsync(wybranaPozycjaKonfekcjiDoRozliczenia.IDZlecenie);

            var rozliczenieLista = new List<tblProdukcjaRozliczenie_RW>();

            foreach (var pozycja in mieszanka)
            {
                var surowiecZSubiekt = await surowiecStrategy.PobierzSurowiecZSubiektDla(pozycja.Surowiec);

                if (surowiecZSubiekt is null)
                    throw new ArgumentNullException("Nie znaleziono surowca do rozliczenia w bazie danych.");

                rozliczenieLista.Add(new ProdukcjaRozliczenieRWAdapter(
                                            pozycja,
                                            surowiecZSubiekt,
                                            PobierzIdSurowcaZNazwyMsAccess(pozycja.Surowiec))
                                        .Konwertuj()
                                        );
            }

            return rozliczenieLista;
        }


        public int PobierzIdSurowcaZNazwyMsAccess(string nazwaSurowca)
        {
            var surowiec = listaSurowcow.SingleOrDefault(s => s.NazwaSurowca == nazwaSurowca);
            if (surowiec != null)
                return surowiec.Id;

            return 0;
        }

        /// <summary>
        /// Dodaje do listy RW ilosci zgodnie z udzialem. 
        /// Sumaryczna ilosc kg bierze z PW jako sume z wagi i sume z odpadu dla wszystkich elementow
        /// </summary>
        /// <param name="listaRWSurowca"></param>
        /// <param name="listaPwWyrobuGotowego"></param>
        public async Task DodajIlosciKgIWartoscDoRW(ObservableCollection<tblProdukcjaRozliczenie_RW> listaRWSurowca,
                                              ObservableCollection<tblProdukcjaRozliczenie_PW> listaPwWyrobuGotowego)
        {
            if (listaRWSurowca is null ||
                !listaRWSurowca.Any())
                throw new ArgumentException("Brak listy surowca do analizy");

            if (listaPwWyrobuGotowego is null ||
                !listaPwWyrobuGotowego.Any())
                throw new ArgumentException("Brak listy surowca do analizy");

            var sumaIlosciKgDlaPW = listaPwWyrobuGotowego.Sum(s => s.Ilosc_kg) + listaPwWyrobuGotowego.Sum(s => s.Odpad_kg);

            foreach (var poz in listaRWSurowca)
            {
                poz.Ilosc = sumaIlosciKgDlaPW * poz.Udzial;
                poz.Wartosc = poz.Ilosc * poz.CenaJednostkowa;
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

        #endregion

        #region Cena mieszanki
        
        /// <summary>
        /// Pobiera mieszanke dla danego zlecenia
        /// </summary>
        /// <param name="idZlecenia"></param>
        /// <returns></returns>
        private async Task<IEnumerable<NormyZuzycia>> PobierzMieszankeDlaZleceniaAsync(int idZlecenia)
        {
            var mieszanka = await unitOfWorkMsAccess.NormyZuzycia.GetAllAsync();
            mieszanka = mieszanka.Where(m => m.ZlecenieID == idZlecenia).ToList();

            if (mieszanka is null
                || !mieszanka.Any())
                throw new ArgumentException("Brak mieszanki dla danego zlecenia!");

            return mieszanka;
        }


        public async Task<decimal> GenerujCeneMieszanki(IEnumerable<NormyZuzycia> mieszanka)
        {
            decimal cena = 0;
            foreach (var pozycja in mieszanka)
            {
                var surowiecSubiekt = await surowiecStrategy.PobierzSurowiecZSubiektDla(pozycja.Surowiec);

                cena += pozycja.Ilosc * surowiecSubiekt.Cena;
            }

            return cena;
        }

        public async Task<decimal> GenerujCeneMieszanki(int idZlecenia)
        {
            var mieszanka = listNormZuzycia.Where(z => z.ZlecenieID == idZlecenia);
            decimal cena = 0;
            foreach (var pozycja in mieszanka)
            {
                var surowiecSubiekt = await surowiecStrategy.PobierzSurowiecZSubiektDla(pozycja.Surowiec);

                cena += pozycja.Ilosc * surowiecSubiekt.Cena;
            }

            return cena;
        }


        #endregion

        #region Odpad

        #endregion
    }
}
