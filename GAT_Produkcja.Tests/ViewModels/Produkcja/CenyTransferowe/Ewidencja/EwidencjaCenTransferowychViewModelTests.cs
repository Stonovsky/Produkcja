using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.CenyTransferowe.Dodaj;
using GAT_Produkcja.ViewModel.Produkcja.CenyTransferowe.Ewidencja;
using GAT_Produkcja.ViewModel.Produkcja.CenyTransferowe.Message;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.CenyTransferowe.Ewidencja
{
    public class EwidencjaCenTransferowychViewModelTests : TestBase
    {
        private EwidencjaCenTransferowychViewModel sut;
        private Mock<ITblProdukcjaRozliczenie_CenyTransferoweRepository> tblProdukcjaRozliczenie_CenyTransferowe;

        public override void SetUp()
        {
            base.SetUp();

            tblProdukcjaRozliczenie_CenyTransferowe = new Mock<ITblProdukcjaRozliczenie_CenyTransferoweRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaRozliczenie_CenyTransferowe).Returns(tblProdukcjaRozliczenie_CenyTransferowe.Object);

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new EwidencjaCenTransferowychViewModel(ViewModelService.Object);
        }

        #region Messengers
        [Test]
        public void RejestracjaMessengerow()
        {
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<ZmianaCenTrasferowychMessage>>(), It.IsAny<bool>()));

        }
        #endregion

        #region LoadCommand
        [Test]
        public void LoadCommanExecute_LadujeZBazy()
        {
            sut.LoadCommand.Execute(null);

            tblProdukcjaRozliczenie_CenyTransferowe.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRozliczenie_CenyTransferowe, bool>>>()));
        }

        #endregion

        #region ZmienCenyTransferoweCommand
        [Test]
        public void ZmienCenyTransferoweCommandExecute_UruchamiaFormularzDodawaniaCenTransferowych()
        {
            sut.ZmienCenyTransferoweCommand.Execute(null);

            ViewService.Verify(v => v.Show<DodajCenyTransferoweViewModel>());
        }
        #endregion
    }
}
