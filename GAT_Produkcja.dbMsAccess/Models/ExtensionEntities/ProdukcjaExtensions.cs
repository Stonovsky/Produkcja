using GAT_Produkcja.db;
using GAT_Produkcja.db.EntitesInterfaces;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.dbMsAccess.Models.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess
{
    public static class ProdukcjaExtensions 
    {
        /// <summary>
        /// Generuje nazwe towaru
        /// </summary>
        /// <typeparam name="T">typ zawierajacy implementacje <see cref="IProdukcjaRuchTowar"/></typeparam>
        /// <param name="pozycja">pozycja implementujaca interface <see cref="IProdukcjaRuchTowar"/></param>
        /// <param name="preFix">prefix sluzacy do rozroznienia nazw towaru dla gniazd wloknin oraz kalandra</param>
        /// <returns></returns>
        public static string GenerujNazweTowaru<T>(this Produkcja_tblProdukcjaRuchTowarAdapter pozycja, string preFix = null)
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
        public static string GenerujSymbolTowaru<T>(this ItblProdukcjaRuchTowar pozycja, string preFix = null)
        {
            if (pozycja == null) return null;

            string symbol = "ALT_";

            symbol += $"{pozycja.SurowiecSkrot}_{pozycja.Gramatura}_{pozycja.Szerokosc_m}/{pozycja.Dlugosc_m}";
            if (!string.IsNullOrEmpty(preFix))
                symbol = $"{preFix}_" + symbol;

            return symbol;
        }

        /// <summary>
        /// Generuje skrot surowca z nazwy towaru
        /// </summary>
        /// <param name="nazwaTowaru">nazwa towaru</param>
        /// <returns></returns>
        public static string GenerujSurowiecSkrot(this ItblProdukcjaRuchTowar pozycja, string nazwaTowaru)
        {
            if (nazwaTowaru is null) return default;

            Regex pattern = new Regex(@"(?<Surowiec>PES|PP)\s*(?<Gramatura>\d+)\s*(?<UV>UV)*");
            Match match = pattern.Match(nazwaTowaru);
            var surowiec = match.Groups["Surowiec"].Value;
            var gramatura = match.Groups["Gramatura"].Value;

            return surowiec;
        }
        /// <summary>
        /// Generuje gramature z nazwy towaru
        /// </summary>
        /// <param name="nazwaTowaru">nazwa towaru</param>
        /// <returns></returns>
        public static int GenerujGramature(this ItblProdukcjaRuchTowar pozycja, string nazwaTowaru)
        {
            if (nazwaTowaru is null) return default;

            Regex pattern = new Regex(@"(?<Surowiec>PES|PP)\s*(?<Gramatura>\d+)\s*(?<UV>UV)*");
            Match match = pattern.Match(nazwaTowaru);

            var gramatura = match.Groups["Gramatura"].Value;

            int gramaturaInt = 0;
            if (int.TryParse(gramatura, out gramaturaInt) == false)
                return 0;

            return gramaturaInt;
        }

    }
}
