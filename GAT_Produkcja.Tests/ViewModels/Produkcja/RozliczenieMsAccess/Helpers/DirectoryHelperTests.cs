using GAT_Produkcja.db;
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
    public class DirectoryHelperTests : TestBaseGeneric<DirectoryHelper>
    {
        private Mock<IDirectoryInfoWrapper> directoryInfo;

        public override void SetUp()
        {
            base.SetUp();

            directoryInfo = new Mock<IDirectoryInfoWrapper>();

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new DirectoryHelper(directoryInfo.Object);
        }

        [Test]
        public void GenerujSciezke_GdySciezkaIstnieje_DodajNumer()
        {
            var produkcja = new List<tblProdukcjaRozliczenie_PW> { new tblProdukcjaRozliczenie_PW { NrZlecenia = "001" } };
            directoryInfo.Setup(s => s.DirectoryExists(It.IsAny<string>())).Returns(true);

            var sciezka = sut.GenerujSciezke(produkcja);

            Assert.IsTrue(sciezka.Contains("_1"));

        }
    }
}
