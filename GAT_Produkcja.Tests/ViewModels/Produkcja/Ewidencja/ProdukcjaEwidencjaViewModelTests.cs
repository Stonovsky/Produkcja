using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.dbMsAccess.Repositories;
using GAT_Produkcja.dbMsAccess.UnitOfWork;
using GAT_Produkcja.Startup;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.Ewidencja;
using GAT_Produkcja.ViewModel.Produkcja.Ewidencja.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.Ewidencja.State;
using GAT_Produkcja.ViewModel.Produkcja.Ewidencja.State.Factory;
using GAT_Produkcja.ViewModel.Produkcja.Ewidencja.State.MsAccess;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.Ewidencja
{
    public class ProdukcjaEwidencjaViewModelTests : TestBase
    {
        private ProdukcjaEwidencjaViewModel sut;
        private Mock<IKonfekcjaRepository> Konfekcja;
        private Mock<IProdukcjaRepository> Produkcja;
        private Mock<IKalanderRepository> Kalander;
        private Mock<IProdukcjaEwidencjaSQLStateFactory> produkcjaEwidencjaStateFactory;
        private Mock<IProdukcjaEwidencjaSQLState> state;
        private Mock<ILiniaWlokninProdukcjaEwidencjaSQLState> liniaWlokninProdukcjaEwidencjaState;
        private Mock<IUnitOfWorkMsAccess> UnitOfWorkMsAccess;
        private Mock<IProdukcjaEwidencjaHelper> helper;
        private Mock<IRozliczenieMsAccesHelper> rozliczenieMsAccesHelper;
        private Mock<ITblProdukcjaRuchTowarRepository> tblProdukcjaRuchTowar;

        public override void SetUp()
        {
            base.SetUp();

            produkcjaEwidencjaStateFactory = new Mock<IProdukcjaEwidencjaSQLStateFactory>();
            UnitOfWorkMsAccess = new Mock<IUnitOfWorkMsAccess>();
            helper = new Mock<IProdukcjaEwidencjaHelper>();
            
            state = new Mock<IProdukcjaEwidencjaSQLState>();
            produkcjaEwidencjaStateFactory.Setup(s => s.PobierzStan(It.IsAny<int>()))
                                          .Returns(state.Object);

            liniaWlokninProdukcjaEwidencjaState = new Mock<ILiniaWlokninProdukcjaEwidencjaSQLState>();
            produkcjaEwidencjaStateFactory.Setup(s => s.PobierzStan(It.IsAny<int>()))
                                          .Returns(liniaWlokninProdukcjaEwidencjaState.Object);

            rozliczenieMsAccesHelper = new Mock<IRozliczenieMsAccesHelper>();
            helper.Setup(s => s.RozliczenieMsAccesHelper).Returns(rozliczenieMsAccesHelper.Object);

            Produkcja = new Mock<IProdukcjaRepository>();
            UnitOfWorkMsAccess.Setup(s => s.Produkcja).Returns(Produkcja.Object);

            Kalander = new Mock<IKalanderRepository>();
            UnitOfWorkMsAccess.Setup(s => s.Kalander).Returns(Kalander.Object);

            Konfekcja = new Mock<IKonfekcjaRepository>();
            UnitOfWorkMsAccess.Setup(s => s.Konfekcja).Returns(Konfekcja.Object);

            tblProdukcjaRuchTowar = new Mock<ITblProdukcjaRuchTowarRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaRuchTowar).Returns(tblProdukcjaRuchTowar.Object);
            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new ProdukcjaEwidencjaViewModel(ViewModelService.Object, produkcjaEwidencjaStateFactory.Object);
        }


        #region Poroperties
        #region ZaznaczonyTabItem
        [Test]
        public void ZaznaczonyTabItem_GdyZmianaTabItema_PobieraNowyStan()
        {
            sut.ZaznaczonyTabItem = 1;

            produkcjaEwidencjaStateFactory.Verify(v => v.PobierzStan(It.IsAny<int>()));
        }
        #endregion
        #endregion

        #region LoadCommand
        [Test]
        public void LoadCommandExecute_UruchamiaLoadAsyncNaState()
        {
            sut.State = state.Object;

            sut.LoadCommand.Execute(null);

            state.Verify(x => x.LoadAsync());
        }
        #endregion

        #region SzukajCommand
        /// <summary>
        /// Metoda LoadAsync powinna byc wywolana za kazdym razem, gdy naciskamy szukaj poniewaz przy zmianie stanu (State) moge nie byc podczytane wszystkie niezbedne dane
        /// </summary>
        [Test]
        public void SzukajCommandExecute_MetodaLoadAsyncJestWywolywana()
        {
            sut.State = state.Object;

            sut.SzukajCommand.Execute(null);

            state.Verify(v => v.LoadAsync());

        }
        [Test]
        public void SzukajCommandExecute_MetodyStatesSaWykonywane()
        {
            sut.State = state.Object;

            sut.SzukajCommand.Execute(null);

            state.Verify(v => v.PobierzListeRolek());

        }
        #endregion
    }
}
