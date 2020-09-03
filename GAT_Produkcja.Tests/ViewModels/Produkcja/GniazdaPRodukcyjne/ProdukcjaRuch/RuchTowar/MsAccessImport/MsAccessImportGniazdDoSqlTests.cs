using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.dbMsAccess.Models.Adapters;
using GAT_Produkcja.dbMsAccess.Repositories;
using GAT_Produkcja.dbMsAccess.UnitOfWork;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.MsAccessImport;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.MsAccessImport
{
    public class MsAccessImportGniazdDoSqlTests : TestBaseGeneric<MsAccessImportGniazdDoSql>
    {
        private Mock<IUnitOfWorkMsAccess> unitOfWorkMsAccess;
        private Mock<IProdukcjaRepository> produkcja;
        private Mock<ITblProdukcjaRuchTowarRepository> tblProdukcjaRuchTowar;
        private Mock<ITblProdukcjaZlecenieRepository> tblProdukcjaZlecenie;
        private Mock<ITblProdukcjaRuchNaglowekRepository> tblProdukcjaRuchNaglowek;

        public override void SetUp()
        {
            base.SetUp();

            unitOfWorkMsAccess = new Mock<IUnitOfWorkMsAccess>();
            produkcja = new Mock<IProdukcjaRepository>();

            unitOfWorkMsAccess.Setup(s => s.Produkcja).Returns(produkcja.Object);

            tblProdukcjaRuchTowar = new Mock<ITblProdukcjaRuchTowarRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaRuchTowar).Returns(tblProdukcjaRuchTowar.Object);
            tblProdukcjaZlecenie = new Mock<ITblProdukcjaZlecenieRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaZlecenie).Returns(tblProdukcjaZlecenie.Object);
            tblProdukcjaRuchNaglowek = new Mock<ITblProdukcjaRuchNaglowekRepository>();

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new MsAccessImportGniazdDoSql(UnitOfWork.Object, unitOfWorkMsAccess.Object);
        }

        [Test]
        [Ignore("Uruchomic gdy potrzeba - ruch towaru z MSAccess do bazy")]
        public async Task RealInvoke()
        {
            var uow = new UnitOfWork(new GAT_ProdukcjaModel());
            var uowAccess = new UnitOfWorkMsAccess();
            var msAccessImport = new MsAccessImportGniazdDoSql(uow, uowAccess);

            //await msAccessImport.ImportLiniaWloknin();
            //await msAccessImport.ImportLiniaKalandra();
            await msAccessImport.ImportLiniaKonfekcji();
        }


        [Test]
        public void ProdukcjaAdapter_SprawdzeniePoprawnosciKonwersji()
        {
            var pozycjaProdukcja = new dbMsAccess.Models.Produkcja()
            {
                Artykul = "ALTEX AT PES 90",
                Data = new DateTime(2020,1,1),
                NrSztuki = "t1",
                Id = 1,
                Zlecenie="180",
                ZlecenieID=1,
                Godzina= new DateTime() + new TimeSpan(11,0,0)
            };

            var pozycjaAdapter = new Produkcja_tblProdukcjaRuchTowarAdapter(pozycjaProdukcja);

            Assert.AreEqual(1, pozycjaAdapter.IDMsAccess);
            Assert.AreEqual(new DateTime(2020,1,1,11,0,0), pozycjaAdapter.DataDodania);
            Assert.AreEqual("t1", pozycjaAdapter.NrRolkiPelny);
            Assert.AreEqual(null, pozycjaAdapter.IDRolkaBazowa);
            Assert.AreEqual(3, pozycjaAdapter.IDGramatura);
            Assert.AreEqual("180", pozycjaAdapter.ZlecenieNazwa);
            Assert.IsNotEmpty(pozycjaAdapter.SurowiecSkrot);
        }


        [Test]
        [Ignore("Test bledny")]
        public async Task Import_GdyListaZSQLNull_WywolujMetodeAddRange()
        {
            produkcja.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<dbMsAccess.Models.Produkcja>
            {
                new dbMsAccess.Models.Produkcja()
                {
                    Artykul = "ALTEX AT PES 90",
                    Data = new DateTime(2020,1,1),
                    NrSztuki = "t1",
                    Id = 1,
                    Zlecenie="180",
                    ZlecenieID=1,
                    Godzina= new DateTime() + new TimeSpan(11,0,0)
                },
                new dbMsAccess.Models.Produkcja()
                {
                    Artykul = "ALTEX AT PES 90",
                    Data = new DateTime(2020,1,1),
                    NrSztuki = "t1",
                    Id = 2,
                    Zlecenie="180",
                    ZlecenieID=1,
                    Godzina= new DateTime() + new TimeSpan(11,0,0)
                },
            });

            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()));
            tblProdukcjaZlecenie.Setup(x => x.GetAll())
                                .Returns(new List<tblProdukcjaZlecenie>
                                {
                                    new tblProdukcjaZlecenie{IDMsAccess=1}
                                });

            await sut.ImportLiniaWloknin();

            tblProdukcjaRuchNaglowek.Verify(x => x.Add(It.IsAny<tblProdukcjaRuchNaglowek>()));
        }


        [Test]
        [Ignore("Test bledny")]
        public async Task Import_GdyDaneOk_WywolujMetodeAddRange()
        { 
            produkcja.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<dbMsAccess.Models.Produkcja>
            {
                new dbMsAccess.Models.Produkcja()
                {
                    Artykul = "ALTEX AT PES 90",
                    Data = new DateTime(2020,1,1),
                    NrSztuki = "t1",
                    Id = 1,
                    Zlecenie="180",
                    ZlecenieID=1,
                    Godzina= new DateTime() + new TimeSpan(11,0,0)
                },
                new dbMsAccess.Models.Produkcja()
                {
                    Artykul = "ALTEX AT PES 90",
                    Data = new DateTime(2020,1,1),
                    NrSztuki = "t1",
                    Id = 2,
                    Zlecenie="180",
                    ZlecenieID=1,
                    Godzina= new DateTime() + new TimeSpan(11,0,0)
                },
            });

            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()))
                                 .ReturnsAsync(new List<tblProdukcjaRuchTowar>
                                 {
                                     new tblProdukcjaRuchTowar{IDMsAccess=2}
                                 });

            await sut.ImportLiniaWloknin();

            tblProdukcjaRuchTowar.Verify(x => x.AddRange(It.IsAny<IEnumerable<tblProdukcjaRuchTowar>>()));
        }
        [Test]
        public async Task Import_GdyWBazieSQLWszystkieRekordy_NieWywolujMetodyAddRange()
        {
            produkcja.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<dbMsAccess.Models.Produkcja>
            {
                new dbMsAccess.Models.Produkcja()
                {
                    Artykul = "ALTEX AT PES 90",
                    Data = new DateTime(2020,1,1),
                    NrSztuki = "t1",
                    Id = 1,
                    Zlecenie="180",
                    ZlecenieID=1,
                    Godzina= new DateTime() + new TimeSpan(11,0,0)
                },
                new dbMsAccess.Models.Produkcja()
                {
                    Artykul = "ALTEX AT PES 90",
                    Data = new DateTime(2020,1,1),
                    NrSztuki = "t1",
                    Id = 2,
                    Zlecenie="180",
                    ZlecenieID=1,
                    Godzina= new DateTime() + new TimeSpan(11,0,0)
                },
            });

            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()))
                                 .ReturnsAsync(new List<tblProdukcjaRuchTowar>
                                 {
                                     new tblProdukcjaRuchTowar{IDMsAccess=1},
                                     new tblProdukcjaRuchTowar{IDMsAccess=2}
                                 });

            await sut.ImportLiniaWloknin();

            tblProdukcjaRuchTowar.Verify(x => x.AddRange(It.IsAny<IEnumerable<tblProdukcjaRuchTowar>>()),Times.Never);
        }

        [Test]
        public void MethodName_Condition_Expectations()
        {
            var access = new dbMsAccess.Models.Produkcja()
            {
                Artykul = "ALTEX AT PES 90",
                Data = new DateTime(2020, 1, 1),
                NrSztuki = "t1",
                Id = 1,
                Zlecenie = "180",
                ZlecenieID = 1,
                Godzina = new DateTime() + new TimeSpan(11, 0, 0)
            };

            var adapter = new Produkcja_tblProdukcjaRuchTowarAdapter(access);

            var listaTbl = new List<tblProdukcjaRuchTowar>();
            listaTbl.Add(adapter);


        }
    }
}
