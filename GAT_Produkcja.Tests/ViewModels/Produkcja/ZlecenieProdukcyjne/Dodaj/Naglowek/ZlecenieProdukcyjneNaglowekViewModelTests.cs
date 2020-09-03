using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.BaseClasses.Messages;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Dodaj.Messages;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Mieszanka;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Naglowek;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Towar;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Towar.LiniaKalandra;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Towar.LiniaWloknin;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.ZlecenieProdukcyjne.Dodaj.Naglowek
{
    public class ZlecenieProdukcyjneNaglowekViewModelTests : TestBase
    {

        private Mock<IZlecenieProdukcyjneMieszankaViewModel> mieszankaVM;
        private Mock<IZlecenieProdukcyjneTowarViewModel> towarVM;
        private Mock<IZlecenieProdukcyjneTowarLiniaWlokninViewModel> liniaWlokninVM;
        private Mock<IZlecenieProdukcyjneTowarLiniaKalandraViewModel> liniaKalandraVM;
        private Mock<ITblPracownikGATRepository> tblPracownikGAT;
        private Mock<ITblProdukcjaZlecenieStatusRepository> tblProdukcjaZlecenieStatus;
        private Mock<ITblProdukcjaZlecenieRepository> tblProdukcjaZlcecenieProdukcyjne;
        private ZlecenieProdukcyjneNaglowekViewModel sut;

        public override void SetUp()
        {
            base.SetUp();

            mieszankaVM = new Mock<IZlecenieProdukcyjneMieszankaViewModel>();
            towarVM = new Mock<IZlecenieProdukcyjneTowarViewModel>();
            liniaWlokninVM = new Mock<IZlecenieProdukcyjneTowarLiniaWlokninViewModel>();
            liniaKalandraVM = new Mock<IZlecenieProdukcyjneTowarLiniaKalandraViewModel>();

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
            sut = new ZlecenieProdukcyjneNaglowekViewModel(ViewModelService.Object,
                                                           mieszankaVM.Object,
                                                           liniaWlokninVM.Object,
                                                           liniaKalandraVM.Object);
        }

        #region Messengers
        #region Rejestracja
        [Test]
        public void RejestracjaMessengerow()
        {
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<tblProdukcjaZlecenieTowar>>(), It.IsAny<bool>()));
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<tblProdukcjaZlecenie>>(), It.IsAny<bool>())); // na potrzeby podsumowania ZP z mieszankaVM
            //Messenger.Verify(v => v.Register(sut, It.IsAny<Action<EdytujTowarMessage>>(), It.IsAny<bool>()));

        }
        #endregion


        #region GdyPrzeslanoZlecenieProdukcyjne
        [Test]
        public void GdyPrzeslanoZlecenieProdukcyjne_GdyZlecenieNieNull_WypelniajPodsumowaniaZlecenia()
        {
            MessengerSend(new tblProdukcjaZlecenie { WartoscMieszanki_zl = 1, CenaMieszanki_zl = 1, UdzialSurowcowWMieszance = 1 });

            Assert.AreEqual(1, sut.VMEntity.WartoscMieszanki_zl);
            Assert.AreEqual(1, sut.VMEntity.CenaMieszanki_zl);
            Assert.AreEqual(1, sut.VMEntity.UdzialSurowcowWMieszance);
        }

        [Test]
        public void GdyPrzeslanoZlecenieProdukcyjne_GdyZlecenieNull_NieWypelniajPodsumowaniaZlecenia()
        {

            MessengerSend((tblProdukcjaZlecenie)null);

            Assert.AreEqual(0, sut.VMEntity.WartoscMieszanki_zl);
            Assert.AreEqual(0, sut.VMEntity.CenaMieszanki_zl);
            Assert.AreEqual(0, sut.VMEntity.UdzialSurowcowWMieszance);
        }

        #endregion

        #region GdyPrzeslanoEdycjaMessage
        [Test]
        public async    Task GdyPrzeslanoEdycjaMessage_GdyObjNull_NicNieRob()
        {
            MessengerSend((tblProdukcjaZlecenieTowar)null);

            await sut.LoadAsync();

            tblProdukcjaZlcecenieProdukcyjne.Verify(v => v.GetNewNumberAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenie, bool>>>(),
                                                                             It.IsAny<Func<tblProdukcjaZlecenie, int>>()));
        }



        #endregion

        #endregion

        #region LoadCommandExecute
        [Test]
        public async Task LoadCommandExecute_GdyNiePrzeslanoZleceniaTowar_PobieraZBazyNowyNrZleceniaIPelnyNrDokumentu()
        {
            await sut.LoadAsync();


            tblProdukcjaZlcecenieProdukcyjne.Verify(v => v.GetNewNumberAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenie, bool>>>(),
                                                                             It.IsAny<Func<tblProdukcjaZlecenie, int>>()));

            tblProdukcjaZlcecenieProdukcyjne.Verify(v => v.GetNewFullNumberAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenie, bool>>>(),
                                                                                 It.IsAny<Func<tblProdukcjaZlecenie, int>>(),
                                                                                 It.IsAny<string>()));
        }


        [Test]
        public async Task LoadCommandExecute_GdyNiePrzeslanoZleceniaTowar_NadajWlasciwyStatus()
        {

            await sut.LoadAsync();

            Assert.AreEqual((int)ProdukcjaZlecenieStatusEnum.Oczekuje, sut.VMEntity.IDProdukcjaZlecenieStatus);
        }



        [Test]
        public async Task LoadCommandExecute_GdyPrzeslanoZleceniaTowar_PobierzZBazyZlecenieProdukcyjne()
        {
            MessengerSend(new tblProdukcjaZlecenieTowar() { IDProdukcjaGniazdoProdukcyjne = 1 });
            tblProdukcjaZlcecenieProdukcyjne.Setup(s => s.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenie, bool>>>())).ReturnsAsync(new db.tblProdukcjaZlecenie
            {
                IDProdukcjaZlecenie = 1,
                IDProdukcjaZlecenieStatus = 2
            });

            //await Task.Run(()=> sut.LoadCommand.Execute(null));
            await sut.LoadAsync();

            tblProdukcjaZlcecenieProdukcyjne.Verify(v => v.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenie, bool>>>()));
        }

        [Test]
        public async Task LoadCommandExecute_GdyNiePrzeslanoZleceniaTowar_OkreslStatus()
        {
            await sut.LoadAsync();

            Assert.AreEqual((int)ProdukcjaZlecenieStatusEnum.Oczekuje, sut.VMEntity.IDProdukcjaZlecenieStatus);
        }

        [Test]
        public async Task LoadCommandExecute_GdyPrzeslanoZleceniaTowar_StatusPozostajeNiezmienony()
        {
            MessengerSend(new tblProdukcjaZlecenieTowar() { IDProdukcjaGniazdoProdukcyjne = 1, IDProdukcjaZlecenieStatus = 2 });
            tblProdukcjaZlcecenieProdukcyjne.Setup(s => s.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenie, bool>>>())).ReturnsAsync(new db.tblProdukcjaZlecenie
            {
                IDProdukcjaZlecenieStatus = 2
            });

            await sut.LoadAsync();

            Assert.AreEqual(2, sut.VMEntity.IDProdukcjaZlecenieStatus);
        }

        [Test]
        public async Task LoadCommandExecute_GdyPrzeslanoZleceniaTowar_ZaladujChildViewModels()
        {
            MessengerSend(new tblProdukcjaZlecenieTowar() { IDProdukcjaZlecenie = 1, IDProdukcjaGniazdoProdukcyjne = 1, IDProdukcjaZlecenieStatus = 2 });
            tblProdukcjaZlcecenieProdukcyjne.Setup(s => s.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenie, bool>>>())).ReturnsAsync(new db.tblProdukcjaZlecenie
            {
                IDProdukcjaZlecenie = 1,
                IDProdukcjaZlecenieStatus = 2
            });

            await sut.LoadAsync();

            mieszankaVM.Verify(v => v.LoadAsync(1));
            liniaWlokninVM.Verify(v => v.LoadAsync(1));
            liniaKalandraVM.Verify(v => v.LoadAsync(1));
        }


        #endregion

        #region SaveCommand
        #region CanExecute

        [Test]
        public void SaveCommandCanExecute_GdyListaWlokninIKalandraPusta_Zwraca_False()
        {
            SaveCommandExecute_True();

            liniaKalandraVM.Setup(x => x.ListOfVMEntities).Returns(new ObservableCollection<tblProdukcjaZlecenieTowar>());
            liniaWlokninVM.Setup(x => x.ListOfVMEntities).Returns(new ObservableCollection<tblProdukcjaZlecenieTowar>());

            var actual = sut.SaveCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }



        [Test]
        [Ignore("Zmieniono logike, mieszanka moze byc pusta!")]
        public void SaveCommandCanExecute_GdyIsValid_True_Zwraca_True()
        {
            SaveCommandExecute_True();

            mieszankaVM.Setup(x => x.IsValid).Returns(true);
            liniaKalandraVM.Setup(x => x.ListOfVMEntities).Returns(new ObservableCollection<tblProdukcjaZlecenieTowar>());
            liniaWlokninVM.Setup(x => x.ListOfVMEntities).Returns(new ObservableCollection<tblProdukcjaZlecenieTowar>());

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
            sut.VMEntity = new tblProdukcjaZlecenie
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
            liniaKalandraVM.Setup(x => x.ListOfVMEntities).Returns(new ObservableCollection<tblProdukcjaZlecenieTowar>() { new tblProdukcjaZlecenieTowar() });
            liniaWlokninVM.Setup(x => x.ListOfVMEntities).Returns(new ObservableCollection<tblProdukcjaZlecenieTowar>());

            //mieszankaVM.Setup(x => x.IsValid).Returns(true);

        }

        #endregion


        #region Execute
        [Test]
        public void SaveCommandExcute_IDPracownikaGAT_NieJestZerowe()
        {
            SaveCommandExecute_True();

            sut.SaveCommand.Execute(null);

            Assert.IsTrue(sut.VMEntity.IDZlecajacy != 0);
        }

        [Test]
        public void SaveCommandExcute_GdyZlecenieNowe_IDStatus_Oczekuje()
        {
            SaveCommandExecute_True();

            sut.SaveCommand.Execute(null);

            Assert.IsTrue(sut.VMEntity.IDProdukcjaZlecenie == 0);
            Assert.IsTrue(sut.VMEntity.IDProdukcjaZlecenieStatus == (int)ProdukcjaZlecenieStatusEnum.Oczekuje);
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
            sut.VMEntity.IDProdukcjaZlecenie = 1;

            sut.SaveCommand.Execute(null);

            tblProdukcjaZlcecenieProdukcyjne.Verify(v => v.Add(It.IsAny<tblProdukcjaZlecenie>()), Times.Never);
            UnitOfWork.Verify(v => v.SaveAsync());
        }

        [Test]
        public void SaveCommandExecute_PoZapisieZleceniaProd_WywolajMetodySaveWrazZOdpowiednimNaChildFormularzach()
        {
            SaveCommandExecute_True();
            sut.VMEntity.IDProdukcjaZlecenie = 1;

            sut.SaveCommand.Execute(null);

            mieszankaVM.Verify(v => v.SaveAsync(1));
            liniaWlokninVM.Verify(v => v.SaveAsync(1));
            liniaKalandraVM.Verify(v => v.SaveAsync(1));
        }


        [Test]
        public void SaveCommandExecute_OkreślDateUtworzeniaBezposrednioPrzedZapisem()
        {
            SaveCommandExecute_True();

            sut.SaveCommand.Execute(null);

            Assert.AreNotEqual(new DateTime(0001, 1, 1), sut.VMEntity.DataUtworzenia);
        }


        [Test]
        public void SaveCommandExecute_WyslijMessagePoZapisie()
        {
            SaveCommandExecute_True();

            sut.SaveCommand.Execute(null);

            Messenger.Verify(v => v.Send(sut.VMEntity, nameof(RefreshListMessage)));
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
            Messenger.Setup(s => s.Send(sut.VMEntity, nameof(RefreshListMessage))).Callback(() => kolejnosc += "1");
            ViewService.Setup(s => s.Close(sut.GetType().Name)).Callback(() => kolejnosc += "2");

            sut.SaveCommand.Execute(null);

            Assert.AreEqual("12", kolejnosc);
        }

        #endregion

        #endregion

    }
}
