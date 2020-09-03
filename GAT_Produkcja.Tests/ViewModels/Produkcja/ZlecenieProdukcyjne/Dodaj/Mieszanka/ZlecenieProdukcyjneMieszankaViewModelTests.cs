using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Magazyn.Ewidencje.StanTowaru;
using GAT_Produkcja.ViewModel.Magazyn.Towar.Ewidencja;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Mieszanka;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Naglowek;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Messages;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.ZlecenieProdukcyjne.ZlecenieProdukcyjneMieszanka
{
    [TestFixture]
    public class ZlecenieProdukcyjneMieszankaViewModelTests
    {

        private Mock<IUnitOfWork> unitOfWork;
        private Mock<IUnitOfWorkFactory> unitOfWorkFactory;
        private Mock<IViewService> viewService;
        private Mock<IDialogService> dialogService;
        private Mock<IMessenger> messenger;
        private IMessenger messengerOrg;
        private Mock<ITblProdukcjaZlecenieRepository> tblProdukcjaZlcecenieProdukcyjne;
        private Mock<ITblProdukcjaZlecenieProdukcyjne_MieszankaRepository> tblProdukcjaZlecenieProdukcyjne_Mieszanka;
        private Mock<ITblTowarRepository> tblTowar;
        private Mock<IVwTowarGTXRepository> vwTowarGTX;
        private ZlecenieProdukcyjneMieszankaViewModel sut;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            viewService = new Mock<IViewService>();
            dialogService = new Mock<IDialogService>();
            messenger = new Mock<IMessenger>();
            messengerOrg = new Messenger();

            tblProdukcjaZlcecenieProdukcyjne = new Mock<ITblProdukcjaZlecenieRepository>();
            unitOfWork.Setup(s => s.tblProdukcjaZlecenie).Returns(tblProdukcjaZlcecenieProdukcyjne.Object);

            tblProdukcjaZlecenieProdukcyjne_Mieszanka = new Mock<ITblProdukcjaZlecenieProdukcyjne_MieszankaRepository>();
            unitOfWork.Setup(s => s.tblProdukcjaZlecenieProdukcyjne_Mieszanka).Returns(tblProdukcjaZlecenieProdukcyjne_Mieszanka.Object);

            tblTowar = new Mock<ITblTowarRepository>();
            unitOfWork.Setup(s => s.tblTowar).Returns(tblTowar.Object);

            vwTowarGTX = new Mock<IVwTowarGTXRepository>();
            unitOfWork.Setup(s => s.vwTowarGTX).Returns(vwTowarGTX.Object);


            sut = CreateSut(messenger.Object);
            sut.ListaPozycjiMieszanki = new ObservableCollection<tblProdukcjaZlecenieProdukcyjne_Mieszanka>();
        }

        private ZlecenieProdukcyjneMieszankaViewModel CreateSut(IMessenger messenger)
        {
            return new ZlecenieProdukcyjneMieszankaViewModel(unitOfWork.Object, viewService.Object, dialogService.Object, messenger);
        }

        #region Messenger
        [Test]
        public void IsMessengerRegistered()
        {
            messenger.Verify(v => v.Register(sut, It.IsAny<Action<vwMagazynRuchGTX>>(), It.IsAny<bool>()));
            messenger.Verify(v => v.Register<ZmianaIlosciMieszankiMessage>(sut, It.IsAny<Action<ZmianaIlosciMieszankiMessage>>(), It.IsAny<bool>()));
        }

        #region GdyWyslanoSurowiec
        [Test]
        public void GdyWyslanoSurowiec_GdySurowiecNull_NieTworzWybranejPozycjiMieszanki()
        {
            sut = CreateSut(messengerOrg);

            Assert.IsNull(sut.WybranaPozycjaMieszanki);
        }

        [Test]
        public void GdyWyslanoSurowiec_GdySurowiecNieNull_PrzypiszCeneZakupuDoWybranejPozycjiMieszanki()
        {
            sut = CreateSut(messengerOrg);
            
            messengerOrg.Send(new vwMagazynRuchGTX
            {
                Cena = 1
            });

            Assert.AreEqual(1, sut.WybranaPozycjaMieszanki.Cena_kg);
        }

        [Test]
        public void GdyWyslanoSurowiec_GdyPrzeslanoSurowiecKtoryJuzJestNaLiscie_WyswietlDialogService()
        {
            sut = CreateSut(messengerOrg);
            sut.ListaPozycjiMieszanki = new ObservableCollection<tblProdukcjaZlecenieProdukcyjne_Mieszanka>
            {
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka{IDZlecenieProdukcyjneMieszanka=1, IDTowar=1},
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka{IDZlecenieProdukcyjneMieszanka=2, IDTowar=2}
            };

            messengerOrg.Send(new vwMagazynRuchGTX { IdTowar = 1 });

            dialogService.Verify(v => v.ShowError_BtnOK(It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        public void GdyWyslanoSurowiec_GdyPrzeslanoSurowiecKtoryJuzJestNaLiscie_NieDodawajDoListy()
        {
            sut = CreateSut(messengerOrg);
            sut.ListaPozycjiMieszanki = new ObservableCollection<tblProdukcjaZlecenieProdukcyjne_Mieszanka>
            {
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka{IDZlecenieProdukcyjneMieszanka=1, IDTowar=1},
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka{IDZlecenieProdukcyjneMieszanka=2, IDTowar=2}
            };

            messengerOrg.Send(new vwMagazynRuchGTX { IdTowar = 1 });

            Assert.AreEqual(2, sut.ListaPozycjiMieszanki.Count());
        }


        [Test]
        public void GdyWyslanoSurowiec_GdyPrzeslanoSurowiecKtoregoNieMaNaLiscie_DodajDoListy()
        {
            sut = CreateSut(messengerOrg);
            sut.ListaPozycjiMieszanki = new ObservableCollection<tblProdukcjaZlecenieProdukcyjne_Mieszanka>
            {
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka{IDZlecenieProdukcyjneMieszanka=1, IDTowar=1},
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka{IDZlecenieProdukcyjneMieszanka=2, IDTowar=2}
            };

            messengerOrg.Send(new vwMagazynRuchGTX { IdTowar = 3 });

            Assert.AreEqual(3, sut.ListaPozycjiMieszanki.Count());
        }

        #endregion

        #endregion

        #region Properties
        [Test]
        public void SumarycznaIloscMieszanki_GdyZmiana_PrzeliczCenyIIlosciMieszanki()
        {
            sut.ListaPozycjiMieszanki = new ObservableCollection<tblProdukcjaZlecenieProdukcyjne_Mieszanka>
            {
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka{IDTowar=1, ZawartoscProcentowa=0.5m, Cena_kg=2, IloscKg=1 },
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka{IDTowar=2, ZawartoscProcentowa=0.5m, Cena_kg=4, IloscKg=1 },
            };

            sut.SumarycznaIloscMieszanki = 2;

            Assert.AreEqual(6, sut.ZlcecenieProdukcyjne.WartoscMieszanki_zl);
            Assert.AreEqual(3, sut.ZlcecenieProdukcyjne.CenaMieszanki_zl);
        }

        [Test]
        public void SumarycznaIloscMieszanki_GdyZmiana_WyslijPodsumowanieMieszankiMessengerem()
        {
            sut.ListaPozycjiMieszanki = new ObservableCollection<tblProdukcjaZlecenieProdukcyjne_Mieszanka>
            {
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka{IDTowar=1, ZawartoscProcentowa=0.5m, Cena_kg=2, IloscKg=1 },
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka{IDTowar=2, ZawartoscProcentowa=0.5m, Cena_kg=4, IloscKg=1 },
            };

            sut.SumarycznaIloscMieszanki = 2;

            messenger.Verify(v => v.Send<tblProdukcjaZlecenie, ZlecenieProdukcyjneNaglowekViewModel_old>(sut.ZlcecenieProdukcyjne));
        }


        #endregion

        #region Validation

        [Test]
        public void tblZlecenieProdukcyjneMieszanka_WhenAllNeededFieldsAreFilled_IsValid()
        {
            var mieszanka = new tblProdukcjaZlecenieProdukcyjne_Mieszanka
            {
                IDTowar = 1,
                ZawartoscProcentowa = 0.5M,
                IloscMieszanki_kg = 1200,
                IDJm = 7,
                NazwaTowaru = "test",
                JmNazwa = "test"
            };

            Assert.IsTrue(mieszanka.IsValid);
        }
        [Test]
        public void tblZlecenieProdukcyjneMieszanka_WhenNotAllNeededFieldsAreFilled_IsNotValid()
        {
            var mieszanka = new tblProdukcjaZlecenieProdukcyjne_Mieszanka();
  

            Assert.IsFalse(mieszanka.IsValid);
        }
        [Test]
        public void IsValid_ListIsEmpty_False()
        {
            Assert.IsFalse(sut.IsValid);
        }
        [Test]
        public void IsValid_ListIsNull_False()
        {
            sut.ListaPozycjiMieszanki = null;
            
            Assert.IsFalse(sut.IsValid);
        }

        [Test]
        public void IsValid_ListIsNotEmpty_But_ItemIsNotValid_False()
        {
            sut.ListaPozycjiMieszanki = new ObservableCollection<tblProdukcjaZlecenieProdukcyjne_Mieszanka>
            {
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka{ IsValid=false},
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka {IsValid=true}
            };

            Assert.IsFalse(sut.IsValid);
        }

        [Test]
        public void IsValid_ListIsNotEmpty_AllItemsAreValid_ZawartoscProcentowaIsOne_True()
        {
            sut.ListaPozycjiMieszanki = new ObservableCollection<tblProdukcjaZlecenieProdukcyjne_Mieszanka>
            {
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka
                {
                    IDTowar = 1,
                    ZawartoscProcentowa = 0.5M,
                    IloscMieszanki_kg = 1200,
                    IDJm = 7,
                    NazwaTowaru = "test",
                    JmNazwa = "test"
                },
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka
                {
                    IDTowar = 2,
                    ZawartoscProcentowa = 0.5M,
                    IloscMieszanki_kg = 1200,
                    IDJm = 7,
                    NazwaTowaru = "test2",
                    JmNazwa = "test2"
                }
            };

            Assert.IsTrue(sut.IsValid);
        }
        [Test]
        public void IsValid_ListIsNotEmpty_AllItemsAreValid_ZawartoscProcentowaIsNotOne_True()
        {
            sut.ListaPozycjiMieszanki = new ObservableCollection<tblProdukcjaZlecenieProdukcyjne_Mieszanka>
            {
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka
                {
                    IDTowar = 1,
                    ZawartoscProcentowa = 0.6M,
                    IloscMieszanki_kg = 1200,
                    IDJm = 7,
                    NazwaTowaru = "test",
                    JmNazwa = "test"
                },
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka
                {
                    IDTowar = 2,
                    ZawartoscProcentowa = 0.5M,
                    IloscMieszanki_kg = 1200,
                    IDJm = 7,
                    NazwaTowaru = "test2",
                    JmNazwa = "test2"
                }
            };

            Assert.IsFalse(sut.IsValid);
        }
        #endregion

        #region ZmianaIlosci
        [Test]
        public void GdyZmienionoIlosc_RecalculateItemQuantities()
        {
            sut = CreateSut(messengerOrg);
            sut.ListaPozycjiMieszanki = new ObservableCollection<tblProdukcjaZlecenieProdukcyjne_Mieszanka>
            {
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka{IDZlecenieProdukcyjneMieszanka=1, IDTowar=1, IDProdukcjaZlecenieProdukcyjne=1, ZawartoscProcentowa=0.1M, IsValid=true },
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka{IDZlecenieProdukcyjneMieszanka=2, IDTowar=2, IDProdukcjaZlecenieProdukcyjne=1, ZawartoscProcentowa=0.3M, IsValid=true },
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka{IDZlecenieProdukcyjneMieszanka=3, IDTowar=3, IDProdukcjaZlecenieProdukcyjne=1, ZawartoscProcentowa=0.6M, IsValid=true }
            };

            messengerOrg.Send(new ZmianaIlosciMieszankiMessage { Ilosc = 10 });

            Assert.IsTrue(sut.ListaPozycjiMieszanki[0].IloscKg == 1);
            Assert.IsTrue(sut.ListaPozycjiMieszanki[1].IloscKg == 3);
            Assert.IsTrue(sut.ListaPozycjiMieszanki[2].IloscKg == 6);
        }

        [Test]
        public void GdyZmienionoIlosc_SumarycznaIloscMieszankiZostajeZmieniona()
        {
            sut = CreateSut(messengerOrg);
            messengerOrg.Send<ZmianaIlosciMieszankiMessage, ZlecenieProdukcyjneMieszankaViewModel>(new ZmianaIlosciMieszankiMessage { Ilosc = 100 });

            Assert.IsTrue(sut.SumarycznaIloscMieszanki == 100);
        }


        #endregion

        #region LoadAsync
        [Test]
        public async Task LoadAsync_WhenIdIsNull_CreateNewList()
        {
            await sut.LoadAsync(null);

            Assert.IsEmpty(sut.ListaPozycjiMieszanki);
        }

        [Test]
        public async Task LoadAsync_WhenIdIsNotNull_UoWShouldBeInvoked()
        {
            await sut.LoadAsync(1);

            unitOfWork.Verify(v => v.tblProdukcjaZlecenieProdukcyjne_Mieszanka.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieProdukcyjne_Mieszanka, bool>>>()));
        }

        [Test]
        public async Task LoadAsync_WhenIdIsNotNull_ListShouldNotBeNull()
        {
            tblProdukcjaZlecenieProdukcyjne_Mieszanka
                                .Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieProdukcyjne_Mieszanka, bool>>>()))
                                .ReturnsAsync(new List<tblProdukcjaZlecenieProdukcyjne_Mieszanka>
                                                                            {
                                                                                new tblProdukcjaZlecenieProdukcyjne_Mieszanka()
                                                                            }) ;

            sut.LoadAsync(1);

            Assert.IsNotNull(sut.ListaPozycjiMieszanki);

        }
        [Test]
        public async Task LoadAsync_WhenIdIsNotNull_CheckIsSummaryIsCalculated()
        {
            tblProdukcjaZlecenieProdukcyjne_Mieszanka
                                .Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieProdukcyjne_Mieszanka, bool>>>()))
                                .ReturnsAsync(new List<tblProdukcjaZlecenieProdukcyjne_Mieszanka>
                                                                            {
                                                                                new tblProdukcjaZlecenieProdukcyjne_Mieszanka(){IloscKg=1}
                                                                            });

            sut.LoadAsync(1);

            Assert.IsTrue(sut.SumarycznaIloscMieszanki > 0);
        }
        #endregion



        #region Delete
        [Test]
        public void OnDelete_IdIsZero_Returns()
        {
            sut = CreateSut(messengerOrg);

            messengerOrg.Send(new UsunietoZlecenieProdukcyjneMessage());

            unitOfWork.Verify(v => v.tblProdukcjaZlecenieProdukcyjne_Mieszanka.RemoveRange(sut.ListaPozycjiMieszanki), Times.Never);
        }


        #endregion

        #region Save
        [Test]
        public async  Task SaveAsync_IdIsNotZero_UowADDMethodIsNotInvoked()
        {
            await sut.SaveAsync(null);

            tblProdukcjaZlcecenieProdukcyjne.Verify(v => v.Add(It.IsAny<tblProdukcjaZlecenie>()), Times.Never);
            //unitOfWork.Verify(v => v.tblProdukcjaZlecenieProdukcyjne_Mieszanka.RemoveRange(sut.ListaPozycjiMieszanki));

        }
        [Test]
        public async Task SaveAsync_WhenIDZlecenieIsZero_Retruns()
        {
            await sut.SaveAsync(0);

            tblProdukcjaZlcecenieProdukcyjne.Verify(v => v.Add(It.IsAny<tblProdukcjaZlecenie>()), Times.Never);
            unitOfWork.Verify(v => v.SaveAsync(), Times.Never);
        }

        [Test]
        public async Task SaveAsync_WhenIdIsNotNull_IdZleceniaProdukcyjnegoIsAssignToAllItemsOfList()
        {

            sut.ListaPozycjiMieszanki = new ObservableCollection<tblProdukcjaZlecenieProdukcyjne_Mieszanka>
            {
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka{IDProdukcjaZlecenieProdukcyjne=1}
            };

            await sut.SaveAsync(2);

            Assert.IsTrue(sut.ListaPozycjiMieszanki[0].IDProdukcjaZlecenieProdukcyjne == 2);
        }

        [Test]
        public async Task SaveAsync_WhenIdIsNotNullAndNotZero_UoWSaveAsyncIsInvoked()
        {
            await sut.SaveAsync(1);
            
            unitOfWork.Verify(v => v.SaveAsync());
        }

        [Test]
        public async Task SaveAsync_WhenListaPozycjiIDisZero_UowAddMethodIsInvoked()
        {
            sut.ListaPozycjiMieszanki = new ObservableCollection<tblProdukcjaZlecenieProdukcyjne_Mieszanka>
            {
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka{IDZlecenieProdukcyjneMieszanka=0, ZawartoscProcentowa=1}
            };

            await sut.SaveAsync(1);

            tblProdukcjaZlecenieProdukcyjne_Mieszanka.Verify(v => v.Add(It.IsAny<tblProdukcjaZlecenieProdukcyjne_Mieszanka>()));
        }

        [Test]
        public async Task SaveAsync_WhenListaPozycjiIDisNotZero_UowAddMethodIsNotInvoked()
        {
            sut.ListaPozycjiMieszanki = new ObservableCollection<tblProdukcjaZlecenieProdukcyjne_Mieszanka>
            {
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka{IDZlecenieProdukcyjneMieszanka=1}
            };

            await sut.SaveAsync(1);

            tblProdukcjaZlecenieProdukcyjne_Mieszanka.Verify(v => v.Add(It.IsAny<tblProdukcjaZlecenieProdukcyjne_Mieszanka>()),Times.Never);
        }

        [Test]
        public async Task SaveAsync_WhenDone_IsChangedShouldBeFalse()
        {
            sut.ListaPozycjiMieszanki = new ObservableCollection<tblProdukcjaZlecenieProdukcyjne_Mieszanka>
            {
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka{IDZlecenieProdukcyjneMieszanka=1},
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka{IDZlecenieProdukcyjneMieszanka=2}
            };
            
            await sut.SaveAsync(1);

            Assert.IsFalse(sut.IsChanged);
        }
        #endregion


        [Test]
        public void PoEdycjiKomorkiDataGridCommandExecute_WysylajPodsumowanieMieszankiMessengerem()
        {
            sut.PoEdycjiKomorkiDataGridCommand.Execute(null);

            messenger.Verify(v => v.Send<tblProdukcjaZlecenie, ZlecenieProdukcyjneNaglowekViewModel_old>(sut.ZlcecenieProdukcyjne));
        }

        [Test]
        public void OtworzEwidencjeTowarowCommand_WhenCalled_OpenWindow()
        {
            sut.OtworzEwidencjeTowarowCommand.Execute(null);

            viewService.Verify(v => v.Show<TowarEwidencjaViewModel>());
        }



        [Test]
        public void DodajPozycjeMieszankiCommandExecute_CreatesNewInstanceOfWybranaPozycjaMieszanki()
        {
            sut.DodajPozycjeMieszankiCommand.Execute(null);

            Assert.IsTrue(sut.WybranaPozycjaMieszanki.IDZlecenieProdukcyjneMieszanka == 0);
        }



        [Test]
        public void DodajPozycjeMieszankiCommandExecute_Opens_SendMessageWithVwMagazynGTX()
        {
            sut.DodajPozycjeMieszankiCommand.Execute(null);

            messenger.Verify(v => v.Send(It.IsAny<vwMagazynGTX>()));
        }

        [Test]
        public void ZmienPozycjeMieszankiCommandExecute_WhenCalled_OpensMagazynStanTowaruView()
        {
            sut.ZmienPozycjeMieszankiCommand.Execute(null);

            viewService.Verify(v => v.Show<MagazynStanTowaruViewModel>());
        }

        [Test]
        public void GdyWyslanoStanTowaru_WhenStanTowaruIsNull_Returns()
        {
            sut = CreateSut(messengerOrg);

            messengerOrg.Send((vwStanTowaru)null);

            Assert.IsTrue(sut.WybranaPozycjaMieszanki == null);
        }

        [Test]
        public void GdyWyslanoStanTowaru_WhenStanTowaruIsNotNull_AssignIDTowaruToWybranaPozycjaMieszanki()
        {
            sut = CreateSut(messengerOrg);

            messengerOrg.Send(new vwMagazynRuchGTX { IdTowar = 1 });

            Assert.IsTrue(sut.WybranaPozycjaMieszanki.IDTowar == 1);
        }
        [Test]
        public void GdyWyslanoStanTowaru_WhenStanTowaruIsNotNull_AssignNazwaTowaruToWybranaPozycjaMieszanki()
        {
            sut = CreateSut(messengerOrg);

            messengerOrg.Send(new vwMagazynRuchGTX { IdTowar = 1, TowarNazwa = "test" });

            Assert.IsTrue(sut.WybranaPozycjaMieszanki.NazwaTowaru == "test");
        }
        [Test]
        public void GdyWyslanoStanTowaru_WhenStanTowaruIsNotNull_CloseWidnow_MagazynStanTowaruViewModel()
        {
            sut = CreateSut(messengerOrg);

            messengerOrg.Send(new vwMagazynRuchGTX { IdTowar = 1 });

            viewService.Verify(v => v.Close<MagazynStanTowaruViewModel>());
        }

        [Test]
        public void MessengerIsRegisteredForArg_NrZleceniaMessage()
        {
            messenger.Verify(v => v.Register<NrZleceniaMessage>(sut, It.IsAny<Action<NrZleceniaMessage>>(), It.IsAny<bool>()));
        }

        [Test]
        public void GdyPrzeslanoNrZlecenia_tblProdukcjaZlecenieProdukcyjne_Mieszanka_ShouldBeInvoked()
        {
            sut = CreateSut(messengerOrg);

            messengerOrg.Send(new NrZleceniaMessage { NrZlecenia = 1 });

            tblProdukcjaZlecenieProdukcyjne_Mieszanka.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieProdukcyjne_Mieszanka, bool>>>()));
        }
        [Test]
        public void GdyPrzeslanoNrZlecenia_KopiaListyMieszankiPowinnaZostacStworzona()
        {
            sut = CreateSut(messengerOrg);
            tblProdukcjaZlecenieProdukcyjne_Mieszanka
                .Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieProdukcyjne_Mieszanka, bool>>>()))
                .ReturnsAsync(new ObservableCollection<tblProdukcjaZlecenieProdukcyjne_Mieszanka>
                {
                    new tblProdukcjaZlecenieProdukcyjne_Mieszanka {IDZlecenieProdukcyjneMieszanka=1, IDTowar=1}
                }
                );

            messengerOrg.Send(new NrZleceniaMessage { NrZlecenia = 1 });

            Assert.IsTrue(sut.ListaPozycjiMieszanki.Compare(sut.ListaPozycjiMieszankiStartowa));
        }

        #region IsChanged
        [Test]
        public void IsChanged_TwoListsAreDifferent_True()
        {
            sut.ListaPozycjiMieszankiStartowa = new ObservableCollection<tblProdukcjaZlecenieProdukcyjne_Mieszanka>()
            {
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka
                {
                    IDTowar=1,
                    IDZlecenieProdukcyjneMieszanka=1,
                }
            };

            sut.ListaPozycjiMieszanki = new ObservableCollection<tblProdukcjaZlecenieProdukcyjne_Mieszanka>
            {
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka
                {
                    IDTowar=1,
                    IDZlecenieProdukcyjneMieszanka=2
                }
            };

            var result = sut.IsChanged;

            Assert.IsTrue(result);
        }

        [Test]
        public void IsChanged_TwoListsAreTheSame_False()
        {
            sut.ListaPozycjiMieszankiStartowa = new ObservableCollection<tblProdukcjaZlecenieProdukcyjne_Mieszanka>()
            {
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka
                {
                    IDTowar=1,
                    IDZlecenieProdukcyjneMieszanka=1,
                }
            };

            sut.ListaPozycjiMieszanki = new ObservableCollection<tblProdukcjaZlecenieProdukcyjne_Mieszanka>
            {
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka
                {
                    IDTowar=1,
                    IDZlecenieProdukcyjneMieszanka=1
                }
            };

            var result = sut.IsChanged;

            Assert.IsFalse(result);
        }

        [Test]
        public void IsChanged_TwoListsAreDifferentInItemsCount_False()
        {
            sut.ListaPozycjiMieszankiStartowa = new ObservableCollection<tblProdukcjaZlecenieProdukcyjne_Mieszanka>()
            {
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka
                {
                    IDTowar=1,
                    IDZlecenieProdukcyjneMieszanka=1,
                }
            };

            sut.ListaPozycjiMieszanki = new ObservableCollection<tblProdukcjaZlecenieProdukcyjne_Mieszanka>
            {
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka
                {
                    IDTowar=1,
                    IDZlecenieProdukcyjneMieszanka=1
                },
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka
                {
                    IDTowar=2,
                    IDZlecenieProdukcyjneMieszanka=1
                }
            };

            var result = sut.IsChanged;

            Assert.IsTrue(result);
        }

        #endregion

    }
}
