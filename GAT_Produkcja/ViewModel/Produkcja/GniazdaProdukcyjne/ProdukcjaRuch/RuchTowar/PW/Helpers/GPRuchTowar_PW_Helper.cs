using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Helpers;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Helpers
{
    public class GPRuchTowar_PW_Helper : IGPRuchTowar_PW_Helper
    {
        public IGPRuchTowar_PW_ZaawansowanieHelper ZaawansowanieHelper { get; }
        public IGPRuchTowar_RolkaHelper RolkaHelper { get; }

        public IGPRuchTowar_PW_PodsumowanieHelper PodsumowanieHelper { get; }

        public IGPRuchTowar_PW_KonfekcjaHelper KonfekcjaHelper { get; }

        public IGPRuchTowar_PW_RolkaBazowaHelper RolkaBazowaHelper { get; }

        public GPRuchTowar_PW_Helper(IGPRuchTowar_PW_ZaawansowanieHelper towar_PW_ZaawansowanieHelper,
                                     IGPRuchTowar_RolkaHelper rolkaHelper,
                                     IGPRuchTowar_PW_PodsumowanieHelper podsumowanieHelper,
                                     IGPRuchTowar_PW_KonfekcjaHelper konfekcjaHelper,
                                     IGPRuchTowar_PW_RolkaBazowaHelper rolkaBazowaHelper)
        {
            ZaawansowanieHelper = towar_PW_ZaawansowanieHelper;
            RolkaHelper = rolkaHelper;
            PodsumowanieHelper = podsumowanieHelper;
            KonfekcjaHelper = konfekcjaHelper;
            RolkaBazowaHelper = rolkaBazowaHelper;
        }

    }
}
