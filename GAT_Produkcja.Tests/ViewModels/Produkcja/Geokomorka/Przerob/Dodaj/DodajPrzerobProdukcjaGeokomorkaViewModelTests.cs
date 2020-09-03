using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.Geokomorka.Przerob.DodajPrzerob;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.Geokomorka.Przerob.Dodaj
{
    public class DodajPrzerobProdukcjaGeokomorkaViewModelTests : TestBase
    {
        private DodajPrzerobProdukcjaGeokomorkaViewModel sut;
        private Mock<ITblProdukcjaGeokomorkaPodsumowaniePrzerobRepository> tblProdukcjaGeokomorkaPodsumowaniePrzerob;

        public override void SetUp()
        {
            base.SetUp();

            tblProdukcjaGeokomorkaPodsumowaniePrzerob = new Mock<ITblProdukcjaGeokomorkaPodsumowaniePrzerobRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaGeokomorkaPodsumowaniePrzerob).Returns(tblProdukcjaGeokomorkaPodsumowaniePrzerob.Object);

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new DodajPrzerobProdukcjaGeokomorkaViewModel(ViewModelService.Object);
        }

        #region Messenger

        #region GetElementByIdAsync
        [Test]
        public void GetElementByIdAsync_GdyPrzeslanoElementUsuniety_NiePrzypisujDanychDoWlasciwosci()
        {
            MessengerSend(new tblProdukcjaGeokomorkaPodsumowaniePrzerob { IdProdukcjaGeokomorkaPodsumowaniePrzerob = 1 });
            tblProdukcjaGeokomorkaPodsumowaniePrzerob.Setup(s => s.GetByIdAsync(It.IsAny<int>()));
            var przerob = new tblProdukcjaGeokomorkaPodsumowaniePrzerob();
            Assert.DoesNotThrow(() => sut.GetIdFromEntityWhenSentByMessenger(przerob));
        
        }

        [Test]
        public async Task GetElementByIdAsync_GdyPrzeslanoElementDoEdycji_AktualizujWlasciwosciZElementemPobranymZBazy()
        {
            tblProdukcjaGeokomorkaPodsumowaniePrzerob.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new db.tblProdukcjaGeokomorkaPodsumowaniePrzerob
            {
                IdProdukcjaGeokomorkaPodsumowaniePrzerob = 1,
                IloscWyprodukowana_kg =1,
                IloscNawrot_kg=1,
            });
            MessengerSend(new tblProdukcjaGeokomorkaPodsumowaniePrzerob { IdProdukcjaGeokomorkaPodsumowaniePrzerob = 1 });

            await sut.LoadAsync();

            Assert.AreEqual(1, sut.VMEntity.IloscWyprodukowana_kg);
            Assert.AreEqual(1, sut.VMEntity.IloscNawrot_kg);
        }

        #endregion

        #endregion
    }
}
