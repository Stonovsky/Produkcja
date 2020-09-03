using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Helpers;
using GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Modele;
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
    public class PodsumowanieMagazynyHelperTests : TestBase
    {
        private PodsumowanieMagazynyHelper sut;
        private Mock<IVwMagazynRuchAGGRepository> vwMagazynRuchAGG;
        private Mock<IVwMagazynRuchGTXRepository> vwMagazynRuchGTX;
        private Mock<IVwMagazynRuchGTX2Repository> vwMagazynRuchGTX2;

        public override void SetUp()
        {
            base.SetUp();

            vwMagazynRuchAGG = new Mock<IVwMagazynRuchAGGRepository>();
            UnitOfWork.Setup(s => s.vwMagazynRuchAGG).Returns(vwMagazynRuchAGG.Object);

            vwMagazynRuchGTX = new Mock<IVwMagazynRuchGTXRepository>();
            UnitOfWork.Setup(s => s.vwMagazynRuchGTX).Returns(vwMagazynRuchGTX.Object);

            vwMagazynRuchGTX2= new Mock<IVwMagazynRuchGTX2Repository>();
            UnitOfWork.Setup(s => s.vwMagazynRuchGTX2).Returns(vwMagazynRuchGTX2.Object);

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new PodsumowanieMagazynyHelper(UnitOfWork.Object);
        }

        [Test]
        public async Task PobierzPodsumowanieMagazynuAGGDoDaty_GdyMagazynyNiepuste_PobieraZBazyDane()
        {
            var podsumowanieMagazynu = await sut.PobierzPodsumowanieMagazynuDoDaty(new vwMagazynRuchGTX2(), DateTime.Now.Date);

            vwMagazynRuchGTX2.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<vwMagazynRuchGTX2, bool>>>()));
        }

        [Test]
        public async Task PobierzPodsumowanieMagazynuDoDaty_GdyPrzeslanoTypMagazynuGTX2_PobieraZBazyDane()
        {
            var podsumowanieMagazynu = await sut.PobierzPodsumowanieMagazynuDoDaty(new vwMagazynRuchGTX2(), DateTime.Now.Date);

            vwMagazynRuchGTX2.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<vwMagazynRuchGTX2, bool>>>()));
        }

        [Test]
        public async Task PobierzPodsumowanieMagazynuDoDaty_GdyPrzeslanoTypMagazynuGTX_PobieraZBazyDane()
        {
            var podsumowanieMagazynu = await sut.PobierzPodsumowanieMagazynuDoDaty(new vwMagazynRuchGTX(), DateTime.Now.Date);

            vwMagazynRuchGTX.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<vwMagazynRuchGTX, bool>>>()));
        }
        [Test]
        public async Task PobierzPodsumowanieMagazynuDoDaty_GdyPrzeslanoTypMagazynuAGG_PobieraZBazyDane()
        {
            var podsumowanieMagazynu = await sut.PobierzPodsumowanieMagazynuDoDaty(new vwMagazynRuchAGG(), DateTime.Now.Date);

            vwMagazynRuchAGG.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<vwMagazynRuchAGG, bool>>>()));
        }
        [Test]
        public async Task PobierzPodsumowanieMagazynuDoDaty_GdyMagazynyNiepuste_GrupujeMagazynyWgNazwy()
        {
            vwMagazynRuchGTX2.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<vwMagazynRuchGTX2, bool>>>()))
                             .ReturnsAsync(new List<vwMagazynRuchGTX2>
                             {
                                 new vwMagazynRuchGTX2 {IdMagazyn=1, MagazynNazwa="M1", Pozostalo=1, Wartosc=1, Jm="m2"},
                                 new vwMagazynRuchGTX2 {IdMagazyn=1, MagazynNazwa="M1", Pozostalo=1, Wartosc=1, Jm="m2"},
                                 new vwMagazynRuchGTX2 {IdMagazyn=2, MagazynNazwa="M2", Pozostalo=1, Wartosc=1, Jm="m2"},
                             });

            var podsumowanieMagazynow = await sut.PobierzPodsumowanieMagazynuDoDaty(new vwMagazynRuchGTX2(), DateTime.Now.Date);

            Assert.AreEqual(2, podsumowanieMagazynow.Count());
            Assert.AreEqual(2, podsumowanieMagazynow.First().Ilosc);
            Assert.AreEqual(2, podsumowanieMagazynow.First().Wartosc);
            Assert.AreEqual("m2", podsumowanieMagazynow.First().Jm);
        }

        [Test]
        public async Task PobierzPodsumowanieMagazynuAGGDoDaty_GdyMagazynyNiepuste_DodajeLokalizacje()
        {
            vwMagazynRuchGTX2.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<vwMagazynRuchGTX2, bool>>>()))
                             .ReturnsAsync(new List<vwMagazynRuchGTX2>
                             {
                                 new vwMagazynRuchGTX2 {IdMagazyn=1, MagazynNazwa="Eurofolie", Pozostalo=1, Wartosc=1, Jm="m2"},
                                 new vwMagazynRuchGTX2 {IdMagazyn=2, MagazynNazwa="Budownictwo W", Pozostalo=1, Wartosc=1, Jm="m2"},
                                 new vwMagazynRuchGTX2 {IdMagazyn=3, MagazynNazwa="M2", Pozostalo=1, Wartosc=1, Jm="m2"},
                             });

            var podsumowanieMagazynow =new List<PodsFinans_MagazynyModel> 
                    ( await sut.PobierzPodsumowanieMagazynuDoDaty(new vwMagazynRuchGTX2(), DateTime.Now.Date)) ;

            Assert.AreEqual(3, podsumowanieMagazynow.Count());
            Assert.AreEqual("Magazyn regionalny", podsumowanieMagazynow[0].Lokalizacja);
            Assert.AreEqual("Magazyn regionalny", podsumowanieMagazynow[1].Lokalizacja);
            Assert.AreEqual("Magazyn Studzienice", podsumowanieMagazynow[2].Lokalizacja);
        }


    }
}
