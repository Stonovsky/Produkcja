using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Magazyn.Ewidencje.StanTowaru;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using GAT_Produkcja.ViewModel.Magazyn.Ewidencje.RuchTowaru;
using GAT_Produkcja.ViewModel.Magazyn.RuchTowaru;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.ViewModel.Kontrahent.Dodaj;
using GAT_Produkcja.ViewModel.Magazyn.Towar.Ewidencja;
using AutoFixture;
using GAT_Produkcja.ViewModel.Magazyn.Helpers;
using GAT_Produkcja.UI.ViewModel.Kontrahent.Ewidencja;

namespace GAT_Produkcja.Tests.ViewModels.Magazyn.RuchTowaru
{
    [TestFixture]
    public class RuchTowaruViewModelTests
    {
        private Mock<IUnitOfWork> unitOfWork;
        private Mock<IUnitOfWorkFactory> unitOfWorkFactory;
        private Mock<IViewService> viewService;
        private Mock<IDialogService> dialogService;
        private Mock<IMessenger> messenger;
        private IMessenger messengerOrg;
        private Mock<ITblRuchTowarHelper> tblRuchTowarHelper;
        private Fixture fixture;
        private Mock<ITblRuchNaglowekRepository> tblRuchNaglowek;
        private Mock<ITblRuchStatusRepository> tblRuchStatus;
        private Mock<ITblRuchTowarRepository> tblRuchTowar;
        private Mock<ITblJmRepository> tblJm;
        private Mock<ITblPracownikGATRepository> tblPracownikGAT;
        private Mock<ITblFirmaRepository> tblFirma;
        private Mock<ITblTowarRepository> tblTowar;
        private Mock<ITblMagazynRepository> tblMagazyn;
        private Mock<ITblDokumentTypRepository> tblDokumentTyp;
        private Mock<ITblProdukcjaZlecenieRepository> tblProdukcjaZlcecenieProdukcyjne;
        private RuchTowaruViewModel sut;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            viewService = new Mock<IViewService>();
            dialogService = new Mock<IDialogService>();
            messenger = new Mock<IMessenger>();
            messengerOrg = new Messenger();
            tblRuchTowarHelper = new Mock<ITblRuchTowarHelper>();
            
            fixture = new Fixture();
            
            tblRuchNaglowek = new Mock<ITblRuchNaglowekRepository>();
            tblRuchStatus = new Mock<ITblRuchStatusRepository>();
            tblRuchTowar = new Mock<ITblRuchTowarRepository>();
            tblJm = new Mock<ITblJmRepository>();
            tblPracownikGAT = new Mock<ITblPracownikGATRepository>();
            tblFirma = new Mock<ITblFirmaRepository>();
            tblTowar = new Mock<ITblTowarRepository>();
            tblMagazyn = new Mock<ITblMagazynRepository>();
            tblDokumentTyp = new Mock<ITblDokumentTypRepository>();
            tblProdukcjaZlcecenieProdukcyjne = new Mock<ITblProdukcjaZlecenieRepository>();


            unitOfWork.Setup(s => s.tblRuchNaglowek).Returns(tblRuchNaglowek.Object);
            unitOfWork.Setup(s => s.tblRuchStatus).Returns(tblRuchStatus.Object);
            unitOfWork.Setup(s => s.tblRuchTowar).Returns(tblRuchTowar.Object);
            unitOfWork.Setup(s => s.tblJm).Returns(tblJm.Object);
            unitOfWork.Setup(s => s.tblPracownikGAT).Returns(tblPracownikGAT.Object);
            unitOfWork.Setup(s => s.tblFirma).Returns(tblFirma.Object);
            unitOfWork.Setup(s => s.tblTowar).Returns(tblTowar.Object);
            unitOfWork.Setup(s => s.tblMagazyn).Returns(tblMagazyn.Object);
            unitOfWork.Setup(s => s.tblDokumentTyp).Returns(tblDokumentTyp.Object);
            unitOfWork.Setup(s => s.tblProdukcjaZlecenie).Returns(tblProdukcjaZlcecenieProdukcyjne.Object);
            unitOfWorkFactory.Setup(s => s.Create()).Returns(unitOfWork.Object);
            sut= GenerateSUT(messenger.Object);
        }
        private RuchTowaruViewModel GenerateSUT(IMessenger messenger)
        {
            return new RuchTowaruViewModel(unitOfWork.Object, viewService.Object, dialogService.Object, unitOfWorkFactory.Object,messenger, tblRuchTowarHelper.Object);
        }

        #region Initialize
        [Test]
        public void OnStart_ListaRuchuTowarow_NotNull()
        {
            Assert.IsNotNull(sut.ListaRuchuTowarow);
        }
        [Test]
        public void OnStart_RuchNaglowek_NotNull()
        {
            Assert.IsNotNull(sut.RuchNaglowek);
        }
        [Test]
        public void OnStart_WybranyTowarRuch_NotNull()
        {
            Assert.IsNotNull(sut.WybranyTowarRuch);
        }
        [Test]
        public void OnStart_MessengerForVwStanTowaru_IsRegistered()
        {
            messenger.Verify(v => v.Register<vwStanTowaru>(sut, It.IsAny<Action<vwStanTowaru>>(), It.IsAny<bool>()));
        }

