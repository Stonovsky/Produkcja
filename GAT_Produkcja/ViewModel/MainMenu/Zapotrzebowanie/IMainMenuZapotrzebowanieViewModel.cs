using GAT_Produkcja.Helpers.Interfaces;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.MainMenu.Zapotrzebowanie
{
    public interface IMainMenuZapotrzebowanieViewModel : IButtonActive
    {
        Task LoadAsync(int? id);
    }
}