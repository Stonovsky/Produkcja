using System.Collections.Generic;
using System.Threading.Tasks;

namespace GAT_Produkcja.UI.Utilities.WebScraper
{
    public interface IDaneKontrahentaZGUSWebScraper
    {
        Task<bool?> CzyCzynnyPodatnikVatAsync(string nip);
        Task<List<string>> PobierzAsync(string nip);
    }
}