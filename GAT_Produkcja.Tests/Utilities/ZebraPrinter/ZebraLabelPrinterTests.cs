using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.ZebraPrinter;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.Utilities.ZebraPrinter
{
    [TestFixture]
    class ZebraLabelPrinterTests
    {
        private IZebraLabelPrinter sut;
        private Mock<IViewService> viewService;
        private Mock<IDialogService> dialogService;

        [SetUp]

        public void SetUp()
        {
            viewService = new Mock<IViewService>();
            dialogService = new Mock<IDialogService>();

            sut = new ZebraLabelPrinter();
        }
    }
}
