using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Modele
{
    public class RMA_DaneWejscioweModel
    {
        public string TowarNazwa { get; set; }
        public DateTime DataDoRozliczenia { get; set; }

        public decimal Szerokosc { get; set; }
        public decimal Dlugosc { get; set; }
        public decimal Ilosc_m2 { get; set; }
        public int IloscRolek { get; set; }

    }
}
