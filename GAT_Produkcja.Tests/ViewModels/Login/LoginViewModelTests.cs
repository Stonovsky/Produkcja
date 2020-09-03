using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.UI.ViewModel.Login;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;
using System.Threading;
using System.Linq.Expressions;
using GAT_Produkcja.Helpers.Theme;
using GAT_Produkcja.Tests.BaseClasses;

namespace GAT_Produkcja.Tests.ViewModels.Login
{
    [TestFixture]
    public class LoginViewModelTests : TestBase
    {
        private Mock<ITblPracownikGATRepository> tblPracownicy;
        private LoginViewModel sut;

        public override void SetUp()
        {

            base.SetUp();

            tblPracownicy = new Mock<ITblPracownikGATRepository>();

            UnitOfWork.Setup(s => s.tblPracownikGAT).Returns(tblPracownicy.Object);

            tblPracownicy.Setup(s => s.PobierzPracownikowPracujacychAsync())
                        .ReturnsAsync(new List<tblPracownikGAT>()
                        {
                            new tblPracownikGAT()
                            {
                                ID_PracownikGAT = 1, ImieINazwiskoGAT = "Test", HasloPracownika = "Test"
                            }
                        });

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new LoginViewModel(ViewModelService.Object);
        }

        [Test]
        public void ZaladujWartosciPoczatkoweCommand_InvokeUoWMethod()
        {
            sut.ZaladujWartosciPoczatkoweCommand.Execute(null);

            UnitOfWork.Verify(s => s.tblPracownikGAT.PobierzPracownikowPracujacychAsync());
        }

        [Test, Apartment(ApartmentState.STA)] //, RequiresSTA
        [TestCase("Test", "Test", true)]
        [TestCase("Test", "badPassword", false)]
        public void ZalogujCommandExecute(string imieINazwisko, string haslo, bool expected)
        {
            //sut.PracownikGAT = new tblPracownikGAT() { ID_PracownikGAT = 1, HasloPracownika = "Test" };
            //PasswordBox passwordBox = new PasswordBox() { Password = haslo };

            //tblPracownikGAT pracownik = new tblPracownikGAT();
            //Messenger.Default.Register<tblPracownikGAT>(this, "MainMenu", m => pracownik = m);

            //sut.ZaladujWartosciPoczatkoweCommand.Execute(null);
            //sut.ZalogujCommand.Execute(passwordBox);

            //bool actual = haslo == pracownik.HasloPracownika;

            //Assert.AreEqual(actual, expected);

        }

        [Test]
        public void WhenKodKreskowyPropertyChanged_ZalogujZKoduKreskowegoShouldBeInvoked()
        {
            sut.KodKreskowy = "1231231231231";

            tblPracownicy.Verify(v => v.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblPracownikGAT, bool>>>()));
        }


        [Test]
        public void ZalogujZKoduKreskowego_WhenKodKreskowyIsWrong_ViewServiceIsNotInvoked()
        {
            tblPracownicy.Setup(s => s.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblPracownikGAT, bool>>>()));
            sut.KodKreskowy = "1231231231231";

            ViewService.Verify(v => v.Close<LoginViewModel>(), Times.Never);
        }
        [Test]
        public void ZalogujZKoduKreskowego_WhenKodKreskowyIsCorrect_ViewServiceIsInvoked()
        {
            tblPracownicy.Setup(s => s.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblPracownikGAT, bool>>>()))
                         .ReturnsAsync(new tblPracownikGAT { ID_PracownikGAT = 1, KodKreskowy = "1231231231231" });

            sut.KodKreskowy = "1231231231231";

            ViewService.Verify(v => v.Close<LoginViewModel>());
        }

        [Test]
        public void WhenKodKreskowyPropertyChanged_ZalogujZKoduKreskowego_SendMessageWithPracownikGAT()
        {
            tblPracownicy.Setup(s => s.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblPracownikGAT, bool>>>()))
                         .ReturnsAsync(new tblPracownikGAT { ID_PracownikGAT = 1, KodKreskowy = "1231231231231" });
            sut.KodKreskowy = "1231231231231";

            Messenger.Verify(v => v.Send<tblPracownikGAT>(It.IsAny<tblPracownikGAT>(), "MainMenu"));
        }

    }
}
