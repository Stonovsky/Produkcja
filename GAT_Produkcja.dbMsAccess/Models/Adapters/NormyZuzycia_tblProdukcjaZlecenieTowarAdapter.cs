using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.dbMsAccess.Enums;
using GAT_Produkcja.dbMsAccess.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess.Models.Adapters
{
    public class NormyZuzycia_tblProdukcjaZlecenieTowarAdapter : tblProdukcjaZlecenieTowar
    {
        private readonly NormyZuzycia mieszankaMsAccess;
        private readonly Dyspozycje zlecenieMsAccess;
        private readonly IEnumerable<Surowiec> surowiec;
        private int gramatura;
        private int gramaturaID;
        private string surowiecSkrot;
        private int surowiecID;
        private DostawcaEnum dostawcaEnum;
        private SurowceDictionary surowceDictionary;
        private int towarID;

        public NormyZuzycia_tblProdukcjaZlecenieTowarAdapter(IEnumerable<NormyZuzycia> normyZuzycia,
                                                             Dyspozycje dyspozycje,
                                                             IEnumerable<Surowiec> surowiec)
        {
            this.mieszankaMsAccess = normyZuzycia.First();
            this.zlecenieMsAccess = dyspozycje;
            this.surowiec = surowiec;
            surowceDictionary = new SurowceDictionary();

            GenerujGramature(mieszankaMsAccess.Artykul);
            GenerujSurowiec(mieszankaMsAccess.Artykul);
            GenerujDostawce(mieszankaMsAccess.Dostawca);
            GenerujTowar(surowiec);
        }

        private void GenerujTowar(IEnumerable<Surowiec> surowiec)
        {
            var towarIdMsAccess = surowiec.Where(s => s.NazwaSurowca == mieszankaMsAccess.Surowiec).FirstOrDefault().Id;
            towarID = surowceDictionary.PobierzIdSurowacaZSubiekt(towarIdMsAccess);
        }

        private void GenerujDostawce(int dostawcaId)
        {
            Enum.TryParse(dostawcaId.ToString(), out dostawcaEnum);
        }

        private void GenerujSurowiec(string towar)
        {
            surowiecSkrot = ParametryZNazwyTowaruHelper.SurowiecSkrot(towar);

            TowarGeowlokninaSurowiecEnum surowiecEnum;
            Enum.TryParse(surowiecSkrot, out surowiecEnum);

            surowiecID = (int)surowiecEnum;
        }

        private void GenerujGramature(string towar)
        {
            gramatura = ParametryZNazwyTowaruHelper.Gramatura(towar);
            TowarGeowlokninaGramaturaEnum gramaturaEnum;
            Enum.TryParse("Gramatura_" + gramatura.ToString(), out gramaturaEnum);

            gramaturaID = (int)gramaturaEnum;
        }

        public override int? IDProdukcjaGniazdoProdukcyjne => (int)GniazdaProdukcyjneEnum.LiniaWloknin;
        public override int? IDProdukcjaZlecenieStatus => (int)ProdukcjaZlecenieStatusEnum.Zakonczone;
        public override int IDTowarGeowlokninaParametryGramatura => gramaturaID;
        public override int? Gramatura => gramatura;
        public override int IDTowarGeowlokninaParametrySurowiec => surowiecID;
        public override string Surowiec => surowiecSkrot;
        public override string Uwagi => dostawcaEnum.ToString();
        public override bool CzyKalandrowana => false;
        public override bool CzyWielokrotnoscDlugosci => true;
        public override string RodzajPakowania => "Jumbo rolla bez dodatkowego pakowania";
        public override decimal Ilosc_m2 => zlecenieMsAccess.Ilosc_m2;
        public override decimal Zaawansowanie => 1;
        public override string TowarNazwa => zlecenieMsAccess.Artykul;


        public tblProdukcjaZlecenieTowar Generuj()
        {
            return new tblProdukcjaZlecenieTowar
            {
                IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaWloknin,
                IDProdukcjaZlecenieStatus = (int)ProdukcjaZlecenieStatusEnum.Zakonczone,
                IDTowarGeowlokninaParametryGramatura = gramaturaID,
                IDTowarGeowlokninaParametrySurowiec = surowiecID,
                IDTowar=towarID,
                Gramatura = gramatura,
                Surowiec = surowiecSkrot,
                Uwagi = dostawcaEnum.ToString() + "; szer. i dł. rolki - brak w MSAccess",
                CzyKalandrowana = false,
                CzyWielokrotnoscDlugosci = true,
                RodzajPakowania = "Jumbo rolla bez dodatkowego pakowania",
                Szerokosc_m=1,
                Dlugosc_m=1,
                Ilosc_m2 = zlecenieMsAccess.Ilosc_m2,
                Zaawansowanie = 1,
                TowarNazwa = zlecenieMsAccess.Artykul,

            };
        }
    }
}
