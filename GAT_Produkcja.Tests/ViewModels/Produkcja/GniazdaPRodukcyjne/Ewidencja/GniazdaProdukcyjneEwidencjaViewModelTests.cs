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

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.GniazdaProdukcyjne.Ewidencja
{
    [TestFixture]
    public class GniazdaProdukcyjneEwidencjaViewModelTests
    {

        private Mock<IUnitOfWork> unitOfWork;
        private Mock<IUnitOfWorkFactory> unitOfWorkFactory;
        private Mock<IViewService> viewService;
        private Mock<IDialogService> dialogService;
        private Mock<IMessenger> messenger;
        private Mock<ITblProdukcjaGniazdoProdukcyjneRepository> tblProdukcjaGniazdoProdukcyjne;
        private Mock<ITblTowarGrupaRepository> tblTowarGrupa;

        public GniazdaProdukcyjneEwidencjaViewModel sut { get; private set; }

        [SetUp]
        public void SetUp()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            viewService = new Mock<IViewService>();
            dialogService = new Mock<IDialogService>();
            messenger = new Mock<IMessenger>();

            tblProdukcjaGniazdoProdukcyjne = new Mock<ITblProdukcjaGniazdoProdukcyjneRepository>();
            unitOfWork.Setup(s => s.tblProdukcjaGniazdoProdukcyjne).Returns(tblProdukcjaGniazdoProdukcyjne.Object);

            tblTowarGrupa = new Mock<ITblTowarGrupaRepository>();
            unitOfWork.Setup(s => s.tblTowarGrupa).Returns(tblTowarGrupa.Object);

            unitOfWorkFactory.Setup(s => s.Create()).Returns(unitOfWork.Object);

            StworzSUT();
        }

        private void StworzSUT()
        {
            sut = new GniazdaProdukcyjneEwidencjaViewModel(unitOfWork.Object, unitOfWorkFactory.Object, viewService.Object, dialogService.Object, messenger.Object);

        }
        
        #region WartosciPoczatkowe

        [Test]
        public void ZaladujWartosciPoczatkoweCommand_WhenCalled_tblTowarGrupaGetAllAsyncIsInvoked()
        {

            sut.ZaladujWartosciPoczatkoweCommand.Execute(null);

            tblProdukcjaGniazdoProdukcyjne.Verify(v => v.GetAllAsync());
            tblTowarGrupa.Verify(v => v.PobierzGrupeTowarowDlaGniazdAsync());
        }

        #endregion

        #region WyslijGniazdo
        [Test]
        public void WyslijMessageZGniazdemCommandExecute_SendsWybraneGniazdoProdukcyjne()
        {
            Messenger.OverrideDefault(messenger.Object);

            sut.WyslijMessageZGniazdemCommand.Execute(null);

            messenger.Verify(v => v.Send(It.IsAny<tblProdukcjaGniazdoProdukcyjne>()));
        }
        [Test]
        public void WyslijMessageZGniazdemCommandExecute_IfWybraneGniazdoProdukcyjneIsNull()
        {
            Messenger.OverrideDefault(messenger.Object);
            sut.WybraneGniazdoProdukcyjne = null;

            sut.WyslijMessageZGniazdemCommand.Execute(null);

            messenger.Verify(v => v.Send(It.IsAny<tblProdukcjaGniazdoProdukcyjne>()), Times.Never);
        }
        #endregion

        #region UsunGniazdoCommand
        #region Execute
        [Test]
        public void UsunCommandExecute_IfIdIsNotNull_DialogServiceShouldBeInvokedToConfirm()
        {
            sut.WybraneGniazdoProdukcyjne.IDProdukcjaGniazdoProdukcyjne = 1;

            sut.UsunGniazdoCommand.Execute(null);

            dialogService.Verify(v => v.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>()));
        }
        [Test]
        public void UsunCommandExecute_IfIdIsNotNull_UoWReomveShouldBeInvoked()
        {
            sut.WybraneGniazdoProdukcyjne.IDProdukcjaGniazdoProdukcyjne = 1;
            dialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            sut.UsunGniazdoCommand.Execute(null);

            tblProdukcjaGniazdoProdukcyjne.Verify(v => v.Remove(It.IsAny<tblProdukcjaGniazdoProdukcyjne>()));
        }
        [Test]
        public void UsunCommandExecute_IfIdIsNotNull_UoWSaveAsyncShouldBeInvoked()
        {
            sut.WybraneGniazdoProdukcyjne.IDProdukcjaGniazdoProdukcyjne = 1;
            dialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            sut.UsunGniazdoCommand.Execute(null);

            unitOfWork.Verify(v => v.SaveAsync());
        }
        #endregion

        #region CanExecute
        [Test]
        public void UsunGniazdoCommandCanExecute_IfIDisZero_False()
        {
            sut.WybraneGniazdoProdukcyjne.IDProdukcjaGniazdoProdukcyjne = 0;

            var result = sut.UsunGniazdoCommand.CanExecute(null);

            Assert.IsFalse(result);
        }
        [Test]
        public void UsunGniazdoCommandCanExecute_IfIDisNull_False()
        {
            sut.WybraneGniazdoProdukcyjne = null;

            var result = sut.UsunGniazdoCommand.CanExecute(null);

            Assert.IsFalse(result);
        }

        [Test]
        public void UsunGniazdoCommandCanExecute_IdIsNot0_ReturnsTrue()
        {
            sut.WybraneGniazdoProdukcyjne.IDProdukcjaGniazdoProdukcyjne = 1;

            var result = sut.UsunGniazdoCommand.CanExecute(null);

            Assert.IsTrue(result);
        }


        #endregion

        #endregion

        #region DodajGniazdoCommand
        [Test]
        public void DodajGniazdoCommandExecute_viewServiceShouldBeInvoked()
        {
            sut.DodajGniazdoCommand.Execute(null);

            viewService.Verify(v => v.ShowDialog<GniazdaProdukcyjneDodajViewModel>());
        }
        #endregion
    }
}
