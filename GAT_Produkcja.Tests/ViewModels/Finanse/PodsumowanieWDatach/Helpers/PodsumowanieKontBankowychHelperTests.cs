using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.Utilities.CurrencyService.NBP;
using GAT_Produkcja.Utilities.CurrencyService.NBP.Model;
using GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Helpers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Finanse.PodsumowanieWDatach.Helpers
{
    class PodsumowanieKontBankowychHelperTests : TestBase
    {
        private PodsumowanieKontBankowychHelper sut;
        private Mock<INBPService> nbpService;
        private Mock<IVwFinanseBankAGGRepository> vwFinanseBankAGG;
        private Mock<IVwFinanseBankGTXRepository> vwFinanseBankGTX;
        private Mock<IVwFinanseBankGTX2Repository> vwFinanseBankGTX2;
        private Mock<ITblFinanseStanKontaRepository> tblFinanseStanKonta;

        public override void SetUp()
        {
            base.SetUp();

            nbpService = new Mock<INBPService>();

            vwFinanseBankAGG = new Mock<IVwFinanseBankAGGRepository>();
            vwFinanseBankGTX = new Mock<IVwFinanseBankGTXRepository>();
            vwFinanseBankGTX2 = new Mock<IVwFinanseBankGTX2Repository>();

            UnitOfWork.Setup(s => s.vwFinanseBankAGG).Returns(vwFinanseBankAGG.Object);
            UnitOfWork.Setup(s => s.vwFinanseBankGTX).Returns(vwFinanseBankGTX.Object);
            UnitOfWork.Setup(s => s.vwFinanseBankGTX2).Returns(vwFinanseBankGTX2.Object);

            tblFinanseStanKonta = new Mock<ITblFinanseStanKontaRepository>();
            UnitOfWork.Setup(s => s.tblFinanseStanKonta).Returns(tblFinanseStanKonta.Object);


            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new PodsumowanieKontBankowychHelper(UnitOfWork.Object, nbpService.Object);
        }

        #region PobierzKontaDoDaty

        [Test]
        public async Task PobierzStanKontZDaty_GdyWBazieBrakStanow_ZwracaNowyModel()
        {
            tblFinanseStanKonta.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblFinanseStanKonta, bool>>>()));

            var actual = await sut.PobierzStanKontZDaty(DateTime.Now.Date);

            Assert.IsNull(actual);
        }


        [Test]
        public void PobierzStanKontZDaty_PobieraZBazyKonta()
        {
            sut.PobierzStanKontZDaty(DateTime.Now.Date);

            tblFinanseStanKonta.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<tblFinanseStanKonta, bool>>>()));
        }

        [Test]
        public async Task PobierzStanKontZDaty_PobieraKontaTylkoZNajnowszaDataStanu()
        {
            tblFinanseStanKonta.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblFinanseStanKonta, bool>>>()))
                .ReturnsAsync(new List<tblFinanseStanKonta>
                {
                    new tblFinanseStanKonta{IDFinanseStanKonta=3, DataStanu=DateTime.Now.Date.AddDays(-3)},
                    new tblFinanseStanKonta{IDFinanseStanKonta=4, DataStanu=DateTime.Now.Date.AddDays(-8)},
                    new tblFinanseStanKonta{IDFinanseStanKonta=1, DataStanu=DateTime.Now.Date},
                    new tblFinanseStanKonta{IDFinanseStanKonta=2, DataStanu=DateTime.Now.Date},
                });

            var actual = await sut.PobierzStanKontZDaty(DateTime.Now.Date);

            Assert.AreEqual(2, actual.Count());
        }

        #region KursyWalut
        [Test]
        public async Task PobierzStanKontZDaty_PrzypipsujeKursWgWalut()
        {
            tblFinanseStanKonta.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblFinanseStanKonta, bool>>>()))
                .ReturnsAsync(new List<tblFinanseStanKonta>
                {
                    new tblFinanseStanKonta{IDFinanseStanKonta=1, DataStanu=DateTime.Now.Date, Stan=1, Waluta="EUR"},
                    new tblFinanseStanKonta{IDFinanseStanKonta=2, DataStanu=DateTime.Now.Date, Stan =1, Waluta="PLN"},
                });
            nbpService.Setup(s => s.GetActualCurrencyRate(CurrencyShorcutEnum.EUR))
                      .ReturnsAsync(2);
            nbpService.Setup(s => s.GetActualCurrencyRate("EUR"))
                      .ReturnsAsync(2);
            var actual = await sut.PobierzStanKontZDaty(DateTime.Now.Date);

            Assert.AreEqual(2, actual.Count());
            Assert.AreEqual(2, actual.First().StanWPrzeliczeniu);
        }
        #endregion

        #endregion

        #region Podsumowanie
        [Test]
        public void PodsumowanieProperty_GdyStanyKontNull_ZwrtacaZero()
        {
            Assert.AreEqual(0, sut.Podsumowanie);
        }

        [Test]
        public async Task PodsumowanieProperty_GdyStanyKontNieNull_ZwrtacaSumeStanowKont()
        {
            tblFinanseStanKonta.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblFinanseStanKonta, bool>>>()))
            .ReturnsAsync(new List<tblFinanseStanKonta>
            {
                new tblFinanseStanKonta{IDFinanseStanKonta=1, DataStanu=DateTime.Now.Date, Stan=1, Waluta="PLN"},
                new tblFinanseStanKonta{IDFinanseStanKonta=2, DataStanu=DateTime.Now.Date, Stan=1, Waluta="PLN"},
            });
            nbpService.Setup(s => s.GetActualCurrencyRate(It.IsAny<string>())).ReturnsAsync(1);

            var actual = await sut.PobierzStanKontZDaty(DateTime.Now.Date);

            Assert.AreEqual(2, sut.Podsumowanie);
        }
        #endregion


    }
}
