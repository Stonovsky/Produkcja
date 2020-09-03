using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.Utilities.ScaleService;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.Utilities.ScaleService
{
    [TestFixture]
    class ScaleServiceTests : TestBaseGeneric<ScaleReader>
    {
        public override void SetUp()
        {
            base.SetUp();

            CreateSut();
        }

        [Test]
        [Ignore ("Wlaczac gdy waga jest podlaczona")]
        public void Porty_Condition_Expectations()
        {
            List<string> portNames = null;
            var ports = SerialPort.GetPortNames();
            foreach (var port in SerialPort.GetPortNames())
            {
                portNames.Add( port);
            }

            SerialPort serialPort = new SerialPort();
        }

        [Test]
        public void MethodName_Condition_Expectations()
        {
            sut.Read();
        }

        public override void CreateSut()
        {
            sut = new ScaleReader();
        }
    }
}
