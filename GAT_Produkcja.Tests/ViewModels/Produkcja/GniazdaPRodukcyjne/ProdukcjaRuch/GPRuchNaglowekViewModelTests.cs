using AutoFixture;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.BaseClasses.Messages;
using GAT_Produkcja.Utilities.ZebraPrinter;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.Messages;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Ewidencja;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Naglowek;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Naglowek.States;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Messages;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.RW;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Dodaj.Naglowek;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Ewidencja;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieDodajTowar;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Naglowek;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Ewidencja;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar
{
    public class GPRuchNaglowekViewModelTests : TestBase
    {
        private Fixture fixture;
        private Mock<IMagazynRuchNaglowekSaveHelper> magazynRuchNaglowekSaveHelper;
        private Mock<IMagazynRuchTowarSaveHelper> magazynRuchTowaruSaveHelper;
        private Mock<IZebraLabelPrinter> zebraLabelPrinter;
        private Mock<IGPRuchTowarRWViewModel> vmRW;
        private Mock<IGPRuchTowarPWViewModel> vmPW;
        private Mock<IGPRuchTowar_Naglowek_Helper> naglowekHelper;
        private Mock<IGPRuchNaglowekStateFactory> stateFactory;
        private Mock<IGPRuchNaglowekState> state;
        private Mock<ITblPracownikGATRepository> tblPracownikGAT;
        private Mock<ITblProdukcjaRuchNaglowekRepository> tblProdukcjaRuchNaglowek;
        private Mock<ITblProdukcjaZlecenieRepository> tblProdukcjaZlcecenieProdukcyjne;
        private Mock<ITblProdukcjaZlecenieCieciaRepository> tblProdukcjaZlecenieCiecia;
        private Mock<ITblProdukcjaZlecenieTowarRepository> tblProdukcjaZlecenieCieciaTowar;
        private Mock<ITblProdukcjaGniazdoWlokninaRepository> tblProdukcjaGniazdoWloknina;
        private Mock<ITblProdukcjaGniazdoProdukcyjneRepository> tblProdukcjaGniazdoProdukcyjne;
        private Mock<ITblProdukcjaRuchTowarRepository> tblProdukcjaRuchTowar;
        private Mock<ITblProdukcjaZlecenieTowarRepository> tblProdukcjaZlecenieTowar;
        private GPRuchNaglowekViewModel sut;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            fixture = new Fixture();
            zebraLabelPrinter = new Mock<IZebraLabelPrinter>();
            vmRW = new Mock<IGPRuchTowarRWViewModel>();
            vmPW = new Mock<IGPRuchTowarPWViewModel>();
            magazynRuchTowaruSaveHelper = new Mock<IMagazynRuchTowarSaveHelper>();
            magazynRuchNaglowekSaveHelper = new Mock<IMagazynRuchNaglowekSaveHelper>();
            naglowekHelper = new Mock<IGPRuchTowar_Naglowek_Helper>();
            stateFactory = new Mock<IGPRuchNaglowekStateFactory>();
            state = new Mock<IGPRuchNaglowekState>();

            naglowekHelper.Setup(s => s.MagazynRuchNaglowekSaveHelper).Returns(magazynRuchNaglowekSaveHelper.Object);
            naglowekHelper.Setup(s => s.MagazynRuchTowarSaveHelper).Returns(magazynRuchTowaruSaveHelper.Object);

            stateFactory.Setup(s => s.GetState(It.IsAny<IGPRuchNaglowekViewModel>())).Returns(state.Object);

            tblPracownikGAT = new Mock<ITblPracownikGATRepository>();
            UnitOfWork.Setup(s => s.tblPracownikGAT).Returns(tblPracownikGAT.Object);

            tblProdukcjaRuchNaglowek = new Mock<ITblProdukcjaRuchNaglowekRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaRuchNaglowek).Returns(tblProdukcjaRuchNaglowek.Object);

            tblProdukcjaZlcecenieProdukcyjne = new Mock<ITblProdukcjaZlecenieRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaZlecenie).Returns(tblProdukcjaZlcecenieProdukcyjne.Object);

            tblProdukcjaZlecenieCiecia = new Mock<ITblProdukcjaZlecenieCieciaRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaZlecenieCiecia).Returns(tblProdukcjaZlecenieCiecia.Object);

            tblProdukcjaZlecenieCieciaTowar = new Mock<ITblProdukcjaZlecenieTowarRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaZlecenieTowar).Returns(tblProdukcjaZlecenieCieciaTowar.Object);

            tblProdukcjaGniazdoWloknina = new Mock<ITblProdukcjaGniazdoWlokninaRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaGniazdoWloknina).Returns(tblProdukcjaGniazdoWloknina.Object);

            tblProdukcjaGniazdoProdukcyjne = new Mock<ITblProdukcjaGniazdoProdukcyjneRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaGniazdoProdukcyjne).Returns(tblProdukcjaGniazdoProdukcyjne.Object);

            tblProdukcjaRuchTowar = new Mock<ITblProdukcjaRuchTowarRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaRuchTowar).Returns(tblProdukcjaRuchTowar.Object);

            tblProdukcjaZlecenieTowar = new Mock<ITblProdukcjaZlecenieTowarRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaZlecenieTowar).Returns(tblProdukcjaZlecenieTowar.Object);

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new GPRuchNaglowekViewModel(ViewModelService.Object,
                                              vmRW.Object,
                                              vmPW.Object,
                                              naglowekHelper.Object,
                                              stateFactory.Object);
        }

        #region Messengery
        [Test]
        public void RejestracjaMessengerow_tblProdukcjaRuchNaglowek()
        {
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<tblProdukcjaGniazdoProdukcyjne>>(), It.IsAny<bool>()));
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<tblProdukcjaZlecenieTowar>>(), It.IsAny<bool>()));
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<GPSaveMessage>>(), It.IsAny<bool>()));

        }

        #region GdyPrzeslanoNaglowek
        [Test]
        public async Task GdyPrzeslanoNaglowek_GdyPrzeslano_NaglowekPobranyZBazy()
        {
            tblProdukcjaRuchNaglowek.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<tblProdukcjaRuchNaglowek>
            {
                new tblProdukcjaRuchNaglowek{IDProdukcjaRuchNaglowek=1}
            });

            MessengerSend(new tblProdukcjaRuchNaglowek { IDProdukcjaRuchNaglowek = 1 });

            await sut.LoadAsync();

            tblProdukcjaRuchNaglowek.Verify(v => v.GetByIdAsync(It.IsAny<int>()));
        }
        [Test]
        public void GdyPrzeslanoNaglowek_GdyPrzeslanoNull_NiePobieramyZBazy()
        {
            tblProdukcjaRuchNaglowek.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<tblProdukcjaRuchNaglowek>
            {
                new tblProdukcjaRuchNaglowek{IDProdukcjaRuchNaglowek=1}
            });

            MessengerSend((tblProdukcjaRuchNaglowek)null);

            sut.LoadCommand.Execute(null);

            Assert.IsNotNull(sut.VMEntity);
        }
        [Test]
        public void GdyPrzeslanoNaglowek_GdyNaglowkaNieMaWBazie_StworzNowyNaglowek()
        {
            tblProdukcjaRuchNaglowek.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<tblProdukcjaRuchNaglowek>
            {
                new tblProdukcjaRuchNaglowek{IDProdukcjaRuchNaglowek=1}
            });

            MessengerSend(new tblProdukcjaRuchNaglowek { IDProdukcjaRuchNaglowek = 2 });
            sut.LoadCommand.Execute(null);

            Assert.IsNotNull(sut.VMEntity);
        }
        #endregion

        #region GdyPrzeslanoGniazdoProdukcyjne
        [Test]
        public async Task GdyPrzeslanoGniazdoProdukcyjne_GniazdoNieNullIdNieZero_PrzypiszDoWybranegoGniazda()
        {
            tblProdukcjaGniazdoProdukcyjne.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<tblProdukcjaGniazdoProdukcyjne>
            {
                new tblProdukcjaGniazdoProdukcyjne{IDProdukcjaGniazdoProdukcyjne=(int)GniazdaProdukcyjneEnum.LiniaWloknin}
            });
            MessengerSend(new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaWloknin });

            await sut.LoadAsync();

            Assert.IsNotNull(sut.WybraneGniazdo);
            Assert.AreEqual((int)GniazdaProdukcyjneEnum.LiniaWloknin, sut.WybraneGniazdo.IDProdukcjaGniazdoProdukcyjne);
        }
        [Test]
        public void GdyPrzeslanoGniazdoProdukcyjne_GniazdaNieMaNaLiscie_WybraneGniazdoJEstNull()
        {
            tblProdukcjaGniazdoProdukcyjne.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<tblProdukcjaGniazdoProdukcyjne>
            {
                new tblProdukcjaGniazdoProdukcyjne{IDProdukcjaGniazdoProdukcyjne=(int)GniazdaProdukcyjneEnum.LiniaWloknin}
            });
            MessengerSend(new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKalandowania });

            sut.LoadCommand.Execute(null);

            Assert.IsNull(sut.WybraneGniazdo);
        }
        #endregion

        #region GdyPrzeslanoZlecenieTowar



        [Test]
        public void GdyPrzeslanoZlecenieTowar_GenerujNazweTowaru()
        {
            tblProdukcjaZlecenieTowar.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new db.tblProdukcjaZlecenieTowar());
            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()));

            MessengerSend(new tblProdukcjaZlecenieTowar
            {
                IDProdukcjaZlecenieCiecia = 0,
                IDTowarGeowlokninaParametryGramatura = 1,
                IDTowarGeowlokninaParametrySurowiec = 1,
                Szerokosc_m = 1,
                Dlugosc_m = 1,
                tblProdukcjaZlecenieCiecia = new tblProdukcjaZlecenieCiecia { tblKontrahent = new tblKontrahent { Nazwa = "KontrahentTest" } },
                tblTowarGeowlokninaParametryGramatura = new tblTowarGeowlokninaParametryGramatura { IDTowarGeowlokninaParametryGramatura = 1, Gramatura = 100 },
                tblTowarGeowlokninaParametrySurowiec = new tblTowarGeowlokninaParametrySurowiec { IDTowarGeowlokninaParametrySurowiec = 1, Skrot = "PP" }
            });

            Assert.IsNotNull(sut.ZlecenieTowar.TowarNazwa);
        }

        [Test]
        public void GdyPrzeslanoZlecenieTowar_ZlecenieTowarNiePuste()
        {
            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()));

            MessengerSend(new tblProdukcjaZlecenieTowar());

            Assert.IsNotNull(sut.ZlecenieTowar);
        }

        [Test]
        public async Task GdyPrzslanoTowar_WybieraGniazdoProdukcyjne()
        {
            tblProdukcjaGniazdoProdukcyjne.Setup(s => s.GetAllAsync())
                                          .ReturnsAsync(new List<tblProdukcjaGniazdoProdukcyjne>
                                          {
                                              new tblProdukcjaGniazdoProdukcyjne{IDProdukcjaGniazdoProdukcyjne=1}
                                          });
            tblProdukcjaRuchNaglowek.Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                                    .ReturnsAsync(new tblProdukcjaRuchNaglowek { IDProdukcjaRuchNaglowek = 1, IDProdukcjaGniazdoProdukcyjne = 1 });
            MessengerSend(new tblProdukcjaRuchNaglowek
            {
                IDProdukcjaGniazdoProdukcyjne = 1,
                IDProdukcjaRuchNaglowek = 1,
            });

            await sut.LoadAsync();

            Assert.IsNotNull(sut.WybraneGniazdo);
        }

        [Test]
        public void GdyPrzslanoTowar_UruchomLoadAsyncNaVMZaleznych()
        {
            MessengerSend(new tblProdukcjaRuchNaglowek
            {
                IDProdukcjaGniazdoProdukcyjne = 1,
                IDProdukcjaRuchNaglowek = 1,
            });

            sut.LoadAdditionally();

            vmPW.Verify(x => x.LoadAsync(It.IsAny<int?>()));
            vmRW.Verify(x => x.LoadAsync(It.IsAny<int?>()));
        }


        [Test]
        public void GdyPrzeslanoZlecenieTowar_KtoreJestWBazie_WysylaMessageDoNaglowkaAbySciagnacWszystkieRolkiDlaTegoZlecenia()
        {
            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()))
                                 .ReturnsAsync(new List<tblProdukcjaRuchTowar>
                                 {
                                    new tblProdukcjaRuchTowar()
                                 });

            MessengerSend(new tblProdukcjaZlecenieTowar());

            Messenger.Verify(v => v.Send(It.IsAny<tblProdukcjaRuchNaglowek>()));
        }

        [Test]
        public void GdyPrzeslanoZlecenieTowar_KtoregoNieMaWBazie_NieWysylaMessaguDoNaglowkaAbySciagnacWszystkieRolkiDlaTegoZlecenia()
        {
            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()));

            MessengerSend(new tblProdukcjaZlecenieTowar());

            Messenger.Verify(v => v.Send(It.IsAny<tblProdukcjaRuchNaglowek>()),Times.Never);
        }

        #endregion

        #region GdyPrzeslanoGPSaveMessage
        [Test]
        public void GdyPrzeslanoSaveMessage_GdyAutoSaveFalse_NieZapisuj()
        {
            MessengerSend(new GPSaveMessage());

            sut.AutoSave = false;

            UnitOfWork.Verify(v => v.SaveAsync(), Times.Never);
        }
        #endregion


        #endregion

        #region Load
        [Test]
        public async Task LoadCommandExecute_MetodyLoadAsyncNaChildVMPowinnyBycUruchomione()
        {
            await sut.LoadAsync();

            vmPW.Verify(v => v.LoadAsync(It.IsAny<int?>()));
            vmRW.Verify(v => v.LoadAsync(It.IsAny<int?>()));
        }

        [Test]
        public async Task LoadCommandExecute_PobieraElementyZBazy()
        {
            await sut.LoadAsync();

            tblPracownikGAT.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<tblPracownikGAT,bool>>>()));
            tblProdukcjaGniazdoProdukcyjne.Verify(v => v.GetAllAsync());
        }
        [Test]
        public async Task LoadCommandExecute_RuchTowarRWViewModelJestWywolywanyZArgIdNaglowka()
        {

            sut.VMEntity = new tblProdukcjaRuchNaglowek { IDProdukcjaRuchNaglowek = 1, IDProdukcjaZlecenieTowar=2 };

            await sut.LoadAsync();

            vmRW.Verify(v => v.LoadAsync(2));
        }
        [Test]
        public async Task LoadCommandExecute_RuchTowarPWViewModelJestWywolywanyZArgIdNaglowka()
        {

            sut.VMEntity = new tblProdukcjaRuchNaglowek { IDProdukcjaRuchNaglowek = 1, IDProdukcjaZlecenieTowar=2 };

            await sut.LoadAsync();

            vmPW.Verify(v => v.LoadAsync(2));
        }



        [Test]
        public void LoadCommandExecute_UzytkownikZalogowanyNull_NieWybieraJPRacownika()
        {
            sut.VMEntity = new tblProdukcjaRuchNaglowek { IDProdukcjaRuchNaglowek = 1 };

            sut.LoadCommand.Execute(null);

            Assert.IsNull(sut.WybranyPracownik_1);
        }

        [Test]
        public async Task LoadCommandExecute_GdyWybraneGniazdoNull_CzyDodajZlecenieButtonAntywny_False()
        {
            await sut.LoadAsync();

            Assert.IsFalse(sut.CzyDodajZlecenieButtonAktywny);
        }

        [Test]
        public async Task LoadCommandExecute_GdyWybraneGniazdoNieJestNull_CzyDodajZlecenieButtonAntywny_True()
        {
            MessengerSend(new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = 1 });
            tblProdukcjaGniazdoProdukcyjne.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<tblProdukcjaGniazdoProdukcyjne>
            {
                new tblProdukcjaGniazdoProdukcyjne{IDProdukcjaGniazdoProdukcyjne=1},
                new tblProdukcjaGniazdoProdukcyjne{IDProdukcjaGniazdoProdukcyjne=2},
            });

            await sut.LoadAsync();

            Assert.IsTrue(sut.CzyDodajZlecenieButtonAktywny);
        }


        #endregion

        #region Properties

        [Test]
        public void WybranoGniazdo_WsyslaMessageZGniazdem()
        {
            sut.WybraneGniazdo = new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = 1 };

            Messenger.Verify(v => v.Send(It.IsAny<tblProdukcjaGniazdoProdukcyjne>()));
        }

        [Test]
        public void WybranoOperatora_WysylaMessageZOperatoremDoPW()
        {
            sut.WybranyPracownik_1 = new tblPracownikGAT { ID_PracownikGAT = 1 };

            Messenger.Verify(x => x.Send<tblPracownikGAT,GPRuchTowarPWViewModel>(It.IsAny<tblPracownikGAT>()));
        }
        #endregion

        #region DodajZlecenieCieciaCommand
        [Test]
        public void DodajZlecenieCieciaCommandExecute_OtwieraOknoZeZleceniamiCiecia()
        {
            sut.DodajZlecenieCieciaCommand.Execute(null);

            ViewService.Verify(v => v.Show<ZlecenieCieciaEwidencjaViewModel>());
        }
        #endregion

        #region DodajZlecenieProdukcyjneCommand
        [Test]
        public void DodajZlecenieProdukcyjneCommandExecute_OtwieraEwidencjeZlecenProdukcyjnych()
        {
            sut.DodajZlecenieProdukcyjneCommand.Execute(null);

            ViewService.Verify(v => v.Show<ZlecenieProdukcyjneEwidencjaViewModel>());
        }

        [Test]
        public void DodajZlecenieProdukcyjneCommandExecute_WysylaGniazdo()
        {
            sut.DodajZlecenieProdukcyjneCommand.Execute(null);

            Messenger.Verify(v => v.Send<tblProdukcjaGniazdoProdukcyjne, ZlecenieProdukcyjneEwidencjaViewModel>(It.IsAny<tblProdukcjaGniazdoProdukcyjne>()));
        }
        #endregion

        #region SaveCommand

        #region CanExecute

        [Test]
        public void SaveAsyncCommandCanExecute_JakikolwiekChildIsNotValid_False()
        {
            vmRW.Setup(s => s.IsValid).Returns(false);
            vmRW.Setup(s => s.IsChanged).Returns(true);
            vmPW.Setup(s => s.IsValid).Returns(true);
            vmPW.Setup(s => s.IsChanged).Returns(true);

            var actual = sut.SaveCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }

        [Test]
        public void SaveAsyncCommandCanExecute_JakikolwiekChildIsValid_True_Naglowek_False_ZwrocFalse()
        {
            SaveCommandCanExecute_True();
            sut.VMEntity.IDPracownikGAT = 0;

            Assert.IsFalse(sut.SaveCommand.CanExecute(null));
        }
        [Test]
        public void SaveAsyncCommandCanExecute_JakikolwiekChildIsChanged_ChildsAreValid_NaglowekIsValid_ZwrocTrue()
        {
            SaveCommandCanExecute_True();

            Assert.IsTrue(sut.SaveCommand.CanExecute(null));
        }
        private void NaglowekIsValid()
        {
            sut.VMEntity = new tblProdukcjaRuchNaglowek
            {
                IDProdukcjaGniazdoProdukcyjne = 1,
                IDPracownikGAT = 1
            };
        }
        private void SaveCommandCanExecute_True()
        {
            MessengerSend(new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji });
            state.Setup(s => s.IsChanged).Returns(true);
            state.Setup(s => s.IsValid).Returns(true);

            //vmRW.Setup(s => s.IsValid).Returns(true);
            //vmRW.Setup(s => s.IsChanged).Returns(true);
            //vmPW.Setup(s => s.IsValid).Returns(true);
            //vmPW.Setup(s => s.IsChanged).Returns(true);

            NaglowekIsValid();
        }
        #endregion

        #region Execute

        [Test]
        public void SaveCommandExecute_GdyIDjestZeroDodajMagazynDoNaglowka()
        {
            SaveCommandCanExecute_True();

            sut.SaveCommand.Execute(null);

            Assert.AreEqual((int)MagazynyEnum.ProdukcjaGeowlokniny_PRGW, sut.VMEntity.IDMagazyn);
        }

        [Test]
        public void SavecCommandExecute_GdyIDjestZero_UoWAddJestWywolane()
        {
            SaveCommandCanExecute_True();

            sut.SaveCommand.Execute(null);

            state.Verify(v => v.SaveAsync());
        }

        [Test]
        public void SaveCommandExecute_GdyIdZero_DodajDoBazy()
        {
            SaveCommandCanExecute_True();
            sut.VMEntity.IDProdukcjaRuchNaglowek = 0;

            sut.SaveCommand.Execute(null);

            tblProdukcjaRuchNaglowek.Verify(v => v.Add(sut.VMEntity));
        }

        [Test]
        public void SaveCommandExecute_GdyIdWiekszeOdZera_NieDodawajDoBazy()
        {
            SaveCommandCanExecute_True();
            sut.VMEntity.IDProdukcjaRuchNaglowek = 1;

            sut.SaveCommand.Execute(null);

            tblProdukcjaRuchNaglowek.Verify(v => v.Add(sut.VMEntity), Times.Never);
        }


        [Test]
        public void SaveCommandExecute_PoZapisieWyslijMessage()
        {
            SaveCommandCanExecute_True();

            sut.SaveCommand.Execute(null);

            Messenger.Verify(v => v.Send(sut.VMEntity, nameof(RefreshListMessage)));
        }


        [Test]
        [Ignore("zamiast zamkniecia okna, wyswietla dialog")]
        public void SaveCommandExecute_PoZapisieZamknijOkno()
        {
            SaveCommandCanExecute_True();

            sut.SaveCommand.Execute(null);

            ViewService.Verify(v => v.Close(sut.GetType().Name));
        }


        [Test]
        public void SaveCommandExecute_PoZapisie_GdyAutoSave_NieZamykaOkna()
        {
            SaveCommandCanExecute_True();
            sut.AutoSave = true;

            sut.SaveCommand.Execute(null);

            ViewService.Verify(v => v.Close(sut.GetType().Name), Times.Never);
        }

        [Test]
        public void SaveCommandExecute_PoZapisieNieWyswietlaDialog()
        {
            SaveCommandCanExecute_True();

            sut.SaveCommand.Execute(null);

            DialogService.Verify(v => v.ShowInfo_BtnOK(It.IsAny<string>(), It.IsAny<string>()),Times.Never);
        }

        [Test]
        public void SaveCommandExecute_GdyBrakRekordowDlaDanegoZlecenia_ZmienStatusZleceniaNaRozpoczete()
        {
            SaveCommandCanExecute_True();
            MessengerSend(new tblProdukcjaZlecenieTowar
            {
                tblProdukcjaZlecenieCiecia = new tblProdukcjaZlecenieCiecia
                {
                    IDProdukcjaZlecenieCiecia = 1,
                    IDProdukcjaZlecenieStatus = (int)ProdukcjaZlecenieStatusEnum.Oczekuje,
                    tblKontrahent = new tblKontrahent { Nazwa = "test" }
                }

            });
            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()))
                                 .ReturnsAsync(new List<tblProdukcjaRuchTowar>
                                 {
                                     new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=1, tblProdukcjaRuchNaglowek = new tblProdukcjaRuchNaglowek{ IDProdukcjaZlecenieCiecia=1}},
                                     new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=2, tblProdukcjaRuchNaglowek = new tblProdukcjaRuchNaglowek{ IDProdukcjaZlecenieCiecia=1}},
                                 });
            MessengerSend(new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji });
            sut.VMEntity = new tblProdukcjaRuchNaglowek
            {
                IDProdukcjaGniazdoProdukcyjne = 1,
                IDPracownikGAT = 1
            };

            sut.SaveCommand.Execute(null);

            UnitOfWork.Verify(v => v.SaveAsync());
        }

        #endregion

        #endregion


        #region ZamknijOknoCommand
        [Test]
        public void ZamknijOknoCommand_IsChangedFalse_NieWyswietlajOkna()
        {
            sut.CloseWindowCommand.Execute(null);

            DialogService.Verify(v => v.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void ZamknijOknoCommand_IsChangedFalse_ZamknijOkno()
        {
            sut.CloseWindowCommand.Execute(null);

            ViewService.Verify(v => v.Close(sut.GetType().Name));
        }

        [Test]
        public void ZamknijOknoCommand_IsChangedTrue_WyswietlOkno()
        {
            state.Setup(s => s.IsChanged).Returns(true);

            sut.CloseWindowCommand.Execute(null);

            DialogService.Verify(v => v.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        public void ZamknijOknoCommandExecute_IsChanged_True_Dialog_True_ViewServiceIsInvoked()
        {
            state.Setup(s => s.IsChanged).Returns(true);
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            sut.CloseWindowCommand.Execute(null);

            ViewService.Verify(v => v.Close(sut.GetType().Name));
        }
        [Test]
        public void ZamknijOknoCommandExecute_IsChanged_True_Dialog_False_NicNieRob()
        {
            state.Setup(s => s.IsChanged).Returns(true);
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            sut.CloseWindowCommand.Execute(null);

            ViewService.Verify(v => v.Close(sut.GetType().Name), Times.Never);
        }
        #endregion


        #region IsValid
        private void NaglowekIsValid_True()
        {
            sut.VMEntity.IDPracownikGAT = 1;
            sut.VMEntity.IDProdukcjaGniazdoProdukcyjne = 1;
        }
        [Test]
        [TestCase(true, true)]
        [TestCase(false, false)]
        public void IsValidProp_CheckIsValidOnChildrens(bool stateIsValid, bool expected)
        {
            NaglowekIsValid_True();
            state.Setup(s => s.IsValid).Returns(stateIsValid);

            Assert.AreEqual(expected, sut.IsValid);
        }



        public void DeleteCommandExecute_PrzedUsunieciemWyswietlDialogZPytaniem()
        {
            throw new NotImplementedException();
        }

        public void DeleteCommandExecute_Dialog_True_Usun()
        {
            throw new NotImplementedException();
        }

        public void DeleteCommandExecute_PoUsunieciuWyslijMessage()
        {
            throw new NotImplementedException();
        }

        public void DeleteCommandExecute_PoUsunieciuWyswietlInformacje()
        {
            throw new NotImplementedException();
        }

        public void DeleteCommandExecute_PoUsunieciuZamknijOkno()
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
