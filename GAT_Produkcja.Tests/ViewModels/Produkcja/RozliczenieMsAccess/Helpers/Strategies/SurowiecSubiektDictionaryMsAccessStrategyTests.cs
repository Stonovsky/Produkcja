using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.dbMsAccess.Repositories;
using GAT_Produkcja.dbMsAccess.UnitOfWork;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.Strategy;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.RozliczenieMsAccess.Helpers.Strategies
{
    public class SurowiecSubiektDictionaryMsAccessStrategyTests : TestBase
    {
        private Mock<IUnitOfWorkMsAccess> unitOfWorkMsAccess;
        private Mock<ISurowiecRepository> surowiec;
        private SurowiecSubiektDictionaryMsAccessStrategy sut;
        private Mock<IVwMagazynRuchGTXRepository> vwMagazynRuchGTX;

        public override void SetUp()
        {
            base.SetUp();
            
            unitOfWorkMsAccess = new Mock<IUnitOfWorkMsAccess>();


            surowiec = new Mock<ISurowiecRepository>();
            unitOfWorkMsAccess.Setup(s => s.Surowiec).Returns(surowiec.Object);

            vwMagazynRuchGTX = new Mock<IVwMagazynRuchGTXRepository>();
            UnitOfWork.Setup(s => s.vwMagazynRuchGTX).Returns(vwMagazynRuchGTX.Object);


            CreateSut();

        }
        public override void CreateSut()
        {
            sut = new SurowiecSubiektDictionaryMsAccessStrategy(UnitOfWork.Object, unitOfWorkMsAccess.Object);
        }

        [Test]
        public async Task MethodName_Condition_Expectations()
        {

            surowiec.Setup(s => s.GetAllAsync())
                .ReturnsAsync(new List<Surowiec>
                {
                    new Surowiec{ Id = 1, NazwaSurowca = "PP" }
                });

            vwMagazynRuchGTX.Setup(s => s.GetAllAsync())
                .ReturnsAsync(new List<vwMagazynRuchGTX>
                {
                    new vwMagazynRuchGTX{ IdTowar = 71, Cena = 1 },
                    new vwMagazynRuchGTX{ IdTowar = 72, Cena = 1 },
                });

            var surowiecSubiekt = await sut.PobierzSurowiecZSubiektDla("PP");

            Assert.AreEqual(72, surowiecSubiekt.IdTowar);
        }

        #region CzySurowiecWSlownikuMsAccess

        [Test]
        public void CzySurowiecWSlownikuMsAccess_GdyJestWSlowniku_True()
        {
            var actual = sut.CzySurowiecWSlownikuMsAccess(1);

            Assert.IsTrue(actual);
        }
        [Test]
        public void CzySurowiecWSlownikuMsAccess_GdyBrakWSlowniku_False()
        {
            var actual = sut.CzySurowiecWSlownikuMsAccess(111);

            Assert.IsFalse(actual);
        }

        #endregion

    }
}
