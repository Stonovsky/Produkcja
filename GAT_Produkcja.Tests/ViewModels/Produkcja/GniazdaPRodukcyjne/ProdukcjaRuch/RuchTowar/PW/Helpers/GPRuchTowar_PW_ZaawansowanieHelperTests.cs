using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Helpers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Helpers
{
    [TestFixture]
    public class GPRuchTowar_PW_ZaawansowanieHelperTests
    {
        private Mock<IUnitOfWork> unitOfWork;
        private Mock<ITblProdukcjaZlecenieTowarRepository> tblProdukcjaZlecenieTowar;
        private Mock<ITblProdukcjaRuchTowarRepository> tblProdukcjaRuchTowar;
        private Mock<ITblProdukcjaZlecenieCieciaRepository> tblProdukcjaZlecenieCiecia;
        private Mock<ITblProdukcjaZlecenieRepository> tblProdukcjaZlcecenieProdukcyjne;
        private GPRuchTowar_PW_ZaawansowanieHelper sut;
        private tblProdukcjaZlecenieTowar zlecenieTowar;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = new Mock<IUnitOfWork>();

            tblProdukcjaZlecenieTowar = new Mock<ITblProdukcjaZlecenieTowarRepository>();
            unitOfWork.Setup(s => s.tblProdukcjaZlecenieTowar).Returns(tblProdukcjaZlecenieTowar.Object);

            tblProdukcjaRuchTowar = new Mock<ITblProdukcjaRuchTowarRepository>();
            unitOfWork.Setup(s => s.tblProdukcjaRuchTowar).Returns(tblProdukcjaRuchTowar.Object);

            tblProdukcjaZlecenieCiecia = new Mock<ITblProdukcjaZlecenieCieciaRepository>();
            unitOfWork.Setup(s => s.tblProdukcjaZlecenieCiecia).Returns(tblProdukcjaZlecenieCiecia.Object);

            tblProdukcjaZlcecenieProdukcyjne = new Mock<ITblProdukcjaZlecenieRepository>();
            unitOfWork.Setup(s => s.tblProdukcjaZlecenie).Returns(tblProdukcjaZlcecenieProdukcyjne.Object);


            sut = new GPRuchTowar_PW_ZaawansowanieHelper(unitOfWork.Object);
        }

        private void ListaZElementami()
        {
            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>())).ReturnsAsync(new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=1, IDProdukcjaZlecenieTowar=1, Ilosc_m2=1},
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=2, IDProdukcjaZlecenieTowar=1, Ilosc_m2=1},
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=3, IDProdukcjaZlecenieTowar=1, Ilosc_m2=1},
            });
        }


        #region PobierzZawansowanie_ProdukcjaZlecenie
        //ZelecenieProdukcyjne zaawansowanie jako stosunek sumy rolek wyprodukowanych na obu gniadach (linia wloknin + kalander) do sumy ilosci dla obu gniazd ze zlecenia
        //ZlecenieCiecia zaawansowanie tylko na podstawie rolek z gniazda konfekcji

        [Test]
        public async Task PobierzZawansowanie_ProdukcjaZlecenie_GdyWszystkieDaneOK_PobieraZaawansowanie()
        {
            tblProdukcjaZlecenieTowar.Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                                     .ReturnsAsync(new db.tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieCiecia = 1 });
            tblProdukcjaZlecenieTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieTowar, bool>>>()))
                                     .ReturnsAsync(new List<tblProdukcjaZlecenieTowar>
                                     {
                                         new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=1, IDProdukcjaZlecenieCiecia=1, Ilosc_m2=2},
                                         new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=2, IDProdukcjaZlecenieCiecia=1, Ilosc_m2=2}
                                     });

            tblProdukcjaRuchTowar.Setup(s => s.SumAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>(), It.IsAny<Func<tblProdukcjaRuchTowar, decimal>>()))
                                 .ReturnsAsync(1);

            var zaawansowanie = await sut.PobierzZawansowanie_ProdukcjaZlecenie(1);

            Assert.AreEqual(0.5m, zaawansowanie);
        }
        #endregion

        private void Zaawansowanie_0()
        {
            zlecenieTowar = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 1, Ilosc_m2 = 2, IDTowarGeowlokninaParametryGramatura = 1 };

            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()))
                                 .ReturnsAsync(new List<tblProdukcjaRuchTowar>
                                 {
                                 });

        }
        private void Zaawansowanie_0_5()
        {
            zlecenieTowar = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 1, Ilosc_m2 = 2, IDTowarGeowlokninaParametryGramatura = 1 };

            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()))
                                 .ReturnsAsync(new List<tblProdukcjaRuchTowar>
                                 {
                                    new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar = 1, IDProdukcjaZlecenieTowar=1, Ilosc_m2=1, IDRuchStatus = (int) StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW, IDGramatura=1},
                                 });


        }
        private void Zaawansowanie_1()
        {
            zlecenieTowar = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 1, Ilosc_m2 = 2, IDTowarGeowlokninaParametryGramatura = 1 };

            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()))
                                 .ReturnsAsync(new List<tblProdukcjaRuchTowar>
                                 {
                                    new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar = 1, IDProdukcjaZlecenieTowar=1, Ilosc_m2=2, IDRuchStatus = (int) StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW, IDGramatura=1},
                                 });


        }

        #region ZlecenieTowar

        #region PobierzZaawansowanie_ProdukcjaZlecenieTowar

        private void ListaBezElementow()
        {
            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()));
        }


        [Test]
        public async Task PobierzZaawansowanie_ProdukcjaZlecenieTowar_GdyZlecenieTowarNull_ZwrocZero()
        {
            var zlecenieTowar = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 2 };
            var zaawansowanie = await sut.PobierzZaawansowanie_ProdukcjaZlecenieTowar(zlecenieTowar);

            Assert.AreEqual(0, zaawansowanie);
        }

        [Test]
        public async Task PobierzZaawansowanie_ProdukcjaZlecenieTowar_GdyBrakWyprodukowanychRolekDlaDanegoZlecenia_ZwrocZero()
        {
            var zlecenieTowar = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 1 };

            tblProdukcjaZlecenieTowar.Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                                     .ReturnsAsync(new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 1, Ilosc_m2 = 1 });

            var zaawansowanie = await sut.PobierzZaawansowanie_ProdukcjaZlecenieTowar(zlecenieTowar);

            Assert.AreEqual(0, zaawansowanie);
        }

        [Test]
        public async Task PobierzZaawansowanie_ProdukcjaZlecenieTowar_GdyWyprodukowanoRolkiDlaDanegoZlecenia_PrzeliczZaawansowanie()
        {
            var zlecenieTowar = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 1, Ilosc_m2 = 2, IDTowarGeowlokninaParametryGramatura = 1 };

            tblProdukcjaZlecenieTowar.Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                                     .ReturnsAsync(new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 1, Ilosc_m2 = 2, IDTowarGeowlokninaParametryGramatura = 1 });

            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()))
                                 .ReturnsAsync(new List<tblProdukcjaRuchTowar>
                                 {
                                    new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar = 1, IDProdukcjaZlecenieTowar=1, Ilosc_m2=1, IDGramatura=1, IDRuchStatus = (int)StatusRuchuTowarowEnum.RozchodWewnetrzny_RW },
                                    new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar = 2, IDProdukcjaZlecenieTowar=1, Ilosc_m2=1, IDGramatura=1, IDRuchStatus = (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW },
                                 });

            var zaawansowanie = await sut.PobierzZaawansowanie_ProdukcjaZlecenieTowar(zlecenieTowar);

            Assert.AreEqual(0.5, zaawansowanie);
        }
        [Test]
        public async Task PobierzZaawansowanie_ProdukcjaZlecenieTowar_OkreslenieZaawansowaniaTylkoNaPodstawiePW()
        {
            var zlecenieTowar = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 1, Ilosc_m2 = 2 };

            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()))
                                 .ReturnsAsync(new List<tblProdukcjaRuchTowar>
                                 {
                                    new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar = 1, IDProdukcjaZlecenieTowar=1, Ilosc_m2=1, IDRuchStatus = (int) StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW},
                                    new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar = 2, IDProdukcjaZlecenieTowar=1, Ilosc_m2=1, IDRuchStatus = (int) StatusRuchuTowarowEnum.RozchodWewnetrzny_RW},
                                 });

            var zaawansowanie = await sut.PobierzZaawansowanie_ProdukcjaZlecenieTowar(zlecenieTowar);

            Assert.AreEqual(0.5, zaawansowanie);
        }

        [Test]
        public async Task PobierzZaawansowanie_ProdukcjaZlecenieTowar_OkreslenieZaawansowaniaNaPodstawiePWOrazGramatury()
        {
            var zlecenieTowar = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 1, Ilosc_m2 = 2, IDTowarGeowlokninaParametryGramatura = 1 };

            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()))
                                 .ReturnsAsync(new List<tblProdukcjaRuchTowar>
                                 {
                                    new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar = 1, IDProdukcjaZlecenieTowar=1, Ilosc_m2=1, IDRuchStatus = (int) StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW, IDGramatura=1},
                                    new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar = 1, IDProdukcjaZlecenieTowar=1, Ilosc_m2=1, IDRuchStatus = (int) StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW, IDGramatura=2},
                                    new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar = 1, IDProdukcjaZlecenieTowar=1, Ilosc_m2=1, IDRuchStatus = (int) StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW, IDGramatura=1},
                                    new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar = 2, IDProdukcjaZlecenieTowar=1, Ilosc_m2=1, IDRuchStatus = (int) StatusRuchuTowarowEnum.RozchodWewnetrzny_RW, IDGramatura=1},
                                 });

            var zaawansowanie = await sut.PobierzZaawansowanie_ProdukcjaZlecenieTowar(zlecenieTowar);

            Assert.AreEqual(1, zaawansowanie);
        }
        #endregion

        #region PobierzStatusZleceniaTowar
        [Test]
        public async Task PobierzStatusZleceniaTowar_GdyZaawansowanie_0_Oczekuje()
        {
            Zaawansowanie_0();

            var status = await sut.PobierzStatusZleceniaTowar(zlecenieTowar);

            Assert.AreEqual(ProdukcjaZlecenieStatusEnum.Oczekuje, status);
        }
        [Test]
        public async Task PobierzStatusZleceniaTowar_GdyZaawansowanie_0_5_WTrackie()
        {
            Zaawansowanie_0_5();

            var status = await sut.PobierzStatusZleceniaTowar(zlecenieTowar);

            Assert.AreEqual(ProdukcjaZlecenieStatusEnum.WTrakcie, status);
        }
        [Test]
        public async Task PobierzStatusZleceniaTowar_GdyZaawansowanie_1_Zakonczone()
        {
            Zaawansowanie_1();

            var status = await sut.PobierzStatusZleceniaTowar(zlecenieTowar);

            Assert.AreEqual(ProdukcjaZlecenieStatusEnum.Zakonczone, status);
        }
        #endregion

        #region AktualizujStatusZleceniaTowar
        [Test]
        public async Task AktualizujStatusZleceniaTowar_PobieraZBazyZlecenieTowar()
        {
            Zaawansowanie_0();
            tblProdukcjaZlecenieTowar.Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                         .ReturnsAsync(new db.tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieStatus = (int)ProdukcjaZlecenieStatusEnum.Oczekuje });

            await sut.AktualizujStatusZleceniaTowar(zlecenieTowar);

            tblProdukcjaZlecenieTowar.Verify(v => v.GetByIdAsync(1));
        }
        [Test]
        public async Task AktualizujStatusZleceniaTowar_GdyStatusSieZmienil_Update()
        {
            Zaawansowanie_0_5();
            tblProdukcjaZlecenieTowar.Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                         .ReturnsAsync(new db.tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieStatus = (int)ProdukcjaZlecenieStatusEnum.Oczekuje });

            await sut.AktualizujStatusZleceniaTowar(zlecenieTowar);

            unitOfWork.Verify(v => v.SaveAsync());
        }
        [Test]
        public async Task AktualizujStatusZleceniaTowar_GdyStatusSieNieZmienil_BrakUpdateu()
        {
            Zaawansowanie_0(); // status oczekuje
            tblProdukcjaZlecenieTowar.Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                                     .ReturnsAsync(new db.tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieStatus = (int)ProdukcjaZlecenieStatusEnum.Oczekuje });

            await sut.AktualizujStatusZleceniaTowar(zlecenieTowar);

            unitOfWork.Verify(v => v.SaveAsync(), Times.Never);
        }
        #endregion

        #region PobierzIloscPozostalaDlaTowaruZlecenia
        [Test]
        public async Task PobierzIloscPozostalaDlaTowaruZlecenia_GdyZlecenieTowarNull_RzucaWyjatek()
        {
            //Assert.That(() => sut.PobierzIloscPozostalaDlaTowaruZlecenia(null), Throws.TypeOf(typeof(ArgumentNullException)));
            Assert.That(() => sut.PobierzIloscPozostalaDlaTowaruZlecenia(null), Throws.Exception.TypeOf<ArgumentNullException>());
        }

        [Test]
        public async Task PobierzIloscPozostalaDlaTowaruZlecenia_GdyBrakWyprodukowanychTowarowDlaZlecenia_IlosciDoKonfekcjiZgodneZeZleceniem()
        {
            zlecenieTowar = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 1, Ilosc_m2 = 10, Ilosc_szt = 10 };
            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()));

            var ilosci = await sut.PobierzIloscPozostalaDlaTowaruZlecenia(zlecenieTowar);

            Assert.That(ilosci.pozostalaIlosc_m2, Is.EqualTo(zlecenieTowar.Ilosc_m2));
            Assert.That(ilosci.pozostalaIlosc_szt, Is.EqualTo(zlecenieTowar.Ilosc_szt));
        }


        [Test]
        public async Task PobierzIloscPozostalaDlaTowaruZlecenia_GdyZlecenieTowarNieNull_ObliczaIlosci()
        {
            zlecenieTowar = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 1, Ilosc_m2 = 10, Ilosc_szt = 10 };
            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>())).ReturnsAsync(new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=1, IDProdukcjaZlecenieTowar=1, Ilosc_m2=2, IDRuchStatus = (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW},
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=2, IDProdukcjaZlecenieTowar=1, Ilosc_m2=2, IDRuchStatus = (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW},
            });
            var ilosci = await sut.PobierzIloscPozostalaDlaTowaruZlecenia(zlecenieTowar);

            Assert.That(ilosci.pozostalaIlosc_m2, Is.EqualTo(6));
            Assert.That(ilosci.pozostalaIlosc_szt, Is.EqualTo(8));
        }
        #endregion

        #endregion

        #region Zlecenie

        #region PobierzZawansowanie_ProdukcjaZlecenie
        [Test]
        public async Task PobierzZawansowanie_ProdukcjaZlecenie_GdyZlecenieProdukcyjne_Null_PobieraTowaryDlaZleceniaCieciaZBazy()
        {
            var zlecenie = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieCiecia = 1 };

            var zaawansowanieZlecenia = await sut.PobierzZawansowanie_ProdukcjaZlecenie(zlecenie);

            tblProdukcjaZlecenieTowar.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieTowar, bool>>>()));

            //Expression<Func<tblProdukcjaZlecenieTowar, bool>> warunek = w => w.IDProdukcjaZlecenieProdukcyjne == zlecenie.IDProdukcjaZlecenieProdukcyjne;
            //tblProdukcjaZlecenieTowar.Verify(v=>v.WhereAsync(It.Is<Expression<Func<tblProdukcjaZlecenieTowar,bool>>>(c=> LambdaExpression.Equal(criteria,warunek)));

        }
        [Test]
        public async Task PobierzZawansowanie_ProdukcjaZlecenie_GdyZlecenieCiecia_Null_PobieraTowaryDlaZleceniaProdukcyjnegoZBazy()
        {
            var zlecenie = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenie = 1 };

            var zaawansowanieZlecenia = await sut.PobierzZawansowanie_ProdukcjaZlecenie(zlecenie);

            tblProdukcjaZlecenieTowar.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieTowar, bool>>>()));

        }

        [Test]
        public async Task PobierzZawansowanie_ProdukcjaZlecenie_GdyJakiekolwiekIDZlecenia_WiekszeOdZera_PobieraTowaryDlaZleceniaZBazy()
        {
            var zlecenie = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenie = 1 };

            var zaawansowanieZlecenia = await sut.PobierzZawansowanie_ProdukcjaZlecenie(zlecenie);

            tblProdukcjaZlecenieTowar.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieTowar, bool>>>()));

        }

        [Test]
        public async Task PobierzZawansowanie_ProdukcjaZlecenie_GenerujSumeIlosciDlaCalegoZlecenia()
        {
            var zlecenieTowar = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 1, IDProdukcjaZlecenie = 1 };
            tblProdukcjaZlecenieTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieTowar, bool>>>())).ReturnsAsync(new List<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=1, Ilosc_m2=5},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=2, Ilosc_m2=10}
            });

            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>())).ReturnsAsync(new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=1, IDProdukcjaZlecenieTowar=1, Ilosc_m2=1},
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=2, IDProdukcjaZlecenieTowar=1, Ilosc_m2=1},
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=3, IDProdukcjaZlecenieTowar=2, Ilosc_m2=1},
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=4, IDProdukcjaZlecenieTowar=2, Ilosc_m2=1},

            });

            var zaawansowanie = await sut.PobierzZawansowanie_ProdukcjaZlecenie(zlecenieTowar);

            Assert.AreEqual((4m / 15m), zaawansowanie);
        }

        [Test]
        public async Task PobierzZawansowanie_ProdukcjaZlecenie_GdyBrakIdZleceniaCieiaLubBrakIdZleceniaProd_ZwracaZer0()
        {
            var zlecenieTowar = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 1 };
            tblProdukcjaZlecenieTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieTowar, bool>>>())).ReturnsAsync(new List<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=1, Ilosc_m2=5},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=2, Ilosc_m2=10}
            });

            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>())).ReturnsAsync(new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=1, IDProdukcjaZlecenieTowar=1, Ilosc_m2=1},
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=2, IDProdukcjaZlecenieTowar=1, Ilosc_m2=1},
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=3, IDProdukcjaZlecenieTowar=2, Ilosc_m2=1},
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=4, IDProdukcjaZlecenieTowar=2, Ilosc_m2=1},

            });

            var zaawansowanie = await sut.PobierzZawansowanie_ProdukcjaZlecenie(zlecenieTowar);

            Assert.AreEqual(0, zaawansowanie);
        }


        #region PobierzStatusZlecenia
        private void Zaawansowanie_Zlecenie_0()
        {
            zlecenieTowar = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 1, IDProdukcjaZlecenie = 1 };
            tblProdukcjaZlecenieTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieTowar, bool>>>())).ReturnsAsync(new List<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=1, Ilosc_m2=5},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=2, Ilosc_m2=10}
            });

            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>())).ReturnsAsync(new List<tblProdukcjaRuchTowar>
            {

            });

        }
        private void Zaawansowanie_Zlecenie_0_5()
        {
            zlecenieTowar = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 1, IDProdukcjaZlecenie = 1 };
            tblProdukcjaZlecenieTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieTowar, bool>>>())).ReturnsAsync(new List<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=1, Ilosc_m2=4},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=2, Ilosc_m2=4}
            });

            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>())).ReturnsAsync(new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=1, IDProdukcjaZlecenieTowar=1, Ilosc_m2=1},
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=2, IDProdukcjaZlecenieTowar=1, Ilosc_m2=1},
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=3, IDProdukcjaZlecenieTowar=2, Ilosc_m2=1},
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=4, IDProdukcjaZlecenieTowar=2, Ilosc_m2=1},

            });

        }
        private void Zaawansowanie_Zlecenie_1()
        {
            zlecenieTowar = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 1, IDProdukcjaZlecenie = 1 };
            tblProdukcjaZlecenieTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieTowar, bool>>>())).ReturnsAsync(new List<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=1, Ilosc_m2=2},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=2, Ilosc_m2=2}
            });

            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>())).ReturnsAsync(new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=1, IDProdukcjaZlecenieTowar=1, Ilosc_m2=1},
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=2, IDProdukcjaZlecenieTowar=1, Ilosc_m2=1},
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=3, IDProdukcjaZlecenieTowar=2, Ilosc_m2=1},
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=4, IDProdukcjaZlecenieTowar=2, Ilosc_m2=1},

            });

        }

        [Test]
        public async Task PobierzStatusZlecenia_Zaawansowanie_0_Oczekuje()
        {
            Zaawansowanie_Zlecenie_0();

            var status = await sut.PobierzStatusZlecenia(zlecenieTowar);

            Assert.AreEqual(ProdukcjaZlecenieStatusEnum.Oczekuje, status);
        }

        [Test]
        public async Task PobierzStatusZlecenia_Zaawansowanie_0_5_WTrakcie()
        {
            Zaawansowanie_Zlecenie_0_5();

            var status = await sut.PobierzStatusZlecenia(zlecenieTowar);

            Assert.AreEqual(ProdukcjaZlecenieStatusEnum.WTrakcie, status);
        }

        [Test]
        public async Task PobierzStatusZlecenia_Zaawansowanie_1_Zakonczone()
        {
            Zaawansowanie_Zlecenie_1();

            var status = await sut.PobierzStatusZlecenia(zlecenieTowar);

            Assert.AreEqual(ProdukcjaZlecenieStatusEnum.Zakonczone, status);
        }
        #endregion

        #region PobierzWyprodukowanePozycjeDlaZlecenia
        [Test]
        public void PobierzWyprodukowanePozycjeDlaZlecenia_PobieraPozcjeDlaListyTowarowZeZlecenia()
        {
            var listaPozycjiWyprodukowanych = new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaZlecenieTowar=1, Ilosc_m2=1},
                new tblProdukcjaRuchTowar{IDProdukcjaZlecenieTowar=2, Ilosc_m2=1},
                new tblProdukcjaRuchTowar{IDProdukcjaZlecenieTowar=3, Ilosc_m2=1},
                new tblProdukcjaRuchTowar{IDProdukcjaZlecenieTowar=4, Ilosc_m2=1},
                new tblProdukcjaRuchTowar{IDProdukcjaZlecenieTowar=5, Ilosc_m2=1},
                new tblProdukcjaRuchTowar{IDProdukcjaZlecenieTowar=1, Ilosc_m2=1},
            };
            var listaZlecen = new List<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=1},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=2}
            };

            var listaPozycjiDlaZlecenia = listaPozycjiWyprodukowanych.Where(p => listaZlecen.Any(z => z.IDProdukcjaZlecenieTowar == p.IDProdukcjaZlecenieTowar));

            Assert.AreEqual(3, listaPozycjiDlaZlecenia.Count());

        }

        [Test]
        public void PobierzWyprodukowanePozycjeDlaZlecenia_PobieraPozcje_GdyBrak_ZwracaPustaListe()
        {
            var listaPozycjiWyprodukowanych = new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaZlecenieTowar=3, Ilosc_m2=1},
                new tblProdukcjaRuchTowar{IDProdukcjaZlecenieTowar=3, Ilosc_m2=1},
                new tblProdukcjaRuchTowar{IDProdukcjaZlecenieTowar=3, Ilosc_m2=1},
                new tblProdukcjaRuchTowar{IDProdukcjaZlecenieTowar=4, Ilosc_m2=1},
                new tblProdukcjaRuchTowar{IDProdukcjaZlecenieTowar=5, Ilosc_m2=1},
                new tblProdukcjaRuchTowar{IDProdukcjaZlecenieTowar=6, Ilosc_m2=1},
            };
            var listaZlecen = new List<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=1},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=2}
            };

            var listaPozycjiDlaZlecenia = listaPozycjiWyprodukowanych.Where(p => listaZlecen.Any(z => z.IDProdukcjaZlecenieTowar == p.IDProdukcjaZlecenieTowar));

            Assert.IsEmpty(listaPozycjiDlaZlecenia);

        }
        #endregion

        #region AktualizujStatusZlecenia
        [Test]
        public void AktualizujStatusZlecenia_GdyStatusNieZmieniaSie_NieZapisujWBazie()
        {
            Zaawansowanie_Zlecenie_0(); //-> status W Oczekuje
            tblProdukcjaZlcecenieProdukcyjne.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new db.tblProdukcjaZlecenie { IDProdukcjaZlecenieStatus = (int)ProdukcjaZlecenieStatusEnum.Oczekuje });

            sut.AktualizujStatusZlecenia(zlecenieTowar);

            unitOfWork.Verify(v => v.SaveAsync(), Times.Never);
        }

        [Test]
        public void AktualizujStatusZlecenia_GdyStatusZmieniaSie_ZapiszWBazie()
        {
            Zaawansowanie_Zlecenie_0_5(); //-> status W Trakcie
            tblProdukcjaZlcecenieProdukcyjne.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new db.tblProdukcjaZlecenie { IDProdukcjaZlecenieStatus = (int)ProdukcjaZlecenieStatusEnum.Oczekuje });

            sut.AktualizujStatusZlecenia(zlecenieTowar);

            unitOfWork.Verify(v => v.SaveAsync(), Times.Once);
        }
        #endregion
        #endregion


        #endregion

        #region RolkaRW
        #region GuardClause

        [Test]
        public void PobierzRozchodRolkiRW_NiePrzeslanoRolki_ZwracaZero()
        {
            var rozchod = sut.PobierzRozchodRolkiRw(null, null);

            Assert.That(rozchod, Is.EqualTo(0));
        }
        [Test]
        public void PobierzRozchodRolkiRW_PrzeslanoRolkeRWzIdZero_ZwracaZero()
        {
            var rozchod = sut.PobierzRozchodRolkiRw(new db.tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 0 }, null);

            Assert.That(rozchod, Is.EqualTo(0));
        }

        [Test]
        public void PobierzRozchodRolkiRW_ListaPwNull_ZwracaZero()
        {
            var rozchod = sut.PobierzRozchodRolkiRw(new db.tblProdukcjaRuchTowar(), null);

            Assert.That(rozchod, Is.EqualTo(0));
        }

        [Test]
        public void PobierzRozchodRolkiRW_ListaPwPusta_ZwracaZero()
        {
            var rozchod = sut.PobierzRozchodRolkiRw(new db.tblProdukcjaRuchTowar(), new List<tblProdukcjaRuchTowar>());

            Assert.That(rozchod, Is.EqualTo(0));
        }
        [Test]
        public void PobierzRozchodRolkiRW_RolkaRwMaIloscZero_ZwracaZero()
        {
            var rozchod = sut.PobierzRozchodRolkiRw(new tblProdukcjaRuchTowar
            {
                IDProdukcjaRuchTowar = 1,
                Ilosc_m2 = 0
            }, new List<tblProdukcjaRuchTowar>() { new db.tblProdukcjaRuchTowar() });

            Assert.That(rozchod, Is.EqualTo(0));
        }

        #endregion

        [Test]
        public void PobierzRozchodRolkiRw_()
        {
            var rolkaRw = new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1, Ilosc_m2 = 10 };

            var rolkiPw = new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=2, Ilosc_m2=1, IDRolkaBazowa=1},
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=3, Ilosc_m2=1, IDRolkaBazowa=1},
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=4, Ilosc_m2=1, IDRolkaBazowa=2},
            };

            var rozchod = sut.PobierzRozchodRolkiRw(rolkaRw, rolkiPw);

            Assert.That(rozchod, Is.EqualTo(0.2m));
        }
        #endregion
    }
}
