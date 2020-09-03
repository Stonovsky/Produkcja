using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Modele
{
    public class PodsFinans_ZamowienieOdKlientowModel
    {
        public bool CzyZrealizowano { get; set; }
        public decimal IloscTK { get; set; }
        public decimal WartoscTK { get; set; }
        public decimal IloscCalkowita { get; set; }
        public decimal WartoscCalkowita { get; set; }
    }
}
