using GAT_Produkcja.db.Enums;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Helpers;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Strategy
{
    public interface IGPRuchTowar_PW_PodsumowanieStrategyFactory
    {
        IGPRuchTowar_PW_PodsumowanieHelper PobierzPodsumowanieStrategy(GniazdaProdukcyjneEnum gniazdaProdukcyjneEnum);
    }
}