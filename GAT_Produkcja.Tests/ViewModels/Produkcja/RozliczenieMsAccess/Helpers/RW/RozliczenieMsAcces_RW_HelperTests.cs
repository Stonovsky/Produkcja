using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.dbMsAccess.Repositories;
using GAT_Produkcja.dbMsAccess.UnitOfWork;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.RW;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.Strategy;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.Strategy.Factory;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Modele;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.RozliczenieMsAccess.Helpers.RW
{
    public class RozliczenieMsAcces_RW_HelperTests : TestBase
    {
        private Mock<IUnitOfWorkMsAccess> unitOfWorkMsAccess;
        private Mock<ISurowiecSubiektStrategyFactory> surowiecSubiektStrategyFactory;
        private Mock<ISurowiecSubiektStrategy> surowiecStrategy;
        private Mock<IVwMagazynRuchGTXRepository> vwMagazynRuchGTX;
        private RozliczenieMsAcces_RW_Helper sut;
        private Mock<ISurowiecRepository> surowiec;
        private Mock<INormyZuzyciaRepository> normyZuzycia;

        public override void SetUp()
        {
            base.SetUp();

            unitOfWorkMsAccess = new Mock<IUnitOfWorkMsAccess>();

            surowiecSubiektStrategyFactory = new Mock<ISurowiecSubiektStrategyFactory>();
            surowiecStrategy = new Mock<ISurowiecSubiektStrategy>();
            surowiecSubiektStrategyFactory.Setup(s => s.PobierzStrategie(It.IsAny<SurowiecSubiektFactoryEnum>())).Returns(surowiecStrategy.Object);

            vwMagazynRuchGTX = new Mock<IVwMagazynRuchGTXRepository>();
            UnitOfWork.Setup(s => s.vwMagazynRuchGTX).Returns(vwMagazynRuchGTX.Object);

            surowiec = new Mock<ISurowiecRepository>();
            unitOfWorkMsAccess.Setup(s => s.Surowiec).Returns(surowiec.Object);

            normyZuzycia = new Mock<INormyZuzyciaRepository>();
            unitOfWorkMsAccess.Setup(s => s.NormyZuzycia).Returns(normyZuzycia.Object);


            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new RozliczenieMsAcces_RW_Helper(UnitOfWork.Object, unitOfWorkMsAccess.Object, surowiecSubiektStrategyFactory.Object);
        }

        [Test]
        public void LoadaAsync_LadujeDanePoczatkowe()
        {
            sut.LoadAsync();

            surowiec.Verify(v => v.GetAllAsync());
        }

        #region GenerujRozliczenieRWAsync
        [Test]
        public void GenerujRozliczenieRWAsync_GdyWybranaPozycjaKonfekcjiNull_Wyjatek()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => sut.GenerujRozliczenieRWAsync(null));
        }

        [Test]
        public void GenerujRozliczenieRWAsync_GdyWybranaPozycjaKonfekcjiPusta_Wyjatek()
        {
            Assert.ThrowsAsync<ArgumentException>(() => sut.GenerujRozliczenieRWAsync(new tblProdukcjaRozliczenie_PW()));
        }

        [Test]
        public async Task GenerujRozliczenieRWAsync_GdyMieszankaOk_BrakSurwocaWBazie_Wyjatek()
        {
            normyZuzycia.Setup(s=>s.GetAllAsync()).ReturnsAsync(new List<NormyZuzycia>
            {
                new NormyZuzycia
                {
                    ZlecenieID=1,
                    Artykul="Altex PP 90",
                    Ilosc=0.7m,
                    Surowiec="PP 4/64",
                }
            });
            vwMagazynRuchGTX.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<vwMagazynRuchGTX, bool>>>()));
            await sut.LoadAsync();

            Assert.ThrowsAsync<ArgumentNullException>(() => sut.GenerujRozliczenieRWAsync(new tblProdukcjaRozliczenie_PW { IDZlecenie = 1 }));
        }

        [Test]
        public async Task GenerujRozliczenieRWAsync_GdyMieszankaOk_SurowiecZBazyOK_GenerujRozliczenieRW()
        {
            normyZuzycia.Setup(s => s.GetAllAsync())
                        .ReturnsAsync(new List<NormyZuzycia>
                        {
                            new NormyZuzycia
                            {
                                ZlecenieID=1,
                                Artykul="Altex PP 90",
                                Ilosc=0.7m,
                                Surowiec="PP 4/64",
                            }
                        });
            surowiec.Setup(s => s.GetAllAsync())
                    .ReturnsAsync(new List<Surowiec> { new Surowiec { Id = 1, NazwaSurowca = "surowiecAccess" } });
            vwMagazynRuchGTX.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<vwMagazynRuchGTX, bool>>>()))
                            .ReturnsAsync(new List<vwMagazynRuchGTX>
                            {
                                new vwMagazynRuchGTX
                                {
                                    IdTowar = 1,
                                    TowarNazwa = "surowiecSubiekt",
                                    Cena = 1,
                                } }
                            );
            surowiecStrategy.Setup(s => s.PobierzSurowiecZSubiektDla("PP 4/64", It.IsAny<decimal>()))
                            .ReturnsAsync(new db.vwMagazynRuchGTX() { Cena = 1, TowarNazwa = "surowiecSubiekt" });
            await sut.LoadAsync();

            var rozliczenieRW = await sut.GenerujRozliczenieRWAsync(new tblProdukcjaRozliczenie_PW { IDZlecenie = 1 });

            Assert.IsNotEmpty(rozliczenieRW);
            Assert.AreEqual(1, rozliczenieRW.First().CenaJednostkowa);
            Assert.AreEqual("surowiecSubiekt", rozliczenieRW.First().NazwaTowaruSubiekt);
            Assert.AreEqual("PP 4/64", rozliczenieRW.First().NazwaSurowcaMsAccess);
        }

        #endregion

        #region DodajIlosciKgDoRW
        [Test]
        public void DodajIlosciKgDoRW_GdyListaRW_NullLubPusta_Wyjatek()
        {
            Assert.ThrowsAsync<ArgumentException>(() => sut.DodajIlosciKgIWartoscDoRW(null, new ObservableCollection<tblProdukcjaRozliczenie_PW>()));
        }
        [Test]
        public void DodajIlosciKgDoRW_GdyListaPW_NullLubPusta_Wyjatek()
        {
            Assert.ThrowsAsync<ArgumentException>(() => sut.DodajIlosciKgIWartoscDoRW(new ObservableCollection<tblProdukcjaRozliczenie_RW>(), null));
        }

        [Test]
        public async Task DodajIlosciKgDoRW_GdyArgumentyOK_PrzeliczSumeKgWgUdzialowWRw()
        {
            var listaRW = new ObservableCollection<tblProdukcjaRozliczenie_RW>
            {
                new tblProdukcjaRozliczenie_RW{IDSurowiecMsAccess=1,Udzial=0.6m},
                new tblProdukcjaRozliczenie_RW{IDSurowiecMsAccess=2,Udzial=0.4m}
            };

            var listaPW = new ObservableCollection<tblProdukcjaRozliczenie_PW>
            {
                new tblProdukcjaRozliczenie_PW{ Ilosc_kg=4, Odpad_kg=1},
                new tblProdukcjaRozliczenie_PW{ Ilosc_kg=4, Odpad_kg=1},
            };

            await sut.DodajIlosciKgIWartoscDoRW(listaRW, listaPW);

            Assert.AreEqual(6, listaRW.First().Ilosc);
            Assert.AreEqual(4, listaRW.Last().Ilosc);
        }
        #endregion

        #region PodsumowanieRW
        [Test]
        public void PodsumujRW_GdyListaOk_ZwrocPodsumowanie()
        {
            var listaRW = new List<tblProdukcjaRozliczenie_RW>
            {
                new tblProdukcjaRozliczenie_RW{Ilosc=1, CenaJednostkowa=1, Wartosc=1 },
                new tblProdukcjaRozliczenie_RW{Ilosc=2, CenaJednostkowa=2 , Wartosc=4},
            };

            RwPodsumowanieModel rozliczenie = sut.PodsumujRW(listaRW);

            Assert.AreEqual(3, rozliczenie.IloscKg);
            Assert.AreEqual(2, rozliczenie.IloscPozycji);
            Assert.AreEqual(5, rozliczenie.Koszt);
        }

        [Test]
        public void PodsumujRW_GdyListaNullLubPusta_Wyjatek()
        {
            Assert.Throws<ArgumentException>(() => sut.PodsumujRW(null));
            Assert.Throws<ArgumentException>(() => sut.PodsumujRW(new List<tblProdukcjaRozliczenie_RW>()));
        }
        #endregion


        #region GenerujCeneMieszanki

        [Test]
        public async Task GenerujCeneMieszanki_GdyWszystkoOk_GenerujeCene()
        {
            var mieszanka = new List<NormyZuzycia>
            {
                new NormyZuzycia { Surowiec="PP", Ilosc=0.7m},
                new NormyZuzycia { Surowiec="PP2", Ilosc=0.3m},
            };
            surowiecStrategy.Setup(s => s.PobierzSurowiecZSubiektDla("PP", It.IsAny<decimal>()))
                    .ReturnsAsync(new vwMagazynRuchGTX
                    {
                        Cena = 1,
                    });
            surowiecStrategy.Setup(s => s.PobierzSurowiecZSubiektDla("PP2", It.IsAny<decimal>()))
                  .ReturnsAsync(new vwMagazynRuchGTX
                  {
                      Cena = 2,
                  });


            var cena = await sut.GenerujCeneMieszanki(mieszanka);


            Assert.AreEqual(1.3m, cena);
        }



        [Test]
        public async Task GenerujCeneMieszanki_GdyPrzeslanoIDZlecenia_GenerujCene()
        {
            surowiec.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<Surowiec>
            {
                new Surowiec { Id = 1, NazwaSurowca = "PP" },
                new Surowiec { Id = 2, NazwaSurowca = "PP2" },
            });
            normyZuzycia.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<NormyZuzycia>
            {
                new NormyZuzycia { Surowiec="PP", Ilosc=0.7m, ZlecenieID=1},
                new NormyZuzycia { Surowiec="PP2", Ilosc=0.3m, ZlecenieID=1},
            });
            surowiecStrategy.Setup(s => s.PobierzSurowiecZSubiektDla("PP", It.IsAny<decimal>()))
                    .ReturnsAsync(new vwMagazynRuchGTX
                    {
                        Cena = 1,
                    });
            surowiecStrategy.Setup(s => s.PobierzSurowiecZSubiektDla("PP2", It.IsAny<decimal>()))
                  .ReturnsAsync(new vwMagazynRuchGTX
                  {
                      Cena = 2,
                  });
            await sut.LoadAsync();


            var cena = await sut.GenerujCeneMieszanki(1);


            Assert.AreEqual(1.3m, cena);
        }

        #endregion

    }
}
