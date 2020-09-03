using System.Collections.Generic;
using System.Threading.Tasks;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Helpers
{
    public interface IGPRuchTowar_RolkaHelper
    {
        Task<int?> PobierzIDRolkiBazowejAsync(tblProdukcjaRuchTowar rolkaRW, GniazdaProdukcyjneEnum gniazdoProdukcyjnePW);
        Task<decimal> PobierzOdpadZRolkiRwAsync(int idRolkaRW);
        Task<int> PobierzKolejnyNrRolkiAsync(int idGniazdoProdukcyjne);
        Task<int> PobierzKolejnyNrRolkiAsync(tblProdukcjaGniazdoProdukcyjne gniazdoProdukcyjne);
        Task<int> PobierzKolejnyNrRolkiAsync(tblProdukcjaGniazdoProdukcyjne gniazdoProdukcyjne, IEnumerable<tblProdukcjaRuchTowar> listaTowarow);
        
        Task<string> PobierzKolejnyPelnyNrRolkiAsync(tblProdukcjaRuchTowar ruchTowar);
        Task<string> PobierzKolejnyPelnyNrRolkiAsync(tblProdukcjaGniazdoProdukcyjne gniazdoProdukcyjne,
                                                     tblProdukcjaRuchTowar ruchTowar,
                                                     IEnumerable<tblProdukcjaRuchTowar> listaTowarow);
        Task<decimal> PobierzKosztRolki(tblProdukcjaRuchTowar rolkaRW, GniazdaProdukcyjneEnum gniazdaProdukcyjneEnum);
        Task<decimal> PobierzOdpadZRolkiRwAsync(int iDProdukcjaRuchTowar, tblProdukcjaGniazdoProdukcyjne gniazdoProdukcyjne);
    }
}