        [Test]
        public void OnStart_MessengerForTblTowaru_IsRegistered()
        {
            messenger.Verify(v => v.Register<tblTowar>(sut, It.IsAny<Action<tblTowar>>(), It.IsAny<bool>()));
        }

        [Test]
        public void GdyPrzeslanoTowarTblTowar_IfObjIsNull_Returns()
        {
            sut = GenerateSUT(messengerOrg);
            tblTowar towar = null;

            messengerOrg.Send(towar);

            Assert.IsTrue(sut.WybranyTowarRuch.IDTowar == null);
        }
        [Test]
        public void OnStart_MessengerStatusRuchuTowaru_IsRegistered()
        {
            messenger.Verify(v => v.Register<StatusRuchuTowarowEnum>(sut, It.IsAny<Action<StatusRuchuTowarowEnum>>(), It.IsAny<bool>()));
        }

        [Test]
        public void ZaladujWartosciPoczatkoweCommandExecute_AllNeededDataGetFromDB()
        {
            sut.ZaladujWartosciPoczatkoweCommand.Execute(null);

            tblRuchStatus.Verify(tblRuchStatus => tblRuchStatus.GetAllAsync());
            tblJm.Verify(tblJm => tblJm.GetAllAsync());
            tblPracownikGAT.Verify(tblPracownikGAT => tblPracownikGAT.PobierzPracownikowPracujacychAsync());
            tblFirma.Verify(tblFirma => tblFirma.GetAllAsync());
            //tblTowar.Verify(tblTowar => tblTowar.GetAllAsync());
            tblMagazyn.Verify(tblMagazyn => tblMagazyn.GetAllAsync());
            tblProdukcjaZlcecenieProdukcyjne.Verify(tblPZP => tblPZP.GetAllAsync());
        }
        #endregion

        #region SzukajTowarCommand
        [Test]
        public void SzukajTowarCommandExecute_WhenWybranyStatusIsNotPZ_ViewServiceShowShouldBeInvokedWithAppropriateArgument()
        {
            sut.WybranyStatusRuchu = new tblRuchStatus { IDRuchStatus = (int)StatusRuchuTowarowEnum.PrzesuniecieMiedzymagazynowe_MM };

            sut.SzukajTowarCommand.Execute(null);

            viewService.Verify(v => v.Show<MagazynStanTowaruViewModel>());
        }
        [Test]
        public void GdyPrzeslanoTowar_TowarIsNull_DialogWindowIsNotClosed()
        {
            sut = GenerateSUT(messengerOrg);
            vwStanTowaru stan = null;
            messengerOrg.Send(stan);

            viewService.Verify(v => v.Close<MagazynStanTowaruViewModel>(), Times.Never);
        }

        [Test]
        public void GdyPrzeslanoTowar_WybranyStatusRuchuHasTheSameParameters()
        {
            sut = GenerateSUT(messengerOrg);

            messengerOrg.Send(new vwStanTowaru { IDTowar = 1, IDMagazyn = 1, IDFirma = 1, IloscDostepna = 1, IDJm = 1 });

            Assert.IsTrue(sut.WybranyTowarRuch.IDTowar == 1);
            Assert.IsTrue(sut.WybranyTowarRuch.IDMagazyn == 1);
            Assert.IsTrue(sut.WybranyTowarRuch.IloscPrzed == 1);
            Assert.IsTrue(sut.WybranyTowarRuch.IDJm == 1);
        }
        [Test]
        public void GdyPrzeslanoTowar_DialogWindowMagazynStanTowaruViewModel_ShouldBeClosed()
        {
            sut = GenerateSUT(messengerOrg);

            messengerOrg.Send(new vwStanTowaru { IDTowar = 1, IDMagazyn = 1, IDFirma = 1, IloscDostepna = 1, IDJm = 1 });

            viewService.Verify(v => v.Close<MagazynStanTowaruViewModel>());
        }
        [Test]
        public void GdyPrzeslanoTowar_TowarShouldBeAddedToWubranyTowarRuch()
        {
            sut = GenerateSUT(messengerOrg);

            messengerOrg.Send(new vwStanTowaru { IDTowar = 1, IDMagazyn = 1, IDFirma = 1, IloscDostepna = 1, IDJm = 1 });

            Assert.IsTrue(sut.WybranyTowarRuch.IDTowar == 1);
            Assert.IsTrue(sut.WybranyTowarRuch.IloscPrzed == 1);
            Assert.IsTrue(sut.WybranyTowarRuch.IDJm == 1);
        }

        [Test]
        public void SzukajTowarCommandExecute_WhenWybranyStatusIsPZ_ViewServiceWillShowDialogTowarEwidencjaViewModel()
        {
            sut.WybranyStatusRuchu = new tblRuchStatus { IDRuchStatus = (int)StatusRuchuTowarowEnum.PrzyjecieZewnetrne_PZ };

            sut.SzukajTowarCommand.Execute(null);

            viewService.Verify(v => v.Show<TowarEwidencjaViewModel>());
        }
        [Test]
        public void GdyPrzeslanoTowar_ViewServiceShouldCloseTowarEwidencjaViewModel()
        {
            sut = GenerateSUT(messengerOrg);

            messengerOrg.Send(new tblTowar { IDTowar = 1 });

            viewService.Verify(v => v.Close<TowarEwidencjaViewModel>());
        }
        #endregion

