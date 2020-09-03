using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Tests.Repositories
{
    [TestFixture]
    [Ignore("Uruchamiac kiedy trzeba")]
    class TblProdukcjaRuchTowarRepositoryTests
    {
        private UnitOfWork sut;

        [SetUp]
        public void SetUp()
        {
            sut = new UnitOfWork(new GAT_ProdukcjaModel());
        }
        [Test]
        public void PobierzZBazy_PobranaListaNiepusta()
        {
            var lista = sut.tblProdukcjaRuchTowar.Where(t => t.IDProdukcjaGniazdoProdukcyjne==2
                                                          && t.KodKreskowy == "728-Z/1"
                                                          //&& (t.IDProdukcjaRuchTowarStatus == null || t.IDProdukcjaRuchTowarStatus!=(int)GAT_Produkcja.db.Enums.ProdukcjaRuchTowarStatusEnum.Rozchodowano)
                                                          && t.IDProdukcjaRozliczenieStatus == 1
                                                          );

            var lista2=  sut.tblProdukcjaRuchTowar
                            .Where(r => r.IDZleceniePodstawowe == 264
                            && r.WagaOdpad_kg > 0
                            && r.IDProdukcjaRozliczenieStatus != (int)ProdukcjaRozliczenieStatusEnum.Rozliczono);


            //lista = sut.tblProdukcjaRuchTowar.Where(t => t.KodKreskowy.Contains("755"));
            //lista = sut.tblProdukcjaRuchTowar.Where(t => t.KodKreskowy=="755-p/4");

            Assert.IsNotEmpty(lista);
        }
    }
}
