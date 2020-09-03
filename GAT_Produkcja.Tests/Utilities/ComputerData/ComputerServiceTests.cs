using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.Utilities.ComputerData;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.Utilities.ComputerData
{
    class ComputerServiceTests : TestBaseGeneric<ComputerService>
    {
        public override void SetUp()
        {
            base.SetUp();

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new ComputerService();
        }

        [Test]
        public void GetComputerName_ReturnsCompName()
        {
            var result = sut.GetComputerName();

            Assert.IsNotEmpty(result);
        }

    }


}
