using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Modele
{
    public class PodsFinans_NaleznosciIZobowiazaniaModel
    {
        public string Firma { get; set; }
        public decimal NaleznosciDoDaty { get; set; }
        public decimal ZobowiazaniaDoDaty { get; set; }
        public decimal NaleznosciAll { get; set; }
        public decimal ZobowiazaniaAll { get; set; }
    }
}
