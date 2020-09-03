using GAT_Produkcja.db;
using GAT_Produkcja.db.EntitesInterfaces;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Finanse;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Finanse
{
    [TestFixture]
    public class FinanseZobowiazaniaNaleznosciViewModelTests : TestBase
    {
        private FinanseZobowiazaniaNaleznosciViewModel sut;
        private Mock<IVwFinanseNalZobAGGRepository> vwFinanseNalZobAGG;
        private Mock<IVwFinanseNalZobGTXRepository> vwFinanseNalZobGTX;
        private Mock<ITblFirmaRepository> tblFirma;
        private Mock<IVwFinanseNalZobGTX2Repository> vwFinanseNalZobGTX2;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            vwFinanseNalZobAGG = new Mock<IVwFinanseNalZobAGGRepository>();
            UnitOfWork.Setup(s => s.vwFinanseNalZobAGG).Returns(vwFinanseNalZobAGG.Object);

            vwFinanseNalZobGTX = new Mock<IVwFinanseNalZobGTXRepository>();
            UnitOfWork.Setup(s => s.vwFinanseNalZobGTX).Returns(vwFinanseNalZobGTX.Object);

            vwFinanseNalZobGTX2 = new Mock<IVwFinanseNalZobGTX2Repository>();
            UnitOfWork.Setup(s => s.vwFinanseNalZobGTX2).Returns(vwFinanseNalZobGTX2.Object);

            tblFirma = new Mock<ITblFirmaRepository>();
            UnitOfWork.Setup(s => s.tblFirma).Returns(tblFirma.Object);

            CreateSut();
        }

        public override void CreateSut()
        {
            sut = new FinanseZobowiazaniaNaleznosciViewModel(ViewModelService.Object);
        }

        #region LoadCommand
        [Test]
        public void LoadCommandExecute_PobieraZBazyListy()
        {
            sut.LoadCommand.Execute(null);

            vwFinanseNalZobAGG.Verify(v => v.GetAllAsync());
            vwFinanseNalZobGTX.Verify(v => v.GetAllAsync());
            tblFirma.Verify(v => v.GetAllAsync());
        }

        [Test]
        public void LoadCommandExecute_LadujeTylkoDwieFirmyAGGiGTEX()
        {
            tblFirma.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<tblFirma>
            {
                new tblFirma{Nazwa="Grupa Alians Trade"},
                new tblFirma{Nazwa="EMG Plast"},
                new tblFirma{Nazwa="AG Geosynthetics"},
                new tblFirma{Nazwa="GTEX"},
            });

            sut.LoadCommand.Execute(null);

            Assert.AreEqual(2, sut.ListaFirm.Count());
        }

        [Test]
        public void LoadCommandExecute_ListaZobINal_LaczyTabele()
        {
            vwFinanseNalZobAGG.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<vwFinanseNalZobAGG>
            {
                new vwFinanseNalZobAGG{Id=1}
            });
            vwFinanseNalZobGTX.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<vwFinanseNalZobGTX>
            {
                new vwFinanseNalZobGTX {Id=1}
            }) ;

            sut.LoadCommand.Execute(null);

            //Assert.AreEqual(2, sut.ListaZobowiazanINaleznosci.Count());
        }

        #endregion

        #region SzukajCommand
        [Test]
        public void SzukajCommandExecute_GenerujePodsumowanie()
        {
            vwFinanseNalZobAGG.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<vwFinanseNalZobAGG>
            {
                new vwFinanseNalZobAGG{Naleznosc=1, TerminPlatnosci=DateTime.Now.Date.AddDays(-1)},
                new vwFinanseNalZobAGG{Naleznosc=1, TerminPlatnosci=DateTime.Now.Date.AddDays(-1)},
                new vwFinanseNalZobAGG{Zobowiazanie=1, TerminPlatnosci=DateTime.Now.Date.AddDays(-1)},

            });            
            vwFinanseNalZobGTX.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<vwFinanseNalZobGTX>
            {
                new vwFinanseNalZobGTX{Naleznosc=1, TerminPlatnosci=DateTime.Now.Date.AddDays(-1)},
                new vwFinanseNalZobGTX{Naleznosc=1, TerminPlatnosci=DateTime.Now.Date.AddDays(-1)},
                new vwFinanseNalZobGTX{Zobowiazanie=1, TerminPlatnosci=DateTime.Now.Date.AddDays(-1)},
            });
            vwFinanseNalZobGTX2.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<vwFinanseNalZobGTX2>
            {
                new vwFinanseNalZobGTX2{Naleznosc=1, TerminPlatnosci=DateTime.Now.Date.AddDays(-1)},
                new vwFinanseNalZobGTX2{Naleznosc=1, TerminPlatnosci=DateTime.Now.Date.AddDays(-1)},
                new vwFinanseNalZobGTX2{Zobowiazanie=1, TerminPlatnosci=DateTime.Now.Date.AddDays(-1)},
            });
            sut.LoadCommand.Execute(null);

            sut.SzukajCommand.Execute(null);

            Assert.AreEqual(6, sut.NaleznosciSuma);
            Assert.AreEqual(3, sut.ZobowiazaniaSuma);
        }
        #endregion

    }

}
