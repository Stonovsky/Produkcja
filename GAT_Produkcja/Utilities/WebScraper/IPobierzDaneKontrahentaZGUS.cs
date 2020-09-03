using System.Threading.Tasks;
using GAT_Produkcja.db;

namespace GAT_Produkcja.UI.Utilities.WebScraper
{
    public interface IPobierzDaneKontrahentaZGUS
    {
        Task<tblKontrahent> PobierzAsync(tblKontrahent kontrahent);
    }
}