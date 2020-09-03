using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.dbMsAccess.Repositories;
using GAT_Produkcja.dbMsAccess.UnitOfWork;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Ewidencja.Helpers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.RozliczenieMsAccess.Ewidencja.Helpers
{
    public class RozliczenieMsAccessEwidencjaDeleteHelperTests : TestBaseGeneric<RozliczenieMsAccessEwidencjaDeleteHelper>
    {
        private Mock<IUnitOfWorkMsAccess> unitOfWorkMsAccess;
        private Mock<ITblProdukcjaRozliczenie_PWRepository> tblProdukcjaRozliczenie_PW;
        private Mock<ITblProdukcjaRozliczenie_RWRepository> tblProdukcjaRozliczenie_RW;
        private Mock<ITblProdukcjaRozliczenie_PWPodsumowanieRepository> tblProdukcjaRozliczenie_PWPodsumowanie;
        private Mock<IKonfekcjaRepository> konfekcja;

        public override void SetUp()
        {
            base.SetUp();
            unitOfWorkMsAccess = new Mock<IUnitOfWorkMsAccess>();
            
            tblProdukcjaRozliczenie_PW = new Mock<ITblProdukcjaRozliczenie_PWRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaRozliczenie_PW).Returns(tblProdukcjaRozliczenie_PW.Object);
            
            tblProdukcjaRozliczenie_RW = new Mock<ITblProdukcjaRozliczenie_RWRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaRozliczenie_RW).Returns(tblProdukcjaRozliczenie_RW.Object);

            tblProdukcjaRozliczenie_PWPodsumowanie = new Mock<ITblProdukcjaRozliczenie_PWPodsumowanieRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaRozliczenie_PWPodsumowanie).Returns(tblProdukcjaRozliczenie_PWPodsumowanie.Object);

            konfekcja = new Mock<IKonfekcjaRepository>();
            unitOfWorkMsAccess.Setup(s => s.Konfekcja).Returns(konfekcja.Object);


            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new RozliczenieMsAccessEwidencjaDeleteHelper(UnitOfWork.Object, unitOfWorkMsAccess.Object);
        }

        #region UsunRozliczenieAsync
        [Test]
        public void UsunRozliczenieAsync_GdyBrakRolekDoUsniecia_Wyjatek()
        {
            var podsumowanie = new tblProdukcjaRozliczenie_PWPodsumowanie();

            Assert.ThrowsAsync<ArgumentException>(() => sut.UsunRozliczenieAsync(podsumowanie));
        }

        [Test]
        public void UsunRozliczenieAsync_GdySaRolkiDoUsunieciaWBazie_ZmieniaRekordWMsAccess()
        {
            var podsumowanie = new tblProdukcjaRozliczenie_PWPodsumowanie();
            tblProdukcjaRozliczenie_PW.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRozliczenie_PW, bool>>>())).ReturnsAsync(new List<tblProdukcjaRozliczenie_PW>
            {
                new tblProdukcjaRozliczenie_PW(),
                new tblProdukcjaRozliczenie_PW(),
            });

            sut.UsunRozliczenieAsync(podsumowanie);

            konfekcja.Verify(v => v.UpdateRangeNieZaksiegowano(It.IsAny<IEnumerable<tblProdukcjaRozliczenie_PW>>()));
            tblProdukcjaRozliczenie_PW.Verify(v => v.RemoveRange(It.IsAny<IEnumerable<tblProdukcjaRozliczenie_PW>>()));
            tblProdukcjaRozliczenie_RW.Verify(v => v.RemoveRange(It.IsAny<IEnumerable<tblProdukcjaRozliczenie_RW>>()));
            tblProdukcjaRozliczenie_PWPodsumowanie.Verify(v => v.RemoveRange(It.IsAny<IEnumerable<tblProdukcjaRozliczenie_PWPodsumowanie>>()));
        }
        #endregion

    }
}
