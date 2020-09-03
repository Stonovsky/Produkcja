using GAT_Produkcja.db.EntitesInterfaces;
using GAT_Produkcja.db.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess.Helpers
{
    public static class ModelHelper
    {
        /// <summary>
        /// Generuje nazwe towaru
        /// </summary>
        /// <typeparam name="T">typ zawierajacy implementacje <see cref="IProdukcjaRuchTowar"/></typeparam>
        /// <param name="pozycja">pozycja implementujaca interface <see cref="IProdukcjaRuchTowar"/></param>
        /// <param name="preFix">prefix sluzacy do rozroznienia nazw towaru dla gniazd wloknin oraz kalandra</param>
        /// <returns></returns>
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

        /// <summary>
        /// Generuje skrot surowca z nazwy towaru
        /// </summary>
        /// <param name="nazwaTowaru">nazwa towaru</param>
        /// <returns></returns>
        public static string GenerujSurowiecSkrot(string nazwaTowaru)
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
        public static int GenerujGramature(string nazwaTowaru)
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

        public static int GenerujGramaturaId(string nazwaTowaru)
        {
            if (nazwaTowaru is null) return default;

            var gramatura = GenerujGramature(nazwaTowaru);
            var gramaturaEnum = $"Gramatura_{gramatura}";
            return (int)Enum.Parse(typeof(TowarGeowlokninaGramaturaEnum), gramaturaEnum);
        }
        public static int GenerujSurowiecId(string nazwaTowaru)
        {
            if (nazwaTowaru is null) return default;

            var surowiec = GenerujSurowiecSkrot(nazwaTowaru);
            return (int)Enum.Parse(typeof(TowarGeowlokninaSurowiecEnum), surowiec);
        }

        public static int CzyRozliczono(bool czyZaksiegowano)
        {
            if (czyZaksiegowano)
                return (int)ProdukcjaRozliczenieStatusEnum.Rozliczono;
            else
                return (int)ProdukcjaRozliczenieStatusEnum.NieRozliczono;
        }
    }
}
