using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Magazyn.Ewidencje.StanTowaru;
using GAT_Produkcja.ViewModel.Magazyn.RuchTowaru.Messages;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Magazyn.Ewidencje.StanTowaru
{
    [TestFixture]
    public class MagazynStanTowaruViewModelTests :TestBase
    {

        private Mock<IVwMagazynRuchGTXRepository> vwMagazynRuchGTX;
        private Mock<IVwMagazynGTXRepository> vwMagazynGTX;
        private Mock<IVwStanTowaruRepository> vwStanTowaru;
        private MagazynStanTowaruViewModel sut;

        public override void SetUp()
        {
            base.SetUp();

            vwMagazynRuchGTX = new Mock<IVwMagazynRuchGTXRepository>();
            UnitOfWork.Setup(s => s.vwMagazynRuchGTX).Returns(vwMagazynRuchGTX.Object);

            vwMagazynGTX = new Mock<IVwMagazynGTXRepository>();
            UnitOfWork.Setup(s => s.vwMagazynGTX).Returns(vwMagazynGTX.Object);

            vwStanTowaru = new Mock<IVwStanTowaruRepository>();
            UnitOfWork.Setup(s => s.vwStanTowaru).Returns(vwStanTowaru.Object);

            UnitOfWorkFactory.Setup(s => s.Create()).Returns(UnitOfWork.Object);

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new MagazynStanTowaruViewModel(ViewModelService.Object);
        }
        #region Messengers

        #region Rejestracja
        [Test]
        public void RejestacjaMessengerow()
        {
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<vwMagazynGTX>>(), It.IsAny<bool>()));

        }
        #endregion

        #region GdyWyslanoMagazyn
        [Test]
        public void GdyPrzeslanoMagazyn_MagazynNieNull_FiltrujeListeTowarowDlaPrzeslanegoMagazynu()
        {
            MessengerSend(new vwMagazynGTX { IdMagazyn = 1 });

            sut.LoadCommand.Execute(null);

            vwMagazynRuchGTX.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<vwMagazynRuchGTX,bool>>>()));
        }

        [Test]
        public void GdyPrzeslanoMagazyn_MagazynJestNull_NiePobierajZBazy()
        {
            MessengerSend((vwMagazynGTX)null);
            
            sut.LoadCommand.Execute(null);

            vwMagazynRuchGTX.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<vwMagazynRuchGTX, bool>>>()),Times.Never);
        }
        #endregion

        #endregion

        #region LoadCommand
        [Test]
        public void LoadCommandExecute_LadujeZBazyTabele()
        {
            sut.LoadCommand.Execute(null);

            vwMagazynRuchGTX.Verify(v => v.GetAllAsync());
            vwMagazynGTX.Verify(v => v.GetAllAsync());
        }
        #endregion

        #region WyslijTowarMessengerem
        [Test]
        public void WyslijTowarMessengeremCommandExecute_GdyKliknietoDwukrotnieNaTowarITowarNiepusty_WyslijMessengerem()
        {
            sut.WybranyTowar = new vwMagazynRuchGTX { IdMagazyn = 1 };

            sut.WyslijTowarMessengeremCommand.Execute(null);

            Messenger.Verify(v => v.Send(It.IsAny<vwMagazynRuchGTX>()));
        }
        [Test]
        public void WyslijTowarMessengeremCommandExecute_GdyKliknietoDwukrotnieNaTowarITowarPusty_NieWysylajMessage()
        {
            sut.WybranyTowar = null;

            sut.WyslijTowarMessengeremCommand.Execute(null);

            Messenger.Verify(v => v.Send(It.IsAny<vwMagazynRuchGTX>()),Times.Never);
        }
        #endregion

        #region ZmianaElementuNaTreeViewCommand
        [Test]
        public void ZmianaElementuNaTreeViewCommandExecute_GdyWybranyMagazynNull_NiePoierajZBazyTowarow()
        {
            sut.WybranyMagazyn = null;

            sut.ZmianaElementuNaTreeViewCommand.Execute(null);

            vwMagazynRuchGTX.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<vwMagazynRuchGTX, bool>>>()), Times.Never);

        }

        [Test]
        public void ZmianaElementuNaTreeViewCommandExecute_GdyWybranyMagazynNieNull_PoierajZBazyOdfiltrowaneTowary()
        {
            sut.WybranyMagazyn = new vwMagazynGTX();

            sut.ZmianaElementuNaTreeViewCommand.Execute(null);

            vwMagazynRuchGTX.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<vwMagazynRuchGTX, bool>>>()));

        }




        #endregion
    }
}
