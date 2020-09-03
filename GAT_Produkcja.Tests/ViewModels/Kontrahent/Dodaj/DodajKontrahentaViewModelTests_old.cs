using AutoFixture;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.UI.Utilities.WebScraper;
using GAT_Produkcja.ViewModel.Kontrahent.Dodaj;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Kontrahent.Dodaj
{
    [TestFixture]
    public class DodajKontrahentaViewModelTests_old
    {
        private Mock<IDialogService> dialogService;
        private Mock<IViewService> viewService;
        private Mock<IUnitOfWork> unitOfWork;
        private Mock<IMessenger> messenger;
        private Fixture fixture;
        private Mock<IPobierzDaneKontrahentaZGUS> pobierzKontrahentaZGUS;
        private DodajKontrahentaViewModel_old viewModel;

        [SetUp]
        public void SetUp()
        {
            dialogService = new Mock<IDialogService>();
            viewService = new Mock<IViewService>();
            unitOfWork = new Mock<IUnitOfWork>();
            messenger = new Mock<IMessenger>();

            fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                            .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            
            pobierzKontrahentaZGUS = new Mock<IPobierzDaneKontrahentaZGUS>();

            viewModel = PobierzViewModel();
        }

        private DodajKontrahentaViewModel_old PobierzViewModel()
        {
            return new DodajKontrahentaViewModel_old(unitOfWork.Object,
                                                            dialogService.Object,
                                                            viewService.Object,
                                                            pobierzKontrahentaZGUS.Object,
                                                            messenger.Object
                                                            );
        }

        [Test]
        public void ZapiszCommandCanExecute_KontrahentIsNotValid_False()
        {
            viewModel.Kontrahent.IsValid = false;

            Assert.IsFalse(viewModel.ZapiszCommand.CanExecute(null));
        }

        private tblKontrahent GetValidKontrahent()
        {
            return new tblKontrahent
            {
                Nazwa = "test",
                NIP = "PL1234567891",
                Ulica = "test",
                Miasto = "Test",
                KodPocztowy = "Test"
            };
        }

        [Test]
        public void ZapiszCommandExecute_KontrahentIDisZeroAndKontrahentNieWystWBazie_InvokeUnitOfWorkAddMethod()
        {
            //Arrange
            GetValidKontrahent();
            var kontrahent = new Mock<ITblKontrahentRepository>();
            unitOfWork.Setup(s => s.tblKontrahent).Returns(kontrahent.Object);

            viewModel = PobierzViewModel();
            viewModel.Kontrahent = GetValidKontrahent();

            //Act
            viewModel.ZapiszCommand.Execute(null);

            //Assert
            unitOfWork.Verify(s => s.tblKontrahent.Add(It.IsAny<tblKontrahent>()));
        }

        [Test]
        public void ZapiszCommandExecute_KontrahentIstnieje_InvokeSaveAsync()
        {
            viewModel.Kontrahent = GetValidKontrahent();
            viewModel.Kontrahent.ID_Kontrahent = 1;

            //Act
            viewModel.ZapiszCommand.Execute(null);

            unitOfWork.Verify(s => s.SaveAsync());
        }
        [Test]
        public void UsunCommandCanExecute_KontrahendIdIsNotZero_True()
        {
            viewModel.Kontrahent.ID_Kontrahent = 1;

            Assert.IsTrue(viewModel.UsunCommand.CanExecute(null));
        }
        [Test]
        public void UsunCommanCanExecute_GdyEdytowanoKontrahentaBrakMozliwosciUsuniecia_False()
        {
            unitOfWork.Setup(s => s.tblKontrahent.GetByIdAsync(It.IsAny<int>()))
                      .ReturnsAsync(new tblKontrahent() { ID_Kontrahent = 1, Nazwa = "test", NIP = "test" });

            viewModel = PobierzViewModel();
            Messenger.Default.Send(new tblKontrahent() { ID_Kontrahent = 1, Nazwa = "test1", NIP = "test1" });

            Assert.IsFalse(viewModel.UsunCommand.CanExecute(null));
        }

        [Test]
        public void UsunCommanCanExecute_KontrahentIdIsZero_Fasle()
        {
            viewModel.Kontrahent.ID_Kontrahent = 0;

            Assert.IsFalse(viewModel.UsunCommand.CanExecute(null));
        }

    }
}
