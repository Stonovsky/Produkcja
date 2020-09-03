using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.Helpers
{
    public interface IMagazynRuchNaglowekSaveHelper
    {
        Task<tblRuchNaglowek> ZapiszRekordDoTblRuchNaglowek(int idZlecenieProdukcyjne, GniazdaProdukcyjneEnum gniazdoProdukcyjne, StatusRuchuTowarowEnum statusRuchuTowarowEnum, FirmaEnum firmaZ, FirmaEnum firmaDo, MagazynyEnum magazynZ, MagazynyEnum magazynDo);
    }
}