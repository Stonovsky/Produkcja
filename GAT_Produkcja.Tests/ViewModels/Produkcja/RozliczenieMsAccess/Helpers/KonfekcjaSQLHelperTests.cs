using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.Strategy;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.Strategy.Factory;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.RozliczenieMsAccess.Helpers
{
    public class KonfekcjaSQLHelperTests : TestBaseGeneric<KonfekcjaHelper>
    {
        private Mock<ISurowiecSubiektStrategyFactory> surowiecStrategyFactory;
        private Mock<ISurowiecSubiektZNazwyMsAccessStrategy> surowiecStrategy;
        private Mock<ITblProdukcjaRuchTowarRepository> tblProdukcjaRuchTowar;
        private Mock<ITblProdukcjaZlecenieProdukcyjne_MieszankaRepository> tblProdukcjaZlecenieProdukcyjne_Mieszanka;
        private Mock<IVwTowarGTXRepository> vwTowarGTX;
        private Mock<IVwMagazynRuchGTXRepository> vwMagazynRuchGTX;

        public override void SetUp()
        {
            base.SetUp();
            
            surowiecStrategyFactory = new Mock<ISurowiecSubiektStrategyFactory>();
            surowiecStrategy = new Mock<ISurowiecSubiektZNazwyMsAccessStrategy>();
            surowiecStrategyFactory.Setup(s => s.PobierzStrategie(It.IsAny<SurowiecSubiektFactoryEnum>())).Returns(surowiecStrategy.Object);

            tblProdukcjaRuchTowar = new Mock<ITblProdukcjaRuchTowarRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaRuchTowar).Returns(tblProdukcjaRuchTowar.Object);

            tblProdukcjaZlecenieProdukcyjne_Mieszanka = new Mock<ITblProdukcjaZlecenieProdukcyjne_MieszankaRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaZlecenieProdukcyjne_Mieszanka).Returns(tblProdukcjaZlecenieProdukcyjne_Mieszanka.Object);

            vwTowarGTX = new Mock<IVwTowarGTXRepository>();
            UnitOfWork.Setup(s => s.vwTowarGTX).Returns(vwTowarGTX.Object);

            vwMagazynRuchGTX = new Mock<IVwMagazynRuchGTXRepository>();
            UnitOfWork.Setup(s => s.vwMagazynRuchGTX).Returns(vwMagazynRuchGTX.Object);
            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new KonfekcjaHelper(UnitOfWork.Object,surowiecStrategyFactory.Object);
        }

        [Test]
        public void PobierzKonfekcjeDoRozliczenia_GdyEncjeWBazie_ZwracaEncje()
        {
            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()));
        }

        [Test]
        public void PobierzKonfekcjeDoRozliczenia_GdyBrakEncjiWBazie_ZwracaPustaKolekcje()
        {

        }

        [Test]
        public void GenerujRozliczenieRWAsync_GdyBrakPozycjiKonfekcji_Wyjatek()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => sut.GenerujRozliczenieRWAsync(null));
        }

        [Test]
        public void GenerurRozliczenieRWAsync_Condition_Expectations()
        {

        }

        [Test]
        public void PobierzCeneDlaZleceniaProdukcyjnego_GdyIdZero_Wyjatek()
        {
            Assert.ThrowsAsync<ArgumentException>(() => sut.PobierzCeneMieszankiDlaZleceniaProdukcji(0));
        }

        [Test]
        public async Task PobierzCeneDlaZleceniaProdukcyjnego_GdyIdWiekszeOdZera_LiczyCene()
        {
            tblProdukcjaZlecenieProdukcyjne_Mieszanka.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieProdukcyjne_Mieszanka, bool>>>())).ReturnsAsync(new List<tblProdukcjaZlecenieProdukcyjne_Mieszanka>
            {
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka{IDZlecenieProdukcyjneMieszanka=1, IDProdukcjaZlecenieProdukcyjne=1, IloscKg=1,ZawartoscProcentowa=0.5m, CenaMieszanki_kg=0, Cena_kg=1},
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka{IDZlecenieProdukcyjneMieszanka=2, IDProdukcjaZlecenieProdukcyjne=1, IloscKg=1,ZawartoscProcentowa=0.5m, CenaMieszanki_kg=0, Cena_kg=2},
            });
            vwMagazynRuchGTX.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<vwMagazynRuchGTX, bool>>>())).ReturnsAsync(new List<vwMagazynRuchGTX>
            {
                new vwMagazynRuchGTX{IdTowar=1, Cena=1},
                new vwMagazynRuchGTX{IdTowar=1, Cena=2}
            });
            var cena = await sut.PobierzCeneMieszankiDlaZleceniaProdukcji(1);

            Assert.AreEqual(1, cena);
        }
    }
}
