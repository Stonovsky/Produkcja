using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.UI.Utilities.Dostep;
using GAT_Produkcja.Utilities.ZebraPrinter.S4M;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.Utilities.ZebraPrinter.S4M
{
    class ZebraS4MServiceTests : TestBaseGeneric<ZebraS4MService>
    {
        private Mock<IZebraZPLCELabelGenerator> zebraZPLCELabel;
        private Mock<IZebraZLPLabelGenerator> zebraZLPLabel;
        private Mock<IZebraS4MPrinter> zebraS4MPrinter;
        private Mock<ITblKonfiguracjaUrzadzenRepository> tblKonfiguracjaUrzadzen;

        public Action KonfiguracjaUrzadzenAction { get; set; }

        public override void SetUp()
        {
            base.SetUp();

            zebraZPLCELabel = new Mock<IZebraZPLCELabelGenerator>();
            zebraZLPLabel = new Mock<IZebraZLPLabelGenerator>();
            zebraS4MPrinter = new Mock<IZebraS4MPrinter>();

            tblKonfiguracjaUrzadzen = new Mock<ITblKonfiguracjaUrzadzenRepository>();
            UnitOfWork.Setup(s => s.tblKonfiguracjaUrzadzen).Returns(tblKonfiguracjaUrzadzen.Object);

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new ZebraS4MService(zebraZPLCELabel.Object, zebraZLPLabel.Object, zebraS4MPrinter.Object, UnitOfWork.Object);
        }

        [Test]
        public void CanPrint_WhenIpIsNullOrEmpty_False()
        {
            UzytkownikZalogowany.KonfiguracjaUrzadzen = new tblKonfiguracjaUrzadzen();

            var result = sut.CanPrint();

            Assert.IsFalse(result);
        }

        [Test]
        public void CanPrint_WhenIpIsNotNullOrEmpty_True()
        {
            tblKonfiguracjaUrzadzen.Setup(s => s.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblKonfiguracjaUrzadzen, bool>>>()))
                                   .ReturnsAsync(new db.tblKonfiguracjaUrzadzen { DrukarkaIP = "1" });
            UzytkownikZalogowany.KonfiguracjaUrzadzen = new tblKonfiguracjaUrzadzen() { DrukarkaIP="1"};
            sut.LoadAsync();

            var result = sut.CanPrint();

            Assert.IsTrue(result);
        }
    }
}
