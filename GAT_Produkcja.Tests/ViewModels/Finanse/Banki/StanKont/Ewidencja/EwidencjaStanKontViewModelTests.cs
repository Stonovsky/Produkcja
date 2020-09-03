using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Finanse.Banki.StanKont.DodajStanKonta;
using GAT_Produkcja.ViewModel.Finanse.Banki.StanKont.EwidencjaStanKont;
using GAT_Produkcja.ViewModel.Finanse.Banki.StanKont.EwidencjaStanKont.Messages;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Finanse.Banki.StanKont.Ewidencja
{
    public class EwidencjaStanKontViewModelTests : TestBase
    {
        private EwidencjaStanKontViewModel sut;
        private Mock<ITblFinanseStanKontaRepository> tblFinanseStanKonta;

        public override void SetUp()
        {
            base.SetUp();

            tblFinanseStanKonta = new Mock<ITblFinanseStanKontaRepository>();
            UnitOfWork.Setup(s => s.tblFinanseStanKonta).Returns(tblFinanseStanKonta.Object);

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new EwidencjaStanKontViewModel(ViewModelService.Object);
        }

        #region Messenger
        #region Register
        #endregion

        #endregion

        #region LoadCommand
        [Test]
        public void LoadCommandExecute_LadujeZestawienieZBazy()
        {
            sut.LoadCommand.Execute(null);

            tblFinanseStanKonta.Verify(v => v.GetAllAsync());
        }
        #endregion

        #region AddCommand
        [Test]
        public void AddCommandExecute_OtwieraFormularz()
        {
            sut.AddCommand.Execute(null);

            ViewService.Verify(v => v.Show<DodajStanKontaViewModel>());
        }
        #endregion

        #region EditCommand
        #region CanExecute
        [Test]
        public void EditCommandCanExecute_GdyBrakWybrnejPozycji_False()
        {
            var actual = sut.EditCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }
        [Test]

        public void EditCommandCanExecute_GdyWybrnaPozycja_True()
        {
            sut.SelectedVMEntity = new tblFinanseStanKonta();

            var actual = sut.EditCommand.CanExecute(null);

            Assert.IsTrue(actual);
        }
        #endregion
        #region Execute
        [Test]
        public void EditCommandExecute_OtwieraFormularz()
        {
            sut.SelectedVMEntity = new tblFinanseStanKonta();

            sut.EditCommand.Execute(null);

            ViewService.Verify(v => v.Show<DodajStanKontaViewModel>());
        }
        [Test]
        public void EditCommandExecute_WysylaWybranaPozycje()
        {
            sut.SelectedVMEntity = new tblFinanseStanKonta();

            sut.EditCommand.Execute(null);

            Messenger.Verify(v => v.Send(sut.SelectedVMEntity));
        }
        #endregion

        #endregion

        #region DeleteCommand
        #region CanExecute
        [Test]
        public void DeleteCommandCanExecute_GdyBrakWybranegoStanu_False()
        {
            var actual = sut.DeleteCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }
        #endregion

        #region Execute
        [Test]
        public void DeleteCommandExecute_WyswietlaKomunikat()
        {
            sut.SelectedVMEntity = new tblFinanseStanKonta();

            sut.DeleteCommand.Execute(null);

            DialogService.Verify(v => v.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        public void DeleteCommandExecute_GdyKomunikatFalse_UOWRemoveNieJestWywolane()
        {

            sut.SelectedVMEntity = new tblFinanseStanKonta();
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            sut.DeleteCommand.Execute(null);

            tblFinanseStanKonta.Verify(v => v.Remove(It.IsAny<tblFinanseStanKonta>()), Times.Never);
        }

        [Test]
        public void DeleteCommandExecute_GdyKomunikatTrue_UOWRemoveJestWywolane()
        {

            sut.SelectedVMEntity = new tblFinanseStanKonta();
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            sut.DeleteCommand.Execute(null);

            tblFinanseStanKonta.Verify(v => v.Remove(It.IsAny<tblFinanseStanKonta>()));
            UnitOfWork.Verify(v => v.SaveAsync());
        }

        [Test]
        public void DeleteCommandExecute_PoUsuieciu_WyswietlaKomunikat()
        {

            sut.SelectedVMEntity = new tblFinanseStanKonta();
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            sut.DeleteCommand.Execute(null);

            DialogService.Verify(v => v.ShowInfo_BtnOK(It.IsAny<string>(), It.IsAny<string>()));
        }
        [Test]
        public void DeleteCommandExecute_PoUsuieciu_OdswiezaListe()
        {

            sut.SelectedVMEntity = new tblFinanseStanKonta();
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            sut.DeleteCommand.Execute(null);

            tblFinanseStanKonta.Verify(v => v.GetAllAsync());
        }
        #endregion
        #endregion
    }
}
