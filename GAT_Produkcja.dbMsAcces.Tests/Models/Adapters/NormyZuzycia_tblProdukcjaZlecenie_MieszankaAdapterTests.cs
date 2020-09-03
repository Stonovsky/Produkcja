using GAT_Produkcja.db.Enums;
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
    public class NormyZuzycia_tblProdukcjaZlecenie_MieszankaAdapterTests 
    {

        [Test]
        [Ignore("Sprawdzone na bazie SQL")]
        public void GdyDaneOk_SprawdzaCzyKonwersjaJestWlasciwa()
        {

            var surowce = new List<Surowiec>();
            var normyZuzycia = new NormyZuzycia
            {
                Artykul = "ALTEX PP 90",
                Dostawca = 1,
                Ilosc = 0.4m,
                Surowiec = "PP 4,4/75/UV",
                Zlecenie = "170-W-1",
                ZlecenieID = 1,
                Id=1
            };

            var result = new NormyZuzycia_tblProdukcjaZlecenie_MieszankaAdapter(normyZuzycia,surowce);

            Assert.AreEqual("LEGS", result.Uwagi.ToUpper());
            Assert.AreEqual("kg", result.JmNazwa);
            Assert.AreEqual("PP 4,4/75/UV", result.NazwaTowaru);
            Assert.AreEqual(0.4m, result.ZawartoscProcentowa);
            Assert.AreEqual(1, result.IDMsAccess);
            Assert.AreEqual((int)JmEnum.kg, result.IDJm);

        }

    }
}
