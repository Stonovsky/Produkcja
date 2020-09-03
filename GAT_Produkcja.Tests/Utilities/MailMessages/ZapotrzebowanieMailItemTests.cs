using GAT_Produkcja.db;
using GAT_Produkcja.Utilities.MailSenders.MailMessages;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.Utilities.MailMessages
{
    [TestFixture]
    public class ZapotrzebowanieMailItemTests
    {
        private ZapotrzebowanieMailItem sut;

        [SetUp]
        public void SetUp()
        {
            CreateSut(null, null);
        }

        private void CreateSut(tblZapotrzebowanie zapotrzebowanie, List<string> listaAdresow)
        {
            sut = new ZapotrzebowanieMailItem(zapotrzebowanie, listaAdresow);

        }

        #region CombineAddressesTo
        [Test]
        public void CombineAddressesTo_GdyListaAdresowPusta_PustyString()
        {
            var listaAdresow = sut.CombineAddressesTo();

            Assert.IsEmpty(listaAdresow);
        }
        [Test]
        public void CombineAddressesTo_GdyPrzeslanoAdressNull_OmijaTenMailWIteracji()
        {
            CreateSut(null, new List<string> { "tom@tom.pl", null });

            var listaAdresow = sut.CombineAddressesTo();

            Assert.AreEqual("tom@tom.pl",listaAdresow);
        }

        [Test]
        public void CombineAddressesTo_GdyPrzeslanoAdressyOK_GenerujStringa()
        {
            CreateSut(null, new List<string> { "tom@tom.pl","tom@tom.pl", "tom@tom.pl", "t@t.pl", "tt@t.pl" });

            var listaAdresow = sut.CombineAddressesTo();

            Assert.AreEqual("tom@tom.pl; t@t.pl; tt@t.pl", listaAdresow); 
        }

        #endregion
    }
}
