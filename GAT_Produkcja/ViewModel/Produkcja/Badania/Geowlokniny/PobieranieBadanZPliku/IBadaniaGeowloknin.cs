using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny.PobieranieBadanZPliku
{
    public interface IBadaniaGeowloknin
    {
        Task DodajBadaniaZPlikowExcel();
    }
}