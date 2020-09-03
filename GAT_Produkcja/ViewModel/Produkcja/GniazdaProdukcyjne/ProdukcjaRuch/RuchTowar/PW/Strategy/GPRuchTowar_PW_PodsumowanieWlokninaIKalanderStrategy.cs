using GAT_Produkcja.db;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Strategy
{
    public class GPRuchTowar_PW_PodsumowanieWlokninaIKalanderStrategy : IGPRuchTowar_PW_PodsumowanieHelper
    {
        private List<tblProdukcjaRuchTowar> listaPW;
        private tblProdukcjaZlecenieTowar zlecenieTowar;
        private PodsumowaniePWModel podsumowaniePW_NieKwalifikowane;
        private PodsumowaniePWModel podsumowaniePW_Kwalifikowane;

        public GPRuchTowar_PW_PodsumowanieWlokninaIKalanderStrategy()
        {
        }

        public void Init(IEnumerable<tblProdukcjaRuchTowar> listaPw, tblProdukcjaZlecenieTowar zlecenieTowar)
        {

            this.listaPW = listaPw.CopyList() as List<tblProdukcjaRuchTowar>;
            this.zlecenieTowar = zlecenieTowar.DeepClone();
        }

        public PodsumowaniePWModel PodsumowanieRolekKwalifikowanych()
        {
            podsumowaniePW_Kwalifikowane = PodsumujPW(p => CzyRolkaZgodna(p), true);
            return podsumowaniePW_Kwalifikowane;
        }

        public PodsumowaniePWModel PodsumowanieRolekNieKwalifikowanych()
        {
            podsumowaniePW_NieKwalifikowane = PodsumujPW(p => !CzyRolkaZgodna(p), false);
            return podsumowaniePW_NieKwalifikowane;
        }
        private bool CzyRolkaZgodna(tblProdukcjaRuchTowar towar)
        {
            return towar.IDGramatura == zlecenieTowar.IDTowarGeowlokninaParametryGramatura
                    && WielokrotnoscDlugosci(towar.Dlugosc_m)
                    && WielokrotnoscSzerokoscie(towar.Szerokosc_m);
        }

        private bool WielokrotnoscDlugosci(decimal dlugosc_m)
        {
            return dlugosc_m / zlecenieTowar.Dlugosc_m >= 1;
        }

        private bool WielokrotnoscSzerokoscie(decimal szerokosc_m)
        {
            return szerokosc_m / zlecenieTowar.Szerokosc_m >= 1;
        }
        private bool CzyRolkaNiezgodna(tblProdukcjaRuchTowar p)
        {
            return p.IDGramatura != zlecenieTowar.IDTowarGeowlokninaParametryGramatura
                                                                       || !WielokrotnoscDlugosci(p.Dlugosc_m)
                                                                       || p.Szerokosc_m > zlecenieTowar.Szerokosc_m * 1.05m
                                                                       || p.Szerokosc_m < zlecenieTowar.Szerokosc_m * 0.95m;
        }

        private PodsumowaniePWModel PodsumujPW(Func<tblProdukcjaRuchTowar, bool> predicate, bool czyKorygowacDlugoscIM2)
        {
            var pozycjeWybrane = new List<tblProdukcjaRuchTowar>( listaPW.Where(predicate).ToList());

            if (czyKorygowacDlugoscIM2)
                pozycjeWybrane = KorygujListeZeWzglNaDlugoscISzerokosc(pozycjeWybrane);

            return new PodsumowaniePWModel
            {
                IloscSzt = pozycjeWybrane.Count(),
                Ilosc_m2 = pozycjeWybrane.Sum(t => t.Ilosc_m2),
                Waga_kg =  pozycjeWybrane.Sum(t => t.Waga_kg),
                Odpad_kg = pozycjeWybrane.Sum(t => t.WagaOdpad_kg)

            };
        }

        private List<tblProdukcjaRuchTowar> KorygujListeZeWzglNaDlugoscISzerokosc(List<tblProdukcjaRuchTowar> pozycjeWybrane)
        {
            var lista = new List<tblProdukcjaRuchTowar>();

            foreach (var pozycja in pozycjeWybrane)
            {
                int krotnoscDlugosci = (int)(pozycja.Dlugosc_m / zlecenieTowar.Dlugosc_m);
                int krotnoscSzerokosci = (int)(pozycja.Szerokosc_m / zlecenieTowar.Szerokosc_m);

                pozycja.Dlugosc_m = krotnoscDlugosci * zlecenieTowar.Dlugosc_m;
                pozycja.Szerokosc_m = krotnoscSzerokosci * zlecenieTowar.Szerokosc_m;

                pozycja.Ilosc_m2 = pozycja.Szerokosc_m * pozycja.Dlugosc_m;
                //TODO odlozyc obliczanie wagi wg proporcji!!!
                lista.Add(pozycja);
            }

            return lista;
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

    }

}
