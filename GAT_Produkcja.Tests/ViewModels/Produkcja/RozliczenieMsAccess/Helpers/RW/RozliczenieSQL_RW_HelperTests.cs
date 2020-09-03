using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.RW;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.Strategy;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.Strategy.Factory;
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
    public class RozliczenieSQL_RW_HelperTests : TestBaseGeneric<RozliczenieSQL_RW_Helper>
    {
        private Mock<ISurowiecSubiektStrategyFactory> surowiecSubiektStrategyFactory;
        private Mock<ISurowiecSubiektStrategy> surowiecStrategy;
        private Mock<ITblProdukcjaRuchTowarRepository> tblProdukcjaRuchTowar;
        private Mock<ITblProdukcjaZlecenieProdukcyjne_MieszankaRepository> tblProdukcjaZlecenieProdukcyjne_Mieszanka;
        private Mock<IVwMagazynRuchGTXRepository> vwMagazynRuchGTX;
        private Mock<IVwTowarGTXRepository> vwTowarGTX;

        public override void SetUp()
        {
            base.SetUp();

            surowiecSubiektStrategyFactory = new Mock<ISurowiecSubiektStrategyFactory>();
            surowiecStrategy = new Mock<ISurowiecSubiektStrategy>();
            surowiecSubiektStrategyFactory.Setup(s => s.PobierzStrategie(It.IsAny<SurowiecSubiektFactoryEnum>())).Returns(surowiecStrategy.Object);

            tblProdukcjaRuchTowar = new Mock<ITblProdukcjaRuchTowarRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaRuchTowar).Returns(tblProdukcjaRuchTowar.Object);

            tblProdukcjaZlecenieProdukcyjne_Mieszanka = new Mock<ITblProdukcjaZlecenieProdukcyjne_MieszankaRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaZlecenieProdukcyjne_Mieszanka).Returns(tblProdukcjaZlecenieProdukcyjne_Mieszanka.Object);

            vwMagazynRuchGTX = new Mock<IVwMagazynRuchGTXRepository>();
            UnitOfWork.Setup(s => s.vwMagazynRuchGTX).Returns(vwMagazynRuchGTX.Object);

            vwTowarGTX = new Mock<IVwTowarGTXRepository>();
            UnitOfWork.Setup(s => s.vwTowarGTX).Returns(vwTowarGTX.Object);

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new RozliczenieSQL_RW_Helper(UnitOfWork.Object, surowiecSubiektStrategyFactory.Object);
        }


        #region DodajIlosciKgIWartoscDoRW
        [Test]
        public void DodajIlosciKgIWartoscDoRW_GdyListaRWPusta_Wyjatek()
        {
            Assert.ThrowsAsync<ArgumentException>(() => sut.DodajIlosciKgIWartoscDoRW(null, new ObservableCollection<tblProdukcjaRozliczenie_PW>()));

        }

        [Test]
        public void DodajIlosciKgIWartoscDoRW_GdyListaPWPusta_Wyjatek()
        {
            Assert.ThrowsAsync<ArgumentException>(() => sut.DodajIlosciKgIWartoscDoRW(new ObservableCollection<tblProdukcjaRozliczenie_RW>(), null));
        }

        [Test]
        public async Task DodajIlosciKgIWartoscDoRW_GdyListyNiepuste_DodajIloscOrazWartoscDoListyRW()
        {
            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()));

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

        #region GenerujCeneMieszanki
        [Test]
        public void GenerujCeneMieszanki_GdyIdZleceniaZero_Wyjatek()
        {
            Assert.ThrowsAsync<ArgumentException>(() => sut.GenerujCeneMieszanki(0));
        }

        [Test]
        public void GenerujCeneMieszanki_GdyIdWiekszeOdZera_BrakMieszankiDlaZlecenia_Wyjatek()
        {
            Assert.ThrowsAsync<InvalidOperationException>(() => sut.GenerujCeneMieszanki(1));
        }

        [Test]
        public async Task GenerujCeneMieszanki_GdyIdWiekszeOdZera_MieszankaIstniejeIPosiadaCeneMieszanki_ZwracaCeneMieszankiZEncjiMieszanki()
        {
            tblProdukcjaZlecenieProdukcyjne_Mieszanka.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieProdukcyjne_Mieszanka, bool>>>()))
                                                     .ReturnsAsync(new List<tblProdukcjaZlecenieProdukcyjne_Mieszanka>
                                                     {
                                                         new tblProdukcjaZlecenieProdukcyjne_Mieszanka{CenaMieszanki_kg=1}
                                                     });

            var result = await sut.GenerujCeneMieszanki(1);

            Assert.AreEqual(1, result);
        }

        [Test]
        public async Task GenerujCeneMieszanki_GdyIdWiekszeOdZera_MieszankaIstniejeINiePosiadaCenyMieszanki_WolaVwMagazynRuchGTXDoObliczeniaCeny()
        {
            tblProdukcjaZlecenieProdukcyjne_Mieszanka.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieProdukcyjne_Mieszanka, bool>>>()))
                                                     .ReturnsAsync(new List<tblProdukcjaZlecenieProdukcyjne_Mieszanka>
                                                     {
                                                         new tblProdukcjaZlecenieProdukcyjne_Mieszanka{CenaMieszanki_kg=0}
                                                     });
            vwMagazynRuchGTX.Setup(x => x.WhereAsync(It.IsAny<Expression<Func<vwMagazynRuchGTX, bool>>>()))
                             .ReturnsAsync(new List<vwMagazynRuchGTX>
                             {
                                 new vwMagazynRuchGTX{Cena=1}
                             });

            var result = await sut.GenerujCeneMieszanki(1);

            vwMagazynRuchGTX.Verify(x => x.WhereAsync(It.IsAny<Expression<Func<vwMagazynRuchGTX, bool>>>()));
        }

        [Test]
        public async Task GenerujCeneMieszanki_GdyIdWiekszeOdZera_MieszankaIstniejeINiePosiadaCenyMieszanki_GenerujeCeneNaPodstawiePobranychTowarowZVwMagazynRuchGTEX()
        {
            tblProdukcjaZlecenieProdukcyjne_Mieszanka.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieProdukcyjne_Mieszanka, bool>>>()))
                                                     .ReturnsAsync(new List<tblProdukcjaZlecenieProdukcyjne_Mieszanka>
                                                     {
                                                         new tblProdukcjaZlecenieProdukcyjne_Mieszanka{CenaMieszanki_kg=0, ZawartoscProcentowa=0.5m}
                                                     });
            vwMagazynRuchGTX.Setup(x => x.WhereAsync(It.IsAny<Expression<Func<vwMagazynRuchGTX, bool>>>()))
                             .ReturnsAsync(new List<vwMagazynRuchGTX>
                             {
                                 new vwMagazynRuchGTX{Cena=1}
                             });

            var result = await sut.GenerujCeneMieszanki(1);

            Assert.AreEqual(0.5m, result);
        }
        #endregion

        #region GenerujRozliczenieRW
        [Test]
        public void GenerujRozliczenieRWAsync_GdyWybranaPozycjaKonfekcjiNull_Wyjatek()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => sut.GenerujRozliczenieRWAsync(null));
        }

        [Test]
        public void GenerujRozliczenieRWAsync_GdyWybranaPozycjaKonfekcjiPusta_Wyjatek()
        {
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => sut.GenerujRozliczenieRWAsync(new tblProdukcjaRozliczenie_PW()));
        }

        [Test]
        public async Task GenerujRozliczenieRWAsync_MieszankaIstniejeIPosiadaCenyKgDlaSurowcow_ZwracaListeZSurowcami_DlaTychKonkrentychCen()
        {
            tblProdukcjaZlecenieProdukcyjne_Mieszanka.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieProdukcyjne_Mieszanka, bool>>>()))
                                                     .ReturnsAsync(new List<tblProdukcjaZlecenieProdukcyjne_Mieszanka>
                                                     {
                                                         new tblProdukcjaZlecenieProdukcyjne_Mieszanka{Cena_kg=1},
                                                         new tblProdukcjaZlecenieProdukcyjne_Mieszanka{Cena_kg=2 },
                                                     });

            vwMagazynRuchGTX.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<vwMagazynRuchGTX, bool>>>()))
                            .ReturnsAsync(new List<vwMagazynRuchGTX>
                            {
                                new vwMagazynRuchGTX{IdTowar=1}
                            });

            var listaRW = await sut.GenerujRozliczenieRWAsync(new tblProdukcjaRozliczenie_PW());

            Assert.IsNotEmpty(listaRW);
            Assert.AreEqual(2,listaRW.Count());
        }

        [Test]
        public async Task GenerujRozliczenieRWAsync_MieszankaIstniejeICenaKgDlaSurowcowZero_ZwracaListeZSurowcami_DlaPierwszegoKtoregoIloscPozostalaWiekszaOdZera()
        {
            tblProdukcjaZlecenieProdukcyjne_Mieszanka.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieProdukcyjne_Mieszanka, bool>>>()))
                                                     .ReturnsAsync(new List<tblProdukcjaZlecenieProdukcyjne_Mieszanka>
                                                     {
                                                         new tblProdukcjaZlecenieProdukcyjne_Mieszanka{Cena_kg=0},
                                                         new tblProdukcjaZlecenieProdukcyjne_Mieszanka{Cena_kg=0},
                                                     });

            vwMagazynRuchGTX.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<vwMagazynRuchGTX, bool>>>()))
                            .ReturnsAsync(new List<vwMagazynRuchGTX>
                            {
                                new vwMagazynRuchGTX{IdTowar=1}
                            });

            var listaRW = await sut.GenerujRozliczenieRWAsync(new tblProdukcjaRozliczenie_PW());

            Assert.IsNotEmpty(listaRW);
            Assert.AreEqual(2, listaRW.Count());
        }

        #endregion

        #region GenerujOdpad

        #endregion
    }
}
