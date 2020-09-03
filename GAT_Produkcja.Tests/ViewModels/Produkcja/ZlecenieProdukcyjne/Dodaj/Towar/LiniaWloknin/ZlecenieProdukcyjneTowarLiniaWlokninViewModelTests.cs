using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieDodajTowar.Messages;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Towar.LiniaWloknin;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.ZlecenieProdukcyjne.Dodaj.Towar.LiniaWloknin
{
    class ZlecenieProdukcyjneTowarLiniaWlokninViewModelTests
        : TestBaseGeneric<ZlecenieProdukcyjneTowarLiniaWlokninViewModel>
    {
        public override void SetUp()
        {
            base.SetUp();

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new ZlecenieProdukcyjneTowarLiniaWlokninViewModel(ViewModelService.Object);
        }
        #region Messenger
        [Test]
        public void RejestracjaMessengerow()
        {
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<tblProdukcjaZlecenieTowar>>(), It.IsAny<bool>()));
        }
        #region GdyPrzeslanoTowarDoDodania
        [Test]
        public void GdyPrzeslanoDodajMessage_GdyGniazdoToLiniaWloknin_DodajeDoPola_OstatnioDodanyTowar()
        {
            MessengerSend(new ProdukcjaZlecenieDodajTowarMessage
            {
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Dodaj,
                GniazdaProdukcyjneEnum = GniazdaProdukcyjneEnum.LiniaWloknin,
                ZlecenieTowar = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 1 },
            });


            Assert.IsNotNull(sut.OstatnioDodanyTowar);
            Assert.AreEqual(1, sut.OstatnioDodanyTowar.IDProdukcjaZlecenieTowar);
        }
        #endregion
        #endregion

        #region UpdateEntityBeforeSave

        [Test]
        public void UpdateEntityBeforeSave_PrzedZapisem_PrzypisujeIDZleceniaProdukcyjnego()
        {
            sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{},
            };

            sut.SaveAsync(1);

            Assert.AreEqual(1, sut.ListOfVMEntities.First().IDProdukcjaZlecenie);
        }


        [Test]
        public void UpdateEntityBeforeSave_PrzedZapisem_PrzypisujeGniazdoProdukcyjne()
        {
            sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{},
            };

            sut.SaveAsync(1);

            Assert.AreEqual((int)GniazdaProdukcyjneEnum.LiniaWloknin, sut.ListOfVMEntities.First().IDProdukcjaGniazdoProdukcyjne);
        }

        [Test]
        public void UpdateEntityBeforeSave_PrzedZapisem_PrzypisujeStatusZlecenia()
        {
            sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{},
            };

            sut.SaveAsync(1);

            Assert.AreEqual((int)ProdukcjaZlecenieStatusEnum.Oczekuje, sut.ListOfVMEntities.First().IDProdukcjaZlecenieStatus);
        }
        #endregion
    }
}
