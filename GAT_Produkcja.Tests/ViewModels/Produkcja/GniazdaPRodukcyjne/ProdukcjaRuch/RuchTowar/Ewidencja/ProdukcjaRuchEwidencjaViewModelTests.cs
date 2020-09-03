using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Ewidencja;
using Moq;
using NUnit.Framework;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Ewidencja
{
    public class ProdukcjaRuchEwidencjaViewModelTests : TestBase
    {
        private Mock<ITblProdukcjaRuchTowarRepository> tblProdukcjaRuchTowar;
        private ProdukcjaRuchEwidencjaUCViewModel_old sut;

        public override void SetUp()
        {
            base.SetUp();

            tblProdukcjaRuchTowar = new Mock<ITblProdukcjaRuchTowarRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaRuchTowar).Returns(tblProdukcjaRuchTowar.Object);

            CreateSut();
        }


        public override void CreateSut()
        {

            sut = new ProdukcjaRuchEwidencjaUCViewModel_old(ViewModelService.Object);
        }

        #region Messengery
        [Test]
        public void RejestracjaMessengerow()
        {

        }
        #endregion

        #region LoadCommand
        [Test]
        public void LoadCommandExecute_LadujWszystkieListy()
        {

            sut.LoadCommand.Execute(null);

            tblProdukcjaRuchTowar.Verify(v => v.GetAllAsync());
        }
        #endregion
    }
}
