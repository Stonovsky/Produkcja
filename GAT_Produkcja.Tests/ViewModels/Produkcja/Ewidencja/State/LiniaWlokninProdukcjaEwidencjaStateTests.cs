using GAT_Produkcja.db;
using GAT_Produkcja.db.EntitesInterfaces;
using GAT_Produkcja.dbMsAccess.EntitesInterfaces;
using GAT_Produkcja.dbMsAccess.Repositories;
using GAT_Produkcja.dbMsAccess.UnitOfWork;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.Ewidencja.Helpers;
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

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.Ewidencja.State
{
    public class LiniaWlokninProdukcjaEwidencjaStateTests : TestBase
    {
        private Mock<IUnitOfWorkMsAccess> unitOfWorkMsAccess;
        private Mock<IProdukcjaEwidencjaHelper> helper;
        private Mock<IProdukcjaRepository> produkcja;
        private Mock<IRozliczenieMsAccesHelper> rozliczenieMsAccessHelper;
        private LiniaWlokninProdukcjaEwidencjaState sut;

        public override void SetUp()
        {
            base.SetUp();


            unitOfWorkMsAccess = new Mock<IUnitOfWorkMsAccess>();
            helper = new Mock<IProdukcjaEwidencjaHelper>();

            produkcja = new Mock<IProdukcjaRepository>();
            unitOfWorkMsAccess.Setup(s => s.Produkcja).Returns(produkcja.Object);
            rozliczenieMsAccessHelper = new Mock<IRozliczenieMsAccesHelper>();
            helper.Setup(s => s.RozliczenieMsAccesHelper).Returns(rozliczenieMsAccessHelper.Object);

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new LiniaWlokninProdukcjaEwidencjaState(unitOfWorkMsAccess.Object, helper.Object);
        }


        #region PobierzListeRolekZMsAccess
        [Test]
        public void PobierzListeRolekZMsAccess_PobieraRolkiZLiniWloknin()
        {
            sut.PobierzListeRolekZMsAccess();

            produkcja.Verify(v => v.GetByDateAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()));
        }
        #endregion

        #region LoadAsync
        [Test]
        public async Task LoadAsync_LadujeRozliczenieMsAccessHelper()
        {
            await sut.LoadAsync();

            helper.Verify(v => v.RozliczenieMsAccesHelper.LoadAsync());
        }
        #endregion

        #region PodsumujListe
        [Test]
        public void PodsumujListe_GdyListaNiepusta_GenerujePodsumowanie()
        {
            sut.ListaZgrupowanychPozycjiLiniWloknin = new List<tblProdukcjaRozliczenie_PW>
            {
                new tblProdukcjaRozliczenie_PW{Ilosc=1,Ilosc_szt=1,Ilosc_kg=1, Wartosc=1, WartoscOdpad=1},
                new tblProdukcjaRozliczenie_PW{Ilosc=1,Ilosc_szt=1,Ilosc_kg=1, Wartosc=1, WartoscOdpad=1},
                new tblProdukcjaRozliczenie_PW{Ilosc=1,Ilosc_szt=1,Ilosc_kg=1, Wartosc=1, WartoscOdpad=1},
            };

            sut.PodsumujListe();

            Assert.AreEqual(3, sut.Podsumowanie.IloscSuma);
            Assert.AreEqual(3, sut.Podsumowanie.IloscSztSuma);
        }

        [Test]
        public void PodsumujListe_GdyPusta_ZwracaNowyObiekt()
        {
            sut.ListaZgrupowanychPozycjiLiniWloknin = new List<tblProdukcjaRozliczenie_PW>
            {
                new tblProdukcjaRozliczenie_PW{Ilosc=1,Ilosc_szt=1,Ilosc_kg=1, Wartosc=1, WartoscOdpad=1},
                new tblProdukcjaRozliczenie_PW{Ilosc=1,Ilosc_szt=1,Ilosc_kg=1, Wartosc=1, WartoscOdpad=1},
                new tblProdukcjaRozliczenie_PW{Ilosc=1,Ilosc_szt=1,Ilosc_kg=1, Wartosc=1, WartoscOdpad=1},
            };

            sut.PodsumujListe();

            Assert.IsNotNull(sut.Podsumowanie);
        }
        #endregion

        #region GrupujTowary
        [Test]
        public void GrupujTowary_GdyListaNiepusta_Grupuje()
        {
            sut.ListaPozycjiWloknin = new ObservableCollection<IGniazdoProdukcyjne>
            {
                new dbMsAccess.Models.Produkcja{Artykul="A1", IloscM2=1},
                new dbMsAccess.Models.Produkcja{Artykul="A1", IloscM2=1},
                new dbMsAccess.Models.Produkcja{Artykul="A1", IloscM2=1},
            };
            //helper.Setup(s => s.RozliczenieMsAccesHelper.GenerujRozliczeniePW(It.IsAny<IEnumerable<IProdukcjaRuchTowar>>(), It.IsAny<decimal>()));

            sut.GrupujTowary();

            //helper.Verify(v => v.RozliczenieMsAccesHelper.GenerujRozliczeniePW(It.IsAny<IEnumerable<IProdukcjaRuchTowar>>(), It.IsAny<decimal>()));
            helper.Verify(v => v.RozliczenieMsAccesHelper.PodsumujPWPodzialTowar(It.IsAny<IEnumerable<tblProdukcjaRozliczenie_PW>>()));
        }

        [Test]
        public void GrupujTowary_GdyPusta_Wyjatek()
        {
            Assert.ThrowsAsync<ArgumentException>(() => sut.GrupujTowary());
        }

        #endregion

        #region FiltrujTowar
        [Test]
        public void FiltrujTowar_GdyFiltrNiepusty_ListaFiltrowana()
        {
            produkcja.Setup(s => s.GetByDateAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                     .ReturnsAsync(new List<dbMsAccess.Models.Produkcja>
                        {
                            new dbMsAccess.Models.Produkcja{Artykul="A", ZlecenieID=1},
                            new dbMsAccess.Models.Produkcja{Artykul="B", ZlecenieID=1},
                            new dbMsAccess.Models.Produkcja{Artykul="C", ZlecenieID=1},
                        });
            rozliczenieMsAccessHelper.Setup(s => s.PodsumujPWPodzialTowar(It.IsAny<IEnumerable<tblProdukcjaRozliczenie_PW>>()))
                                      .Returns(new List<tblProdukcjaRozliczenie_PW>
                                        {
                                            new tblProdukcjaRozliczenie_PW {NazwaTowaruSubiekt = "A"},
                                            new tblProdukcjaRozliczenie_PW {NazwaTowaruSubiekt = "a"},
                                            new tblProdukcjaRozliczenie_PW {NazwaTowaruSubiekt = "B"},
                                            new tblProdukcjaRozliczenie_PW {NazwaTowaruSubiekt = "C"},
                                        });
            sut.PobierzListeRolekZMsAccess();
            sut.TowarNazwaFiltr = "a";

            sut.GrupujTowary();

            Assert.AreEqual(2, sut.ListaZgrupowanychPozycjiLiniWloknin.Count());
        }

        #endregion

    }
}
