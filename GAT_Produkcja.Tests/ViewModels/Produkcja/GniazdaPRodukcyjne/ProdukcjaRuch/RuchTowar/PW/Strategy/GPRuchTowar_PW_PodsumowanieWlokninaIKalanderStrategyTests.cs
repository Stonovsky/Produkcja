using GAT_Produkcja.db;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Strategy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Helpers
{
    class GPRuchTowar_PW_PodsumowanieWlokninaIKalanderStrategyTests : TestBaseGeneric<GPRuchTowar_PW_PodsumowanieWlokninaIKalanderStrategy>
    {
        public override void SetUp()
        {
            base.SetUp();

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new GPRuchTowar_PW_PodsumowanieWlokninaIKalanderStrategy();
        }

        #region Rolki kwalifikowane

        [Test]
        public void PodsumowanieKwalifikowane_GdyRolkaTrocheDluzszaAleNieWielokrotnie_LiczyDlugoscIm2ZKrotszejWielokrotnosci()
        {
            var zlecenie = new tblProdukcjaZlecenieTowar { IDTowarGeowlokninaParametryGramatura = 1, Dlugosc_m = 1, Szerokosc_m = 1 };
            var listaPw = new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDGramatura=1,Szerokosc_m=1,Dlugosc_m=1.5m, Ilosc_m2=1.5m},
            };
            sut.Init(listaPw, zlecenie);

            var podsumowanie = sut.PodsumowanieRolekKwalifikowanych();

            Assert.AreEqual(1, podsumowanie.IloscSzt);
            Assert.AreEqual(1, podsumowanie.Ilosc_m2);
        }

        [Test]
        public void MethodName_Condition_Expectations()
        {

        }

        [Test]
        public void PodsumowanieKwalifikowane_GdySzerokoscWielkszaAleNieWielokrotnosc_LiczySzerokoscZMniejszejWielokrotnosci()
        {
            var zlecenie = new tblProdukcjaZlecenieTowar { IDTowarGeowlokninaParametryGramatura = 1, Dlugosc_m = 1, Szerokosc_m = 1 };
            var listaPw = new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDGramatura=1,Szerokosc_m=1.2m,Dlugosc_m=1m, Ilosc_m2=1.2m},
            };
            sut.Init(listaPw, zlecenie);

            var podsumowanie = sut.PodsumowanieRolekKwalifikowanych();

            Assert.AreEqual(1, podsumowanie.IloscSzt);
            Assert.AreEqual(1, podsumowanie.Ilosc_m2);
        }


        [Test]
        public void PodsumowanieKwalifikowane_GdyOkLiczyPodsumowanie()
        {
            var zlecenie = new tblProdukcjaZlecenieTowar { IDTowarGeowlokninaParametryGramatura = 1, Dlugosc_m = 1, Szerokosc_m = 1 };
            var listaPw = new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDGramatura=1,Szerokosc_m=1,Dlugosc_m=1, Ilosc_m2=1},
                new tblProdukcjaRuchTowar{IDGramatura=1,Szerokosc_m=1,Dlugosc_m=1.5m, Ilosc_m2=1.5m},
                new tblProdukcjaRuchTowar{IDGramatura=1,Szerokosc_m=1,Dlugosc_m=2, Ilosc_m2=2}
            };
            sut.Init(listaPw, zlecenie);

            var podsumowanie = sut.PodsumowanieRolekKwalifikowanych();

            Assert.AreEqual(3, podsumowanie.IloscSzt);
            Assert.AreEqual(4, podsumowanie.Ilosc_m2);
        }
        #endregion

        #region Rolki niekwalifikowane
        [Test]
        public void PodsumowanieNieKwalifikowane_GdyBrakListyZRolkamiWlasciwymi_ZwracaPodsumowanieNiekwalifikowane()
        {
            var zlecenie = new tblProdukcjaZlecenieTowar { IDTowarGeowlokninaParametryGramatura = 1, Dlugosc_m = 3, Szerokosc_m = 1 };
            var listaPw = new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDGramatura=1,Szerokosc_m=1,Dlugosc_m=1, Ilosc_m2=1},
                new tblProdukcjaRuchTowar{IDGramatura=1,Szerokosc_m=1,Dlugosc_m=1.5m, Ilosc_m2=1.5m},
                new tblProdukcjaRuchTowar{IDGramatura=1,Szerokosc_m=1,Dlugosc_m=2, Ilosc_m2=2}
            };
            sut.Init(listaPw, zlecenie);

            var podsumowanie = sut.PodsumowanieRolekNieKwalifikowanych();

            Assert.AreEqual(3, podsumowanie.IloscSzt);
            Assert.AreEqual(4.5m, podsumowanie.Ilosc_m2);
        }

        #endregion
    }
}
