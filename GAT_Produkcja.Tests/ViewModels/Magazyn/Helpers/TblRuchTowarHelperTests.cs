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
    public class TblRuchTowarHelperTests
    {

        private Mock<IUnitOfWork> unitOfWork;
        private Mock<ITblRuchTowarRepository> tblRuchTowar;
        private TblRuchTowarHelper sut;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            tblRuchTowar = new Mock<ITblRuchTowarRepository>();
            unitOfWork.Setup(s => s.tblRuchTowar).Returns(tblRuchTowar.Object);

            sut = new TblRuchTowarHelper(unitOfWork.Object);
        }

        #region IDDokumentTyp
        [Test]
        public async Task DodajDoBazyDanych_StatusPZ_IDdokumentTypShouldBe_PZ()
        {
            var ruchNaglowek = new tblRuchNaglowek { IDRuchNaglowek = 1, IDMagazynZ = 1, IDMagazynDo = 2, IsValid = true };
            var status = new tblRuchStatus { IDRuchStatus = (int)StatusRuchuTowarowEnum.PrzyjecieZewnetrne_PZ };
            var ruchTowar = new tblRuchTowar { IDRuchTowar = 0, Ilosc = 10, IsValid = true };
            tblRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblRuchTowar, bool>>>())).ReturnsAsync(new List<tblRuchTowar>
            {
                new tblRuchTowar{IDRuchTowar=1, Ilosc=10},
                new tblRuchTowar{IDRuchTowar=2, Ilosc=20}
            });


            await sut.DodajDoBazyDanych(ruchTowar, status, ruchNaglowek);

            Assert.AreEqual((int)DokumentTypEnum.PrzyjęcieZewnętrzne_PZ, sut.Towar.IDDokumentTyp);
        }
        [Test]
        public async Task DodajDoBazyDanych_StatusMM_IDdokumentTypShouldBe_MM()
        {
            var ruchNaglowek = new tblRuchNaglowek { IDRuchNaglowek = 1, IDMagazynZ = 1, IDMagazynDo = 2, IsValid = true };
            var status = new tblRuchStatus { IDRuchStatus = (int)StatusRuchuTowarowEnum.PrzesuniecieMiedzymagazynowe_MM };
            var ruchTowar = new tblRuchTowar { IDRuchTowar = 0, Ilosc = 10, IsValid = true };
            tblRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblRuchTowar, bool>>>())).ReturnsAsync(new List<tblRuchTowar>
            {
                new tblRuchTowar{IDRuchTowar=1, Ilosc=10},
                new tblRuchTowar{IDRuchTowar=2, Ilosc=20}
            });


            await sut.DodajDoBazyDanych(ruchTowar, status, ruchNaglowek);

            Assert.AreEqual((int)DokumentTypEnum.PrzesuniecieMiedzymagazynowe_MM, sut.Towar.IDDokumentTyp);
        }

        [Test]
        public async Task DodajDoBazyDanych_StatusRW_IDdokumentTypShouldBe_RW()
        {
            var ruchNaglowek = new tblRuchNaglowek { IDRuchNaglowek = 1, IDMagazynZ = 1, IDMagazynDo = 2, IsValid = true };
            var status = new tblRuchStatus { IDRuchStatus = (int)StatusRuchuTowarowEnum.RozchodWewnetrzny_RW };
            var ruchTowar = new tblRuchTowar { IDRuchTowar = 0, Ilosc = 10, IsValid = true };
            tblRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblRuchTowar, bool>>>())).ReturnsAsync(new List<tblRuchTowar>
            {
                new tblRuchTowar{IDRuchTowar=1, Ilosc=10},
                new tblRuchTowar{IDRuchTowar=2, Ilosc=20}
            });

            await sut.DodajDoBazyDanych(ruchTowar, status, ruchNaglowek);

            Assert.AreEqual((int)DokumentTypEnum.RozchodWewnetrzny_RW, sut.Towar.IDDokumentTyp);
        }

        #endregion

        #region UOW
        [Test]
        public async Task DodajDoBazyDanych_StatusMM_AddShouldBeInvokedTwice()
        {
            var ruchNaglowek = new tblRuchNaglowek { IDRuchNaglowek = 1, IDMagazynZ = 1, IDMagazynDo = 2, IsValid = true };
            var status = new tblRuchStatus { IDRuchStatus = (int)StatusRuchuTowarowEnum.PrzesuniecieMiedzymagazynowe_MM };
            var ruchTowar = new tblRuchTowar { IDRuchTowar = 0, Ilosc = 10, IsValid = true };
            tblRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblRuchTowar, bool>>>())).ReturnsAsync(new List<tblRuchTowar>
            {
                new tblRuchTowar{IDRuchTowar=1, Ilosc=10},
                new tblRuchTowar{IDRuchTowar=2, Ilosc=20}
            });


            await sut.DodajDoBazyDanych(ruchTowar, status, ruchNaglowek);

            tblRuchTowar.Verify(v => v.Add(It.IsAny<tblRuchTowar>()), Times.Exactly(2));

        }
        [Test]
        [TestCase(StatusRuchuTowarowEnum.PrzyjecieZewnetrne_PZ)]
        [TestCase(StatusRuchuTowarowEnum.WydanieZewnetrzne_WZ)]
        [TestCase(StatusRuchuTowarowEnum.RozchodWewnetrzny_RW)]
        [TestCase(StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW)]
        public async Task DodajDoBazyDanych_StatusPZandWZ_UoWShouldBeInvokedOnce(StatusRuchuTowarowEnum statusRuchu)
        {
            var ruchNaglowek = new tblRuchNaglowek { IDRuchNaglowek = 1, IDMagazynZ = 1, IDMagazynDo = 2, IsValid = true };
            var status = new tblRuchStatus { IDRuchStatus = (int)statusRuchu };
            var ruchTowar = new tblRuchTowar { IDRuchTowar = 0, Ilosc = 10, IsValid = true };
            tblRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblRuchTowar, bool>>>())).ReturnsAsync(new List<tblRuchTowar>
            {
                new tblRuchTowar{IDRuchTowar=1, Ilosc=10},
                new tblRuchTowar{IDRuchTowar=2, Ilosc=20}
            });

            await sut.DodajDoBazyDanych(ruchTowar, status, ruchNaglowek);

            tblRuchTowar.Verify(v => v.Add(It.IsAny<tblRuchTowar>()), Times.Exactly(1));
        }

        #endregion

        #region SprawdzenieIlosci

        [Test]
        [TestCase(StatusRuchuTowarowEnum.RozchodWewnetrzny_RW)]
        [TestCase(StatusRuchuTowarowEnum.WydanieZewnetrzne_WZ)]
        public async Task DodajDoBazyDanych_StatusRW_IloscMusiBycOdjetaWMagazynie(StatusRuchuTowarowEnum statusRuchu)
        {
            var ruchNaglowek = new tblRuchNaglowek { IDRuchNaglowek = 1, IDMagazynZ = 1, IDMagazynDo = 2, IsValid = true };
            var status = new tblRuchStatus { IDRuchStatus = (int)statusRuchu };
            var ruchTowar = new tblRuchTowar { IDRuchTowar = 0, Ilosc = 10, IsValid = true };
            tblRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblRuchTowar, bool>>>())).ReturnsAsync(new List<tblRuchTowar>
            {
                new tblRuchTowar{IDRuchTowar=1, Ilosc=10},
                new tblRuchTowar{IDRuchTowar=2, Ilosc=20}
            });

            await sut.DodajDoBazyDanych(ruchTowar, status, ruchNaglowek);

            Assert.AreEqual(20, sut.Towar.IloscPo);
        }

        [Test]
        [TestCase(StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW, 40)]
        [TestCase(StatusRuchuTowarowEnum.PrzyjecieZewnetrne_PZ, 40)]
        [TestCase(StatusRuchuTowarowEnum.PrzesuniecieMiedzymagazynowe_MM, 20)] // exp = 20 poniewaz najpierw dodajemy 10 do 30 a nastepnie odejmujemy 10 rowniez od 30
        public async Task DodajDoBazyDanych_IloscMusiBycDodanaDoMagazynu(StatusRuchuTowarowEnum statusRuchu, int expected)
        {
            var ruchNaglowek = new tblRuchNaglowek { IDRuchNaglowek = 1, IDMagazynZ = 1, IDMagazynDo = 2, IsValid = true };
            var status = new tblRuchStatus { IDRuchStatus = (int)statusRuchu };
            var ruchTowar = new tblRuchTowar { IDRuchTowar = 0, Ilosc = 10, IsValid = true };
            tblRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblRuchTowar, bool>>>())).ReturnsAsync(new List<tblRuchTowar>
            {
                new tblRuchTowar{IDRuchTowar=1, Ilosc=10},
                new tblRuchTowar{IDRuchTowar=2, Ilosc=20}
            });

            await sut.DodajDoBazyDanych(ruchTowar, status, ruchNaglowek);

            Assert.AreEqual(expected, sut.Towar.IloscPo);
        }

        [Test]
        [TestCase(StatusRuchuTowarowEnum.PrzesuniecieMiedzymagazynowe_MM)]
        public async Task DodajDoBazyDanych_IloscMusiBycDodanaOrazOdjetaZRoznychMagazynow(StatusRuchuTowarowEnum statusRuchu)
        {
            var ruchNaglowek = new tblRuchNaglowek { IDRuchNaglowek = 1, IDMagazynZ = 1, IDMagazynDo = 2, IsValid = true };
            var status = new tblRuchStatus { IDRuchStatus = (int)statusRuchu };
            var ruchTowar = new tblRuchTowar { IDRuchTowar = 0, Ilosc = 10, IsValid = true };
            tblRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblRuchTowar, bool>>>())).ReturnsAsync(new List<tblRuchTowar>
            {
                new tblRuchTowar{IDRuchTowar=1, Ilosc=10},
                new tblRuchTowar{IDRuchTowar=2, Ilosc=20}
            });

            await sut.DodajDoBazyDanych(ruchTowar, status, ruchNaglowek);

            tblRuchTowar.Verify(v => v.Add(It.IsAny<tblRuchTowar>()), Times.Exactly(2));
            unitOfWork.Verify(v => v.SaveAsync(), Times.Exactly(2));
            Assert.AreEqual(20, sut.Towar.IloscPo);
        }

        #endregion    
    }
}