        #region PoEdycjiKomorkiDataGridCommand
        [Test]
        [TestCase(StatusRuchuTowarowEnum.PrzyjecieZewnetrne_PZ,0,5,5)]
        [TestCase(StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW,0,5,5)]
        [TestCase(StatusRuchuTowarowEnum.RozchodWewnetrzny_RW,10,5,5)]
        [TestCase(StatusRuchuTowarowEnum.WydanieZewnetrzne_WZ,10,5,5)]
        public void PoEdycjiKomorkiDataGridCommandExecute_WhenQuantityChanges(StatusRuchuTowarowEnum status, decimal iloscPrzed, decimal ilosc, decimal expected)
        {
            sut.WybranyStatusRuchu.IDRuchStatus = (int)status;
            sut.WybranyTowarRuch.Ilosc = ilosc;
            tblRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblRuchTowar, bool>>>())).ReturnsAsync(new List<tblRuchTowar>
            {
                new tblRuchTowar{Ilosc=iloscPrzed}
            });

            sut.PoEdycjiKomorkiDataGridCommand.Execute(null);

            Assert.IsTrue(sut.WybranyTowarRuch.IloscPo == expected);
        }
        #endregion

        #region PobierzDaneZKoduKreskowegoCommand
        [Test]
        public void PobierzDaneZKoduKreskowegoCommandCanExecute_IfKodIsNullOrEmpty_ReturnsFalse()
        {
            sut.WybranyTowarRuch.NrParti = null;

            var result = sut.PobierzDaneZKoduKreskowegoCommand.CanExecute(null);

            Assert.IsFalse(result);
        }
        [Test]
        public void PobierzDaneZKoduKreskowegoCommandCanExecute_WybranyTowarRuchIsNull_ReturnFalse()
        {
            sut.WybranyTowarRuch = null;

            var result = sut.PobierzDaneZKoduKreskowegoCommand.CanExecute(null);

            Assert.IsFalse(result);
        }
        [Test]
        public void PobierzDaneZKoduKreskowegoCommandExecute_WhenCalled_UOWInvoked()
        {
            sut.WybranyTowarRuch.NrParti = "123123123";

            sut.PobierzDaneZKoduKreskowegoCommand.Execute(null);

            tblRuchTowar.Verify(v => v.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblRuchTowar, bool>>>()));
        }

        [Test]
        public void PobierzDaneZKoduKreskowegoCommandExecute_WhenWybranyTowarRuchIsNull_UOWShouldNotBeInvoked()
        {
            sut.WybranyTowarRuch.NrParti = "123123123";

            tblRuchTowar towarNull = null;
            tblRuchTowar.Setup(s => s.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblRuchTowar, bool>>>())).ReturnsAsync(towarNull);

            sut.PobierzDaneZKoduKreskowegoCommand.Execute(null);

            tblTowar.Verify(v => v.GetByIdAsync(It.IsAny<int>()), Times.Never);

        }

        #endregion

        #region ZapiszCommand

        #region CanExecute
        [Test]
        public void ZapiszCommandCanExecute_RuchNaglowekIsNull_ReturnsFalse()
        {
            sut.RuchNaglowek = null;

            var result = sut.ZapiszCommand.CanExecute(null);

            Assert.IsFalse(result);
        }
        [Test]
        public void ZapiszCommandCanExecute_RuchNaglowekIsNotValid_ReturnsFalse()
        {
            sut.RuchNaglowek.IsValid = false;

            var result = sut.ZapiszCommand.CanExecute(null);

            Assert.IsFalse(result);
        }

        [Test]
        public void ZapiszCommandCanExecute_IfAnyRuchTowarIsNotValid_ReturnsFalse()
        {
            sut.RuchNaglowek.IsValid = true;
            sut.ListaRuchuTowarow = new ObservableCollection<tblRuchTowar>
            {
                new tblRuchTowar{IDRuchTowar=1, IsValid=true},
                new tblRuchTowar{IDRuchTowar=1, IsValid=false}
            };

            var result = sut.ZapiszCommand.CanExecute(null);

            Assert.IsFalse(result);
        }

        [Test]
        public void ZapiszCommandCanExecute_IfRuchTowarListIsEmpty_ReturnsFalse()
        {
            sut.RuchNaglowek.IsValid = true;
            sut.ListaRuchuTowarow = null;

            var result = sut.ZapiszCommand.CanExecute(null);

            Assert.IsFalse(result);
        }

        private void ZapiszCommandCanExecute_True()
        {
            //fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
            //fixture.Behaviors.Add(new OmitOnRecursionBehavior())
            sut.RuchNaglowek = new tblRuchNaglowek
            {
                NrDokumentu = 1
            };
            //sut.RuchNaglowek.IsValid = true;
            sut.ListaRuchuTowarow = new ObservableCollection<tblRuchTowar>
            {
                //new tblRuchTowar{IDRuchTowar=1, IsValid=true}
                //new tblRuchTowar{IDRuchTowar=1, IsValid=true}
            };
            sut.ListaRuchuTowarow.Add(CreateRuchTowar());

        }
        private tblRuchTowar CreateRuchTowar()
        {
            return fixture.Build<tblRuchTowar>()
                                             .Without(r => r.tblDokumentTyp)
                                             .Without(r => r.tblJm)
                                             .Without(r => r.tblMagazyn)
                                             .Without(r => r.tblMieszankaSklad)
                                             .Without(r => r.tblRuchNaglowek)
                                             .Without(r => r.tblTowar)
                                             .Without(r => r.tblVAT)
                                             .Create();
        }
        [Test]
        public void ZapiszCommandCanExecute_IDRuchNaglowekIsZero_UoWtblRuchNaglowekADDIsInvoked()
        {
            ZapiszCommandCanExecute_True();
            sut.RuchNaglowek.IDRuchNaglowek = 0;

            sut.ZapiszCommand.Execute(null);

            tblRuchNaglowek.Verify(v => v.Add(It.IsAny<tblRuchNaglowek>()));
            unitOfWork.Verify(v => v.SaveAsync());
        }


        [Test]
        public void ZapiszCommandCanExecute_IDRuchNaglowekIsNotZero_UoWtblRuchNaglowekADDIsNotInvoked()
        {
            ZapiszCommandCanExecute_True();
            sut.RuchNaglowek.IDRuchNaglowek = 1;

            sut.ZapiszCommand.Execute(null);

            tblRuchNaglowek.Verify(v => v.Add(It.IsAny<tblRuchNaglowek>()), Times.Never);
            unitOfWork.Verify(v => v.SaveAsync());
        }
        [Test]
        public void ZapiszCommandExecute_IDRuchTowarIsZero_UoWtblRuchTowarIsInvoked()
        {
            ZapiszCommandCanExecute_True();
            var rt1 = CreateRuchTowar(); rt1.IDRuchTowar = 0;
            var rt2 = CreateRuchTowar();

            sut.ListaRuchuTowarow = new ObservableCollection<tblRuchTowar>();
            sut.ListaRuchuTowarow.Add(rt1);
            sut.ListaRuchuTowarow.Add(rt2);
            
            sut.WybranyStatusRuchu.IDRuchStatus = (int)StatusRuchuTowarowEnum.PrzyjecieZewnetrne_PZ;

            sut.ZapiszCommand.Execute(null);

            tblRuchTowarHelper.Verify(v => v.DodajDoBazyDanych(It.IsAny<tblRuchTowar>(), It.IsAny<tblRuchStatus>(), It.IsAny<tblRuchNaglowek>()));
            //tblRuchTowar.Verify(v => v.Add(It.IsAny<tblRuchTowar>()));
            unitOfWork.Verify(v => v.SaveAsync());
        }

        [Test]
        public void ZapiszCommandExecute_WhenStatusIsRWAndIDProdukcjaZlecenieProdukcyjneIsZero_ReturnsFalse()
        {
            ZapiszCommandCanExecute_True();
            sut.WybranyStatusRuchu.IDRuchStatus = (int)StatusRuchuTowarowEnum.RozchodWewnetrzny_RW;

            var result = sut.ZapiszCommand.CanExecute(null);

            Assert.IsFalse(result);
        }

        #endregion

        #region Execute
        [Test]
        public void ZapiszCommandExecute_IDRuchNaglowekIsPassedToListaRuchuTowarow()
        {
            ZapiszCommandCanExecute_True();
            sut.RuchNaglowek.IDRuchNaglowek = 0;
            tblRuchNaglowek.Setup(s => s.Add(It.IsAny<tblRuchNaglowek>())).Callback(() => sut.RuchNaglowek.IDRuchNaglowek = 1);

            sut.ZapiszCommand.Execute(null);

            Assert.IsTrue(sut.ListaRuchuTowarow.Where(t => t.IDRuchNaglowek == 1).Count() == 1);
        }

        [Test]
        public void ZapiszCommandExecute_IDRuchTowarIsNotZero_UoWtblRuchTowarIsInvoked()
        {
            ZapiszCommandCanExecute_True();

            sut.ZapiszCommand.Execute(null);

            tblRuchTowar.Verify(v => v.Add(It.IsAny<tblRuchTowar>()), Times.Never);
            unitOfWork.Verify(v => v.SaveAsync());
        }

        [Test]
        public void ZapiszCommandExecute_WhenSaved_DialogServiceInfoShouldBeInvoked()
        {
            ZapiszCommandCanExecute_True();

            sut.ZapiszCommand.Execute(null);

            dialogService.Verify(v => v.ShowInfo_BtnOK(It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        public void ZapiszCommandExecute_WhenSaved_MessageShouldBeSent()
        {
            ZapiszCommandCanExecute_True();
            Messenger.OverrideDefault(messenger.Object);

            sut.ZapiszCommand.Execute(null);

            messenger.Verify(v => v.Send<string, MagazynRuchTowaruViewModel>(It.IsAny<string>()));
            Messenger.OverrideDefault(Messenger.Default);
        }
        [Test]
        public void ZapiszCommandExecute_WhenSaved_WindowShouldBeClosed()
        {
            ZapiszCommandCanExecute_True();

            sut.ZapiszCommand.Execute(null);

            viewService.Verify(v => v.Close<RuchTowaruViewModel>());
        }

        [Test]
        public void ZapiszCommandExecute_WhenStatusRuchuEnumIsMM_AddTwoRecordsToTblRuchTowarTable()
        {
            ZapiszCommandCanExecute_True();

            sut.WybranyStatusRuchu.IDRuchStatus = (int)StatusRuchuTowarowEnum.PrzesuniecieMiedzymagazynowe_MM;
            sut.RuchNaglowek = new tblRuchNaglowek { IDRuchNaglowek = 1, IDMagazynZ = 1, IDMagazynDo = 2};

            //dodanie do listy towaru o ID=0 zeby mozna bylo przetestowac uow
            var rt = CreateRuchTowar();     rt.IDRuchTowar = 0;
            sut.ListaRuchuTowarow.Add(rt);
            //sprawdzenie przeliczenia ilosci minimalnych, mockujemy tabele => dodajemy liste elementow
            tblRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblRuchTowar, bool>>>())).ReturnsAsync(new List<tblRuchTowar>
            {
                new tblRuchTowar{IDRuchTowar=1, Ilosc=10},
                new tblRuchTowar{IDRuchTowar=2, Ilosc=20}
            });

            sut.ZapiszCommand.Execute(null);

            tblRuchTowarHelper.Verify(v => v.DodajDoBazyDanych(It.IsAny<tblRuchTowar>(), It.IsAny<tblRuchStatus>(), It.IsAny<tblRuchNaglowek>()));
        }


        #endregion
        #endregion

        #region UsunTowarRuchCommand
        [Test]
        public void UsunTowarRuchCommandCanExecute_WhenWybranyTowarRuchIsNill_ReturnsFalse()
        {
            sut.WybranyTowarRuch = null;

            var result = sut.UsunTowarRuchCommand.CanExecute(null);

            Assert.IsFalse(result);
        }
        [Test]
        public void UsunTowarRuchCommandCanExecute_WybranytowarRuchIdIsZero_ReturnsTrue()
        {
            var result = sut.UsunTowarRuchCommand.CanExecute(null);

            Assert.IsTrue(result);
        }

        [Test]
        public void UsunTowarRuchCommandExecute_IdIsZero_RemoveFromListNotFromDB()
        {
            sut.ListaRuchuTowarow = new ObservableCollection<tblRuchTowar>
            {
                new tblRuchTowar{IDRuchTowar=0},
                new tblRuchTowar{IDRuchTowar=1}
            };
            sut.WybranyTowarRuch = sut.ListaRuchuTowarow.SingleOrDefault(r => r.IDRuchTowar == 0);

            sut.UsunTowarRuchCommand.Execute(null);

            tblRuchTowar.Verify(v => v.Remove(It.IsAny<tblRuchTowar>()), Times.Never);
            unitOfWork.Verify(v => v.SaveAsync(), Times.Never);
            Assert.IsTrue(sut.ListaRuchuTowarow.Count() == 1);
        }
        [Test]
        public void UsunTowarRuchCommandExecute_IdIsNotZero_RemoveFromListAndDB()
        {
            sut.ListaRuchuTowarow = new ObservableCollection<tblRuchTowar>
            {
                new tblRuchTowar{IDRuchTowar=0},
                new tblRuchTowar{IDRuchTowar=1}
            };
            sut.WybranyTowarRuch = sut.ListaRuchuTowarow.SingleOrDefault(r => r.IDRuchTowar == 1);

            sut.UsunTowarRuchCommand.Execute(null);

            tblRuchTowar.Verify(v => v.Remove(It.IsAny<tblRuchTowar>()));
            unitOfWork.Verify(v => v.SaveAsync());
            Assert.IsTrue(sut.ListaRuchuTowarow.Count() == 1);
        }
        [Test]
        public void UsunTowarRuchCommandExecute_IfListContainsOneElementWhichIsDeleted_BrandNewElementShouldBeAddedToList()
        {
            //If we do not add elemen to empty list we wont be able to add any new row do datagrid
            sut.ListaRuchuTowarow = new ObservableCollection<tblRuchTowar>
            {
                new tblRuchTowar{IDRuchTowar=1}
            };
            sut.WybranyTowarRuch = sut.ListaRuchuTowarow.SingleOrDefault(r => r.IDRuchTowar == 1);

            sut.UsunTowarRuchCommand.Execute(null);

            Assert.IsTrue(sut.ListaRuchuTowarow.Count() == 1);
        }

        #endregion

        #region UsunCommand
        [Test]
        public void UsunCommandCanExecute_IDRuchNaglowekIsZero_ReturnsFalse()
        {
            sut.RuchNaglowek.IDRuchNaglowek = 0;

            var result = sut.UsunCommand.CanExecute(null);

            Assert.IsFalse(result);
        }
        [Test]
        public void UsunCommandCanExecute_IdRuchNaglowekIsNotZero_ReturnsTrue()
        {
            sut.RuchNaglowek.IDRuchNaglowek = 1;

            var result = sut.UsunCommand.CanExecute(null);

            Assert.IsTrue(result);
        }
        private void UsunCommandExecute_Setup()
        {
            sut.RuchNaglowek.IDRuchNaglowek = 1;
            dialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        }
        [Test]
        public void UsunCommandExecute_ListaRuchuTowarowIsEmpty_WontDeleteListFromDB()
        {
            UsunCommandExecute_Setup();

            sut.UsunCommand.Execute(null);

            tblRuchTowar.Verify(v => v.Remove(It.IsAny<tblRuchTowar>()), Times.Never);
        }
        [Test]
        public void UsunCommandExecute_TowarIdIsZeroInListaRuchTowarow_WontDeleteFromDB()
        {
            UsunCommandExecute_Setup();
            sut.ListaRuchuTowarow = new ObservableCollection<tblRuchTowar>
            {
                new tblRuchTowar{IDRuchTowar=0},
                new tblRuchTowar{IDRuchTowar=1}
            };

            sut.UsunCommand.Execute(null);

            tblRuchTowar.Verify(v => v.Remove(It.IsAny<tblRuchTowar>()), Times.Once);
            unitOfWork.Verify(v => v.SaveAsync(), Times.Once);
        }
        [Test]
        public void UsunCommandExecute_DialogServiceIsCalled()
        {
            UsunCommandExecute_Setup();

            sut.UsunCommand.Execute(null);

            dialogService.Verify(v => v.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>()));
        }
        [Test]
        public void UsunCommandExecute_WhenDeleted_DialogServiceInfoIsCalled()
        {
            UsunCommandExecute_Setup();

            sut.UsunCommand.Execute(null);

            dialogService.Verify(v => v.ShowInfo_BtnOK(It.IsAny<string>(), It.IsAny<string>()));
        }
        [Test]
        public void UsunCommandExecute_WhenNotDeleted_DialogServiceInfoIsNotCalled()
        {
            UsunCommandExecute_Setup();
            dialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            sut.UsunCommand.Execute(null);

            dialogService.Verify(v => v.ShowInfo_BtnOK(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
        [Test]
        public void UsunCommandExecute_DialogServiceIsFale_UWOisNotInvokedNothingHappens()
        {
            UsunCommandExecute_Setup();
            dialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            sut.UsunCommand.Execute(null);

            unitOfWork.Verify(v => v.SaveAsync(), Times.Never);
        }

        [Test]
        public void UsunCommandExecute_RuchNaglowekIdIsZero_NothingHappensAndCloseWindow()
        {
            // Wariant zalatwiony przez CanExecute
        }
        [Test]
        public void UsunCommandExecute_RuchNaglowekIdIsNotZero_DeleteRuchNaglowek()
        {
            UsunCommandExecute_Setup();
            dialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            sut.UsunCommand.Execute(null);

        }

        [Test]
        public void UsunCommandExecute_WhenDeleted_MessageShouldBeSent()
        {
            UsunCommandExecute_Setup();
            Messenger.OverrideDefault(messenger.Object);
            dialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            sut.UsunCommand.Execute(null);

            messenger.Verify(v => v.Send<string, MagazynRuchTowaruViewModel>(It.IsAny<string>()));
            Messenger.OverrideDefault(Messenger.Default);
        }

        [Test]
        public void UsunCommandExecute_WhenNotDeleted_MessageShouldNotBeSent()
        {
            UsunCommandExecute_Setup();
            Messenger.OverrideDefault(messenger.Object);
            dialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            sut.UsunCommand.Execute(null);

            messenger.Verify(v => v.Send<string, MagazynRuchTowaruViewModel>(It.IsAny<string>()), Times.Never);
            Messenger.OverrideDefault(Messenger.Default);
        }

        [Test]
        public void UsunCommandExecute_WhenDeleted_WindowShouldBeClosed()
        {
            UsunCommandExecute_Setup();
            dialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            sut.UsunCommand.Execute(null);

            viewService.Verify(v => v.Close<RuchTowaruViewModel>());
        }

        [Test]
        public void UsunCommandExecute_WhenNotDeleted_WindowShouldNotBeClosed()
        {
            UsunCommandExecute_Setup();
            dialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            sut.UsunCommand.Execute(null);

            viewService.Verify(v => v.Close<RuchTowaruViewModel>(), Times.Never);
        }

        #endregion

        #region GenerujKodKreskowyCommand
        [Test]
        public void GenerujKodKreskowyCommandCanExecute_WybranyTowarRuchIsNull_ReturnsFalse()
        {
            sut.WybranyTowarRuch = null;

            var result = sut.GenerujKodKreskowyCommand.CanExecute(null);

            Assert.IsFalse(result);
        }

        [Test]
        public void GenerujKodKreskowyCommandExecute_WhenCalled_NrPartiiIsNotNullOrEmpty()
        {
            sut.GenerujKodKreskowyCommand.Execute(null);

            Assert.IsTrue(!String.IsNullOrEmpty(sut.WybranyTowarRuch.NrParti));
        }
        #endregion

        #region PokazEwidencjeKontrahentowCommand
        [Test]
        public void PokazEwidencjeKontrahentowCommandExecute_WhenCalled_ViewServiceEwidencjaKontrahentowViewModelShouldBeInvoked()
        {

            sut.PokazEwidencjeKontrahentowCommand.Execute(null);

            viewService.Verify(v => v.ShowDialog<EwidencjaKontrahentowViewModel_old>());
        }
        #endregion

        #region GdyPrzeslanoKontrahenta
        [Test]
        public void GdyPrzeslanoKontrahenta_WhenCalled_ViewServiceShouldCloseEwidencjeKontrahentow()
        {
            sut = GenerateSUT(messengerOrg);

            messengerOrg.Send(new tblKontrahent { ID_Kontrahent = 1 });

            viewService.Verify(v => v.Close<EwidencjaKontrahentowViewModel_old>());
        }
        [Test]
        public void GdyPrzeslanoKontrahenta_WhenCalled_KontrahentPelnaNazwaIsNotNullOrEmpty()
        {
            sut = GenerateSUT(messengerOrg);
            messengerOrg.Send(new tblKontrahent { ID_Kontrahent = 1, Nazwa = "KontrahentTest", Ulica = "Testowa 2", KodPocztowy = "11-111", Miasto = "Testowo" });

            Assert.IsNotNull(sut.KontrahentPelnaNazwa);
            Assert.IsNotEmpty(sut.KontrahentPelnaNazwa.Replace(",", "").Replace(" ", ""));
        }
        [Test]
        public void GdyPrzeslanoKontrahenta_WhenCalled_RuchNaglowekIDKontrahenIsNotZeroAndNotNull()
        {
            sut = GenerateSUT(messengerOrg);

            messengerOrg.Send(new tblKontrahent { ID_Kontrahent = 1, Nazwa = "KontrahentTest", Ulica = "Testowa 2", KodPocztowy = "11-111", Miasto = "Testowo" });

            Assert.IsTrue(sut.RuchNaglowek.IDKontrahent != 0);
            Assert.IsNotNull(sut.RuchNaglowek.IDKontrahent);
        }

        [Test]
        public void GdyPrzeslanoKontrahenta_WhenCalled_DodajKontrahentaViewShouldBeClosed()
        {
            sut = GenerateSUT(messengerOrg);

            messengerOrg.Send(new tblKontrahent { ID_Kontrahent = 1, Nazwa = "KontrahentTest", Ulica = "Testowa 2", KodPocztowy = "11-111", Miasto = "Testowo" });

            viewService.Verify(v => v.Close<DodajKontrahentaViewModel_old>());
        }
        #endregion

        #region GdyPrzeslanoStatusRuchuEnum
        [Test]
        [TestCase(StatusRuchuTowarowEnum.PrzyjecieZewnetrne_PZ, true)]
        [TestCase(StatusRuchuTowarowEnum.RozchodWewnetrzny_RW, false)]
        [TestCase(StatusRuchuTowarowEnum.PrzesuniecieMiedzymagazynowe_MM, false)]
        [TestCase(StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW, false)]
        [TestCase(StatusRuchuTowarowEnum.WydanieZewnetrzne_WZ, false)]
        public void GdyPrzeslanoStatusRuchuEnum_WhenCalled_PrzyjecieZewnetrne_PZ(StatusRuchuTowarowEnum status, bool expected)
        {
            sut = GenerateSUT(messengerOrg);
            messengerOrg.Send(status);

            Assert.AreEqual(expected, sut.CzyStatusPZ == true);
        }

        [Test]
        [TestCase(StatusRuchuTowarowEnum.PrzyjecieZewnetrne_PZ, false)]
        [TestCase(StatusRuchuTowarowEnum.RozchodWewnetrzny_RW, false)]
        [TestCase(StatusRuchuTowarowEnum.PrzesuniecieMiedzymagazynowe_MM, true)]
        [TestCase(StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW, true)]
        [TestCase(StatusRuchuTowarowEnum.WydanieZewnetrzne_WZ, false)]
        public void GdyPrzeslanoStatusRuchuEnum_WhenCalled_PrzesuniecieMiedzymagazynowe_MM(StatusRuchuTowarowEnum status, bool expected)
        {
            sut = GenerateSUT(messengerOrg);
            messengerOrg.Send(status);

            Assert.AreEqual(expected, sut.CzyStatusMM_PW == true);
        }

        #endregion

        #region GenerowanieNrDokumentuWewnetrznego
        [Ignore("Metoda do przetestowania na TblRuchNaglowekRepository")]
        [Test]
        public void NrDokumentuGenerator_WhenZeroRecordsInDb_ShouldReturnsOne()
        {
            tblRuchNaglowek.Setup(s => s.PobierzNrDokumentuWewnetrznegoAsync(It.IsAny<int>())).ReturnsAsync(0);
            sut.ListaStatusowRuchu = new List<tblRuchStatus>
            {
                new tblRuchStatus{ IDRuchStatus=1, Symbol="PZ"},
                new tblRuchStatus{ IDRuchStatus=2, Symbol="WZ"}
            };

            Messenger.Default.Send(StatusRuchuTowarowEnum.PrzyjecieZewnetrne_PZ);

            Assert.IsTrue(sut.RuchNaglowek.NrDokumentuPelny == "PZ 1/2019");
        }

        [Ignore("Metoda do przetestowania na TblRuchNaglowekRepository")]
        [Test]
        public void NrDokumentuGenerator_WhenWybranyStatusRuchuIsNull_ShouldReturnsNull()
        {
            tblRuchNaglowek.Setup(s => s.PobierzNrDokumentuWewnetrznegoAsync(It.IsAny<int>())).ReturnsAsync(1);
            sut.ListaStatusowRuchu = new List<tblRuchStatus>
            {
                new tblRuchStatus{ IDRuchStatus=1, Symbol="PZ"},
                new tblRuchStatus{ IDRuchStatus=2, Symbol="WZ"}
            };
            Assert.IsNull(sut.RuchNaglowek.NrDokumentuPelny);
        }

        [Ignore("Metoda do przetestowania na TblRuchNaglowekRepository")]
        [Test]
        public void PobierzNowyNrDokumentu_WhenListIsEmpty_ReturnsOne()
        {
            //tblRuchNaglowek.Setup(s => s.PobierzNrDokumentuWewnetrznegoAsync(It.IsAny<int>())).ReturnsAsync(0);
            //tblRuchNaglowek.Setup(s => s.FindAsync(It.IsAny<Expression<Func<tblRuchNaglowek, bool>>>())).ReturnsAsync(new List<tblRuchNaglowek>
            //{
            //});

            tblRuchNaglowek.Setup(s=>s.PobierzNrDokumentuWewnetrznegoAsync((int)StatusRuchuTowarowEnum.PrzyjecieZewnetrne_PZ))
                           .ReturnsAsync(0);

            sut.ListaStatusowRuchu = new List<tblRuchStatus>
            {
                new tblRuchStatus{ IDRuchStatus=1, Symbol="PZ"},
                new tblRuchStatus{ IDRuchStatus=2, Symbol="WZ"}
            };

            Messenger.Default.Send(StatusRuchuTowarowEnum.PrzyjecieZewnetrne_PZ);

            Assert.IsTrue(sut.RuchNaglowek.NrDokumentu == 1);
        }

        [Ignore("Metoda do przetestowania na TblRuchNaglowekRepository")]

        [Test]
        [TestCase(1,2)]
        [TestCase(10,11)]
        public void PobierzNowyNrDokumentu_WhenListIsNotEmpty_ReturnsLastNrDokumentuPlusOne(int lastNo, int newNo)
        {
            //tblRuchNaglowek.Setup(s => s.PobierzNrDokumentuWewnetrznegoAsync(It.IsAny<int>())).ReturnsAsync(0);
            tblRuchNaglowek.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblRuchNaglowek, bool>>>())).ReturnsAsync(new List<tblRuchNaglowek>
            {
                new tblRuchNaglowek{IDRuchStatus=1,NrDokumentu=lastNo}
            });
            sut.ListaStatusowRuchu = new List<tblRuchStatus>
            {
                new tblRuchStatus{ IDRuchStatus=1, Symbol="PZ"},
                new tblRuchStatus{ IDRuchStatus=2, Symbol="WZ"}
            };

            Messenger.Default.Send(StatusRuchuTowarowEnum.PrzyjecieZewnetrne_PZ);

            Assert.IsTrue(sut.RuchNaglowek.NrDokumentu == newNo);
        }

        #endregion

        #region Magazyny
        [Test]
        [Ignore("Na potrzeby testu View, w produkcji nie dziala")]
        public void FiltrujListeMagazynowPoZmianieFirmy_WhenChanged_OneMagazynShouldBeSelected()
        {
            tblMagazyn.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<tblMagazyn>
            {
                new tblMagazyn{ IDMagazyn=1,IDFirma=1},
                new tblMagazyn{ IDMagazyn=2,IDFirma=2}
            });

            sut.ZaladujWartosciPoczatkoweCommand.Execute(null);
            sut.WybranaFirmaGAT_Z = new tblFirma { IDFirma = 1 };

            Assert.AreEqual(1, sut.ListaMagazynow_Z.Count());
        }
        #endregion

        #region PoEdycjiKomorkiDataGridCommand
        [Test]
        public void PoEdycjiKomorkiDataGridCommandExecute_WhenTowarIdIsZero_Returns()
        {
            sut.WybranyTowarRuch.IDTowar = 0;
            sut.WybranyStatusRuchu.IDRuchStatus = 1;

            sut.PoEdycjiKomorkiDataGridCommand.Execute(null);

            Assert.IsNull(sut.WybranyTowarRuch.IloscPo);
        }
        [Test]
        public void PoEdycjiKomorkiDataGridCommandExecute_WhenMagazynIdIsZero_Returns()
        {
            sut.RuchNaglowek.IDMagazynDo = 0;
            sut.WybranyTowarRuch = new db.tblRuchTowar { IDTowar=1, IloscPrzed=10, Ilosc=10};
            sut.WybranyStatusRuchu.IDRuchStatus = 1;

            sut.PoEdycjiKomorkiDataGridCommand.Execute(null);

            Assert.IsNull(sut.WybranyTowarRuch.IloscPo);
        }
        [Test]
        public void PoEdycjiKomorkiDataGridCommandExecute_WhenMagazynIdAndTowarIDIsNotZero_CalculatesQuantity()
        {
            sut.RuchNaglowek.IDMagazynDo = 1;
            sut.WybranyTowarRuch = new tblRuchTowar { IDTowar = 1, Ilosc = 10 };
            sut.WybranyStatusRuchu.IDRuchStatus = 1;
            tblRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblRuchTowar, bool>>>())).ReturnsAsync(new List<tblRuchTowar>
            {
                new tblRuchTowar{IDTowar=1, Ilosc=10},
                new tblRuchTowar{IDTowar=1, Ilosc=20}
            });

            sut.PoEdycjiKomorkiDataGridCommand.Execute(null);

            Assert.AreEqual(40,sut.WybranyTowarRuch.IloscPo);
        }

        #endregion
    }
}
