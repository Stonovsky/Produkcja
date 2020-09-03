using GAT_Produkcja.db;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.Strategy
{
    public interface ISurowiecSubiektDictionaryMsAccessStrategy : ISurowiecSubiektStrategy
    {
        bool CzySurowiecWSlownikuMsAccess(int idSurowiecMsAccess);
        int PobierzIdSurowcaZNazwyMsAccess(string nazwaSurowca);
    }
}