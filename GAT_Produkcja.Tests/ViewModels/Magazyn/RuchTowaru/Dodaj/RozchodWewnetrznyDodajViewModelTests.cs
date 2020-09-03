using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Magazyn.RozchodWewnetrzny;
using GAT_Produkcja.ViewModel.Magazyn.RozchodWewnetrzny.Dodaj;
using GAT_Produkcja.ViewModel.Magazyn.RuchTowaru;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Magazyn.RuchTowaru.Dodaj
{
    [TestFixture]
    public class RozchodWewnetrznyDodajViewModelTests
    {
        private Mock<IUnitOfWork> unitOfWork;
        private Mock<IUnitOfWorkFactory> unitOfWorkFactory;
        private Mock<IViewService> viewService;
        private Mock<IDialogService> dialogService;
        private Mock<IMessenger> messenger;
        private Mock<ITblRuchTowarRepository> tblRuchTowar;
        private Mock<ITblTowarRepository> tblTowar;
        private Mock<ITblJmRepository> tblJm;
        private Mock<ITblMagazynRepository> tblMagazyn;
        private RozchodWewnetrznyDodajViewModel sut;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            viewService = new Mock<IViewService>();
            dialogService = new Mock<IDialogService>();
            messenger = new Mock<IMessenger>();

            tblRuchTowar = new Mock<ITblRuchTowarRepository>();
            tblTowar = new Mock<ITblTowarRepository>();
            tblJm = new Mock<ITblJmRepository>();
            tblMagazyn = new Mock<ITblMagazynRepository>();

            unitOfWork.Setup(s => s.tblRuchTowar).Returns(tblRuchTowar.Object);
            unitOfWork.Setup(s => s.tblJm).Returns(tblJm.Object);
            unitOfWork.Setup(s => s.tblTowar).Returns(tblTowar.Object);
            unitOfWork.Setup(s => s.tblMagazyn).Returns(tblMagazyn.Object);


            sut = new RozchodWewnetrznyDodajViewModel(unitOfWork.Object, viewService.Object, dialogService.Object,messenger.Object);
        }



        [Test]
        [Ignore("Nie rozwiajm tego VM dalej")]
        public void GdyPrzeslanoTowar_CzyMessengerZarejestowanyNaOdebranieTowaru()
        {
            Messenger.Default.Send<vwStanTowaru, RuchTowaruViewModel>(new vwStanTowaru { IDTowar=1, IDMagazyn=1, IloscDostepna=1});
            
            tblTowar.Verify(v => v.GetByIdAsync(It.IsAny<int>()));
        }
    }
}
