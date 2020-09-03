using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Dodaj.Messages;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Mieszanka;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Naglowek;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Towar;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.ZlecenieProdukcyjne.Dodaj.Naglowek
{
    public class ZlecenieProdukcyjneNaglowekViewModelTests_old : TestBase
    {
        private Mock<IZlecenieProdukcyjneMieszankaViewModel> mieszankaVM;
        private Mock<IZlecenieProdukcyjneTowarViewModel> towarVM;
        private Mock<ITblPracownikGATRepository> tblPracownikGAT;
        private Mock<ITblProdukcjaZlecenieStatusRepository> tblProdukcjaZlecenieStatus;
        private Mock<ITblProdukcjaZlecenieRepository> tblProdukcjaZlcecenieProdukcyjne;
        private ZlecenieProdukcyjneNaglowekViewModel_old sut;

        public override void SetUp()
        {
            base.SetUp();

            mieszankaVM = new Mock<IZlecenieProdukcyjneMieszankaViewModel>();
            towarVM = new Mock<IZlecenieProdukcyjneTowarViewModel>();

            tblPracownikGAT = new Mock<ITblPracownikGATRepository>();
            UnitOfWork.Setup(s => s.tblPracownikGAT).Returns(tblPracownikGAT.Object);

            tblProdukcjaZlecenieStatus = new Mock<ITblProdukcjaZlecenieStatusRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaZlecenieStatus).Returns(tblProdukcjaZlecenieStatus.Object);

            tblProdukcjaZlcecenieProdukcyjne = new Mock<ITblProdukcjaZlecenieRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaZlecenie).Returns(tblProdukcjaZlcecenieProdukcyjne.Object);

            UnitOfWorkFactory.Setup(s => s.Create()).Returns(UnitOfWork.Object);

            CreateSut();
        }

        public override void CreateSut()
        {
            sut = new ZlecenieProdukcyjneNaglowekViewModel_old(ViewModelService.Object,
                                                           mieszankaVM.Object,
                                                           towarVM.Object);
        }

        #region Messengers
        #region Rejestracja
        [Test]
        public void RejestracjaMessengerow()
        {
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<tblProdukcjaZlecenieTowar>>(), It.IsAny<bool>()));
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<tblProdukcjaZlecenie>>(), It.IsAny<bool>())); // na potrzeby podsumowania ZP z mieszankaVM
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<EdytujTowarMessage>>(), It.IsAny<bool>()));

        }
        #endregion

        #region GdyPrzeslanoZlecenieTowar

        #endregion

        #region GdyPrzeslanoZlecenieProdukcyjne
        [Test]
        public void GdyPrzeslanoZlecenieProdukcyjne_GdyZlecenieNieNull_WypelniajPodsumowaniaZlecenia()
        {
            MessengerSend(new tblProdukcjaZlecenie { WartoscMieszanki_zl = 1, CenaMieszanki_zl = 1, UdzialSurowcowWMieszance = 1 });

            Assert.AreEqual(1, sut.ZlecenieProdukcyjne.WartoscMieszanki_zl);
            Assert.AreEqual(1, sut.ZlecenieProdukcyjne.CenaMieszanki_zl);
            Assert.AreEqual(1, sut.ZlecenieProdukcyjne.UdzialSurowcowWMieszance);
        }

        [Test]
        public void GdyPrzeslanoZlecenieProdukcyjne_GdyZlecenieNull_NieWypelniajPodsumowaniaZlecenia()
        {

            MessengerSend((tblProdukcjaZlecenie)null);

            Assert.AreEqual(0, sut.ZlecenieProdukcyjne.WartoscMieszanki_zl);
            Assert.AreEqual(0, sut.ZlecenieProdukcyjne.CenaMieszanki_zl);
            Assert.AreEqual(0, sut.ZlecenieProdukcyjne.UdzialSurowcowWMieszance);
        }

        #endregion

        #region GdyPrzeslanoEdycjaMessage
        [Test]
        public void GdyPrzeslanoEdycjaMessage_GdyObjNull_NicNieRob()
        {
            MessengerSend((EdytujTowarMessage)null);

            sut.LoadCommand.Execute(null);

            tblProdukcjaZlcecenieProdukcyjne.Verify(v => v.GetNewNumberAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenie, bool>>>(),
                                                                             It.IsAny<Func<tblProdukcjaZlecenie, int>>()));
        }

        [Test]
        public void GdyPrzeslanoEdycjaMessage_GdyTowarNull_NicNieRob()
        {
            MessengerSend(new EdytujTowarMessage { Towar = null });

            sut.LoadCommand.Execute(null);

            tblProdukcjaZlcecenieProdukcyjne.Verify(v => v.GetNewNumberAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenie, bool>>>(),
                                                                             It.IsAny<Func<tblProdukcjaZlecenie, int>>()));
        }


        [Test]
        public void GdyPrzeslanoEdycjaMessage_GdyObjNieNull_PrzypiszZlecenieTowar()
        {
            MessengerSend(new EdytujTowarMessage { Towar = new tblProdukcjaZlecenieTowar() });

            sut.LoadCommand.Execute(null);

            tblProdukcjaZlcecenieProdukcyjne.Verify(v => v.GetNewNumberAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenie, bool>>>(),
                                                                             It.IsAny<Func<tblProdukcjaZlecenie, int>>())
                                                                             ,Times.Never);
        }


        #endregion

        #endregion

        #region LoadCommandExecute
        [Test]
        public void LoadCommandExecute_GdyNiePrzeslanoZleceniaTowar_PobieraZBazyNowyNrZleceniaIPelnyNrDokumentu()
        {

            sut.LoadCommand.Execute(null);


            tblProdukcjaZlcecenieProdukcyjne.Verify(v => v.GetNewNumberAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenie, bool>>>(),
                                                                             It.IsAny<Func<tblProdukcjaZlecenie, int>>()));

            tblProdukcjaZlcecenieProdukcyjne.Verify(v => v.GetNewFullNumberAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenie, bool>>>(),
                                                                                 It.IsAny<Func<tblProdukcjaZlecenie, int>>(),
                                                                                 It.IsAny<string>()));

        }


        [Test]
        public void LoadCommandExecute_GdyNiePrzeslanoZleceniaTowar_NadajWlasciwyStatus()
        {

            sut.LoadCommand.Execute(null);

            Assert.AreEqual((int)ProdukcjaZlecenieStatusEnum.Oczekuje, sut.ZlecenieProdukcyjne.IDProdukcjaZlecenieStatus);
        }



        [Test]
        public void LoadCommandExecute_GdyPrzeslanoZleceniaTowar_PobierzZBazyZlecenieProdukcyjne()
        {
            MessengerSend(new tblProdukcjaZlecenieTowar() { IDProdukcjaGniazdoProdukcyjne = 1 });

            sut.LoadCommand.Execute(null);

            tblProdukcjaZlcecenieProdukcyjne.Verify(v => v.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenie, bool>>>()));
        }

        [Test]
        public void LoadCommandExecute_GdyNiePrzeslanoZleceniaTowar_OkreslStatus()
        {
            sut.LoadCommand.Execute(null);

            Assert.AreEqual((int)ProdukcjaZlecenieStatusEnum.Oczekuje, sut.ZlecenieProdukcyjne.IDProdukcjaZlecenieStatus);
        }

        [Test]
        public void LoadCommandExecute_GdyPrzeslanoZleceniaTowar_StatusPozostajeNiezmienony()
        {
            MessengerSend(new tblProdukcjaZlecenieTowar() { IDProdukcjaGniazdoProdukcyjne = 1, IDProdukcjaZlecenieStatus = 2 });
            tblProdukcjaZlcecenieProdukcyjne.Setup(s => s.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenie, bool>>>())).ReturnsAsync(new db.tblProdukcjaZlecenie
            {
                IDProdukcjaZlecenieStatus = 2
            });

            sut.LoadCommand.Execute(null);

            Assert.AreEqual(2, sut.ZlecenieProdukcyjne.IDProdukcjaZlecenieStatus);
        }

        [Test]
        public void LoadCommandExecute_GdyPrzeslanoZleceniaTowar_ZaladujChildViewModels()
        {
            MessengerSend(new tblProdukcjaZlecenieTowar() { IDProdukcjaZlecenie = 1, IDProdukcjaGniazdoProdukcyjne = 1, IDProdukcjaZlecenieStatus = 2 });
            tblProdukcjaZlcecenieProdukcyjne.Setup(s => s.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenie, bool>>>())).ReturnsAsync(new db.tblProdukcjaZlecenie
            {
                IDProdukcjaZlecenie = 1,
                IDProdukcjaZlecenieStatus = 2
            });

            sut.LoadCommand.Execute(null);

            mieszankaVM.Verify(v => v.LoadAsync(1));
            towarVM.Verify(v => v.LoadAsync(1));
        }


        #endregion

        #region SaveCommand
        #region CanExecute
        [Test]
        public void SaveCommandCanExecute_GdyIsValid_True_Zwraca_True()
        {
            SaveCommandExecute_True();

            mieszankaVM.Setup(x => x.IsValid).Returns(true);

            var actual = sut.SaveCommand.CanExecute(null);

            Assert.IsTrue(actual);
        }

        [Test]
        public void SaveCommandCanExecute_GdyIsValid_False_Zwraca_False()
        {
            mieszankaVM.Setup(x => x.IsValid).Returns(true);

            var actual = sut.SaveCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }

        private void SaveCommandExecute_True()
        {
            sut.ZlecenieProdukcyjne = new tblProdukcjaZlecenie
            {
                IDZlecajacy = 1,
                NrZlecenia = 1,
                NazwaZlecenia = "Test",
                DataRozpoczecia = new DateTime(2002, 1, 1),
                DataZakonczenia = new DateTime(2002, 1, 2),
                WartoscMieszanki_zl = 1,
                CenaMieszanki_zl = 1,
                UdzialSurowcowWMieszance = 1
            };
            mieszankaVM.Setup(x => x.IsValid).Returns(true);
        }

        #endregion


        #region Execute
        [Test]
        public void SaveCommandExcute_IDPracownikaGAT_NieJestZerowe()
        {
            SaveCommandExecute_True();

            sut.SaveCommand.Execute(null);

            Assert.IsTrue(sut.ZlecenieProdukcyjne.IDZlecajacy != 0);
        }

        [Test]
        public void SaveCommandExcute_IDStatus_Oczekuje()
        {
            SaveCommandExecute_True();

            sut.SaveCommand.Execute(null);

            Assert.IsTrue(sut.ZlecenieProdukcyjne.IDProdukcjaZlecenieStatus == (int)ProdukcjaZlecenieStatusEnum.Oczekuje);
        }

        [Test]
        public void SaveCommandExecute_GdyZlecenieNowe_DodajeDoBazy()
        {
            SaveCommandExecute_True();

            sut.SaveCommand.Execute(null);

            tblProdukcjaZlcecenieProdukcyjne.Verify(v => v.Add(It.IsAny<tblProdukcjaZlecenie>()));
            UnitOfWork.Verify(v => v.SaveAsync());
        }

        [Test]
        public void SaveCommandExecute_GdyZlecenieEdytowane_UpdateWBazie()
        {
            SaveCommandExecute_True();
            sut.ZlecenieProdukcyjne.IDProdukcjaZlecenie = 1;

            sut.SaveCommand.Execute(null);

            tblProdukcjaZlcecenieProdukcyjne.Verify(v => v.Add(It.IsAny<tblProdukcjaZlecenie>()), Times.Never);
            UnitOfWork.Verify(v => v.SaveAsync());
        }

        [Test]
        public void SaveCommandExecute_PoZapisieZleceniaProd_WywolajMetodySaveWrazZOdpowiednimNaChildFormularzach()
        {
            SaveCommandExecute_True();
            sut.ZlecenieProdukcyjne.IDProdukcjaZlecenie = 1;

            sut.SaveCommand.Execute(null);

            mieszankaVM.Verify(v => v.SaveAsync(1));
            towarVM.Verify(v => v.SaveAsync(1));
        }


        [Test]
        public void SaveCommandExecute_OkreślDateUtworzeniaBezposrednioPrzedZapisem()
        {
            SaveCommandExecute_True();

            sut.SaveCommand.Execute(null);

            Assert.AreNotEqual(new DateTime(0001, 1, 1), sut.ZlecenieProdukcyjne.DataUtworzenia);
        }


        [Test]
        public void SaveCommandExecute_WyslijMessagePoZapisie()
        {
            SaveCommandExecute_True();

            sut.SaveCommand.Execute(null);

            Messenger.Verify(v => v.Send(sut.ZlecenieProdukcyjne));
        }

        [Test]
        public void SaveCommandExecute_ZamknijOknoPoWyslaniuWiadomosci()
        {
            SaveCommandExecute_True();

            sut.SaveCommand.Execute(null);

            ViewService.Verify(v => v.Close(sut.GetType().Name));
        }

        [Test]
        public void SaveCommandExecute_KolejnoscWywolywaniaMetod()
        {
            SaveCommandExecute_True();
            string kolejnosc = string.Empty;
            Messenger.Setup(s => s.Send(sut.ZlecenieProdukcyjne)).Callback(() => kolejnosc += "1");
            ViewService.Setup(s => s.Close(sut.GetType().Name)).Callback(() => kolejnosc += "2");

            sut.SaveCommand.Execute(null);

            Assert.AreEqual("12", kolejnosc);
        }

        #endregion

        #endregion

        #region DeleteCommand
        #region CanExecute
        [Test]
        public void DeleteCommandCanExecute_GdyZlecProdIdJestZero_False()
        {
            var actual = sut.DeleteCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }
        #endregion

        #region Execute
        [Test]
        public void DeleteCommandExecute_GdyIdNieZero_ZapytajCzyUsunac()
        {
            sut.ZlecenieProdukcyjne.IDProdukcjaZlecenie = 1;

            sut.DeleteCommand.Execute(null);

            DialogService.Verify(v => v.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>()));

        }

        [Test]
        public void DeleteCommandExecute_DialogServiceTrue_UsunZTabelZaleznych()
        {
            sut.ZlecenieProdukcyjne.IDProdukcjaZlecenie = 1;
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            sut.DeleteCommand.Execute(null);

            mieszankaVM.Verify(v => v.DeleteAsync(1));
            towarVM.Verify(v => v.DeleteAsync(1));
        }

        [Test]
        public void DeleteCommandExecute_DialogServiceTrue_UsunZTabeliZleceniaProd()
        {
            sut.ZlecenieProdukcyjne.IDProdukcjaZlecenie = 1;
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            sut.DeleteCommand.Execute(null);

            tblProdukcjaZlcecenieProdukcyjne.Verify(v => v.Remove(sut.ZlecenieProdukcyjne));
            UnitOfWork.Verify(v => v.SaveAsync());
        }

        [Test]
        public void DeleteCommandExecute_DialogServiceTrue_KolejnoscMetod()
        {
            sut.ZlecenieProdukcyjne.IDProdukcjaZlecenie = 1;
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            string kolejnosc = string.Empty;
            mieszankaVM.Setup(v => v.DeleteAsync(1)).Callback(() => kolejnosc += "1");
            towarVM.Setup(s => s.DeleteAsync(1)).Callback(() => kolejnosc += "2");
            tblProdukcjaZlcecenieProdukcyjne.Setup(s => s.Remove(sut.ZlecenieProdukcyjne)).Callback(() => kolejnosc += "3");
            UnitOfWork.Setup(s => s.SaveAsync()).Callback(() => kolejnosc += "4");


            sut.DeleteCommand.Execute(null);

            Assert.AreEqual("1234", kolejnosc);
        }

        #endregion

        #endregion
    }
}
