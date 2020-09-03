using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Helpers.Theme;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.Helpers.Theme
{
    [TestFixture]
    public class ThemeChangerHelperTests
    {
        private Mock<IUnitOfWork> unitOfWork;
        private Mock<ITblPracownikGATRepository> tblPracownikGAT;
        private ThemeChangerHelper sut;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            tblPracownikGAT = new Mock<ITblPracownikGATRepository>();

            unitOfWork.Setup(s => s.tblPracownikGAT).Returns(tblPracownikGAT.Object);

            sut = new ThemeChangerHelper(unitOfWork.Object);
        }

        [Test]
        public async Task DodajMotywKoloruDoBazy_GdyPracownikNiezalogowany_NicNieRob()
        {
            await sut.AddToDataBase(null, ThemeColorEnum.Light);

            tblPracownikGAT.Verify(v => v.GetByIdAsync(It.IsAny<int>()), Times.Never);
            unitOfWork.Verify(v => v.SaveAsync(), Times.Never);
        }

        [Test]
        public async Task DodajMotywKoloruDoBazy_GdyPracownikIDjestZero_NicNieRob()
        {
            await sut.AddToDataBase(0, ThemeColorEnum.Light);

            tblPracownikGAT.Verify(v => v.GetByIdAsync(It.IsAny<int>()), Times.Never);
            unitOfWork.Verify(v => v.SaveAsync(), Times.Never);
        }


        [Test]
        public async Task DodajMotywKoloruDoBazy_GdyZmianaKOloruNaToSamoCoWBazie_NieZapisuj()
        {
            tblPracownikGAT.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new db.tblPracownikGAT { ID_PracownikGAT = 1, MotywKoloru = 1 });

            await sut.AddToDataBase(1, ThemeColorEnum.Light);

            unitOfWork.Verify(v => v.SaveAsync(), Times.Never);
        }

    }
}
