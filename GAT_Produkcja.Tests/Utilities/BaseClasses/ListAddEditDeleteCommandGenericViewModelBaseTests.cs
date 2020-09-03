using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.Utilities.BaseClasses.Messages;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.Utilities.BaseClasses
{
    public class ListAddEditDeleteCommandGenericViewModelBaseTests : TestBase
    {
        private ListAddEditDeleteCommandGenericViewModelBaseInstance sut;
        private Mock<ITblProdukcjaGeokomorkaPodsumowaniePrzerobRepository> tblProdukcjaGeokomorkaPodsumowaniePrzerob;
        private Mock<ListAddEditDeleteCommandGenericViewModelBaseInstance> sutMock;

        public override void SetUp()
        {
            base.SetUp();

            tblProdukcjaGeokomorkaPodsumowaniePrzerob = new Mock<ITblProdukcjaGeokomorkaPodsumowaniePrzerobRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaGeokomorkaPodsumowaniePrzerob).Returns(tblProdukcjaGeokomorkaPodsumowaniePrzerob.Object);
            sutMock = new Mock<ListAddEditDeleteCommandGenericViewModelBaseInstance>(ViewModelService.Object);
            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new ListAddEditDeleteCommandGenericViewModelBaseInstance(ViewModelService.Object);
        }


        #region Messengers

        #endregion

        #region LoadCommand
        [Test]
        public void LoadCommandExecute_LoadEntitiesFromDB()
        {
            sut.LoadCommand.Execute(null);

            tblProdukcjaGeokomorkaPodsumowaniePrzerob.Verify(v => v.GetAllAsync());
        }
        #endregion

        #region EditCommand
        #region CanExecute
        [Test]
        public void EditCommandCanExecute_WhenElementIsNotSelected_False()
        {
            var actual = sut.EditCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }

        [Test]
        public void EditCommandCanExecute_WhenElementIsSelected_True()
        {
            sut.SelectedVMEntity = new tblProdukcjaGeokomorkaPodsumowaniePrzerob();

            var actual = sut.EditCommand.CanExecute(null);

            Assert.IsTrue(actual);
        }
        #endregion
        #region Execute
        [Test]
        public void EditCommandExecute_SendsMessageWithSelectedElement()
        {
            sut.SelectedVMEntity = new tblProdukcjaGeokomorkaPodsumowaniePrzerob();

            sut.EditCommand.Execute(null);

            Messenger.Verify(v => v.Send(sut.SelectedVMEntity));
        }
        #endregion
        #endregion

        #region DeleteCommand
        #region CanExecute
        [Test]
        public void DeleteCommandCanExecute_WhenElementIsNotSelected_False()
        {
            var actual = sut.DeleteCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }

        [Test]
        public void DeleteCommandCanExecute_WhenElementIsSelected_True()
        {
            sut.SelectedVMEntity = new tblProdukcjaGeokomorkaPodsumowaniePrzerob();

            var actual = sut.DeleteCommand.CanExecute(null);

            Assert.IsTrue(actual);
        }
        #endregion

        #region Execute
        private void DeleteCommandCanExecute_True()
        {
            sut.SelectedVMEntity = new tblProdukcjaGeokomorkaPodsumowaniePrzerob();
        }

        [Test]
        public void DeleteCommandExecute_ShowBoolQuestionToUser()
        {
            DeleteCommandCanExecute_True();

            sut.DeleteCommand.Execute(null);

            DialogService.Verify(v => v.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        public void DeleteCommandExecute_WhenUserDoesntWantToRemove_UOWisNotInvoked()
        {
            DeleteCommandCanExecute_True();
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            sut.DeleteCommand.Execute(null);

            tblProdukcjaGeokomorkaPodsumowaniePrzerob.Verify(v => v.Remove(It.IsAny<tblProdukcjaGeokomorkaPodsumowaniePrzerob>()), Times.Never);
            UnitOfWork.Verify(v => v.SaveAsync(), Times.Never);
        }

        [Test]
        public void DeleteCommandExecute_WhenUserWantsToRemove_UOWisInvoked()
        {
            DeleteCommandCanExecute_True();
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            sut.DeleteCommand.Execute(null);

            tblProdukcjaGeokomorkaPodsumowaniePrzerob.Verify(v => v.Remove(It.IsAny<tblProdukcjaGeokomorkaPodsumowaniePrzerob>()));
            UnitOfWork.Verify(v => v.SaveAsync());
        }

        [Test]
        public void DeleteCommandExecute_WhenDeleted_MessengerSendsElement()
        {
            DeleteCommandCanExecute_True();
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            sut.DeleteCommand.Execute(null);

            Messenger.Verify(v => v.Send(sut.SelectedVMEntity,nameof(RefreshListMessage)));
        }

        [Test]
        public void DeleteCommandExecute_WhenDeleted_InfoToUser()
        {
            DeleteCommandCanExecute_True();
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            sut.DeleteCommand.Execute(null);

            DialogService.Verify(v => v.ShowInfo_BtnOK(It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        public void DeleteCommandExecute_WhenDeleted_RefreshList()
        {
            DeleteCommandCanExecute_True();
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            sut.DeleteCommand.Execute(null);

            tblProdukcjaGeokomorkaPodsumowaniePrzerob.Verify(v => v.GetAllAsync());
        }
        #endregion

        #endregion
    }
}
