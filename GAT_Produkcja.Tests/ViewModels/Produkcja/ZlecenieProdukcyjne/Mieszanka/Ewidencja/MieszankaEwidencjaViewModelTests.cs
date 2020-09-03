using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Produkcja.Mieszanka.Dodaj;
using GAT_Produkcja.ViewModel.Produkcja.Mieszanka.Ewidencja;
using Moq;
using NUnit.Framework;
using System;
using System.Linq.Expressions;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.ZlecenieProdukcyjne.Mieszanka.Ewidencja
{
    [TestFixture]
    public class MieszankaEwidencjaViewModelTests
    {
        private MieszankaEwidencjaViewModel sut;
        private Mock<IUnitOfWork> unitOfWork;
        private Mock<IViewService> viewService;
        private Mock<IDialogService> dialogService;
        private Mock<IMessenger> messenger;
        private IMessenger messengerOrg;
        private Mock<ITblMieszankaRepository> tblMieszanka;
        private Mock<ITblJmRepository> tblJm;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            viewService = new Mock<IViewService>();
            dialogService = new Mock<IDialogService>();
            messenger = new Mock<IMessenger>();
            messengerOrg = new Messenger();

            tblMieszanka = new Mock<ITblMieszankaRepository>();
            tblJm = new Mock<ITblJmRepository>();
            unitOfWork.Setup(s => s.tblMieszanka).Returns(tblMieszanka.Object);
            unitOfWork.Setup(s => s.tblJm).Returns(tblJm.Object);

            Messenger.OverrideDefault(Messenger.Default);

            sut = CreateSut(messenger.Object);
        }
        private MieszankaEwidencjaViewModel CreateSut(IMessenger messenger)
        {
            return new MieszankaEwidencjaViewModel(unitOfWork.Object, viewService.Object, messenger);

        }

        #region Initializing

        [Test]
        public void ZaladujWartosciPoczatkoweCommandExecute_WhenCalled_UOWtblMieszankaNeedsToBeInvoked()
        {
            sut.ZaladujWartosciPoczatkoweCommand.Execute(null);

            tblMieszanka.Verify(v => v.GetAllWithJmAsync());
        }
        [Test]
        public void WhenVMisInitialized_TytulCannotBeNullOrEmpty()
        {
            Assert.IsFalse(string.IsNullOrEmpty(sut.Tytul));
        } 
        #endregion

        #region MessengerRegistered

        [Test]
        public void WhenMessageIsSentFromMieszankaDodajViewModel_UOWtblMieszankaNeedsToBeInvoked()
        {
            sut = CreateSut(messengerOrg);
            messengerOrg.Send<string, MieszankaEwidencjaViewModel>("Odswiez");

            tblMieszanka.Verify(v => v.GetAllWithJmAsync());
        } 
        #endregion

        #region SzukajCommand
        [Test]
        public void SzukajCommandCanExecute_ShouldBeAlwaysTrue_ReturnsTrue()
        {
            var result = sut.SzukajCommand.CanExecute(null);

            Assert.IsTrue(result);
        }

        [Test]
        public void SzukajCommandExecute_WhenCalled_UOWtblMieszankaFindAsyncShouldBeCalledWithNazwaMieszankiDoWyszukania()
        {
            sut.NazwaMieszankiDoWyszukania = "Mieszanka1";

            sut.SzukajCommand.Execute(null);

            tblMieszanka.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<tblMieszanka, bool>>>()));
        }

        #endregion

        #region DodajMieszankeCommand
        [Test]
        public void DodajMieszankeCommandExecute_InvokeViewServiceWithMieszankaDodajViewModel()
        {
            sut.DodajMieszankeCommand.Execute(null);

            viewService.Verify(v => v.Show<MieszankaDodajViewModel>());
        } 
        #endregion

        #region PokazSzczegolyCommand
        [Test]
        public void PokazSzczegolyCommandExecute_WhenCalled_MessageToMieszankaDodajViewModelShouldBeSent()
        {
            Messenger.OverrideDefault(messenger.Object);

            sut.PokazSzczegolyCommand.Execute(null);

            messenger.Verify(v => v.Send<tblMieszanka, MieszankaDodajViewModel>(It.IsAny<tblMieszanka>()));
            Messenger.OverrideDefault(Messenger.Default);
        }
        [Test]
        public void PokazSzczegolyCommandExecute_WhenCalled_viewServiceWithDodajViewModelShouldBeInvoked()
        {
            sut.PokazSzczegolyCommand.Execute(null);

            viewService.Verify(v => v.Show<MieszankaDodajViewModel>());
        }
        [Test]
        public void PokazSzczegolyCommandExecute_CheckIfMethodsAreCalledInCorrectOrder()
        {
            string callOrder = "";
            viewService.Setup(s => s.Show<MieszankaDodajViewModel>()).Callback(() => callOrder += "1");
            Messenger.OverrideDefault(messenger.Object);
            messenger.Setup(s => s.Send<tblMieszanka, MieszankaDodajViewModel>(It.IsAny<tblMieszanka>())).Callback(() => callOrder += "2");

            sut.PokazSzczegolyCommand.Execute(null);

            Assert.AreEqual("12", callOrder);
            Messenger.OverrideDefault(Messenger.Default);
        }
        #endregion
    }
}
