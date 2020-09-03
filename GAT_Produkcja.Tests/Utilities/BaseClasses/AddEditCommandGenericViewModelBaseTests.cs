using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.Utilities.BaseClasses.Messages;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.Utilities.BaseClasses
{
    public class AddEditCommandGenericViewModelBaseTests : TestBase
    {
        private AddEditCommandGenericViewModelBaseInstance sut;
        private Mock<AddEditCommandGenericViewModelBaseInstance> sutMock;
        private Mock<ITblTowarRepository> tblTowar;

        public override void SetUp()
        {
            base.SetUp();

            sutMock = new Mock<AddEditCommandGenericViewModelBaseInstance>(ViewModelService.Object);

            
            tblTowar = new Mock<ITblTowarRepository>();
            UnitOfWork.Setup(s => s.tblTowar).Returns(tblTowar.Object);

            CreateSut();

        }
        public override void CreateSut()
        {
            sut = new AddEditCommandGenericViewModelBaseInstance(ViewModelService.Object);
        }

        #region Messengers
        [Test]
        public void MessengerRejestracja()
        {
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<tblTowar>>(), It.IsAny<bool>()));
        }

        #endregion

        #region OnElementSent
        [Test]
        public void OnElementSent_IsChangeProperty_False()
        {
            //MessengerSend(new tblTowar() { IDTowar = 1, Nazwa = "test" },()=>sut=sutMock.Object);
            MessengerSend(new tblTowar() { IDTowar = 1, Nazwa = "test" });

            //sutMock.Verify(v => v.IsChanged_False());
            Assert.IsFalse(sut.IsChanged);
        }
        #endregion

        #region IsChangeProp
        [Test]
        public void IsChangeProp_WhenElementChanges_True()
        {
            sut.VMEntity = new tblTowar { Nazwa = "Test" };

            Assert.IsTrue(sut.IsChanged);
        }

        [Test]
        public void IsChangeProp_WhenElementIsNotChanged_False()
        {
            Assert.IsFalse(sut.IsChanged);
        }

        #endregion

        #region IsChanged_False
        [Test]
        public void IsChangedFalse_IsChangePropertyIsFalse()
        {
            sut.VMEntity = new tblTowar { Nazwa = "Test" };

            sut.IsChanged_False();

            Assert.IsFalse(sut.IsChanged);
        }
        #endregion

        #region SaveCommand
        #region CanExecute
        [Test]
        public void SaveCommandCanExecute_ElementIsNotValid_False()
        {
            var actual = sut.SaveCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }

        [Test]
        public void SaveCommandCanExecute_ElementIsValid_True ()
        {
            sut.VMEntity = new tblTowar
            {
                IDTowarGrupa = 1,
                Nazwa = "T",
                IDJm = 1,
            };

            var actual = sut.SaveCommand.CanExecute(null);

            Assert.IsTrue(actual);
        }

        private void SaveCommandCanExecute_True()
        {
            sut.VMEntity = new tblTowar
            {
                IDTowarGrupa = 1,
                Nazwa = "T",
                IDJm = 1,
            };
        }

        #endregion

        #region Execute
        [Test]
        public void SaveCommandExecute_UOWSaveAsyncIsInvoked()
        {
            SaveCommandCanExecute_True();

            sut.SaveCommand.Execute(null);

            UnitOfWork.Verify(v => v.SaveAsync());
        }

        [Test]
        public void SaveCommandExecute_DialogService_Info_IsShown()
        {
            SaveCommandCanExecute_True();

            sut.SaveCommand.Execute(null);

            DialogService.Verify(v => v.ShowInfo_BtnOK(It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        public void SaveCommandExecute_SendMessage()
        {
            SaveCommandCanExecute_True();

            sut.SaveCommand.Execute(null);

            Messenger.Verify(v => v.Send(sut.VMEntity,nameof(RefreshListMessage)));
        }

        [Test]
        public void SaveCommandExecute_CloseForm()
        {
            SaveCommandCanExecute_True();

            sut.SaveCommand.Execute(null);

            ViewService.Verify(v => v.Close(sut.GetType().Name));
        }

        #endregion
        #endregion
    }
}
