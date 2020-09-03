using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.UI.Utilities.WebScraper;
using GAT_Produkcja.ViewModel.Kontrahent.Dodaj;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Kontrahent.Dodaj
{
    public class DodajKontrahentaViewModelTests : TestBaseGeneric<DodajKontrahentaViewModel>
    {
        private Mock<IPobierzDaneKontrahentaZGUS> pobierzKontrahentaZGUS;
        private Mock<ITblKontrahentRepository> tblKontrahent;

        public override void SetUp()
        {
            base.SetUp();

            pobierzKontrahentaZGUS = new Mock<IPobierzDaneKontrahentaZGUS>();

            tblKontrahent = new Mock<ITblKontrahentRepository>();
            UnitOfWork.Setup(s => s.tblKontrahent).Returns(tblKontrahent.Object);   

            CreateSut();
        }


        public override void CreateSut()
        {
            sut = new DodajKontrahentaViewModel(ViewModelService.Object, pobierzKontrahentaZGUS.Object);
        }

        #region Messengers

        #region Rejestracja

        [Test]
        public void MessengerRegistration()
        {
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<tblKontrahent>>(), It.IsAny<bool>()));
        }

        #endregion

        #endregion

        [Test]
        public void ZapiszCommandCanExecute_KontrahentIsNotValid_False()
        {
            sut.VMEntity.IsValid = false;

            Assert.IsFalse(sut.SaveCommand.CanExecute(null));
        }

        private tblKontrahent GetValidKontrahent()
        {
            return new tblKontrahent
            {
                Nazwa = "test",
                NIP = "PL1234567891",
                Ulica = "test",
                Miasto = "Test",
                KodPocztowy = "Test"
            };
        }

        [Test]
        public void ZapiszCommandExecute_KontrahentIDisZeroAndKontrahentNieWystWBazie_InvokeUnitOfWorkAddMethod()
        {
            //Arrange
            GetValidKontrahent();

            sut.VMEntity= GetValidKontrahent();

            //Act
            sut.SaveCommand.Execute(null);

            //Assert
            UnitOfWork.Verify(s => s.tblKontrahent.Add(It.IsAny<tblKontrahent>()));
        }

        [Test]
        public void ZapiszCommandExecute_KontrahentIstnieje_InvokeSaveAsync()
        {
            sut.VMEntity = GetValidKontrahent();
            sut.VMEntity.ID_Kontrahent = 1;

            //Act
            sut.SaveCommand.Execute(null);

            UnitOfWork.Verify(s => s.SaveAsync());
        }
    }
}
