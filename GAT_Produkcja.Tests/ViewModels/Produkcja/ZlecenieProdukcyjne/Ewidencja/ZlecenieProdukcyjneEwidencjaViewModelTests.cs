using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.BaseClasses.Messages;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Dodaj.Messages;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieDodajTowar;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieDodajTowar.Messages;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Naglowek;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Ewidencja;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.ZlecenieProdukcyjne.Ewidencja
{
    [TestFixture]
    public class ZlecenieProdukcyjneEwidencjaViewModelTests : TestBase
    {

        private Mock<ITblProdukcjaZlecenieRepository> tblProdukcjaZlecenie;
        private Mock<ITblProdukcjaZlecenieTowarRepository> tblProdukcjaZlecenieTowar;
        private Mock<ITblProdukcjaZlecenieProdukcyjne_MieszankaRepository> tblProdukcjaZlecenieProdukcyjne_Mieszanka;
        private ZlecenieProdukcyjneEwidencjaViewModel sut;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            UnitOfWorkFactory.Setup(s => s.Create()).Returns(UnitOfWork.Object);

            tblProdukcjaZlecenie = new Mock<ITblProdukcjaZlecenieRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaZlecenie).Returns(tblProdukcjaZlecenie.Object);

            tblProdukcjaZlecenieTowar = new Mock<ITblProdukcjaZlecenieTowarRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaZlecenieTowar).Returns(tblProdukcjaZlecenieTowar.Object);

            tblProdukcjaZlecenieProdukcyjne_Mieszanka = new Mock<ITblProdukcjaZlecenieProdukcyjne_MieszankaRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaZlecenieProdukcyjne_Mieszanka).Returns(tblProdukcjaZlecenieProdukcyjne_Mieszanka.Object);

            CreateSut();
        }

        public override void CreateSut()
        {
            sut = new ZlecenieProdukcyjneEwidencjaViewModel(ViewModelService.Object);
        }


        #region Messengers
        #region Rejestracja
        [Test]
        public void RejestracjaMessengerow()
        {
            //Messenger.Verify(v => v.Register(sut, It.IsAny<Action<tblProdukcjaZlecenieTowar>>(), It.IsAny<bool>()));
            Messenger.Verify(v => v.Register(sut, nameof(RefreshListMessage), It.IsAny<Action<tblProdukcjaZlecenieTowar>>(), It.IsAny<bool>())); //nameof(RefreshListMessage)
            Messenger.Verify(v => v.Register(sut, nameof(RefreshListMessage), It.IsAny<Action<tblProdukcjaZlecenie>>(), It.IsAny<bool>())); //nameof(RefreshListMessage)
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<tblProdukcjaGniazdoProdukcyjne>>(), It.IsAny<bool>()));

        }
        #endregion



        [Test]
        public async Task GdyPrzeslanoZlecenieProdukcyjne_OdswiezaEwidencje()
        {
            //var messeger = new Messenger();
            //ViewModelService.Setup(s => s.Messenger).Returns(messeger);
            //CreateSut();
            //messeger.Send(new tblProdukcjaZlcecenieProdukcyjne(), "ToList");

            await sut.LoadAsync();

            tblProdukcjaZlecenieTowar.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieTowar, bool>>>()));
        }
        [Test]
        public async Task GdyPrzeslano_tblProdukcjaZlecenieTowar_OdswiezaEwidencje()
        {
            //var messenger = new Messenger();
            //ViewModelService.Setup(s => s.Messenger).Returns(messenger);
            //CreateSut();
            //messenger.Send(new tblProdukcjaZlecenieTowar(), "ToList");

            await sut.LoadAsync();

            tblProdukcjaZlecenieTowar.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieTowar, bool>>>()));
        }

        #endregion


        #region LoadCommand
        [Test]
        public async Task LoadCommand_PobieraListeZlecenProdukcyjnychZBazy()
        {
            await sut.LoadAsync();

            tblProdukcjaZlecenieTowar.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieTowar, bool>>>()));
        }
        #endregion

        #region AddCommand
        [Test]
        public void AddCommandExecute_OtworzNoweZlecenieProdukcyjne()
        {
            sut.AddCommand.Execute(null);

            ViewService.Verify(v => v.Show<ZlecenieProdukcyjneNaglowekViewModel>());
        }
        #endregion

        #region EditCommand
        #region CanExecute
        [Test]
        public void EditCommandCanExecute_GdyWybraneZlecenieNull_False()
        {
            var actual = sut.EditCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }
        [Test]
        public void EditCommandCanExecute_GdyWybraneZlecenieNiejestNull_True()
        {
            sut.SelectedVMEntity = new tblProdukcjaZlecenieTowar();

            var actual = sut.EditCommand.CanExecute(null);

            Assert.IsTrue(actual);
        }

        private void EditCommandCanExecute_True()
        {
            sut.SelectedVMEntity = new tblProdukcjaZlecenieTowar();
        }
        #endregion

        #region Execute

        [Test]
        public void EditCommandExecute_OtworzNoweZlecenieProdukcyjne()
        {
            EditCommandCanExecute_True();

            sut.EditCommand.Execute(null);

            ViewService.Verify(v => v.Show<ZlecenieProdukcyjneNaglowekViewModel>());
        }
        [Test]
        public void EditCommandExecute_WyslijMessage_EdytujTowarMessage()
        {
            EditCommandCanExecute_True();

            sut.EditCommand.Execute(null);

            Messenger.Verify(v => v.Send(It.IsAny<tblProdukcjaZlecenieTowar>()));
        }

        #endregion
        #endregion

        #region DeleteCommand
        #region CanExecute
        [Test]
        public void UsunCommadCanExecute_GdyBrakWybranegoTowaruDoUsuniecia_False()
        {
            var actual = sut.DeleteCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }


        [Test]
        public void UsunCommadCanExecute_GdyWybranoTowarDoUsuniecia_AleStatusInnyNiz_Oczekuje_False()
        {
            sut.SelectedVMEntity = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieStatus = (int)ProdukcjaZlecenieStatusEnum.WTrakcie };

            var actual = sut.DeleteCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }

        [Test]
        public void UsunCommadCanExecute_GdyWybranoTowarDoUsuniecia_IStatus_Oczekuje_True()
        {
            DeleteCommandCanExecute_True();

            var actual = sut.DeleteCommand.CanExecute(null);

            Assert.IsTrue(actual);
        }
        private void DeleteCommandCanExecute_True()
        {
            sut.SelectedVMEntity = new tblProdukcjaZlecenieTowar
            { 
                IDProdukcjaZlecenieStatus = (int)ProdukcjaZlecenieStatusEnum.Oczekuje 
            };

        }
        #endregion

        #region Execute
        [Test]
        public void DeleteCommandExecute_Wyswietl_Dialog()
        {
            DeleteCommandCanExecute_True();

            sut.DeleteCommand.Execute(null);

            DialogService.Verify(v => v.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>()));

        }
        [Test]
        public void DeleteCommandExecute_GdyDialog_False_NieUsuwajZBazy()
        {
            DeleteCommandCanExecute_True();
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            sut.DeleteCommand.Execute(null);

            DialogService.Verify(v => v.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>()));

        }

        [Test]
        public void DeleteCommandExecute_UsuwaWskazanyTowarZlecenia()
        {
            DeleteCommandCanExecute_True();
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            var towarDoUsuniecia = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 1 };
            tblProdukcjaZlecenieTowar.Setup(s => s.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieTowar, bool>>>())).ReturnsAsync(towarDoUsuniecia);

            sut.DeleteCommand.Execute(null);

            tblProdukcjaZlecenieTowar.Verify(x => x.Remove(towarDoUsuniecia));
        }

        [Test]
        public void DeleteCommandExecute_GdyZleceniePuste_UsuwaZlecenie()
        {
            DeleteCommandCanExecute_True();
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            var towarDoUsuniecia = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 1 };
            tblProdukcjaZlecenieTowar.Setup(s => s.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieTowar, bool>>>())).ReturnsAsync(towarDoUsuniecia);
            var zlecenieDoUsuniecia = new tblProdukcjaZlecenie { IDProdukcjaZlecenie = 1 };
            tblProdukcjaZlecenieTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieTowar, bool>>>()));
            tblProdukcjaZlecenie.Setup(s => s.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenie, bool>>>())).ReturnsAsync(zlecenieDoUsuniecia);

            sut.DeleteCommand.Execute(null);

            tblProdukcjaZlecenie.Verify(x => x.Remove(zlecenieDoUsuniecia));
        }

        [Test]
        public void DeleteCommandExecute_GdyZlecenieNieJestPuste_NieUsuwaZlecenia()
        {
            DeleteCommandCanExecute_True();
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            var towarDoUsuniecia = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 1 };
            tblProdukcjaZlecenieTowar.Setup(s => s.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieTowar, bool>>>())).ReturnsAsync(towarDoUsuniecia);
            var zlecenieDoUsuniecia = new tblProdukcjaZlecenie { IDProdukcjaZlecenie = 1 };
            tblProdukcjaZlecenieTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieTowar, bool>>>())).ReturnsAsync(new List<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar()
            });
            tblProdukcjaZlecenie.Setup(s => s.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenie, bool>>>())).ReturnsAsync(zlecenieDoUsuniecia);

            sut.DeleteCommand.Execute(null);

            tblProdukcjaZlecenie.Verify(x => x.Remove(zlecenieDoUsuniecia), Times.Never);
        }


        [Test]
        public void DeleteCommandExecute_UsunTabeleZgodnieZKolejnoscia()
        {
            DeleteCommandCanExecute_True();
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            string kolejnosc = string.Empty;
            tblProdukcjaZlecenieTowar.Setup(s => s.Remove(It.IsAny<tblProdukcjaZlecenieTowar>())).Callback(() => kolejnosc += "1");
            tblProdukcjaZlecenie.Setup(s => s.Remove(It.IsAny<tblProdukcjaZlecenie>())).Callback(() => kolejnosc += "2");
            tblProdukcjaZlecenieProdukcyjne_Mieszanka.Setup(s => s.RemoveRange(It.IsAny<IEnumerable<tblProdukcjaZlecenieProdukcyjne_Mieszanka>>())).Callback(() => kolejnosc += 3);

            sut.DeleteCommand.Execute(null);

            Assert.AreEqual("123", kolejnosc);
        }

        [Test]
        public void DeleteCommandExecute_PoUsunieciuPokazDialog()
        {
            DeleteCommandCanExecute_True();
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            sut.DeleteCommand.Execute(null);

            DialogService.Verify(v => v.ShowInfo_BtnOK(It.IsAny<string>(), It.IsAny<string>()));
        }


        #endregion
        #endregion

    }
}
