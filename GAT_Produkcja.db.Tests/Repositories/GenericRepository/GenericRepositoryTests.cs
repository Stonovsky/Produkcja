using GAT_Produkcja.db.Repositories.UnitOfWork;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Tests.Repositories.GenericRepository
{
    [TestFixture, Ignore("Test na bazie danych - wlaczac kiedy trzeba")]
    public class GenericRepositoryTests
    {
        private UnitOfWork sut;

        [SetUp]
        public void SetUp()
        {
            sut = new UnitOfWork(new GAT_ProdukcjaModel());

        }

        #region Generuj

        [Test]
        public async Task GenerateNumberAsync_GdyListaPusta_Zwraca1()
        {

            var nr = await sut.tblProdukcjaZlecenie.GetNewNumberAsync(n=>n.DataUtworzenia.Year==2018,n=>n.NrZlecenia.Value);

            Assert.IsTrue(nr==1);
        }

        [Test]
        public async Task Generuj_GdyListaNiePusta_ZwracaMaxPlus1()
        {
            var nr = await sut.tblProdukcjaZlecenie.GetNewNumberAsync(n => n.DataUtworzenia.Year == DateTime.Now.Year, n => n.NrZlecenia.Value);

            Assert.IsTrue(nr > 0);

         }
        #endregion

        #region GenerujPelnyNr
        [Test]
        public async Task GenerujPelnyNr_GdyListaPusta_Zwraca1()
        {
            var nr = await sut.tblProdukcjaZlecenie.GetNewFullNumberAsync(n => n.DataUtworzenia.Year == 2018, n => n.NrZlecenia.Value,"ZLC");

            Assert.IsTrue(nr == "1/ZLC/2020");

        }

        [Test]
        public async Task GenerujPelnyNr_GdyListaNiePusta_NrMaxPlus1OrazReszta()
        {
            var nr = await sut.tblProdukcjaZlecenie.GetNewFullNumberAsync(n => n.DataRozpoczecia.Value.Year == DateTime.Now.Year, n => n.NrZlecenia.Value, "ZLC");

            Assert.IsTrue(nr != "1/ZLC/2020");

        }
        #endregion

    }
}
