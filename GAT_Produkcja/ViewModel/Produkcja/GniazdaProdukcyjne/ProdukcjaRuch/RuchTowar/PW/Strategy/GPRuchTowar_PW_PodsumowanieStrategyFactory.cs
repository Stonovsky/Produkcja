using GAT_Produkcja.db.Enums;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Strategy
{
    public class GPRuchTowar_PW_PodsumowanieStrategyFactory : IGPRuchTowar_PW_PodsumowanieStrategyFactory
    {
        public IGPRuchTowar_PW_PodsumowanieHelper PobierzPodsumowanieStrategy(GniazdaProdukcyjneEnum gniazdaProdukcyjneEnum)
        {
            switch (gniazdaProdukcyjneEnum)
            {
                case GniazdaProdukcyjneEnum.LiniaWloknin:
                    return new GPRuchTowar_PW_PodsumowanieWlokninaIKalanderStrategy();
                case GniazdaProdukcyjneEnum.LiniaDoKalandowania:
                    return new GPRuchTowar_PW_PodsumowanieWlokninaIKalanderStrategy();
                case GniazdaProdukcyjneEnum.LiniaDoKonfekcji:
                    return new GPRuchTowar_PW_PodsumowanieKonfekcjaStrategy();
                default:
                    return new GPRuchTowar_PW_PodsumowanieWlokninaIKalanderStrategy();
            }
        }
    }
}
