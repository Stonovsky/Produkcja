using AutoFixture;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Helpers.Geowloknina;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.ZebraPrinter;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.Konfekcja.DrukEtykiet;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.DrukEtykiet
{
    [TestFixture]
    class DrukEtykietViewModelTests
    {

        private Mock<IUnitOfWork> unitOfWork;
        private UnitOfWork unitOfWorkOrg;
        private Mock<IUnitOfWorkFactory> unitOfWorkFactory;
        private Mock<IViewService> viewService;
        private Mock<IDialogService> dialogService;
        private Mock<IMessenger> messenger;
        private Mock<IZebraLabelPrinter> zebraLabelPrinter;
        private Mock<IGeowlokninaHelper> geowlokninaHelper;
        private Fixture fixture;
        private Mock<ITblTowarGeowlokninaParametryRodzajRepository> tblTowarGeowlokninaParametryRodzaj;
        private Mock<ITblTowarGeowlokninaParametryGramaturaRepository> tblTowarGeowlokninaParametryGramatura;
        private DrukEtykietViewModel sut;
        private DrukEtykietViewModel sutUoW;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            unitOfWorkOrg = new UnitOfWork(new GAT_ProdukcjaModel());
            unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            viewService = new Mock<IViewService>();
            dialogService = new Mock<IDialogService>();
            messenger = new Mock<IMessenger>();
            zebraLabelPrinter = new Mock<IZebraLabelPrinter>();
            geowlokninaHelper = new Mock<IGeowlokninaHelper>();

            fixture = new Fixture();

            tblTowarGeowlokninaParametryRodzaj = new Mock<ITblTowarGeowlokninaParametryRodzajRepository>();
            tblTowarGeowlokninaParametryGramatura = new Mock<ITblTowarGeowlokninaParametryGramaturaRepository>();

            unitOfWork.Setup(s => s.tblTowarGeowlokninaParametryRodzaj).Returns(tblTowarGeowlokninaParametryRodzaj.Object);
            unitOfWork.Setup(s => s.tblTowarGeowlokninaParametryGramatura).Returns(tblTowarGeowlokninaParametryGramatura.Object);

            sut = new DrukEtykietViewModel(dialogService.Object, viewService.Object, unitOfWork.Object, zebraLabelPrinter.Object,geowlokninaHelper.Object,messenger.Object);
            sutUoW = new DrukEtykietViewModel(dialogService.Object, viewService.Object, unitOfWorkOrg, zebraLabelPrinter.Object,geowlokninaHelper.Object,messenger.Object);
        }

        [Test]
        public void ZapiszCommandCanExecute_WhenLabelModelIsNotValid_ReturnsFalse()
        {
            sut.LabelModel = new LabelModel { IsValid = false };

            var result = sut.ZapiszCommand.CanExecute(null);

            Assert.IsFalse(result);
        }

        [Ignore("Uzytkownik sam wpisuje sumaryczna ilość etykiet ")]
        [Test]
        public void WhenIloscEtykietDlaSztukIsEntered_CalculateTotalIloscEtykiet()
        {
            sut.LabelModel.Ilosc = 10;
            sut.LabelModel.IloscEtykietNaJednaSztuke = 2;

            sut.PoZmianieIlosciCommand.Execute(null);

            Assert.AreEqual(20, sut.LabelModel.IloscEtykietDoDruku);
        }

        [Test]
        public void ResetujCommandExecute_WhenCalled_NewLabelModel()
        {
            sut.LabelModel = new LabelModel { IsValid = true };

            sut.ResetujCommand.Execute(null);

            Assert.IsFalse(sut.LabelModel.IsValid);
        }

        [Test]
        public void ZaladujWartosciPoczatkoweCommandExecuteAsync_PrinterNameIsNull_ErrorInfoIsInvoked()
        {
            sut.ZaladujWartosciPoczatkoweCommand.Execute(null);

            dialogService.Verify(v => v.ShowError_BtnOK(It.IsAny<string>(), It.IsAny<string>()));
        }


        #region ZapiszCommand
        [Test]
        public void ZapiszCommandExecute_WhenAddedToDB_InfoBoxShouldBeDisplayed()
        {
            sut.LabelModel = fixture.Create<LabelModel>();
            sut.RuchTowaruModel = new tblRuchTowar
            {
                IDRuchTowar = 1
            };

            sut.ZapiszCommand.Execute(null);

            dialogService.Verify(v => v.ShowInfo_BtnOK(It.IsAny<string>(), It.IsAny<string>()));
        }
        [Test]
        public void ZapiszCommandExecute_WhenAddedToDB_MessageShouldBeSent()
        {
            sut.LabelModel=fixture.Create<LabelModel>();
            sut.RuchTowaruModel = new tblRuchTowar
            {
                IDRuchTowar = 1
            };

            sut.ZapiszCommand.Execute(null);

            messenger.Verify(v => v.Send(It.IsAny<tblRuchTowar>()));
        }
        #endregion

        #region ResetujCommand
        [Test]
        public void ResetujCommandExecute_WhenCalled_RuchTowaruModelShouldBeInstantiated()
        {
            sut.RuchTowaruModel = new tblRuchTowar { IDRuchTowar = 1 };

            sut.ResetujCommand.Execute(null);

            Assert.AreEqual(0, sut.RuchTowaruModel.IDRuchTowar);
        }
        #endregion
    }
}
