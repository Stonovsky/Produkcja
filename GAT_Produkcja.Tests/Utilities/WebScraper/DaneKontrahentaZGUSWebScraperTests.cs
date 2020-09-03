using GAT_Produkcja.db;
using GAT_Produkcja.UI.Utilities.WebScraper;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.Utilities.WebScraper
{
    [TestFixture]
    class DaneKontrahentaZGUSWebScraperTests
    {
        private DaneKontrahentaZGUSWebScraper sut;

        [SetUp]
        public void SetUp()
        {
            sut = new DaneKontrahentaZGUSWebScraper();
        }

        [Test]
        [Ignore("Test POZYTYWNY, wlaczyc gdy potrzeba")]
        [TestCase("638-18-39-687")]
        [TestCase("7820021269")]
        [TestCase("651 159 47 94")]
        public async Task PobierzAsync_GdyNipOk_PobieraDane(string nip)
        {
            var k = await sut.PobierzAsync(nip);

            Assert.IsTrue(k.Count>0);
        }

        [Test]
        public async Task PobierzAsync_GdyNIPbledny_ZwrocNull()
        {
            var k = await sut.PobierzAsync("65116686301");

            Assert.IsNull(k);
        }

    }
}
