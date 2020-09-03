using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Finanse.Banki.StanKont.DodajStanKonta;
using GAT_Produkcja.ViewModel.Finanse.Banki.StanKont.EwidencjaStanKont.Messages;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Finanse.Banki.StanKont.DodajStanKonta
{
    public class DodajStanKontaViewModelTests : TestBase
    {
        private DodajStanKontaViewModel sut;
        private Mock<IVwFinanseBankAGGRepository> vwFinanseBankAGG;
        private Mock<IVwFinanseBankGTXRepository> vwFinanseBankGTX;
        private Mock<IVwFinanseBankGTX2Repository> vwFinanseBankGTX2;
        private Mock<ITblFinanseStanKontaRepository> tblFinanseStanKonta;

        public override void SetUp()
        {
            base.SetUp();

            vwFinanseBankAGG = new Mock<IVwFinanseBankAGGRepository>();
            vwFinanseBankGTX = new Mock<IVwFinanseBankGTXRepository>();
            vwFinanseBankGTX2 = new Mock<IVwFinanseBankGTX2Repository>();
            tblFinanseStanKonta = new Mock<ITblFinanseStanKontaRepository>();

            UnitOfWork.Setup(s => s.vwFinanseBankAGG).Returns(vwFinanseBankAGG.Object);
            UnitOfWork.Setup(s => s.vwFinanseBankGTX).Returns(vwFinanseBankGTX.Object);
            UnitOfWork.Setup(s => s.vwFinanseBankGTX2).Returns(vwFinanseBankGTX2.Object);
            UnitOfWork.Setup(s => s.tblFinanseStanKonta).Returns(tblFinanseStanKonta.Object);

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new DodajStanKontaViewModel(ViewModelService.Object);
        }

        #region Messenger
        #region Registration
        [Test]
        public void MessengerRegistration()
        {
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<tblFinanseStanKonta>>(), It.IsAny<bool>()));

        }

        [Test]
        public void GdyPrzeslanoStanKonta_DodajeTylkoTenStanDoListyCelemEdycji()
        {
            MessengerSend(new tblFinanseStanKonta { IDFinanseStanKonta = 1, Stan = 1 });

            sut.LoadCommand.Execute(null);

            Assert.AreEqual(1, sut.StanyKont.Count());
        }
        #endregion
        #endregion

        #region LoadCommand

        [Test]
        public void LoadCommand_LadujeBankiZBazy()
        {
            sut.LoadCommand.Execute(null);

            vwFinanseBankAGG.Verify(v => v.GetAllAsync());
            vwFinanseBankGTX.Verify(v => v.GetAllAsync());
            vwFinanseBankGTX2.Verify(v => v.GetAllAsync());
        }

        [Test]
        public void LoadCommand_DodajeBankiDoStanowKont()
        {
            vwFinanseBankAGG.Setup(s => s.GetAllAsync())
                            .ReturnsAsync(new List<vwFinanseBankAGG>
                            {
                                new vwFinanseBankAGG{Nazwa="PKO",Id=1,IdFirma=1,Numer="111", Waluta="PLN"},
                                new vwFinanseBankAGG{Nazwa="PKO",Id=1,IdFirma=1,Numer="222", Waluta="EUR"}
                            });

            vwFinanseBankGTX.Setup(s => s.GetAllAsync())
                            .ReturnsAsync(new List<vwFinanseBankGTX>
                            {
                                new vwFinanseBankGTX{Nazwa="PKO",Id=1,IdFirma=1,Numer="333",Waluta="PLN"}
                            });

            vwFinanseBankGTX2.Setup(s => s.GetAllAsync())
                             .ReturnsAsync(new List<vwFinanseBankGTX2>
                             {
                                new vwFinanseBankGTX2{Nazwa="PKO",Id=1,IdFirma=1,Numer="444", Waluta="EUR"}
                             });


            sut.LoadCommand.Execute(null);

            Assert.AreEqual(4, sut.StanyKont.Count());
            Assert.AreEqual("EUR", sut.StanyKont[1].Waluta);
            Assert.AreEqual("PKO", sut.StanyKont[1].BankNazwa);
            Assert.AreEqual("222", sut.StanyKont[1].NrKonta);
            Assert.AreEqual(7, sut.StanyKont[1].IdOperator);
        }

        #endregion

        #region IsValid
        [Test]
        public void IsValid_GdyBrakWpisanychStanowKont_False()
        {
            sut.StanyKont = new ObservableCollection<tblFinanseStanKonta>
            {
                new tblFinanseStanKonta()
            };

            var isValid = sut.IsValid;

            Assert.IsFalse(isValid);
        }
        [Test]
        public void IsValid_GdyWpisanoChocJedenStanKonta_True()
        {
            sut.StanyKont = new ObservableCollection<tblFinanseStanKonta>
            {
                new tblFinanseStanKonta(){Stan=1},
                new tblFinanseStanKonta(){Stan=0},
            };

            var isValid = sut.IsValid;

            Assert.IsTrue(isValid);
        }
        #endregion

        #region SaveCommand
        private void SaveCommandCanExecute_True()
        {

        }
        #region CanExecute
        [Test]
        public void SaveCommandCanExecute_IsValidFals_False()
        {
            var actual = sut.SaveCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }

        [Test]
        public void SaveCommandCanExecute_IsValidTrue_True()
        {
            sut.StanyKont = new ObservableCollection<tblFinanseStanKonta>
            {
                new tblFinanseStanKonta(){Stan=1},
                new tblFinanseStanKonta(){Stan=0},
            };
            var actual = sut.SaveCommand.CanExecute(null);

            Assert.IsTrue(actual);
        }
        #endregion

        #region Execute
        [Test]
        public void SaveCommandExecute_DodajeDateDoListy()
        {
            sut.StanyKont = new ObservableCollection<tblFinanseStanKonta>
            {
                new tblFinanseStanKonta(){Stan=1},
                new tblFinanseStanKonta(){Stan=2},
            };

            sut.SaveCommand.Execute(null);

            Assert.IsTrue(sut.StanyKont[0].DataDodania > DateTime.MinValue);
        }

        [Test]
        public void SaveCommandExecute_NieZapisujeStanowZerowych()
        {
            sut.StanyKont = new ObservableCollection<tblFinanseStanKonta>
            {
                new tblFinanseStanKonta(){Stan=0},
            };

            sut.SaveCommand.Execute(null);

            tblFinanseStanKonta.Verify(v => v.AddRange(It.IsAny<IEnumerable<tblFinanseStanKonta>>()), Times.Never);
        }

        [Test]
        public void SaveCommandExecute_NieDodajeDoBazyStanowONieZerowymId()
        {
            sut.StanyKont = new ObservableCollection<tblFinanseStanKonta>
            {
                new tblFinanseStanKonta(){IDFinanseStanKonta=1, Stan=1},
            };

            sut.SaveCommand.Execute(null);

            tblFinanseStanKonta.Verify(v => v.AddRange(It.IsAny<IEnumerable<tblFinanseStanKonta>>()),Times.Never);
            UnitOfWork.Verify(v => v.SaveAsync());
        }

        [Test]
        public void SaveCommandExecute_ZapisujeDoBazyStanyNieZerowe()
        {
            sut.StanyKont = new ObservableCollection<tblFinanseStanKonta>
            {
                new tblFinanseStanKonta(){Stan=1},
            };

            sut.SaveCommand.Execute(null);

            tblFinanseStanKonta.Verify(v => v.AddRange(It.IsAny<IEnumerable<tblFinanseStanKonta>>()));
            UnitOfWork.Verify(v => v.SaveAsync());
        }

        [Test]
        public void SaveCommandExecute_PoZapisieDoBazy_WysylaMessage()
        {
            sut.StanyKont = new ObservableCollection<tblFinanseStanKonta>
            {
                new tblFinanseStanKonta(){Stan=1},
            };

            sut.SaveCommand.Execute(null);

            Messenger.Verify(v => v.Send(It.IsAny<tblFinanseStanKonta>(),"ToList"));
        }

        [Test]
        public void SaveCommandExecute_PoZapisieDoBazy_NiePokazujeInformacjiUzytkownikowi()
        {
            sut.StanyKont = new ObservableCollection<tblFinanseStanKonta>
            {
                new tblFinanseStanKonta(){Stan=1},
            };

            sut.SaveCommand.Execute(null);

            DialogService.Verify(v => v.ShowInfo_BtnOK(It.IsAny<string>(), It.IsAny<string>()),Times.Never);
        }
        [Test]
        public void SaveCommandExecute_PoZapisieDoBazy_ZamykaFormularz()
        {
            sut.StanyKont = new ObservableCollection<tblFinanseStanKonta>
            {
                new tblFinanseStanKonta(){Stan=1},
            };

            sut.SaveCommand.Execute(null);

            ViewService.Verify(v => v.Close(sut.GetType().Name));
        }
        #endregion
        #endregion
    }
}
