using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Helpers.Theme;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.CurrencyService.NBP;
using GAT_Produkcja.Utilities.Logger;
using GAT_Produkcja.ViewModel.MainMenu;
using GAT_Produkcja.ViewModel.MainMenu.Zapotrzebowanie;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Naglowek;
using GAT_Produkcja.ViewModel.ZamowienieOdKlienta.Szczegoly;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.MainMenu
{
    public class MainMenuViewModelTests : TestBase
    {

        private Mock<IZamowienieOdKlientaEwidencjaSzczegolyUCViewModel> zkSzczegolyUC;
        private Mock<IMainMenuZapotrzebowanieViewModel> zapotrzebowanieVM;
        private Mock<INBPService> nbpService;
        private Mock<ITblPracownikGATRepository> tblPracownikGAT;
        private MainMenuViewModel sut;

        public override void SetUp()
        {
            base.SetUp();

            zkSzczegolyUC = new Mock<IZamowienieOdKlientaEwidencjaSzczegolyUCViewModel>();
            zapotrzebowanieVM = new Mock<IMainMenuZapotrzebowanieViewModel>();
            nbpService = new Mock<INBPService>();

            tblPracownikGAT = new Mock<ITblPracownikGATRepository>();
            UnitOfWork.Setup(s => s.tblPracownikGAT).Returns(tblPracownikGAT.Object);

            CreateSut();
        }

        public override void CreateSut()
        {
            sut = new MainMenuViewModel(ViewModelService.Object, 
                                        nbpService.Object,
                                        zkSzczegolyUC.Object,
                                        zapotrzebowanieVM.Object);
        }

    }
}
