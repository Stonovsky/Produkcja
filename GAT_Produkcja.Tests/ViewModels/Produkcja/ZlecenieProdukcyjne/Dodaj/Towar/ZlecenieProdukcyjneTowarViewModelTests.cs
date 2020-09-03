using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Dodaj.Messages;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieDodajTowar;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieDodajTowar.Messages;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Towar;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.ZlecenieProdukcyjne.ZlecenieProdukcyjneGniazda.TowarParametry
{
    [TestFixture]
    public class ZlecenieProdukcyjneTowarViewModelTests : TestBase
    {

        private Mock<ITblProdukcjaZlecenieTowarRepository> tblProdukcjaZlecenieTowar;
        private ZlecenieProdukcyjneTowarViewModel sut;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();


            tblProdukcjaZlecenieTowar = new Mock<ITblProdukcjaZlecenieTowarRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaZlecenieTowar).Returns(tblProdukcjaZlecenieTowar.Object);

            CreateSut();
        }

        public override void CreateSut()
        {
            sut = new ZlecenieProdukcyjneTowarViewModel(ViewModelService.Object);
        }


        #region Messengers

        #region Rejestracja
        [Test]
        public void CTOR_Rejestracja()
        {
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<ProdukcjaZlecenieDodajTowarMessage>>(), It.IsAny<bool>()));
        }
        #endregion

        #region GdyPrzeslanoDodajMessage

        [Test]
        public void GdyPrzeslanoDodajMessage_GdyMessageNull_RzucWyjatkiem()
        {
            MessengerSend((ProdukcjaZlecenieDodajTowarMessage)null);

            //Assert.That(() => messengerOrg.Send((ProdukcjaZlecenieDodajTowarMessage)null),
            //                                     Throws.TypeOf<ArgumentOutOfRangeException>());
            //Assert.Throws<NullReferenceException>(() => messengerOrg.Send((ProdukcjaZlecenieDodajTowarMessage)null));

            //var ex = Assert.Throws<ArgumentOutOfRangeException>(() => messengerOrg.Send((ProdukcjaZlecenieDodajTowarMessage)null));
            //Assert.AreEqual(ex.ParamName, "dodajMessage");

            Assert.IsEmpty(sut.ListaTowarowLiniaKalandra);
            Assert.IsEmpty(sut.ListaTowarowLiniaWloknin);
        }

        [Test]
        public void GdyPrzeslanoDodajMessage_GdyProdukcjaZlecenieTowarJestNull_NieDodawajDoList()
        {
            MessengerSend(new ProdukcjaZlecenieDodajTowarMessage
            {
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Dodaj,
                GniazdaProdukcyjneEnum = GniazdaProdukcyjneEnum.LiniaWloknin,
            });


            Assert.IsEmpty(sut.ListaTowarowLiniaKalandra);
            Assert.IsEmpty(sut.ListaTowarowLiniaWloknin);
        }

        private void Wyslij_ProdukcjaZlecenieDodajTowarMessage(GniazdaProdukcyjneEnum gniazdaProdukcyjneEnum)
        {
            MessengerSend(new ProdukcjaZlecenieDodajTowarMessage
            {
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Dodaj,
                GniazdaProdukcyjneEnum = gniazdaProdukcyjneEnum,
                ZlecenieTowar = new tblProdukcjaZlecenieTowar()
            });
        }

        [Test]
        public void GdyPrzeslanoDodajMessage_GdyGniazdoProdukcyjneLiniaWloknin_DodajDoListyLiniiWloknin()
        {
            Wyslij_ProdukcjaZlecenieDodajTowarMessage(GniazdaProdukcyjneEnum.LiniaWloknin);

            Assert.IsNotEmpty(sut.ListaTowarowLiniaWloknin);
            Assert.IsEmpty(sut.ListaTowarowLiniaKalandra);
        }

        [Test]
        public void GdyPrzeslanoDodajMessage_GdyGniazdoProdukcyjneLiniaKalandra_DodajDoListyLiniiKalandra()
        {
            Wyslij_ProdukcjaZlecenieDodajTowarMessage(GniazdaProdukcyjneEnum.LiniaDoKalandowania);

            Assert.IsNotEmpty(sut.ListaTowarowLiniaKalandra);
            Assert.IsEmpty(sut.ListaTowarowLiniaWloknin);
        }

        [Test]
        public void GdyPrzeslanoDodajMessage_GdyGniazdoProdukcyjneLiniaKonfekcji_NieDodawajDoList()
        {

            Wyslij_ProdukcjaZlecenieDodajTowarMessage(GniazdaProdukcyjneEnum.LiniaDoKonfekcji);

            Assert.IsEmpty(sut.ListaTowarowLiniaKalandra);
            Assert.IsEmpty(sut.ListaTowarowLiniaWloknin);
        }


        [Test]
        public void GdyPrzeslanoDodajMessage_GdyGniazdoLiniaWlokninWMessage_DodajDoListyWloknin()
        {
            MessengerSend(new ProdukcjaZlecenieDodajTowarMessage
            {
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Dodaj,
                GniazdaProdukcyjneEnum = GniazdaProdukcyjneEnum.LiniaWloknin,
                ZlecenieTowar = new tblProdukcjaZlecenieTowar
                {
                    IDProdukcjaZlecenieTowar = 1
                }
            });

            Assert.IsNotEmpty(sut.ListaTowarowLiniaWloknin);
        }

        [Test]
        public void GdyPrzeslanoDodajMessage_ZamykaOknoDodawania()
        {
            MessengerSend(new ProdukcjaZlecenieDodajTowarMessage());

            ViewService.Verify(v => v.Close<ZlecenieDodajTowarViewModel>());
        }

        #endregion

        #endregion

        #region DodajDoLiniWlokninCommand
        [Test]
        public void DodajDoLiniWlokninCommandExecute_OtwieraOknoDodawania()
        {
            sut.DodajDoLiniiWlokninCommand.Execute(null);

            ViewService.Verify(v => v.Show<ZlecenieDodajTowarViewModel>());
        }
        [Test]
        public void DodajDoLiniWlokninCommandExecute_WysylaMessage()
        {
            sut.DodajDoLiniiWlokninCommand.Execute(null);

            Messenger.Verify(v => v.Send<ProdukcjaZlecenieDodajTowarMessage, ZlecenieDodajTowarViewModel>
                                                        (It.Is<ProdukcjaZlecenieDodajTowarMessage>
                                                                        (m => m.GniazdaProdukcyjneEnum == GniazdaProdukcyjneEnum.LiniaWloknin)));

        }
        #endregion

        #region EdytujTowarLiniiWloknin
        [Test]
        public void EdytujTowarLiniiiWlokninCommandCanExecute_GdyWybranyTowarNull_False()
        {
            sut.WybranyTowarLiniaWloknin = null;

            var actual = sut.EdytujTowarLiniiWlokninCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }
        [Test]
        public void EdytujTowarLiniiiWlokninCommandExeucute_OtwieraOknoEdycji()
        {
            sut.EdytujTowarLiniiWlokninCommand.Execute(null);

            ViewService.Verify(v => v.Show<ZlecenieDodajTowarViewModel>());
        }

        [Test]
        public void EdytujTowarLiniiiWlokninCommandExeucute_WysylaWiadomosc()
        {
            sut.EdytujTowarLiniiWlokninCommand.Execute(null);

            Messenger.Verify(v => v.Send(It.IsAny<EdytujTowarMessage>()));
        }

        #endregion

        #region DodajDoLiniiKalandraCommand
        [Test]
        public void DodajDoLiniiKalandraCommandExecute_OtwieraOknoDodawania()
        {
            sut.DodajDoLiniiKalandraCommand.Execute(null);

            ViewService.Verify(v => v.Show<ZlecenieDodajTowarViewModel>());
        }
        [Test]
        public void DodajDoLiniiKalandraCommandExecute_WysylaMessage()
        {
            sut.DodajDoLiniiKalandraCommand.Execute(null);

            Messenger.Verify(v => v.Send<ProdukcjaZlecenieDodajTowarMessage, ZlecenieDodajTowarViewModel>
                                                        (It.Is<ProdukcjaZlecenieDodajTowarMessage>
                                                                        (m => m.GniazdaProdukcyjneEnum == GniazdaProdukcyjneEnum.LiniaDoKalandowania)));

        }
        #endregion

        #region EdytujTowarLiniiKalandra
        [Test]
        public void EdytujTowarLiniiKalandraCommandCanExecute_GdyWybranyTowarNull_False()
        {
            sut.WybranyTowarLiniaKalandra = null;

            var actual = sut.EdytujTowarLiniiKalandraCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }
        [Test]
        public void EdytujTowarLiniiKalandraCommandExeucute_OtwieraOknoEdycji()
        {
            sut.EdytujTowarLiniiKalandraCommand.Execute(null);

            ViewService.Verify(v => v.Show<ZlecenieDodajTowarViewModel>());
        }

        [Test]
        public void EdytujTowarLiniiKalandraCommandExeucute_WysylaWiadomosc()
        {
            sut.EdytujTowarLiniiKalandraCommand.Execute(null);

            Messenger.Verify(v => v.Send(It.IsAny<EdytujTowarMessage>()));
        }

        #endregion

        #region LoadAsync
        [Test]
        public void LoadAsync_GdyNiePrzeslanoIdZlecProd()
        {
            sut.LoadAsync(null);

            Assert.IsEmpty(sut.ListaTowarowLiniaKalandra);
            Assert.IsEmpty(sut.ListaTowarowLiniaWloknin);
        }
        [Test]
        public void LoadAsync_GdyPrzeslanoIdZlecProd_PobierzListyZBazy()
        {
            tblProdukcjaZlecenieTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieTowar, bool>>>())).ReturnsAsync(new List<tblProdukcjaZlecenieTowar>
            {
                new   tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=1},
                new   tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=2},
            });

            sut.LoadAsync(1);

            Assert.IsNotEmpty(sut.ListaTowarowLiniaKalandra);
            Assert.IsNotEmpty(sut.ListaTowarowLiniaWloknin);
        }
        #endregion

        #region SaveAsync
        [Test]
        public void SaveAsync_GdyIdZlecProJestNull_NieZapisuje()
        {
            sut.SaveAsync(null);

            UnitOfWork.Verify(v => v.SaveAsync(), Times.Never);
        }

        [Test]
        public void SaveAsync_GdyIdNieJestNull_PrzypiszIdZleceniaProdukcyjnegoWszystkimTowarom()
        {
            sut.ListaTowarowLiniaKalandra = new ObservableCollection<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=0},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=2},
            };

            sut.ListaTowarowLiniaWloknin = new ObservableCollection<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=0},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=4},
            };

            sut.SaveAsync(1);

            var iloscElementowZPrzypisanymId = sut.ListaTowarowLiniaWloknin.Where(t => t.IDProdukcjaZlecenie == 1).Count();
            iloscElementowZPrzypisanymId += sut.ListaTowarowLiniaKalandra.Where(t => t.IDProdukcjaZlecenie == 1).Count();

            Assert.AreEqual(4, iloscElementowZPrzypisanymId);

        }

        [Test]
        public void SaveAsync_GdyIdNieJestNull_PrzypiszIdGniazdaWszystkimTowarom()
        {
            sut.ListaTowarowLiniaKalandra = new ObservableCollection<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=0},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=2},
            };

            sut.ListaTowarowLiniaWloknin = new ObservableCollection<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=0},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=4},
            };

            sut.SaveAsync(1);

            var iloscElementowZDlaGniazdaWloknin = sut.ListaTowarowLiniaWloknin.Where(t => t.IDProdukcjaGniazdoProdukcyjne == (int)GniazdaProdukcyjneEnum.LiniaWloknin).Count();
            var iloscElementowZDlaGniazdaKalandra = sut.ListaTowarowLiniaKalandra.Where(t => t.IDProdukcjaGniazdoProdukcyjne == (int)GniazdaProdukcyjneEnum.LiniaDoKalandowania).Count();

            Assert.AreEqual(2, iloscElementowZDlaGniazdaWloknin);
            Assert.AreEqual(2, iloscElementowZDlaGniazdaKalandra);

        }


        [Test]
        public void SaveAsync_GdyIdNieJestNull_GdyElementyListMajaPrzypisaneId_TylkoUpdateWBazie()
        {
            sut.ListaTowarowLiniaKalandra = new ObservableCollection<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=1},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=2},
            };

            sut.ListaTowarowLiniaWloknin = new ObservableCollection<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=3},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=4},
            };

            sut.SaveAsync(1);

            tblProdukcjaZlecenieTowar.Verify(v => v.AddRange(It.IsAny<IEnumerable<tblProdukcjaZlecenieTowar>>()), Times.Never);
            UnitOfWork.Verify(v => v.SaveAsync(), Times.Once);
        }

        [Test]
        public void SaveAsync_GdyIdNieJestNull_GdyElementyListNieMajaId_DodajTeElementyDoBazy()
        {
            sut.ListaTowarowLiniaKalandra = new ObservableCollection<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=0},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=2},
            };

            sut.ListaTowarowLiniaWloknin = new ObservableCollection<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=0},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=4},
            };

            sut.SaveAsync(1);

            tblProdukcjaZlecenieTowar.Verify(v => v.AddRange(It.IsAny<IEnumerable<tblProdukcjaZlecenieTowar>>()), Times.Exactly(2));
            UnitOfWork.Verify(v => v.SaveAsync(), Times.Once);
        }


        #endregion

        #region DeleteAsync
        [Test]
        public void DeleteAsync_GdyIdJestZero_NicNieRob()
        {
            sut.DeleteAsync(0);

            tblProdukcjaZlecenieTowar.Verify(v => v.RemoveRange(It.IsAny<IEnumerable<tblProdukcjaZlecenieTowar>>()), Times.Never);
        }
        [Test]
        public void DeleteAsync_GdyIdNieJestZero_UsunZBazyWszystkieElementyListKtoreMajaId()
        {
            sut.ListaTowarowLiniaWloknin = new ObservableCollection<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=0, IDProdukcjaZlecenie=1, IDProdukcjaGniazdoProdukcyjne=(int)GniazdaProdukcyjneEnum.LiniaWloknin },
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=0, IDProdukcjaZlecenie=1, IDProdukcjaGniazdoProdukcyjne=(int)GniazdaProdukcyjneEnum.LiniaWloknin },
            };

            sut.ListaTowarowLiniaKalandra = new ObservableCollection<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=3, IDProdukcjaZlecenie=1, IDProdukcjaGniazdoProdukcyjne=(int)GniazdaProdukcyjneEnum.LiniaDoKalandowania },
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=4, IDProdukcjaZlecenie=1, IDProdukcjaGniazdoProdukcyjne=(int)GniazdaProdukcyjneEnum.LiniaDoKalandowania },
            };

            sut.DeleteAsync(1);

            tblProdukcjaZlecenieTowar.Verify(v => v.RemoveRange(It.IsAny<IEnumerable<tblProdukcjaZlecenieTowar>>()), Times.Once);
            UnitOfWork.Verify(v => v.SaveAsync());
        }


        #endregion
    }
}
