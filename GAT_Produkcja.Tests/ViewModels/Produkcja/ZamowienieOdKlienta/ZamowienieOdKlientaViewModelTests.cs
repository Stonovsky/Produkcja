using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Produkcja.ZamowieniaOdKlienta;
using GAT_Produkcja.ViewModel.Produkcja.ZamowieniaOdKlienta.Pakowanie;
using GAT_Produkcja.ViewModel.Produkcja.ZamowieniaOdKlienta.TowarGeokomorka;
using GAT_Produkcja.ViewModel.Produkcja.ZamowieniaOdKlienta.TowarGeowloknina;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.ZamowienieOdKlienta
{
    [TestFixture]
    public class ZamowienieOdKlientaViewModelTests
    {
        private Mock<IUnitOfWork> unitOfWork;
        private Mock<IViewService> viewService;
        private Mock<IDialogService> dialogService;
        private Mock<IUnitOfWorkFactory> unitOfWorkFactory;
        private Mock<IMessenger> messenger;
        private Mock<IPakowanieViewModel> pakowanieViewModel;
        private Mock<ITowarGeowlokninaViewModel> geowlokninaViewModel;
        private Mock<ITowarGeokomorkaViewModel> geokomorkaViewModel;
        private ZamowienieOdKlientaViewModel viewModel;

        [SetUp]
        public void Setup()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            viewService = new Mock<IViewService>();
            dialogService = new Mock<IDialogService>();
            unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            messenger = new Mock<IMessenger>();

            pakowanieViewModel = new Mock<IPakowanieViewModel>();
            geowlokninaViewModel = new Mock<ITowarGeowlokninaViewModel>();
            geokomorkaViewModel = new Mock<ITowarGeokomorkaViewModel>();


            viewModel = new ZamowienieOdKlientaViewModel(unitOfWork.Object,
                                                        unitOfWorkFactory.Object,
                                                        dialogService.Object,
                                                        viewService.Object,
                                                        pakowanieViewModel.Object,
                                                        geowlokninaViewModel.Object,
                                                        geokomorkaViewModel.Object,
                                                        messenger.Object);

        }

        [Test]
        public void ViewModelInheritsFromViewModelBase_True()
        {
            var result = viewModel as ViewModelBase;

            Assert.IsNotNull(result);
        }

        [Test]
        public void ZapiszCommandCanExecute_PakowanieViewModelIsNotValid_False()
        {
            pakowanieViewModel.Setup(p => p.IsValid).Returns(false);
            geokomorkaViewModel.Setup(s => s.ListaPozycjiGeokomorek).Returns(new ObservableCollection<tblZamowienieHandloweTowarGeokomorka>());
            geowlokninaViewModel.Setup(s => s.ListaPozycjiGeowloknin).Returns(new ObservableCollection<tblZamowienieHandloweTowarGeowloknina>());
            var result = viewModel.ZapiszCommand.CanExecute(null);

            Assert.IsFalse(result);
        }
        [Test]
        public void ZapiszCommandCanExecute_TowarViewModelIsNotValid_False()
        {
            pakowanieViewModel.Setup(p => p.IsValid).Returns(true);
            geowlokninaViewModel.Setup(t => t.IsValid).Returns(false);
            geokomorkaViewModel.Setup(s => s.ListaPozycjiGeokomorek).Returns(new ObservableCollection<tblZamowienieHandloweTowarGeokomorka>());
            geowlokninaViewModel.Setup(s => s.ListaPozycjiGeowloknin).Returns(new ObservableCollection<tblZamowienieHandloweTowarGeowloknina>());

            var result = viewModel.ZapiszCommand.CanExecute(null);

            Assert.IsFalse(result);
        }

        [Test]
        public void ZapiszCommandCanExecute_ZamowienieIsNotValid_False()
        {
            pakowanieViewModel.Setup(p => p.IsValid).Returns(true);
            geowlokninaViewModel.Setup(t => t.IsValid).Returns(true);

            geokomorkaViewModel.Setup(s => s.ListaPozycjiGeokomorek).Returns(new ObservableCollection<tblZamowienieHandloweTowarGeokomorka>());
            geowlokninaViewModel.Setup(s => s.ListaPozycjiGeowloknin).Returns(new ObservableCollection<tblZamowienieHandloweTowarGeowloknina>());

            viewModel.Zamowienie.IsValid = false;
            var result = viewModel.ZapiszCommand.CanExecute(null);

            Assert.IsFalse(result);
        }
    }
}
