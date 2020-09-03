using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.Utilities.ScaleService;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.Utilities.ScaleService
{
    public class ScalePortRadWagWPT3C1ReaderTests : TestBaseGeneric<ScalePortRadWagWPT3C1Reader>
    {
        public override void SetUp()
        {
            base.SetUp();

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new ScalePortRadWagWPT3C1Reader();
        }
        [Test]
        [Ignore ("Wlaczyc gdy waga jest podlaczona")]
        public async Task MethodName_Condition_Expectations()
        {
            var weightStr = await sut.GetWeightInString();
            var weight = await sut.GetWeight();

            Assert.IsNotEmpty(weightStr);
            Assert.IsTrue(weight > 0);
        }

        [Test]
        public void DecimalParse_Condition_Expectations()
        {
            string numberStr = "10.10";

            decimal w = decimal.Parse(numberStr,CultureInfo.InvariantCulture);
        }
    }
}
