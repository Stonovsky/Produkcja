using GAT_Produkcja.db;
using GAT_Produkcja.db.EntitesInterfaces;
using GAT_Produkcja.db.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers
{
    class RozliczenieMsAccessAdapterHelper
    {
        public IEnumerable<tblProdukcjaRozliczenie_PW> GenerujRozliczeniePW(IEnumerable<IProdukcjaRuchTowar> listaPozycjiKonfekcji)
        {
            if (listaPozycjiKonfekcji is null ||
                !listaPozycjiKonfekcji.Any())
                throw new ArgumentNullException($"Brak argumentów dla metody {nameof(GenerujRozliczeniePW)}");


            var listaPW = new List<tblProdukcjaRozliczenie_PW>();


            foreach (var pozycja in listaPozycjiKonfekcji)
            {
                if (pozycja.Szerokosc_m == 0) continue;
                if (pozycja.Dlugosc_m == 0) continue;

                //var cenaMieszanki = GenerujCeneMieszanki(pozycja.NrZlecenia);

                var pozycjaPW = new tblProdukcjaRozliczenie_PW
                {
                    NrZlecenia = pozycja.ZlecenieNazwa,
                    IDZlecenie = pozycja.IDProdukcjaZlecenieProdukcyjne.GetValueOrDefault(),
                    NazwaTowaruSubiekt = pozycja.TowarNazwaSubiekt,
                    SymbolTowaruSubiekt = pozycja.TowarSymbolSubiekt,
                    Ilosc = pozycja.Ilosc_m2,
                    Ilosc_kg = pozycja.Waga_kg,
                    Odpad_kg = pozycja.WagaOdpad_kg,
                    IDJm = (int)JmEnum.m2,
                    Jm = "m2",
                    //TODO
                    //CenaProduktuBezNarzutow_kg = cenaMieszanki,
                    //CenaJednostkowa = cenaMieszanki,
                    //Wartosc = (pozycja.Waga_kg + pozycja.WagaOdpad_kg) * cenaMieszanki,
                    Szerokosc_m = pozycja.Szerokosc_m,
                    Dlugosc_m = pozycja.Dlugosc_m,
                };

                listaPW.Add(pozycjaPW);
            }
            return listaPW;
        }
    }
}
