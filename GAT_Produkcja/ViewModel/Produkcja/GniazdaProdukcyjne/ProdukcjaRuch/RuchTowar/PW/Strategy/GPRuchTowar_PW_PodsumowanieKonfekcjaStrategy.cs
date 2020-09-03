using GAT_Produkcja.db;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Helpers
{
    public class GPRuchTowar_PW_PodsumowanieKonfekcjaStrategy : IGPRuchTowar_PW_PodsumowanieHelper
    {
        private List<tblProdukcjaRuchTowar> listaPW;
        private tblProdukcjaZlecenieTowar zlecenieTowar;
        private PodsumowaniePWModel podsumowaniePW_NieKwalifikowane;
        private PodsumowaniePWModel podsumowaniePW_Kwalifikowane;

        public GPRuchTowar_PW_PodsumowanieKonfekcjaStrategy()
        {
        }

        public void Init(IEnumerable<tblProdukcjaRuchTowar> listaPw, tblProdukcjaZlecenieTowar zlecenieTowar)
        {
            this.listaPW = new List<tblProdukcjaRuchTowar>(listaPw);
            this.zlecenieTowar = zlecenieTowar.DeepClone();
        }

        public PodsumowaniePWModel PodsumowanieRolekKwalifikowanych()
        {
            podsumowaniePW_Kwalifikowane = PodsumujPW(p => p.IDGramatura == zlecenieTowar.IDTowarGeowlokninaParametryGramatura
                                                         && p.Dlugosc_m <= zlecenieTowar.Dlugosc_m * 1.05m
                                                         && p.Dlugosc_m >= zlecenieTowar.Dlugosc_m * 0.95m
                                                         && p.Szerokosc_m <= zlecenieTowar.Szerokosc_m * 1.05m
                                                         && p.Szerokosc_m >= zlecenieTowar.Szerokosc_m * 0.95m);
            return podsumowaniePW_Kwalifikowane;
        }
        public PodsumowaniePWModel PodsumowanieRolekNieKwalifikowanych()
        {
            podsumowaniePW_NieKwalifikowane = PodsumujPW(p => p.IDGramatura != zlecenieTowar.IDTowarGeowlokninaParametryGramatura
                                                           || p.Dlugosc_m > zlecenieTowar.Dlugosc_m * 1.05m
                                                           || p.Dlugosc_m < zlecenieTowar.Dlugosc_m * 0.95m
                                                           || p.Szerokosc_m > zlecenieTowar.Szerokosc_m * 1.05m
                                                           || p.Szerokosc_m < zlecenieTowar.Szerokosc_m * 0.95m);
            return podsumowaniePW_NieKwalifikowane;
        }
        public PodsumowaniePWModel PodsumowaniePozostalo()
        {
            if (zlecenieTowar is null) return null;

            PodsumowanieRolekKwalifikowanych();

            return new PodsumowaniePWModel
            {
                IloscSzt = zlecenieTowar.Ilosc_szt - podsumowaniePW_Kwalifikowane.IloscSzt,
                Ilosc_m2 = zlecenieTowar.Ilosc_m2 - podsumowaniePW_Kwalifikowane.Ilosc_m2,
            };
        }
        private PodsumowaniePWModel PodsumujPW(Func<tblProdukcjaRuchTowar, bool> predicate)
        {
            var pozycjeWybrane = listaPW.Where(predicate).ToList();

            return new PodsumowaniePWModel
            {
                IloscSzt = pozycjeWybrane.Count(),
                Ilosc_m2 = pozycjeWybrane.Sum(t => t.Ilosc_m2),
                Waga_kg = pozycjeWybrane.Sum(t => t.Waga_kg),
                Odpad_kg = pozycjeWybrane.Sum(t => t.WagaOdpad_kg)

            };
        }

    }

}
