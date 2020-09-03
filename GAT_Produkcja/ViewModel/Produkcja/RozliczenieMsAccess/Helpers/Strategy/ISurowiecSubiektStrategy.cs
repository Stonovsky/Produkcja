using GAT_Produkcja.db;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.Strategy
{
    public interface ISurowiecSubiektStrategy
    {
        Task<int> PobierzIdSurowcaZSubiektDla(string nazwaSurowcaMsAccess, decimal ilosc = 1);
        Task<vwMagazynRuchGTX> PobierzSurowiecZSubiektDla(string nazwaSurowcaMsAccess, decimal ilosc = 1);
    }
}