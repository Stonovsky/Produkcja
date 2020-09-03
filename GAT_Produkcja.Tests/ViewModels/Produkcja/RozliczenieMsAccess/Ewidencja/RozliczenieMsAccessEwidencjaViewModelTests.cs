using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Ewidencja;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Ewidencja.Helpers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.RozliczenieMsAccess.Ewidencja
{
    class RozliczenieMsAccessEwidencjaViewModelTests : TestBase
    {
        private Mock<ITblProdukcjaRozliczenie_PWPodsumowanieRepository> tblProdukcjaRozliczenie_PWPodsumowanie;
        private Mock<IRozliczenieMsAccessEwidencjaDeleteHelper> deleteHelper;
        private RozliczenieMsAccessEwidencjaViewModel sut;

        public override void SetUp()
        {
            base.SetUp();
            deleteHelper = new Mock<IRozliczenieMsAccessEwidencjaDeleteHelper>();

            tblProdukcjaRozliczenie_PWPodsumowanie = new Mock<ITblProdukcjaRozliczenie_PWPodsumowanieRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaRozliczenie_PWPodsumowanie).Returns(tblProdukcjaRozliczenie_PWPodsumowanie.Object);

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new RozliczenieMsAccessEwidencjaViewModel(ViewModelService.Object,deleteHelper.Object);
        }


        #region Tytul
        [Test]
        public void TytulNiepusty()
        {
            Assert.IsNotEmpty(sut.Title);
        }
        #endregion

        #region LoadCommand
        [Test]
        public void LoadCommandExecute_PobieraWszystkieNiezbedneDaneZBazy()
        {

            sut.LoadCommand.Execute(null);

            tblProdukcjaRozliczenie_PWPodsumowanie.Verify(v => v.GetAllAsync()); 
        }
        #endregion

        #region DeleteCommand
        #region Execute
        [Test]
        public void DeleteCommandExecute_UsunRozliczenieHelperJestWywolany()
        {
            sut.SelectedVMEntity = new tblProdukcjaRozliczenie_PWPodsumowanie();
            DialogService.Setup(x => x.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            
            sut.DeleteCommand.Execute(null);

            deleteHelper.Verify(x => x.UsunRozliczenieAsync(It.IsAny<tblProdukcjaRozliczenie_PWPodsumowanie>()));
        }

        [Test]
        public void DeleteCommandExecute_GdyUsunRozliczenieHelperRzucaArgumentException_DialogServicePokazujeToUzytkownikowi()
        {
            sut.SelectedVMEntity = new tblProdukcjaRozliczenie_PWPodsumowanie();
            DialogService.Setup(x => x.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            deleteHelper.Setup(x => x.UsunRozliczenieAsync(It.IsAny<tblProdukcjaRozliczenie_PWPodsumowanie>())).Throws<ArgumentException>();

            sut.DeleteCommand.Execute(null);

            DialogService.Verify(x => x.ShowError_BtnOK(It.IsAny<string>(),It.IsAny<string>()));
        }
        #endregion
        #endregion

    }
}
