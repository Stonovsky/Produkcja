using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.Utilities.Printers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.Utilities.Printers
{
    class PrinterServiceTests : TestBaseGeneric<PrinterService>
    {
        public override void SetUp()
        {
            base.SetUp();

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new PrinterService();
        }

        [Test]
        public void GetPrinters_GetsNameOfPrinters()
        {
            var result = sut.GetPrinters();

            Assert.IsNotEmpty(result);
        }

        [Test]
        public void GetNetworkIP_PrinterIsNetworkPrinter_ReturnsIP()
        {
            var result = sut.GetPrinterIP("samsung");

            Assert.IsNotEmpty(result);
        }

        [Test]
        public void GetNetworkIP_PrinterIsNotNetworkPrinter_ThrowException()
        {
            Assert.Throws<ArgumentException>(() => sut.GetPrinterIP("xyz"));
        }


    }
}
