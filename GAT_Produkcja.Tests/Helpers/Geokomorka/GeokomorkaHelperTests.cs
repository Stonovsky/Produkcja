using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Helpers.Geokomórka;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace GAT_Produkcja.Tests.Helpers.Geokomorka
{
    [TestFixture]
    public class GeokomorkaHelperTests
    {
        private Mock<IUnitOfWork> unitOfWork;
        private Mock<ITblTowarGeokomorkaParametryTypRepository> tblTyp;
        private Mock<ITblTowarRepository> tblTowar;
        private GeokomorkaHelper geokomorka;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            tblTyp = new Mock<ITblTowarGeokomorkaParametryTypRepository>();
            tblTowar = new Mock<ITblTowarRepository>();

            unitOfWork.Setup(s => s.tblTowarGeokomorkaParametryTyp).Returns(tblTyp.Object);
            unitOfWork.Setup(s => s.tblTowar).Returns(tblTowar.Object);

            geokomorka = new GeokomorkaHelper(unitOfWork.Object);


            geokomorka.ListaZgrzewow = new List<tblTowarGeokomorkaParametryZgrzew>()
            {
                new tblTowarGeokomorkaParametryZgrzew(){IDTowarGeokomorkaParametryZgrzew=1,KodZgrzewu="001", Zgrzew=335},
                new tblTowarGeokomorkaParametryZgrzew(){IDTowarGeokomorkaParametryZgrzew=2,KodZgrzewu="002", Zgrzew=340},
                new tblTowarGeokomorkaParametryZgrzew(){IDTowarGeokomorkaParametryZgrzew=3,KodZgrzewu="003", Zgrzew=356}
            };
            geokomorka.ListaTypow = new List<tblTowarGeokomorkaParametryTyp>() {
                new tblTowarGeokomorkaParametryTyp() { IDTowarGeokomorkaParametryTyp=1, Typ="STANDARD", GruboscPasa=1.15M},
                new tblTowarGeokomorkaParametryTyp() { IDTowarGeokomorkaParametryTyp=2, Typ="COMFORT", GruboscPasa=1.35M},
                new tblTowarGeokomorkaParametryTyp() { IDTowarGeokomorkaParametryTyp=3, Typ="COMFORT plus", GruboscPasa=1.45M}
            };

            geokomorka.ListaRodzajow = new List<tblTowarGeokomorkaParametryRodzaj>()
            {
                new tblTowarGeokomorkaParametryRodzaj(){IDTowarGeokomorkaParametryRodzaj=1, Rodzaj="AT CELL"},
                new tblTowarGeokomorkaParametryRodzaj(){IDTowarGeokomorkaParametryRodzaj=2,Rodzaj="PINEMA" }
            };

            geokomorka.ListaParametrowGeometrycznych = new List<tblTowarGeokomorkaParametryGeometryczne>()
            {
                new tblTowarGeokomorkaParametryGeometryczne()
                {
                    IDTowarGeokomorkaParametryGeometryczne =1,
                    PowierzchniaKomorki_cm2=258.6M,
                    IDTowarParametryGeokomorkaZgrzew =1,
                    DlugoscStandardowaSekcji_m=3.5M,
                    SzerokoscStandardowaSekcji_m=6.651M,
                    PowierzchniaSekcji_m2=23.28M
                },
                new tblTowarGeokomorkaParametryGeometryczne()
                {
                    IDTowarGeokomorkaParametryGeometryczne =2,
                    PowierzchniaKomorki_cm2=266.8M,
                    IDTowarParametryGeokomorkaZgrzew =2,
                    DlugoscStandardowaSekcji_m=3.5M,
                    SzerokoscStandardowaSekcji_m=6.632M,
                    PowierzchniaSekcji_m2=23.21M
                },
                new tblTowarGeokomorkaParametryGeometryczne()
                {
                    IDTowarGeokomorkaParametryGeometryczne =3,
                    PowierzchniaKomorki_cm2=293.0m,
                    IDTowarParametryGeokomorkaZgrzew =3,
                    DlugoscStandardowaSekcji_m=3.5M,
                    SzerokoscStandardowaSekcji_m=6.564M,
                    PowierzchniaSekcji_m2=22.97M
                }
            };
        }

        [Test]
        public void ObliczIloscM2ZgodnaZPowierzchniaSekcji_IloscMniejszaOdPowierzchniSekcji_IloscRownaPowierzchniSekcji()
        {
            var result = geokomorka.ObliczIloscM2ZgodnaZPowierzchniaSekcji("001", 2.10M, 10.15M, 35);
            var expected = 2.10M * 10.15M * 2;

            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase("001", 2.10, 10.15, 35, 2)]
        [TestCase("001", 2100, 10150, 35, 2)] //gdy przeslano szer i dl w mm
        public void ObliczIloscSekcji_IloscMniejszaOdPowierzchniSekcji_IloscSekcjiZaorkaglonaWGore(string kodZgrzewu, decimal szerokoscSekcji_m, decimal dlugoscSekcji_m, decimal ilosc_m2, int expected)
        {
            var result = geokomorka.ObliczIloscSekcji(kodZgrzewu, szerokoscSekcji_m, dlugoscSekcji_m, ilosc_m2);

            Assert.AreEqual(expected, result);
        }
        [Test]
        [TestCase("AT CELL COMFORT PLUS 002.100", 30, 2)]
        [TestCase("AT CELL COMFORT PLUS 002.100", 15, 1)]
        [TestCase("AT CELL COMFORT PLUS 102.100", 15, 0)]     // bledny zgrzew
        [TestCase("AT CELL COMFORsT PLUS 002.100", 15, 1)]    // bledny typ - liczy dobrze
        public void ObliczIloscSekcjiZNazwy(string nazwa, decimal ilosc, int expected)
        {
            geokomorka.ListaZgrzewow = new List<tblTowarGeokomorkaParametryZgrzew>()
            {
                new tblTowarGeokomorkaParametryZgrzew(){IDTowarGeokomorkaParametryZgrzew=1,KodZgrzewu="001"},
                new tblTowarGeokomorkaParametryZgrzew(){IDTowarGeokomorkaParametryZgrzew=2,KodZgrzewu="002"}
            };
            geokomorka.ListaTypow = new List<tblTowarGeokomorkaParametryTyp>() {
                new tblTowarGeokomorkaParametryTyp() { IDTowarGeokomorkaParametryTyp=1, Typ="COMFORT"},
                new tblTowarGeokomorkaParametryTyp() { IDTowarGeokomorkaParametryTyp=2, Typ="COMFORT plus"}
            };

            geokomorka.ListaParametrowGeometrycznych = new List<tblTowarGeokomorkaParametryGeometryczne>()
            {
                new tblTowarGeokomorkaParametryGeometryczne()
                {
                    IDTowarGeokomorkaParametryGeometryczne =1,
                    PowierzchniaKomorki_cm2=258.6M,
                    IDTowarParametryGeokomorkaZgrzew =1,
                    DlugoscStandardowaSekcji_m=3.5M,
                    SzerokoscStandardowaSekcji_m=6.651M

                },
                new tblTowarGeokomorkaParametryGeometryczne()
                {
                    IDTowarGeokomorkaParametryGeometryczne =2,
                    PowierzchniaKomorki_cm2=266.8M,
                    IDTowarParametryGeokomorkaZgrzew =2,
                    DlugoscStandardowaSekcji_m=3.5M,
                    SzerokoscStandardowaSekcji_m=6.632M
                }
            };

            var result = geokomorka.ObliczIloscSekcji(nazwa, ilosc);

            Assert.AreEqual(expected, result);
        }


        [Test]
        [TestCase("AT CELL COMFORT PLUS 001.100", "COMFORT PLUS")]
        [TestCase("AT CELL COMFORT 001.100", "COMFORT")]
        [TestCase("AT CELL COMFORsT 001.100", null)]
        public void PobierzTypZNazwy_NazwaWlasciwa_ZwroconoTyp(string nazwa, string expected)
        {
            geokomorka.ListaTypow = new List<tblTowarGeokomorkaParametryTyp>() {
                new tblTowarGeokomorkaParametryTyp() { IDTowarGeokomorkaParametryTyp=1, Typ="COMFORT"},
                new tblTowarGeokomorkaParametryTyp() { IDTowarGeokomorkaParametryTyp=1, Typ="COMFORT plus"}
            };

            var result = geokomorka.PobierzTypZNazwy("AT CELL COMFORT PLUS 001.100");

            Assert.AreEqual("COMFORT PLUS", result?.Typ.ToUpper());
        }
        [Test]
        [TestCase("AT CELL COMFORT PLUS 001.100", "001")]
        [TestCase("AT CELL COMFORT PLUS 1001.100", null)]
        public void PobierzZgrzewZNazwy(string nazwa, string expected)
        {
            geokomorka.ListaZgrzewow = new List<tblTowarGeokomorkaParametryZgrzew>()
            {
                new tblTowarGeokomorkaParametryZgrzew(){IDTowarGeokomorkaParametryZgrzew=1,KodZgrzewu="001"},
                new tblTowarGeokomorkaParametryZgrzew(){IDTowarGeokomorkaParametryZgrzew=2,KodZgrzewu="002"}
            };
            var result = geokomorka.PobierzZgrzewZNazwy(nazwa);

            Assert.AreEqual(expected, result?.KodZgrzewu);
        }

        [Test]
        [TestCase("AT CELL COMFORT PLUS 001.100", "AT CELL")]
        [TestCase("PINEMA COMFORT PLUS 001.100", "PINEMA")]
        [TestCase("AT CEdLL COMFORT PLUS 001.100", null)]
        public void PobierzRodzajZNazwy(string nazwa, string expectedRodzaj)
        {
            geokomorka.ListaRodzajow = new List<tblTowarGeokomorkaParametryRodzaj>()
            {
                new tblTowarGeokomorkaParametryRodzaj(){IDTowarGeokomorkaParametryRodzaj=1,Rodzaj="AT CELL"},
                new tblTowarGeokomorkaParametryRodzaj(){IDTowarGeokomorkaParametryRodzaj=2,Rodzaj="PINEMA"}
            };

            var result = geokomorka.PobierzRodzajZNazwy(nazwa);

            Assert.AreEqual(expectedRodzaj, result?.Rodzaj);
        }

        [Test]
        [TestCase("AT CELL COMFORT 001", 0)]
        [TestCase("AT CELL COMFORT 001.", 0)]
        [TestCase("AT CELL COMFORT 001.150", 150)]
        public void PobierzWysokoscZNazwy(string nazwa, decimal expected)
        {
            var result = geokomorka.PobierzWysokoscZNazwy(nazwa);

            Assert.AreEqual(expected, result);
        }


        [Test]
        [TestCase("AT CELL STANDARD 002.100", 21.109)] //nazwa wlasciwa
        [TestCase("AT CELL COMFORT 002.050", 13.528)] //nazwa wlasciwa
        [TestCase("AT CELL COMFOaRT 002.50", 0)] // nazwa niewlasciwa
        [TestCase("AT CELL COMFORT 102.50", 0)] // nazwa niewlasciwa
        public void ObliczWage_m1(string nazwa, decimal expectedValue)
        {
            //Assert
            //Act
            var result = geokomorka.ObliczWage(nazwa, 23.21M);
            result = Math.Round(result, 3);

            //Assert
            Assert.IsTrue(result == expectedValue);
        }

        [Test]
        [TestCase(340, "COMFORT", 50, 23.21, 13.528)] //nazwa wlasciwa
        [TestCase(340, "STANDdARD", 50, 23.21, 0)] //nazwa niewlasciwa
        [TestCase(341, "STANDdARD", 50, 23.21, 0)] //nazwa niewlasciwa
        public void ObliczWage_m2(int zgrzew, string typ, int wysokosc, decimal ilosc, decimal expectedValue)
        {
            //Assert
            geokomorka.ListaTypow = new List<tblTowarGeokomorkaParametryTyp>()
            {
                new tblTowarGeokomorkaParametryTyp() { IDTowarGeokomorkaParametryTyp = 1, Typ = "COMFORT",GruboscPasa = 1.35M },
                new tblTowarGeokomorkaParametryTyp() { IDTowarGeokomorkaParametryTyp = 1, Typ = "STANDARD",GruboscPasa = 1.15M }

            };

            geokomorka.ListaZgrzewow = new List<tblTowarGeokomorkaParametryZgrzew>()
            {
                new tblTowarGeokomorkaParametryZgrzew() { IDTowarGeokomorkaParametryZgrzew = 1, Zgrzew = 340, KodZgrzewu = "002" }
            };
            geokomorka.ListaParametrowGeometrycznych = new List<tblTowarGeokomorkaParametryGeometryczne>()
            {
                            new tblTowarGeokomorkaParametryGeometryczne()
                            {
                                IDTowarGeokomorkaParametryGeometryczne =1,
                                PowierzchniaKomorki_cm2=266.8M,
                                IDTowarParametryGeokomorkaZgrzew =1
                            }
            };



            //Act
            var result = geokomorka.ObliczWage(zgrzew, typ, wysokosc, ilosc);
            result = Math.Round(result, 3);

            //Assert
            Assert.IsTrue(result == expectedValue);
        }

        [Test]
        public void GenerujNazwePelna()
        {

            string expected = String.Format($"Geokomórka AT CELL COMFORT 002.{100} ({2000}x{6000}mm) [zgrzew: {340}], Ilość [m2]: {12}, Ilość sekcji [szt.]: {1}");

            string result = geokomorka.GenerujNazwePelna("AT CELL", "COMFORT", "002", 340, 100, 2000, 6000, 12);

            StringAssert.AreEqualIgnoringCase(expected, result);

        }
        [Test]
        [TestCase("AT CELL COMFORT 002.100", 40)]
        public void GenerujNazwePelna(string nazwa, decimal ilosc_m2)
        {
            var result = geokomorka.GenerujNazwePelna(nazwa, ilosc_m2);
            string expected = String.Format($"Geokomórka AT CELL COMFORT 002.{100} ({3500}x{6632}mm) [zgrzew: {340}], Ilość [m2]: {46.42}, Ilość sekcji [szt.]: {2}");

            StringAssert.AreEqualIgnoringCase(expected, result);
        }
        [Test]
        [TestCase("AT CELL STANDARD 002.100", 6.632)] //nazwa wlasciwa
        [TestCase("AT CELL STANDARD 003.100", 6.564)] //nazwa wlasciwa
        public void PobierzStandardowaSzerokoscSekcjiZNazwy_m(string nazwa, decimal szerokoscExpected)
        {
            var result = geokomorka.PobierzStandardowaSzerokoscSekcjiZNazwy_m(nazwa);

            Assert.AreEqual(szerokoscExpected, result);
        }

        [Test]
        [TestCase("AT CELL STANDARD 002.100", 3.5)] //nazwa wlasciwa
        [TestCase("AT CELL STANDARD 003.100", 3.5)] //nazwa wlasciwa
        public void PobierzStandardowaDlugoscSekcjiZNazwy_m(string nazwa, decimal szerokoscExpected)
        {
            var result = geokomorka.PobierzStandardowaDlugoscSekcjiZNazwy_m(nazwa);

            Assert.AreEqual(szerokoscExpected, result);
        }

        [Test]
        [TestCase("001", 1)]
        [TestCase("002", 2)]
        public void PobierzParametryGeometryczneZeZgrzewu(string kodZgrzewu, int IDParametrGeometryczny)
        {
            var parametryGometryczne = geokomorka.PobierzParametryGeometryczneZeZgrzewu(kodZgrzewu);

            Assert.AreEqual(IDParametrGeometryczny, parametryGometryczne.IDTowarGeokomorkaParametryGeometryczne);
        }

        [Test]
        [TestCase(150)]
        public async Task PobierzTowarAsync_TypyZPLUSemIBez_ReturnsPojedynczyTowar(int wysokosc)
        {
            //Arrange
            var rodzaj = geokomorka.ListaRodzajow.Find(d => d.IDTowarGeokomorkaParametryRodzaj == 1);
            var typ = geokomorka.ListaTypow.Find(f => f.IDTowarGeokomorkaParametryTyp == 1);
            var zgrzew = geokomorka.ListaZgrzewow.Find(f => f.IDTowarGeokomorkaParametryZgrzew == 1);

            unitOfWork.Setup(s => s.tblTowar.WhereAsync(It.IsAny<Expression<Func<tblTowar, bool>>>()))
                        .ReturnsAsync(new List<tblTowar>()
                        {
                            //new tblTowar() { IDTowar = 1, Nazwa = "Geokomórka AT CELL Comfort 001.100" },
                            new tblTowar() { IDTowar = 1, Nazwa = "Geokomórka AT CELL Comfort 001."+wysokosc.ToString() },
                            new tblTowar() { IDTowar = 1, Nazwa = "Geokomórka AT CELL Comfort Plus 001."+wysokosc.ToString() },
                            //new tblTowar() { IDTowar = 1, Nazwa = "Geokomórka AT CELL Comfort 001.250" }
                        }
                        );

            //Act
            var towar = await geokomorka.PobierzTowarAsync(rodzaj, typ, zgrzew, wysokosc);

            //Assert
            Assert.IsNotNull(towar);
        }
        [Test]
        [TestCase(50)]
        public void PobierzTowarAsync_Wysokosci50i150_ReturnsSingleTowar(int wysokosc)
        {
            List<tblTowar> listaTowarow = new List<tblTowar>()
            {
                new tblTowar() { IDTowar = 1, Nazwa = "Geokomórka AT CELL Comfort 001.50" },
                new tblTowar() { IDTowar = 1, Nazwa = "Geokomórka AT CELL Comfort 001.150" },
            };


            var t = listaTowarow.SingleOrDefault(f => f.Nazwa.Contains("." + wysokosc.ToString()));

            Assert.IsNotNull(t);
        }

        [Test]
        [TestCase(null, 0)]
        [TestCase("Comfort 001.100",27.61)]
        public void ObliczWageSekcji_kg(string nazwa, decimal expected)
        {
            var result = geokomorka.ObliczWageSekcji_kg(nazwa);

            Assert.AreEqual(expected, result);
        }

    }
}
