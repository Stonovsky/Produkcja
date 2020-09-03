using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Extensions;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers;
using Org.BouncyCastle.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.RW.Decorators
{
    public class RolkaRWDecorator
    {
        private readonly tblProdukcjaRuchTowar rolkaRW;
        private readonly NazwaTowaruSubiektHelper nazwaTowaruSubiektHelper;

        public RolkaRWDecorator(tblProdukcjaRuchTowar rolkaRW)
        {
            this.rolkaRW = rolkaRW;
            nazwaTowaruSubiektHelper = new NazwaTowaruSubiektHelper();
        }

        public tblProdukcjaRuchTowar Uzupelnij(int? idRuchNaglowek, tblProdukcjaZlecenieTowar zlecenieTowar)
        {
            rolkaRW.IDProdukcjaZlecenieTowar = zlecenieTowar is null ? 0 : zlecenieTowar.IDProdukcjaZlecenieTowar;
            rolkaRW.IDProdukcjaRuchNaglowek = idRuchNaglowek.GetValueOrDefault();
            rolkaRW.WagaOdpad_kg = 0;
            rolkaRW.IDRuchStatus = (int)StatusRuchuTowarowEnum.RozchodWewnetrzny_RW;
            rolkaRW.NrZlecenia = PobierzNrZlecenia(zlecenieTowar);
            rolkaRW.TowarNazwaSubiekt = this.nazwaTowaruSubiektHelper.GenerujNazweTowaru(rolkaRW);
            rolkaRW.TowarSymbolSubiekt = this.nazwaTowaruSubiektHelper.GenerujSymbolTowaru(rolkaRW);
            rolkaRW.KierunekPrzychodu = rolkaRW.PobierzKierunekPrzychodu();
            if (rolkaRW.IDProdukcjaRozliczenieStatus == 0)
                rolkaRW.IDProdukcjaRozliczenieStatus = (int)ProdukcjaRozliczenieStatusEnum.NieRozliczono;
            return rolkaRW;
        }

        private int PobierzNrZlecenia(tblProdukcjaZlecenieTowar zlecenieTowar)
        {
            if (zlecenieTowar is null) return default;

            var zlecenieCiecia = zlecenieTowar.tblProdukcjaZlecenieCiecia;
            if (zlecenieCiecia != null)
                return zlecenieCiecia.NrZleceniaCiecia;

            var zlecenieProdukcyjne = zlecenieTowar.tblProdukcjaZlecenie;
            if (zlecenieProdukcyjne != null)
            {
                if (zlecenieProdukcyjne.NrZlecenia.HasValue)
                    return zlecenieProdukcyjne.NrZlecenia.Value;
            }

            return 0;
        }
    }
}
