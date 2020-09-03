using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers
{
    public interface ICenyTransferoweHelper
    {
        Task LoadAsync();
        decimal PobierzCeneHurtowa(string nazwaTowaruSubiekt);
        decimal PobierzCeneTransferowa(string nazwaTowaruSubiekt);
    }
}