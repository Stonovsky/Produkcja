using GAT_Produkcja.db.EntitesInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Helpers
{
    public class NazwaTowaru
    {
        public static string GenerujNazweTowaru<T>(T pozycja, string preFix = null)
                                      where T : IProdukcjaRuchTowar
        {
            if (pozycja == null) return null;

            string nazwa = "Geowłóknina ALTEX AT";
            nazwa += $" {pozycja.SurowiecSkrot} {pozycja.Gramatura} ({pozycja.Szerokosc_m}mx{pozycja.Dlugosc_m}m)";

            if (!string.IsNullOrEmpty(preFix))
                nazwa = $"{preFix} " + nazwa;

            return nazwa;
        }
        /// <summary>
        /// Generuje symbol towaru
        /// </summary>
        /// <typeparam name="T">typ zawierajacy implementacje <see cref="IProdukcjaRuchTowar"/></typeparam>
        /// <param name="pozycja">pozycja implementujaca interface <see cref="IProdukcjaRuchTowar"/></param>
        /// <param name="preFix">prefix sluzacy do rozroznienia nazw towaru dla gniazd wloknin oraz kalandra</param>
        /// <returns></returns>
        public static string GenerujSymbolTowaru<T>(T pozycja, string preFix = null)
                                               where T : IProdukcjaRuchTowar

        {
            if (pozycja == null) return null;

            string symbol = "ALT_";

            symbol += $"{pozycja.SurowiecSkrot}_{pozycja.Gramatura}_{pozycja.Szerokosc_m}/{pozycja.Dlugosc_m}";
            if (!string.IsNullOrEmpty(preFix))
                symbol = $"{preFix}_" + symbol;

            return symbol;
        }

    }
}
