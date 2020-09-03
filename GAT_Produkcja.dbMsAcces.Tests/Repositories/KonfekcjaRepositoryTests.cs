using GAT_Produkcja.dbMsAccess.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAcces.Tests.Repositories
{
    [TestFixture, Ignore("Wlaczac gdy potrzeba - laczy z baza danych")]
    public class KonfekcjaRepositoryTests
    {
        private KonfekcjaRepository sut;

        [SetUp]
        public void SetUp()
        {
            sut = new KonfekcjaRepository();
        }

        [Test]
        public async Task GetByParameters_GdyWszystkoPodane_PobieraDaneZBazy()
        {
            var lista = await sut.GetByParametersAsync(0.38M, 250.00M, "ALTEX PP 90",new DateTime(2019,12,30));

            Assert.IsNotEmpty(lista);
        }
    }
}
