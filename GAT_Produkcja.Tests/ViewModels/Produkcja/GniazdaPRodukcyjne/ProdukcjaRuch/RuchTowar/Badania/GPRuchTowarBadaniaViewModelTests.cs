using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny.SprawdzenieWynikowBadan;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Badania;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Badania
{
    public class GPRuchTowarBadaniaViewModelTests : TestBaseGeneric<GPRuchTowarBadaniaViewModel>
    {
        private Mock<IWeryfikacjaGramaturyGeowlokninHelper> weryfikacjaGramatury;

        public override void SetUp()
        {
            base.SetUp();

            weryfikacjaGramatury = new Mock<IWeryfikacjaGramaturyGeowlokninHelper>();

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new GPRuchTowarBadaniaViewModel(ViewModelService.Object, weryfikacjaGramatury.Object);
        }

        #region LoadAsync
        [Test]
        public void LoadAsync_Wywoluje_Expectations()
        {
            sut.LoadAsync(null);

            weryfikacjaGramatury.Verify(v => v.LoadAsync());
        }

        #endregion


        #region GramaturaSrednia

        [Test]
        [TestCase(100, 0, 0, 100)]
        [TestCase(100, 90, 0, 95)]
        [TestCase(100, 90, 80, 90)]
        public void PrzeliczGramatureSrednia_GdyKtorasGramaturaNieZerowa_ObliczaGramatureSrednia(int gr1, int gr2, int gr3, decimal grsrednia)
        {
            sut.Gramatura1 = gr1;
            sut.Gramatura2 = gr2;
            sut.Gramatura3 = gr3;

            Assert.AreEqual(grsrednia, sut.VMEntity.GramaturaSrednia);
        }

        #endregion
    }
}
