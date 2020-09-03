using GAT_Produkcja.db;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Models;
using System.Collections.Generic;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Helpers
{
    public interface IGPRuchTowar_PW_PodsumowanieHelper
    {
        void Init(IEnumerable<tblProdukcjaRuchTowar> listaPw, tblProdukcjaZlecenieTowar zlecenieTowar);

        PodsumowaniePWModel PodsumowaniePozostalo();
        PodsumowaniePWModel PodsumowanieRolekKwalifikowanych();
        PodsumowaniePWModel PodsumowanieRolekNieKwalifikowanych();
    }
}