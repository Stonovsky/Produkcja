using GAT_Produkcja.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny.ImportBadanZRaportuXls.ImportZPliku
{
    public class ImportZPlikuModel
    {
        public tblWynikiBadanGeowloknin WynikiOgolne { get; set; }
        public List<tblWynikiBadanDlaProbek> WynikiSzczegoloweDlaProbek { get; set; }
    }
}
