using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieDodajTowar.Messages
{
    public class ProdukcjaZlecenieDodajTowarMessage
    {
        public GniazdaProdukcyjneEnum GniazdaProdukcyjneEnum { get; set; }
        public DodajUsunEdytujEnum DodajUsunEdytujEnum{ get; set; }
        public tblProdukcjaZlecenieTowar ZlecenieTowar { get; set; }
    }
}
