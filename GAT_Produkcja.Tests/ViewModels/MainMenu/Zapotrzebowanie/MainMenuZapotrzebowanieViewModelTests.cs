using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.MailSenders;
using GAT_Produkcja.ViewModel.MainMenu.Zapotrzebowanie;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.MainMenu.Zapotrzebowanie
{
    [TestFixture]
    public class MainMenuZapotrzebowanieViewModelTests : TestBase
    {

        private Mock<IOutlookMailSender> outlookMailSender;
        private MainMenuZapotrzebowanieViewModel sut;

        public override void SetUp()
        {
            base.SetUp();

            outlookMailSender = new Mock<IOutlookMailSender>();

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new MainMenuZapotrzebowanieViewModel(ViewModelService.Object,
                                                        outlookMailSender.Object);
        }



        private MainMenuZapotrzebowanieViewModel CreateSut(IMessenger messenger)
        {
            return new MainMenuZapotrzebowanieViewModel(ViewModelService.Object,
                                                        outlookMailSender.Object);
        }

    }
}
