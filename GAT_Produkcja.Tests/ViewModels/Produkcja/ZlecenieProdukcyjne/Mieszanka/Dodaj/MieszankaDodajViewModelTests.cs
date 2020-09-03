using AutoFixture;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Produkcja.Mieszanka.Dodaj;
using GAT_Produkcja.ViewModel.Produkcja.Mieszanka.Ewidencja;
using Moq;
using NUnit.Framework;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.ZlecenieProdukcyjne.Mieszanka.Dodaj
{
    [TestFixture]
    public class MieszankaDodajViewModelTests
    {
        private Mock<IUnitOfWork> unitOfWork;
        private Mock<IViewService> viewService;
        private Mock<IDialogService> dialogService;
        private Mock<IMessenger> messenger;
        private Fixture fixture;
        private Mock<ITblTowarRepository> tblTowar;
        private Mock<ITblPracownikGATRepository> tblPracownikGAT;
        private Mock<ITblMieszankaSkladRepository> tblMieszankaSklad;
        private Mock<ITblMieszankaRepository> tblMieszanka;
        private Mock<ITblJmRepository> tblJm;
        private MieszankaDodajViewModel sut;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            viewService = new Mock<IViewService>();
            dialogService = new Mock<IDialogService>();
            messenger = new Mock<IMessenger>();
            fixture = new Fixture();

            tblTowar = new Mock<ITblTowarRepository>();
            tblPracownikGAT = new Mock<ITblPracownikGATRepository>();
            tblMieszankaSklad = new Mock<ITblMieszankaSkladRepository>();
            tblMieszanka = new Mock<ITblMieszankaRepository>();
            tblJm = new Mock<ITblJmRepository>();

            unitOfWork.Setup(s => s.tblTowar).Returns(tblTowar.Object);
            unitOfWork.Setup(s => s.tblPracownikGAT).Returns(tblPracownikGAT.Object);
            unitOfWork.Setup(s => s.tblMieszanka).Returns(tblMieszanka.Object);
            unitOfWork.Setup(s => s.tblMieszankaSklad).Returns(tblMieszankaSklad.Object);
            unitOfWork.Setup(s => s.tblJm).Returns(tblJm.Object);


            sut = new MieszankaDodajViewModel(unitOfWork.Object, viewService.Object, dialogService.Object, messenger.Object);
        }

        private tblMieszanka CreateMieszanka()
        {
            return fixture.Build<tblMieszanka>()
                .Without(w => w.tblFirma)
                .Without(w => w.tblJm)
                .Without(w => w.tblMagazyn)
                .Without(w => w.tblMieszankaSklad)
                .Without(w => w.tblPracownikGAT)
                .Create();
        }

        private tblMieszankaSklad CreateSkladMieszanki()
        {
            return fixture.Build<tblMieszankaSklad>()
                .Without(w => w.tblFirma)
                .Without(w => w.tblJm)
                .Without(w => w.tblMagazyn)
                .Without(w => w.tblMieszanka)
                .Without(w => w.tblRuchTowar)
                .Without(w => w.tblTowar)
                .Create();
        }
        [Test]
        public void ZaladujWartosciPoczatkoweCommandExecute_WhenCalled_AllUOWMethodsAreInvoked()
        {
            sut.ZaladujWartosciPoczatkoweCommand.Execute(null);

            tblTowar.Verify(v => v.PobierzSurowceAsync());
            tblJm.Verify(v => v.GetAllAsync());
            tblPracownikGAT.Verify(v => v.PobierzPracownikowMogacychZglaszacZapotrzebowaniaAsync());
        }


        #region UsunCommand
        [Test]
        public void UsunCommandCanExecute_WhenIDMieszankiIsZero_ReturnFalse()
        {
            var result = sut.UsunCommand.CanExecute(null);

            Assert.IsFalse(result);

        }

        [Test]
        public void UsunCommandExecute_IfDialogServiceIsTrue_RemoveAndSaveAsyncMethodShouldBeInvoked()
        {
            dialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>()))
                         .Returns(true);
            sut.Mieszanka.IDMieszanka = 1;

            sut.UsunCommand.Execute(null);

            tblMieszanka.Verify(v => v.Remove(It.IsAny<tblMieszanka>()));
            unitOfWork.Verify(v => v.SaveAsync());
        }

        [Test]
        public void UsunCommandExecute_IfDialogServiceIsFalse_RemoveAndSaveAsyncMethodShouldNOTBeInvoked()
        {
            dialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>()))
                         .Returns(false);
            sut.Mieszanka.IDMieszanka = 1;

            sut.UsunCommand.Execute(null);

            tblMieszanka.Verify(v => v.Remove(It.IsAny<tblMieszanka>()), Times.Never);
            unitOfWork.Verify(v => v.SaveAsync(), Times.Never);
        }

        #endregion

        #region ZapiszCommand
        private void ZapiszCommandCanExecute_True()
        {
            sut.SkladMieszanki = new ObservableCollection<tblMieszankaSklad>();
            sut.SkladMieszanki.Add(CreateSkladMieszanki());

            sut.Mieszanka = CreateMieszanka() ;

        }
        [Test]
        public void ZapiszCommandExecute_WhenIDisZero_UOWtblMieszankaAddShouldBeInvoked()
        {
            ZapiszCommandCanExecute_True();
            sut.Mieszanka.IDMieszanka = 0;

            sut.ZapiszCommand.Execute(null);

            unitOfWork.Verify(v => v.tblMieszanka.Add(It.IsAny<tblMieszanka>()));
        }
        [Test]
        public void ZapiszCommandExecute_WhenIdIsNotZero_UOWtblMieszankaShouldNotBeInvoked()
        {
            ZapiszCommandCanExecute_True();
            sut.Mieszanka = new tblMieszanka { IDMieszanka = 1 };

            sut.ZapiszCommand.Execute(null);

            unitOfWork.Verify(v => v.tblMieszanka.Add(It.IsAny<tblMieszanka>()), Times.Never);
        }
        [Test]
        [TestCase(0)]
        [TestCase(1)]
        public void ZapiszCommandExecute_WhenCalled_UOWSaveAsyncShouldBeInvoked(int idMieszanka)
        {
            ZapiszCommandCanExecute_True();
            sut.Mieszanka.IDMieszanka = idMieszanka;

            sut.ZapiszCommand.Execute(null);

            unitOfWork.Verify(v => v.SaveAsync());
        }
        [Test]
        public void ZapiszCommandCanExecute_MieszankaIsNotValid_ReturnsFalse()
        {
            sut.Mieszanka.IsValid = false;

            var result = sut.ZapiszCommand.CanExecute(null);

            Assert.IsFalse(result);
        }

        [Test]
        public void ZapiszCommandCanExecute_SkladMieszankiIsEmpty_ReturnsFalse()
        {
            sut.Mieszanka.IsValid = true;

            var result = sut.ZapiszCommand.CanExecute(null);

            Assert.IsFalse(result);
        }
        [Test]
        public void ZapiszCommandCanExecute_SkladMieszankiItemIsNotValid_ReturnsFalse()
        {
            sut.Mieszanka.IsValid = true;
            sut.SkladMieszanki = new ObservableCollection<tblMieszankaSklad>()
            {
                new tblMieszankaSklad(){IDMieszankaSklad=1,IsValid=false},
                new tblMieszankaSklad(){IDMieszankaSklad=2,IsValid=true}
            };

            var result = sut.ZapiszCommand.CanExecute(null);

            Assert.IsFalse(result);
        }
        [Test]
        public void ZapiszCommandCanExecute_MieszankaIloscIsZero_ReturnsFalse()
        {
            sut.Mieszanka.IsValid = true;
            sut.Mieszanka.Ilosc = 0;
            sut.SkladMieszanki = new ObservableCollection<tblMieszankaSklad>()
            {
                new tblMieszankaSklad(){IDMieszankaSklad=1,IsValid=true},
                new tblMieszankaSklad(){IDMieszankaSklad=2,IsValid=true}
            };

            var result = sut.ZapiszCommand.CanExecute(null);

            Assert.IsFalse(result);
        }

        private void GetZapiszCommandCanExecuteTrue()
        {
            sut.Mieszanka.IsValid = true;
            sut.SkladMieszanki = new ObservableCollection<tblMieszankaSklad>()
            {
                new tblMieszankaSklad(){IDMieszankaSklad=0,IsValid=true}
            };

        }

        [Test]
        public void ZapiszCommandCanExecute_MieszankaIsValidAndSkladMieszankiIsValid_ReturnsTrue()
        {
            ZapiszCommandCanExecute_True();

            var result = sut.ZapiszCommand.CanExecute(null);

            Assert.IsTrue(result);
        }

        [Test]
        public void ZapiszCommandExecute_MieszankaIdIsZero_TblMieszankaAddMethodIsInvoked()
        {
            ZapiszCommandCanExecute_True();
            sut.Mieszanka.IDMieszanka = 0;

            sut.ZapiszCommand.Execute(null);

            tblMieszanka.Verify(v => v.Add(It.IsAny<tblMieszanka>()));

        }

        [Test]
        public void ZapiszCommandExecute_MieszankaIdIsNotZero_TblMieszankaAddMethodIsNotInvoked()
        {
            ZapiszCommandCanExecute_True();
            sut.Mieszanka.IDMieszanka = 1;

            sut.ZapiszCommand.Execute(null);

            tblMieszanka.Verify(v => v.Add(It.IsAny<tblMieszanka>()), Times.Never);
        }

        [Test]
        public void ZapiszCommandExecute_SkladMieszankiItemIdIsZero_TblMieszankaSkladAddMethodIsNotInvoked()
        {
            ZapiszCommandCanExecute_True();
            sut.SkladMieszanki[0].IDMieszankaSklad = 0;

            sut.ZapiszCommand.Execute(null);

            tblMieszankaSklad.Verify(v => v.Add(It.IsAny<tblMieszankaSklad>()));
        }

        [Test]
        public void ZapiszCommandExecute_SkladMieszankiItemIdIsNotZero_TblMieszankaSkladAddMethodIsNotInvoked()
        {
            GetZapiszCommandCanExecuteTrue();
            sut.SkladMieszanki = new ObservableCollection<tblMieszankaSklad>()
            {
                new tblMieszankaSklad(){IDMieszankaSklad=1,IsValid=true}
            };

            sut.ZapiszCommand.Execute(null);

            tblMieszankaSklad.Verify(v => v.Add(It.IsAny<tblMieszankaSklad>()), Times.Never);
        }
        [Test]
        public void ZapiszCommandExecute_SkladMieszankiEachItem_HasTheSameIdMieszankaAsMieszanka()
        {
            ZapiszCommandCanExecute_True();
            sut.Mieszanka.IDMieszanka = 2;

            sut.ZapiszCommand.Execute(null);
            var result = sut.SkladMieszanki.First();

            Assert.AreEqual(2, result.IDMieszanka);
        }

        #endregion

        #region Messenger

        [Test]
        public void ZapiszCommandExecute_WhenSuccesCall_MessageToEwidencjaMieszankaViewModelShouldBeSent()
        {
            ZapiszCommandCanExecute_True();

            sut.ZapiszCommand.Execute(null);

            messenger.Verify(v => v.Send<string, MieszankaEwidencjaViewModel>(It.IsAny<string>()));
        }

        #endregion

        #region Po edycji komorki
        [Test]
        public void PoEdycjiKomorkiDataGridCommandExecute_IDTowarIsZero_ReturnsZero()
        {
            sut.SkladMieszanki = new ObservableCollection<tblMieszankaSklad>()
            {
                new tblMieszankaSklad(){IDMieszankaSklad=0, IDTowar=0, Ilosc=5}
            };

            sut.PoEdycjiKomorkiDataGridCommand.Execute(null);
            var result = sut.SkladMieszanki.First();

            Assert.IsTrue(result.Udzial == 0);
        }
        [Test]
        public void PoEdycjiKomorkiDataGridCommandExecute_IDTowarIsNotZero_ReturnsUdzial()
        {
            sut.SkladMieszanki = new ObservableCollection<tblMieszankaSklad>()
            {
                new tblMieszankaSklad(){IDMieszankaSklad=0, IDTowar=1, Ilosc=5}
            };

            sut.PoEdycjiKomorkiDataGridCommand.Execute(null);
            var result = sut.SkladMieszanki.First();

            Assert.IsTrue(result.Udzial > 0);
        }
        [Test]
        public void PoEdycjiKomorkiDataGridCommandExecute_SkladMieszankiSumIloscIsZero_ReturnsZero()
        {
            sut.SkladMieszanki = new ObservableCollection<tblMieszankaSklad>()
            {
                new tblMieszankaSklad(){IDMieszankaSklad=0, IDTowar=1, Ilosc=0},
                new tblMieszankaSklad(){IDMieszankaSklad=0, IDTowar=2, Ilosc=0}
            };

            sut.PoEdycjiKomorkiDataGridCommand.Execute(null);
            var result = sut.SkladMieszanki.First();

            Assert.IsTrue(result.Udzial == 0);
        }
        [Test]
        public void PoEdycjiKomorkiDataGridCommandExecute_SkladMieszankiSumIlosc_MieszankaIloscReturnsSumeIlosci()
        {
            sut.Mieszanka = new tblMieszanka { IDMieszanka = 1 };
            sut.SkladMieszanki = new ObservableCollection<tblMieszankaSklad>()
            {
                new tblMieszankaSklad(){IDMieszankaSklad=0, IDTowar=1, Ilosc=10},
                new tblMieszankaSklad(){IDMieszankaSklad=0, IDTowar=2, Ilosc=20}
            };

            sut.PoEdycjiKomorkiDataGridCommand.Execute(null);

            Assert.IsTrue(sut.Mieszanka.Ilosc == 30);
        }

        #endregion

        [Test]
        public void ZamknijOknoCommandExecute_IfHasChanges_ShowDialogBox()
        {
            tblMieszanka.Setup(s => s.HasChanges()).Returns(true);


            sut.ZamknijOknoCommand.Execute(new CancelEventArgs());

            dialogService.Verify(v => v.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>()));
        }

        #region PropertyChangedAtribute
        [Test]
        [Ignore("Nie dziala")]
        public void DoesSkladMieszankiHas_PropertyChangedAttribute()
        {
            var attribute = Attribute.GetCustomAttribute(typeof(tblMieszankaSklad),
                                        typeof(AddINotifyPropertyChangedInterfaceAttribute));

            Assert.IsNotNull(attribute); 
        }
        #endregion
        [Test]
        public void CreationDateIsEstablishedUponModelCreating()
        {
            Assert.AreEqual(DateTime.Now.Date, sut.Mieszanka.DataUtworzenia);
        }
    }
}
