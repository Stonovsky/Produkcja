using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Modele
{
    public class RwPodsumowanieModel
    {
        public string NrZlecen{ get; set; }
        public int IloscPozycji { get; set; }
        public decimal IloscKg { get; set; }
        public decimal Koszt { get; set; }
        public decimal UdzialSurowca { get; set; }
    }
}
