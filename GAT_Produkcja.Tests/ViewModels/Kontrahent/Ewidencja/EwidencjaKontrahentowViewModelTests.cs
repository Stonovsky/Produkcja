using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.GenericRepository;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Kontrahent.Dodaj;
using GAT_Produkcja.ViewModel.Kontrahent.Ewidencja;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Kontrahent.Ewidencja
{
    public class EwidencjaKontrahentowViewModelTests : TestBaseGeneric<EwidencjaKontrahentowViewModel>
    {
        private Messenger messengerOrg;
        private Mock<ITblKontrahentRepository> tblKontrahent;

        public override void SetUp()
        {
            base.SetUp();
            messengerOrg = new Messenger();
            tblKontrahent = new Mock<ITblKontrahentRepository>();
            UnitOfWork.Setup(s => s.tblKontrahent).Returns(tblKontrahent.Object);

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new EwidencjaKontrahentowViewModel(ViewModelService.Object);
        }

        #region Messengers
        #region Registration
        [Test]
        public void RejestracjaMessengerow()
        {
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<ListViewModelStatesEnum>>(), It.IsAny<bool>()));

        }
        #endregion
        #endregion


        #region ListViewModelStatesEnum
        [Test]
        public void WhenListViewModelStatesEnumIsSent_ButtonTextChanges()
        {
            ViewModelService.Setup(s => s.Messenger).Returns(messengerOrg);
            CreateSut();

            messengerOrg.Send(ListViewModelStatesEnum.Select);

            Assert.AreEqual("Wybierz", sut.SelectEditButtonTitle);

            messengerOrg.Send(ListViewModelStatesEnum.AddEdit);

            Assert.AreEqual("Edytuj", sut.SelectEditButtonTitle);

        }

        [Test]
        public void WhenSelectIsSent_CloseWidndow()
        {
            WhenSeletIsSentSetup();

            sut.EditCommand.Execute(null);

            ViewService.Verify(x => x.Close(sut.GetType().Name));

        }

        private void WhenSeletIsSentSetup()
        {
            ViewModelService.Setup(s => s.Messenger).Returns(messengerOrg);
            CreateSut();
            messengerOrg.Send(ListViewModelStatesEnum.Select);
            sut.SelectedVMEntity = new tblKontrahent();
        }
        #endregion

        [Test]
        public void RepositorySelected()
        {
            var s = sut.Repository;
            var t = this.tblKontrahent.Object;

            Assert.IsInstanceOf(t.GetType(),s);
        }

        [Test]
        public void ShowAddEditWindow_ViewServiceShowIsInvoked()
        {
            sut.ShowAddEditWindow.Invoke();

            ViewService.Verify(x => x.Show<DodajKontrahentaViewModel>());
        }

        #region EditCommand
        [Test]
        public void WhenEditCommandIsPressed_SendMessengerWithType()
        {
            sut.SelectedVMEntity = new tblKontrahent();

            sut.EditCommand.Execute(null);

            Messenger.Verify(x => x.Send(It.IsAny<tblKontrahent>()));
        }
        #endregion
    }
}
