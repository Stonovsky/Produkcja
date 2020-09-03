using GAT_Produkcja.db;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Ewidencja.Palety.Adapters
{
    public class ProdukcjaRuchPaletyAdapter
    {
        private readonly IEnumerable<tblProdukcjaRuchTowar> listOfEntities;

        #region CTOR
        public ProdukcjaRuchPaletyAdapter(IEnumerable<tblProdukcjaRuchTowar> listOfEntities)
        {
            this.listOfEntities = listOfEntities;
        }

        #endregion

        public IEnumerable<tblProdukcjaRuchTowar> GroupBy<T>(Func<tblProdukcjaRuchTowar, T> selector)
        {
            return listOfEntities.GroupBy(selector)
                                 .Select(se => new tblProdukcjaRuchTowar
                                 {
                                     DataDodania = se.Last().DataDodania,
                                     TowarNazwaSubiekt = se.First().TowarNazwaSubiekt,
                                     TowarSymbolSubiekt = se.First().TowarSymbolSubiekt,
                                     NrPalety = se.First().NrPalety,
                                     NrZlecenia = se.First().NrZlecenia,
                                     tblProdukcjaZlecenieTowar = new tblProdukcjaZlecenieTowar
                                     {
                                         tblProdukcjaZlecenieCiecia = new tblProdukcjaZlecenieCiecia
                                         {
                                             tblKontrahent = se.First().tblProdukcjaZlecenieTowar?.tblProdukcjaZlecenieCiecia?.tblKontrahent,
                                         }
                                     }
                                 });
        }
    }
}
