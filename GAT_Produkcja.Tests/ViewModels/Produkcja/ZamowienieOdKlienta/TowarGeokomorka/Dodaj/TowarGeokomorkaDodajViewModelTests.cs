using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Produkcja.ZamowieniaOdKlienta.TowarGeokomorka.Dodaj;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.Helpers.Geokomórka;
using AutoFixture;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.ZamowienieOdKlienta.TowarGeokomorka.Dodaj
{
    [TestFixture]
    class TowarGeokomorkaDodajViewModelTests
    {
        private Mock<IUnitOfWork> unitOfWork;
        private Mock<IUnitOfWorkFactory> unitOfWorkFactory;
        private Mock<IViewService> viewService;
        private Mock<IDialogService> dialogService;
        private Mock<IMessenger> messenger;
        private IMessenger messengerOrg;
        private Mock<IGeokomorkaHelper> geokomorkaHelper;
        private Mock<ITblZamowienieHandloweTowarGeokomorkaRepository> tblZamowienieHandloweTowarGeokomorka;
        private Mock<ITblTowarGeokomorkaParametryGeometryczneRepository> tblTowarGeokomorkaParametryGeometryczne;
        private Fixture fixture;
        private TowarGeokomorkaDodajViewModel viewModel;

        #region SetUp
        [SetUp]
        public void SetUp()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            viewService = new Mock<IViewService>();
            dialogService = new Mock<IDialogService>();
            messenger = new Mock<IMessenger>();
            messengerOrg = new Messenger();
            geokomorkaHelper = new Mock<IGeokomorkaHelper>();
            tblZamowienieHandloweTowarGeokomorka = new Mock<ITblZamowienieHandloweTowarGeokomorkaRepository>();
            tblTowarGeokomorkaParametryGeometryczne = new Mock<ITblTowarGeokomorkaParametryGeometryczneRepository>();
            fixture = new Fixture();
            unitOfWork.Setup(s => s.tblZamowienieHandloweTowarGeokomorka)
                        .Returns(tblZamowienieHandloweTowarGeokomorka.Object);

            unitOfWork.Setup(s => s.tblTowarGeokomorkaParametryGeometryczne)
                        .Returns(tblTowarGeokomorkaParametryGeometryczne.Object);


            viewModel = PobierzViewModel(messenger.Object);

            GenerujListy();
            GenerujMockiDlaGeokomorkaHelper();

            //viewModel.WybranyTowar = new tblTowar() { IDTowar = 1, Nazwa = "test" };
        }
        private void GenerujMockiDlaGeokomorkaHelper()
        {
            geokomorkaHelper.Setup(s => s.ListaParametrowGeometrycznych).Returns(new List<tblTowarGeokomorkaParametryGeometryczne>()
            {
                new tblTowarGeokomorkaParametryGeometryczne(){IDTowarGeokomorkaParametryGeometryczne=1}
            });

            geokomorkaHelper.Setup(s => s.PobierzZgrzewZNazwy(It.IsAny<string>())).Returns(new tblTowarGeokomorkaParametryZgrzew() { IDTowarGeokomorkaParametryZgrzew = 1 });
            geokomorkaHelper.Setup(s => s.PobierzRodzajZNazwy(It.IsAny<string>())).Returns(new tblTowarGeokomorkaParametryRodzaj() { IDTowarGeokomorkaParametryRodzaj = 1 });
            geokomorkaHelper.Setup(s => s.PobierzTypZNazwy(It.IsAny<string>())).Returns(new tblTowarGeokomorkaParametryTyp() { IDTowarGeokomorkaParametryTyp = 1 });


        }
        private tblZamowienieHandloweTowarGeokomorka CreateGeokomorka()
        {
            var geokomorka = fixture.Build<tblZamowienieHandloweTowarGeokomorka>()
                .Without(w => w.tblTowar)
                .Without(w => w.tblTowarGeokomorkaParametryRodzaj)
                .Without(w => w.tblTowarGeokomorkaParametryTyp)
                .Without(w => w.tblTowarGeokomorkaParametryZgrzew)
                .Without(w => w.tblZamowienieHandlowe)
                .Create();
            
            geokomorka.Ilosc_m2 = 10M;

            return geokomorka;
        }
        private void GenerujListy()
        {
            viewModel.ListaPrametrowGeometrycznychGeokomorki = new List<tblTowarGeokomorkaParametryGeometryczne>()
            {
                new tblTowarGeokomorkaParametryGeometryczne(){IDTowarGeokomorkaParametryGeometryczne=1, DlugoscStandardowaSekcji_m=6.651M, SzerokoscStandardowaSekcji_m=3.5M},
                new tblTowarGeokomorkaParametryGeometryczne(){IDTowarGeokomorkaParametryGeometryczne=1, DlugoscStandardowaSekcji_m=6.632M, SzerokoscStandardowaSekcji_m=3.5M},
                new tblTowarGeokomorkaParametryGeometryczne(){IDTowarGeokomorkaParametryGeometryczne=1, DlugoscStandardowaSekcji_m=6.6546M, SzerokoscStandardowaSekcji_m=3.5M}
            };

            viewModel.ListaRodzajow = new List<tblTowarGeokomorkaParametryRodzaj>()
            {
                new tblTowarGeokomorkaParametryRodzaj(){IDTowarGeokomorkaParametryRodzaj=1}
            };

            viewModel.ListaTypow = new List<tblTowarGeokomorkaParametryTyp>()
            {
                new tblTowarGeokomorkaParametryTyp(){IDTowarGeokomorkaParametryTyp=1}
            };

            viewModel.ListaZgrzewow = new List<tblTowarGeokomorkaParametryZgrzew>()
            {
                new tblTowarGeokomorkaParametryZgrzew(){IDTowarGeokomorkaParametryZgrzew=1}
            };
        }

        private TowarGeokomorkaDodajViewModel PobierzViewModel(IMessenger messenger)
        {
            return new TowarGeokomorkaDodajViewModel(
                                                    unitOfWork.Object,
                                                    unitOfWorkFactory.Object,
                                                    dialogService.Object,
                                                    viewService.Object,
                                                    geokomorkaHelper.Object,
                                                    messenger
                                                    );
        }

        #endregion

        [Test]
        public void GdyPrzeslanoPozycjeGeokomorki_WhenEntityIsReceived_UOWGetByIdAsyncIsInvoked2()
        {

        }

        [Test]
        public void GdyPrzeslanoPozycjeGeokomorki_WhenEntityIsReceived_UOWGetByIdAsyncIsInvoked()
        {
            viewModel = PobierzViewModel(messengerOrg);
            messengerOrg.Send(new tblZamowienieHandloweTowarGeokomorka() { IDZamowienieHandloweTowarGeokomorka = 1 });

            unitOfWork.Verify(u => u.tblZamowienieHandloweTowarGeokomorka.GetByIdAsync(It.IsAny<int>()));

        }
        [Test]
        public void ZapiszCommandCanExecute_GdyGeokomorkaIsNull_False()
        {

            Assert.IsFalse(viewModel.ZapiszCommand.CanExecute(null));
        }

        [Test]
        public void ZapiszCommandCanExecute_GdyGeokomorkaIsNotValid_False()
        {
            viewModel.Geokomorka.IsValid = false;

            Assert.IsFalse(viewModel.ZapiszCommand.CanExecute(null));
        }

        [Test]
        public void ZapiszCommandExecute_GeokomorkaIDisZero_MessengerSendIsInvoked()
        {
            viewModel.Geokomorka = CreateGeokomorka();
            viewModel.Geokomorka.Ilosc_m2 = 10M;
            viewModel.Geokomorka.IDZamowienieHandloweTowarGeokomorka = 0;
            viewModel.Geokomorka.IDTowarGeokomorkaParametryRodzaj = 1;

            viewModel.ZapiszCommand.Execute(null);

            messenger.Verify(v => v.Send(It.IsAny<tblZamowienieHandloweTowarGeokomorka>(),"Zapisz"));

        }
        [Test]
        public void ZapiszCommandExecute_GeokomorkaIdIsNot0_MessengerSendIsInvoked()
        {
            viewModel.Geokomorka = CreateGeokomorka();
            viewModel.Geokomorka.Ilosc_m2 = 10M;

            viewModel.Geokomorka.IDZamowienieHandloweTowarGeokomorka = 1;

            viewModel.ZapiszCommand.Execute(null);

            messenger.Verify(v => v.Send(It.IsAny<tblZamowienieHandloweTowarGeokomorka>(), "Zapisz"));
        }


        [Test]
        public void UsunCommandCanExecute_GeokomorkaIdIsZero_True()
        {
            Assert.IsFalse(viewModel.UsunCommand.CanExecute(null));
        }

        [Test]
        public void UsunCommandExecute_DialogServiceFalse_MessengerSendMethodIsNotInvoked()
        {
            dialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>()))
                            .Returns(false);
            viewModel.Geokomorka.IDZamowienieHandloweTowarGeokomorka = 1;

            tblZamowienieHandloweTowarGeokomorka actual = new tblZamowienieHandloweTowarGeokomorka();
            Messenger.Default.Register<tblZamowienieHandloweTowarGeokomorka>(this, "Usun", t => actual = t);

            viewModel.UsunCommand.Execute(null);

            tblZamowienieHandloweTowarGeokomorka expected = new tblZamowienieHandloweTowarGeokomorka() { IDZamowienieHandloweTowarGeokomorka = 1 };

            Assert.AreNotEqual(expected.IDZamowienieHandloweTowarGeokomorka, actual.IDZamowienieHandloweTowarGeokomorka);
        }

        [Test]
        public void UsunCommandExecute_DialogServiceTrue_MessengerSendMethodIsInvoked()
        {
            dialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>()))
                            .Returns(true);
            viewModel.Geokomorka.IDZamowienieHandloweTowarGeokomorka = 1;

            viewModel.UsunCommand.Execute(null);

            messenger.Verify(v => v.Send(It.IsAny<tblZamowienieHandloweTowarGeokomorka>(), "Usun"));
        }

        [Test]
        public void WybranyZgrzewChanged_CzyPobieraWlasciweDane_True()
        {
            //arrange
            geokomorkaHelper.Setup(s => s.PobierzStandardowaSzerokoscSekcjiZNazwy_m(It.IsAny<string>())).Returns(1);
            geokomorkaHelper.Setup(s => s.PobierzStandardowaDlugoscSekcjiZNazwy_m(It.IsAny<string>())).Returns(1);
            viewModel.ListaPrametrowGeometrycznychGeokomorki.Add(new tblTowarGeokomorkaParametryGeometryczne()
            {
                IDTowarParametryGeokomorkaZgrzew = 1,
                SzerokoscStandardowaSekcji_m = 1,
                DlugoscStandardowaSekcji_m = 1
            });
            viewModel.Geokomorka.IDTowarGeokomorkaParametryZgrzew = 1;
            viewModel.WybranyTowar = new tblTowar();
            //act
            viewModel.WybranyZgrzew = new tblTowarGeokomorkaParametryZgrzew() { IDTowarGeokomorkaParametryZgrzew = 1 };
            viewModel.RaisePropertyChanged("WybranyZgrzew");

            //assert
            Assert.IsTrue(viewModel.Geokomorka.DlugoscSekcji_mm == 1 * 1000); //przeliczenie dl i szer. na [mm]
            Assert.IsTrue(viewModel.Geokomorka.SzerokoscSekcji_mm == 1 * 1000);
        }

        [Test]
        public void WhenWybranyTowarChanged_GenerujPelneDaneIsInvoked()
        {

            viewModel.WybranyTowar = new tblTowar() { IDTowar = 1 };

            geokomorkaHelper.Verify(v => v.PobierzStandardowaSzerokoscSekcjiZNazwy_m(It.IsAny<string>()));
        }

        [Test]
        public void WhenWybranyZgrzewChanged_GenerujPelneDaneIsInvoked()
        {

            viewModel.WybranyZgrzew = new tblTowarGeokomorkaParametryZgrzew() { IDTowarGeokomorkaParametryZgrzew = 1 };

            geokomorkaHelper.Verify(v => v.PobierzStandardowaSzerokoscSekcjiZNazwy_m(It.IsAny<string>()));
        }

        [Test]
        public void WhenWybranyRodzajChanged_GenerujPelneDaneIsInvoked()
        {

            viewModel.WybranyRodzaj = new tblTowarGeokomorkaParametryRodzaj() { IDTowarGeokomorkaParametryRodzaj = 1 };

            geokomorkaHelper.Verify(v => v.PobierzStandardowaSzerokoscSekcjiZNazwy_m(It.IsAny<string>()));
        }

        [Test]
        public void WhenWybranyTypChanged_GenerujPelneDaneIsInvoked()
        {

            viewModel.WybranyTyp = new tblTowarGeokomorkaParametryTyp() { IDTowarGeokomorkaParametryTyp = 1 };

            geokomorkaHelper.Verify(v => v.PobierzStandardowaSzerokoscSekcjiZNazwy_m(It.IsAny<string>()));
        }
        [Test]
        public void GnerujPelneDane_InvokeAllMethods()
        {
            viewModel.WybranyTowar = new tblTowar() { IDTowar = 1, Nazwa = "test" };
            viewModel.WybranyTyp = new tblTowarGeokomorkaParametryTyp() { IDTowarGeokomorkaParametryTyp = 1 };

            geokomorkaHelper.Verify(v => v.PobierzStandardowaSzerokoscSekcjiZNazwy_m(It.IsAny<string>()));
            geokomorkaHelper.Verify(v => v.PobierzStandardowaDlugoscSekcjiZNazwy_m(It.IsAny<string>()));
            geokomorkaHelper.Verify(v => v.PobierzRodzajZNazwy(It.IsAny<string>()));
            geokomorkaHelper.Verify(v => v.PobierzTypZNazwy(It.IsAny<string>()));
            geokomorkaHelper.Verify(v => v.PobierzZgrzewZNazwy(It.IsAny<string>()));
            geokomorkaHelper.Verify(v => v.PobierzWysokoscZNazwy(It.IsAny<string>()));
        }

        [Test]
        [TestCase(1,3500)]
        [TestCase(2,3500)]
        [TestCase(3,3500)]
        public void GenerujPelneDane_SprawdzaPobieranieSzerokosci(int idTowar, int szerokosc)
        {
            viewModel.ListaTowarow = new List<tblTowar>()
            {
                new tblTowar() { IDTowar = 1, Nazwa = "Geokomórka AT CELL Comfort 001.100" },
                new tblTowar() { IDTowar = 2, Nazwa = "Geokomórka AT CELL Comfort 002.100" },
                new tblTowar() { IDTowar = 3, Nazwa = "Geokomórka AT CELL Comfort 003.100" },
            };

            geokomorkaHelper.Setup(s => s.PobierzStandardowaSzerokoscSekcjiZNazwy_m(It.IsAny<string>())).Returns(3.5M);

            viewModel.WybranyTowar = viewModel.ListaTowarow.SingleOrDefault(s => s.IDTowar == idTowar);
            viewModel.WybranyRodzaj = viewModel.ListaRodzajow.SingleOrDefault(s => s.IDTowarGeokomorkaParametryRodzaj == 1);

            Assert.AreEqual(szerokosc, viewModel.Geokomorka.SzerokoscSekcji_mm);
        }
        [Test]
        [TestCase(1, 6651)]
        [TestCase(2, 6632)]
        [TestCase(3, 6654)]
        public void GenerujPelneDane_SprawdzaPobieranieDlugosci(int idTowar, int dlugosc)
        {
            viewModel.ListaTowarow = new List<tblTowar>()
            {
                new tblTowar() { IDTowar = 1, Nazwa = "Geokomórka AT CELL Comfort 001.100" },
                new tblTowar() { IDTowar = 2, Nazwa = "Geokomórka AT CELL Comfort 002.100" },
                new tblTowar() { IDTowar = 3, Nazwa = "Geokomórka AT CELL Comfort 003.100" },
            };

            geokomorkaHelper.Setup(s => s.PobierzStandardowaDlugoscSekcjiZNazwy_m("Geokomórka AT CELL Comfort 001.100")).Returns(6.651M);
            geokomorkaHelper.Setup(s => s.PobierzStandardowaDlugoscSekcjiZNazwy_m("Geokomórka AT CELL Comfort 002.100")).Returns(6.632M);
            geokomorkaHelper.Setup(s => s.PobierzStandardowaDlugoscSekcjiZNazwy_m("Geokomórka AT CELL Comfort 003.100")).Returns(6.654M);

            viewModel.WybranyTowar = viewModel.ListaTowarow.SingleOrDefault(s => s.IDTowar == idTowar);
            viewModel.WybranyRodzaj = viewModel.ListaRodzajow.SingleOrDefault(s => s.IDTowarGeokomorkaParametryRodzaj == 1);

            Assert.AreEqual(dlugosc, viewModel.Geokomorka.DlugoscSekcji_mm);
        }

        [Test]
        public void ObliczIloscM2iIloscSekcji_InvokeMethodsWhenGeokomorkaChanged()
        {
            viewModel.Geokomorka.Ilosc_m2 = 25M;
            viewModel.Geokomorka = new tblZamowienieHandloweTowarGeokomorka() { IDZamowienieHandloweTowarGeokomorka = 1, Ilosc_m2 = 1M };


            geokomorkaHelper.Verify(v => v.ObliczIloscSekcji(It.IsAny<string>(), It.IsAny<decimal>()));
            geokomorkaHelper.Verify(v => v.ObliczIloscM2ZgodnaZPowierzchniaSekcji(It.IsAny<string>(), It.IsAny<decimal>()));
        }

    }
}
