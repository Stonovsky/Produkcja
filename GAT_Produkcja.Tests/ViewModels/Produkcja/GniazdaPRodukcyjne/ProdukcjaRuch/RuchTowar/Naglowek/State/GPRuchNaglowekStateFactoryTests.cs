using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Naglowek;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Naglowek.States;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Naglowek.State
{
    public class GPRuchNaglowekStateFactoryTests : TestBaseGeneric<GPRuchNaglowekStateFactory>
    {
        private Mock<IGPRuchNaglowekStateFactory> naglowekStateFactory;

        public override void SetUp()
        {
            base.SetUp();

            naglowekStateFactory = new Mock<IGPRuchNaglowekStateFactory>();

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new GPRuchNaglowekStateFactory();
        }

        #region GetState
        [Test]
        public void GetState_Gdy_RuchNaglowekViewModelJestNull_ZwracaLiniaWlokninState()
        {
            var vm = sut.GetState(null);

            Assert.AreEqual(typeof(GPRuchNaglowekLiniaWlokninState), vm.GetType());
        }

        [Test]
        public void GetState_Gdy_RuchNaglowekViewModelNieJestNull_AleWybraneGniazdoJestNull_ZwracaLiniaWlokninState()
        {
            var naglowekVM = new GPRuchNaglowekViewModel(ViewModelService.Object, null, null, null, naglowekStateFactory.Object);
            
            var vm = sut.GetState(naglowekVM);

            Assert.AreEqual(typeof(GPRuchNaglowekLiniaWlokninState), vm.GetType());
        }

        [Test]
        public void GetState_Gdy_RuchNaglowekViewJestOk_ZwracaOdpowiedniStan_LiniaKalandra()
        {
            var naglowekVM = new GPRuchNaglowekViewModel(ViewModelService.Object, null, null, null, naglowekStateFactory.Object);
            naglowekVM.WybraneGniazdo = new db.tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = 2 };
            
            var vm = sut.GetState(naglowekVM);

            Assert.AreEqual(typeof(GPRuchNaglowekKalanderState), vm.GetType());
        }

        [Test]
        public void GetState_Gdy_RuchNaglowekViewJestOk_ZwracaOdpowiedniStan_LiniaKonfekcji()
        {
            var naglowekVM = new GPRuchNaglowekViewModel(ViewModelService.Object, null, null, null, naglowekStateFactory.Object);
            naglowekVM.WybraneGniazdo = new db.tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = 3 };

            var vm = sut.GetState(naglowekVM);

            Assert.AreEqual(typeof(GPRuchNaglowekKonfekcjaState), vm.GetType());
        }

        #endregion
    }
}
