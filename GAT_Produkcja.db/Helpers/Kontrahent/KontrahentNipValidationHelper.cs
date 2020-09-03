using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Helpers.Kontrahent
{
    public class KontrahentNipValidationHelper
    {
        public bool CzyNipPoprawny(string nip)
        {
            var nipZwalidowany = ZwalidowanyNipPL(nip);

            if (nipZwalidowany == string.Empty)
                return false;

            return true;
        }

        public bool CzyNipPoprawnyDoPobraniaDanychZGus(string nip)
        {

            if (nip == null)
                return false;

            var prefiks = WyseparujPrefiksKraju(nip);
            var nipBezPrefiksu = WyseparujCyfry(nip);

            if (prefiks != string.Empty &&
                prefiks != "PL")
            {
                return false;
            }
            
            if (nipBezPrefiksu.Length != 10)
            {
                return false;
            }

            return true;
        }

        public string ZwalidowanyNipPL(string nip)
        {
            if (string.IsNullOrWhiteSpace(nip))
                return null;

            var prefiks = WyseparujPrefiksKraju(nip);

            if (prefiks != string.Empty &&
                prefiks.Length != 2)
            {
                return string.Empty;
            }

            var nipBezPrefiksu = WyseparujCyfry(nip);

            if (nipBezPrefiksu.Length != 10)
            {
                return string.Empty;
            }
            else
            {
                return nipBezPrefiksu;
            }
        }

        public string WyseparujCyfry(string nip)
        {
            return WyseparujPrefixINrNip(nip).nrNip;
        }

        public string WyseparujPrefiksKraju(string nip)
        {
            return WyseparujPrefixINrNip(nip).prefix;
        }
        private (string prefix,string nrNip) WyseparujPrefixINrNip(string nip)
        {
            nip = nip.Replace(" ", "").Replace("-", "");

            if (string.IsNullOrWhiteSpace(nip))
                return (null,null) ;

            Regex pattern = new Regex(@"(?<Prefix>[a-zA-z]*)\s*(?<NrNip>\d+)");
            Match match = pattern.Match(nip);
            var prefix = match.Groups["Prefix"].Value;
            var nrNip = match.Groups["NrNip"].Value;

            return (prefix,nrNip);
        } 

    }
}
