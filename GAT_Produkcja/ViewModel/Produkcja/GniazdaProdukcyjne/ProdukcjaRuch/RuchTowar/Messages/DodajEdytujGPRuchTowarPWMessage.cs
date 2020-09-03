using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Messages
{
    public class DodajEdytujGPRuchTowarMessage
    {
        public tblRuchStatus RuchStatus { get; set; }
        public tblProdukcjaRuchTowar RuchTowar { get; set; }
        public tblProdukcjaRuchTowar RolkaRW { get; set; }
        public tblProdukcjaZlecenie ZlecenieProdukcyjne { get; set; }
        public DodajUsunEdytujEnum DodajUsunEdytujEnum { get; set; }
        public ProdukcjaZlecenieEnum ProdukcjaZlecenieEnum { get; set; }
    }
}
