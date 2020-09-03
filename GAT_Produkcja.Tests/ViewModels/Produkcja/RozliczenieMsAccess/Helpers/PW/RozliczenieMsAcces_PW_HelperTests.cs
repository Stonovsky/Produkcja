using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.dbMsAccess.Models.Adapters;
using GAT_Produkcja.dbMsAccess.Repositories;
using GAT_Produkcja.dbMsAccess.UnitOfWork;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.PW;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Modele;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.RozliczenieMsAccess.Helpers.PW
{
    public class RozliczenieMsAcces_PW_HelperTests : TestBase
    {
        private RozliczenieMsAcces_PW_Helper sut;
        private Mock<IUnitOfWorkMsAccess> unitOfWorkMsAccess;
        private Mock<IKonfekcjaRepository> konfekcja;
        private Mock<ITblProdukcjaRozliczenie_CenyTransferoweRepository> tblProdukcjaRozliczenie_CenyTransferowe;
        private Mock<ITblProdukcjaRuchTowarRepository> tblProdukcjaRuchTowar;

        public override void SetUp()
        {
            base.SetUp();

            unitOfWorkMsAccess = new Mock<IUnitOfWorkMsAccess>();

            konfekcja = new Mock<IKonfekcjaRepository>();
            unitOfWorkMsAccess.Setup(s => s.Konfekcja).Returns(konfekcja.Object);

            tblProdukcjaRozliczenie_CenyTransferowe = new Mock<ITblProdukcjaRozliczenie_CenyTransferoweRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaRozliczenie_CenyTransferowe).Returns(tblProdukcjaRozliczenie_CenyTransferowe.Object);

            tblProdukcjaRuchTowar = new Mock<ITblProdukcjaRuchTowarRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaRuchTowar).Returns(tblProdukcjaRuchTowar.Object);

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new RozliczenieMsAcces_PW_Helper(UnitOfWork.Object, unitOfWorkMsAccess.Object);
        }

        #region GenerujRozliczeniePW
        [Test]
        public void GenerujRozliczeniePWAsync_GdyListaNull_Wyjatek()
        {
            Assert.Throws<ArgumentNullException>(() => sut.GenerujRozliczeniePW((List<Konfekcja>)null, 1));
        }

        [Test]
        public void GenerujRozliczeniePWAsync_GdyListaPusta_Wyjatek()
        {
            Assert.Throws<ArgumentNullException>(() => sut.GenerujRozliczeniePW(new List<Konfekcja>(), 1));
        }



        [Test]
        public async Task GenerujRozliczeniePWAsync_GdyArgumentyOk_GenerujListe()
        {
            tblProdukcjaRozliczenie_CenyTransferowe.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<tblProdukcjaRozliczenie_CenyTransferowe>
            {
               new tblProdukcjaRozliczenie_CenyTransferowe {TowarNazwa="Altex AT PES 100", CenaHurtowa=2,CenaTransferowa=1}
            });
            var konfekcja = new List<Konfekcja> { new Konfekcja
            {
                Artykul="PES 100",
                Waga=1,
                IloscM2=1,
                Szerokosc=100,
                Dlugosc=1

            }};
            var konfakecjaAdapter = konfekcja.Select(s => new KonfekcjaAdapter(s));
            await sut.LoadAsync();

            var listaPW = sut.GenerujRozliczeniePW(konfakecjaAdapter, 1);

            Assert.IsNotEmpty(listaPW);
            Assert.AreEqual(1, listaPW.First().CenaProduktuBezNarzutow_m2);
            Assert.AreEqual(1, listaPW.First().Szerokosc_m);
            Assert.AreEqual(1, listaPW.First().Dlugosc_m);
        }
        #endregion

        #region GenerujOdpad
        [Test]
        public void GenerujOdpadDlaPW_GdyIDZleceniaJestZero_Wyjatek()
        {
            Assert.ThrowsAsync<ArgumentException>(() => sut.GenerujOdpadDlaPW(0));
        }

        [Test]
        public async Task GenerujOdpadDlaPW_GdyIDZleceniaNieJestZero_BrakRolekRozchodowanych_Wyjatek()
        {
            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()));

            Assert.ThrowsAsync<ArgumentException>(() => sut.GenerujOdpadDlaPW(1));
        }

        [Test]
        public async Task GenerujOdpadDlaPW_GdyIDZleceniaNieJestZero_GenerujOdpad()
        {
            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()))
            .ReturnsAsync(new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{tblProdukcjaZlcecenieProdukcyjne = new tblProdukcjaZlecenie{IDProdukcjaZlecenie=1, NrZlecenia=1},WagaOdpad_kg=1, TowarNazwaSubiekt="ALTEX AT PP 90 (2x50)",NrPalety=1},
                new tblProdukcjaRuchTowar{tblProdukcjaZlcecenieProdukcyjne = new tblProdukcjaZlecenie{IDProdukcjaZlecenie=1,NrZlecenia=1},WagaOdpad_kg=2},
            });

            var odpad = await sut.GenerujOdpadDlaPW(1);

            Assert.AreEqual(1, odpad.IDZlecenie);
            Assert.IsTrue(odpad.SymbolTowaruSubiekt.ToLower().Contains("tasmy"));
        }
        #endregion

        #region PobierzCeneTransferowa
        [Test]
        public void PobierzCeneTransferowa_GdyNazwaNull_Wyjatek()
        {
            Assert.Throws<ArgumentException>(() => sut.PobierzCeneTransferowa(null));
        }
        [Test]
        public void PobierzCeneTransferowa_GdyNazwaPusta_Wyjatek()
        {
            Assert.Throws<ArgumentException>(() => sut.PobierzCeneTransferowa(""));
        }

        [Test]
        public async Task PobierzCeneTransferowa_GdyNazwaNiePusta_AleBrakTowaruNaLiscieCenTransferowych_ZwracaZero()
        {
            //listaCenTransferowychGTEX
            tblProdukcjaRozliczenie_CenyTransferowe.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<tblProdukcjaRozliczenie_CenyTransferowe>
            {
                new tblProdukcjaRozliczenie_CenyTransferowe{ TowarNazwa="Altex AT PP 90", CenaTransferowa=1, CenaHurtowa=2},
                new tblProdukcjaRozliczenie_CenyTransferowe{ TowarNazwa="Altex AT PP 100", CenaTransferowa=2, CenaHurtowa=3},
            });
            await sut.LoadAsync();
            var cena = sut.PobierzCeneTransferowa("PP 125");

            Assert.AreEqual(0, cena);
        }
        [Test]
        public async Task PobierzCeneTransferowa_GdyNazwaNiePusta_IJestNaLiscieCenTransferowych_ZwracaCene()
        {
            //listaCenTransferowychGTEX
            tblProdukcjaRozliczenie_CenyTransferowe
                .Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRozliczenie_CenyTransferowe, bool>>>()))
                .ReturnsAsync(new List<tblProdukcjaRozliczenie_CenyTransferowe>
                {
                    new tblProdukcjaRozliczenie_CenyTransferowe{ TowarNazwa="Altex AT PP 90", CenaTransferowa=1, CenaHurtowa=2},
                    new tblProdukcjaRozliczenie_CenyTransferowe{ TowarNazwa="Altex AT PP 100", CenaTransferowa=2, CenaHurtowa=3},
                });
            await sut.LoadAsync();

            var cena = sut.PobierzCeneTransferowa("Altex AT PP 90 (1mx100m)");

            Assert.AreEqual(1, cena);
        }
        #endregion

        #region PobierzCeneHurtowa
        private async Task CenyHurtoweZBazy()
        {
            tblProdukcjaRozliczenie_CenyTransferowe.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRozliczenie_CenyTransferowe, bool>>>()))
                                                   .ReturnsAsync(new List<tblProdukcjaRozliczenie_CenyTransferowe>
            {
                new tblProdukcjaRozliczenie_CenyTransferowe{TowarNazwa = "ALTEX AT PP 90" , CenaHurtowa=0.91m },
                new tblProdukcjaRozliczenie_CenyTransferowe{TowarNazwa = "ALTEX AT PP 100" , CenaHurtowa=0.95m },
                new tblProdukcjaRozliczenie_CenyTransferowe{TowarNazwa = "ALTEX AT PES 100" , CenaHurtowa=0.7m },
                new tblProdukcjaRozliczenie_CenyTransferowe{TowarNazwa = "ALTEX AT PES 100" , CenaHurtowa=0.7m },
            });
            await sut.LoadAsync();

        }
        [Test]
        public async Task PobierzCeneHurtowa_GdyPodanoNazwe_ObliczaWlasciwaCene()
        {
            await CenyHurtoweZBazy();

            var cena = sut.PobierzCeneHurtowa("Geowłóknina ALTEX AT PP 100 (2mx50m)");

            Assert.AreEqual(0.95m, cena);
        }
        #endregion

        #region DodajNazwyRolekBazowych
        [Test]
        public void DodajNazwyRolekBazowychDoListy_GdyBrakRolekWBazieOPodanymNumerze_ZwrocNull()
        {
            var listaPW = new List<tblProdukcjaRozliczenie_PW>
            {
                new tblProdukcjaRozliczenie_PW { NrRolkiBazowej="1L" }
            };
            unitOfWorkMsAccess.Setup(s => s.Konfekcja.GetByNrSztuki(It.IsAny<IEnumerable<string>>())).Returns(new List<Konfekcja>());

            sut.DodajNazwyRolekBazowychDoListy(listaPW);

            Assert.IsNull(listaPW.First().NazwaRolkiBazowej);
        }

        [Test]
        public void DodajNazwyRolekBazowychDoListy_GdySaRolkiWBazieOPodanymNumerze_ZwrocNazweISymbol()
        {
            var listaPW = new List<tblProdukcjaRozliczenie_PW>
            {
                new tblProdukcjaRozliczenie_PW { NrRolkiBazowej="1L" }
            };
            unitOfWorkMsAccess.Setup(s => s.Konfekcja.GetByNrSztuki(It.IsAny<IEnumerable<string>>())).Returns(new List<Konfekcja>()
            {
                new Konfekcja {Artykul="Altex AT PP 90", Szerokosc=50, Dlugosc=100, NrSztuki="1L", Przychody="Linia"},
                new Konfekcja {Artykul="Altex AT PES 90", Szerokosc=50, Dlugosc=100, NrSztuki="2L", Przychody="Linia"},
            });

            sut.DodajNazwyRolekBazowychDoListy(listaPW);

            Assert.IsNotEmpty(listaPW.First().NazwaRolkiBazowej);
            Assert.IsNotEmpty(listaPW.First().SymbolRolkiBazowej);
        }

        [Test]
        public void DodajNazwyRolekBazowychDoListy_GdyListaJestPusta_Wyjatek()
        {
            var listaPW = new List<tblProdukcjaRozliczenie_PW>();

            Assert.Throws<ArgumentException>(() => sut.DodajNazwyRolekBazowychDoListy(listaPW));
        }
        [Test]
        public void DodajNazwyRolekBazowychDoListy_GdyListaJestNull_Wyjatek()
        {
            Assert.Throws<ArgumentException>(() => sut.DodajNazwyRolekBazowychDoListy(null));
        }

        [Test]
        public void DodajNazwyRolekBazowychDoListy_ForEach()
        {
            var listaTestowa = new List<string> { "1-m", "2-m", "3" };
            listaTestowa = listaTestowa.Select(s => s.Replace("m", "")).ToList();
            //foreach (var item in listaTestowa)
            //{
            //    var s = item.Replace("m", "");
            //}

            //listaTestowa.ToList().ForEach(e => e = e.Replace("m", ""));

            Assert.AreEqual("1-", listaTestowa.First());
            Assert.AreEqual("3", listaTestowa.Last());
        }
        #endregion

        #region Podsumowanie

        [Test]
        public void PodsumujPW_GdyListaOk_ZwrocPodsumowanie()
        {
            var listaPW = new List<tblProdukcjaRozliczenie_PW>
            {
                new tblProdukcjaRozliczenie_PW{Ilosc_kg=10, Odpad_kg=1, Ilosc=1, CenaProduktuBezNarzutow_kg=1, Wartosc=10 },
                new tblProdukcjaRozliczenie_PW{Ilosc_kg=10, Odpad_kg=1, Ilosc=1, CenaProduktuBezNarzutow_kg=2, Wartosc=20 },
            };

            PwPodsumowanieModel rozliczenie = sut.PodsumujPW(listaPW);

            Assert.AreEqual(20, rozliczenie.WagaKg);
            Assert.AreEqual(2, rozliczenie.OdpadKg);
            Assert.AreEqual(30, rozliczenie.Wartosc);
            Assert.AreEqual(2, rozliczenie.IloscM2);
        }
        [Test]
        public void PodsumujPW_GdyListaNullLubPusta_Wyjatek()
        {
            Assert.Throws<ArgumentException>(() => sut.PodsumujPW(null));
            Assert.Throws<ArgumentException>(() => sut.PodsumujPW(new List<tblProdukcjaRozliczenie_PW>()));
        }

        #region GenerujOdpadDlaPW


        [Test]
        public void GenerujOdpadDlaPW_GdyListaNullLubPusta_Wyjatek()
        {
            Assert.Throws<ArgumentException>(() => sut.GenerujOdpadDlaPW(null));
            Assert.Throws<ArgumentException>(() => sut.GenerujOdpadDlaPW(new List<tblProdukcjaRozliczenie_PW>()));
        }

        [Test]
        public void GenerujOdpadDlaPW_GdyListaNiePusta_GenerujeOdpad()
        {
            var listaPW = new List<tblProdukcjaRozliczenie_PW>
            {
                new tblProdukcjaRozliczenie_PW{ IDZlecenie=1 , NrZlecenia="001", SymbolTowaruSubiekt="ALT_PP_1/100",NazwaTowaruSubiekt="Altex AT PP 90", Ilosc=10, Odpad_kg=1,CenaProduktuBezNarzutow_kg=1, NrWz="wz1" },
                new tblProdukcjaRozliczenie_PW{ IDZlecenie=1 , NrZlecenia="001", SymbolTowaruSubiekt="ALT_PP_1/100",NazwaTowaruSubiekt="Altex AT PP 90", Ilosc=10, Odpad_kg=1,CenaProduktuBezNarzutow_kg=1, NrWz="wz1" },

            };

            var odpad = sut.GenerujOdpadDlaPW(listaPW);

            Assert.AreEqual(1, odpad.IDZlecenie);
            Assert.IsTrue(odpad.SymbolTowaruSubiekt.Contains("PP"));
            Assert.AreEqual(2, odpad.Ilosc);
            Assert.AreEqual(2, odpad.Ilosc_kg);
        }


        #endregion

        #region PodsumujPWPodzialTowar
        [Test]
        public void PodsumujPWPodzialTowa_GdyListaNullLubPusta_Wyjatek()
        {
            Assert.Throws<ArgumentException>(() => sut.PodsumujPWPodzialTowar(null));
            Assert.Throws<ArgumentException>(() => sut.PodsumujPWPodzialTowar(new List<tblProdukcjaRozliczenie_PW>()));
        }

        [Test]
        public void PodsumujPWPodzialTowar_GdyElementowWiecejNaLiscie_Grupuje()
        {
            List<tblProdukcjaRozliczenie_PW> listaPW = new List<tblProdukcjaRozliczenie_PW>
            {
                new tblProdukcjaRozliczenie_PW{SymbolTowaruSubiekt="1", NazwaTowaruSubiekt="Test1", Ilosc=1,Ilosc_kg=1,CenaSprzedazyGtex_m2=1,CenaProduktuBezNarzutow_kg=1,CenaProduktuBezNarzutow_m2=1, CenaHurtowaAGG_m2=1, Szerokosc_m=1, Dlugosc_m=1},
                new tblProdukcjaRozliczenie_PW{SymbolTowaruSubiekt="1", NazwaTowaruSubiekt="Test1", Ilosc=1,Ilosc_kg=1,CenaSprzedazyGtex_m2=1,CenaProduktuBezNarzutow_kg=1,CenaProduktuBezNarzutow_m2=1, CenaHurtowaAGG_m2=1, Szerokosc_m=1, Dlugosc_m=1},
                new tblProdukcjaRozliczenie_PW{SymbolTowaruSubiekt="1", NazwaTowaruSubiekt="Test1", Ilosc=1,Ilosc_kg=1,CenaSprzedazyGtex_m2=1,CenaProduktuBezNarzutow_kg=1,CenaProduktuBezNarzutow_m2=1, CenaHurtowaAGG_m2=1, Szerokosc_m=1, Dlugosc_m=1},
                new tblProdukcjaRozliczenie_PW{SymbolTowaruSubiekt="2", NazwaTowaruSubiekt="Test2", Ilosc=1,Ilosc_kg=1,CenaSprzedazyGtex_m2=2,CenaProduktuBezNarzutow_kg=1,CenaProduktuBezNarzutow_m2=1, CenaHurtowaAGG_m2=1, Szerokosc_m=1, Dlugosc_m=1},
                new tblProdukcjaRozliczenie_PW{SymbolTowaruSubiekt="2", NazwaTowaruSubiekt="Test2", Ilosc=1,Ilosc_kg=1,CenaSprzedazyGtex_m2=2,CenaProduktuBezNarzutow_kg=1,CenaProduktuBezNarzutow_m2=1, CenaHurtowaAGG_m2=1, Szerokosc_m=1, Dlugosc_m=1},
            };

            var listaZgrupowana = sut.PodsumujPWPodzialTowar(listaPW);

            Assert.AreEqual(2, listaZgrupowana.Last().CenaSprzedazyGtex_m2);
            Assert.AreEqual(2, listaZgrupowana.Count());
            Assert.AreEqual(3, listaZgrupowana.First().Ilosc);
            Assert.AreEqual(2, listaZgrupowana.Last().Ilosc);
            Assert.AreEqual(2, listaZgrupowana.Last().Ilosc_kg);
            Assert.AreEqual("1", listaZgrupowana.First().SymbolTowaruSubiekt);
            Assert.AreEqual("Test1", listaZgrupowana.First().NazwaTowaruSubiekt);
            Assert.AreEqual("2", listaZgrupowana.Last().SymbolTowaruSubiekt);
            Assert.AreEqual("Test2", listaZgrupowana.Last().NazwaTowaruSubiekt);
            Assert.AreEqual(1, listaZgrupowana.First().Szerokosc_m);
        }
        [Test]
        public void PodsumujPWPodzialTowar_GdyPrzychodLiniaIMagazyn_GrupujeLinieIMagazyn()
        {
            List<tblProdukcjaRozliczenie_PW> listaPW = new List<tblProdukcjaRozliczenie_PW>
            {
                new tblProdukcjaRozliczenie_PW{SymbolTowaruSubiekt="1", NazwaTowaruSubiekt="Test1", Ilosc=1,Ilosc_kg=1,CenaSprzedazyGtex_m2=1,CenaProduktuBezNarzutow_kg=1,CenaProduktuBezNarzutow_m2=1, CenaHurtowaAGG_m2=1, Szerokosc_m=1, Dlugosc_m=1,Przychod="Linia"},
                new tblProdukcjaRozliczenie_PW{SymbolTowaruSubiekt="1", NazwaTowaruSubiekt="Test1", Ilosc=1,Ilosc_kg=1,CenaSprzedazyGtex_m2=1,CenaProduktuBezNarzutow_kg=1,CenaProduktuBezNarzutow_m2=1, CenaHurtowaAGG_m2=1, Szerokosc_m=1, Dlugosc_m=1,Przychod="Linia"},
                new tblProdukcjaRozliczenie_PW{SymbolTowaruSubiekt="1", NazwaTowaruSubiekt="Test1", Ilosc=1,Ilosc_kg=1,CenaSprzedazyGtex_m2=1,CenaProduktuBezNarzutow_kg=1,CenaProduktuBezNarzutow_m2=1, CenaHurtowaAGG_m2=1, Szerokosc_m=1, Dlugosc_m=1,Przychod="Magazyn"},
                new tblProdukcjaRozliczenie_PW{SymbolTowaruSubiekt="2", NazwaTowaruSubiekt="Test2", Ilosc=2,Ilosc_kg=1,CenaSprzedazyGtex_m2=2,CenaProduktuBezNarzutow_kg=1,CenaProduktuBezNarzutow_m2=1, CenaHurtowaAGG_m2=1, Szerokosc_m=1, Dlugosc_m=1,Przychod="Magazyn"},
                new tblProdukcjaRozliczenie_PW{SymbolTowaruSubiekt="2", NazwaTowaruSubiekt="Test2", Ilosc=2,Ilosc_kg=1,CenaSprzedazyGtex_m2=2,CenaProduktuBezNarzutow_kg=1,CenaProduktuBezNarzutow_m2=1, CenaHurtowaAGG_m2=1, Szerokosc_m=1, Dlugosc_m=1,Przychod="Magazyn"},
            };

            var listaZgrupowana = sut.PodsumujPWPodzialTowar(listaPW);

            Assert.AreEqual(3, listaZgrupowana.Count());
            Assert.AreEqual(2, listaZgrupowana.First().Ilosc);
            Assert.AreEqual(4, listaZgrupowana.Last().Ilosc);
        }

        #endregion

        #endregion

    }
}
