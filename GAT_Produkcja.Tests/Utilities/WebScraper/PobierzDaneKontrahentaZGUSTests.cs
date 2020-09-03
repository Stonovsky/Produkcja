
using GAT_Produkcja.db;
using GAT_Produkcja.UI.Utilities.WebScraper;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.Utilities.WebScraper
{
    [TestFixture]
    public class PobierzDaneKontrahentaZGUSTests
    {
        private Mock<IDaneKontrahentaZGUSWebScraper> daneKontrahentaZGUSWebScraper;
        private PobierzDaneKontrahentaZGUS sut;

        [SetUp]
        public void SetUp()
        {
            daneKontrahentaZGUSWebScraper = new Mock<IDaneKontrahentaZGUSWebScraper>();

            sut = new PobierzDaneKontrahentaZGUS(daneKontrahentaZGUSWebScraper.Object);
        }

        [Test]
        public async Task PobierzAsync_GdyListaPobranychDanychJestPusta_ZwracaNowegoKontrahenta()
        {
            daneKontrahentaZGUSWebScraper.Setup(s => s.PobierzAsync(It.IsAny<string>())).ReturnsAsync(new List<string>());
            var kontrahent = new tblKontrahent { ID_Kontrahent = 1 };

            var actual = await sut.PobierzAsync(kontrahent);

            Assert.IsTrue(actual.Compare(new tblKontrahent()));
        }

        [Test]
        public async Task PobierzAsync_GdyListaPobranychDanychNieJestPustaAleNieMaWojewodztwa_ZwracaNowegoKontrahenta()
        {
            daneKontrahentaZGUSWebScraper.Setup(s => s.PobierzAsync(It.IsAny<string>())).ReturnsAsync(new List<string>() { "0", "1", "Nazwa firmy",  });
            var kontrahent = new tblKontrahent { ID_Kontrahent = 1 };

            var actual = await sut.PobierzAsync(kontrahent);

            Assert.IsNull(actual.Wojewodztwo);
        }
    }
}
