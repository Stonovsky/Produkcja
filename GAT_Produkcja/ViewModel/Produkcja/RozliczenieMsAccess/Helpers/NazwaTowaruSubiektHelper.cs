using GAT_Produkcja.db;
using GAT_Produkcja.db.EntitesInterfaces;
using GAT_Produkcja.dbMsAccess.EntitesInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers
{
    public class NazwaTowaruSubiektHelper
    {
        /// <summary>
        /// Generuje nazwe towaru dla Subiekta z nazwy artykulu z MsAccess 
        /// </summary>
        /// <param name="pozycja"></param>
        /// <returns></returns>
        public virtual string GenerujNazweTowaru(IGniazdoProdukcyjne pozycja)
        {
            if (pozycja is null) return null;

            string nazwa = "Geowłóknina ALTEX AT";

            var DaneZTowaru = PobierzDaneZNazwyTowaru(pozycja.Artykul);

            nazwa += $" {DaneZTowaru.surowiec} {DaneZTowaru.gramatura} ({pozycja.Szerokosc.ToString("#")}mx{pozycja.Dlugosc.ToString("#")}m)";

            return nazwa;
        }

        public virtual string GenerujNazweTowaru(tblProdukcjaRuchTowar pozycja)
        {
            if (pozycja is null) return null;
            if (pozycja.tblTowarGeowlokninaParametryGramatura is null) return null;
            if (pozycja.tblTowarGeowlokninaParametrySurowiec is null) return null;

            string nazwa = "Geowłóknina ALTEX AT";
            
            nazwa += $" {pozycja.tblTowarGeowlokninaParametrySurowiec.Skrot} {pozycja.tblTowarGeowlokninaParametryGramatura.Gramatura} ({pozycja.Szerokosc_m.ToString("#")}mx{pozycja.Dlugosc_m.ToString("#")}m)";

            return nazwa;
        }
        /// <summary>
        /// Generuje symbol towaru dla Subiekta z nazwy artykulu z MsAccess 
        /// </summary>
        /// <param name="pozycja"></param>
        /// <returns></returns>
        public virtual string GenerujSymbolTowaru(IGniazdoProdukcyjne pozycja)
        {
            if (pozycja is null) return null;

            string symbol = "ALT_";
            var daneZTowaru = PobierzDaneZNazwyTowaru(pozycja.Artykul);

            symbol += $"{daneZTowaru.surowiec}_{daneZTowaru.gramatura}_{pozycja.Szerokosc.ToString("#")}/{pozycja.Dlugosc.ToString("#")}";

            return symbol;
        }

        /// <summary>
        /// Generuje symbol towaru dla Subiekta z nazwy artykulu z <see cref="tblProdukcjaRuchTowar"/> 
        /// </summary>
        /// <param name="pozycja"> Pozycja pw z ktorej generowana jest nazwa towaru.
        ///  Niezbedne sa tabele zalezne dot gramatury <see cref="tblTowarGeowlokninaParametryGramatura"/>
        ///  oraz surowca <see cref="tblTowarGeowlokninaParametrySurowiec"/>
        ///  </param>
        /// <returns></returns>
        public virtual string GenerujSymbolTowaru(tblProdukcjaRuchTowar pozycja)
        {
            if (pozycja is null) return null;
            if (pozycja.tblTowarGeowlokninaParametryGramatura is null) return null;
            if (pozycja.tblTowarGeowlokninaParametrySurowiec is null) return null;

            string symbol = "ALT_";

            symbol += $"{pozycja.tblTowarGeowlokninaParametrySurowiec.Skrot}_{pozycja.tblTowarGeowlokninaParametryGramatura.Gramatura}_{pozycja.Szerokosc_m.ToString("#")}/{pozycja.Dlugosc_m.ToString("#")}";

            return symbol;
        }
        /// <summary>
        /// Pobiera surowiec oraz gramature z nazwy za pomoca Regexa
        /// </summary>
        /// <param name="pozycja"></param>
        /// <returns></returns>
        public (string surowiec, string gramatura) PobierzDaneZNazwyTowaru(string nazwaTowaru)
        {
            Regex pattern = new Regex(@"(?<Surowiec>PES|PP)\s*(?<Gramatura>\d+)\s*(?<UV>UV)*");
            Match match = pattern.Match(nazwaTowaru);
            var surowiec = match.Groups["Surowiec"].Value;
            var gramatura = match.Groups["Gramatura"].Value;

            return (surowiec, gramatura);
        }

    }
}
