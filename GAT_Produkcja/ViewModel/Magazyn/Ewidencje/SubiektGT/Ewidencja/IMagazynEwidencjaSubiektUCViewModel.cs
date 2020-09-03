using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Magazyn.Ewidencje.SubiektGT.Ewidencja
{
    public interface IMagazynEwidencjaSubiektUCViewModel
    {
        Task LoadAsync(int? id);
    }
}