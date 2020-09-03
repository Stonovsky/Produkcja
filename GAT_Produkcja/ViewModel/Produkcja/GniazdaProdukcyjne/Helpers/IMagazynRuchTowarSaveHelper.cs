using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.Helpers
{
    public interface IMagazynRuchTowarSaveHelper
    {
        Task<tblRuchTowar> ZapiszRekordDoTblRuchTowaru(JmEnum jmEnum, StatusRuchuTowarowEnum statusRuchuTowarowEnum, DokumentTypEnum dokumentTypEnum, MagazynyEnum naMagazynEnum, VatEnum vatEnum, int idTowar, int idRuchNaglowek, string nrRolki, string nrPartii);
    }
}