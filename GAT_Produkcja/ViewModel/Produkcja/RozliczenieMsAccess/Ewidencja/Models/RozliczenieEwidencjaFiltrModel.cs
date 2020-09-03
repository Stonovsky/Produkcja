using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Ewidencja.Models
{
    public class RozliczenieEwidencjaFiltrModel
    {
        public string Jm { get; set; }
        public string Towar { get; set; }
        public string Rodzaj { get; set; }
        public DateTime DataOd { get; set; }
        public DateTime DataDo { get; set; }
    }
}
