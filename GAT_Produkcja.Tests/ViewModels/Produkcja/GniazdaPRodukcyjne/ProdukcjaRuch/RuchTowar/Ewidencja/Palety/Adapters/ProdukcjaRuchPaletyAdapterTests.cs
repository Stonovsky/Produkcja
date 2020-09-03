using GAT_Produkcja.db;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Ewidencja.Palety.Adapters;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Ewidencja.Palety.Adapters
{
    public class ProdukcjaRuchPaletyAdapterTests : TestBaseGeneric<ProdukcjaRuchPaletyAdapter>
    {
        private IEnumerable<tblProdukcjaRuchTowar> listOfEntities;

        public override void SetUp()
        {
            base.SetUp();

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new ProdukcjaRuchPaletyAdapter(listOfEntities);
        }

        [Test]
        public void GroupBy_NrPalety_Grupuje()
        {
            listOfEntities = new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{NrPalety=1, DataDodania=DateTime.Now.Date.AddDays(-1), TowarNazwaSubiekt="T1",TowarSymbolSubiekt="t1", },
                new tblProdukcjaRuchTowar{NrPalety=1, DataDodania=DateTime.Now.Date.AddDays(-1), TowarNazwaSubiekt="T2",TowarSymbolSubiekt="t2", },
                new tblProdukcjaRuchTowar{NrPalety=2, DataDodania=DateTime.Now.Date.AddDays(-1), TowarNazwaSubiekt="T3",TowarSymbolSubiekt="t3", },
            };
            CreateSut();

            var grouped = sut.GroupBy(g => new { g.NrPalety});

            Assert.AreEqual(2, grouped.Count());
        }
    }
}
