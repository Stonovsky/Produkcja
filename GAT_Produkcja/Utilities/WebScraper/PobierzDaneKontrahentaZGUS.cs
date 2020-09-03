using GAT_Produkcja.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace GAT_Produkcja.UI.Utilities.WebScraper
{
    public class PobierzDaneKontrahentaZGUS : IPobierzDaneKontrahentaZGUS
    {
        private List<string> daneFirmyZGUSarr;
        private readonly IDaneKontrahentaZGUSWebScraper daneKontrahentaZGUSWebScraper;

        public bool czyDaneKontrahentaIstniejaWGUS { get; private set; }

        public PobierzDaneKontrahentaZGUS(IDaneKontrahentaZGUSWebScraper daneKontrahentaZGUSWebScraper)
        {
            this.daneKontrahentaZGUSWebScraper = daneKontrahentaZGUSWebScraper;
        }

        public async Task<tblKontrahent> PobierzAsync(tblKontrahent kontrahent)
        {
            daneFirmyZGUSarr = await PobierzDaneFirmyZGus(kontrahent);

            if (daneFirmyZGUSarr == null ||
                daneFirmyZGUSarr.Count == 0)
                return new tblKontrahent();

            kontrahent = DodajDaneKontrahentZGusDoModelu(daneFirmyZGUSarr, kontrahent);

            //jezeli lista jest pusta to zwraca kontrahenta bez dodania powyzszych danych
            //czyDaneKontrahentaIstniejaWGUS = false;
            return kontrahent;
        }

        private async Task<List<string>> PobierzDaneFirmyZGus(tblKontrahent kontrahent)
        {
            try
            {
                return await Task.Run(() => daneKontrahentaZGUSWebScraper.PobierzAsync(kontrahent.NIP));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private tblKontrahent DodajDaneKontrahentZGusDoModelu(List<string> daneFirmyZGus, tblKontrahent kontrahent)
        {
            try
            {
                if (daneFirmyZGUSarr.Count >= 2 + 1)
                {
                    kontrahent.Kraj = "Polska";
                    kontrahent.Nazwa = daneFirmyZGUSarr[2];

                    if (daneFirmyZGUSarr.Count >= 3 + 1)
                        kontrahent.Wojewodztwo = daneFirmyZGUSarr[3];


                    if (daneFirmyZGUSarr.Count >= 6 + 1
                        && daneFirmyZGUSarr[6] != null)
                    {
                        Regex pattern = new Regex(@"(?<KodPocztowy>\d{2}-?\d{3})");
                        Match match = pattern.Match(daneFirmyZGUSarr[6]);
                        var kodPocztowy = match.Groups["KodPocztowy"].Value;

                        kontrahent.KodPocztowy = kodPocztowy;
                    }

                    if (daneFirmyZGUSarr.Count >= 7 + 1
                        && daneFirmyZGUSarr[7] != null)
                        kontrahent.Miasto = daneFirmyZGUSarr[7];

                    if (daneFirmyZGUSarr.Count >= 8 + 1
                        && daneFirmyZGUSarr[8] != null)
                    {
                        var ulicaSplit = daneFirmyZGUSarr[8].Split(new string[] { ". " }, StringSplitOptions.None);
                        if (ulicaSplit.Count() > 1)
                            kontrahent.Ulica = ulicaSplit[1];
                    }

                    czyDaneKontrahentaIstniejaWGUS = true;
                    return kontrahent;
                }
                else
                {
                    kontrahent = new tblKontrahent();
                }

            }
            catch (Exception)
            {
                throw;
            }
            return kontrahent;
        }

    }
}
