using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Helpers;
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
    public class PodsumowanieNaleznosciIZobowiazaniaHelperTests : TestBase
    {
        private PodsumowanieNaleznosciIZobowiazaniaHelper sut;
        private Mock<IVwFinanseNalZobGTX2Repository> vwFinanseNalZobGTX2;
        private Mock<IVwFinanseNalZobAGGRepository> vwFinanseNalZobAGG;
        private Mock<IVwFinanseNalZobGTXRepository> vwFinanseNalZobGTX;

        public override void SetUp()
        {
            base.SetUp();

            vwFinanseNalZobAGG = new Mock<IVwFinanseNalZobAGGRepository>();
            UnitOfWork.Setup(s => s.vwFinanseNalZobAGG).Returns(vwFinanseNalZobAGG.Object);

            vwFinanseNalZobGTX = new Mock<IVwFinanseNalZobGTXRepository>();
            UnitOfWork.Setup(s => s.vwFinanseNalZobGTX).Returns(vwFinanseNalZobGTX.Object);

            vwFinanseNalZobGTX2 = new Mock<IVwFinanseNalZobGTX2Repository>();
            UnitOfWork.Setup(s => s.vwFinanseNalZobGTX2).Returns(vwFinanseNalZobGTX2.Object);

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new PodsumowanieNaleznosciIZobowiazaniaHelper(UnitOfWork.Object);
        }

        [Test]
        public void PobierzPodsumowanieNalzenosciIZobowiazan_PobieraDaneZBazy()
        {
            var podsumowanie = sut.PobierzPodsumowanieNalzenosciIZobowiazan(DateTime.Now.Date);

            //vwFinanseNalZobAGG.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<vwFinanseNalZobAGG, bool>>>()));
            //vwFinanseNalZobGTX.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<vwFinanseNalZobGTX, bool>>>()));
            //vwFinanseNalZobGTX2.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<vwFinanseNalZobGTX2, bool>>>()));            

            vwFinanseNalZobAGG.Verify(v => v.GetAllAsync());
            vwFinanseNalZobGTX.Verify(v => v.GetAllAsync());
            vwFinanseNalZobGTX2.Verify(v => v.GetAllAsync());
        }


        [Test]
        public async Task PobierzPodsumowanieNalzenosciIZobowiazan_PodsumowaujeWlasciwiePrzeslaneDane()
        {
            vwFinanseNalZobAGG.Setup(s => s.GetAllAsync())
                              .ReturnsAsync(new List<vwFinanseNalZobAGG>
                              {
                                  new vwFinanseNalZobAGG{Naleznosc=1,Zobowiazanie=1,TerminPlatnosci=DateTime.Now.Date.AddDays(0)},
                                  new vwFinanseNalZobAGG{Naleznosc=1,Zobowiazanie=1,TerminPlatnosci=DateTime.Now.Date.AddDays(1)},
                              });
            vwFinanseNalZobGTX.Setup(s => s.GetAllAsync())
                              .ReturnsAsync(new List<vwFinanseNalZobGTX>
                              {
                                    new vwFinanseNalZobGTX{Naleznosc=1,Zobowiazanie=1, TerminPlatnosci=DateTime.Now.Date.AddDays(0)},
                                    new vwFinanseNalZobGTX{Naleznosc=1,Zobowiazanie=1, TerminPlatnosci=DateTime.Now.Date.AddDays(1)},
                              });
            vwFinanseNalZobGTX2.Setup(s => s.GetAllAsync())
                              .ReturnsAsync(new List<vwFinanseNalZobGTX2>
                              {
                                    new vwFinanseNalZobGTX2{Naleznosc=1,Zobowiazanie=1, TerminPlatnosci=DateTime.Now.Date.AddDays(0)},
                                    new vwFinanseNalZobGTX2{Naleznosc=1,Zobowiazanie=1, TerminPlatnosci=DateTime.Now.Date.AddDays(1)},
                              });

            var podsumowanie = await sut.PobierzPodsumowanieNalzenosciIZobowiazan(DateTime.Now.Date);

            Assert.AreEqual(3, podsumowanie.Count());
            Assert.AreEqual("AG Geosynthetics sp. z o.o. sp. k.", podsumowanie.First().Firma);
            Assert.AreEqual("GTEX2 sp. z o.o.", podsumowanie.Last().Firma);
            Assert.AreEqual(1, podsumowanie.Last().NaleznosciDoDaty);
            Assert.AreEqual(2, podsumowanie.Last().NaleznosciAll);
        }

    }
}
