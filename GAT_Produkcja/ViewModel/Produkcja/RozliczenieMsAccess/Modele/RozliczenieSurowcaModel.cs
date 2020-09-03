using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Modele
{
    public class RozliczenieSurowcaModel
    {
        public string NrZlecenia { get; set; }
        public int IdSurowcaMsAccess { get; set; }
        public int IdSurowcaSubiekt { get; set; }
        public decimal UdzialWMieszance { get; set; }
        public int IdNormaZuzycia { get; set; }

        public string NazwaSurowcaSubiekt { get; set; }
        public string NazwaSurowcaMsAccess { get; set; }
        public decimal Ilosc_kg { get; set; }
        public decimal CenaJednostkowa { get; set; }
        public decimal Wartosc { get; set; }
    }
}
