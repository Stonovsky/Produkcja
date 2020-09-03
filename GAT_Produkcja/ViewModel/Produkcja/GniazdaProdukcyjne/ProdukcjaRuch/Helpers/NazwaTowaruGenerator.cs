using GAT_Produkcja.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.Helpers
{
    public class NazwaTowaruGenerator
    {
        private bool ZlecenieIsValid(tblProdukcjaZlecenieTowar zlecenieTowar)
        {
            if (zlecenieTowar is null) return false;
            if (zlecenieTowar.tblTowarGeowlokninaParametryGramatura is null) return false;
            if (zlecenieTowar.tblTowarGeowlokninaParametrySurowiec is null) return false;
            if (zlecenieTowar.Szerokosc_m == 0) return false;
            if (zlecenieTowar.Dlugosc_m == 0) return false;

            return true;
        }

        public string GenerujNazweTowaru(tblProdukcjaZlecenieTowar zlecenieTowar)
        {
            if (!ZlecenieIsValid(zlecenieTowar)) return null;

            return $"Geowłóknina ALTEX AT {zlecenieTowar.tblTowarGeowlokninaParametrySurowiec.Skrot} {zlecenieTowar.tblTowarGeowlokninaParametryGramatura.Gramatura} " +
                   $"({zlecenieTowar.Szerokosc_m}mx{zlecenieTowar.Dlugosc_m}m)";
        }

        public string GenerujNazweTowaru(tblProdukcjaRuchTowar towar)
        {
            if (!RuchTowar(towar)) return null;

            return $"Geowłóknina ALTEX AT {towar.tblTowarGeowlokninaParametrySurowiec.Skrot} {towar.tblTowarGeowlokninaParametryGramatura.Gramatura} " +
                   $"({towar.Szerokosc_m}mx{towar.Dlugosc_m}m)";
        }

        private bool RuchTowar(tblProdukcjaRuchTowar towar)
        {
            if (towar is null) return false;
            if (towar.tblTowarGeowlokninaParametryGramatura is null) return false;
            if (towar.tblTowarGeowlokninaParametrySurowiec is null) return false;
            if (towar.Dlugosc_m == 0) return false;
            if (towar.Szerokosc_m == 0) return false;

            return true;
        }
    }
}
