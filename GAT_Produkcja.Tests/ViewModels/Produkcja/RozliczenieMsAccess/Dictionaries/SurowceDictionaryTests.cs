using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Dictionaries;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.RozliczenieMsAccess.Dictionaries
{
    [TestFixture]
    public class SurowceDictionaryTests
    {
        private SurowceDictionary sut;

        [SetUp]
        public void SetUp()
        {
            sut = new SurowceDictionary();
        }

        [Test]
        public void PobierzIdSurowcaComarch_GdyIdJestWSlowniku_ZwracaIdComarchu()
        {
            var idSurowcaComarch = sut.PobierzIdSurowcaComarch(1);

            Assert.AreEqual(idSurowcaComarch, 343);
        }

        [Test]
        [Ignore("Zmieniono na zwracanie 0")]
        public void PobierzIdSurowcaComarch_GdyIdNieMaSlowniku_RzucaWyjatek()
        {

            Assert.That(() => sut.PobierzIdSurowcaComarch(100),
                                Throws.Exception
                              .TypeOf<KeyNotFoundException>()
                              );
        }

        [Test]
        public void PobierzIdSurowcaComarch_GdyIdNieMaSlowniku_ZwracaZero()
        {
            var actual = sut.PobierzIdSurowcaComarch(100);

            Assert.AreEqual(0, actual);
        }

        [Test]
        public void PobierzIdSurowcaWSubiecie_GdyIdJestWSlowniku_ZwracaIdSurowcaZSubiekta()
        {
            var idSurowcaSubiekt= sut.PobierzIdSurowacaZSubiekt(1);

            Assert.AreEqual(idSurowcaSubiekt, 72);
        }

        [Test]
        public void PobierzIdSurowcaWSubiecie_GdyIdNieMaWSlowniku_ZwracaZero()
        {
            var idSurowcaSubiekt = sut.PobierzIdSurowacaZSubiekt(13213);

            Assert.AreEqual(idSurowcaSubiekt, 0);
        }

        [Test]
        public void PobierzIdSurowcaWMsAccess_GdyIdJestWSlowniku_ZwracaIdSurowcaZMsAccess()
        {
            var idSurowcaMsAccess = sut.PobierzIdSurowacaZMsAccess(72);

            Assert.AreEqual(1, idSurowcaMsAccess);
        }

        [Test]
        public void PobierzIdSurowcaWMsAccess_GdyIdNieMaWSlowniku_ZwracaZero()
        {
            var idSurowcaMsAccess = sut.PobierzIdSurowacaZMsAccess(13213);

            Assert.AreEqual(idSurowcaMsAccess, 0);
        }
    }

}
