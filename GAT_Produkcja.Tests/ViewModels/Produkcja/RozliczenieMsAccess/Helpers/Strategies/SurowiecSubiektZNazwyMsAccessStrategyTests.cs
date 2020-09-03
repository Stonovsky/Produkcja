using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.Strategy;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.RozliczenieMsAccess.Helpers.Strategies
{
    public class SurowiecSubiektZNazwyMsAccessStrategyTests : TestBase
    {
        private SurowiecSubiektZNazwyMsAccessStrategy sut;
        private Mock<IVwMagazynRuchGTXRepository> vwMagazynRuchGTX;

        public override void SetUp()
        {
            base.SetUp();

            vwMagazynRuchGTX = new Mock<IVwMagazynRuchGTXRepository>();
            UnitOfWork.Setup(s => s.vwMagazynRuchGTX).Returns(vwMagazynRuchGTX.Object);

            CreateSut();
        }

        public override void CreateSut()
        {
            sut = new SurowiecSubiektZNazwyMsAccessStrategy(UnitOfWork.Object);
        }

        private void GenerujListeSurowcowSubiekt()
        {
            vwMagazynRuchGTX.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<vwMagazynRuchGTX, bool>>>())).ReturnsAsync(new List<vwMagazynRuchGTX>
            {
                new vwMagazynRuchGTX(){IdTowar=1, TowarSymbol= "PES_3,3/66", Pozostalo=1},
                new vwMagazynRuchGTX(){IdTowar=2, TowarSymbol= "PES_3/64", Pozostalo=0},
                new vwMagazynRuchGTX(){IdTowar=3, TowarSymbol= "PES_6/64", Pozostalo=1},
                new vwMagazynRuchGTX(){IdTowar=4, TowarSymbol= "PES_6/66", Pozostalo=5},
                new vwMagazynRuchGTX(){IdTowar=5, TowarSymbol= "PES_4/51", Pozostalo=1},
                new vwMagazynRuchGTX(){IdTowar=6, TowarSymbol= "SUROWIEC_PES_TASMY", Pozostalo=1},
                new vwMagazynRuchGTX(){IdTowar=7, TowarSymbol= "SUROWIEC_PP_TASMY", Pozostalo=1},
                new vwMagazynRuchGTX(){IdTowar=8, TowarSymbol= "TASMY_PES", Pozostalo=1},
                new vwMagazynRuchGTX(){IdTowar=9, TowarSymbol= "TASMY_PP", Pozostalo=1},
            });
        }

        [Test]
        [TestCase("BICO 4/51 biały", 0, 5)]
        [TestCase("PES 3/64 biały", 0, 1)]
        [TestCase("PES 6/64 biały", 0, 3)]
        [TestCase("PES 6/64 biały", 2, 4)]
        [TestCase("Szarpanka PES", 0, 6)]
        [TestCase("Szarpanka PP", 0, 7)]
        public async Task PobierzIdSurowcaZSubiektDla_SurowiecMsAccessJestWlasciwy_ZwracaId(string nazwaSurowcaMsAccess, decimal ilosc, int idSurowcaSubiekt)
        {
            GenerujListeSurowcowSubiekt();

            var surowiecId = await sut.PobierzIdSurowcaZSubiektDla(nazwaSurowcaMsAccess,ilosc);

            Assert.AreEqual(idSurowcaSubiekt, surowiecId);
        }

        [Test]
        [TestCase("BICO 4/51 biały")]
        public void PobierzSurowiecZSubiektDla_GdyBrakSurowcaSpelniajacegoKryteria_RzucWyjatkiem(string nazwaSurowcaMsAccess)
        {
            vwMagazynRuchGTX.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<vwMagazynRuchGTX, bool>>>())).ReturnsAsync(new List<vwMagazynRuchGTX>
            {
                new vwMagazynRuchGTX(){IdTowar=6, TowarSymbol= "SUROWIEC_PES_TASMY", Pozostalo=1},
            });

            Assert.ThrowsAsync<ArgumentException>(() => sut.PobierzSurowiecZSubiektDla(nazwaSurowcaMsAccess));
        }

        [Test]
        [TestCase("BICO 4/51 biały")]
        public async Task PobierzSurowiecZSubiektDla_GdySurowiecSpelniajaKryteria_ZwrocSurowiec(string nazwaSurowcaMsAccess)
        {
            vwMagazynRuchGTX.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<vwMagazynRuchGTX, bool>>>())).ReturnsAsync(new List<vwMagazynRuchGTX>
            {
                new vwMagazynRuchGTX(){IdTowar=6, TowarSymbol= "PES_4/51", Pozostalo=1},
            });

            var surowiec = await sut.PobierzSurowiecZSubiektDla(nazwaSurowcaMsAccess);

            Assert.IsNotNull(surowiec);
        }
        [Test]
        [TestCase("Szarpanka PES")]
        [TestCase("Szarpanka PP")]
        public async Task PobierzSurowiecZSubiektDla_GdySzarpanka_ZwrocTasmy(string nazwaSurowcaMsAccess)
        {
            vwMagazynRuchGTX.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<vwMagazynRuchGTX, bool>>>())).ReturnsAsync(new List<vwMagazynRuchGTX>
            {
                new vwMagazynRuchGTX(){IdTowar=6, TowarSymbol= "TASMY_PES", Pozostalo=1},
                new vwMagazynRuchGTX(){IdTowar=6, TowarSymbol= "TASMY_PP", Pozostalo=1},
            });

            var surowiec = await sut.PobierzSurowiecZSubiektDla(nazwaSurowcaMsAccess);

            Assert.IsNotNull(surowiec);
        }
    }
}
