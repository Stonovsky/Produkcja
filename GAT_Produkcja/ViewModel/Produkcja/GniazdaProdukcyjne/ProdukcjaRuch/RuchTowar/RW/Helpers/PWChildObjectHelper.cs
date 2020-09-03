using GAT_Produkcja.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.RW.Helpers
{
    public class PWChildObjectHelper
    {
        public virtual void Remove(IEnumerable<tblProdukcjaRuchTowar> listOfEntites, IEnumerable<string> childsNotToRemove)
        {
            var newEntites = listOfEntites.Where(e => e.IDProdukcjaRuchTowar == 0);
            newEntites.ToList().ForEach(e => e.RemoveChildObjects(childsNotToRemove));
        }
    }
}
