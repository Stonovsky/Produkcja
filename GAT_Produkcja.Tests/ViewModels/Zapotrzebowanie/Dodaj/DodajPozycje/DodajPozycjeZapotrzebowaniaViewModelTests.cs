using AutoFixture;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Zapotrzebowanie.Dodaj.DodajPozycje;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Zapotrzebowanie.Dodaj.DodajPozycje
{
    [TestFixture]
    public class DodajPozycjeZapotrzebowaniaViewModelTests
    {

        private Mock<IUnitOfWork> unitOfWork;
        private Mock<IUnitOfWorkFactory> unitOfWorkFactory;
        private Mock<IViewService> viewService;
        private Mock<IDialogService> dialogService;
        private Mock<IMessenger> messenger;
        private Messenger messengerOrg;
        private Fixture fixture;
        private Mock<ITblZapotrzebowaniePozycjeRepository> tblZapotrzebowaniePozycje;
        private DodajPozycjeZapotrzebowaniaViewModel sut;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            viewService = new Mock<IViewService>();
            dialogService = new Mock<IDialogService>();
            messenger = new Mock<IMessenger>();
            messengerOrg = new Messenger();
            fixture = new Fixture();

            tblZapotrzebowaniePozycje = new Mock<ITblZapotrzebowaniePozycjeRepository>();
            unitOfWork.Setup(s => s.tblZapotrzebowaniePozycje).Returns(tblZapotrzebowaniePozycje.Object);


            sut = CreateSut(messenger.Object);
        }

        private DodajPozycjeZapotrzebowaniaViewModel CreateSut(IMessenger messenger)
        {
            return new DodajPozycjeZapotrzebowaniaViewModel(unitOfWork.Object, viewService.Object, dialogService.Object, messenger);
        }

        private tblZapotrzebowaniePozycje ZapotrzebowaniePozycje_IsValid()
        {
            return fixture.Build<tblZapotrzebowaniePozycje>().Create();
        }

        #region MessengerRegistered

        [Test]
        public void MessengerRegistered_ForSendingID()
        {
            messenger.Verify(v => v.Register<tblZapotrzebowaniePozycje>(sut, It.IsAny<Action<tblZapotrzebowaniePozycje>>(), It.IsAny<bool>()));
        }

        #endregion

        #region GdyPrzeslanoZapotrzebowaniePozycje
        [Test]
        public void GdyPrzeslanoZapotrzebowaniePozycje_WhenArgIsNull_DoNothing()
        {
            sut = CreateSut(messengerOrg);

            messengerOrg.Send((tblZapotrzebowanie)null);

            tblZapotrzebowaniePozycje.Verify(v => v.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }
        [Test]
        public void GdyPrzeslanoZapotrzebowaniePozycje_WhenArgIsNotNull_InvokeGetByIdAsync()
        {
            sut = CreateSut(messengerOrg);

            messengerOrg.Send(new tblZapotrzebowaniePozycje { IDZapotrzebowaniePozycja=1});

            tblZapotrzebowaniePozycje.Verify(v => v.GetByIdAsync(It.IsAny<int>()));
        }
        [Test]
        public void GdyPrzeslanoZapotrzebowaniePozycje_WhenArgIsNotNull_IDZapotrzebowPozycjaIsZero_DoNotInvokeGetByIdAsync()
        {
            sut = CreateSut(messengerOrg);

            messengerOrg.Send(new tblZapotrzebowaniePozycje { IDZapotrzebowaniePozycja = 0 });

            tblZapotrzebowaniePozycje.Verify(v => v.GetByIdAsync(It.IsAny<int>()),Times.Never);
        }
        #endregion
    }
}
