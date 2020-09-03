using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Magazyn.Ewidencje.SubiektGT.Ewidencja;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Magazyn.Ewidencje.SubiektGT
{
    [TestFixture]
    public class MagazynEwidencjaSubiektUCViewModelTests : TestBase
    {
        private MagazynEwidencjaSubiektUCViewModel sut;
        private Mock<IVwMagazynAGGRepository> vwMagazynAGG;
        private Mock<IVwMagazynGTXRepository> vwMagazynGTX;
        private Mock<IVwMagazynRuchAGGRepository> vwMagazynRuchAGG;
        private Mock<IVwMagazynRuchGTXRepository> vwMagazynRuchGTX;
        private Mock<IVwMagazynRuchGTX2Repository> vwMagazynRuchGTX2;
        private Mock<IVwMagazynGTX2Repository> vwMagazynGTX2;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            vwMagazynAGG = new Mock<IVwMagazynAGGRepository>();
            UnitOfWork.Setup(s => s.vwMagazynAGG).Returns(vwMagazynAGG.Object);

            vwMagazynGTX = new Mock<IVwMagazynGTXRepository>();
            UnitOfWork.Setup(s => s.vwMagazynGTX).Returns(vwMagazynGTX.Object);

            vwMagazynGTX2 = new Mock<IVwMagazynGTX2Repository>();
            UnitOfWork.Setup(s => s.vwMagazynGTX2).Returns(vwMagazynGTX2.Object);

            vwMagazynRuchAGG = new Mock<IVwMagazynRuchAGGRepository>();
            UnitOfWork.Setup(s => s.vwMagazynRuchAGG).Returns(vwMagazynRuchAGG.Object);

            vwMagazynRuchGTX = new Mock<IVwMagazynRuchGTXRepository>();
            UnitOfWork.Setup(s => s.vwMagazynRuchGTX).Returns(vwMagazynRuchGTX.Object);

            vwMagazynRuchGTX2 = new Mock<IVwMagazynRuchGTX2Repository>();
            UnitOfWork.Setup(s => s.vwMagazynRuchGTX2).Returns(vwMagazynRuchGTX2.Object);

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new MagazynEwidencjaSubiektUCViewModel(ViewModelService.Object);
        }

        #region Properties
        [Test]
        public void WybranaFirma_GdyWybranoFirme_PrzyporzadkujOdpowiedniaListeMagazynow()
        {
            vwMagazynAGG.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<vwMagazynAGG>
            {
                new vwMagazynAGG{IdMagazyn=1, Nazwa="AGG_1"},
                new vwMagazynAGG{IdMagazyn=2, Nazwa="AGG_2"}
            });
            vwMagazynGTX.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<vwMagazynGTX>
            {
                new vwMagazynGTX{IdMagazyn=3, Nazwa="GTX_1"},
                new vwMagazynGTX{IdMagazyn=4, Nazwa="GTX_2"}
            });
            sut.LoadAsync(null);

            sut.WybranaFirma = "GTEX";

            Assert.IsNotEmpty(sut.ListaMagazynow);
            Assert.AreEqual("GTX_1", sut.ListaMagazynow.First().Nazwa);
        }

        #endregion

        #region LoadAsync
        [Test]
        public void LoadCommandExecute_PobieraDaneZBazy()
        {
            sut.LoadAsync(null);

            vwMagazynAGG.Verify(v => v.GetAllAsync());
            vwMagazynGTX.Verify(v => v.GetAllAsync());
            vwMagazynGTX2.Verify(v => v.GetAllAsync());
            vwMagazynRuchAGG.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<vwMagazynRuchAGG, bool>>>()));
            vwMagazynRuchGTX.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<vwMagazynRuchGTX, bool>>>()));
            vwMagazynRuchGTX2.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<vwMagazynRuchGTX2, bool>>>()));

        }

        #endregion

        #region SzukajCommand

        private void GenerujListeTowarow()
        {
            vwMagazynRuchAGG.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<vwMagazynRuchAGG, bool>>>())).ReturnsAsync(new List<vwMagazynRuchAGG>
            {
                new vwMagazynRuchAGG{IdMagazyn=1, TowarNazwa="AGG1", Ilosc=1, Wartosc=10}
            });
            vwMagazynRuchGTX.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<vwMagazynRuchGTX, bool>>>())).ReturnsAsync(new List<vwMagazynRuchGTX>
            {
                new vwMagazynRuchGTX{IdMagazyn=2, TowarNazwa="GTX1", Ilosc=1, Wartosc=10}
            });
        }
        [Test]
        public void SzukajCommandExecute_GdyBrakWpisanychDanych_LaczWynikZDwochList()
        {
            GenerujListeTowarow();
            sut.LoadAsync(null);

            sut.SzukajCommand.Execute(null);

            Assert.AreEqual(2, sut.ListaTowarowNaMagazynie.Count());
        }
        [Test]
        public void SzukajCommandExecute_GdyWpisanoNazwe_FiltrujPoNazwie()
        {
            GenerujListeTowarow();
            sut.NazwaTowaru = "AGG1";
            sut.LoadAsync(null);

            sut.SzukajCommand.Execute(null);

            Assert.AreEqual(1, sut.ListaTowarowNaMagazynie.Count());
        }
        [Test]
        public void SzukajCommandExecute_GdyWybranoMagazyn_FiltrujPoMagazynie()
        {
            GenerujListeTowarow();
            sut.WybranyMagazyn = new vwMagazynAGG { IdMagazyn = 1 };
            sut.LoadAsync(null);

            sut.SzukajCommand.Execute(null);

            Assert.AreEqual(1, sut.ListaTowarowNaMagazynie.Count());
        }

        [Test]
        public void SzukajCommandExecute_GdyWybranoFirme_FiltrujPoFirmie()
        {
            GenerujListeTowarow();
            sut.WybranaFirma = "GTEX";
            sut.LoadAsync(null);

            sut.SzukajCommand.Execute(null);

            Assert.AreEqual(1, sut.ListaTowarowNaMagazynie.Count());
        }

        [Test]
        public void SzukajCommandExecute_GenerujPodsumowanie()
        {
            GenerujListeTowarow();
            sut.LoadAsync(null);

            sut.SzukajCommand.Execute(null);

            Assert.AreEqual(2, sut.ListaTowarowNaMagazynie.Sum(s=>s.Ilosc));
            Assert.AreEqual(20, sut.ListaTowarowNaMagazynie.Sum(s=>s.Wartosc));
        }
        #endregion
    }
}
