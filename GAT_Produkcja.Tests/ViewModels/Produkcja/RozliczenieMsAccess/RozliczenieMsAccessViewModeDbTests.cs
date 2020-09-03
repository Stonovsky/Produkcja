using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.dbMsAccess.UnitOfWork;
using GAT_Produkcja.Utilities.EppFile;
using GAT_Produkcja.Utilities.ExcelReport;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.RozliczenieMsAccess
{
    [TestFixture]
    public class RozliczenieMsAccessViewModeDbTests
    {
        public UnitOfWork UnitOfWork { get; private set; }

        private Mock<IXlsService> excelReportGenerator;
        private Mock<IEppFileGenerator> eppFileGenerator;
        private Mock<IUnitOfWorkMsAccess> unitOfWorkMsAccess;
        private RozliczenieMsAccesHelper sut;

        [SetUp]
        public void SetUp()
        {
            UnitOfWork = new UnitOfWork(new GAT_ProdukcjaModel());
            excelReportGenerator = new Mock<IXlsService>();
            eppFileGenerator = new Mock<IEppFileGenerator>();
            unitOfWorkMsAccess = new Mock<IUnitOfWorkMsAccess>();


        }

    }
}
