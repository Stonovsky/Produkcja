using GAT_Produkcja.db;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Badania
{
    public interface IGPRuchTowarBadaniaViewModel
    {
        Task LoadAsync(int? ididTblProdukcjaRuchTowar);
        tblProdukcjaRuchTowarBadania Save(int? idTblProdukcjaRuchTowar);

    }
}