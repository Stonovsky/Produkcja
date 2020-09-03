using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
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
    public class PodsumowanieSprzedazHelperTests : TestBase
    {
        private PodsumowanieSprzedazHelper sut;
        private Mock<IVwZestSprzedazyAGGRepository> vwZestSprzedazyAGG;

        public override void SetUp()
        {
            base.SetUp();

            vwZestSprzedazyAGG = new Mock<IVwZestSprzedazyAGGRepository>();
            UnitOfWork.Setup(s => s.vwZestSprzedazyAGG).Returns(vwZestSprzedazyAGG.Object);

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new PodsumowanieSprzedazHelper(UnitOfWork.Object);
        }

        #region PobierzSprzedazAGGWDatach

        [Test]
        public async Task PobierzSprzedazAGGWDatach_GdyPodaneDaty_PobieraSprzedazZBazy()
        {
            var dataOd = DateTime.Now.Date;
            var dataDo = DateTime.Now.Date.AddDays(2);

            var sprzedazWDatach = await sut.PobierzSprzedazAGGWDatach(dataOd, dataDo);

            vwZestSprzedazyAGG.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<vwZestSprzedazyAGG, bool>>>()));
        }

        [Test]
        public async Task PobierzSprzedazAGGWDatach_GdyPodaneDaty_GenerujePodsumowanie()
        {
            var dataOd = DateTime.Now.Date;
            var dataDo = DateTime.Now.Date.AddDays(2);
            vwZestSprzedazyAGG.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<vwZestSprzedazyAGG, bool>>>())).ReturnsAsync(new List<vwZestSprzedazyAGG>
            {
                new vwZestSprzedazyAGG{Ilosc=1,Netto=1,Zysk=1},
                new vwZestSprzedazyAGG{Ilosc=1,Netto=1,Zysk=1},
            });

            var sprzedazWDatach = await sut.PobierzSprzedazAGGWDatach(dataOd, dataDo);

            Assert.AreEqual(2, sprzedazWDatach.First().Ilosc_m2);
            Assert.AreEqual(2, sprzedazWDatach.First().Netto);

        }

        [Test]
        public async Task PobierzSprzedazAGGWDatach_GdyBrakSprzedazyWPodanychDatach_GenerujePodsumowanieZerowe()
        {
            var dataOd = DateTime.Now.Date;
            var dataDo = DateTime.Now.Date.AddDays(2);

            var sprzedazWDatach = await sut.PobierzSprzedazAGGWDatach(dataOd, dataDo);

            Assert.AreEqual(0, sprzedazWDatach.First().Ilosc_m2);
            Assert.AreEqual(0, sprzedazWDatach.First().Netto);
        }
        #endregion
    }
}
