using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.Utilities.ExcelReport;
using GAT_Produkcja.ViewModel.Produkcja.CenyTransferowe.Dodaj;
using GAT_Produkcja.ViewModel.Produkcja.CenyTransferowe.Message;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.CenyTransferowe
{
    public class DodajCenyTransferoweViewModelTests : TestBase
    {
        private DodajCenyTransferoweViewModel sut;
        private Mock<IXlsService> xlsService;
        private Mock<ITblProdukcjaRozliczenie_CenyTransferoweRepository> tblProdukcjaRozliczenie_CenyTransferowe;

        public override void SetUp()
        {
            base.SetUp();
            
            xlsService = new Mock<IXlsService>();

            tblProdukcjaRozliczenie_CenyTransferowe = new Mock<ITblProdukcjaRozliczenie_CenyTransferoweRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaRozliczenie_CenyTransferowe).Returns(tblProdukcjaRozliczenie_CenyTransferowe.Object);

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new DodajCenyTransferoweViewModel(ViewModelService.Object, xlsService.Object);
        }

        #region LoadCommand
        [Test]
        public void LoadCommandExecute_LadujeDaneZBD()
        {
            sut.LoadCommand.Execute(null);

            //tblProdukcjaRozliczenie_CenyTransferowe.Verify(v => v.GetAllAsync());
            tblProdukcjaRozliczenie_CenyTransferowe.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRozliczenie_CenyTransferowe,bool>>>()));
        }
        #endregion

        #region DodajCenyTransferoweZPliku
        [Test]
        public void DodajCenyTransferoweZPliku_Condition_Expectations()
        {
            sut.ListaCenTransferowych = new List<tblProdukcjaRozliczenie_CenyTransferowe>
            {
                new tblProdukcjaRozliczenie_CenyTransferowe {TowarNazwa="AT 90", CenaTransferowa=1, CenaHurtowa=1}
            };
            xlsService.Setup(s => s.GetTranferPrices(It.IsAny<string>(), It.IsAny<string>())).Returns(new List<tblProdukcjaRozliczenie_CenyTransferowe>
            {
                new tblProdukcjaRozliczenie_CenyTransferowe {TowarNazwa="AT 90", CenaTransferowa=2, CenaHurtowa=2},
                new tblProdukcjaRozliczenie_CenyTransferowe {TowarNazwa="AT 100", CenaTransferowa=1, CenaHurtowa=1},
            });


            sut.DodajCenyTransferoweZPlikuXls.Execute(null);


            Assert.AreEqual(2, sut.ListaZmienionychCen.Count());
            Assert.AreEqual(2, sut.ListaZmienionychCen.First().CenaTransferowa);
            Assert.AreEqual(true, sut.ListaZmienionychCen.First().CzyAktualna);
            Assert.AreEqual(DateTime.Now.Date, sut.ListaZmienionychCen.First().DataDodania.GetValueOrDefault());
        }

        [Test]
        public void DodajCenyTransferoweZPliku_DodajeIDGrupyDoCeny()
        {
            sut.ListaCenTransferowych = new List<tblProdukcjaRozliczenie_CenyTransferowe>
            {
                new tblProdukcjaRozliczenie_CenyTransferowe {TowarNazwa="AT 90", CenaTransferowa=1, CenaHurtowa=1}
            };
            xlsService.Setup(s => s.GetTranferPrices(It.IsAny<string>(), It.IsAny<string>())).Returns(new List<tblProdukcjaRozliczenie_CenyTransferowe>
            {
                new tblProdukcjaRozliczenie_CenyTransferowe {TowarNazwa="ALTEX AT 90", CenaTransferowa=2, CenaHurtowa=2},
                new tblProdukcjaRozliczenie_CenyTransferowe {TowarNazwa="ALTEX AT 100", CenaTransferowa=1, CenaHurtowa=1},
                new tblProdukcjaRozliczenie_CenyTransferowe {TowarNazwa="AT CELL Comfort 001.200", CenaTransferowa=1, CenaHurtowa=1},
                new tblProdukcjaRozliczenie_CenyTransferowe {TowarNazwa="AT CELL Comfort 002.200", CenaTransferowa=1, CenaHurtowa=1},
                new tblProdukcjaRozliczenie_CenyTransferowe {TowarNazwa="AT BORD 900", CenaTransferowa=1, CenaHurtowa=1},
            });


            sut.DodajCenyTransferoweZPlikuXls.Execute(null);


            Assert.AreEqual(2, sut.ListaZmienionychCen[0].IDTowarGrupa);
            Assert.AreEqual(2, sut.ListaZmienionychCen[1].IDTowarGrupa);
            Assert.AreEqual(1, sut.ListaZmienionychCen[2].IDTowarGrupa);
            Assert.AreEqual(1, sut.ListaZmienionychCen[3].IDTowarGrupa);
            Assert.AreEqual(18, sut.ListaZmienionychCen[4].IDTowarGrupa);
        }

        [Test]
        public void DodajCenyTransferoweZPliku_ZmieniaStareCenyNaNieaktualne()
        {
            sut.ListaCenTransferowych = new List<tblProdukcjaRozliczenie_CenyTransferowe>
            {
                new tblProdukcjaRozliczenie_CenyTransferowe {IDProdukcjaRozliczenie_CenyTransferowe=1, TowarNazwa="ALTEX AT 90", CenaTransferowa=1, CenaHurtowa=1, CzyAktualna=true},
                new tblProdukcjaRozliczenie_CenyTransferowe {IDProdukcjaRozliczenie_CenyTransferowe=2, TowarNazwa="ALTEX AT 100", CenaTransferowa=1, CenaHurtowa=1, CzyAktualna=true},
                new tblProdukcjaRozliczenie_CenyTransferowe {IDProdukcjaRozliczenie_CenyTransferowe=2, TowarNazwa="ALTEX AT 150", CenaTransferowa=1, CenaHurtowa=1, CzyAktualna=true},
                new tblProdukcjaRozliczenie_CenyTransferowe {IDProdukcjaRozliczenie_CenyTransferowe=2, TowarNazwa="ALTEX AT 200", CenaTransferowa=1, CenaHurtowa=1, CzyAktualna=true},
            };
            xlsService.Setup(s => s.GetTranferPrices(It.IsAny<string>(), It.IsAny<string>())).Returns(new List<tblProdukcjaRozliczenie_CenyTransferowe>
            {
                new tblProdukcjaRozliczenie_CenyTransferowe {TowarNazwa="ALTEX AT 90", CenaTransferowa=2, CenaHurtowa=2},
                new tblProdukcjaRozliczenie_CenyTransferowe {TowarNazwa="ALTEX AT 100", CenaTransferowa=2, CenaHurtowa=2},
            });


            sut.DodajCenyTransferoweZPlikuXls.Execute(null);

            Assert.IsFalse(sut.ListaCenTransferowych.First().CzyAktualna);
            Assert.IsTrue(sut.ListaCenTransferowych.Last().CzyAktualna);
        }
        #endregion

        #region ZmienDateCommand
        #region CanExecute
        [Test]
        public void ZmienDateCanExecute_GdyListaZmienionychCenNull_False()
        {
            sut.ListaZmienionychCen = null;
            sut.DataDodania = DateTime.Now.Date;

            var actual = sut.ZmienDateCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }
        [Test]
        public void ZmienDateCanExecute_GdyListaZmienionychCenPusta_False()
        {
            sut.ListaZmienionychCen = new List<tblProdukcjaRozliczenie_CenyTransferowe>();
            sut.DataDodania = DateTime.Now.Date;

            var actual = sut.ZmienDateCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }
        [Test]
        public void ZmienDateCanExecute_GdyDataTakaSamaJakDataDodania_False()
        {
            sut.ListaZmienionychCen = new List<tblProdukcjaRozliczenie_CenyTransferowe>
            {
                new tblProdukcjaRozliczenie_CenyTransferowe{DataDodania=DateTime.Now.Date}
            };
            sut.DataDodania = DateTime.Now.Date;

            var actual = sut.ZmienDateCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }

        [Test]
        public void ZmienDateCanExecute_GdyDataInnaOdDatyDodania_False()
        {
            sut.ListaZmienionychCen = new List<tblProdukcjaRozliczenie_CenyTransferowe>
            {
                new tblProdukcjaRozliczenie_CenyTransferowe{DataDodania=DateTime.Now.Date.AddDays(-1)}
            };
            sut.DataDodania = DateTime.Now.Date;

            var actual = sut.ZmienDateCommand.CanExecute(null);

            Assert.IsTrue(actual);
        }
        #endregion

        #region Execute
        [Test]
        public void ZmienDateExecute_ZmieniaDateDlaWszystkichNowychCenTransferowych()
        {
            sut.ListaZmienionychCen = new List<tblProdukcjaRozliczenie_CenyTransferowe>
            {
                new tblProdukcjaRozliczenie_CenyTransferowe{DataDodania=DateTime.Now.Date.AddDays(-1)},
                new tblProdukcjaRozliczenie_CenyTransferowe{DataDodania=DateTime.Now.Date.AddDays(-2)},
                new tblProdukcjaRozliczenie_CenyTransferowe{DataDodania=DateTime.Now.Date.AddDays(-3)},
                
            };
            sut.DataDodania = DateTime.Now.Date;

            sut.ZmienDateCommand.Execute(null);

            foreach (var cena in sut.ListaZmienionychCen)
            {
                Assert.AreEqual(DateTime.Now.Date,cena.DataDodania);
            }
        }

        #endregion

        #endregion

        #region IsChanged
        [Test]
        public void IsChanged_GdyListaNowychCenPusta_True()
        {
            Assert.IsFalse(sut.IsChanged);
        }


        #endregion

        #region SaveCommand
        #region CanExecute
        [Test]
        public void SaveCommandCanExecute_GdyListaNull_False()
        {
            var actual = sut.SaveCommand.CanExecute(null);

            Assert.IsFalse(actual); 
        }

        [Test]
        public void SaveCommandCanExecute_GdyListaPusta_False()
        {
            sut.ListaZmienionychCen = new List<tblProdukcjaRozliczenie_CenyTransferowe>();

            var actual = sut.SaveCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }

        [Test]
        public void SaveCommandCanExecute_GdyIsChangedIsFalse_False()
        {
            sut.ListaZmienionychCen = new List<tblProdukcjaRozliczenie_CenyTransferowe>()
            { new tblProdukcjaRozliczenie_CenyTransferowe()};
            sut.ListaZmienionychCenOrg = sut.ListaZmienionychCen.DeepClone();

            var actual = sut.SaveCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }

        #endregion

        #region Execute
        [Test]
        public void SaveCommandExecute_GdyWsyzstkieRekordyMajaIdNieDodajeDoBazy()
        {
            sut.ListaZmienionychCen = new List<tblProdukcjaRozliczenie_CenyTransferowe>
            {
                new tblProdukcjaRozliczenie_CenyTransferowe(){IDProdukcjaRozliczenie_CenyTransferowe=1,CenaTransferowa=1},
                new tblProdukcjaRozliczenie_CenyTransferowe(){IDProdukcjaRozliczenie_CenyTransferowe=1,CenaTransferowa=1},
            };

            sut.SaveCommand.Execute(null);

            tblProdukcjaRozliczenie_CenyTransferowe.Verify(v => v.AddRange(It.IsAny<IEnumerable<tblProdukcjaRozliczenie_CenyTransferowe>>()),Times.Never);
            UnitOfWork.Verify(v => v.SaveAsync());
        }
        [Test]
        public void SaveCommandExecute_ZapisujeListyWBazie()
        {
            sut.ListaZmienionychCen = new List<tblProdukcjaRozliczenie_CenyTransferowe>
            {
                new tblProdukcjaRozliczenie_CenyTransferowe(){IDProdukcjaRozliczenie_CenyTransferowe=0,CenaTransferowa=1},
                new tblProdukcjaRozliczenie_CenyTransferowe(){IDProdukcjaRozliczenie_CenyTransferowe=1,CenaTransferowa=1},
            };

            sut.SaveCommand.Execute(null);

            tblProdukcjaRozliczenie_CenyTransferowe.Verify(v => v.AddRange(It.IsAny<IEnumerable<tblProdukcjaRozliczenie_CenyTransferowe>>()));
            UnitOfWork.Verify(v => v.SaveAsync());
        }

        [Test]
        public void SaveCommandExecute_PoZapisie_InformujeUzytkownika()
        {
            sut.ListaZmienionychCen = new List<tblProdukcjaRozliczenie_CenyTransferowe>
            {
                new tblProdukcjaRozliczenie_CenyTransferowe(){IDProdukcjaRozliczenie_CenyTransferowe=0,CenaTransferowa=1},
                new tblProdukcjaRozliczenie_CenyTransferowe(){IDProdukcjaRozliczenie_CenyTransferowe=1,CenaTransferowa=1},
            };

            sut.SaveCommand.Execute(null);

            DialogService.Verify(v => v.ShowInfo_BtnOK(It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        public void SaveCommandExecute_PoZapisie_WysylaMessage()
        {
            sut.ListaZmienionychCen = new List<tblProdukcjaRozliczenie_CenyTransferowe>
            {
                new tblProdukcjaRozliczenie_CenyTransferowe(){IDProdukcjaRozliczenie_CenyTransferowe=0,CenaTransferowa=1},
                new tblProdukcjaRozliczenie_CenyTransferowe(){IDProdukcjaRozliczenie_CenyTransferowe=1,CenaTransferowa=1},
            };

            sut.SaveCommand.Execute(null);

            Messenger.Verify(v => v.Send(It.IsAny<ZmianaCenTrasferowychMessage>()));
        }

        [Test]
        public void SaveCommandExecute_PoZapisie_ZamykaOkno()
        {
            sut.ListaZmienionychCen = new List<tblProdukcjaRozliczenie_CenyTransferowe>
            {
                new tblProdukcjaRozliczenie_CenyTransferowe(){IDProdukcjaRozliczenie_CenyTransferowe=0,CenaTransferowa=1},
                new tblProdukcjaRozliczenie_CenyTransferowe(){IDProdukcjaRozliczenie_CenyTransferowe=1,CenaTransferowa=1},
            };

            sut.SaveCommand.Execute(null);

            ViewService.Verify(v => v.Close(sut.GetType().Name));
        }
        #endregion

        #endregion

    }
}
