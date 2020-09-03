using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Helpers;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Helpers
{
    public interface IGPRuchTowar_PW_Helper
    {
        IGPRuchTowar_PW_ZaawansowanieHelper ZaawansowanieHelper { get; }
        IGPRuchTowar_RolkaHelper RolkaHelper { get; }
        IGPRuchTowar_PW_PodsumowanieHelper PodsumowanieHelper { get; }
        IGPRuchTowar_PW_KonfekcjaHelper KonfekcjaHelper { get; }
        IGPRuchTowar_PW_RolkaBazowaHelper RolkaBazowaHelper { get; }
    }
}