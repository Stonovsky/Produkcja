using GAT_Produkcja.dbComarch.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbComarch.Tests.Repositories
{
    [TestFixture]
    public class SurowiecRepositoryTests
    {
        private SurowiecRepository sut;

        [SetUp]

        public void SetUp()
        {

            sut = new SurowiecRepository();
        }

        [Test]
        [Ignore ("Wykomentuj atrybut, gdy trzeba sprawdzic czy pobiera dane z bazy")]
        public async Task PobierzListeSurowcowZCenamiAsync_ListaNieMozeBycPusta()
        {
            var lista = await sut.PobierzListeSurowcowZCenamiAsync();

            Assert.IsNotEmpty(lista);

        }
    }
}
