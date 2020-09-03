using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Magazyn.Ewidencje.RuchTowaru;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Magazyn.Ewidencje.RuchTowaru
{
    [TestFixture]
    public class MagazynRuchTowaruViewModelTests
    {
        private Mock<IUnitOfWork> unitOfWork;
        private Mock<IUnitOfWorkFactory> unitOfWorkFactory;
        private Mock<IViewService> viewService;
        private Mock<IDialogService> dialogService;
        private Mock<IMessenger> messenger;
        private Mock<ITblRuchNaglowekRepository> tblRuchNaglowek;
        private Mock<ITblRuchTowarRepository> tblRuchTowar;
        private Mock<ITblRuchTowarGeowlokninaParametryRepository> tblRuchTowarGeowlokninaParametry;
        private MagazynRuchTowaruViewModel sut;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            viewService = new Mock<IViewService>();
            dialogService = new Mock<IDialogService>();
            messenger = new Mock<IMessenger>();

            tblRuchNaglowek = new Mock<ITblRuchNaglowekRepository>();
            tblRuchTowar = new Mock<ITblRuchTowarRepository>();
            tblRuchTowarGeowlokninaParametry = new Mock<ITblRuchTowarGeowlokninaParametryRepository>();

            unitOfWork.Setup(s => s.tblRuchNaglowek).Returns(tblRuchNaglowek.Object);
            unitOfWork.Setup(s => s.tblRuchTowar).Returns(tblRuchTowar.Object);
            unitOfWork.Setup(s => s.tblRuchTowarGeowlokninaParametry).Returns(tblRuchTowarGeowlokninaParametry.Object);

            sut = new MagazynRuchTowaruViewModel(unitOfWork.Object, viewService.Object, dialogService.Object,messenger.Object);
        }
        [Test]
        public void WhenInitialized_ListaRuchuTowarowIsNotNull()
        {
            Assert.IsNotNull(sut.ListaRuchuTowarow);
        }

        [Test]
        public void WhenInitialized_MessengerIsRegistered()
        {
            Messenger.OverrideDefault(messenger.Object);
            sut = new MagazynRuchTowaruViewModel(unitOfWork.Object, viewService.Object, dialogService.Object,messenger.Object);

            messenger.Verify(v => v.Register<string>(sut, It.IsAny<Action<string>>(),It.IsAny<bool>()));
        }

        [Test]
        public void UsunRuchTowaruCommandExecute_WhenCalled_BoolQuestionShouldBeDisplayed()
        {
            sut.UsunRuchTowaruCommand.Execute(null);
            dialogService.Verify(v => v.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>()));
        }
        [Test]
        public void UsunRuchTowaruCommandExecute_WhenWybranyProduktIsNull_UoWGetByIDShouldNotBeInvoked()
        {
            DialogService_True();
            sut.UsunRuchTowaruCommand.Execute(null);

            tblRuchTowar.Verify(v => v.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }
        [Test]
        public void UsunRuchTowaruCommandExecute_WhenTowarRuchGeowlokninaParametryIsNull_UoWRemoveIsNotInvoked()
        {
            DialogService_True();
            sut.WybranyTowar = new vwRuchTowaru() { IDRuchTowar = 1 };
            tblRuchTowarGeowlokninaParametry.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((tblRuchTowarGeowlokninaParametry)null);

            sut.UsunRuchTowaruCommand.Execute(null);

            tblRuchTowarGeowlokninaParametry.Verify(v => v.Remove(It.IsAny<tblRuchTowarGeowlokninaParametry>()), Times.Never);
        }
        [Test]
        public void UsunRuchTowaruCommandExecute_towarRuchIsNull_UoWRemoveIsNotInvoked()
        {
            DialogService_True();
            sut.WybranyTowar = new vwRuchTowaru() { IDRuchTowar = 1 };
            tblRuchTowar.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((tblRuchTowar)null);

            sut.UsunRuchTowaruCommand.Execute(null);

            tblRuchTowar.Verify(v => v.Remove(It.IsAny<tblRuchTowar>()), Times.Never);
        }
        [Test]
        public void UsunRuchTowaruCommandExecute_RuchTowarListOfRuchNaglowekIDisEmpty_RemoveRuchNaglowekIsNotInvoked()
        {
            DialogService_True();
            sut.WybranyTowar = new vwRuchTowaru() { IDRuchTowar = 1 };
            tblRuchTowar.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((tblRuchTowar)null);
            tblRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblRuchTowar, bool>>>())).ReturnsAsync(new List<tblRuchTowar>());

            sut.UsunRuchTowaruCommand.Execute(null);

            tblRuchNaglowek.Verify(v => v.Remove(It.IsAny<tblRuchNaglowek>()),Times.Never);
        }
        [Test]
        public void UsunRuchTowaruCommandExecute_RuchTowarListOfRuchNaglowekIDisNotEmpty_RemoveRuchNaglowek()
        {
            DialogService_True();
            sut.WybranyTowar = new vwRuchTowaru() { IDRuchTowar = 1 };
            tblRuchTowar.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((tblRuchTowar)null);
            tblRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblRuchTowar, bool>>>())).ReturnsAsync(new List<tblRuchTowar>
            {
            new tblRuchTowar{IDTowar=1, IDRuchTowar=1, IDRuchNaglowek=1}
            });

            sut.UsunRuchTowaruCommand.Execute(null);

            tblRuchNaglowek.Verify(v => v.Remove(It.IsAny<tblRuchNaglowek>()));
        }

        [Test]
        public void UsunRuchTowaruCommandExecute_WhenSaved_MessageShouldBeSent()
        {
            DialogService_True();
            Messenger.OverrideDefault(messenger.Object);
            sut.WybranyTowar = new vwRuchTowaru { IDRuchTowar = 1 };

            sut.UsunRuchTowaruCommand.Execute(null);

            messenger.Verify(v => v.Send(It.IsAny<string>()));
        }

        private void DialogService_True()
        {
            dialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        }
    }
}
