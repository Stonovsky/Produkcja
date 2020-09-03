using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Magazyn.RuchTowaru.KodKreskowy;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Magazyn.RuchTowaru.KodKreskowy
{
    [TestFixture]
    class RuchTowaruKodKreskowyDodajViewModelTests
    {
        private Mock<IViewService> viewService;
        private Mock<IMessenger> messenger;
        private RuchTowaruKodKreskowyDodajViewModel sut;

        [SetUp]
        public void SetUp()
        {
            viewService = new Mock<IViewService>();
            messenger = new Mock<IMessenger>();
            sut = new RuchTowaruKodKreskowyDodajViewModel(viewService.Object,messenger.Object);
        }

        [Test]
        public void WhenKodKreskowyPropertyChanged_SendsMessage()
        {
            Messenger.OverrideDefault(messenger.Object);

            sut.KodKreskowy = "123";

            messenger.Verify(v => v.Send(It.IsAny<string>(), "KodKreskowy"));
        }
        [Test]
        public void WhenKodKreskowyPropertyChanged_CloseWindow()
        {
            Messenger.OverrideDefault(messenger.Object);

            sut.KodKreskowy = "123";

            viewService.Verify(v => v.Close<RuchTowaruKodKreskowyDodajViewModel>());
        }
    }
}
