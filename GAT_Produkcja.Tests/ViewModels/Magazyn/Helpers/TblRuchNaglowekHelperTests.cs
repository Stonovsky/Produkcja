using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.ViewModel.Magazyn.Helpers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Magazyn.Helpers
{
    [TestFixture]
    public class TblRuchNaglowekHelperTests
    {
        private Mock<IUnitOfWork> unitOfWork;
        private Mock<ITblRuchStatusRepository> tblRuchStatus;
        private Mock<ITblRuchNaglowekRepository> tblRuchNaglowek;
        private TblRuchNaglowekHelper sut;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = new Mock<IUnitOfWork>();

            tblRuchStatus = new Mock<ITblRuchStatusRepository>();
            tblRuchNaglowek = new Mock<ITblRuchNaglowekRepository>();

            unitOfWork.Setup(s => s.tblRuchStatus).Returns(tblRuchStatus.Object);
            unitOfWork.Setup(s => s.tblRuchNaglowek).Returns(tblRuchNaglowek.Object);

            sut = new TblRuchNaglowekHelper(unitOfWork.Object);
        }

        [Test]
        public async Task NrDokumentuGenerator_GdyBrakRekordowWBaize_Zwraca_1()
        {
            tblRuchNaglowek.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblRuchNaglowek, bool>>>())).ReturnsAsync(new List<tblRuchNaglowek>
            {
            });
            tblRuchStatus.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new tblRuchStatus { IDRuchStatus = 1, Status = "PZ", Symbol = "PZ" });

            var nrDok = await sut.NrDokumentuGenerator(StatusRuchuTowarowEnum.PrzyjecieZewnetrne_PZ);
            var test = DateTime.Now.Year;
            var expected = $"PZ 1/{DateTime.Now.Year}";
            Assert.AreEqual(expected, nrDok.PelnyNrDokumentu);
        }

        [Test]
        public async Task NrDokumentuGenerator_GdySaRekordyWBazie_Zwraca_NrOJedenWiekszy()
        {
            tblRuchNaglowek.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblRuchNaglowek, bool>>>())).ReturnsAsync(new List<tblRuchNaglowek>
            {
                new tblRuchNaglowek{ IDRuchNaglowek=1, IDRuchStatus=1, NrDokumentu=2}
            });
            tblRuchStatus.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new tblRuchStatus { IDRuchStatus = 1, Status = "PZ", Symbol = "PZ" });

            var nrDok = await sut.NrDokumentuGenerator(StatusRuchuTowarowEnum.PrzyjecieZewnetrne_PZ);
            var expected = $"PZ 3/{DateTime.Now.Year}";
            
            Assert.AreEqual(expected, nrDok.PelnyNrDokumentu);
        }
    }
}
