using AutoFixture;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Produkcja.ZamowieniaOdKlienta.Pakowanie;
using Moq;
using NUnit.Framework;
using System.Collections.ObjectModel;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.ZamowienieOdKlienta.Pakowanie
{
    [TestFixture]
    public class PakowanieViewModelTests
    {
        private Mock<IUnitOfWork> unitOfWork;
        private Mock<IViewService> viewService;
        private Mock<IDialogService> dialogService;
        private Mock<IMessenger> messenger;
        private Fixture fixture;
        private PakowanieViewModel viewModel;

        [SetUp]
        public void Setup()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            viewService = new Mock<IViewService>();
            dialogService = new Mock<IDialogService>();
            messenger = new Mock<IMessenger>();
            fixture = new Fixture();

            viewModel = new PakowanieViewModel(unitOfWork.Object, dialogService.Object,messenger.Object);
        }

        [Test]
        public void ValidujModel_ListaPakowanieNull_True()
        {
            viewModel.ValidujModel();
            bool valid = viewModel.IsValid;

            Assert.IsTrue(valid);
        }

        [Test]
        public void ValidujModel_ListaPakowanieNotNull_True()
        {
            viewModel.ListaPakowanie = new ObservableCollection<tblZamowienieHandlowePakowanie>();
            viewModel.ValidujModel();
            bool valid = viewModel.IsValid;

            Assert.IsTrue(valid);
        }

        [Ignore("tblZamowienieHandlowePakowanie zawsze jest Valid")]
        [Test]
        public void ValidujModel_ListaPakowanieNotNullPakowanieNotValid_False()
        {
            viewModel.ListaPakowanie = new ObservableCollection<tblZamowienieHandlowePakowanie>();
            var pakowanie = new tblZamowienieHandlowePakowanie();
            //var pakowanie = fixture.Build<tblZamowienieHandlowePakowanie>()
            //                        .Without(w => w.tblZamowienieHandlowe)
            //                        .Without(w => w.tblZamowienieHandlowePakowanieRodzaj)
            //                        .Create();

            pakowanie.IsValid = false;
            viewModel.ListaPakowanie.Add(pakowanie);

            viewModel.ValidujModel();

            Assert.IsFalse(viewModel.IsValid);
        }

        [Test]
        public void ViewModel_InheritFromViewModelBase_True()
        {
            var type = viewModel as ViewModelBase;

            Assert.IsNotNull(type);
        }


        [Test]
        public void UsunCommandCanExecute_WybranePakowanieIsNotNull_True()
        {
            dialogService.Setup(d => d.ShowQuestion_BoolResult(It.IsAny<string>(),It.IsAny<string>())).Returns(true);
            viewModel.WybranePakowanie = new tblZamowienieHandlowePakowanie();

            Assert.IsTrue(viewModel.UsunCommand.CanExecute(null));
        }

        [Test]
        public void UsunCommandCanExecute_WybranePakowanieIsNull_False()
        {
            dialogService.Setup(d => d.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            viewModel.WybranePakowanie = null;

            Assert.IsFalse(viewModel.UsunCommand.CanExecute(null));
        }
    }
}
