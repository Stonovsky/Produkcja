using GAT_Produkcja.db;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.Helpers
{
    [TestFixture]
    public class NazwaTowaruGeneratorTests
    {
        private NazwaTowaruGenerator sut;

        [SetUp]
        public void SetUp()
        {
            sut = new NazwaTowaruGenerator();
        }

        [Test]
        public void GenerujNazwe_GdyZlecenieTowar_StworzNazweTowaru()
        {
            var zlecenieTowar = new tblProdukcjaZlecenieTowar
            {
                IDProdukcjaZlecenieCiecia = 0,
                IDTowarGeowlokninaParametryGramatura = 1,
                IDTowarGeowlokninaParametrySurowiec = 1,
                Szerokosc_m = 1,
                Dlugosc_m = 1,
                tblProdukcjaZlecenieCiecia = new tblProdukcjaZlecenieCiecia { tblKontrahent = new tblKontrahent { Nazwa = "KontrahentTest" } },
                tblTowarGeowlokninaParametryGramatura = new tblTowarGeowlokninaParametryGramatura { IDTowarGeowlokninaParametryGramatura = 1, Gramatura = 100 },
                tblTowarGeowlokninaParametrySurowiec = new tblTowarGeowlokninaParametrySurowiec { IDTowarGeowlokninaParametrySurowiec = 1, Skrot = "PP" }
            };

            var nazwa = sut.GenerujNazweTowaru(zlecenieTowar);

            Assert.IsNotNull(nazwa);
        }
        [Test]
        public void GenerujNazwe_GdyZlecenieTowarBezTabelZaleznych_ZwrocNull()
        {
            var zlecenieTowar = new tblProdukcjaZlecenieTowar
            {
                IDProdukcjaZlecenieCiecia = 0,
                IDTowarGeowlokninaParametryGramatura = 1,
                IDTowarGeowlokninaParametrySurowiec = 1,
                Szerokosc_m = 1,
                Dlugosc_m = 0,
            };

            var nazwa = sut.GenerujNazweTowaru(zlecenieTowar);

            Assert.IsNull(nazwa);
        }
        [Test]
        public void GenerujNazwe_GdyZlecenieTowarJestNull_ZwrocNull()
        {
            var nazwa = sut.GenerujNazweTowaru((tblProdukcjaZlecenieTowar)null);

            Assert.IsNull(nazwa);
        }
    }
}
