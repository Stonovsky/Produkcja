using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.UI.Utilities.Dostep;
using GAT_Produkcja.UI.ViewModel.Kontrahent.Ewidencja;
using GAT_Produkcja.Utilities.BaseClasses.Messages;
using GAT_Produkcja.ViewModel.Kontrahent.Dodaj;
using GAT_Produkcja.ViewModel.Kontrahent.Ewidencja;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Dodaj.Naglowek;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Dodaj.Towar;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.ZlecenieCiecia.Dodaj.Naglowek
{
    [TestFixture]
    public class ZlecenieCieciaVMEntityViewModelTests_new : TestBase
    {
        private Mock<IZlecenieCieciaTowarViewModel> zlecenieCieciaTowarViewModel;
        private Mock<ITblPracownikGATRepository> tblPracownikGAT;
        private Mock<ITblProdukcjaZlecenieCieciaRepository> tblProdukcjaZlecenieCiecia;
        private Mock<ITblProdukcjaZlecenieRepository> tblProdukcjaZlecenie;
        private Mock<ITblProdukcjaZlecenieStatusRepository> tblProdukcjaZlecenieStatus;
        private ZlecenieCieciaNaglowekViewModel sut;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            zlecenieCieciaTowarViewModel = new Mock<IZlecenieCieciaTowarViewModel>();

            tblPracownikGAT = new Mock<ITblPracownikGATRepository>();
            UnitOfWork.Setup(s => s.tblPracownikGAT).Returns(tblPracownikGAT.Object);

            tblProdukcjaZlecenieCiecia = new Mock<ITblProdukcjaZlecenieCieciaRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaZlecenieCiecia).Returns(tblProdukcjaZlecenieCiecia.Object);

            tblProdukcjaZlecenie = new Mock<ITblProdukcjaZlecenieRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaZlecenie).Returns(tblProdukcjaZlecenie.Object);

            tblProdukcjaZlecenieStatus = new Mock<ITblProdukcjaZlecenieStatusRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaZlecenieStatus).Returns(tblProdukcjaZlecenieStatus.Object);

            CreateSut();    
        }
        public override void CreateSut()
        {
            sut = new ZlecenieCieciaNaglowekViewModel(ViewModelService.Object, zlecenieCieciaTowarViewModel.Object);
        }

        
        #region Messengers
        [Test]
        public void Messengers_Rejestracja()
        {
            //Messenger.Verify(v => v.Register(sut, It.IsAny<Action<tblProdukcjaZlecenieCiecia>>(), It.IsAny<bool>()));
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<tblKontrahent>>(), It.IsAny<bool>()));

        }


        #region GdyPrzeslanoZlecenieCiecia
        [Test]
        public void GdyPrzseslanoZlecenieCiecia_GdyZlecenieNull_NiePobierajZBazy()
        {
            MessengerSend((tblProdukcjaZlecenieCiecia)null);

            sut.LoadCommand.Execute(null);

            tblProdukcjaZlecenieCiecia.Verify(v => v.GetByIdAsync(It.IsAny<int>()),Times.Never);
        }

        [Test]
        public async Task GdyPrzeslanoZlecenie_GdyOk_PobierzZlecenieZBazy()
        {
            MessengerSend(new tblProdukcjaZlecenie { IDKontrahent = 1, IDProdukcjaZlecenie = 1 });

            await sut.LoadAsync();

            tblProdukcjaZlecenie.Verify(x => x.GetByIdAsync(It.IsAny<int>()));
        }
        #endregion

        #region GdyPrzeslanoKontrahenta
        [Test]
        public void GdyPrzeslanoKontrahenta_GduObjJestNull_NiePrzypisuj()
        {
            MessengerSend((tblKontrahent)null);

            Assert.IsNull(sut.Kontrahent);
        }

        [Test]
        public void GdyPrzeslanoKontrahenta_GdyIdKontrahentaJestZero_NiePrzypisuj()
        {
            MessengerSend( new tblKontrahent { ID_Kontrahent=0});

            Assert.IsNull(sut.Kontrahent);
        }
        [Test]
        public void GdyPrzeslanoKontrahenta_GdyObjOk_PrzypiszDoWlasciwosci()
        {
            MessengerSend(new tblKontrahent { ID_Kontrahent = 1 });

            Assert.IsNotNull(sut.Kontrahent);
        }

        #endregion

        #endregion


        #region DodajKontrahentaCommand
        [Test]
        public void WybierzKontrahentaCommandExecute_OtwieraOknoZEwidencjaKontrahentow()
        {
            sut.WybierzKontrahentaCommand.Execute(null);

            ViewService.Verify(v => v.Show<EwidencjaKontrahentowViewModel>());
        }

        [Test]
        public void WybierzKontrahentaCommandExecute_WysylaMessageZeStanemEwidencji_Select()
        {
            sut.WybierzKontrahentaCommand.Execute(null);

            Messenger.Verify(x => x.Send(ListViewModelStatesEnum.Select));
        }
        #endregion


        #region LoadCommand
        [Test]
        public void LoadCommandExecute_PobieraListyZBazy()
        {
            sut.LoadCommand.Execute(null);

            tblPracownikGAT.Verify(v => v.GetAllAsync());
        }

        [Test]
        public void LoadCommandExecute_UstawienieDatyZleceniaNaBiezaca()
        {
            sut.LoadCommand.Execute(null);

            Assert.AreEqual(DateTime.Now.Date, sut.VMEntity.DataUtworzenia.Date);
        }


        [Test]
        public void LoadCommandExecute_PrzypiszDaneWyjsciowe_GdyListaPracownikowNieJestPusta_WybierzZlecajacegoJakoPracownikazZalogowanego()
        {
            UzytkownikZalogowany.Uzytkownik = new tblPracownikGAT { ID_PracownikGAT = 1 };

            tblPracownikGAT.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<tblPracownikGAT>
            {
                new tblPracownikGAT{ID_PracownikGAT=1},
                new tblPracownikGAT{ID_PracownikGAT=2},
                new tblPracownikGAT{ID_PracownikGAT=3},
            });

            sut.LoadCommand.Execute(null);

            Assert.AreEqual(1, sut.VMEntity.IDZlecajacy);
        }

        [Test]
        public void LoadCommandExecute_PrzypiszDaneWyjsciowe_GdyListaPracownikowJestPusta_NieWybierajZlecajacego()
        {
            UzytkownikZalogowany.Uzytkownik = new tblPracownikGAT { ID_PracownikGAT = 1 };

            tblPracownikGAT.Setup(s => s.GetAllAsync());

            sut.LoadCommand.Execute(null);

            Assert.IsNull(sut.WybranyPracownikZlecajacy);
        }
        #endregion


        #region SaveCommand
        #region CanExecute
        [Test]
        public void SaveCommandCanExecute_GdyIsValidJestFalse_False()
        {
            sut.VMEntity.IsValid= false;

            var actual = sut.SaveCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }
        [Test]
        public void SaveCommandCanExecute_GdyIsValidJestTrue_True()
        {
            SaveCanExecute_True();

            var actual = sut.SaveCommand.CanExecute(null);

            Assert.IsTrue(actual);
        }

        private void SaveCanExecute_True()
        {
            sut.VMEntity = new tblProdukcjaZlecenie
            {
                IDKontrahent = 1,
                IDWykonujacy = 1,
                IDZlecajacy = 2,
                DataRozpoczecia = DateTime.Now,
                DataZakonczenia = DateTime.Now,
                NrZlecenia = 1,
                NazwaZlecenia = "test",
            };
            //sut.VMEntity.IsValid = true;
            zlecenieCieciaTowarViewModel.Setup(s => s.IsValid).Returns(true);


        }
        #endregion
        #region Execute
        [Test]
        public void SaveCommandExecute_GdyIdZlecCeiciaJestZero_AddISaveAsyncJestWywolane()
        {
            SaveCanExecute_True();
            sut.VMEntity.IDProdukcjaZlecenie= 0;

            sut.SaveCommand.Execute(null);

            tblProdukcjaZlecenie.Verify(v => v.Add(It.IsAny<tblProdukcjaZlecenie>()));
            UnitOfWork.Verify(v => v.SaveAsync());
        }

        [Test]
        public void SaveCommandExecute_GdyIdZlecCeiciaNieJestZero_TylkoSaveAsyncJestWywolane()
        {
            SaveCanExecute_True();
            sut.VMEntity.IDProdukcjaZlecenie= 1;

            sut.SaveCommand.Execute(null);

            tblProdukcjaZlecenie.Verify(v => v.Add(It.IsAny<tblProdukcjaZlecenie>()),Times.Never);
            UnitOfWork.Verify(v => v.SaveAsync());
        }

        [Test]
        public void SaveCommandExecute_KolejnoscMetodGdyIdJestZero()
        {
            SaveCanExecute_True();
            sut.VMEntity.IDProdukcjaZlecenie= 0;

            string callOrder = "";
            tblProdukcjaZlecenie.Setup(s => s.Add(It.IsAny<tblProdukcjaZlecenie>())).Callback(() => callOrder += "1");
            UnitOfWork.Setup(s => s.SaveAsync()).Callback(() => callOrder += "2");
            zlecenieCieciaTowarViewModel.Setup(s => s.SaveAsync(It.IsAny<int>())).Callback(() => callOrder += "3");


            sut.SaveCommand.Execute(null);


            Assert.AreEqual("123", callOrder);
        }

        [Test]
        public void SaveCommandExecute_KolejnoscMetodGdyIdNieJestZero()
        {
            SaveCanExecute_True();
            sut.VMEntity.IDProdukcjaZlecenie= 1;

            string callOrder = "";
            UnitOfWork.Setup(s => s.SaveAsync()).Callback(() => callOrder += "2");
            zlecenieCieciaTowarViewModel.Setup(s => s.SaveAsync(It.IsAny<int>())).Callback(() => callOrder += "3");


            sut.SaveCommand.Execute(null);


            Assert.AreEqual("23", callOrder);
        }
        [Test]
        public void SaveCommandExecute_PoZapisieWyswietlDialog()
        {
            SaveCanExecute_True();
            sut.VMEntity.IDProdukcjaZlecenie= 1;

            sut.SaveCommand.Execute(null);

            DialogService.Verify(v => v.ShowInfo_BtnOK(It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        public void SaveCommandExecute_PoZapisieWyslijMessage()
        {
            SaveCanExecute_True();
            sut.VMEntity.IDProdukcjaZlecenie= 1;

            sut.SaveCommand.Execute(null);

            Messenger.Verify(v => v.Send(It.IsAny<tblProdukcjaZlecenie>(),nameof(RefreshListMessage)));
        }
        [Test]
        public void SaveCommandExecute_PoZapisieZamknijOkno()
        {
            SaveCanExecute_True();
            sut.VMEntity.IDProdukcjaZlecenie= 1;

            sut.SaveCommand.Execute(null);

            ViewService.Verify(v => v.Close(sut.GetType().Name));
        }
        #endregion
        #endregion
    }
}
