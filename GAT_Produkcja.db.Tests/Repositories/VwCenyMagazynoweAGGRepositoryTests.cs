using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Tests.Repositories
{
    [TestFixture, Ignore("Testy na bazie danych. Wlaczac kiedy trzeba")]
    public class VwCenyMagazynoweAGGRepositoryTests
    {
        private UnitOfWork sut;

        [SetUp]
        public void SetUp()
        {
            sut = new UnitOfWork(new GAT_ProdukcjaModel());
        }

        [Test]
        public async Task GetPriceFromPRoductName_GdyProduktJestWBazie_ZwracaCene()
        {
            var cena = await sut.vwCenyMagazynoweAGG.GetPriceFromPRoductName("ALTEX AT PP 90");

            Assert.IsTrue(cena > 0);
        }

        [Test]
        public async Task GetPriceFromPRoductName_GdyProduktJestWBazieZUV_ZwracaCene()
        {
            var cena = await sut.vwCenyMagazynoweAGG.GetPriceFromPRoductName("ALTEX AT PP 90 UV");

            Assert.IsTrue(cena > 0);
        }
        [Test]
        public async Task GetPriceFromPRoductName_GdyProduktuNieMaWBazie_ZwracaZero()
        {
            var cena = await sut.vwCenyMagazynoweAGG.GetPriceFromPRoductName("ALTEX AT PP 190 UV");

            Assert.IsTrue(cena == 0);
        }
    }
}
