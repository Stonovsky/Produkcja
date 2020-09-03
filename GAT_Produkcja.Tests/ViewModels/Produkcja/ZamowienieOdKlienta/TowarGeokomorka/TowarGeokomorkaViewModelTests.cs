using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.EntityValidation;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Helpers.Geokomórka;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Produkcja.ZamowieniaOdKlienta.TowarGeokomorka;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.ZamowienieOdKlienta.TowarGeokomorka
{
    [TestFixture]
    public class TowarGeokomorkaViewModelTests
    {
        private Mock<IUnitOfWork> unitOfWork;
        private Mock<IUnitOfWorkFactory> unitOfWorkFactory;
        private Mock<IViewService> viewService;
        private Mock<IDialogService> dialogService;
        private Mock<IGeokomorkaHelper> geokomorkaHelper;
        private Mock<IMessenger> messenger;
        private IMessenger messengerOrg;
        private TowarGeokomorkaViewModel viewModel;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            viewService = new Mock<IViewService>();
            dialogService = new Mock<IDialogService>();
            geokomorkaHelper = new Mock<IGeokomorkaHelper>();
            messenger = new Mock<IMessenger>();
            messengerOrg = new Messenger();

            var tblZamowienieTowarGeokomorka = new Mock<ITblZamowienieHandloweTowarGeokomorkaRepository>();
            unitOfWork.Setup(s => s.tblZamowienieHandloweTowarGeokomorka).Returns(tblZamowienieTowarGeokomorka.Object);
            unitOfWork.Setup(s => s.tblZamowienieHandloweTowarGeokomorka
                                .WhereAsync(It.IsAny<Expression<Func<tblZamowienieHandloweTowarGeokomorka, bool>>>()))
                                .ReturnsAsync(new List<tblZamowienieHandloweTowarGeokomorka>()
                                {
                                    new tblZamowienieHandloweTowarGeokomorka() { IDZamowienieHandlowe = 1 }
                                });

            viewModel = PobierzViewModel(messenger.Object);
        }
        private TowarGeokomorkaViewModel PobierzViewModel(IMessenger messenger)
        {
            return new TowarGeokomorkaViewModel(unitOfWork.Object,
                                                viewService.Object,
                                                dialogService.Object,
                                                unitOfWorkFactory.Object,
                                                geokomorkaHelper.Object,
                                                messenger
                                                );
        }


        [Test]
        public void CTOR_Listy_NotNull()
        {
            Assert.IsNotNull(viewModel.ListaZgrzewow);
            Assert.IsNotNull(viewModel.ListaPozycjiGeokomorek);
            Assert.IsNotNull(viewModel.ListaPrametrowGeometrycznychGeokomorki);
            Assert.IsNotNull(viewModel.ListaRodzajowGeokomorek);
            Assert.IsNotNull(viewModel.ListaTypowGeokomorek);
        }


        [Test]
        public void UsunCommandCanExecute_ListaPusta_False()
        {
            Assert.IsFalse(viewModel.UsunCommand.CanExecute(null));
        }
        [Test]
        public void UsunCommandCanExecute_WybranaPozycjaGeokomorkiIsNull_False()
        {
            viewModel.WybranaPozycjaGeokomorki = null;
            Assert.IsFalse(viewModel.UsunCommand.CanExecute(null));


        }

        [Test]
        public void UsunCommandExecute_DialogBoxTrue_RemoveMethodIsInvoked()
        {
            //Arrange
            dialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>()))
                         .Returns(true);

            var zamowienieTowarGeokomorka = new Mock<ITblZamowienieHandloweTowarGeokomorkaRepository>();
            unitOfWork.Setup(s => s.tblZamowienieHandloweTowarGeokomorka)
                      .Returns(zamowienieTowarGeokomorka.Object);

            viewModel.ListaPozycjiGeokomorek.Add(new tblZamowienieHandloweTowarGeokomorka() { IDZamowienieHandloweTowarGeokomorka = 1 });

            //Act
            viewModel.UsunCommand.Execute(null);
            //Assert
            unitOfWork.Verify(s => s.tblZamowienieHandloweTowarGeokomorka.Remove(It.IsAny<tblZamowienieHandloweTowarGeokomorka>()));
        }

        [Test]
        public void UsunCommandExecute_DialogBoxFalse_RemoveMethodIsNOTInvoked()
        {
            //Arrange
            dialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>()))
                         .Returns(false);

            var zamowienieTowarGeokomorka = new Mock<ITblZamowienieHandloweTowarGeokomorkaRepository>();
            unitOfWork.Setup(s => s.tblZamowienieHandloweTowarGeokomorka)
                      .Returns(zamowienieTowarGeokomorka.Object);

            viewModel.ListaPozycjiGeokomorek.Add(new tblZamowienieHandloweTowarGeokomorka() { IDZamowienieHandloweTowarGeokomorka = 1 });
            //Act
            viewModel.UsunCommand.Execute(null);

            //Assert
            unitOfWork.Verify(s => s.tblZamowienieHandloweTowarGeokomorka.Remove(It.IsAny<tblZamowienieHandloweTowarGeokomorka>()), Times.Never);
        }

        [Test]
        public void IsValid_ListaPozycjiGeokomorekIsZero_True()
        {
            viewModel.PoEdycjiKomorkiCommand.Execute(null);
            Assert.IsTrue(viewModel.IsValid);
        }

        [Test]
        public void IsValid_ListaPozycjiGeokomorekIsGreaterThanZeroAndPositionIsNotValid_False()
        {
            viewModel.ListaPozycjiGeokomorek.Add(new tblZamowienieHandloweTowarGeokomorka() { IDZamowienieHandloweTowarGeokomorka = 1 });

            //WybranaPozycja_GenerujNazwePelna();
            //viewModel = PobierzViewModel();

            viewModel.PoEdycjiKomorkiCommand.Execute(null);

            Assert.IsFalse(viewModel.IsValid);
        }

        [Test]
        public void GdyPrzeslanoZapisz_ListaPozycjiGeokomorekIsZero_UnitOfWorkMethodsNotInvoked()
        {
            var tblZamowienieHandloweTowarGeokomorka = new Mock<ITblZamowienieHandloweTowarGeokomorkaRepository>();
            unitOfWork.Setup(s => s.tblZamowienieHandloweTowarGeokomorka).Returns(tblZamowienieHandloweTowarGeokomorka.Object);

            Messenger.Default.Send("ZapiszTowar", "ZapiszTowar");

            unitOfWork.Verify(u => u.tblZamowienieHandloweTowarGeokomorka.Add(It.IsAny<tblZamowienieHandloweTowarGeokomorka>()), Times.Never);
            unitOfWork.Verify(u => u.SaveAsync(), Times.Never);
        }

        [Test]
        public void GdyPrzeslanoZapisz_ListaPozycjiGeokomorekIsNotZero_UnitOfWorkMethodsInvoked()
        {
            var tblZamowienieHandloweTowarGeokomorka = new Mock<ITblZamowienieHandloweTowarGeokomorkaRepository>();
            unitOfWork.Setup(s => s.tblZamowienieHandloweTowarGeokomorka).Returns(tblZamowienieHandloweTowarGeokomorka.Object);
            viewModel.ListaPozycjiGeokomorek.Add(new tblZamowienieHandloweTowarGeokomorka() { IDZamowienieHandloweTowarGeokomorka = 0 });

            viewModel = PobierzViewModel(messengerOrg);

            messengerOrg.Send(new tblZamowienieHandlowe() { IDZamowienieHandlowe = 1 }, "PrzeslaneZamowienie");
            viewModel.ListaPozycjiGeokomorek.Add(new tblZamowienieHandloweTowarGeokomorka() { IDZamowienieHandloweTowarGeokomorka = 0 });
            messengerOrg.Send(new tblZamowienieHandlowe() { IDZamowienieHandlowe = 1 }, "ZapiszTowar");

            unitOfWork.Verify(u => u.tblZamowienieHandloweTowarGeokomorka.Add(It.IsAny<tblZamowienieHandloweTowarGeokomorka>()));
            unitOfWork.Verify(u => u.SaveAsync());
        }
        [Test]
        public void GdyPrzeslanoZamowinieHandlowe_DodawanaPozycjaPowinnaMiecIDZamowienieHandloweJakWMessenger_True()
        {
            var tblZamowienieHandloweTowarGeokomorka = new Mock<ITblZamowienieHandloweTowarGeokomorkaRepository>();
            unitOfWork.Setup(s => s.tblZamowienieHandloweTowarGeokomorka).Returns(tblZamowienieHandloweTowarGeokomorka.Object);

            viewModel = PobierzViewModel(messengerOrg);

            messengerOrg.Send(new tblZamowienieHandlowe() { IDZamowienieHandlowe = 1 }, "PrzeslaneZamowienie");
            viewModel.ListaPozycjiGeokomorek.Add(new tblZamowienieHandloweTowarGeokomorka() { IDZamowienieHandloweTowarGeokomorka = 0 });
            messengerOrg.Send(new tblZamowienieHandlowe() { IDZamowienieHandlowe = 1 }, "ZapiszTowar");
            //var pozycjaIdZamowienie = viewModel.ListaPozycjiGeokomorek[0].IDZamowienieHandlowe;

            unitOfWork.Verify(u => u.tblZamowienieHandloweTowarGeokomorka.Add(It.Is<tblZamowienieHandloweTowarGeokomorka>(p => p.IDZamowienieHandlowe == 1)));

            //Assert.IsTrue(pozycjaIdZamowienie == 1);
        }
    }
}
