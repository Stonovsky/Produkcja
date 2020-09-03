using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Ewidencja
{
    public interface IProdukcjaRuchEwidencjaUCViewModel
    {
        Task LoadAsync(int? id);
    }
}