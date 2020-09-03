using System.Collections.Generic;
using System.Threading.Tasks;
using GAT_Produkcja.db;

namespace GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny.ImportBadanZRaportuXls.ImportZPliku
{
    public interface IImportBadanZRaportuZXls
    {
        Task<ImportZPlikuModel> PobierzWynikiBadan();
        List<tblWynikiBadanDlaProbek> PobierzListeBadanSzczegolowychDlaProbek();
        Task<List<tblWynikiBadanDlaProbek>> PobierzListeBadanSzczgolowychDlaProbekAsync();
        tblWynikiBadanGeowloknin PobierzWynikiBadanZPlikuExcel();
        Task<tblWynikiBadanGeowloknin> PobierzWynikiBadanZPlikuExcelAsync();
    }
}