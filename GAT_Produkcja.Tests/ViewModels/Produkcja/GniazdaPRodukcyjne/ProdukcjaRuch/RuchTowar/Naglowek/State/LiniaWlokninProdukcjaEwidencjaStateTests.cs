using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.Ewidencja.State;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Naglowek;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Naglowek.States;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Naglowek.State
{
    public class LiniaWlokninProdukcjaEwidencjaStateTests : TestBaseGeneric<GPRuchNaglowekLiniaWlokninState>
    {
        private Mock<IGPRuchNaglowekViewModel> naglowekVM;

        public override void SetUp()
        {
            base.SetUp();

            naglowekVM = new Mock<IGPRuchNaglowekViewModel>();

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new GPRuchNaglowekLiniaWlokninState(naglowekVM.Object);
        }
    }
}
