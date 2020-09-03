using GAT_Produkcja.db;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny.SprawdzenieWynikowBadan
{
    public interface IWeryfikacjaGramaturyGeowlokninHelper
    {
        bool CzyGramaturaZgodna(decimal gramaturaSrednia, int idGramatura);
        bool CzyGramaturaZgodna(tblProdukcjaRuchTowar towar);
        Task LoadAsync();
        tblTowarGeowlokninaParametryGramatura PobierzWlasciwaGramature(decimal gramaturaSrednia, tblProdukcjaRuchTowar towar);
        //int PobierzWlasciwaGramatureId(decimal gramaturaSrednia);
    }
}