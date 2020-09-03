using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Tests.Helpers
{
    [TestFixture]
    public class FeedDbHelperTests
    {
        private UnitOfWork unitOfWork;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = new UnitOfWork(new GAT_ProdukcjaModel());
        }

        [Test]
        [Ignore ("Test dodajacy do bazy encje")]
        public async Task FeedDB_tblTowarGeowlokninaParametry()
        {
            //var parametryWyjsciowe = await unitOfWork.tblTowarGeowlokninaParametry.WhereAsync(e => e.IDTowarGeowlokninaParametrySurowiec == (int)TowarGeowlokninaSurowiecEnum.PP
            //                                                                      && e.CzyBadanieAktualne == true);

            var contex = new GAT_ProdukcjaModel();

            var parametry= await contex.tblTowarGeowlokninaParametry.Where(e => e.IDTowarGeowlokninaParametrySurowiec == (int)TowarGeowlokninaSurowiecEnum.PP
                                                                       && e.CzyBadanieAktualne == true).AsNoTracking().ToListAsync();

            parametry.ForEach(e =>
            {
                e.OdpornoscNaWarunkiAtmosferyczne = "Nalezy przykryc w ciagu 30 dni od wbudowania.";
                e.CzyUV = true;
                e.NrDWU = e.NrDWU + "_UV";
                e.IDTowarGeowlokninaParametry = 0;

            });

            contex.tblTowarGeowlokninaParametry.AddRange(parametry);
            await contex.SaveChangesAsync();
        }
    }
}
