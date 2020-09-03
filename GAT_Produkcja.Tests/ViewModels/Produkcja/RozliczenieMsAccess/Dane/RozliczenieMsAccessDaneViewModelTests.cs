using AutoFixture;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.dbComarch.UnitOfWork;
using GAT_Produkcja.dbMsAccess.UnitOfWork;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Naglowek;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.PW;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.RW;
using Moq;
using NUnit.Framework;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.RozliczenieMsAccess.Dane
{
    [TestFixture]
    public class RozliczenieMsAccessDaneViewModelTests : TestBase
    {

        private Fixture fixture;
        private Mock<IRozliczenieMsAccessPWViewModel> rozliczenieMsAccessPWViewModel;
        private Mock<IRozliczenieMsAccessRWViewModel> rozliczenieMsAccessRWViewModel;
        private Mock<ITblProdukcjaRozliczenie_DaneRepository> tblProdukcjaRozliczenie_Dane;
        private RozliczenieMsAccessNaglowekViewModel sut;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            fixture = new Fixture();

            rozliczenieMsAccessPWViewModel = new Mock<IRozliczenieMsAccessPWViewModel>();
            rozliczenieMsAccessRWViewModel = new Mock<IRozliczenieMsAccessRWViewModel>();

            tblProdukcjaRozliczenie_Dane = new Mock<ITblProdukcjaRozliczenie_DaneRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaRozliczenie_Dane).Returns(tblProdukcjaRozliczenie_Dane.Object);

            CreateSut();
        }

        public override void CreateSut()
        {
            sut = new RozliczenieMsAccessNaglowekViewModel(ViewModelService.Object, 
                                                           rozliczenieMsAccessPWViewModel.Object,
                                                           rozliczenieMsAccessRWViewModel.Object);
        }

        [Test]
        public void MessengerRegistration()
        {

        }

        #region RozliczCommand

        private void RozliczCommandCanExecute_True()
        {
            sut.DaneWejsciowe = fixture.Build<tblProdukcjaRozliczenie_Naglowek>()
                .Without(c => c.tblPracownikGAT)
                .Create();
        }
        [Test]
        public void RozliczCommandCanExecute_GdyIsNotValid_False()
        {
            var actual = sut.RozliczCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }

        [Test, Ignore("Do poprawienia")]
        public void RozliczCommandCanExecute_GdyModelIsValid_True()
        {
            RozliczCommandCanExecute_True();

            var actual = sut.RozliczCommand.CanExecute(null);

            Assert.IsTrue(actual);
        }

        [Test, Ignore("Do poprawienia")]
        public void RozliczCommand_WysylaMessage()
        {
            RozliczCommandCanExecute_True();

            sut.RozliczCommand.Execute(null);

            Messenger.Verify(v => v.Send(It.IsAny<tblProdukcjaRozliczenie_Naglowek>()));
        }

        #endregion

        #region WybranySurowiec

        [Test]
        public void WybranySurowiecProp_KiedyZmianaISurowiecNieWystepujeWSlowniku_PrzypisujeSurowiecZMsAccessJakoZero()
        {
            sut.WybranySurowiec = new tblTowar() { IDTowar = 1, Nazwa = "Włókno PP 6,7/76 W UV HT" };

            Assert.IsTrue(sut.DaneWejsciowe.IDTowarAccess == 0);
        }
        #endregion

        //[Test]
        //public void SaveAsync_JezeliIDJestZero_UoWAddJestWywolane()
        //{
        //    sut.SaveAsync(null);

        //    tblProdukcjaRozliczenie_Dane.Verify(v => v.Add(It.IsAny<tblProdukcjaRozliczenie_Naglowek>()));
        //}
    }
}
