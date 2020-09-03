using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.Geokomorka.Przerob.Ewidencja;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.Geokomorka.Przerob.Ewidencja
{
    public class EwidencjaPrzerobProdukcjaGeokomorkaViewModelTests : TestBase
    {
        private EwidencjaPrzerobProdukcjaGeokomorkaViewModel sut;
        private Mock<ITblProdukcjaGeokomorkaPodsumowaniePrzerobRepository> tblProdukcjaGeokomorkaPodsumowaniePrzerob;

        public override void SetUp()
        {
            base.SetUp();

            tblProdukcjaGeokomorkaPodsumowaniePrzerob = new Mock<ITblProdukcjaGeokomorkaPodsumowaniePrzerobRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaGeokomorkaPodsumowaniePrzerob).Returns(tblProdukcjaGeokomorkaPodsumowaniePrzerob.Object);

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new EwidencjaPrzerobProdukcjaGeokomorkaViewModel(ViewModelService.Object);
        }

    }
}
