using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.Strategy
{
    public class SurowiecSubiektZNazwyMsAccessStrategy : ISurowiecSubiektZNazwyMsAccessStrategy
    {
        private readonly IUnitOfWork unitOfWork;
        private IEnumerable<vwMagazynRuchGTX> surowceSubiekt;

        public SurowiecSubiektZNazwyMsAccessStrategy(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<int> PobierzIdSurowcaZSubiektDla(string nazwaSurowcaMsAccess, decimal ilosc = 1)
        {
            await PobierzSurowceZSubiekt();
            var surowiecSubiekt = PobierzSurowceZSubiektDla(nazwaSurowcaMsAccess, ilosc);

            return surowiecSubiekt.IdTowar;
        }


        public async Task<vwMagazynRuchGTX> PobierzSurowiecZSubiektDla(string nazwaSurowcaMsAccess, decimal ilosc = 1)
        {
            await PobierzSurowceZSubiekt();
            return PobierzSurowceZSubiektDla(nazwaSurowcaMsAccess, ilosc);
        }
        private async Task PobierzSurowceZSubiekt()
        {
            if (surowceSubiekt is null
                || !surowceSubiekt.Any())
                surowceSubiekt = await unitOfWork.vwMagazynRuchGTX.WhereAsync(s => s.IdMagazyn == (int)MagazynyGTXEnum.Surowce_Geowloknina_SWL
                                                                                && s.Pozostalo > 1);
        }

        private vwMagazynRuchGTX PobierzSurowceZSubiektDla(string nazwaSurowcaMsAccess, decimal ilosc)
        {

            var surowiec = PobierzSurowiecIGruboscWlokna(nazwaSurowcaMsAccess).surowiec;
            var gruboscWlokna = PobierzSurowiecIGruboscWlokna(nazwaSurowcaMsAccess).gruboscWlokna;

            var surowiecSubiekt = PobierzSurowiecZSubiekt(surowiec, gruboscWlokna, ilosc);

            return surowiecSubiekt;
        }

        private vwMagazynRuchGTX PobierzSurowiecZSubiekt(string surowiec, string gruboscWlokna, decimal ilosc)
        {
            //foreach (var towar in surowceSubiekt)
            //    DostosujSymbolTowaru(towar);

            IEnumerable<vwMagazynRuchGTX> surowiecSubiekt = PobierzWlasciwySurowiecSubiekt(surowiec, gruboscWlokna, ilosc);

            if (surowiecSubiekt.Any())
                return surowiecSubiekt.First();
            else
                throw new ArgumentException();

        }

        private static void DostosujSymbolTowaru(vwMagazynRuchGTX towar)
        {
            int indexSlash = towar.TowarSymbol.IndexOf("/");
            if (indexSlash > 0)
                towar.TowarSymbol = towar.TowarSymbol.Substring(0, indexSlash);
        }

        private IEnumerable<vwMagazynRuchGTX> PobierzWlasciwySurowiecSubiekt(string surowiec, string gruboscWlokna, decimal ilosc)
        {
            Regex patternSurowiec = new Regex($"{surowiec}");

            var surowiecSubiekt = surowceSubiekt
                .Where(s => patternSurowiec.IsMatch(s.TowarSymbol))
                .Where(s => s.Pozostalo >= ilosc)
                .ToList();

            if (CzyOdpad(gruboscWlokna))
            {
                surowiecSubiekt = surowiecSubiekt.Where(s => s.TowarSymbol.ToLower().Contains("tasmy"))
                .ToList();
            }
            else
            {
                Regex patternGruboscWlokna = new Regex($"_[{gruboscWlokna}]");

                surowiecSubiekt = surowiecSubiekt.Where(s => patternGruboscWlokna.IsMatch(s.TowarSymbol))
                .ToList();
            }
            return surowiecSubiekt;
        }

        private bool CzyOdpad(string gruboscWlokna)
        {
            return string.IsNullOrEmpty(gruboscWlokna);
        }

        private (string surowiec, string gruboscWlokna) PobierzSurowiecIGruboscWlokna(string nazwaSurowcaMsAccess)
        {
            nazwaSurowcaMsAccess = nazwaSurowcaMsAccess.Replace("BICO", "PES");

            Regex pattern = new Regex(@"(?<Surowiec>(PES|PP))\s?(?<GruboscWl>\d?,?\d?)");
            Match match = pattern.Match(nazwaSurowcaMsAccess);
            var surowiec = match.Groups["Surowiec"].Value;
            var gruboscWlokna = match.Groups["GruboscWl"].Value;

            return (surowiec, gruboscWlokna);
        }

    }
}
