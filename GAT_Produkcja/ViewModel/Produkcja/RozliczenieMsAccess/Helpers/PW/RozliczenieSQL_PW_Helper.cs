using DocumentFormat.OpenXml.Drawing.Charts;
using GAT_Produkcja.db;
using GAT_Produkcja.db.EntitesInterfaces;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.dbMsAccess.EntitesInterfaces;
using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.PW.Adapters;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Modele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.PW
{
    public class RozliczenieSQL_PW_Helper : IRozliczenieSQL_PW_Helper
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICenyTransferoweHelper cenyTransferoweHelper;

        #region CTOR
        public RozliczenieSQL_PW_Helper(IUnitOfWork unitOfWork, ICenyTransferoweHelper cenyTransferoweHelper)
        {
            this.unitOfWork = unitOfWork;
            this.cenyTransferoweHelper = cenyTransferoweHelper;
        }
        #endregion
        public tblProdukcjaRozliczenie_PW GenerujOdpadDlaPW(IEnumerable<tblProdukcjaRozliczenie_PW> listaPW)
        {
            throw new NotImplementedException();
        }

        public Task<tblProdukcjaRozliczenie_PW> GenerujOdpadDlaPW(int idZlecenie)
        {
            throw new NotImplementedException();
        }


        #region RozliczeniePW

        public IEnumerable<tblProdukcjaRozliczenie_PW> GenerujRozliczeniePW(IEnumerable<IProdukcjaRuchTowar> listaPozycjiKonfekcji, decimal cenaMieszanki)
        {
            if (listaPozycjiKonfekcji is null || !listaPozycjiKonfekcji.Any())
                throw new ArgumentNullException($"Brak argumentów dla metody {nameof(GenerujRozliczeniePW)}");

            var listaPW = new List<tblProdukcjaRozliczenie_PW>();


            foreach (var pozycja in listaPozycjiKonfekcji)
            {
                if (pozycja.Szerokosc_m == 0) continue;
                if (pozycja.Dlugosc_m == 0) continue;
                if (pozycja.Waga_kg == 0) continue;

                tblProdukcjaRozliczenie_PW pozycjaPW = GenerujEncjeRozliczeniaPW(pozycja, cenaMieszanki);

                listaPW.Add(pozycjaPW);
            }

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
                CenaSprzedazyGtex_m2 = cenyTransferoweHelper.PobierzCeneTransferowa(pozycja.TowarNazwaSubiekt),
                CenaHurtowaAGG_m2 = cenyTransferoweHelper.PobierzCeneHurtowa(pozycja.TowarNazwaSubiekt),
                CenaJednostkowa = cenaMieszanki,
                Wartosc = (pozycja.Waga_kg + pozycja.WagaOdpad_kg) * cenaMieszanki,
                Szerokosc_m = pozycja.Szerokosc_m,
                Dlugosc_m = pozycja.Dlugosc_m,
                Przychod = pozycja.KierunekPrzychodu,
                NrRolkiBazowej = pozycja.NrRolkiBazowej,
                NrRolki = pozycja.NrRolkiPelny,
                IDMsAccess = pozycja.IDMsAccess,
                NrWz = pozycja.NrDokumentu,
                NazwaRolkiBazowej = pozycja.NazwaRolkiBazowej,
                SymbolRolkiBazowej = pozycja.SymbolRolkiBazowej,
            };
        }

        private decimal GenerujCeneM2(IProdukcjaRuchTowar pozycja, decimal cenaMieszanki)
        {
            if (pozycja.Ilosc_m2 == 0) return 0;

            return (pozycja.Waga_kg / pozycja.Ilosc_m2) * cenaMieszanki;

        }

        #endregion

        public string GenerujRozliczoneTowary(IEnumerable<tblProdukcjaRozliczenie_PW> listaPodsumowanPWTowar)
        {
            StringBuilder towar = new StringBuilder();
            foreach (var pozycja in listaPodsumowanPWTowar)
            {
                towar.Append($"{pozycja.NazwaTowaruSubiekt} = {Math.Round(pozycja.Ilosc, 2)}m2; ");
            }
            return towar.ToString();
        }

        public async Task LoadAsync()
        {
            await cenyTransferoweHelper.LoadAsync();
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
    }
}
