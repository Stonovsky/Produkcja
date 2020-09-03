using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.dbMsAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess.Models.Adapters
{
    public class NormyZuzycia_tblProdukcjaZlecenie_MieszankaAdapter : tblProdukcjaZlecenieProdukcyjne_Mieszanka
    {
        private readonly NormyZuzycia mieszankaMsAccess;
        private readonly IEnumerable<Surowiec> surowiec;
        private readonly SurowceDictionary surowceDictionary;
        private int towarID;

        public NormyZuzycia_tblProdukcjaZlecenie_MieszankaAdapter(NormyZuzycia normyZuzycia, IEnumerable<Surowiec> surowiec)
        {
            this.mieszankaMsAccess = normyZuzycia;
            this.surowiec = surowiec;
            
            surowceDictionary = new SurowceDictionary();
            
            GenerujTowar(surowiec);
        }

        private void GenerujTowar(IEnumerable<Surowiec> surowiec)
        {
            var towarIdMsAccess = surowiec.Where(s => s.NazwaSurowca == mieszankaMsAccess.Surowiec).FirstOrDefault().Id;
            towarID = surowceDictionary.PobierzIdSurowacaZSubiekt(towarIdMsAccess);
        }

        public override int IDJm => (int)JmEnum.kg;
        public override int? IDProdukcjaZlecenieProdukcyjne => default;
        public override string NazwaTowaru => mieszankaMsAccess.Surowiec;
        public override string JmNazwa => "kg";
        public override decimal ZawartoscProcentowa => mieszankaMsAccess.Ilosc;
        public override int? IDMsAccess => mieszankaMsAccess.Id;
        public override string Uwagi => ((DostawcaEnum)mieszankaMsAccess.Dostawca).ToString();

        public tblProdukcjaZlecenieProdukcyjne_Mieszanka Generuj()
        {
            return new tblProdukcjaZlecenieProdukcyjne_Mieszanka
            {

                IDJm = (int)JmEnum.kg,
                IDProdukcjaZlecenieProdukcyjne = default,
                NazwaTowaru = mieszankaMsAccess.Surowiec,
                JmNazwa = "kg",
                ZawartoscProcentowa = mieszankaMsAccess.Ilosc,
                IloscMieszanki_kg=1,
                IDMsAccess = mieszankaMsAccess.Id,
                Uwagi = ((DostawcaEnum)mieszankaMsAccess.Dostawca).ToString(),
                IDTowar=towarID,
            };
        }
    }
}
