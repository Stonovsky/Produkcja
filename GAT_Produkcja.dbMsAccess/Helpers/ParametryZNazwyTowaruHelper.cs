using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess.Helpers
{
    public static class ParametryZNazwyTowaruHelper
    {
        public static (string surowiec, string gramatura) PobierzDaneZNazwyTowaru(string nazwaTowaru)
        {
            Regex pattern = new Regex(@"(?<Surowiec>PES|PP)\s*(?<Gramatura>\d+)\s*(?<UV>UV)*");
            Match match = pattern.Match(nazwaTowaru);
            var surowiec = match.Groups["Surowiec"].Value;
            var gramatura = match.Groups["Gramatura"].Value;

            return (surowiec, gramatura);
        }

        public static string SurowiecSkrot(string nazwaTowaru)
        {
            Regex pattern = new Regex(@"(?<Surowiec>PES|PP)\s*(?<Gramatura>\d+)\s*(?<UV>UV)*");
            Match match = pattern.Match(nazwaTowaru);
            var surowiec = match.Groups["Surowiec"].Value;
            var gramatura = match.Groups["Gramatura"].Value;

            return surowiec;
        }
        public static int Gramatura(string nazwaTowaru)
        {
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
