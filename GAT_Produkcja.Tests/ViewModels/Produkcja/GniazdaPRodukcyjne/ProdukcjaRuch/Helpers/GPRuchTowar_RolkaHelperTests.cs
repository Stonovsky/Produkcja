using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Helpers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.Helpers
{
    public class GPRuchTowar_RolkaHelperTests : TestBase
    {
        private GPRuchTowar_RolkaHelper sut;
        private Mock<ITblProdukcjaRuchTowarRepository> tblProdukcjaRuchTowar;

        public override void SetUp()
        {
            base.SetUp();

            tblProdukcjaRuchTowar = new Mock<ITblProdukcjaRuchTowarRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaRuchTowar).Returns(tblProdukcjaRuchTowar.Object);

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new GPRuchTowar_RolkaHelper(UnitOfWork.Object);
        }

        #region PobierzKolejnyNrRolkiAsync - zwraca numer dla id=0 z itemu dodawanego do listy (cala reszta jest zapisywana w bazie -> autozapis)
        [Test]
        public async Task PobierzKolejnyNrRolkiAsync_GdyWBazieBrakRolek_ZwracaJeden()
        {
            var listaPW = new List<tblProdukcjaRuchTowar> { new db.tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 0 } };

            var nrRolki = await sut.PobierzKolejnyNrRolkiAsync(new tblProdukcjaGniazdoProdukcyjne
            {
                IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaWloknin
            },
            listaPW);

            Assert.AreEqual(1, nrRolki);
        }

        [Test]
        public async Task PobierzKolejnyNrRolkiAsync_GdyWBazieBrakRolekAleListaPosiadaItem_ZwracaWlasciwyNr()
        {
            var listaPW = new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar(){NrRolki=1},
            };


            var nrRolki = await sut.PobierzKolejnyNrRolkiAsync(new tblProdukcjaGniazdoProdukcyjne
            {
                IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaWloknin
            },
            listaPW);

            Assert.AreEqual(1, nrRolki);
        }

        [Test]
        public async Task PobierzKolejnyNrRolkiAsync_GdyWBazieSaRolkiIListaPosiadaItemy_ZwracaWlasciwyNr()
        {
            tblProdukcjaRuchTowar.Setup(s => s.GetNewNumberAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>(), It.IsAny<Func<tblProdukcjaRuchTowar, int>>()))
                                 .ReturnsAsync(1);

            var listaPW = new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar(){NrRolki=2},
            };


            var nrRolki = await sut.PobierzKolejnyNrRolkiAsync(new tblProdukcjaGniazdoProdukcyjne
            {
                IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaWloknin
            },
            listaPW);

            Assert.AreEqual(2, nrRolki);
        }
        #endregion

        #region Odpad
        [Test]
        public async Task PobierzOdpadZRolkiAsync_GdyDaneOkIGniazdoKonfekcji_LiczyOdpadBezTolerancji()
        {
            tblProdukcjaRuchTowar.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new tblProdukcjaRuchTowar {IDProdukcjaGniazdoProdukcyjne=1, Waga_kg = 100 });

            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()))
                                 .ReturnsAsync(new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaGniazdoProdukcyjne=1, Waga_kg=10},
                new tblProdukcjaRuchTowar{IDProdukcjaGniazdoProdukcyjne=1,  Waga_kg=20},
                new tblProdukcjaRuchTowar{IDProdukcjaGniazdoProdukcyjne=1,  Waga_kg=50},
            });

            var result = await sut.PobierzOdpadZRolkiRwAsync(1);

            Assert.AreEqual(20, result);
        }

        [Test]
        public async Task PobierzOdpadZRolkiAsync_GdyDaneOkIGniazdoKalander_LiczyOdpadZTolerancja()
        {
            tblProdukcjaRuchTowar.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new tblProdukcjaRuchTowar { IDProdukcjaGniazdoProdukcyjne = 1, Waga_kg = 100 });

            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()))
                                 .ReturnsAsync(new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaGniazdoProdukcyjne=1, Waga_kg=98},
            });
            var gniazdo = new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKalandowania, WspZmniejszeniaMasy=0.035m };
            
            var result = await sut.PobierzOdpadZRolkiRwAsync(1,gniazdo);

            Assert.AreEqual(0, result);
        }


        [Test]
        public async Task PobierzOdpadZRolkiAsync_GdyBrakRolekPW_Wyjatek()
        {
            tblProdukcjaRuchTowar.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new tblProdukcjaRuchTowar { Waga_kg = 100 });

            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()));

            Assert.ThrowsAsync<ArgumentException>(() => sut.PobierzOdpadZRolkiRwAsync(1));
        }

        #endregion
    }
}
