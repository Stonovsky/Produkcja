using System;
using System.Threading.Tasks;
using GAT_Produkcja.Utilities.CurrencyService.NBP.Model;

namespace GAT_Produkcja.Utilities.CurrencyService.NBP
{
    public interface INBPService
    {
        Task<decimal> GetActualCurrencyRate(CurrencyShorcutEnum currencyShorcutEnum);
        Task<decimal> GetActualCurrencyRate(string currencyShortcut);

    }
}