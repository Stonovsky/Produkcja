using GAT_Produkcja.Utilities.CurrencyService.NBP.Model;
using GAT_Produkcja.Utilities.Wrappers;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
//using System.Net.Http;

namespace GAT_Produkcja.Utilities.CurrencyService.NBP
{
    public class NBPService : INBPService
    {
        private string URL = $"https://api.nbp.pl/api/exchangerates/rates/a/eur/2020-05-21/?format=json";
        private DateTime dateOfCurrency;
        private readonly IHttpClientWrapper httpClientHandler;

        #region CTOR
        public NBPService(IHttpClientWrapper httpClientHandler)
        {
            this.httpClientHandler = httpClientHandler;
        }
        #endregion

        public async Task<decimal> GetActualCurrencyRate(CurrencyShorcutEnum currencyShorcutEnum)
        {
            string url = GetUrl(GetCurrency(currencyShorcutEnum));

            var json = await httpClientHandler.GetStringAsync(url);

            if (string.IsNullOrEmpty(json)) return 0;

            var currency = JsonConvert.DeserializeObject<NbpCurrencyModel>(json);
            return currency.rates.First().mid;
        }


        public async Task<decimal> GetActualCurrencyRate(string  currencyShortcut)
        {
            if (string.IsNullOrEmpty(currencyShortcut)) return 0;
            if (currencyShortcut.ToLower().Contains("pln")) return 1;

            return await GetCurrencyFromNbp(currencyShortcut);
        }

        private async Task<decimal> GetCurrencyFromNbp(string currencyShortcut)
        {
            string url = GetUrl(GetCurrencyFromShortcut(currencyShortcut));

            var json = await httpClientHandler.GetStringAsync(url);

            if (string.IsNullOrEmpty(json)) return 0;

            var currency = JsonConvert.DeserializeObject<NbpCurrencyModel>(json);
            return currency.rates.First().mid;
        }

        private string GetCurrencyFromShortcut(string currencyShortcut)
        {
            if (currencyShortcut.ToLower().Contains("eur")) return "eur";
            if (currencyShortcut.ToLower().Contains("usd")) return "usd";
            if (currencyShortcut.ToLower().Contains("rub")) return "rub";

            throw new ArgumentException("Brak waluty w zestawieniu.");
        }

        private string GetUrl(string currencyShortcut)
        {
            return $"https://api.nbp.pl/api/exchangerates/rates/a/{currencyShortcut}/?format=json";

        }

        private string GetCurrency(CurrencyShorcutEnum currencyShorcutEnum)
        {
            switch (currencyShorcutEnum)
            {
                case CurrencyShorcutEnum.EUR:
                    return "eur";
                case CurrencyShorcutEnum.USD:
                    return "usd";
                case CurrencyShorcutEnum.RUB:
                    return "rub";
                default:
                    return null;
            }
        }
    }
}
