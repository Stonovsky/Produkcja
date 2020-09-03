using GAT_Produkcja.db;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Helpers
{
    public class GPRuchTowar_PW_PodsumowanieHelperTests : TestBase
    {
        private GPRuchTowar_PW_PodsumowanieHelper sut;

        public override void SetUp()
        {
            base.SetUp();

            CreateSut();
            Init();
        }
        public override void CreateSut()
        {
            sut = new GPRuchTowar_PW_PodsumowanieHelper();
        }

        private void Init()
        {
            var listaPW = new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{ IDProdukcjaRuchTowar=1, Ilosc_m2=1, Waga_kg=1, IDGramatura=1, Szerokosc_m=1, Dlugosc_m = 50},
                new tblProdukcjaRuchTowar{ IDProdukcjaRuchTowar=1, Ilosc_m2=1, Waga_kg=1, IDGramatura=2, Szerokosc_m=1, Dlugosc_m = 50},
                new tblProdukcjaRuchTowar{ IDProdukcjaRuchTowar=2, Ilosc_m2=1, Waga_kg=1, IDGramatura=1, Szerokosc_m=2, Dlugosc_m=50},
                new tblProdukcjaRuchTowar{ IDProdukcjaRuchTowar=3, Ilosc_m2=1, Waga_kg=1, IDGramatura=1, Szerokosc_m=1, Dlugosc_m=100},

            };

            var zlecenieTowar = new tblProdukcjaZlecenieTowar { IDTowarGeowlokninaParametryGramatura = 1, Szerokosc_m = 1, Dlugosc_m = 50, Ilosc_m2=5, Ilosc_szt=5 };

            sut.Init(listaPW, zlecenieTowar);
        }

        [Test]
        public void PodsumowanieRolekKwalifikowanych_GenerujePodsumowanie()
        {
            Init();

            var podsumowanie = sut.PodsumowanieRolekKwalifikowanych();

            Assert.AreEqual(1, podsumowanie.IloscSzt);
            Assert.AreEqual(1, podsumowanie.Ilosc_m2);

        }

        [Test]
        public void PodsumowanieRolekNieKwalifikowanych_GenerujePodsumowanie()
        {
            Init();

            var podsumowanie = sut.PodsumowanieRolekNieKwalifikowanych();

            Assert.AreEqual(3, podsumowanie.IloscSzt);
            Assert.AreEqual(3, podsumowanie.Ilosc_m2);

        }
        [Test]
        public void Pozostalo_GenerujeModel()
        {
            Init();

            var podsumowanie = sut.PodsumowaniePozostalo();

            Assert.AreEqual(4, podsumowanie.IloscSzt);
            Assert.AreEqual(4, podsumowanie.Ilosc_m2);

        }

    }
}
