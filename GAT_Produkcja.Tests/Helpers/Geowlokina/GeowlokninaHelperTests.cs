using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Helpers.Geowloknina;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.Helpers.Geowlokina
{
    [TestFixture]
    public class GeowlokninaHelperTests
    {
        private Mock<IUnitOfWork> unitOfWork;
        private Mock<ITblTowarGeowlokninaParametryRodzajRepository> tblRodzaj;
        private Mock<ITblTowarGeowlokninaParametryGramaturaRepository> tblGramatura;
        private GeowlokninaHelper geowloknina;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            tblRodzaj = new Mock<ITblTowarGeowlokninaParametryRodzajRepository>();
            tblGramatura = new Mock<ITblTowarGeowlokninaParametryGramaturaRepository>();

            unitOfWork.Setup(s => s.tblTowarGeowlokninaParametryGramatura).Returns(tblGramatura.Object);
            unitOfWork.Setup(s => s.tblTowarGeowlokninaParametryRodzaj).Returns(tblRodzaj.Object);

            geowloknina = new GeowlokninaHelper(unitOfWork.Object);


            var listaRodzajow = new List<tblTowarGeowlokninaParametryRodzaj>()
                                        {
                                            new tblTowarGeowlokninaParametryRodzaj(){IDTowarGeowlokninaParametryRodzaj=1, Rodzaj="ALTEX AT"},
                                            new tblTowarGeowlokninaParametryRodzaj(){IDTowarGeowlokninaParametryRodzaj=2, Rodzaj="ALTEX AT PP"},
                                            new tblTowarGeowlokninaParametryRodzaj(){IDTowarGeowlokninaParametryRodzaj=3, Rodzaj="ALTEX PES"},
                                            new tblTowarGeowlokninaParametryRodzaj(){IDTowarGeowlokninaParametryRodzaj=4, Rodzaj="ALTEX AT PES"}
                                        };

            tblRodzaj.Setup(s => s.GetAllAsync())
                     .ReturnsAsync(listaRodzajow);

            unitOfWork.Setup(s => s.tblTowarGeowlokninaParametryRodzaj.GetAllAsync())
                      .ReturnsAsync(listaRodzajow);
            Thread.Sleep(100);


            geowloknina.ListaRodzajow = new List<tblTowarGeowlokninaParametryRodzaj>()
            {
                new tblTowarGeowlokninaParametryRodzaj(){IDTowarGeowlokninaParametryRodzaj=1, Rodzaj="Geowłóknina ALTEX AT"},
                new tblTowarGeowlokninaParametryRodzaj(){IDTowarGeowlokninaParametryRodzaj=1, Rodzaj="Geowłóknina ALTEX AT PP"},
                new tblTowarGeowlokninaParametryRodzaj(){IDTowarGeowlokninaParametryRodzaj=1, Rodzaj="Geowłóknina ALTEX PES"},
                new tblTowarGeowlokninaParametryRodzaj(){IDTowarGeowlokninaParametryRodzaj=2, Rodzaj="Geowłóknina ALTEX AT PES"}
            };

        }
        [Test]
        [TestCase("Geowłóknina ALTEX AT 100", 100)]
        [TestCase("Geowłóknina ALTEX AT 100g", 100)]
        [TestCase("Geowłóknina ALTEX AT 90g", 90)]
        [TestCase("Geowłóknina ALTEX AT 100(0,5mx1,5m)", 100)]
        [TestCase("Geowłóknina ALTEX AT 100UV(0,5mx1,5m)", 100)]
        [TestCase("Geowłóknina ALTEX AT 100uV(0,5mx1,5m)", 100)]
        public void PobierzGramatureZNazwy(string nazwa, int expected)
        {
            var result = geowloknina.PobierzGramatureZNazwy(nazwa);

            Assert.AreEqual(expected, result);
        }
        [Test]
        [TestCase("Geowłóknina ALTEX AT 100uV(0,5mx1,5m)", "ALTEX AT")]
        [TestCase("Geowłóknina ALTEX AT PP 100uV(0,5mx1,5m)", "ALTEX AT PP")]
        [TestCase("Geowłóknina ALTEX AT PES 100uV(0,5mx1,5m)", "ALTEX AT PES")]
        [TestCase("Geowłóknina ALTEX PES 100uV(0,5mx1,5m)", "ALTEX PES")]
        public void PobierzRodzajZNazwy(string nazwa, string expected)
        {
            geowloknina.ListaRodzajow = new List<tblTowarGeowlokninaParametryRodzaj>()
            {
                new tblTowarGeowlokninaParametryRodzaj(){IDTowarGeowlokninaParametryRodzaj=1, Rodzaj="ALTEX AT"},
                new tblTowarGeowlokninaParametryRodzaj(){IDTowarGeowlokninaParametryRodzaj=1, Rodzaj="ALTEX AT PP"},
                new tblTowarGeowlokninaParametryRodzaj(){IDTowarGeowlokninaParametryRodzaj=1, Rodzaj="ALTEX PES"},
                new tblTowarGeowlokninaParametryRodzaj(){IDTowarGeowlokninaParametryRodzaj=2, Rodzaj="ALTEX AT PES"}
            };

            var result = geowloknina.PobierzRodzajZNazwy(nazwa);
            StringAssert.AreEqualIgnoringCase(expected, result.Rodzaj);

        }

        [Test]
        [TestCase("Geowłóknina ALTEX AT 100uV(0,5mx1,5m)",10,1)]
        [TestCase("Geowłóknina ALTEX AT 200uV(0,5mx1,5m)",10,2)]
        [TestCase("Geowłóknina ALTEX AT 300g/m(0,5mx1,5m)",10,3)]
        public void ObliczWageZNazwy(string nazwa, decimal ilosc,decimal wagaExpected)
        {
            var result = geowloknina.ObliczWageZNazwy(nazwa, ilosc);

            Assert.AreEqual(wagaExpected, result);
        }

        [Test]
        [TestCase("Geowłóknina ALTEX AT 100uV(0,5mx1,5m)", 0.5)]
        [TestCase("Geowłóknina ALTEX AT 100uV (1,35mx1,5m)", 1.35)]
        [TestCase("Geowłóknina ALTEX AT 100uV (1,35 mx1,5m)", 1.35)]
        [TestCase("Geowłóknina ALTEX AT 100uV ( 1,35 mx1,5m)", 1.35)]
        public void PobierzSzerokoscNawojuZNazwy_m(string nazwa, decimal szerokoscExpected)
        {
            var result = geowloknina.PobierzSzerokoscNawojuZNazwy_m(nazwa);

            Assert.AreEqual(szerokoscExpected, result);
        }

        [Test]
        [TestCase("Geowłóknina ALTEX AT 100uV(0,5mx50m)", 50)]
        [TestCase("Geowłóknina ALTEX AT 100uV (1,35mx105m)", 105)]
        [TestCase("Geowłóknina ALTEX AT 100uV (1,35 mx1,5m)", 1.5)]
        [TestCase("Geowłóknina ALTEX AT 100uV ( 1,35 mx 15m)", 15)]
        public void PobierzDlugoscNawojuZNazwy_m(string nazwa, decimal dlugoscExpected)
        {
            var result = geowloknina.PobierzDlugoscNawojuZNazwy_m(nazwa);

            Assert.AreEqual(dlugoscExpected, result);
        }
    }
}
