using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Tests.Repositories
{
    [TestFixture
    ,Ignore("Testy na bazie danych. Wlaczac kiedy trzeba")]
    public class VwTowarGTXRepositoryTests
    {
        private UnitOfWork sut;

        [SetUp]
        public void SetUp()
        {
            sut = new UnitOfWork(new GAT_ProdukcjaModel());

        }

        [Test]
        [TestCase(90, "PP", false)]
        //[TestCase(90, "PES", false)]
        //[TestCase(100, "PES", false)]
        //[TestCase(100, "PP", false)]
        public async Task PobierzTowarZParametrow_GdyParametryOk_PobierzTowar(int gramaturaTowaru, string surowiecSkrot, bool czyUv)
        {
            var gramatura = new tblTowarGeowlokninaParametryGramatura { IDTowarGeowlokninaParametryGramatura = 1, Gramatura = gramaturaTowaru };
            var surowiec = new tblTowarGeowlokninaParametrySurowiec { IDTowarGeowlokninaParametrySurowiec = 1, Skrot = surowiecSkrot };

            var towar = await sut.vwTowarGTX.PobierzTowarZParametrowAsync(gramatura, surowiec, czyUv);

            Assert.IsNotNull(towar);
        }

        [Test]
        [TestCase(90, "PES", true, 0)]
        [TestCase(90, "PP", true, 2)]

        public async Task PobierzTowarZParametrow_GdyParametryNieOk_ZwracaNull(int gramaturaTowaru, string surowiecSkrot, bool czyUv, int idTowar)
        {
            var gramatura = new tblTowarGeowlokninaParametryGramatura { IDTowarGeowlokninaParametryGramatura = 1, Gramatura = gramaturaTowaru };
            var surowiec = new tblTowarGeowlokninaParametrySurowiec { IDTowarGeowlokninaParametrySurowiec = 1, Skrot = surowiecSkrot };

            var towar = await sut.vwTowarGTX.PobierzTowarZParametrowAsync(gramatura, surowiec, czyUv);

            Assert.AreEqual(null, towar);
        }

    }
}
