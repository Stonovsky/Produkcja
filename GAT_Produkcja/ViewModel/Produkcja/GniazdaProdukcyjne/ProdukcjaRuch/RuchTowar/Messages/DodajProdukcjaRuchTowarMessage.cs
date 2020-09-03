using GAT_Produkcja.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Messages
{
    public class DodajProdukcjaRuchTowarMessage
    {
        public int IdRuchStatus { get; set; }
        public tblProdukcjaRuchTowar RuchTowar { get; set; }
    }
}
