using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Helpers;
using Microsoft.Office.Interop.Outlook;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Helpers
{
    public class GPRuchTowar_PW_RolkaBazowaHelperTests : TestBaseGeneric<GPRuchTowar_PW_RolkaBazowaHelper>
    {
        private Mock<IGPRuchTowar_PW_Helper> pwHelper;
        private Mock<IGPRuchTowar_RolkaHelper> rolkaHelper;

        public override void SetUp()
        {
            base.SetUp();

            rolkaHelper = new Mock<IGPRuchTowar_RolkaHelper>();

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new GPRuchTowar_PW_RolkaBazowaHelper(rolkaHelper.Object);
        }


        #region PobierzDaneZeZlecenia

        [Test]
        public void PobierzDaneZeZlecenia_GdyRolkaBazowaNull_Wyjatek()
        {
            var zlecenieTowar = new tblProdukcjaZlecenieTowar
            {
                IDProdukcjaZlecenieCiecia = 1,
                IDProdukcjaZlecenieTowar = 1,
                IDTowarGeowlokninaParametryGramatura = 1,
                IDTowarGeowlokninaParametrySurowiec = 1,
                Szerokosc_m = 1,
                Dlugosc_m = 1,
                tblProdukcjaZlecenieCiecia = new tblProdukcjaZlecenieCiecia { NrZleceniaCiecia = 1 }
            };

            Assert.Throws<ArgumentException>(() => sut.PobierzDaneZeZlecenia(null, zlecenieTowar));
        }


        [Test]
        public void PobierzDaneZeZlecenia_GdyZlecenieTowarNull_ZwrocPrzekzaznaRolkeBazowa()
        {
            var rolkaBazowa = new tblProdukcjaRuchTowar { Dlugosc_m = 1 };

            rolkaBazowa = sut.PobierzDaneZeZlecenia(rolkaBazowa, null);

            Assert.AreEqual(1, rolkaBazowa.Dlugosc_m);
        }

        [Test]
        public void PobierzDaneZeZlecenia_GdyParametryNieSaNull_PrzpiszDaneWyjsciowe()
        {
            var rolkaBazowa = new tblProdukcjaRuchTowar();
            var zlecenieTowar = new tblProdukcjaZlecenieTowar
            {
                //IDProdukcjaZlecenieCiecia = 1,
                IDProdukcjaZlecenie=1,
                IDProdukcjaZlecenieTowar = 1,
                IDTowarGeowlokninaParametryGramatura = 1,
                IDTowarGeowlokninaParametrySurowiec = 1,
                Szerokosc_m = 1,
                Dlugosc_m = 1,
                tblProdukcjaZlecenie = new tblProdukcjaZlecenie { NrZlecenia=1}
            };

            rolkaBazowa = sut.PobierzDaneZeZlecenia(rolkaBazowa, zlecenieTowar);

            Assert.AreEqual(1, rolkaBazowa.Szerokosc_m);
            Assert.AreEqual(1, rolkaBazowa.Dlugosc_m);
            Assert.AreEqual(1, rolkaBazowa.IDProdukcjaZlecenieTowar);
            Assert.AreEqual(1, rolkaBazowa.NrZlecenia);
        }

        #endregion

        #region PobierzDaneZRolkiRw

        [Test]
        public void PobierzDaneZRolkiRw_GdyRolkaBazowaNull_Wyjatek()
        {
            var rolkaRW = new tblProdukcjaRuchTowar { NrRolki = 1, CzyKalandrowana = true };
            var gniazdoProdukcyjne = new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = 1 };

            Assert.Throws<ArgumentException>(() => sut.PobierzDaneZRolkiRw(null, rolkaRW, gniazdoProdukcyjne));
        }

        [Test]
        public async Task PobierzDaneZRolkiRw_GdyRolkaRWNull_ZwrocTaSamaRolkeBazowa()
        {
            var rolkaBazowa = new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1 };
            var gniazdoProdukcyjne = new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = 1 };
            rolkaHelper.Setup(s => s.PobierzIDRolkiBazowejAsync(It.IsAny<tblProdukcjaRuchTowar>(), It.IsAny<GniazdaProdukcyjneEnum>())).ReturnsAsync(1);

            rolkaBazowa = sut.PobierzDaneZRolkiRw(rolkaBazowa, null, gniazdoProdukcyjne); 

            Assert.AreEqual(1, rolkaBazowa.IDProdukcjaRuchTowar);
        }

        [Test]
        public async Task PobierzDaneZRolkiRw_GdyGniazdoProdukcyjneNull_ZwrocRolkeBazowaBezIdRolkiBazowej()
        {
            var rolkaBazowa = new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1 };
            var rolkaRW = new tblProdukcjaRuchTowar { NrRolki = 1, CzyKalandrowana = true, NrRolkiPelny="1KL1KO1"};
            rolkaHelper.Setup(s => s.PobierzIDRolkiBazowejAsync(It.IsAny<tblProdukcjaRuchTowar>(), It.IsAny<GniazdaProdukcyjneEnum>())).ReturnsAsync(1);

            rolkaBazowa = sut.PobierzDaneZRolkiRw(rolkaBazowa, rolkaRW, null);

            Assert.AreEqual(null, rolkaBazowa.IDRolkaBazowa);

            Assert.AreEqual("1KL1KO1", rolkaBazowa.NrRolkiBazowej);
            Assert.IsTrue(rolkaBazowa.CzyKalandrowana);
        }

        [Test]
        public async Task PobierzDaneZRolkiRw_DlaLiniaWlokin_NiePrzypisujIdRolkiBazowej()
        {
            var rolkaBazowa = new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 2, };
            var rolkaRW = new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1, NrRolki = 1, CzyKalandrowana = true, NrRolkiPelny = "1KL1KO1", IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaWloknin };
            var gniazdoProdukcyjne = new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaWloknin };

            rolkaBazowa = sut.PobierzDaneZRolkiRw(rolkaBazowa, rolkaRW, null);

            Assert.AreEqual(null, rolkaBazowa.IDRolkaBazowa);
        }

        [Test]
        public async Task PobierzDaneZRolkiRw_GdyGniazdoNieJestLiniaWlokin_PrzypiszIdRolkiBazowej()
        {
            var rolkaBazowa = new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 2, };
            var rolkaRW = new tblProdukcjaRuchTowar {IDProdukcjaRuchTowar=1, NrRolki = 1, CzyKalandrowana = true, NrRolkiPelny = "1KL1KO1", IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKalandowania };
            var gniazdoProdukcyjne = new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji };

            rolkaBazowa = sut.PobierzDaneZRolkiRw(rolkaBazowa, rolkaRW, gniazdoProdukcyjne);

            Assert.AreEqual(1, rolkaBazowa.IDRolkaBazowa);
        }


        [Test]
        public async Task PobierzDaneZRolkiRw_GdyParametryNieNull_UzupelnijDane()
        {
            var rolkaBazowa = new tblProdukcjaRuchTowar();
            var rolkaRW = new tblProdukcjaRuchTowar {IDProdukcjaRuchTowar=1, CzyKalandrowana = true, NrRolkiPelny="1KL1KO2" };
            var gniazdoProdukcyjne = new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = 1 };
            rolkaHelper.Setup(s => s.PobierzIDRolkiBazowejAsync(It.IsAny<tblProdukcjaRuchTowar>(), It.IsAny<GniazdaProdukcyjneEnum>())).ReturnsAsync(1);

            rolkaBazowa = sut.PobierzDaneZRolkiRw(rolkaBazowa, rolkaRW, gniazdoProdukcyjne);

            Assert.AreEqual("1KL1KO2", rolkaBazowa.NrRolkiBazowej);
            Assert.IsTrue(rolkaBazowa.CzyKalandrowana);
        }



        #endregion

        #region PobierzDaneZGniazdaProdukcyjnego
        [Test]
        public void PobierzDaneZGniazdaProdukcyjnego_RolkaBazowaJestNull_Wyjatek()
        {
            var gniazdoProdukcyjne = new tblProdukcjaGniazdoProdukcyjne();
            var listaPW = new List<tblProdukcjaRuchTowar>();

            Assert.ThrowsAsync<ArgumentException>(() => sut.PobierzNoweNryDlaRolki(null, gniazdoProdukcyjne, listaPW));
        }
        [Test]
        public async Task PobierzDaneZGniazdaProdukcyjnego_GniazdoProdukcyjneJestNull_ZwracaTaSamaRolkeBazowa()
        {
            var rolkaBazowa = new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1 };
            var gniazdoProdukcyjne = new tblProdukcjaGniazdoProdukcyjne();
            var listaPW = new List<tblProdukcjaRuchTowar>();

            rolkaBazowa = await sut.PobierzNoweNryDlaRolki(rolkaBazowa, null, listaPW);

            Assert.AreEqual(1, rolkaBazowa.IDProdukcjaRuchTowar);
            Assert.IsNull(rolkaBazowa.NrRolkiPelny);
        }

        [Test]
        public async Task PobierzDaneZGniazdaProdukcyjnego_ArgOk_WywolujeMetodyDlaRolkaHelper()
        {
            var rolkaBazowa = new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1 };
            var gniazdoProdukcyjne = new tblProdukcjaGniazdoProdukcyjne();
            var listaPW = new List<tblProdukcjaRuchTowar>();

            rolkaBazowa = await sut.PobierzNoweNryDlaRolki(rolkaBazowa, gniazdoProdukcyjne, listaPW);

            rolkaHelper.Verify(v => v.PobierzKolejnyPelnyNrRolkiAsync(It.IsAny<tblProdukcjaGniazdoProdukcyjne>(), It.IsAny<tblProdukcjaRuchTowar>(), It.IsAny<IEnumerable<tblProdukcjaRuchTowar>>()));
            rolkaHelper.Verify(v => v.PobierzKolejnyNrRolkiAsync(It.IsAny<tblProdukcjaGniazdoProdukcyjne>(), It.IsAny<IEnumerable<tblProdukcjaRuchTowar>>()));
        }
        #endregion
    }
}
