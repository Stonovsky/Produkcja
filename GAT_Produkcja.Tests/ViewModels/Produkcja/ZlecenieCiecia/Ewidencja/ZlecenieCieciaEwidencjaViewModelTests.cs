using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Dodaj.Naglowek;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Ewidencja;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.ZlecenieCiecia.Ewidencja
{
    [TestFixture]
    public class ZlecenieCieciaEwidencjaViewModelTests : TestBase
    {

        private Mock<ITblProdukcjaZlecenieCieciaRepository> tblProdukcjaZlecenieCiecia;
        private Mock<ITblProdukcjaZlecenieTowarRepository> tblProdukcjaZlecenieTowar;
        private ZlecenieCieciaEwidencjaViewModel sut;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            tblProdukcjaZlecenieCiecia = new Mock<ITblProdukcjaZlecenieCieciaRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaZlecenieCiecia).Returns(tblProdukcjaZlecenieCiecia.Object);

            tblProdukcjaZlecenieTowar = new Mock<ITblProdukcjaZlecenieTowarRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaZlecenieTowar).Returns(tblProdukcjaZlecenieTowar.Object);

            CreateSut();
        }

        public override void CreateSut()
        {
            sut = new ZlecenieCieciaEwidencjaViewModel(ViewModelService.Object);
        }

        #region Messengers
        [Test]
        public void Messengers_Rejestracja()
        {
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<tblProdukcjaZlecenieCiecia>>(), It.IsAny<bool>()));

        }
        #region GdyPrzeslanoZlecenieCiecia
        [Test]
        public void GdyPrzeslanoZlecenieCiecia_PobieraZBazyListeZlecenTowarow()
        {
            MessengerSend(new tblProdukcjaZlecenieCiecia { IDProdukcjaZlecenieCiecia = 1 });

            tblProdukcjaZlecenieTowar.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieTowar,bool>>>()));

        }
        #endregion
        #endregion

        #region LoadCommand
        [Test]
        public void LoadCommandExecute_LadujeWszystkieListy()
        {
            sut.LoadCommand.Execute(null);

            tblProdukcjaZlecenieTowar.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieTowar,bool>>>()));

        }
        #endregion

        #region EdytujCommandExecute
        [Test]
        public void EdytujCommandExecute_OtwieraOkno()
        {
            sut.EdytujCommand.Execute(null);

            ViewService.Verify(v => v.Show<ZlecenieCieciaNaglowekViewModel_old>());
        }
        [Test]
        public void EdytujCommandExecute_WysylaMessage()
        {
            sut.WybraneZlecenie = new tblProdukcjaZlecenieTowar
            {
                IDProdukcjaZlecenieTowar = 1,
                tblProdukcjaZlecenieCiecia = new db.tblProdukcjaZlecenieCiecia
                {
                    IDProdukcjaZlecenieCiecia = 1
                }
            };

            sut.EdytujCommand.Execute(null);

            Messenger.Verify(v => v.Send(It.IsAny<tblProdukcjaZlecenieCiecia>()));
        }

        [Test]
        public void EdytujCommandExecute_GdyWybraneZlecenieNull_NieWysylaMessage()
        {
            sut.EdytujCommand.Execute(null);

            Messenger.Verify(v => v.Send(It.IsAny<tblProdukcjaZlecenieCiecia>()),Times.Never);
        }
        #endregion

        #region DodajCommandExecute
        [Test]
        public void DodajCommandExecute_OtwieraOkno()
        {
            sut.DodajCommand.Execute(null);

            ViewService.Verify(v => v.Show<ZlecenieCieciaNaglowekViewModel_old>());
        }


        #endregion
    }
}
