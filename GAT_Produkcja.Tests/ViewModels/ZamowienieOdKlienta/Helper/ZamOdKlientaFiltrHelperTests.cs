using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.ZamowienieOdKlienta.Helper;
using GAT_Produkcja.ViewModel.ZamowienieOdKlienta.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.ZamowienieOdKlienta.Helper
{
    class ZamOdKlientaFiltrHelperTests : TestBase
    {
        private ZamOdKlientaFiltrHelper sut;
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
            sut = new ZamOdKlientaFiltrHelper(UnitOfWork.Object);
        }

        private void ListaElementowZBazy()
        {
            vwZamOdKlientaAGG.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<vwZamOdKlientaAGG>
            {
                new vwZamOdKlientaAGG{TowarNazwa="Geokomórka AT CELL 001.100", Ilosc=1, Grupa="Gc", DataMag = DateTime.Now.Date, DataWyst=DateTime.Now.Date, TerminRealizacji=DateTime.Now.Date, Status=(int) ZamOdKlientaAGG_Status_Enum.Niezrealizowane_bezRezerwacji},
                new vwZamOdKlientaAGG{TowarNazwa="Geokomórka AT CELL 001.100", Ilosc=1, Grupa="Gc", DataMag = DateTime.Now.Date, DataWyst=DateTime.Now.Date, TerminRealizacji=DateTime.Now.Date.AddDays(14), Status=(int) ZamOdKlientaAGG_Status_Enum.Niezrealizowane_bezRezerwacji},
                new vwZamOdKlientaAGG{TowarNazwa="test2", Ilosc=2, Grupa="Gb", DataMag = DateTime.Now.Date, DataWyst=DateTime.Now.Date, TerminRealizacji=DateTime.Now.Date, Status=(int) ZamOdKlientaAGG_Status_Enum.Niezrealizowane_bezRezerwacji},
                new vwZamOdKlientaAGG{TowarNazwa="test2", Ilosc=2, Grupa="Ga", DataMag = DateTime.Now.Date, DataWyst=DateTime.Now.Date, TerminRealizacji=DateTime.Now.Date, Status=(int) ZamOdKlientaAGG_Status_Enum.Zrealizowane},
                new vwZamOdKlientaAGG{TowarNazwa="test2", Ilosc=2, Grupa="Ga", DataMag = DateTime.Now.Date, DataWyst=DateTime.Now.Date, TerminRealizacji=DateTime.Now.Date, Status=(int) ZamOdKlientaAGG_Status_Enum.Zrealizowane},
            });
        }


        #region FiltrujAsync
        [Test]
        public async Task FiltrujAsync_GdyBrakArgumentu_PobierzZleceniaAktywne()
        {
            ListaElementowZBazy();

            var lista = await sut.FiltrujAsync(null);

            Assert.AreEqual(3, lista.Count());
        }

        [Test]
        public async Task FiltrujCommandExecute_GdyGrupaWpisana_FiltrujePoGrupie()
        {
            ListaElementowZBazy();

            var lista = await sut.FiltrujAsync(new ZK_Filtr
            {
                Grupa = "Gb"
            });

            Assert.AreEqual(1, lista.Count());
        }


        #endregion

    }
}
