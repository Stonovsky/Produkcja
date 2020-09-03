using GAT_Produkcja.db.Helpers.Kontrahent;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GAT_Produkcja.UI.Utilities.WebScraper
{
    public class DaneKontrahentaZGUSWebScraper : IDaneKontrahentaZGUSWebScraper
    {
        private KontrahentNipValidationHelper kontrahentNipWalidacja;

        public DaneKontrahentaZGUSWebScraper()
        {
            kontrahentNipWalidacja = new KontrahentNipValidationHelper();
        }

        public async Task<List<string>> PobierzAsync(string nip)
        {
            return await Task.Run(() => PobierzDaneZGUS_IE(nip));
        }

        public async Task<bool?> CzyCzynnyPodatnikVatAsync(string nip)
        {
            return await Task.Run(() => CzyJestCzynnymPodanitkiem(nip));
        }

        private List<string> PobierzDaneZGUS_IE(string nip)
        {
            if (!kontrahentNipWalidacja.CzyNipPoprawny(nip))
                return null;
            else
                nip = kontrahentNipWalidacja.ZwalidowanyNipPL(nip);

            var type = Type.GetTypeFromProgID("internetexplorer.application");
            dynamic ie = Activator.CreateInstance(type);

            ie.Visible = false;
            ie.Navigate("https://wyszukiwarkaregon.stat.gov.pl/appBIR/index.aspx");

            //Oczekiwanie na wlaczenie IE i nawigacje do strony
            while (ie.readyState != 4)
            {
                while (ie.Busy)
                    Thread.Sleep(50);
                Thread.Sleep(50);
            }

            // identyfikacja pola NIP
            dynamic inputElement = null;
            while (inputElement == null)
                inputElement = ie.Document.getElementById("txtNip");

            // wpisanie danych NIP - nip zmienna prywatna metody
            inputElement.Value = nip;

            //kliknięcie przycisku szukaj zindenyfikowanego przez "getElementById" 
            ie.Document.getElementById("btnSzukaj").Click();

            //oczekiwanie na odswiezenie strony
            Thread.Sleep(500);

            //
            string textStrony = ie.Document.Body.innerText;

            // loopowanie przez tabele zidentyfikowana na podstawie "getElementsByClassName"
            var table = ie.Document.getElementsByClassName("tabelaZbiorczaListaJednostekAltRow");

            List<string> daneFirmy = new List<string>();
            foreach (var tr in table) //dla kazdego table row (tr)
            {
                foreach (var td in tr.Children) //dla kazdego table data (td)
                {
                    daneFirmy.Add(td.innerText); // dodaje elementy do listy
                }
            }

            ie.Quit();
            ie = null;

            return daneFirmy;

        }

        private bool? CzyJestCzynnymPodanitkiem(string nip)
        {
            if (!kontrahentNipWalidacja.CzyNipPoprawny(nip))
                return null;
            else
                nip = kontrahentNipWalidacja.ZwalidowanyNipPL(nip);

            var type = Type.GetTypeFromProgID("internetexplorer.application");
            dynamic ie = Activator.CreateInstance(type);

            ie.Visible = false;
            ie.Navigate("https://ppuslugi.mf.gov.pl/?link=VAT&");

            while (ie.readyState != 4)
            {
                while (ie.Busy)
                    Thread.Sleep(50);
                Thread.Sleep(50);
            }

            dynamic inputElement = null;
            while (inputElement == null)
                inputElement = ie.Document.getElementById("b-7");

            inputElement.Value = nip;
            ie.Document.getElementById("b-8").Click();

            Thread.Sleep(500);
            string ttyu = ie.Document.Body.innerText;
            ie.Quit();
            ie = null;

            if (ttyu.Contains("NIP jest zarejestrowany jako podatnik VAT czynny"))
                return true;
            else if (ttyu.Contains("NIP nie jest zarejestrowany jako podatnik VAT"))
                return false;

            return false;
        }
    }
}
