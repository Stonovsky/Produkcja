using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Modele
{
    public class PodsFinans_MagazynyModel
    {
        public string Lokalizacja { get; set; }
        public string NazwaMagazynu { get; set; }
        public decimal Ilosc { get; set; }
        public string Jm { get; set; }
        public decimal Wartosc { get; set; }

        public override string ToString()
        {
            return $"{NazwaMagazynu}, {Lokalizacja}, {Ilosc}, {Wartosc}";
        }
    }
}
