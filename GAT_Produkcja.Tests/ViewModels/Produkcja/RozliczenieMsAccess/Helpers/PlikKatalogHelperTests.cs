using GAT_Produkcja.db;
using GAT_Produkcja.db.EntitesInterfaces;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.Utilities.Wrappers;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.RozliczenieMsAccess.Helpers
{
    public class PlikKatalogHelperTests : TestBase
    {
        private const string SCIEZKA_BAZOWA = @"\\192.168.34.57\gtex\10. PRODUKCJA\Rozliczenia ksiegowe\0_Magazyn\!_Program\";
        private DirectoryHelper sut;
        private Mock<IDirectoryInfoWrapper> direcotryInfo;

        public override void SetUp()
        {
            base.SetUp();

            direcotryInfo = new Mock<IDirectoryInfoWrapper>();

            CreateSut();
        }

        public override void CreateSut()
        {
            sut = new DirectoryHelper(direcotryInfo.Object);
        }

        [Test]
        public void GenerujNazweKatalogu_TworzyNazweZUnikalnychWartosci()
        {
            var lista = new List<IProdukcjaRozliczenie>
            {
                new tblProdukcjaRozliczenie_RW {NazwaTowaruSubiekt="ALTEX AT PP 100 (1mx100m)", NrZlecenia="002"},
                new tblProdukcjaRozliczenie_RW {NazwaTowaruSubiekt="ALTEX AT PP 90 (1mx100m)", NrZlecenia="2"},
                new tblProdukcjaRozliczenie_RW {NazwaTowaruSubiekt="ALTEX AT PES 90 (1mx100m)", NrZlecenia="2"},
            };

            var sciezka = sut.GenerujSciezke(lista);

            string expected= $"{SCIEZKA_BAZOWA}{DateTime.Now.Date.ToString("yyyy-MM-dd")} - ZP 002\\";

            Assert.IsTrue(sciezka.Contains("002"));
            Assert.AreEqual(expected, sciezka);
        }
    }
}
