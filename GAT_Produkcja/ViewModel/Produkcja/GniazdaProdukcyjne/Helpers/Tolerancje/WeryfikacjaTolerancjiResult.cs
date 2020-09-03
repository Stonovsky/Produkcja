using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.Helpers.WeryfikacjaTolerancji
{
    public class WeryfikacjaTolerancjiResult
    {
        public bool CzyParametrZgodnyZTolerancja { get; set; }
        public GeowlokninaParametryEnum RodzajSprawdzanegoParametru { get; set; }
        public decimal ParametrRzeczywisty { get; set; }
        public decimal ParametrWymagany { get; set; }
        public string Uwagi { get; set; }
        public bool CzyMoznaPrzekwalifikowac { get; set; }
        public decimal MoznaPrzekwalifikowacNa { get; set; }

    }
}
