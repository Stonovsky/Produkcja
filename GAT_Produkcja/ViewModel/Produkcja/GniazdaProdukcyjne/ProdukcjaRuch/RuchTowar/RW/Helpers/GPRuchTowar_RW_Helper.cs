using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.RW.Helpers
{
    public class GPRuchTowar_RW_Helper : IGPRuchTowar_RW_Helper
    {

        public IGPRuchTowar_RolkaHelper RolkaBazowaHelper { get; }

        public GPRuchTowar_RW_Helper(IGPRuchTowar_RolkaHelper rolkaBazowaHelper)
        {
            RolkaBazowaHelper = rolkaBazowaHelper;
        }
    }
}
