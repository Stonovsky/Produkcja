using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.UI.Utilities.Dostep;
using GAT_Produkcja.Utilities.Printers;
using GAT_Produkcja.ViewModel.Konfiguracja.KonfiguracjaUrzadzen;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Konfiguracja.DrukarkaEtykiet
{
    class KonfiguracjaUrzadzenViewModelTests : TestBaseGeneric<KonfiguracjaUrzadzenViewModel>
    {
        private Mock<IPrinterService> printerService;
        private Mock<ITblKonfiguracjaUrzadzenRepository> tblKonfiguracjaUrzadzen;

        public override void SetUp()
        {
            base.SetUp();

            printerService = new Mock<IPrinterService>();
          
            tblKonfiguracjaUrzadzen = new Mock<ITblKonfiguracjaUrzadzenRepository>();
            UnitOfWork.Setup(s => s.tblKonfiguracjaUrzadzen).Returns(tblKonfiguracjaUrzadzen.Object);

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new KonfiguracjaUrzadzenViewModel(ViewModelService.Object, printerService.Object);
        }

        #region Properties

        [Test]
        public void RepositoryProperty_IsEstablished()
        {
            var repo = sut.Repository;

            Assert.IsNotNull(repo);
            Assert.IsTrue(repo.ToString().Contains("TblKonfiguracjaUrzadzenRepository"));
        }

        [Test]
        public void WhenSelectedPrinterChanges_VMEntityIsUpdated()
        {
            printerService.Setup(s => s.GetPrinterIP(It.IsAny<string>())).Returns("1");

            sut.SelectedPrinter = "p";

            Assert.AreEqual("p", sut.VMEntity.DrukarkaNazwa);
            Assert.AreEqual("1", sut.VMEntity.DrukarkaIP);
        }

        [Test]
        public void WhenScaleComPortChanges_VMEntityIsUpdated()
        {
            sut.SelectedScaleCom = "C1";

            Assert.AreEqual("C1", sut.VMEntity.WagaComPort);
        }

        #endregion

        #region LoadCommand
        [Test]
        public void LoadCommandExecute_GetAllPrintersAsync()
        {
            sut.LoadCommand.Execute(null);

            printerService.Verify(x => x.GetPrintersAsync());
        }

        [Test]
        public void LoadCommandExecute_GetsVMEntityFromDB()
        {
            sut.LoadCommand.Execute(null);

            tblKonfiguracjaUrzadzen.Verify(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblKonfiguracjaUrzadzen,bool>>>()));
        }



        [Test]
        public void LoadCommandExecute_WhenVMEntityIsNotInDb_CreateNewInstance()
        {
            tblKonfiguracjaUrzadzen.Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblKonfiguracjaUrzadzen, bool>>>())).ReturnsAsync((tblKonfiguracjaUrzadzen)null);

            sut.LoadCommand.Execute(null);

            Assert.IsNotNull(sut.VMEntity);
        }


        [Test]
        public void LoadCommandExecute_ComPortsAreNotNull()
        {

            sut.LoadCommand.Execute(null);

            Assert.IsNotNull(sut.ComPorts);
        }

        [Test]
        public void LoadCommandExecute_SelectedPropertiesAreNotNull()
        {
            var comPorts = SerialPort.GetPortNames();
            string comPort = string.Empty;
            if (comPorts.Count() == 0)
                comPort = "test";

            tblKonfiguracjaUrzadzen.Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblKonfiguracjaUrzadzen, bool>>>()))
                                   .ReturnsAsync(new tblKonfiguracjaUrzadzen
                                   {
                                       WagaComPort = comPort,
                                       DrukarkaNazwa="Zebra"
                                   });

            printerService.Setup(s => s.GetPrintersAsync())
                          .ReturnsAsync(new List<string> { "Zebra", "Zebra2", "Samsung" });

            sut.LoadCommand.Execute(null);

            Assert.IsNotNull(sut.SelectedPrinter);
            Assert.IsNotNull(sut.SelectedScaleCom);
        }

        [Test]
        public void QueryList_Condition_Expectations()
        {
            var list = new List<string> { "a", "b", "c" };

            var res = list.SingleOrDefault(x => x.Contains("a"));

            Assert.IsNotNull(res);
        }
        #endregion



        #region SaveCommand
        [Test]
        public void SaveCommandExecute_AddsComputerNameToVMEntity()
        {
            sut.SaveCommand.Execute(null);

            Assert.IsNotNull(sut.VMEntity.NazwaKomputera);
        }

        [Test]
        public void SaveCommandExecute_AddsOperatorIdToVMEntity()
        {
            UzytkownikZalogowany.Uzytkownik = new tblPracownikGAT { ID_PracownikGAT = 1 };

            sut.SaveCommand.Execute(null);

            Assert.AreEqual(1, sut.VMEntity.IDOperator);
        }

        [Test]
        public void SaveCommandExecute_AddsDateToVMEntity()
        {
            sut.SaveCommand.Execute(null);

            Assert.IsNotNull(sut.VMEntity.DataDodania);
            Assert.IsTrue(sut.VMEntity.DataDodania != default);
        }
        [Test]
        public void SaveCommandExecute_AddsNewlySavedDataToUzytkownikZalogowany()
        {
            UzytkownikZalogowany.KonfiguracjaUrzadzen = new tblKonfiguracjaUrzadzen { DrukarkaIP = "1" };
            sut.VMEntity.DrukarkaIP = "2";
            
            sut.SaveCommand.Execute(null);

            Assert.AreEqual("2",UzytkownikZalogowany.KonfiguracjaUrzadzen.DrukarkaIP);
        }

        #endregion
    }
}
