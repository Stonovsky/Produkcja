using GAT_Produkcja.db;
using GAT_Produkcja.Services;
using GAT_Produkcja.Utilities.ExcelReport;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Modele;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GAT_Produkcja.Tests.Utilities.ExcelReport
{
    [TestFixture]
    class XlsServiceTests
    {
        private Mock<IOpenSaveDialogService> openSaveDialogService;
        private XlsService sut;
        private XlsService sutReal;

        [SetUp]
        public void Setup()
        {
            openSaveDialogService = new Mock<IOpenSaveDialogService>();

            sut = new XlsService(openSaveDialogService.Object);
            sutReal = new XlsService(new OpenSaveDialogService());
        }

        [Test]
        public void CreateExcelReport_GdyListaNullLubPusta_Wyjatek()
        {
            Assert.ThrowsAsync<ArgumentException>(() => sut.CreateExcelReportAsync((IEnumerable<string>)null, null));
            Assert.ThrowsAsync<ArgumentException>(() => sut.CreateExcelReportAsync(new List<string>(), null));
        }

        [Test]
        public void CreateExcelReport_GdyBrakSciezkiZapisu_WyswietlaSaveDialogService()
        {
            sut.CreateExcelReportAsync(new List<string> { "test" }, null);

            openSaveDialogService.Verify(v=> v.SaveFile());
        }

        [Test]
        public void TestExportuRW()
        {
            var listaEncji = new List<tblProdukcjaRozliczenie_RW>
            {
                new tblProdukcjaRozliczenie_RW
                {
                    CenaJednostkowa=1,
                    SymbolTowaruSubiekt="t1",
                    NazwaTowaruSubiekt="test1",
                    Ilosc = 1,
                    Udzial=0.1m,
                    DataDodania = DateTime.Now,
                    Wartosc=1
                },
                new tblProdukcjaRozliczenie_RW
                {
                    CenaJednostkowa=1,
                    SymbolTowaruSubiekt="t2",
                    NazwaTowaruSubiekt="test2",
                    Ilosc = 1,
                    Udzial=0.1m,
                    DataDodania = DateTime.Now,
                    Wartosc=1
                },
            };

            sutReal.CreateExcelReportAsync(listaEncji, "RW_Podsumowanie",null, @"C:\Users\Tomasz Strączek\Desktop\Test\test.xlsx");
        }
        [Test]
        public void TestExportuPW()
        {
            var listaEncji = new List<tblProdukcjaRozliczenie_PW>
            {
                new tblProdukcjaRozliczenie_PW
                {
                    SymbolTowaruSubiekt="t1",
                    NazwaTowaruSubiekt="test1",
                    CenaHurtowaAGG_m2=1.45m,
                    CenaProduktuBezNarzutow_kg=6.44m,
                    CenaSprzedazyGtex_m2=1.77m,
                    NrRolki="r1",
                    NrWz="232",
                    Ilosc=1200,
                    Ilosc_kg=234.34m,
                    Odpad_kg=12.33m,
                    Wartosc=1

                },
                new tblProdukcjaRozliczenie_PW
                {
                    SymbolTowaruSubiekt="t2",
                    NazwaTowaruSubiekt="test2",
                    CenaHurtowaAGG_m2=1.45m,
                    CenaProduktuBezNarzutow_kg=6.44m,
                    CenaSprzedazyGtex_m2=1.77m,
                    NrRolki="r1",
                    NrWz="232",
                    Ilosc=1200,
                    Ilosc_kg=234.34m,
                    Odpad_kg=12.33m,
                    Wartosc=1

                },
            };

            sutReal.CreateExcelReportAsync(listaEncji, "RW_Podsumowanie", null, @"C:\Users\Tomasz Strączek\Desktop\Test\testPW.xlsx");
        }
        #region OLD

        //[Test]
        //public void CreateExcelReport_KomunikatBledu_RoznaIloscTabelITytulow()
        //{
        //    //Arrange
        //    List<string> listOfTitles = new List<string>() { "tytul" };
        //    List<ObservableCollection<tblWynikiBadanGeowloknin>> listsOfData = new List<ObservableCollection<tblWynikiBadanGeowloknin>>()
        //    {
        //        new ObservableCollection<tblWynikiBadanGeowloknin>
        //        {
        //            new tblWynikiBadanGeowloknin{ Gramatura = "100"},
        //        },
        //        new ObservableCollection<tblWynikiBadanGeowloknin>
        //        {
        //            new tblWynikiBadanGeowloknin{ Gramatura = "90"},
        //        }
        //    };

        //    Assert.AreNotEqual(listOfTitles.Count(), listsOfData.Count());
        //    string str = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "test.xlsx";
        //    var ex = Assert.Throws<Exception>(() => sut.CreateExcelReport(listsOfData, listOfTitles, true, str));
        //    Assert.That(ex.Message, Is.EqualTo("Ilość tytułów oraz tabel jest nieprawidłowa"));
        //}

        //[Test]
        //public void CreateExcelReport_KomunikatBledu_ListaTabelITytulowPuste()
        //{
        //    List<string> listOfTitles = new List<string>();
        //    List<ObservableCollection<tblWynikiBadanGeowloknin>> listsOfData = new List<ObservableCollection<tblWynikiBadanGeowloknin>>();

        //    Assert.IsEmpty(listOfTitles);
        //    Assert.IsEmpty(listsOfData);

        //    string str = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "test.xlsx";
        //    var ex = Assert.Throws<Exception>(() => sut.CreateExcelReport(listsOfData, listOfTitles, true, str));
        //    Assert.That(ex.Message, Is.EqualTo("Ilość tytułów oraz tabel jest nieprawidłowa"));
        //}

        //[Test]
        //public void CreateExcelReport_ArgumentyOK_RownaIloscTabelITytulow()
        //{
        //    //Arrange
        //    List<string> listOfTitles = new List<string>() { "tytul" };
        //    List<ObservableCollection<tblWynikiBadanGeowloknin>> listsOfData = new List<ObservableCollection<tblWynikiBadanGeowloknin>>()
        //    {
        //        new ObservableCollection<tblWynikiBadanGeowloknin>
        //        {
        //            new tblWynikiBadanGeowloknin{ Gramatura = "100"},
        //        }
        //    };

        //    //Act
        //    string str = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "test.xlsx";
        //    sut.CreateExcelReport(listsOfData, listOfTitles, true, str);

        //    //Assert
        //    Assert.AreEqual(listOfTitles.Count(), listsOfData.Count());
        //}

        //[Test]
        //public void CreateExcelReport_KomunikatBledu_StringPustyAlboNull()
        //{
        //    List<string> listOfTitles = new List<string>() { "tytul" };
        //    List<ObservableCollection<tblWynikiBadanGeowloknin>> listsOfData = new List<ObservableCollection<tblWynikiBadanGeowloknin>>()
        //    {
        //        new ObservableCollection<tblWynikiBadanGeowloknin>
        //        {
        //            new tblWynikiBadanGeowloknin{ Gramatura = "100"},
        //        }
        //    };

        //    string str = string.Empty;
        //    var ex = Assert.Throws<Exception>(() => sut.CreateExcelReport(listsOfData, listOfTitles, true, str));
        //    Assert.That(ex.Message, Is.EqualTo("Nieprawidłowa ścieżka pliku Excel"));

        //}

        //[Test]
        //public void CreateExcelReport_ArgumentyOK_StringExcelPathOK()
        //{
        //    List<string> listOfTitles = new List<string>() { "tytul" };
        //    List<ObservableCollection<tblWynikiBadanGeowloknin>> listsOfData = new List<ObservableCollection<tblWynikiBadanGeowloknin>>()
        //    {
        //        new ObservableCollection<tblWynikiBadanGeowloknin>
        //        {
        //            new tblWynikiBadanGeowloknin{ Gramatura = "100"},
        //        }
        //    };

        //    string str = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "test.xlsx";
        //    sut.CreateExcelReport(listsOfData, listOfTitles, true, str);

        //    Assert.IsNotNull(str);
        //}

        #endregion

        [Test]
        [Ignore("Wlaczac tylko gdy trzeba sprawdzic")]
        public void PobierzCenyTransferowe_GdyPrzeslanoPlikOk_GenerujeListe()
        {
            string sciezka = @"\\192.168.34.57\gat_poufne\Ceny\GTEX_AGG\GTEX_CenyTransferowe_01.05.2020.xlsx";

            var lista = sut.GetTranferPrices("CenyTransferowe", sciezka);

            Assert.IsNotEmpty(lista);
        }
    }
}
