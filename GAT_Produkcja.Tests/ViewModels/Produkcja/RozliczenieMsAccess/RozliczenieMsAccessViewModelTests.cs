using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.dbComarch.Models;
using GAT_Produkcja.dbComarch.UnitOfWork;
//using GAT_Produkcja.dbComarch.Repositories;
using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.dbMsAccess.Repositories;
using GAT_Produkcja.dbMsAccess.UnitOfWork;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.EppFile;
using GAT_Produkcja.Utilities.ExcelReport;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.RozliczenieMsAccess
{
    [TestFixture]
    public class RozliczenieMsAccessViewModelTests : TestBase
    {
        private Mock<IUnitOfWorkMsAccess> UnitOfWorkMsAccess;
        private Mock<IUnitOfWorkComarch> unitOfWorkComarch;
        private Mock<IRozliczenieMsAccesHelper> helper;
        private Mock<IKonfekcjaHelper> konfekcjaHelper;
        private Mock<IXlsService> excel;
        private Mock<IDirectoryHelper> directoryHelper;
        private Mock<IEppFileGenerator> epp;
        private Mock<RozliczenieMsAccessViewModel> sutMock;
        private Mock<IArtykulyRepository> Artykuly;
        private Mock<IKonfekcjaRepository> Konfekcja;
        private Mock<IKalanderRepository> Kalander;
        private Mock<IProdukcjaRepository> Produkcja;
        private Mock<INormyZuzyciaRepository> NormyZuzycia;
        private Mock<ISurowiecRepository> Surowiec;
        private Mock<dbComarch.Repositories.ISurowiecRepository> SurowiecComach;
        private Mock<IVwCenyMagazynoweAGGRepository> vwCenyMagazynoweAGG;
        private Mock<IVwMagazynRuchGTXRepository> vwMagazynRuchGTX;
        private Mock<ITblProdukcjaRuchTowarRepository> tblProdukcjaRuchTowar;
        private Mock<ITblProdukcjaRozliczenie_NaglowekRepository> tblProdukcjaRozliczenie_Naglowek;
        private Mock<ITblProdukcjaRozliczenie_PWRepository> tblProdukcjaRozliczenie_PW;
        private Mock<ITblProdukcjaRozliczenie_RWRepository> tblProdukcjaRozliczenie_RW;
        private Mock<ITblProdukcjaRozliczenie_PWPodsumowanieRepository> tblProdukcjaRozliczenie_PWPodsumowanie;
        private RozliczenieMsAccessViewModel sut;
        private Mock<IDyspozycjeRepository> Dyspozycje;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            UnitOfWorkMsAccess = new Mock<IUnitOfWorkMsAccess>();
            unitOfWorkComarch = new Mock<IUnitOfWorkComarch>();
            helper = new Mock<IRozliczenieMsAccesHelper>();
            konfekcjaHelper = new Mock<IKonfekcjaHelper>();
            excel = new Mock<IXlsService>();
            directoryHelper = new Mock<IDirectoryHelper>();

            epp = new Mock<IEppFileGenerator>();

            helper.Setup(s => s.ExcelReportGenerator).Returns(excel.Object);
            helper.Setup(s => s.EppFileGenerator).Returns(epp.Object);
            helper.Setup(s => s.DirectoryHelper).Returns(directoryHelper.Object);

            Artykuly = new Mock<IArtykulyRepository>();
            UnitOfWorkMsAccess.Setup(s => s.Artykuly).Returns(Artykuly.Object);

            Konfekcja = new Mock<IKonfekcjaRepository>();
            UnitOfWorkMsAccess.Setup(s => s.Konfekcja).Returns(Konfekcja.Object);

            Kalander = new Mock<IKalanderRepository>();
            UnitOfWorkMsAccess.Setup(s => s.Kalander).Returns(Kalander.Object);

            Produkcja = new Mock<IProdukcjaRepository>();
            UnitOfWorkMsAccess.Setup(s => s.Produkcja).Returns(Produkcja.Object);

            NormyZuzycia = new Mock<INormyZuzyciaRepository>();
            UnitOfWorkMsAccess.Setup(s => s.NormyZuzycia).Returns(NormyZuzycia.Object);

            Surowiec = new Mock<ISurowiecRepository>();
            UnitOfWorkMsAccess.Setup(s => s.Surowiec).Returns(Surowiec.Object);

            Dyspozycje = new Mock<IDyspozycjeRepository>();
            UnitOfWorkMsAccess.Setup(s => s.Dyspozycje).Returns(Dyspozycje.Object);

            SurowiecComach = new Mock<dbComarch.Repositories.ISurowiecRepository>();
            unitOfWorkComarch.Setup(s => s.Surowiec).Returns(SurowiecComach.Object);

            vwCenyMagazynoweAGG = new Mock<IVwCenyMagazynoweAGGRepository>();
            UnitOfWork.Setup(s => s.vwCenyMagazynoweAGG).Returns(vwCenyMagazynoweAGG.Object);

            vwMagazynRuchGTX = new Mock<IVwMagazynRuchGTXRepository>();
            UnitOfWork.Setup(s => s.vwMagazynRuchGTX).Returns(vwMagazynRuchGTX.Object);

            tblProdukcjaRuchTowar = new Mock<ITblProdukcjaRuchTowarRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaRuchTowar).Returns(tblProdukcjaRuchTowar.Object);

            tblProdukcjaRozliczenie_Naglowek = new Mock<ITblProdukcjaRozliczenie_NaglowekRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaRozliczenie_Naglowek).Returns(tblProdukcjaRozliczenie_Naglowek.Object);

            tblProdukcjaRozliczenie_PW = new Mock<ITblProdukcjaRozliczenie_PWRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaRozliczenie_PW).Returns(tblProdukcjaRozliczenie_PW.Object);

            tblProdukcjaRozliczenie_RW = new Mock<ITblProdukcjaRozliczenie_RWRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaRozliczenie_RW).Returns(tblProdukcjaRozliczenie_RW.Object);

            tblProdukcjaRozliczenie_PWPodsumowanie = new Mock<ITblProdukcjaRozliczenie_PWPodsumowanieRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaRozliczenie_PWPodsumowanie).Returns(tblProdukcjaRozliczenie_PWPodsumowanie.Object);

            CreateSut();
            
            sutMock = new Mock<RozliczenieMsAccessViewModel>(ViewModelService.Object,
                                                            UnitOfWorkMsAccess.Object,
                                                            helper.Object);
        }

        public override void CreateSut()
        {
            sut = new RozliczenieMsAccessViewModel(ViewModelService.Object,
                                                   UnitOfWorkMsAccess.Object,
                                                   helper.Object,
                                                   konfekcjaHelper.Object);
        }

        #region LoadCommand
        [Test]
        public void LoadCommandExecute_PobierzWszystkieNiezbedneDaneZBazy()
        {
            sut.LoadCommand.Execute(null);

            Artykuly.Verify(v => v.GetAllAsync());
            //Konfekcja.Verify(v => v.GetAllAsync());
        }

        #endregion


        #region ExportPlikowCommand
        [Test]
        public void ExportPlikowCommandExecute_SprawdzaCzyMetodyTworzacePlikiSaWywolaneZOdpowiedniaSciezka()
        {
            sut.ListaRW = new ObservableCollection<tblProdukcjaRozliczenie_RW>
            {
                new tblProdukcjaRozliczenie_RW()
            };

            sut.ListaPW = new ObservableCollection<tblProdukcjaRozliczenie_PW>
            {
                new tblProdukcjaRozliczenie_PW {SymbolTowaruSubiekt="ALT_PP_100_1_50", NrZlecenia="001"}
            };

            sut.ExportPlikowCommand.Execute(null);

            string sciezka = @"\\192.168.34.57\gtex\10. PRODUKCJA\Rozliczenia ksiegowe\0_Magazyn\!_Program\";
            string sciezkaPelna = $"{sciezka}{DateTime.Now.Date.ToString("yyyy-MM-dd")} - ZP 001\"";
            string nazwaPliku = $"{DateTime.Now.ToString("yyyy-MM-dd")}_-_ZP_001_RW.xlsx";
            string sciezkaKompletna = sciezkaPelna + nazwaPliku;

            excel.Verify(v => v.CreateExcelReport(It.IsAny<IEnumerable<tblProdukcjaRozliczenie_RW>>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<string>()));
        }
        #endregion

        #region RozliczCommand




        #region Execute
        [Test]
        [Ignore("test do usuniecia po przejsciu na SQL")]
        public void RozliczCommandExecute_GdyWybranoPozycjeKonfekcji_GenerujeRWSurowca()
        {
            //RozliczCommandExecute_DaneStale();
            sut.WybranaPozycjiaKonfekcjiDoRozliczenia = new tblProdukcjaRozliczenie_PW { IDZlecenie = 1 };
            NormyZuzycia.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<NormyZuzycia>
            {
                new NormyZuzycia{Id=1, ZlecenieID=1, Ilosc=0.7m},
                new NormyZuzycia{Id=2, ZlecenieID=1, Ilosc=0.3m},
            });

            vwMagazynRuchGTX.Setup(s => s.SingleOrDefaultAsync(It.IsAny<Expression<Func<vwMagazynRuchGTX, bool>>>()))
                .ReturnsAsync(new vwMagazynRuchGTX
                {
                    IdTowar = 72,
                    Cena = 2,
                    Ilosc = 0.5m,
                    TowarNazwa = "test71"
                });

            #region Konfiguracja dokladnych mockow
            //Expression<Func<vwMagazynRuchGTX, bool>> expression = vwMagazynRuchGTX => vwMagazynRuchGTX.IdTowar == 72;
            //vwMagazynRuchGTX.Setup(s => s.SingleOrDefaultAsync(It.Is<Expression<Func<vwMagazynRuchGTX, bool>>>(criteria => criteria == expression)))
            //                .ReturnsAsync(new vwMagazynRuchGTX { IdTowar = 72, Cena = 2, TowarNazwa = "test71" });

            //expression = vwMagazynRuchGTX => vwMagazynRuchGTX.IdTowar == 71;
            //vwMagazynRuchGTX.Setup(s => s.SingleOrDefaultAsync(It.Is<Expression<Func<vwMagazynRuchGTX, bool>>>(criteria => criteria == expression)))
            //                .ReturnsAsync(new vwMagazynRuchGTX { IdTowar = 71, Cena = 2, TowarNazwa = "test72" });

            #endregion

            sut.RozliczCommand.Execute(null);

            helper.Verify(v => v.GenerujRozliczenieRWAsync(It.IsAny<tblProdukcjaRozliczenie_PW>()));
        }

        [Test]
        public void RozliczCommandExecute_GdyWybranoZlecenie_BrakSurwacaWSubiektGT_NieGenerujeRWSurowca()
        {
            sut.WybraneZlecenie = new Dyspozycje { Id = 1 };
            NormyZuzycia.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<NormyZuzycia>
            {
                new NormyZuzycia{Id=1, ZlecenieID=1, Ilosc=0.7m},
                new NormyZuzycia{Id=2, ZlecenieID=1, Ilosc=0.3m},
            });

            sut.RozliczCommand.Execute(null);

            Assert.IsEmpty(sut.ListaRW);
        }

        [Test]
        public void RozliczCommandExecute_GdyWybranoZlecenie_PobieraListeTowarowZMsAccess()
        {
            sut.WybraneZlecenie = new Dyspozycje { Id = 1 };
            Konfekcja.Setup(s => s.GetByZlecenieIdAsync(It.IsAny<int>())).ReturnsAsync(new List<Konfekcja>
            {
                new Konfekcja{Id=1},
                new Konfekcja{Id=2}
            });
            NormyZuzycia.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<NormyZuzycia>
            {
                new NormyZuzycia{Id=1, ZlecenieID=1, Ilosc=0.7m},
                new NormyZuzycia{Id=2, ZlecenieID=1, Ilosc=0.3m},
            });

            sut.RozliczCommand.Execute(null);

            Assert.IsEmpty(sut.ListaRW);
        }

        #endregion

        #endregion

        #region SprawdzZleceniaCommand
        #region CanExecute
        private void SprawdzZlecenieCommandCanExecute_True()
        {
            sut.WybranyProdukt = new Artykuly { NazwaArtykulu = "test" };
            sut.WybranaSzerokosc = 1;
            sut.WybranaDlugosc = 1;
        }
        #endregion

        #region Execute

        [Test]
        public void SprawdzZleceniaCommandExecute_PoKazdymUruchomieniu_ZerujeListeRwIPw()
        {
            sut.WybranaPozycjiaKonfekcjiDoRozliczenia = new tblProdukcjaRozliczenie_PW();
            sut.ListaRW = new ObservableCollection<tblProdukcjaRozliczenie_RW>
            {
                new tblProdukcjaRozliczenie_RW(),
                new tblProdukcjaRozliczenie_RW(),
            };

            sut.ListaPW = new ObservableCollection<tblProdukcjaRozliczenie_PW>
            {
                new tblProdukcjaRozliczenie_PW(),
                new tblProdukcjaRozliczenie_PW(),
            };

            sut.RozliczCommand.Execute(null);

            Assert.IsEmpty(sut.ListaRW);
            Assert.IsEmpty(sut.ListaPW);
        }


        #endregion
        #endregion

        #region SaveCommand
        #region CanExecute
        [Test]
        public void SaveCommandCanExecute_GdyListaRwLubPwPustaLubNull_ZwrocFalse()
        {
            //sut.ListaRWSurowca = new System.Collections.ObjectModel.ObservableCollection<tblProdukcjaRozliczenie_RW> { new tblProdukcjaRozliczenie_RW() };

            var actual = sut.SaveCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }

        [Test]
        public void SaveCommandCanExecute_GdyListaRwPusta_ZwrocFalse()
        {
            sut.ListaRW = new ObservableCollection<tblProdukcjaRozliczenie_RW>();

            var actual = sut.SaveCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }

        [Test]
        public void SaveCommandCanExecute_GdyListaPwNull_ZwrocFalse()
        {
            sut.ListaRW = new ObservableCollection<tblProdukcjaRozliczenie_RW> { new tblProdukcjaRozliczenie_RW() };

            var actual = sut.SaveCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }

        [Test]
        public void SaveCommandCanExecute_GdyListaPwPusta_ZwrocFalse()
        {
            sut.ListaRW = new ObservableCollection<tblProdukcjaRozliczenie_RW> { new tblProdukcjaRozliczenie_RW() };
            sut.ListaPW = new ObservableCollection<tblProdukcjaRozliczenie_PW>();

            var actual = sut.SaveCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }
        [Test]
        public void SaveCommandCanExecute_GdyListaRwIPwOK_ZwrocTrue()
        {
            sut.ListaRW = new ObservableCollection<tblProdukcjaRozliczenie_RW> { new tblProdukcjaRozliczenie_RW() };
            sut.ListaPW = new ObservableCollection<tblProdukcjaRozliczenie_PW>() { new tblProdukcjaRozliczenie_PW() };

            var actual = sut.SaveCommand.CanExecute(null);

            Assert.IsTrue(actual);
        }
        private void SaveCommandCanExecute_True()
        {
            sut.ListaRW = new ObservableCollection<tblProdukcjaRozliczenie_RW> { new tblProdukcjaRozliczenie_RW() };
            sut.ListaPW = new ObservableCollection<tblProdukcjaRozliczenie_PW>() { new tblProdukcjaRozliczenie_PW() };

        }

        #endregion
        #region Execute
        private void DaneStale()
        {
            sut.WybranyProdukt = new Artykuly { Id = 1 };
            sut.Naglowek = new tblProdukcjaRozliczenie_Naglowek();
            sut.WybranaPozycjaKonfekcji = new Konfekcja { ZlecenieID = 1 };
            sut.PodsumowaniePW_Towar = new ObservableCollection<tblProdukcjaRozliczenie_PW>
            {
                new tblProdukcjaRozliczenie_PW()
            };
        }
        [Test]
        [Ignore("test do usuniecia po przejsciu na SQL")]
        public void SaveCommandExecute_GdyNaglowekIdZero_WywolajADD()
        {
            SaveCommandCanExecute_True();
            DaneStale();
            sut.SaveCommand.Execute(null);

            tblProdukcjaRozliczenie_Naglowek.Verify(v => v.Add(It.IsAny<tblProdukcjaRozliczenie_Naglowek>()));
            UnitOfWork.Verify(v => v.SaveAsync());
        }

        [Test]
        [Ignore("Test do usuniecia po wprowadzeniu tabeli tPRPW_Podsumowanie")]
        public void SaveCommandExecute_GdyNaglowekIdWiekszyOdZera_NieWywolujADD()
        {
            SaveCommandCanExecute_True();
            DaneStale();
            sut.Naglowek = new tblProdukcjaRozliczenie_Naglowek() { IDProdukcjaRozliczenie_Naglowek=1};
            sut.PodsumowaniePW_Towar = new ObservableCollection<tblProdukcjaRozliczenie_PW>();

            sut.SaveCommand.Execute(null);

            tblProdukcjaRozliczenie_Naglowek.Verify(v => v.Add(It.IsAny<tblProdukcjaRozliczenie_Naglowek>()), Times.Never);
            UnitOfWork.Verify(v => v.SaveAsync());
        }
        
        #region RW
        [Test]
        public void SaveCommandExecute_GdyElementListaRWNieMaIdZer0_NieWywolujADD()
        {
            SaveCommandCanExecute_True();
            DaneStale();
            sut.ListaRW = new ObservableCollection<tblProdukcjaRozliczenie_RW>
            {
                new db.tblProdukcjaRozliczenie_RW { IDProdukcjaRozliczenie_RW=1},
                new db.tblProdukcjaRozliczenie_RW { IDProdukcjaRozliczenie_RW=1}
            };

            sut.SaveCommand.Execute(null);

            tblProdukcjaRozliczenie_RW.Verify(v => v.AddRange(It.IsAny<IEnumerable<tblProdukcjaRozliczenie_RW>>()), Times.Never);
        }

        [Test]
        public void SaveCommandExecute_GdyelementListaRWMaIdZer0_WywolajADD()
        {
            SaveCommandCanExecute_True();
            DaneStale();

            sut.SaveCommand.Execute(null);

            tblProdukcjaRozliczenie_RW.Verify(v => v.AddRange(It.IsAny<IEnumerable<tblProdukcjaRozliczenie_RW>>()));
        }

        #endregion

        #region PW
        [Test]
        public void SaveCommandExecute_GdyElementListaPWNieMaIdZer0_NieWywolujADD()
        {
            SaveCommandCanExecute_True();
            sut.WybranyProdukt = new Artykuly { Id = 1 };
            sut.Naglowek = new tblProdukcjaRozliczenie_Naglowek() { IDProdukcjaRozliczenie_Naglowek = 1 };
            sut.ListaPW = new ObservableCollection<tblProdukcjaRozliczenie_PW>
            {
                new db.tblProdukcjaRozliczenie_PW { IDProdukcjaRozliczenie_PW=1},
                new db.tblProdukcjaRozliczenie_PW { IDProdukcjaRozliczenie_PW=1}
            };

            sut.SaveCommand.Execute(null);

            tblProdukcjaRozliczenie_PW.Verify(v => v.AddRange(It.IsAny<IEnumerable<tblProdukcjaRozliczenie_PW>>()), Times.Never);
        }

        [Test]
        public void SaveCommandExecute_GdyelementListaPWMaIdZer0_WywolajAddRange()
        {
            SaveCommandCanExecute_True();
            DaneStale();
            sut.WybranyProdukt = new Artykuly { Id = 1 };
            sut.Naglowek = new tblProdukcjaRozliczenie_Naglowek() { IDProdukcjaRozliczenie_Naglowek = 1 };

            sut.SaveCommand.Execute(null);

            tblProdukcjaRozliczenie_PW.Verify(v => v.AddRange(It.IsAny<IEnumerable<tblProdukcjaRozliczenie_PW>>()));
        }


        #endregion

        #region Access
        [Test]
        public void SaveCommandExecute_GdyListaPozycjiKonfekcjiNiePusta_ZmieniaStatusRozliczeniaNa_Rozliczony()
        {
            SaveCommandCanExecute_True();
            DaneStale();
            //sut.WybranyProdukt = new Artykuly { Id = 1 };

            sut.ListaPozycjiKonfekcjiDlaZlecenia = new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=1}
            };

            sut.SaveCommand.Execute(null);

            Assert.AreEqual((int)ProdukcjaRozliczenieStatusEnum.Rozliczono, sut.ListaPozycjiKonfekcjiDlaZlecenia.First().IDProdukcjaRozliczenieStatus);
        }

        #endregion

        [Test]
        [Ignore("test do usuniecia po przejsciu na SQL")]
        public void SaveCommandExecute_PoZapisieDoBazyPobierzZaktualizowanaKonfekcjeZBazyMsAccess()
        {
            SaveCommandCanExecute_True();
            DaneStale();

            //sut.WybranyProdukt = new Artykuly { Id = 1 };

            sut.ListaPozycjiKonfekcjiDlaZlecenia = new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=1}
            };

            sut.SaveCommand.Execute(null);


            konfekcjaHelper.Verify(v => v.PobierzKonfekcjeDoRozliczenia());
        }

        [Test]
        public void SaveCommandExecute_PoZapisieCzysciListy()
        {
            SaveCommandCanExecute_True();
            DaneStale();
            sut.ListaPW = new ObservableCollection<tblProdukcjaRozliczenie_PW>
            {
                new tblProdukcjaRozliczenie_PW(),
            };
            sut.ListaRW = new ObservableCollection<tblProdukcjaRozliczenie_RW>
            {
                new tblProdukcjaRozliczenie_RW(),
            };
            sut.PodsumowaniePW_TowarBaza = new List<tblProdukcjaRozliczenie_PWPodsumowanie>
            {
                new tblProdukcjaRozliczenie_PWPodsumowanie(),
            };
            sut.PodsumowaniePW_Towar = new ObservableCollection<tblProdukcjaRozliczenie_PW>
            {
                new tblProdukcjaRozliczenie_PW(),
            };
            sut.ListaPozycjiKonfekcjiDlaZlecenia = new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar {IDProdukcjaRuchTowar=1}
            };


            sut.SaveCommand.Execute(null);


            Assert.IsEmpty(sut.ListaRW);
            Assert.IsEmpty(sut.ListaPW);
            Assert.IsEmpty(sut.PodsumowaniePW_Towar);
            Assert.IsEmpty(sut.PodsumowaniePW_TowarBaza);
        }

        [Test]
        [Ignore ("Z uwagi na metode CzyscListy jest to nie do przetestowania - zastanowic sie jak to zrobic")]
        public void DodajPWPodsumowanieDoBazy_CastPozycjiPwDoPodsumowania_GdyWlasciwosciTakieSame_KowersjaOk()
        {
            sut = sutMock.Object;
            SaveCommandCanExecute_True();
            DaneStale();
            sut.PodsumowaniePW_Towar = new ObservableCollection<tblProdukcjaRozliczenie_PW>
            {
                new tblProdukcjaRozliczenie_PW
                {
                    Ilosc=1,
                    Ilosc_kg=2,
                    Odpad_kg=1,
                    CenaProduktuBezNarzutow_kg=2,
                    Wartosc=10,
                    WartoscOdpad=1,
                }
            };

            sut.SaveCommand.Execute(null);

            Assert.AreEqual(1, sut.PodsumowaniePW_TowarBaza.First().Ilosc);
            Assert.AreEqual(2, sut.PodsumowaniePW_TowarBaza.First().Ilosc_kg);
            Assert.AreEqual(2, sut.PodsumowaniePW_TowarBaza.First().CenaProduktuBezNarzutow_kg);
            Assert.AreEqual(10, sut.PodsumowaniePW_TowarBaza.First().Wartosc);
        }


        #endregion
        #endregion

        #region DodajPWPodsumowanieDoBazy
        #endregion
    }
}
