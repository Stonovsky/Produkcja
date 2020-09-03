using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.MailSenders;
using GAT_Produkcja.Utilities.MailSenders.MailMessages;
using GAT_Produkcja.ViewModel.Zapotrzebowanie.Ewidencja;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Threading.Tasks;
using GAT_Produkcja.db.Repositories.Repositories;

namespace GAT_Produkcja.Tests.ViewModels.Zapotrzebowanie.Ewidencja
{
    [TestFixture]
    public class ZapotrzebowanieEwidencjaViewModelTests
    {

        private Mock<IUnitOfWork> unitOfWork;
        private Mock<IUnitOfWorkFactory> unitOfWorkFactory;
        private Mock<IViewService> viewService;
        private Mock<IDialogService> dialogService;
        private Mock<IMessenger> messenger;
        private Mock<IOutlookMailSender> outlook;
        private Mock<ITblZapotrzebowanieRepository> tblZapotrzebowanie;
        private ZapotrzebowanieEwidencjaViewModel sut;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            viewService = new Mock<IViewService>();
            dialogService = new Mock<IDialogService>();
            messenger = new Mock<IMessenger>();
            outlook = new Mock<IOutlookMailSender>();

            tblZapotrzebowanie = new Mock<ITblZapotrzebowanieRepository>();
            unitOfWork.Setup(s => s.tblZapotrzebowanie).Returns(tblZapotrzebowanie.Object);


            sut = new ZapotrzebowanieEwidencjaViewModel(unitOfWork.Object, unitOfWorkFactory.Object, viewService.Object, outlook.Object, dialogService.Object, messenger.Object);
        }


        [Test]
        [Ignore("Wylaczone, zeby nie wysyalac maili. Do przetestowania gdy cos sie dzieje nie tak.")]
        public async Task Outlook_Wh_Expectations()
        {
            var outlookMail = new OutlookMailSender();

            var zapotrzebowanie = new tblZapotrzebowanie
            {
                tblZapotrzebowanieStatus = new tblZapotrzebowanieStatus { IDZapotrzebowanieStatus = 1, StatusZapotrzebowania = "Akcept" },
                Nr = 1,
                Opis = "test",
                tblPracownikGAT = new tblPracownikGAT { ImieINazwiskoGAT = "test" }
            };

            var mailItem = new ZapotrzebowanieMailItem(zapotrzebowanie,
                                            new List<string> { "tomasz.straczek@ag-geo.eu" });

            var mail = mailItem.Create();

            await outlookMail.SendAsync(mailItem.Create());
        }

        #region ZmianaStatusu
        [Test]
        public void StatusAkceptacjaCommandExecute_GdyZmieniono_WysylaMaila()
        {
            tblZapotrzebowanie.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new tblZapotrzebowanie
            {
                IDZapotrzebowanie=1,
                tblPracownikGAT = new tblPracownikGAT { ID_PracownikGAT=1, Email = "test@test.pl" },
                PracownikOdpZaZakup = new tblPracownikGAT { ID_PracownikGAT=2, Email="test"}
            });
            sut.WybraneZapotrzebowanie = new vwZapotrzebowanieEwidencja { IDZapotrzebowanie = 1 };

            sut.StatusAkceptacjaCommand.Execute(null);

            outlook.Verify(v => v.WyslijMailZZapotrzebowaniemAsync(It.IsAny<tblZapotrzebowanie>(), new List<string> { "test@test.pl", "test" , "dyrektor.produkcji@gtex.pl" }));
        }
        #endregion
    }
}
