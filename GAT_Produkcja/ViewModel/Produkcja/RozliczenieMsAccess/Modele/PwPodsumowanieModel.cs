using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Modele
{
    public class PwPodsumowanieModel
    {
        public int ZlecenieId { get; set; }
        public string NrWz { get; set; }
        public string NrZlecen { get; set; }
        public int IloscPozycji { get; set; }
        public decimal WagaKg { get; set; }
        public decimal OdpadKg { get; set; }
        public decimal IloscM2{ get; set; }
        public decimal Wartosc { get; set; }
    }
}
