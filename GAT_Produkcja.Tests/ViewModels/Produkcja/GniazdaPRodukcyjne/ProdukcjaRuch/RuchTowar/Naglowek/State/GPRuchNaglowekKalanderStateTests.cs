using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Naglowek;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Naglowek.States;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.RW;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Naglowek.State
{
    public class GPRuchNaglowekKalanderStateTests : TestBaseGeneric<GPRuchNaglowekKalanderState>
    {
        private Mock<IGPRuchNaglowekViewModel> naglowekVM;
        private Mock<IGPRuchTowarRWViewModel> vmRW;
        private Mock<IGPRuchTowarPWViewModel> vmPW;

        public override void SetUp()
        {
            base.SetUp();

            naglowekVM = new Mock<IGPRuchNaglowekViewModel>();
            vmRW = new Mock<IGPRuchTowarRWViewModel>();
            vmPW = new Mock<IGPRuchTowarPWViewModel>();

            naglowekVM.Setup(s => s.RuchTowarRWViewModel).Returns(vmRW.Object);
            naglowekVM.Setup(s => s.RuchTowarPWViewModel).Returns(vmPW.Object);

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new GPRuchNaglowekKalanderState(naglowekVM.Object);
        }

        #region Properties

        [Test]
        public void RwEnabled_True()
        {
            Assert.IsTrue(sut.RwEnabled);
        }
        [Test]
        public void CzyZlecProdMaBycWidoczne_True()
        {
            Assert.IsTrue(sut.CzyZlecProdMaBycWidoczne);
        }

        [Test]
        public void CzyZlecCieciaMaBycWidoczne_False()
        {
            Assert.IsFalse(sut.CzyZlecCieciaMaBycWidoczne);
        } 

        #region IsChanged
        [Test]
        [TestCase(false, false, false)]
        [TestCase(true, false, true)]
        [TestCase(false, true, true)]
        [TestCase(true, true, true)]
        public void IsChangedProp_WhenIsChangeFalseCheckIsChangedOnChildren(bool rwIsChanged, bool pwIsChanged, bool expected)
        {
            naglowekVM.Setup(s => s.RuchTowarRWViewModel.IsChanged).Returns(rwIsChanged);
            naglowekVM.Setup(s => s.RuchTowarPWViewModel.IsChanged).Returns(pwIsChanged);

            Assert.AreEqual(expected, sut.IsChanged);

        }


        #endregion

        #region IsValid
        [Test]
        [TestCase(true, true, true)]
        [TestCase(false, true, false)]
        public void IsValidProp_CheckIsValidOnChildrens(bool rwIsValid, bool pwIsValid, bool expected)
        {
            naglowekVM.Setup(s => s.IsValid).Returns(true);
            naglowekVM.Setup(s => s.RuchTowarRWViewModel.IsValid).Returns(rwIsValid);
            naglowekVM.Setup(s => s.RuchTowarPWViewModel.IsValid).Returns(pwIsValid);

            Assert.AreEqual(expected, sut.IsValid);
        }
        #endregion

        #endregion

    }
}
