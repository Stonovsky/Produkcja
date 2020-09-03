using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Helpers;
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
    public class GPRuchTowar_PW_RolkaBazowa_HelperTests
    {

        private Mock<IUnitOfWork> unitOfWork;
        private Mock<ITblProdukcjaRuchTowarRepository> tblProdukcjaRuchTowar;
        private Mock<ITblProdukcjaRuchNaglowekRepository> tblProdukcjaRuchNaglowek;
        private Mock<GPRuchTowar_RolkaHelper> sutMock;
        private GPRuchTowar_RolkaHelper sut;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = new Mock<IUnitOfWork>();

            tblProdukcjaRuchTowar = new Mock<ITblProdukcjaRuchTowarRepository>();
            unitOfWork.Setup(s => s.tblProdukcjaRuchTowar).Returns(tblProdukcjaRuchTowar.Object);

            tblProdukcjaRuchNaglowek = new Mock<ITblProdukcjaRuchNaglowekRepository>();
            unitOfWork.Setup(s => s.tblProdukcjaRuchNaglowek).Returns(tblProdukcjaRuchNaglowek.Object);
            sutMock = new Mock<GPRuchTowar_RolkaHelper>(unitOfWork.Object);

            sut = new GPRuchTowar_RolkaHelper(unitOfWork.Object);
        }

        #region PobierzIDRolkiBazowej

        /// <summary>
        /// Gdy gniazdo dla RW oraz PW jest takie samo sprawdz rolke bazowa z RW i przypisz na PW
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GdyRwMaPrzypisanaRolkeBazowa_GniazdoRWiPW_TakieSamo_PrzypiszTaRolkeBazowaNaPW()
        {
            var rolkaRW = new tblProdukcjaRuchTowar
            {
                IDProdukcjaRuchTowar = 1,
                IDRolkaBazowa = 2,
                tblProdukcjaRuchNaglowek = new tblProdukcjaRuchNaglowek
                {
                    tblProdukcjaGniazdoProdukcyjne = new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji }
                }
            };

            var idRolkaBazowa = await sut.PobierzIDRolkiBazowejAsync(rolkaRW, GniazdaProdukcyjneEnum.LiniaDoKonfekcji);

            Assert.AreEqual(2, idRolkaBazowa);
        }

        /// <summary>
        /// Gdy Pierwszy raz konfekcjonujemy czyli gniazdo na RW to KALANDER - przypisuje IdZRW na PW!
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GdyRwMaPrzypisanaRolkeBazowa_RwGniazdo_Kalander_PwGniazdo_Konfekcja_PrzypiszIdRwJakoIdRolkiBazowejNaPW()
        {
            var rolkaRW = new tblProdukcjaRuchTowar
            {
                IDProdukcjaRuchTowar = 1,
                IDRolkaBazowa = 2,
                tblProdukcjaRuchNaglowek = new tblProdukcjaRuchNaglowek
                {
                    tblProdukcjaGniazdoProdukcyjne = new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKalandowania }
                }
            };

            var idRolkaBazowa = await sut.PobierzIDRolkiBazowejAsync(rolkaRW, GniazdaProdukcyjneEnum.LiniaDoKonfekcji);

            Assert.AreEqual(1, idRolkaBazowa);
        }
        /// <summary>
        /// Sytuacja analigoczna jak powyzej z tym ze pomiedzy innymi gniazdami
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GdyRwMaPrzypisanaRolkeBazowa_RwGniazdo_LiniaWloknin_PwGniazdo_Kalander_PrzypiszIdRwJakoIdRolkiBazowejNaPW()
        {
            var rolkaRW = new tblProdukcjaRuchTowar
            {
                IDProdukcjaRuchTowar = 1,
                IDRolkaBazowa = 2,
                tblProdukcjaRuchNaglowek = new tblProdukcjaRuchNaglowek
                {
                    tblProdukcjaGniazdoProdukcyjne = new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaWloknin }
                }
            };

            var idRolkaBazowa = await sut.PobierzIDRolkiBazowejAsync(rolkaRW, GniazdaProdukcyjneEnum.LiniaDoKalandowania);

            Assert.AreEqual(1, idRolkaBazowa);
        }



        /// <summary>
        /// Gdy Gniazdo na RW to KONFEKCJA a gniazdo PW KALANDER (rzadka sytuacja gdy na kalander pojdzie rolka juz skonfekcjonowana) 
        /// to rolka bazowa z konfekcji musi byc przypisana na kalander
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GdyRwMaPrzypisanaRolkeBazowa_RwGniazdo_Konfekcja_PwGniazdo_Kalander_PrzypiszIDRolkiBazowejNaPW()
        {
            var rolkaRW = new tblProdukcjaRuchTowar
            {
                IDProdukcjaRuchTowar = 1,
                IDRolkaBazowa = 2,
                tblProdukcjaRuchNaglowek = new tblProdukcjaRuchNaglowek
                {
                    tblProdukcjaGniazdoProdukcyjne = new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji }
                }
            };

            var idRolkaBazowa = await sut.PobierzIDRolkiBazowejAsync(rolkaRW, GniazdaProdukcyjneEnum.LiniaDoKalandowania);

            Assert.AreEqual(2, idRolkaBazowa);
        }


        /// <summary>
        /// Gdy z jakiegos powodu nie zostanie przeslane RW wraz z child tabelami - zaciaga gniazdo z bazy
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GdyPrzeslanaRolkaRWNieMaPowiazanychTabel_ZaciagnijZBazyDanych()
        {
            var rolkaRW = new tblProdukcjaRuchTowar
            {
                IDProdukcjaRuchTowar = 1,
                IDRolkaBazowa = 2,
            };

            var idRolkaBazowa = await sut.PobierzIDRolkiBazowejAsync(rolkaRW, GniazdaProdukcyjneEnum.LiniaDoKalandowania);

            tblProdukcjaRuchNaglowek.Verify(v => v.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblProdukcjaRuchNaglowek, bool>>>()));
        }
        #endregion

        #region PobierzKolejnyNrRolkiAsync
        [Test]
        public async Task PobierzKolejnyNrRolkiAsync_GdyPrzeslanoListeTowarow_TowaryNieDodaneDoBazy_ZwrocNrUwzgledniajacyListe()
        {
            var gniazdoProdukcyjne = new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = 1 };
            var listaTowarow = new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=0},
            };
            tblProdukcjaRuchTowar.Setup(s => s.GetNewNumberAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>(),
                                                                 It.IsAny<Func<tblProdukcjaRuchTowar, int>>()))
                                 .ReturnsAsync(4);

            var nrRolki = await sut.PobierzKolejnyNrRolkiAsync(gniazdoProdukcyjne, listaTowarow);

            Assert.That(nrRolki, Is.EqualTo(5));
        }

        [Test]
        public async Task PobierzKolejnyNrRolkiAsync_GdyPrzeslanoListeTowarow_CzescTowarowDodaneDoBazy_ZwrocNr_UwzglListeBezTowarowDodanychDoBazy()
        {
            var gniazdoProdukcyjne = new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = 1 };
            var listaTowarow = new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=0},
            };
            tblProdukcjaRuchTowar.Setup(s => s.GetNewNumberAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>(),
                                                                 It.IsAny<Func<tblProdukcjaRuchTowar, int>>()))
                                 .ReturnsAsync(2);

            var nrRolki = await sut.PobierzKolejnyNrRolkiAsync(gniazdoProdukcyjne, listaTowarow);

            Assert.That(nrRolki, Is.EqualTo(3));
        }
        [Test]
        public async Task PobierzKolejnyNrRolkiAsync_GdyPrzeslanaListaTowarow_Pusta_ZwrocNrUwzglTylkoBaze()
        {
            var gniazdoProdukcyjne = new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = 1 };
            var listaTowarow = new List<tblProdukcjaRuchTowar>
            {
            };
            tblProdukcjaRuchTowar.Setup(s => s.GetNewNumberAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>(),
                                                                 It.IsAny<Func<tblProdukcjaRuchTowar, int>>()))
                                 .ReturnsAsync(1);

            var nrRolki = await sut.PobierzKolejnyNrRolkiAsync(gniazdoProdukcyjne, listaTowarow);

            Assert.That(nrRolki, Is.EqualTo(1));
        }

        #endregion

        #region PobierzKolejnyPelnyNrRolki
        [Test]
        public async Task PobierzKolejnyPelnyNrRolki_GdyGniazdoNull_ZwrocNull()
        {
            var kolejnyNrRolki = await sut.PobierzKolejnyPelnyNrRolkiAsync(null, null, null);

            Assert.IsNull(kolejnyNrRolki);
        }



        [Test]
        public async Task PobierzKolejnyPelnyNrRolki_GdyPrzeslanoGniazdoWloknin_GenerujNr_SameCyfry()
        {
            tblProdukcjaRuchTowar.Setup(s => s.GetNewNumberAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>(),
                                                                 It.IsAny<Func<tblProdukcjaRuchTowar, int>>()))
                                 .ReturnsAsync(1);
            var gniazdoWloknin = new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaWloknin };

            var kolejnyNrRolki = await sut.PobierzKolejnyPelnyNrRolkiAsync(gniazdoWloknin, null, null);

            Assert.AreEqual("1", kolejnyNrRolki);
        }

        private tblProdukcjaRuchTowar IdRoliBazowej_1()
        {
            return new tblProdukcjaRuchTowar
            {
                IDProdukcjaRuchTowar = 1,
                IDRolkaBazowa = 2,
                tblProdukcjaRuchNaglowek = new tblProdukcjaRuchNaglowek
                {
                    tblProdukcjaGniazdoProdukcyjne = new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKalandowania }
                }
            };
        }

        [Test]
        public async Task PobierzKolejnyPelnyNrRolki_GdyPrzeslanoGniazdoKalandra_GenerujNrWlokninPlusKalandra()
        {
            tblProdukcjaRuchTowar.Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                                 .ReturnsAsync(new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1, NrRolki = 1, NrRolkiPelny = "1" });
            tblProdukcjaRuchTowar.Setup(s => s.GetNewNumberAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>(),
                                                                 It.IsAny<Func<tblProdukcjaRuchTowar, int>>())).ReturnsAsync(1);
            var gniazdoProdukcyjne = new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKalandowania };
            var ruchTowar = IdRoliBazowej_1();

            var kolejnyNrRolki = await sut.PobierzKolejnyPelnyNrRolkiAsync(gniazdoProdukcyjne, ruchTowar, null);

            Assert.AreEqual("1_K1", kolejnyNrRolki);
        }

        [Test]
        public async Task PobierzKolejnyPelnyNrRolki_GdyPrzeslanoGniazdoKonfekcji_GenerujNrKalandraPlusKonfekcji()
        {
            tblProdukcjaRuchTowar.Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                                 .ReturnsAsync(new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1, NrRolki = 1, NrRolkiPelny = "1K1" });
            tblProdukcjaRuchTowar.Setup(s => s.GetNewNumberAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>(),
                                                                 It.IsAny<Func<tblProdukcjaRuchTowar, int>>())).ReturnsAsync(1);
            var gniazdoProdukcyjne = new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji };
            var ruchTowar = IdRoliBazowej_1();

            var kolejnyNrRolki = await sut.PobierzKolejnyPelnyNrRolkiAsync(gniazdoProdukcyjne, ruchTowar, null);

            Assert.AreEqual("1K1_KO1", kolejnyNrRolki);
        }

        #endregion

        #region PobierzOdpad
        [Test]
        public async Task PobierzOdpad_GdyPWZawieraRekordyZIdRolkiBazowejTakaJakRolkaRW_ZwracaOdpadJakoRozniceWWagach()
        {
            tblProdukcjaRuchTowar.Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                                    .ReturnsAsync(new db.tblProdukcjaRuchTowar { Waga_kg = 10 });
            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()))
                                 .ReturnsAsync(new List<tblProdukcjaRuchTowar>
                                 {
                                     new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=1, IDRolkaBazowa=1, Waga_kg=1},
                                     new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=2, IDRolkaBazowa=1, Waga_kg=1},
                                     new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=3, IDRolkaBazowa=1, Waga_kg=1},
                                 });

            var odpad = await sut.PobierzOdpadZRolkiRwAsync(1);

            Assert.AreEqual(7, odpad);
        }

        [Test]
        public void PobierzOdpad_GdyPWNieZawieraRekordowZIdRolkiBazowejTakaJakRolkaRW_RzucaWyjatek()
        {
            //rolka RW
            tblProdukcjaRuchTowar.Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                                    .ReturnsAsync(new db.tblProdukcjaRuchTowar { Waga_kg = 10 });
            //listaRolekPW
            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()))
                                 .ReturnsAsync(new List<tblProdukcjaRuchTowar>
                                 {
                                 });

            Assert.That(() => sut.PobierzOdpadZRolkiRwAsync(1), Throws.ArgumentException);
            //Assert.Throws(typeof(Exception), Check);
            //Assert.Throws(typeof(ArgumentException), () => sut.PobierzOdpad(1));
        }

        [Test]
        public async Task PobierzOdpad_GdyBrakRwODanymIdWBazie_ZwracaZero()
        {
            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()))
                                 .ReturnsAsync(new List<tblProdukcjaRuchTowar>
                                 {
                                     new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=1, IDRolkaBazowa=1, Waga_kg=1},
                                 });

            var odpad = await sut.PobierzOdpadZRolkiRwAsync(1);

            Assert.AreEqual(0, odpad);
        }
        #endregion

        #region PobierzKoszt_kg

        private tblProdukcjaRuchTowar RolkaBazowa_2()
        {
            return new tblProdukcjaRuchTowar
            {
                IDProdukcjaRuchTowar = 1,
                IDRolkaBazowa = 2,
                tblProdukcjaRuchNaglowek = new tblProdukcjaRuchNaglowek
                {
                    tblProdukcjaGniazdoProdukcyjne = new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji }
                }
            };
        }
        [Test]
        public async Task PobierzKosztRolki_GdyRolkaPusta_ZwrocZero()
        {
            var koszt = await sut.PobierzKosztRolki(null, GniazdaProdukcyjneEnum.LiniaDoKalandowania);

            Assert.AreEqual(0, koszt);
        }

        [Test]
        public async Task PobierzKosztRolki_GdyGniazdoPuste_ZwrocZero()
        {
            var koszt = await sut.PobierzKosztRolki(new db.tblProdukcjaRuchTowar(), 0); ;

            Assert.AreEqual(0, koszt);
        }



        [Test]
        public async Task PobierzKosztRolki_BrakRolkiWBazie_ZwracaZero()
        {
            var rolkaRW = RolkaBazowa_2();
            tblProdukcjaRuchTowar.Setup(s => s.GetByIdAsync(It.IsAny<int>()));

            var koszt = await sut.PobierzKosztRolki(rolkaRW, GniazdaProdukcyjneEnum.LiniaDoKalandowania);

            Assert.AreEqual(0, koszt);
        }


        [Test]
        public async Task PobierzKosztRolki_GdyDaneOK_PobieraRolkeZBazyWrazZChildTabelami()
        {
            var rolkaRW = RolkaBazowa_2();
            tblProdukcjaRuchTowar.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new db.tblProdukcjaRuchTowar
            {
                Cena_kg = 1,
                tblProdukcjaRuchNaglowek = new tblProdukcjaRuchNaglowek
                {
                    tblProdukcjaGniazdoProdukcyjne = new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = 1 },
                    tblProdukcjaZlcecenieProdukcyjne = new tblProdukcjaZlecenie()
                }
            }); 

            var koszt = await sut.PobierzKosztRolki(rolkaRW, GniazdaProdukcyjneEnum.LiniaDoKalandowania);

            tblProdukcjaRuchTowar.Verify(s => s.GetByIdAsync(It.IsAny<int>()));
        }

        [Test]
        public async Task PobierzKosztRolki_GdyDaneOK_GdyRolkaWBazie_ZwrocCeneKg()
        {
            var rolkaRW = RolkaBazowa_2();

            tblProdukcjaRuchTowar.Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                                 .ReturnsAsync(new db.tblProdukcjaRuchTowar { Cena_kg = 1, IDRolkaBazowa=1 });

            sutMock.Setup(s => s.PobierzIDRolkiBazowejAsync(It.IsAny<tblProdukcjaRuchTowar>(), It.IsAny<GniazdaProdukcyjneEnum>())).ReturnsAsync(1);
            sut = sutMock.Object;
            
            var koszt = await sut.PobierzKosztRolki(rolkaRW, GniazdaProdukcyjneEnum.LiniaDoKalandowania);

            Assert.AreEqual(1, koszt);
        }
        #endregion

        #region PobierzCeneZeZleceniaProdukcyjnego
        [Test]
        public void PobierzCeneZeZleceniaProdukcyjnego_GdyRolkaNiePusta_PobierzCene()
        {
            var rolka = new tblProdukcjaRuchTowar
            {
                tblProdukcjaRuchNaglowek = new tblProdukcjaRuchNaglowek
                {
                    tblProdukcjaZlcecenieProdukcyjne = new tblProdukcjaZlecenie
                    {
                        CenaMieszanki_zl = 1
                    }
                }
            };

            var cena = sut.PobierzCeneZeZleceniaProdukcyjnego(rolka);

            Assert.AreEqual(1, cena);
        }

        [Test]
        public void PobierzCeneZeZleceniaProdukcyjnego_GdyDlaRolki_tblProdukcjaRuchNaglowek_Null_Wyjatek()
        {
            var rolka = new tblProdukcjaRuchTowar();
            Assert.Throws<ArgumentException>(() => sut.PobierzCeneZeZleceniaProdukcyjnego(rolka));
        }

        [Test]
        public void PobierzCeneZeZleceniaProdukcyjnego_GdyZlecenieProdukcyjneNull_Wyjatek()
        {
            var rolka = new tblProdukcjaRuchTowar()
            {
                tblProdukcjaRuchNaglowek = new tblProdukcjaRuchNaglowek()
            };

            Assert.Throws<ArgumentException>(() => sut.PobierzCeneZeZleceniaProdukcyjnego(rolka));
        }
        #endregion

    }
}
