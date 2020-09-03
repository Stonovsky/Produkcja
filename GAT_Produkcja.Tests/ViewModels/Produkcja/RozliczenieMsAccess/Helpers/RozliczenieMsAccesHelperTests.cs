using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.dbMsAccess.Models.Adapters;
using GAT_Produkcja.dbMsAccess.Repositories;
using GAT_Produkcja.dbMsAccess.UnitOfWork;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.Tests.ViewModels.Produkcja.RozliczenieMsAccess.Modele;
using GAT_Produkcja.Utilities.EppFile;
using GAT_Produkcja.Utilities.ExcelReport;
using GAT_Produkcja.Utilities.Wrappers;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.PW;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.RW;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.Strategy;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.Strategy.Factory;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Modele;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.RozliczenieMsAccess.Helpers
{
    [TestFixture]
    public class RozliczenieMsAccesHelperTests : TestBase
    {
        private RozliczenieMsAccesHelper sut;
        private Mock<IXlsService> excelReportGenerator;
        private Mock<IEppFileGenerator> eppFileGenerator;
        private Mock<IUnitOfWorkMsAccess> unitOfWorkMsAccess;
        private Mock<IVwMagazynRuchGTXRepository> vwMagazynRuchGTX;
        private Mock<RozliczenieMsAccesHelper> sutMock;
        private Mock<IRozliczenieSQL_RW_Helper> rwHelper;
        private Mock<IRozliczenieSQL_PW_Helper> pwHelper;
        private Mock<IDirectoryHelper> directoryHelper;
        private Mock<ISurowiecSubiektStrategyFactory> surowiecSubiektStrategyFactory;
        private Mock<ISurowiecSubiektStrategy> surowiecStrategy;
        private Mock<ITblProdukcjaRozliczenie_CenyTransferoweRepository> tblProdukcjaRozliczenie_CenyTransferowe;
        private Mock<ISurowiecRepository> surowiec;
        private Mock<IKonfekcjaRepository> konfekcja;
        private Mock<INormyZuzyciaRepository> normyZuzycia;
        private Mock<Func<SurowiecSubiektFactoryEnum, ISurowiecSubiektStrategy>> funcMock;

        public override void SetUp()
        {
            base.SetUp();

            excelReportGenerator = new Mock<IXlsService>();
            eppFileGenerator = new Mock<IEppFileGenerator>();
            unitOfWorkMsAccess = new Mock<IUnitOfWorkMsAccess>();

            rwHelper = new Mock<IRozliczenieSQL_RW_Helper>();
            pwHelper = new Mock<IRozliczenieSQL_PW_Helper>();

            directoryHelper = new Mock<IDirectoryHelper>();

            //surowiecSubiektStrategyFactory = new Mock<ISurowiecSubiektStrategyFactory>();
            //surowiecStrategy = new Mock<ISurowiecSubiektStrategy>();

            //surowiecSubiektStrategyFactory.Setup(s => s.PobierzStrategie(It.IsAny<SurowiecSubiektFactoryEnum>())).Returns(surowiecStrategy.Object);



            vwMagazynRuchGTX = new Mock<IVwMagazynRuchGTXRepository>();
            UnitOfWork.Setup(s => s.vwMagazynRuchGTX).Returns(vwMagazynRuchGTX.Object);

            tblProdukcjaRozliczenie_CenyTransferowe = new Mock<ITblProdukcjaRozliczenie_CenyTransferoweRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaRozliczenie_CenyTransferowe).Returns(tblProdukcjaRozliczenie_CenyTransferowe.Object);

            surowiec = new Mock<ISurowiecRepository>();
            unitOfWorkMsAccess.Setup(s => s.Surowiec).Returns(surowiec.Object);

            konfekcja = new Mock<IKonfekcjaRepository>();
            unitOfWorkMsAccess.Setup(s => s.Konfekcja).Returns(konfekcja.Object);

            normyZuzycia = new Mock<INormyZuzyciaRepository>();
            unitOfWorkMsAccess.Setup(s => s.NormyZuzycia).Returns(normyZuzycia.Object);


            sutMock = new Mock<RozliczenieMsAccesHelper>(UnitOfWork.Object,
                                                        unitOfWorkMsAccess.Object,
                                                        excelReportGenerator.Object,
                                                        eppFileGenerator.Object,
                                                        rwHelper.Object,
                                                        pwHelper.Object);
            //sutMock.Setup(s => s.GenerujNazweTowaru(It.IsAny<Konfekcja>())).Returns("test");
            //sutMock.Setup(s => s.GenerujSymbolTowaru(It.IsAny<Konfekcja>())).Returns("test");
            //sutMock.Setup(s => s.PobierzIdSurowcaZNazwyMsAccess(It.IsAny<string>())).Returns(1);

            //funcMock = new Mock<Func<SurowiecSubiektFactoryEnum, ISurowiecSubiektStrategy>>();

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new RozliczenieMsAccesHelper(UnitOfWork.Object,
                                               unitOfWorkMsAccess.Object,
                                               excelReportGenerator.Object,
                                               eppFileGenerator.Object,
                                               rwHelper.Object,
                                               pwHelper.Object,
                                               directoryHelper.Object
                                               );
        }

        #region LoadAsync
        [Test]
        public void LoadAsync_LadujeOdpowiednieDane()
        {
            sut.LoadAsync();

            rwHelper.Verify(v => v.LoadAsync());
            pwHelper.Verify(v => v.LoadAsync());
        }

        #endregion

        #region PobierzListeKonfekcjiDlaZlecenia

        [Test]
        public void PobierzListeKonfekcjiDlaZlecenia_JezeliZlecenieZero_Wyjatek()
        {
            Assert.ThrowsAsync<ArgumentException>(() => sut.PobierzListeKonfekcjiDlaZlecenia(0));
        }
        [Test]
        public async Task PobierzListeKonfekcjiDlaZlecenia_GdyPrzeslanoIdZlecenia_PobierzListe()
        {
            //sut = sutMock.Object;
            konfekcja.Setup(s => s.GetByZlecenieIdAsync(It.IsAny<int>())).ReturnsAsync(new List<Konfekcja>
             {
                 new Konfekcja{Przychody="Magazyn",CzyZaksiegowano=false,NrWz="1"},
                 new Konfekcja{Przychody="Linia",CzyZaksiegowano=false,NrWz="1", Szerokosc=1, Dlugosc=50},
                 new Konfekcja{Przychody="Linia",CzyZaksiegowano=true,NrWz="1"},
                 new Konfekcja{Przychody="Linia",CzyZaksiegowano=false,NrWz="0"},
             });

            var lista = await sut.PobierzListeKonfekcjiDlaZlecenia(1);

            Assert.AreEqual(1, lista.Count());
        }
        #endregion


    }
}
