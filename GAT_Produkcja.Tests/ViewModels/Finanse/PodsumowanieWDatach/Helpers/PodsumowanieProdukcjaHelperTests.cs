using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.dbMsAccess.EntitesInterfaces;
using GAT_Produkcja.dbMsAccess.Repositories;
using GAT_Produkcja.dbMsAccess.UnitOfWork;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Finanse.PodsumowanieWDatach.Helpers
{
    public class PodsumowanieProdukcjaHelperTests : TestBase
    {
        private Mock<IUnitOfWorkMsAccess> unitOfWorkMsAccess;
        private Mock<IRozliczenieMsAccesHelper> rozliczenieMsAccessHelper;
        private Mock<IProdukcjaRepository> produkcja;
        private PodsumowanieProdukcjaHelper sut;
        private Mock<ITblProdukcjaGeokomorkaPodsumowaniePrzerobRepository> tblProdukcjaGeokomorkaPodsumowaniePrzerob;

        public override void SetUp()
        {
            base.SetUp();
            unitOfWorkMsAccess = new Mock<IUnitOfWorkMsAccess>();
            rozliczenieMsAccessHelper = new Mock<IRozliczenieMsAccesHelper>();

            produkcja = new Mock<IProdukcjaRepository>();
            unitOfWorkMsAccess.Setup(s => s.Produkcja).Returns(produkcja.Object);


            tblProdukcjaGeokomorkaPodsumowaniePrzerob = new Mock<ITblProdukcjaGeokomorkaPodsumowaniePrzerobRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaGeokomorkaPodsumowaniePrzerob).Returns(tblProdukcjaGeokomorkaPodsumowaniePrzerob.Object);

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new PodsumowanieProdukcjaHelper(unitOfWorkMsAccess.Object, rozliczenieMsAccessHelper.Object, UnitOfWork.Object);
        }

        #region Geowloknina

        [Test]
        public async Task PobierzPodsumowanieProdukcjiWDatach_WywolujeMetodeZUoWMsAccessZWlasciwymiDatami()
        {
            var podsumowanie = await sut.PobierzPodsumowanieProdukcjiWDatach(DateTime.Now.Date.AddDays(-1), DateTime.Now.Date);

            produkcja.Verify(v => v.GetByDateAsync(DateTime.Now.Date.AddDays(-1), DateTime.Now.Date));
        }

        [Test]
        public async Task PobierzPodsumowanieProdukcjiWDatach_GdyProdukowano_ZwracaModelPodsumowania()
        {
            produkcja.Setup(s => s.GetByDateAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                     .ReturnsAsync(new List<dbMsAccess.Models.Produkcja>
                     {
                         new dbMsAccess.Models.Produkcja{Waga=1,WagaOdpadu=1,Artykul="T1"},
                         new dbMsAccess.Models.Produkcja{Waga=1,WagaOdpadu=1,Artykul="T2"},
                     });
            rozliczenieMsAccessHelper.Setup(s => s.GenerujZgrupowanaListePoNazwieZObliczeniemKosztow(It.IsAny<IEnumerable<IGniazdoProdukcyjne>>()))
                                     .ReturnsAsync(new List<tblProdukcjaRozliczenie_PW>
                                     {
                                         new tblProdukcjaRozliczenie_PW{Ilosc_kg=1, Ilosc=1,NazwaTowaruSubiekt="T1"},
                                         new tblProdukcjaRozliczenie_PW{Ilosc_kg=1, Ilosc=1,NazwaTowaruSubiekt="T2"},
                                     });

            var podsumowanie = await sut.PobierzPodsumowanieProdukcjiWDatach(DateTime.Now.Date, DateTime.Now.Date);

            Assert.IsNotNull(podsumowanie);
            Assert.IsNotEmpty(podsumowanie);
            Assert.AreEqual(2, podsumowanie.First().Ilosc_kg);
        }

        [Test]
        public async Task PobierzPodsumowanieProdukcjiWDatach_GdyBrakProdukcji_ZwracaNowyModelPodsumowania()
        {

            var podsumowanie = await sut.PobierzPodsumowanieProdukcjiWDatach(DateTime.Now.Date, DateTime.Now.Date);

            Assert.IsNotNull(podsumowanie);
            Assert.IsNotEmpty(podsumowanie);
            Assert.AreEqual(0, podsumowanie.First().Ilosc_kg);
        }
        #endregion



        #region Geokomorka

        [Test]
        public async Task PodsumowanieProdukcjiGeokomorek_GdyBrakWBazie_NieDodajeDoListy()
        {

            var listaPodsumowania = await sut.PobierzPodsumowanieProdukcjiWDatach(DateTime.Now.Date.AddDays(-1), DateTime.Now.Date);

            Assert.AreEqual(1, listaPodsumowania.Count());
            Assert.AreEqual(0, sut.Podsumowanie.Ilosc_kg);
            Assert.AreEqual(0, sut.Podsumowanie.Ilosc_m2);
            Assert.AreEqual(0, sut.Podsumowanie.Wartosc);
        }

        [Test]
        public async Task PodsumowanieProdukcjiGeokomorek_GdyPobranoZBazy_DodajeDoListy()
        {
            tblProdukcjaGeokomorkaPodsumowaniePrzerob
                .Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaGeokomorkaPodsumowaniePrzerob, bool>>>()))
                .ReturnsAsync(new List<tblProdukcjaGeokomorkaPodsumowaniePrzerob>
                {
                    new db.tblProdukcjaGeokomorkaPodsumowaniePrzerob { Ilosc_kg = 1, Wartosc = 1, Ilosc_m2 = 1 }
                });


            var listaPodsumowania = await sut.PobierzPodsumowanieProdukcjiWDatach(DateTime.Now.Date.AddDays(-1), DateTime.Now.Date);


            Assert.IsNotEmpty(listaPodsumowania);
            Assert.AreEqual(2, listaPodsumowania.Count());
            Assert.AreEqual(1, sut.Podsumowanie.Ilosc_kg);
            Assert.AreEqual(1, sut.Podsumowanie.Ilosc_m2);
            Assert.AreEqual(1, sut.Podsumowanie.Wartosc);
        }
        #endregion
    }
}
