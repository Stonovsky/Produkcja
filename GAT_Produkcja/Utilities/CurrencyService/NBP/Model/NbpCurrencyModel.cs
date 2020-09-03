using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.CurrencyService.NBP.Model
{
    public class NbpCurrencyModel
    {
        public string table { get; set; }
        public string  currency { get; set; }
        public string code { get; set; }
        public List<NbpRateModel> rates { get; set; }

    }
}
