using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.Ewidencja.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.Ewidencja.State;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.Ewidencja
{
    public class LiniaWlokninProdukcjaEwidencjaSQLStateTests : TestBaseGeneric<LiniaWlokninProdukcjaEwidencjaSQLState>
    {
        private Mock<ITblProdukcjaRuchTowarRepository> tblProdukcjaRuchTowar;
        private Mock<IRozliczenieSQLHelper> rozliczenieSQLHelper;
        private Mock<IProdukcjaEwidencjaHelper> produkcjaEwidencjaHelper;
        private Mock<IRozliczenieMsAccesHelper> rozliczenieMsAccessHelper;
        private Mock<ITblProdukcjaZlecenieRepository> tblProdukcjaZlecenie;

        public override void SetUp()
        {
            base.SetUp();

            rozliczenieSQLHelper = new Mock<IRozliczenieSQLHelper>();
            
            produkcjaEwidencjaHelper = new Mock<IProdukcjaEwidencjaHelper>();
            rozliczenieMsAccessHelper = new Mock<IRozliczenieMsAccesHelper>();
            produkcjaEwidencjaHelper.Setup(s => s.RozliczenieMsAccesHelper).Returns(rozliczenieMsAccessHelper.Object);

            tblProdukcjaRuchTowar = new Mock<ITblProdukcjaRuchTowarRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaRuchTowar).Returns(tblProdukcjaRuchTowar.Object);
            tblProdukcjaZlecenie = new Mock<ITblProdukcjaZlecenieRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaZlecenie).Returns(tblProdukcjaZlecenie.Object);

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new LiniaWlokninProdukcjaEwidencjaSQLState(UnitOfWork.Object, rozliczenieSQLHelper.Object,produkcjaEwidencjaHelper.Object);
        }

        [Test]
        public void LoadAsync_LadujeListeZlecen()
        {
            sut.LoadAsync();

            tblProdukcjaZlecenie.Verify(x => x.GetAllAsync());
        }

        [Test]

        public void LoadAsync_LadujeZalezneObiekty()
        {
            sut.LoadAsync();

            rozliczenieMsAccessHelper.Verify(x => x.LoadAsync());
        }
    }
}
