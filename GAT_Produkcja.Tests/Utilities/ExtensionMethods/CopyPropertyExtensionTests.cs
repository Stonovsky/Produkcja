using GAT_Produkcja.db;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.Utilities.ExtensionMethods
{
    [TestFixture]
    public class CopyPropertyExtensionTests 
    {
        [Test]
        public void CopyListPropertiesTo_KopiujeWlasciwosci()
        {
            var towar1 = new List<tblTowar>
            {
                new tblTowar{IDTowar=1},
                new tblTowar{IDTowar=2},
            };

            var towar2 = towar1.CopyList();

            Assert.AreEqual(2, towar2.Count());
        }

    }
}
