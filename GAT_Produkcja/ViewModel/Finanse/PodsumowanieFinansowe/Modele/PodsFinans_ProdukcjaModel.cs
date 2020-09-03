using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Modele
{
    public class PodsFinans_ProdukcjaModel
    {
        public string RodzajProdukcji { get; set; }
        public decimal Ilosc_m2 { get; set; }
        public decimal Ilosc_kg { get; set; }
        public decimal Wartosc { get; set; }
    }
}
