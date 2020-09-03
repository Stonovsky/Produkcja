using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.Helpers.Theme;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.MainMenu.MenuBar;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Naglowek;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.MainMenu.Menubar
{
    public class MainMenuBarViewModelTests : TestBaseGeneric<MainMenuBarViewModel>
    {
        private Mock<IThemeChangerHelper> themeChangerHelper;

        public override void SetUp()
        {
            base.SetUp();

            themeChangerHelper = new Mock<IThemeChangerHelper>();

            CreateSut();
        }

        public override void CreateSut()
        {
            sut = new MainMenuBarViewModel(ViewModelService.Object, themeChangerHelper.Object);
        }

        [Test]
        [TestCase("Linia włóknin")]
        [TestCase("Linia do kalandrowania")]
        [TestCase("Linia do konfekcji")]
        public void RejestracjaProdukcjiOtworOknoCommand_PrzeslanyObjectJestZgodnyy_OtwieraOknoZRejestracja(string gniazdo)
        {
            sut.RejestracjaProdukcjiOtworOknoCommand.Execute(gniazdo);

            ViewService.Verify(v => v.Show<GPRuchNaglowekViewModel>());
        }
        [Test]
        [TestCase("Linia włóknin", GniazdaProdukcyjneEnum.LiniaWloknin)]
        [TestCase("Linia do kalandrowania", GniazdaProdukcyjneEnum.LiniaDoKalandowania)]
        [TestCase("Linia do konfekcji", GniazdaProdukcyjneEnum.LiniaDoKonfekcji)]
        public void RejestracjaProdukcjiOtworOknoCommand_PrzeslanyObjectJestZgodnyy_WysylaStatusRuchu(string gniazdo, GniazdaProdukcyjneEnum gniazdoEnum)
        {
            sut.RejestracjaProdukcjiOtworOknoCommand.Execute(gniazdo);

            Messenger.Verify(m => m.Send(It.Is<tblProdukcjaGniazdoProdukcyjne>(t => t.IDProdukcjaGniazdoProdukcyjne == (int)gniazdoEnum)));
        }

    }
}
