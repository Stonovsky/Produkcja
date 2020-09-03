using GAT_Produkcja.db;
using GAT_Produkcja.dbMsAccess.EntitesInterfaces;
using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.RozliczenieMsAccess.Helpers
{
    public class NazwaTowaruSubiektHelperTests : TestBase
    {
        private NazwaTowaruSubiektHelper sut;

        public override void SetUp()
        {
            base.SetUp();

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new NazwaTowaruSubiektHelper();
        }


        #region NazwaTowaru
        [Test]
        public void GenerujNazweTowaru_GdyBrakPozycjiKonfekcji_ZwrocNull()
        {
            IGniazdoProdukcyjne pozycja = null;

            var nazwa = sut.GenerujNazweTowaru(pozycja);

            Assert.IsNull(nazwa);
        }

        [Test]
        public void GenerujNazweTowaru_PozycjaOk_ZwrocNazwe()
        {
            var pozycja = new Konfekcja
            {
                Artykul = "Altex PP 90",
                Szerokosc = 200,
                Szerokosc_M = 2,
                Dlugosc = 100,
            };

            var nazwa = sut.GenerujNazweTowaru(pozycja);

            Assert.IsNotNull(nazwa);
            Assert.AreEqual("Geowłóknina ALTEX AT PP 90 (2mx100m)", nazwa);
        }
        #endregion


        #region SymbolTowaru dla IGniazdoProdukcyjne
        [Test]
        public void GenerujSymbolTowaru_GdyBrakPozycjiKonfekcji_ZwrocNull()
        {
            IGniazdoProdukcyjne pozycja = null;
            var nazwa = sut.GenerujSymbolTowaru(pozycja);

            Assert.IsNull(nazwa);
        }


        [Test]
        public void GenerujSymbolTowaru_PozycjaOk_ZwrocNazwe()
        {
            var pozycja = new Konfekcja
            {
                Artykul = "Altex PP 90",
                Szerokosc = 200,
                Szerokosc_M = 0,
                Dlugosc = 100,

            };

            var nazwa = sut.GenerujSymbolTowaru(pozycja);

            Assert.IsNotNull(nazwa);
            Assert.AreEqual("ALT_PP_90_2/100", nazwa);
        }


        [Test]
        public void GenerujSymbolTowaru_PozycjaNieNullIZalezneTabeleNieNull_ZwracaSymbol()
        {
            var pozycja = new tblProdukcjaRuchTowar
            {
                tblTowarGeowlokninaParametryGramatura = new tblTowarGeowlokninaParametryGramatura { Gramatura = 100 },
                tblTowarGeowlokninaParametrySurowiec = new tblTowarGeowlokninaParametrySurowiec { Skrot = "PP" },
                Szerokosc_m = 1.00m,
                Dlugosc_m = 100.00m,
            };

            var symbol = sut.GenerujSymbolTowaru(pozycja);

            Assert.AreEqual("ALT_PP_100_1/100", symbol);
        }


        [Test]
        public void GenerujSymbolTowaru_PozycjaNieNullAleZalezneTabeleNull_ZwracaNull()
        {
            var pozycja = new tblProdukcjaRuchTowar
            {
                tblTowarGeowlokninaParametrySurowiec = new tblTowarGeowlokninaParametrySurowiec { Skrot = "PP" },
                Szerokosc_m = 1.00m,
                Dlugosc_m = 100.00m,
            };

            var symbol = sut.GenerujSymbolTowaru(pozycja);

            Assert.IsNull(symbol);
        }

        #endregion

    }
}
