using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Helpers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Finanse.PodsumowanieWDatach.Helpers
{
    public class PodsumowanieZamowieniaOdKlientowHelperTests : TestBase
    {
        private PodsumowanieZamowieniaOdKlientowHelper_ZreazliowaneNiezrealizowane sut;
        private Mock<IVwZamOdKlientaAGGRepository> vwZamOdKlientaAGG;

        public override void SetUp()
        {
            base.SetUp();

            vwZamOdKlientaAGG = new Mock<IVwZamOdKlientaAGGRepository>();
            UnitOfWork.Setup(s => s.vwZamOdKlientaAGG).Returns(vwZamOdKlientaAGG.Object);

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new PodsumowanieZamowieniaOdKlientowHelper_ZreazliowaneNiezrealizowane(UnitOfWork.Object);
        }

        [Test]
        public async Task PodsumowanieZlecenOdKlientow_GdyListaNiePusta_Podsumowuje()
        {
            vwZamOdKlientaAGG.Setup(s=>s.GetAllAsync()).ReturnsAsync(
             new List<vwZamOdKlientaAGG>
            {
                new vwZamOdKlientaAGG {StatusEx=(int)ZamOdKlientaAGG_StatusEx_Enum.NieZrealizowane, Ilosc=1, WartNetto=1},
                new vwZamOdKlientaAGG {StatusEx=(int)ZamOdKlientaAGG_StatusEx_Enum.NieZrealizowane, Ilosc=1, WartNetto=1},
                new vwZamOdKlientaAGG {StatusEx=(int)ZamOdKlientaAGG_StatusEx_Enum.Zrealizowane_Calkowicie, Ilosc=1, WartNetto=1},
            });

            var podsumowanie = await sut.PodsumujZamowieniaOdKlientow(DateTime.Now.Date,DateTime.Now.Date);

            Assert.AreEqual(2, podsumowanie.SingleOrDefault(z => z.CzyZrealizowano == false).IloscCalkowita);
            Assert.AreEqual(1, podsumowanie.SingleOrDefault(z => z.CzyZrealizowano == true).IloscCalkowita);
        }

        [Test]
        public async Task PodsumowanieZlecenOdKlientow_GdyListaPusta_ZwracaNowyObiekt()
        {
            var listaZamowienOdKlientow = new List<vwZamOdKlientaAGG>();

            var podsumowanie = await sut.PodsumujZamowieniaOdKlientow(DateTime.Now.Date,DateTime.Now.Date);

            Assert.IsEmpty(podsumowanie);
        }

        [Test]
        public async Task PodsumowanieZlecenOdKlientow_GdyListaNull_ZwracaNowyObiekt()
        {
            var podsumowanie = await sut.PodsumujZamowieniaOdKlientow(DateTime.Now.Date,DateTime.Now.Date);

            Assert.IsEmpty(podsumowanie);
        }
    }
}
