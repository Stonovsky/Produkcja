using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.Helpers.Tolerancje;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.Helpers.WeryfikacjaTolerancji;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.GniazdaProdukcyjne.Helpers.Tolerancje
{
    [TestFixture]
    public class WeryfikacjaTolerancjiTests
    {
        private Mock<IUnitOfWork> unitOfWork;
        private Mock<ITblTowarGeowlokninaParametryRepository> tblTowarGeowlokninaParametry;
        private WeryfikacjaTolerancji sut;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            tblTowarGeowlokninaParametry = new Mock<ITblTowarGeowlokninaParametryRepository>();

            unitOfWork.Setup(s => s.tblTowarGeowlokninaParametry).Returns(tblTowarGeowlokninaParametry.Object);

            sut = new WeryfikacjaTolerancji(unitOfWork.Object);
        }

        [Test]
        public async Task CzyParametrZgodny_GramaturaOK_CzyParametrZgodny_True()
        {
            tblTowarGeowlokninaParametry.Setup(s => s.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblTowarGeowlokninaParametry, bool>>>())).ReturnsAsync(new db.tblTowarGeowlokninaParametry
            {
                IDTowar = 1,
                MasaPowierzchniowa = 100,
                MasaPowierzchniowa_Minimum = 90,
                MasaPowierzchniowa_Maksimum = 110
            });


            var result = await sut.CzyParametrZgodny(1, GeowlokninaParametryEnum.Gramatura, 95);

            Assert.AreEqual(true, result.CzyParametrZgodnyZTolerancja);

        }

        [Test]
        public async Task CzyParametrZgodny_GramaturaNieOK_CzyParametrZgodny_False()
        {
            tblTowarGeowlokninaParametry.Setup(s => s.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblTowarGeowlokninaParametry, bool>>>())).ReturnsAsync(new db.tblTowarGeowlokninaParametry
            {
                IDTowar = 1,
                MasaPowierzchniowa = 100,
                MasaPowierzchniowa_Minimum = 90,
                MasaPowierzchniowa_Maksimum = 110
            });

            var result = await sut.CzyParametrZgodny(1, GeowlokninaParametryEnum.Gramatura, 85);

            Assert.AreEqual(false, result.CzyParametrZgodnyZTolerancja);
        }

        [Test]
        public async Task CzyParametrZgodny_parametryWymaganeIsNull_ZwracaNull()
        {
            tblTowarGeowlokninaParametry.Setup(s => s.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblTowarGeowlokninaParametry, bool>>>()));
            
            var result = await sut.CzyParametrZgodny(0, GeowlokninaParametryEnum.Gramatura, 85);

            Assert.IsNull(result);
        }
    }
}
