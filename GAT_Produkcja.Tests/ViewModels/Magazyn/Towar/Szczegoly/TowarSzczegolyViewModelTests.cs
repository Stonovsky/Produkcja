using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Magazyn.Towar.Ewidencja;
using GAT_Produkcja.ViewModel.Magazyn.Towar.Szczegoly;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Magazyn.Towar.Szczegoly
{
    [TestFixture]
    public class TowarSzczegolyViewModelTests
    {

        private Mock<IUnitOfWork> unitOfWork;
        private Mock<IUnitOfWorkFactory> unitOfWorkFactory;
        private Mock<IViewService> viewService;
        private Mock<IDialogService> dialogService;
        private Mock<IMessenger> messenger;
        private Mock<ITblTowarRepository> tblTowar;
        private TowarSzczegolyViewModel sut;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            viewService = new Mock<IViewService>();
            dialogService = new Mock<IDialogService>();
            messenger = new Mock<IMessenger>();

            tblTowar = new Mock<ITblTowarRepository>();

            unitOfWork.Setup(s => s.tblTowar).Returns(tblTowar.Object);


            sut = new TowarSzczegolyViewModel(unitOfWork.Object, dialogService.Object, viewService.Object,messenger.Object);
        }
        private void CzyTowarIstniejeWBazie_False()
        {
            tblTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblTowar, bool>>>())).ReturnsAsync((IEnumerable<tblTowar>)null);

        }

        [Test]
        public void ZapiszCommandExecute_WhenSaved_WindowShouldBeClosed()
        {
            CzyTowarIstniejeWBazie_False();
            sut.Towar = new tblTowar
            {
                IDJm = 2,
                Nazwa = "test",
                Symbol = "test",
                IDTowarGrupa = 1,
                IsValid = true
            };

            sut.ZapiszCommand.Execute(null);

            viewService.Verify(v => v.Close<TowarSzczegolyViewModel>());
        }
        [Test]
        public void ZapiszCommandExecute_WhenSaved_DialogBoxShouldShowed()
        {
            CzyTowarIstniejeWBazie_False();
            sut.Towar = new tblTowar
            {
                IDJm = 2,
                Nazwa = "test",
                Symbol = "test",
                IDTowarGrupa = 1,
                IsValid = true
            };

            sut.ZapiszCommand.Execute(null);

            dialogService.Verify(v => v.ShowInfo_BtnOK(It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        public void ZapiszCommandExecute_WhenSaved_MessageShouldBeSent()
        {

            CzyTowarIstniejeWBazie_False();
            sut.Towar = new tblTowar
            {
                IDJm = 2,
                Nazwa = "test",
                Symbol = "test",
                IDTowarGrupa = 1,
                IsValid = true
            };
            Messenger.OverrideDefault(messenger.Object);

            sut.ZapiszCommand.Execute(null);

            messenger.Verify(v => v.Send<string, TowarEwidencjaViewModel>("Odswiez"));
            Messenger.OverrideDefault(Messenger.Default);
        }

        [Test]
        public void ZapiszCommandExecute_GdyTowarIstniejeWBazie_UoWIsNotInvoked()
        {

            tblTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblTowar, bool>>>())).ReturnsAsync(new List<tblTowar>
            {
                new tblTowar{IDTowar=1, Nazwa="test"}
            });
            sut.Towar = new tblTowar
            {
                IDJm = 2,
                Nazwa = "test",
                Symbol = "test",
                IDTowarGrupa = 1,
                IsValid = true
            };

            sut.ZapiszCommand.Execute(null);

            unitOfWork.Verify(v => v.tblTowar.Add(It.IsAny<tblTowar>()),Times.Never);
            unitOfWork.Verify(v => v.SaveAsync(), Times.Never);
        }

        [Test]
        public void ZapiszCommandExecute_GdyTowarNieIstniejeWBazie_UoWIsInvoked()
        {

            tblTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblTowar, bool>>>())).ReturnsAsync(new List<tblTowar>());
            sut.Towar = new tblTowar
            {
                IDJm = 2,
                Nazwa = "test",
                Symbol = "test",
                IDTowarGrupa = 1,
                IsValid = true
            };

            sut.ZapiszCommand.Execute(null);

            unitOfWork.Verify(v => v.tblTowar.Add(It.IsAny<tblTowar>()));
            unitOfWork.Verify(v => v.SaveAsync());
        }
    }
}
