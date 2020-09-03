using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.dbMsAccess.Models;
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
    class MsAccessImportZlecenProdukcyjnychDoSqlTests : TestBaseGeneric<MsAccessImportZlecenProdukcyjnychDoSql>
    {
        private Mock<IUnitOfWorkMsAccess> unitOfWorkMsAccess;
        private Mock<IDyspozycjeRepository> dyspozycje;
        private Mock<INormyZuzyciaRepository> normyZuzycia;
        private Mock<ISurowiecRepository> surowiec;
        private Mock<ITblProdukcjaZlecenieRepository> tblProdukcjaZlecenie;
        private Mock<ITblProdukcjaZlecenieProdukcyjne_MieszankaRepository> tblProdukcjaZlecenieProdukcyjne_Mieszanka;
        private Mock<ITblProdukcjaZlecenieTowarRepository> tblProdukcjaZlecenieTowar;

        public override void SetUp()
        {
            base.SetUp();

            unitOfWorkMsAccess = new Mock<IUnitOfWorkMsAccess>();

            dyspozycje = new Mock<IDyspozycjeRepository>();
            normyZuzycia = new Mock<INormyZuzyciaRepository>();
            surowiec = new Mock<ISurowiecRepository>();

            unitOfWorkMsAccess.Setup(s => s.Dyspozycje).Returns(dyspozycje.Object);
            unitOfWorkMsAccess.Setup(s => s.NormyZuzycia).Returns(normyZuzycia.Object);
            unitOfWorkMsAccess.Setup(s => s.Surowiec).Returns(surowiec.Object);

            tblProdukcjaZlecenie = new Mock<ITblProdukcjaZlecenieRepository>();
            tblProdukcjaZlecenieProdukcyjne_Mieszanka = new Mock<ITblProdukcjaZlecenieProdukcyjne_MieszankaRepository>();
            tblProdukcjaZlecenieTowar = new Mock<ITblProdukcjaZlecenieTowarRepository>();

            UnitOfWork.Setup(s => s.tblProdukcjaZlecenie).Returns(tblProdukcjaZlecenie.Object);
            UnitOfWork.Setup(s => s.tblProdukcjaZlecenieProdukcyjne_Mieszanka).Returns(tblProdukcjaZlecenieProdukcyjne_Mieszanka.Object);
            UnitOfWork.Setup(s => s.tblProdukcjaZlecenieTowar).Returns(tblProdukcjaZlecenieTowar.Object);

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new MsAccessImportZlecenProdukcyjnychDoSql(UnitOfWork.Object, unitOfWorkMsAccess.Object);
        }

        [Test]
        [Ignore("Uruchomic gdy potrzeba - dodaje zlecenia z MS Access do bazy")]
        public async Task RealInvoke()
        {
            var uow = new UnitOfWork(new GAT_ProdukcjaModel());
            var uowAccess = new UnitOfWorkMsAccess();
            var liniaWlokninImport = new MsAccessImportZlecenProdukcyjnychDoSql(uow, uowAccess);

            await liniaWlokninImport.ImportZleceniaProdukcyjne();
        }

        [Test]
        public void GdyZleceniaZMsAccessWczesniejZapisaneWBazie_NieZapisujeWBaziePonownieTychSamychZlecenProdukcyjnych()
        {
            dyspozycje.Setup(s => s.GetAllAsync()).ReturnsAsync(
                new List<Dyspozycje>
                {
                    new Dyspozycje{Id=1},
                    new Dyspozycje{Id=2},
                });
            normyZuzycia.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<NormyZuzycia>
            {
                new NormyZuzycia{ZlecenieID=1, Artykul="ALTEX AT PP 90"},
                new NormyZuzycia{ZlecenieID=2, Artykul="ALTEX AT PP 100"}
            });

            tblProdukcjaZlecenie.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenie, bool>>>()))
                                .ReturnsAsync(new List<tblProdukcjaZlecenie>
                                {
                                    new tblProdukcjaZlecenie{IDMsAccess=1},
                                    new tblProdukcjaZlecenie{IDMsAccess=2}
                                });

            sut.ImportZleceniaProdukcyjne();

            tblProdukcjaZlecenie.Verify(x => x.AddRange(It.IsAny<IEnumerable<tblProdukcjaZlecenie>>()),Times.Never);
            UnitOfWork.Verify(x => x.SaveAsync(),Times.Never);
        }

        [Test]
        [Ignore("Test bledny")]
        public void GdyZleceniaZMsAccessNieZapisaneWBazieSQL_Zapisuje()
        {
            dyspozycje.Setup(s => s.GetAllAsync()).ReturnsAsync(
                new List<Dyspozycje>
                {
                    new Dyspozycje{Id=1},
                    new Dyspozycje{Id=2},
                });
            normyZuzycia.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<NormyZuzycia>
            {
                new NormyZuzycia{ZlecenieID=1, Artykul="ALTEX AT PP 90"},
                new NormyZuzycia{ZlecenieID=2, Artykul="ALTEX AT PP 100"}
            });

            surowiec.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<Surowiec> { });

            tblProdukcjaZlecenie.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenie, bool>>>()))
                                .ReturnsAsync(new List<tblProdukcjaZlecenie>
                                {
                                    new tblProdukcjaZlecenie{IDMsAccess=1},
                                });

            sut.ImportZleceniaProdukcyjne();

            tblProdukcjaZlecenie.Verify(x => x.AddRange(It.IsAny<IEnumerable<tblProdukcjaZlecenie>>()));
            UnitOfWork.Verify(x => x.SaveAsync());
        }
    }
}
