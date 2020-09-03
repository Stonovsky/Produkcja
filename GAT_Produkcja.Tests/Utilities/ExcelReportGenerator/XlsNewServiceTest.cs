using GAT_Produkcja.Services;
using GAT_Produkcja.Startup;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.Utilities.ExcelReportGenerator;
using GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Modele;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.Utilities.ExcelReportGenerator
{
    class XlsNewServiceTest : TestBase
    {
        private Mock<IOpenSaveDialogService> openSaveDialogService;
        private XlsNewService sut;

        public override void SetUp()
        {
            base.SetUp();
            IoC.Setup();

            openSaveDialogService = new Mock<IOpenSaveDialogService>();

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new XlsNewService(openSaveDialogService.Object);
        }

        #region AddListToSheet
        [Test]
        [Ignore("Uruchamiac gdy potrzeba - generuje plik excela")]
        public void AddListToSheet_TworzyPlikExcelaZPodanychListGenerycznych()
        {
            var list1 = new List<PodsFinans_KontaBankoweModel>
            {
                new PodsFinans_KontaBankoweModel {Firma="1",WartoscNaKontach=1 },
                new PodsFinans_KontaBankoweModel {Firma="2",WartoscNaKontach=2 },
            };
            var list2 = new List<PodsFinans_ProdukcjaModel>
            {
                new  PodsFinans_ProdukcjaModel {Ilosc_kg=1,RodzajProdukcji="Geokom",Ilosc_m2=1, Wartosc=1},
                new  PodsFinans_ProdukcjaModel {Ilosc_kg=1,RodzajProdukcji="Geow",Ilosc_m2=1, Wartosc=1},
            };
            var kolumnyDoUsuniecia = new List<string> { "WartoscNaKontach" };

            var builder = new XlsServiceBuilder();
            builder.CreateWorkbook(@"C:\Users\Tomasz Strączek\Desktop\Test\RaportTestowyUsuwanie.xlsx");
            builder.CreateWorksheet("Raport");
            builder.AddListToSheet(list1, "Konta", "Raport", kolumnyDoUsuniecia);
            builder.AddListToSheet(list2, "Produkcja", "Raport");
            builder.Build();

        }

        [Test]
        [Ignore("Uruchamiac gdy potrzeba - generuje plik excela")]
        public void AddListToSheet_GdyPodanoNaglowkiDoUsuniecia_UsuwaNaglowkiDanejTabeli()
        {

            var list1 = new List<PodsFinans_NaleznosciIZobowiazaniaModel>
            {
                new PodsFinans_NaleznosciIZobowiazaniaModel {Firma="AGG", NaleznosciDoDaty=1, NaleznosciAll=1, ZobowiazaniaDoDaty=1, ZobowiazaniaAll=1 },
                new PodsFinans_NaleznosciIZobowiazaniaModel {Firma="GTX", NaleznosciDoDaty=1, NaleznosciAll=1, ZobowiazaniaDoDaty=1, ZobowiazaniaAll=1 },
            };
            var list2 = new List<PodsFinans_ProdukcjaModel>
            {
                new  PodsFinans_ProdukcjaModel {Ilosc_kg=1,RodzajProdukcji="Geokom",Ilosc_m2=1, Wartosc=1},
                new  PodsFinans_ProdukcjaModel {Ilosc_kg=1,RodzajProdukcji="Geow",Ilosc_m2=1, Wartosc=1},
            };
            var list3 = new List<PodsFinans_NaleznosciIZobowiazaniaModel>
            {
                new PodsFinans_NaleznosciIZobowiazaniaModel {Firma="AGG",NaleznosciAll=1, ZobowiazaniaAll=1.234m},
                new PodsFinans_NaleznosciIZobowiazaniaModel {Firma="AGG",NaleznosciAll=1.12m, ZobowiazaniaAll=1.4m},
            };

            var kolumnyDoUsuniecia = new List<string> { "ZobowiazaniaDoDaty", "NaleznosciDoDaty" };

            var builder = new XlsServiceBuilder();
            builder.CreateWorkbook(@"C:\Users\Tomasz Strączek\Desktop\Test\RaportTestowy.xlsx");
            builder.CreateWorksheet("Raport");
            builder.AddListToSheet(list1, "Konta", "Raport", kolumnyDoUsuniecia);
            builder.AddListToSheet(list2, "Produkcja", "Raport");
            builder.AddListToSheet(list3, "Nalezności i Zobowiązania", "Raport");
            builder.Build();
        }
        #endregion
    }
}
