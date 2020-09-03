using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.dbMsAccess.Models.Adapters;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAcces.Tests.Models.Adapters
{
    [TestFixture]
    public class NormyZuzycia_tblProdukcjaZlecenieTowarAdapterTests
    {
        [Test]
        [Ignore("sprawdzone na bazie SQL")]
        public void GdyDaneOk_SprawdzaPrawidlowoscKonwersji()
        {
            var surowce = new List<Surowiec>();
            var mieszanka = new List<NormyZuzycia>{ new NormyZuzycia
            {
                Artykul = "ALTEX PP 90",
                Dostawca = 1,
                Ilosc = 0.4m,
                Surowiec = "PP 4,4/75/UV",
                Zlecenie = "170-W-1",
                ZlecenieID = 1,
                Id = 1
            } };

            var zlecenie = new Dyspozycje
            {
                Artykul = "ALTEX PP 90",
            };

            var result = new NormyZuzycia_tblProdukcjaZlecenieTowarAdapter(mieszanka, zlecenie, surowce);

            Assert.AreEqual(3, result.IDTowarGeowlokninaParametryGramatura);
            Assert.AreEqual(90, result.Gramatura);
            Assert.AreEqual(1, result.IDTowarGeowlokninaParametrySurowiec);
            Assert.AreEqual("PP", result.Surowiec);
            Assert.AreEqual("LEGS", result.Uwagi);
            Assert.AreEqual(zlecenie.Artykul, result.TowarNazwa);


        }
    }
}
