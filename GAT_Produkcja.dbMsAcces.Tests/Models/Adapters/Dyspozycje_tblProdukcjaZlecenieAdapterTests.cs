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
    public class Dyspozycje_tblProdukcjaZlecenieAdapterTests
    {
        [Test]
        public void GdyDaneOk_KonwertujePoprawnie()
        {
            var zlecenie = new Dyspozycje
            {
                Artykul = "ALTEX PES 100",
                CzyZakonczone = false,
                Data = new DateTime(2020, 1, 1),
                Id = 1,
                Ilosc_m2 = 100,
                NrZlecenia = "1-2"
            };

            var result = new Dyspozycje_tblProdukcjaZlecenieAdapter(zlecenie);

            Assert.IsNull(result.KodKreskowy);
            Assert.AreEqual(zlecenie.NrZlecenia, result.NazwaZlecenia);
            Assert.AreEqual(18, result.IDZlecajacy);
            Assert.AreEqual(zlecenie.Data, result.DataRozpoczecia);
            Assert.AreEqual(zlecenie.Data, result.DataRozpoczeciaFakt);
            Assert.AreEqual((int)ProdukcjaZlecenieStatusEnum.Zakonczone, result.IDProdukcjaZlecenieStatus);
            Assert.AreEqual((int)GniazdaProdukcyjneEnum.LiniaWloknin, result.IDProdukcjaGniazdoProdukcyjne);
        }
    }
}
