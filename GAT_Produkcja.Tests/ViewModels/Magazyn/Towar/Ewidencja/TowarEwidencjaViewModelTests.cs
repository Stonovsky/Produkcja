using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Magazyn.Towar.Ewidencja;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Magazyn.Towar.Ewidencja
{
    [TestFixture]
    public class TowarEwidencjaViewModelTests
    {
        private Mock<IUnitOfWork> unitOfWork;
        private Mock<IViewService> viewService;
        private Mock<IDialogService> dialogService;
        private Mock<IMessenger> messenger;
        private TowarEwidencjaViewModel sut;
        private Mock<IUnitOfWorkFactory> unitOfWorkFactory;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            viewService = new Mock<IViewService>();
            dialogService = new Mock<IDialogService>();
            messenger = new Mock<IMessenger>();

            Messenger.OverrideDefault(messenger.Object);

            sut = new TowarEwidencjaViewModel(unitOfWorkFactory.Object, dialogService.Object, viewService.Object,messenger.Object);
            Messenger.OverrideDefault(Messenger.Default);
        }

        #region Initialize

        [Test]
        public void CTOR_MessengerForOdswiezMessage_IsRegistered()
        {
            messenger.Verify(v => v.Register<string>(sut, It.IsAny<Action<string>>(), It.IsAny<bool>()));
        }

        #endregion
    }
}
