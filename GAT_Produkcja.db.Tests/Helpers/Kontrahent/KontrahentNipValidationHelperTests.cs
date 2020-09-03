using GAT_Produkcja.db.Helpers.Kontrahent;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Tests.Helpers.Kontrahent
{
    [TestFixture]
    public class KontrahentNipValidationHelperTests
    {
        private KontrahentNipValidationHelper sut;

        [SetUp]
        public void SetUp()
        {
            sut = new KontrahentNipValidationHelper();
        }

        [Test]
        [TestCase("PL1234567890")]
        [TestCase("PL 1234567890")]
        [TestCase("PL 123-456-78-90")]
        [TestCase("PL 123 456 78 90")]
        [TestCase("123 456 78 90")]
        public void WyseparujCyfry_SameCyfrySaWyseparowane(string nip)
        {
            var actual = sut.WyseparujCyfry(nip);

            Assert.AreEqual("1234567890", actual);
        }

        [Test]
        [TestCase("PL1234567890", "PL")]
        [TestCase("123 456 78 90", "")]
        public void WyseparujPrefiksKraju_SeparujeListery(string nip, string expected)
        {
            var actual = sut.WyseparujPrefiksKraju(nip);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("PL1234567890", "1234567890")]
        //[TestCase("EN1234567890", "")] -> test bledny - zalozenie: mozliwosc sciagania tylko danych dla polskich kontrahentow
        [TestCase("PL 123 456 -78-90", "1234567890")]
        [TestCase("PLE 123 456 -78-90", "")]
        public void ZwalidowanyNipPL_ZwracaWlasciwieZwalidowanyNip(string nip, string expected)
        {
            var actual = sut.ZwalidowanyNipPL(nip);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CzyNipPoprawnyDoPobraniaDanychZGus_GdyNipNull_False()
        {
            var actual = sut.CzyNipPoprawnyDoPobraniaDanychZGus(null);

            Assert.IsFalse(actual);
        }
        [Test]
        [TestCase("PL 651 166 86 30")]
        [TestCase("PL 6511668630")]
        [TestCase("PL6511668630")]
        [TestCase("6511668630")]
        public void CzyNipPoprawnyDoPobraniaDanychZGus_GdyNipOk_True(string nip)
        {
            var actual = sut.CzyNipPoprawnyDoPobraniaDanychZGus(nip);

            Assert.IsTrue(actual);
        }

        #region WyseparujPrefixINrNip
        [Test]
        [TestCase("PL 651 166 86 30")]
        [TestCase("PL 6511668630")]
        [TestCase("PL6511668630")]
        [TestCase("6511668630")]
        public void WyseparujPrefixINrNip_Separuje(string nip)
        {
            var nrNip = sut.WyseparujCyfry(nip);

            Assert.IsNotEmpty(nrNip);
            Assert.AreEqual(10,nrNip.Length);
        }
        #endregion
    }
}
