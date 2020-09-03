using GAT_Produkcja.db;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Ewidencja.Helpers
{
    public interface IRozliczenieMsAccessEwidencjaDeleteHelper
    {
        Task UsunRozliczenieAsync(tblProdukcjaRozliczenie_PWPodsumowanie rozliczenie);
    }
}