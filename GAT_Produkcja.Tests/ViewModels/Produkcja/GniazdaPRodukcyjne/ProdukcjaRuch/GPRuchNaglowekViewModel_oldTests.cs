using AutoFixture;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.ZebraPrinter;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.Messages;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Ewidencja;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Naglowek;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW;
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

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar
{
    public class GPRuchNaglowekViewModel_oldTests : TestBase, SaveDeleteCommandTestsBaseClass
    {
        private Fixture fixture;
        private Mock<IMagazynRuchNaglowekSaveHelper> magazynRuchNaglowekSaveHelper;
        private Mock<IMagazynRuchTowarSaveHelper> magazynRuchTowaruSaveHelper;
        private Mock<IZebraLabelPrinter> zebraLabelPrinter;
        private Mock<IGPRuchTowarRWViewModel> vmRW;
        private Mock<IGPRuchTowarPWViewModel> vmPW;
        private Mock<IGPRuchTowar_Naglowek_Helper> naglowekHelper;
        private Mock<ITblPracownikGATRepository> tblPracownikGAT;
        private Mock<ITblProdukcjaRuchNaglowekRepository> tblProdukcjaRuchNaglowek;
        private Mock<ITblProdukcjaZlecenieRepository> tblProdukcjaZlcecenieProdukcyjne;
        private Mock<ITblProdukcjaZlecenieCieciaRepository> tblProdukcjaZlecenieCiecia;
        private Mock<ITblProdukcjaZlecenieTowarRepository> tblProdukcjaZlecenieCieciaTowar;
        private Mock<ITblProdukcjaGniazdoWlokninaRepository> tblProdukcjaGniazdoWloknina;
        private Mock<ITblProdukcjaGniazdoProdukcyjneRepository> tblProdukcjaGniazdoProdukcyjne;
        private Mock<ITblProdukcjaRuchTowarRepository> tblProdukcjaRuchTowar;
        private Mock<ITblProdukcjaZlecenieTowarRepository> tblProdukcjaZlecenieTowar;
        private GPRuchNaglowekViewModel_old sut;

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

            naglowekHelper.Setup(s => s.MagazynRuchNaglowekSaveHelper).Returns(magazynRuchNaglowekSaveHelper.Object);
            naglowekHelper.Setup(s => s.MagazynRuchTowarSaveHelper).Returns(magazynRuchTowaruSaveHelper.Object);


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
            sut = new GPRuchNaglowekViewModel_old(ViewModelService.Object,
                                              vmRW.Object,
                                              vmPW.Object,
                                              naglowekHelper.Object);
        }

        private tblProdukcjaGniazdoWloknina CreateGaniazdoWloknin()
        {
            return fixture.Build<tblProdukcjaGniazdoWloknina>()
                            .Without(w => w.tblPracownikGAT)
                            .Without(w => w.tblProdukcjaGniazdoProdukcyjne)
                            .Without(w => w.tblProdukcjaZlcecenieProdukcyjne)
                            .Without(w => w.tblTowarGeowlokninaParametryGramatura)
                            .Create();
        }



        #region Messengery


        [Test]
        public void RejestracjaMessengerow_tblProdukcjaRuchNaglowek()
        {
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<tblProdukcjaRuchNaglowek>>(), It.IsAny<bool>()));
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<tblProdukcjaGniazdoProdukcyjne>>(), It.IsAny<bool>()));
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<tblProdukcjaZlecenieTowar>>(), It.IsAny<bool>()));

        }

        #region GdyPrzeslanoNaglowek
        [Test]
        public void GdyPrzeslanoNaglowek_GdyPrzeslano_NaglowekPobranyZBazy()
        {
            tblProdukcjaRuchNaglowek.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<tblProdukcjaRuchNaglowek>
            {
                new tblProdukcjaRuchNaglowek{IDProdukcjaRuchNaglowek=1}
            });

            MessengerSend(new tblProdukcjaRuchNaglowek { IDProdukcjaRuchNaglowek = 1 });

            sut.LoadCommand.Execute(null);

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

            Assert.IsNotNull(sut.Naglowek);
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

            Assert.IsNotNull(sut.Naglowek);
        }
        #endregion

        #region GdyPrzeslanoGniazdoProdukcyjne
        [Test]
        public void GdyPrzeslanoGniazdoProdukcyjne_GniazdoNieNullIdNieZero_PrzypiszDoWybranegoGniazda()
        {
            tblProdukcjaGniazdoProdukcyjne.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<tblProdukcjaGniazdoProdukcyjne>
            {
                new tblProdukcjaGniazdoProdukcyjne{IDProdukcjaGniazdoProdukcyjne=(int)GniazdaProdukcyjneEnum.LiniaWloknin}
            });
            MessengerSend(new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaWloknin });

            sut.LoadCommand.Execute(null);

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
        public void GdyPrzeslanoZlecenieTowar_ZamknijOknoEwidencjiITowarzyszace()
        {
            MessengerSend(new tblProdukcjaZlecenieTowar());

            ViewService.Verify(v => v.Close<ZlecenieProdukcyjneNaglowekViewModel_old>());
            ViewService.Verify(v => v.Close<ZlecenieProdukcyjneEwidencjaViewModel_old>());
            ViewService.Verify(v => v.Close<ZlecenieDodajTowarViewModel>());
            ViewService.Verify(v => v.Close<ZlecenieCieciaNaglowekViewModel_old>());
        }

        [Test]
        public void GdyPrzeslanoZlecenieTowar_GenerujNazweTowaru()
        {
            tblProdukcjaZlecenieTowar.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new db.tblProdukcjaZlecenieTowar());

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
            MessengerSend(new tblProdukcjaZlecenieTowar());

            Assert.IsNotNull(sut.ZlecenieTowar);
        }

        #endregion


        #endregion

        #region Load
        [Test]
        public void LoadCommandExecute_MetodyLoadAsyncNaChildVMPowinnyBycUruchomione()
        {
            sut.LoadCommand.Execute(null);

            vmPW.Verify(v => v.LoadAsync(It.IsAny<int?>()));
            vmRW.Verify(v => v.LoadAsync(It.IsAny<int?>()));
        }

        [Test]
        public void LoadCommandExecute_PobieraElementyZBazy()
        {
            sut.LoadCommand.Execute(null);

            tblPracownikGAT.Verify(v => v.GetAllAsync());
            tblProdukcjaGniazdoProdukcyjne.Verify(v => v.GetAllAsync());
        }
        [Test]
        public void LoadCommandExecute_RuchTowarRWViewModelJestWywolywanyZArgIdNaglowka()
        {

            sut.Naglowek = new tblProdukcjaRuchNaglowek { IDProdukcjaRuchNaglowek = 1 };

            sut.LoadCommand.Execute(null);

            vmRW.Verify(v => v.LoadAsync(1));
        }
        [Test]
        public void LoadCommandExecute_RuchTowarPWViewModelJestWywolywanyZArgIdNaglowka()
        {

            sut.Naglowek = new tblProdukcjaRuchNaglowek { IDProdukcjaRuchNaglowek = 1 };

            sut.LoadCommand.Execute(null);

            vmPW.Verify(v => v.LoadAsync(1));
        }


        [Test]
        public void LoadCommandExecute_PoZaladowaniuZlecen_IsChanged_False()
        {
            UnitOfWork.Setup(v => v.tblProdukcjaZlecenie.PobierzAktywneZleceniaProdukcyjne()).ReturnsAsync(new List<tblProdukcjaZlecenie>
            {
                new tblProdukcjaZlecenie { IDProdukcjaZlecenie = 1 }
            });

            sut.LoadCommand.Execute(null);

            Assert.IsFalse(sut.IsChanged);
        }

        [Test]
        public void LoadCommandExecute_GdyGniazdoJestNull_RwEnabled_True()
        {
            sut.Naglowek = new tblProdukcjaRuchNaglowek { IDProdukcjaRuchNaglowek = 1 };

            sut.LoadCommand.Execute(null);

            Assert.IsTrue(sut.RwEnabled);
        }

        [Test]
        public void LoadCommandExecute_UzytkownikZalogowanyNull_NieWybieraJPRacownika()
        {
            sut.Naglowek = new tblProdukcjaRuchNaglowek { IDProdukcjaRuchNaglowek = 1 };

            sut.LoadCommand.Execute(null);

            Assert.IsNull(sut.WybranyPracownik_1);
        }

        #endregion

        #region Properties

        [Test]
        public void WybranoGniazdo_WsyslaMessageZGniazdem()
        {
            sut.WybraneGniazdo = new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = 1 };

            Messenger.Verify(v => v.Send(It.IsAny<tblProdukcjaGniazdoProdukcyjne>()));
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

            ViewService.Verify(v => v.Show<ZlecenieProdukcyjneEwidencjaViewModel_old>());
        }

        [Test]
        public void DodajZlecenieProdukcyjneCommandExecute_WysylaGniazdo()
        {
            sut.DodajZlecenieProdukcyjneCommand.Execute(null);

            Messenger.Verify(v => v.Send<tblProdukcjaGniazdoProdukcyjne, ZlecenieProdukcyjneEwidencjaViewModel_old>(It.IsAny<tblProdukcjaGniazdoProdukcyjne>()));
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
            sut.Naglowek.IDPracownikGAT = 0;

            Assert.IsFalse(sut.SaveCommand.CanExecute(null));
        }
        [Test]
        public void SaveAsyncCommandCanExecute_JakikolwiekChildIsChanged_ChildsAreValid_NaglowekIsValid_ZwrocTrue()
        {
            SaveCommandCanExecute_True();

            Assert.IsTrue(sut.SaveCommand.CanExecute(null));
        }
        #endregion

        #region Execute
        private void NaglowekIsValid()
        {
            sut.Naglowek = new tblProdukcjaRuchNaglowek
            {
                IDProdukcjaGniazdoProdukcyjne = 1,
                IDPracownikGAT = 1
            };
        }
        private void SaveCommandCanExecute_True()
        {
            MessengerSend(new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji });
            
            vmRW.Setup(s => s.IsValid).Returns(true);
            vmRW.Setup(s => s.IsChanged).Returns(true);
            vmPW.Setup(s => s.IsValid).Returns(true);
            vmPW.Setup(s => s.IsChanged).Returns(true);

            NaglowekIsValid();
        }

        [Test]
        public void SaveCommandExecute_GdyIDjestZeroDodajMagazynDoNaglowka()
        {
            SaveCommandCanExecute_True();

            sut.SaveCommand.Execute(null);

            Assert.AreEqual((int)MagazynyEnum.ProdukcjaGeowlokniny_PRGW, sut.Naglowek.IDMagazyn);
        }

        [Test]
        public void SavecCommandExecute_GdyIDjestZero_UoWAddJestWywolane()
        {
            SaveCommandCanExecute_True();

            sut.SaveCommand.Execute(null);

            vmPW.Verify(v => v.SaveAsync(It.IsAny<int?>()));
            vmRW.Verify(v => v.SaveAsync(It.IsAny<int?>()));
        }

        [Test]
        public void SaveCommandExecute_GdyIdZero_DodajDoBazy()
        {
            SaveCommandCanExecute_True();
            sut.Naglowek.IDProdukcjaRuchNaglowek = 0;

            sut.SaveCommand.Execute(null);

            tblProdukcjaRuchNaglowek.Verify(v => v.Add(sut.Naglowek));
        }

        [Test]
        public void SaveCommandExecute_GdyIdWiekszeOdZera_NieDodawajDoBazy()
        {
            SaveCommandCanExecute_True();
            sut.Naglowek.IDProdukcjaRuchNaglowek = 1;

            sut.SaveCommand.Execute(null);

            tblProdukcjaRuchNaglowek.Verify(v => v.Add(sut.Naglowek), Times.Never);
        }


        [Test]
        public void SaveCommandExecute_PoZapisieWyslijMessage()
        {
            SaveCommandCanExecute_True();

            sut.SaveCommand.Execute(null);

            Messenger.Verify(v => v.Send<tblProdukcjaRuchNaglowek, ProdukcjaRuchEwidencjaUCViewModel_old>(sut.Naglowek));
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
        public void SaveCommandExecute_PoZapisieWyswietlDialog()
        {
            SaveCommandCanExecute_True();

            sut.SaveCommand.Execute(null);

            DialogService.Verify(v => v.ShowInfo_BtnOK(It.IsAny<string>(), It.IsAny<string>()));
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
            sut.Naglowek = new tblProdukcjaRuchNaglowek
            {
                IDProdukcjaGniazdoProdukcyjne = 1,
                IDPracownikGAT = 1
            };

            sut.SaveCommand.Execute(null);

            UnitOfWork.Verify(v => v.SaveAsync());
        }

        #endregion

        #endregion

        #region UsunCommand
        #region CanExecute
        [Test]
        public void UsunCommandCanExecute_GdyIdJestZerem_False()
        {
            sut.Naglowek.IDProdukcjaRuchNaglowek = 0;

            var actual = sut.DeleteCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }
        [Test]
        public void UsunCommandCanExecute_GdyIdNieJestZerem_True()
        {
            sut.Naglowek.IDProdukcjaRuchNaglowek = 1;

            var actual = sut.DeleteCommand.CanExecute(null);

            Assert.IsTrue(actual);
        }
        #endregion

        #region Execute

        [Test]
        public void UsunCommandCanExecute_KiedyIDjestZero_False()
        {
            sut.Naglowek.IDProdukcjaRuchNaglowek = 0;

            var acutal = sut.DeleteCommand.CanExecute(null);

            Assert.IsFalse(acutal);
        }
        private void UsunCommandCanExecute_True()
        {
            sut.Naglowek.IDProdukcjaRuchNaglowek = 1;
        }

        [Test]
        public void UsunCommandCanExecute_KiedyIDNiejestZero_True()
        {
            UsunCommandCanExecute_True();

            var acutal = sut.DeleteCommand.CanExecute(null);

            Assert.IsTrue(acutal);
        }

        [Test]
        public void UsunCommandExecute_DialogServiceJestPokazane()
        {
            UsunCommandCanExecute_True();
            sut.DeleteCommand.Execute(null);

            DialogService.Verify(v => v.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        public void UsunCommandExecute_DialogServiceGdyFalse_NicSieNieDzieje()
        {
            UsunCommandCanExecute_True();
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            sut.DeleteCommand.Execute(null);

            vmPW.Verify(v => v.SaveAsync(It.IsAny<int?>()), Times.Never);
        }

        [Test]
        public void UsunCommandExecute_UoWRemoveJestWywolane()
        {
            UsunCommandCanExecute_True();
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            sut.DeleteCommand.Execute(null);

            //vmPW.Verify(v => v.DeleteAsync(It.IsAny<int>()));
            //vmRW.Verify(v => v.DeleteAsync(It.IsAny<int>()));
        }


        [Test]
        public void UsunCommandExecute_OknoJestZamkniete()
        {
            UsunCommandCanExecute_True();
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            sut.DeleteCommand.Execute(null);

            ViewService.Verify(v => v.Close(sut.GetType().Name));
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
            vmRW.Setup(s => s.IsChanged).Returns(true);
            vmPW.Setup(s => s.IsChanged).Returns(true);

            sut.CloseWindowCommand.Execute(null);

            DialogService.Verify(v => v.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        public void ZamknijOknoCommandExecute_IsChanged_True_Dialog_True_ViewServiceIsInvoked()
        {
            vmRW.Setup(s => s.IsChanged).Returns(true);
            vmPW.Setup(s => s.IsChanged).Returns(true);
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            sut.CloseWindowCommand.Execute(null);

            ViewService.Verify(v => v.Close(sut.GetType().Name));
        }
        [Test]
        public void ZamknijOknoCommandExecute_IsChanged_True_Dialog_False_NicNieRob()
        {
            vmRW.Setup(s => s.IsChanged).Returns(true);
            vmPW.Setup(s => s.IsChanged).Returns(true);
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            sut.CloseWindowCommand.Execute(null);

            ViewService.Verify(v => v.Close(sut.GetType().Name), Times.Never);
        }
        #endregion

        #region IsChanged
        [Test]
        [TestCase(false, false, false)]
        [TestCase(true, false, true)]
        [TestCase(false, true, true)]
        [TestCase(true, true, true)]
        public void IsChangedProp_WhenIsChangeFalseCheckIsChangedOnChildren(bool vmBadanieIsChanged, bool vmOgolneIsChanged, bool expected)
        {
            vmRW.Setup(s => s.IsChanged).Returns(vmBadanieIsChanged);
            vmPW.Setup(s => s.IsChanged).Returns(vmOgolneIsChanged);

            Assert.AreEqual(expected, sut.IsChanged);
        }

        [Test]
        public void IsChangedProp_WhenIsChangedTrueButChildrenIsChangedFalse_Treu()
        {
            vmRW.Setup(s => s.IsChanged).Returns(false);
            vmPW.Setup(s => s.IsChanged).Returns(false);

            sut.Naglowek.IDProdukcjaRuchNaglowek = 1;

            Assert.IsTrue(sut.IsChanged);
        }
        #endregion

        #region IsValid
        private void NaglowekIsValid_True()
        {
            sut.Naglowek.IDPracownikGAT = 1;
            sut.Naglowek.IDProdukcjaGniazdoProdukcyjne = 1;
        }
        [Test]
        [TestCase(true, true, true)]
        [TestCase(false, true, false)]
        public void IsValidProp_CheckIsValidOnChildrens(bool ogolneIsValid, bool badaniaIsValid, bool expected)
        {
            NaglowekIsValid_True();
            vmPW.Setup(s => s.IsValid).Returns(ogolneIsValid);
            vmRW.Setup(s => s.IsValid).Returns(badaniaIsValid);

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
