using GAT_Produkcja.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.PW.Adapters
{
    public class PodsumujPWPodzialTowarAdapter
    {
        #region CTOR
        public PodsumujPWPodzialTowarAdapter()
        {

        }

        public IEnumerable<tblProdukcjaRozliczenie_PW> Grupuj(IEnumerable<tblProdukcjaRozliczenie_PW> listaPW)
        {
            return listaPW
                    .GroupBy(g => new { g.IDZlecenie, g.Przychod, g.SymbolTowaruSubiekt })
                    .Select(se => new tblProdukcjaRozliczenie_PW
                    {
                        SymbolTowaruSubiekt = se.First().SymbolTowaruSubiekt,
                        NazwaTowaruSubiekt = se.First().NazwaTowaruSubiekt,
                        Ilosc = se.Sum(s => s.Ilosc),
                        Ilosc_kg = se.Sum(s => s.Ilosc_kg),
                        Odpad_kg = se.Sum(s => s.Odpad_kg),
                        CenaSprzedazyGtex_m2 = se.First().CenaSprzedazyGtex_m2,
                        IDZlecenie = se.First().IDZlecenie,
                        NrZlecenia = se.First().NrZlecenia,
                        CenaProduktuBezNarzutow_kg = se.First().CenaProduktuBezNarzutow_kg,
                        CenaProduktuBezNarzutow_m2 = se.First().CenaProduktuBezNarzutow_m2,
                        CenaHurtowaAGG_m2 = se.First().CenaHurtowaAGG_m2,
                        Wartosc = se.Sum(s => s.Ilosc_kg) * se.First().CenaProduktuBezNarzutow_kg,
                        WartoscOdpad = se.Sum(s => s.Odpad_kg) * se.First().CenaProduktuBezNarzutow_kg,
                        IDJm = se.First().IDJm,
                        Jm = se.First().Jm,
                        CenaJednostkowa = se.First().CenaProduktuBezNarzutow_m2 == 0 ? se.First().CenaProduktuBezNarzutow_kg : se.First().CenaProduktuBezNarzutow_m2,
                        Ilosc_szt = se.Count(),
                        Szerokosc_m = se.First().Szerokosc_m,
                        Dlugosc_m = se.First().Dlugosc_m,
                        Przychod = se.First().Przychod,
                        SymbolRolkiBazowej = se.First().SymbolRolkiBazowej,
                        NazwaRolkiBazowej = se.First().NazwaRolkiBazowej,
                    }).ToList();
        }
        #endregion

    }
}
