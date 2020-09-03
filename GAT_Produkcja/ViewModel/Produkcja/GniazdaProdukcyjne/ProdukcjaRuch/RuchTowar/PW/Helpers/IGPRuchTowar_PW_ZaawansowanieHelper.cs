using GAT_Produkcja.db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Helpers
{
    public interface IGPRuchTowar_PW_ZaawansowanieHelper
    {
        Task<decimal> PobierzZaawansowanie_ProdukcjaZlecenieTowar(tblProdukcjaZlecenieTowar zlecenieTowar);
        Task<decimal> PobierzZawansowanie_ProdukcjaZlecenie(int idProdukcjaZlecenieTowar);
        decimal PobierzRozchodRolkiRw(tblProdukcjaRuchTowar rolkaRW, IEnumerable<tblProdukcjaRuchTowar> ListaPW);

    }
}