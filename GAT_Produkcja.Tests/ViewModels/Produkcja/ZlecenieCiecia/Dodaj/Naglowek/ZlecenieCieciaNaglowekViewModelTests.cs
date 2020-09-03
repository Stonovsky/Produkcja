using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.UI.Utilities.Dostep;
using GAT_Produkcja.UI.ViewModel.Kontrahent.Ewidencja;
using GAT_Produkcja.ViewModel.Kontrahent.Dodaj;
using GAT_Produkcja.ViewModel.Kontrahent.Ewidencja;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Dodaj.Naglowek;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Dodaj.Towar;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.ZlecenieCiecia.Dodaj.Naglowek
{
    [TestFixture]
    public class ZlecenieCieciaNaglowekViewModelTests : TestBase
    {
        private Mock<IZlecenieCieciaTowarViewModel> zlecenieCieciaTowarViewModel;
        private Mock<ITblPracownikGATRepository> tblPracownikGAT;
        private Mock<ITblProdukcjaZlecenieCieciaRepository> tblProdukcjaZlecenieCiecia;
        private Mock<ITblProdukcjaZlecenieStatusRepository> tblProdukcjaZlecenieStatus;
        private ZlecenieCieciaNaglowekViewModel_old sut;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            zlecenieCieciaTowarViewModel = new Mock<IZlecenieCieciaTowarViewModel>();

            tblPracownikGAT = new Mock<ITblPracownikGATRepository>();
            UnitOfWork.Setup(s => s.tblPracownikGAT).Returns(tblPracownikGAT.Object);

            tblProdukcjaZlecenieCiecia = new Mock<ITblProdukcjaZlecenieCieciaRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaZlecenieCiecia).Returns(tblProdukcjaZlecenieCiecia.Object);

            tblProdukcjaZlecenieStatus = new Mock<ITblProdukcjaZlecenieStatusRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaZlecenieStatus).Returns(tblProdukcjaZlecenieStatus.Object);

            CreateSut();    
        }
        public override void CreateSut()
        {
            sut = new ZlecenieCieciaNaglowekViewModel_old(ViewModelService.Object, zlecenieCieciaTowarViewModel.Object);
        }

        
        #region Messengers
        [Test]
        public void Messengers_Rejestracja()
        {
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<tblProdukcjaZlecenieCiecia>>(), It.IsAny<bool>()));
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
        public void GdyPrzseslanoZlecenieCiecia_GdyZlecenieCiecieIdJestZer0_NiePobierajZBazy()
        {
            MessengerSend(new tblProdukcjaZlecenieCiecia { IDKontrahent = 1, IDProdukcjaZlecenieCiecia = 0 });

            sut.LoadCommand.Execute(null);

            tblProdukcjaZlecenieCiecia.Verify(v => v.GetByIdAsync(It.IsAny<int>()), Times.Never);

        }
        [Test]
        public void GdyPrzseslanoZlecenieCiecia_GdyZlecenieOk_PobierzZBazy()
        {
            MessengerSend(new tblProdukcjaZlecenieCiecia { IDKontrahent = 1, IDProdukcjaZlecenieCiecia = 1 });

            sut.LoadCommand.Execute(null);

            tblProdukcjaZlecenieCiecia.Verify(v => v.GetByIdAsync(It.IsAny<int>()));

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

        [Test]
        public void GdyPrzeslanoKontrahenta_ZamknijOknoEwidencjiKontrahentow()
        {
            MessengerSend(new tblKontrahent { ID_Kontrahent = 1 });

            ViewService.Verify(v => v.Close<EwidencjaKontrahentowViewModel_old>());
        }

        [Test]
        public void GdyPrzeslanoKontrahenta_ZamknijOknoKontrahenta()
        {
            MessengerSend(new tblKontrahent { ID_Kontrahent = 1 });

            ViewService.Verify(v => v.Close<DodajKontrahentaViewModel_old>());
        }
        #endregion

        #endregion


        #region DodajKontrahentaCommand
        [Test]
        public void WybierzKontrahentaCommandExecute_OtwieraOknoZEwidencjaKontrahentow()
        {
            sut.WybierzKontrahentaCommand.Execute(null);

            ViewService.Verify(v => v.Show<EwidencjaKontrahentowViewModel_old>());
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

            Assert.AreEqual(DateTime.Now.Date, sut.Naglowek.DataZlecenia.Date);
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

            Assert.AreEqual(1, sut.Naglowek.IDZlecajacy);
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

        #region DeleteCommand
        #region CanExecute
        [Test]
        public void DeleteCommandCanExecute_GdyIdZleceniaJestZero_False()
        {
            sut.Naglowek.IDProdukcjaZlecenieCiecia = 0;

            var actual = sut.DeleteCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }
        [Test]
        public void DeleteCommandCanExecute_GdyIdZleceniaNieJestZero_True()
        {
            sut.Naglowek.IDProdukcjaZlecenieCiecia = 1;

            var actual = sut.DeleteCommand.CanExecute(null);

            Assert.IsTrue(actual);
        }
        #endregion
        #region Execute
        [Test]
        public void DeleteCommandExecute_DialogZPytaniemJestWyswietlony()
        {
            sut.Naglowek.IDProdukcjaZlecenieCiecia = 1;

            sut.DeleteCommand.Execute(null);

            DialogService.Verify(v => v.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        public void DeleteCommandExecute_Dialog_True_UsunTowarAPotemZlecenie()
        {
            sut.Naglowek.IDProdukcjaZlecenieCiecia = 1;
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            
            string callOrder = "";
            zlecenieCieciaTowarViewModel.Setup(s => s.DeleteAsync(It.IsAny<int>())).Callback(() => callOrder += "1");
            tblProdukcjaZlecenieCiecia.Setup(s => s.Remove(It.IsAny<tblProdukcjaZlecenieCiecia>())).Callback(() => callOrder += "2");
            UnitOfWork.Setup(s => s.SaveAsync()).Callback(() => callOrder += "3");

            sut.DeleteCommand.Execute(null);

            Assert.AreEqual("123", callOrder);
        }


        [Test]
        public void DeleteCommandExecute_PoUsunieciuWyslijMessage()
        {
            sut.Naglowek.IDProdukcjaZlecenieCiecia = 1;
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            sut.DeleteCommand.Execute(null);

            Messenger.Verify(v => v.Send(It.IsAny<tblProdukcjaZlecenieCiecia>()));
        }
        [Test]
        public void DeleteCommandExecute_PoUsunieciuZamknijOkno()
        {
            sut.Naglowek.IDProdukcjaZlecenieCiecia = 1;
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            sut.DeleteCommand.Execute(null);

            ViewService.Verify(v => v.Close(sut.GetType().Name));
        }
        [Test]
        public void DeleteCommandExecute_PoUsunieciuWyswietlInformacje()
        {
            sut.Naglowek.IDProdukcjaZlecenieCiecia = 1;
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            sut.DeleteCommand.Execute(null);

            DialogService.Verify(v => v.ShowInfo_BtnOK(It.IsAny<string>(), It.IsAny<string>()));
        }
        #endregion
        #endregion

        #region SaveCommand
        #region CanExecute
        [Test]
        public void SaveCommandCanExecute_GdyIsValidJestFalse_False()
        {
            sut.Naglowek.IsValid= false;

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
            sut.Naglowek = new tblProdukcjaZlecenieCiecia
            {
                IDKontrahent = 1,
                IDWykonujacy = 1,
                IDZlecajacy = 2,
                DataZlecenia = DateTime.Now,
                DataWykonania = DateTime.Now,
            };
            //sut.Naglowek.IsValid = true;
            zlecenieCieciaTowarViewModel.Setup(s => s.IsValid).Returns(true);


        }
        #endregion
        #region Execute
        [Test]
        public void SaveCommandExecute_GdyIdZlecCeiciaJestZero_AddISaveAsyncJestWywolane()
        {
            SaveCanExecute_True();
            sut.Naglowek.IDProdukcjaZlecenieCiecia = 0;

            sut.SaveCommand.Execute(null);

            tblProdukcjaZlecenieCiecia.Verify(v => v.Add(It.IsAny<tblProdukcjaZlecenieCiecia>()));
            UnitOfWork.Verify(v => v.SaveAsync());
        }

        [Test]
        public void SaveCommandExecute_GdyIdZlecCeiciaNieJestZero_TylkoSaveAsyncJestWywolane()
        {
            SaveCanExecute_True();
            sut.Naglowek.IDProdukcjaZlecenieCiecia = 1;

            sut.SaveCommand.Execute(null);

            tblProdukcjaZlecenieCiecia.Verify(v => v.Add(It.IsAny<tblProdukcjaZlecenieCiecia>()),Times.Never);
            UnitOfWork.Verify(v => v.SaveAsync());
        }

        [Test]
        public void SaveCommandExecute_KolejnoscMetodGdyIdJestZero()
        {
            SaveCanExecute_True();
            sut.Naglowek.IDProdukcjaZlecenieCiecia = 0;

            string callOrder = "";
            tblProdukcjaZlecenieCiecia.Setup(s => s.Add(It.IsAny<tblProdukcjaZlecenieCiecia>())).Callback(() => callOrder += "1");
            UnitOfWork.Setup(s => s.SaveAsync()).Callback(() => callOrder += "2");
            zlecenieCieciaTowarViewModel.Setup(s => s.SaveAsync(It.IsAny<int>())).Callback(() => callOrder += "3");


            sut.SaveCommand.Execute(null);


            Assert.AreEqual("123", callOrder);
        }

        [Test]
        public void SaveCommandExecute_KolejnoscMetodGdyIdNieJestZero()
        {
            SaveCanExecute_True();
            sut.Naglowek.IDProdukcjaZlecenieCiecia = 1;

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
            sut.Naglowek.IDProdukcjaZlecenieCiecia = 1;

            sut.SaveCommand.Execute(null);

            DialogService.Verify(v => v.ShowInfo_BtnOK(It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        public void SaveCommandExecute_PoZapisieWyslijMessage()
        {
            SaveCanExecute_True();
            sut.Naglowek.IDProdukcjaZlecenieCiecia = 1;

            sut.SaveCommand.Execute(null);

            Messenger.Verify(v => v.Send(It.IsAny<tblProdukcjaZlecenieCiecia>()));
        }
        [Test]
        public void SaveCommandExecute_PoZapisieZamknijOkno()
        {
            SaveCanExecute_True();
            sut.Naglowek.IDProdukcjaZlecenieCiecia = 1;

            sut.SaveCommand.Execute(null);

            ViewService.Verify(v => v.Close(sut.GetType().Name));
        }
        #endregion
        #endregion
    }
}
