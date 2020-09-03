using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.Utilities.CurrencyService.NBP;
using GAT_Produkcja.Utilities.CurrencyService.NBP.Model;
using GAT_Produkcja.Utilities.Wrappers;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.Utilities.CurrencyService.NBP
{
    public class NBPServiceTests : TestBase
    {
        private NBPService sut;
        private Mock<IHttpClientWrapper> httpClientHandler;

        public override void SetUp()
        {
            base.SetUp();

            httpClientHandler = new Mock<IHttpClientWrapper>();

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new NBPService(httpClientHandler.Object);
        }

        [Test]
        public async Task GetCurrencyForDate_GdyWpisaneDanePoprawnie_PobieraKurs()
        {
            httpClientHandler.Setup(s => s.GetStringAsync(It.IsAny<string>())).ReturnsAsync(@"{'table':'A','currency':'euro','code':'EUR','rates':[{'no':'098 / A / NBP / 2020','effectiveDate':'2020 - 05 - 21','mid':4.5365}]}");
            
            var kurs = await sut.GetActualCurrencyRate(CurrencyShorcutEnum.EUR);

            Assert.AreEqual(4.5365m, kurs);
        }

        [Test]
        public async Task GetCurrencyForDate_GdyBrakZwroconyJsonJestPusty_ZwracaZero()
        {
            httpClientHandler.Setup(s => s.GetStringAsync(It.IsAny<string>())).ReturnsAsync(string.Empty);

            var kurs = await sut.GetActualCurrencyRate(CurrencyShorcutEnum.EUR);

            Assert.AreEqual(0, kurs);
        }

        [Test]
        public async Task GetCurrencyForDate_GdyBrakZwroconyJsonJestNull_ZwracaZero()
        {
            httpClientHandler.Setup(s => s.GetStringAsync(It.IsAny<string>()));

            var kurs = await sut.GetActualCurrencyRate(CurrencyShorcutEnum.EUR);

            Assert.AreEqual(0, kurs);
        }
    }
}
