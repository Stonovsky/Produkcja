using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Modele
{
    public class PodsFinans_StanyKontModel
    {
        public string Firma { get; set; }
        public string BankNazwa { get; set; }
        public string NrKonta { get; set; }
        public decimal StanKonta { get; set; }
        public string Waluta { get; set; }
        public decimal Kurs { get; set; }
        public decimal StanWPrzeliczeniu { get; set; }
        public DateTime DataStanuKonta { get; set; }
    }
}
