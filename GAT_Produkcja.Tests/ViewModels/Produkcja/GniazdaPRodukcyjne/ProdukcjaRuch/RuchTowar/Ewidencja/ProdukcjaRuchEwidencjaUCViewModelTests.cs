using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Ewidencja;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Ewidencja
{
    public class ProdukcjaRuchEwidencjaUCViewModelTests : TestBaseGeneric<ProdukcjaRuchEwidencjaUCViewModel>
    {
        private Mock<ITblProdukcjaRuchNaglowekRepository> tblProdukcjaRuchNaglowek;
        private Mock<ITblProdukcjaRuchTowarRepository> tblProdukcjaRuchTowar;
        private Mock<ITblProdukcjaRuchTowarBadaniaRepository> tblProdukcjaRuchTowarBadania;

        public override void SetUp()
        {
            base.SetUp();

            tblProdukcjaRuchNaglowek = new Mock<ITblProdukcjaRuchNaglowekRepository>();
            tblProdukcjaRuchTowar = new Mock<ITblProdukcjaRuchTowarRepository>();
            tblProdukcjaRuchTowarBadania = new Mock<ITblProdukcjaRuchTowarBadaniaRepository>();
            
            UnitOfWork.Setup(s => s.tblProdukcjaRuchNaglowek).Returns(tblProdukcjaRuchNaglowek.Object);
            UnitOfWork.Setup(s => s.tblProdukcjaRuchTowar).Returns(tblProdukcjaRuchTowar.Object);
            UnitOfWork.Setup(s => s.tblProdukcjaRuchTowarBadania).Returns(tblProdukcjaRuchTowarBadania.Object);


            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new ProdukcjaRuchEwidencjaUCViewModel(ViewModelService.Object);
        }


        #region UsunNaglowekIRuchPowiazanyCommand

        #region CanExecute

        [Test]
        public void UsunNaglowekIRuchPowiazanyCommandExecute_GdyBrakZaznaczonegoVMEntity_False()
        {
            var expected = sut.UsunNaglowekIRuchPowiazanyCommand.CanExecute(null);

            Assert.IsFalse(expected);
        }

        [Test]
        public void UsunNaglowekIRuchPowiazanyCommandExecute_GdyBrakZaznaczonoVMEntity_True()
        {
            UsunNaglowekCanExecute_True();

            var expected = sut.UsunNaglowekIRuchPowiazanyCommand.CanExecute(null);

            Assert.IsTrue(expected);
        }

        #endregion

        private void UsunNaglowekCanExecute_True()
        {
            sut.SelectedVMEntity = new tblProdukcjaRuchTowar() { IDProdukcjaRuchNaglowek = 1 };
        }

        #region Execute
        [Test]
        public void UsunNaglowekIRuchPowiazanyCommandExecute_GdyNaglowekNull_NicNieRobi()
        {
            UsunNaglowekCanExecute_True();

            sut.UsunNaglowekIRuchPowiazanyCommand.Execute(null);

            UnitOfWork.Verify(v => v.SaveAsync(), Times.Never);
        }

        [Test]
        public void UsunNaglowekIRuchPowiazanyCommandExecute_GdyNaglowekNieNull_UOWRemove()
        {
            UsunNaglowekCanExecute_True();
            tblProdukcjaRuchNaglowek.Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                                    .ReturnsAsync(new tblProdukcjaRuchNaglowek());

            sut.UsunNaglowekIRuchPowiazanyCommand.Execute(null);

            tblProdukcjaRuchNaglowek.Verify(v => v.Remove(It.IsAny<tblProdukcjaRuchNaglowek>()));
            UnitOfWork.Verify(v => v.SaveAsync());
        }

        [Test]
        public void UsunNaglowekIRuchPowiazanyCommandExecute_GdyBrakRuchuTowarowWBazie_UOWRemoveRangeNieWywolywane()
        {
            UsunNaglowekCanExecute_True();
            tblProdukcjaRuchNaglowek.Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                                    .ReturnsAsync(new tblProdukcjaRuchNaglowek());

            sut.UsunNaglowekIRuchPowiazanyCommand.Execute(null);

            tblProdukcjaRuchTowar.Verify(x => x.RemoveRange(It.IsAny<IEnumerable<tblProdukcjaRuchTowar>>()),Times.Never);
            UnitOfWork.Verify(x => x.SaveAsync());
        }

        [Test]
        public void UsunNaglowekIRuchPowiazanyCommandExecute_GdyBrakBadanTowarowWBazie_UOWRemoveRangeNieWywolywane()
        {
            UsunNaglowekCanExecute_True();
            tblProdukcjaRuchNaglowek.Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                                    .ReturnsAsync(new tblProdukcjaRuchNaglowek());

            sut.UsunNaglowekIRuchPowiazanyCommand.Execute(null);

            tblProdukcjaRuchTowarBadania.Verify(x => x.RemoveRange(It.IsAny<IEnumerable<tblProdukcjaRuchTowarBadania>>()), Times.Never);
            UnitOfWork.Verify(x => x.SaveAsync());
        }
        #endregion

        #endregion

        #region Edit
        [Test]
        public void EditCommandExecute_WysylaMessage()
        {
            sut.SelectedVMEntity = new tblProdukcjaRuchTowar();

            sut.EditCommand.Execute(null);

            Messenger.Verify(x => x.Send(sut.SelectedVMEntity.tblProdukcjaRuchNaglowek));
        }
        #endregion
    }
}
