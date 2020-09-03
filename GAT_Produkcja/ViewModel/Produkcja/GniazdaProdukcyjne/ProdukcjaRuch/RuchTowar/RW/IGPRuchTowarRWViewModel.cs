using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.RW
{
    public interface IGPRuchTowarRWViewModel
    {
        void IsChanged_False();
        Task LoadAsync(int? id);
        Task SaveAsync(int? id);

        bool IsValid { get; }
        bool IsChanged { get; }
    }
}