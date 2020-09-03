using GAT_Produkcja.db;
using GAT_Produkcja.Utilities.EppFile;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.Utilities.EppFile
{
    [TestFixture]
    public class EppFileGeneratorTests
    {
        private EppFileGenerator sut;
        private tblProdukcjaRozliczenie_Naglowek naglowek;
        private List<tblProdukcjaRozliczenie_RW> listaRW;
        private List<tblProdukcjaRozliczenie_PW> listaPW;

        [SetUp]
        public void SetUp()
        {
            sut = new EppFileGenerator();

            naglowek = new tblProdukcjaRozliczenie_Naglowek
            {
                DataDodania = DateTime.Now,
                tblPracownikGAT = new tblPracownikGAT { Imie = "Tomasz", Nazwisko = "Straczek", ImieINazwiskoGAT = "Tomasz Strączek" }
            };
            listaRW = new List<tblProdukcjaRozliczenie_RW>
            {
                new tblProdukcjaRozliczenie_RW{SymbolTowaruSubiekt="PES_4/51", NazwaTowaruSubiekt="Włókno PES cięte 4 DEN/51mm", Ilosc=99.2693m, CenaJednostkowa=5.42m, Wartosc=521.16382500m, Jm="kg"},
                new tblProdukcjaRozliczenie_RW{SymbolTowaruSubiekt="PP_4,4/76UV", NazwaTowaruSubiekt="Włókno PP 4,4/76 W UV HT", Ilosc=167.9942m, CenaJednostkowa=6.42m, Wartosc=1078.52276400m, Jm = "kg"},
                new tblProdukcjaRozliczenie_RW{SymbolTowaruSubiekt="PP_6,7/76UV", NazwaTowaruSubiekt="Włókno PP 6,7/76 W UV HT", Ilosc=496.3465m, CenaJednostkowa=6.32m, Wartosc=3161.72720500m, Jm="kg"},
                new tblProdukcjaRozliczenie_RW{SymbolTowaruSubiekt="PP_6,7/76UV", NazwaTowaruSubiekt="Włókno PP 6,7/76 W UV HT", Ilosc=496.3465m, CenaJednostkowa=6.32m, Wartosc=3161.72720500m, Jm="kg"},
            };
            listaPW = new List<tblProdukcjaRozliczenie_PW>
            {
                new tblProdukcjaRozliczenie_PW{SymbolTowaruSubiekt="ALT_PES_150_2/50", NazwaTowaruSubiekt="Geowłóknina ALTEX AT PES 150 (2mx50m)", Ilosc=103.34353435m, CenaJednostkowa=0.54m, CenaProduktuBezNarzutow_m2=0.54m, Wartosc=102.2837875m, Jm="m2"},
                new tblProdukcjaRozliczenie_PW{SymbolTowaruSubiekt="ALT_PES_150_0,5/50", NazwaTowaruSubiekt="Geowłóknina ALTEX AT PES 150 (0,5mx50m)", Ilosc=121.2345609m, CenaJednostkowa=0.76m, CenaProduktuBezNarzutow_m2=0.76m, Wartosc=200.4395486m, Jm="m2"},
                new tblProdukcjaRozliczenie_PW{SymbolTowaruSubiekt="TASMY_PES", NazwaTowaruSubiekt="Surowiec PES taśmy", Ilosc=200.2345609m, CenaJednostkowa=5.76m, CenaProduktuBezNarzutow_m2=0, Wartosc=1200.4395486m, Jm="kg"},
            };

        }


        [Test]
        public void GenerujZawartoscPliku_GdyArgumentyOk_Generuje()
        {
            var text = sut.GenerujZawartoscPliku(db.Enums.StatusRuchuTowarowEnum.RozchodWewnetrzny_RW, naglowek, listaRW);

            Assert.IsNotEmpty(text);
        }
        [Test]
        public void GenerujZawartoscPliku_GdyWNaglowkuBrakPrawocnika_GenerujeTomasza()
        {
            naglowek.tblPracownikGAT = null;

            var text = sut.GenerujZawartoscPliku(db.Enums.StatusRuchuTowarowEnum.RozchodWewnetrzny_RW, naglowek, listaRW);

            Assert.IsTrue(text.Contains("Tomasz"));
        }
        [Test]
        public void GenerujZawartoscPliku_GdyStatusRW_WPlikuStatusRW()
        {
            naglowek.tblPracownikGAT = null;

            var text = sut.GenerujZawartoscPliku(db.Enums.StatusRuchuTowarowEnum.RozchodWewnetrzny_RW, naglowek, listaRW);

            Assert.IsTrue(text.Contains("RW"));
        }

        [Test]
        public void GenerujZawartoscPliku_GdyStatusPW_WPlikuStatusPW()
        {
            naglowek.tblPracownikGAT = null;

            var text = sut.GenerujZawartoscPliku(db.Enums.StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW, naglowek, listaRW);

            Assert.IsTrue(text.Contains("PW"));
        }
        [Test]
        public void GenerujPlikEPP_RW()
        {
            var sciezkaPliku = @"C:\Users\Tomasz Strączek\Dysk Google\Praca\WPF\EPP\RW_test1.epp";

            sut.GenerujPlikEPP(db.Enums.StatusRuchuTowarowEnum.RozchodWewnetrzny_RW, naglowek, listaRW, sciezkaPliku);
        }
        [Test]
        public void GenerujPlikEPP_PW()
        {
            var sciezkaPliku = @"C:\Users\Tomasz Strączek\Dysk Google\Praca\WPF\EPP\PW_test5.epp";

            sut.GenerujPlikEPP(db.Enums.StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW, naglowek, listaPW, sciezkaPliku);
        }
        [Test]
        public void DataDodania_Format()
        {
            var data = DateTime.Now;

            var t = data.ToString("yyyyMMddHHmmss");
        }
    }
}
