using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Modele
{
    public class RozliczenieWyrobuGotowegoModel
    {
        public int IdZlecenia { get; set; }
        public string NrZlecenia { get; set; }
        public string NrWz { get; set; }
        public string NazwaTowaru { get; set; }
        public decimal Waga_kg { get; set; }
        public decimal WagaOdpadu_kg { get; set; }
        public decimal Ilosc_m2 { get; set; }
        public decimal CenaProduktuBezNarzutow_kg { get; set; }
        public decimal CenaProduktuBezNarzutow_m2 { get; set; }
        public decimal CenaSprzedazyGtex_m2 { get; set; }
        public decimal Wartosc { get; set; }
    }
}
