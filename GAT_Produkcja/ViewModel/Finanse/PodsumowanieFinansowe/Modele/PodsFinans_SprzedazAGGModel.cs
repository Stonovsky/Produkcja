using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Modele
{
    public class PodsFinans_SprzedazAGGModel
    {
        public string Nazwa { get; set; }
        public decimal Ilosc_m2 { get; set; }
        public decimal Netto { get; set; }
        public decimal Zysk { get; set; }
    }
}
