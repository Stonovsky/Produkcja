using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.Dodaj;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.Ewidencja;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.GniazdaProdukcyjne.Dodaj
{
    [TestFixture]
    class GniazdaProdukcyjneDodajViewModelTests
    {
        private Mock<IUnitOfWork> unitOfWork;
        private Mock<IUnitOfWorkFactory> unitOfWorkFactory;
        private Mock<IViewService> viewService;
        private Mock<IDialogService> dialogService;
        private Mock<IMessenger> messenger;
        private Mock<ITblProdukcjaGniazdoProdukcyjneRepository> tblProdukcjaGniazdoProdukcyjne;
        private GniazdaProdukcyjneDodajViewModel sut;
        private Mock<ITblTowarGrupaRepository> tblTowarGrupa;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            viewService = new Mock<IViewService>();
            dialogService = new Mock<IDialogService>();
            messenger = new Mock<IMessenger>();

            unitOfWorkFactory.Setup(s => s.Create()).Returns(unitOfWork.Object);

            tblProdukcjaGniazdoProdukcyjne = new Mock<ITblProdukcjaGniazdoProdukcyjneRepository>();
            unitOfWork.Setup(s => s.tblProdukcjaGniazdoProdukcyjne).Returns(tblProdukcjaGniazdoProdukcyjne.Object);

            tblTowarGrupa = new Mock<ITblTowarGrupaRepository>();
            unitOfWork.Setup(s => s.tblTowarGrupa).Returns(tblTowarGrupa.Object);

            sut = new GniazdaProdukcyjneDodajViewModel(unitOfWork.Object, unitOfWorkFactory.Object,dialogService.Object,viewService.Object,messenger.Object);
        }

        #region CloneObject
        [Test]
        public void IsChanged_WhenObjectsDiffers_True()
        {
            sut.Gniazdo.GniazdoNazwa = "test";

            Assert.IsTrue(sut.IsChanged);
        }
        [Test]
        public void IsChanged_WhenObjectsNotDiffers_False()
        {
            sut.Gniazdo.GniazdoNazwa = null;

            Assert.IsFalse(sut.IsChanged);
        }
        #endregion

        #region Commands

        #region ZaladujWartosciPoczatkoweCommand
        [Test]
        public void ZaladujWartosciPoczatkoweCommand_UoW_NeededTblShouldBeInvoked()
        {

            sut.ZaladujWartosciPoczatkoweCommand.Execute(null);

            tblTowarGrupa.Verify(v => v.GetAllAsync());
        }

        #endregion

        #region ZapiszCommand
        private void ZapiszCommandCanExecute_True()
        {
            sut.Gniazdo.GniazdoNazwa = "test";
            sut.Gniazdo.IDTowarGrupa = 1;
            sut.Gniazdo.IsValid = true;
        }

        [Test]
        public void ZapiszCommandCanExecute_NoChangesMade_False()
        {
            var expected = sut.ZapiszCommand.CanExecute(null);

            Assert.IsFalse(expected);
        }

        [Test]
        public void ZapiszCommandCanExecute_NoweGniazdoIsNotValid_False()
        {
            sut.Gniazdo.IsValid = false;

            var expected = sut.ZapiszCommand.CanExecute(null);

            Assert.IsFalse(expected);
        }


        [Test]
        public void ZapiszCommandExecute_IdIsZero_UoWAddMethodIsInvoked()
        {
            ZapiszCommandCanExecute_True();

            sut.ZapiszCommand.Execute(null);

            tblProdukcjaGniazdoProdukcyjne.Verify(v => v.Add(It.IsAny<tblProdukcjaGniazdoProdukcyjne>()));

        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        public void ZapiszCommandExecute_IDisZeroOrAbove_UowSaveAsync(int id)
        {
            ZapiszCommandCanExecute_True();
            sut.Gniazdo.IDProdukcjaGniazdoProdukcyjne = id;
            sut.ZapiszCommand.Execute(null);

            unitOfWork.Verify(v => v.SaveAsync());
        }

        [Test]
        public void ZapiszCommandExecute_IsSaved_MessageIsSent()
        {
            Messenger.OverrideDefault(messenger.Object);
            ZapiszCommandCanExecute_True();
            sut.Gniazdo.IDProdukcjaGniazdoProdukcyjne = 0;
            
            sut.ZapiszCommand.Execute(null);

            messenger.Verify(v => v.Send<string, GniazdaProdukcyjneEwidencjaViewModel>(It.IsAny<string>()));
        }
        [Test]
        public void ZapiszCommandExecute_IsSaved_ViewIsClosed()
        {
            Messenger.OverrideDefault(messenger.Object);
            ZapiszCommandCanExecute_True();
            sut.Gniazdo.IDProdukcjaGniazdoProdukcyjne = 0;

            sut.ZapiszCommand.Execute(null);

            viewService.Verify(v => v.Close<GniazdaProdukcyjneDodajViewModel>());
        }
        #endregion

        #region ZamknijOknoCommandExecute
        [Test]
        public void ZamknijOknoCommandExecute_VMIsChanged_InvokeDialogService()
        {
            sut.Gniazdo.GniazdoNazwa = "test";

            sut.ZamknijOknoCommand.Execute(null);

            dialogService.Verify(v => v.ShowQuestion_BoolResult(It.IsAny<string>(),It.IsAny<string>()));
        }

        [Test]
        public void ZamknijOknoCommandExecute_VMIsChanged_InvokeViewService()
        {
            sut.Gniazdo.GniazdoNazwa = "test";
            dialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);


            sut.ZamknijOknoCommand.Execute(null);

            viewService.Verify(v => v.Close<GniazdaProdukcyjneDodajViewModel>());
        }

        [Test]
        public void ZamknijOknoCommandExecute_VMIsChanged_And_dialogServiceIsFalse_ViewServiceNotInvoked()
        {
            sut.Gniazdo.GniazdoNazwa = "test";
            dialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(false);


            sut.ZamknijOknoCommand.Execute(null);

            viewService.Verify(v => v.Close<GniazdaProdukcyjneDodajViewModel>(),Times.Never);
        }
        #endregion
        #endregion

    }
}
