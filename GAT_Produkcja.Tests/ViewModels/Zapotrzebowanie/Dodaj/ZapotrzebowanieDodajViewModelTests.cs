using AutoFixture;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.FilesManipulations;
using GAT_Produkcja.Utilities.MailSenders;
using GAT_Produkcja.Utilities.MailSenders.MailMessages;
using GAT_Produkcja.ViewModel.Zapotrzebowanie.Dodaj;
using GAT_Produkcja.ViewModel.Zapotrzebowanie.Messages;
using GAT_Produkcja.ViewModel.Zapotrzebowanie.Pliki;
using Microsoft.Office.Interop.Outlook;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Zapotrzebowanie.Dodaj
{
    [TestFixture]
    public class ZapotrzebowanieDodajViewModelTests
    {
        private Mock<IUnitOfWork> unitOfWork;
        private Mock<IUnitOfWorkFactory> unitOfWorkFactory;
        private Mock<IViewService> viewService;
        private Mock<IDialogService> dialogService;
        private Mock<IMessenger> messenger;
        private Messenger messengerOrg;
        private Mock<IFilesManipulation> filesManipulation;
        private Mock<IGmailSender> gmailSender;
        private Mock<IPlikiCRUD> plikiCRUD;
        private Fixture fixture;
        private Mock<IOutlookMailSender> outlook;
        private Mock<ITblZapotrzebowanieRepository> tblZapotrzebowanie;
        private Mock<ITblZapotrzebowaniePozycjeRepository> tblZapotrzebowaniePozycje;
        private Mock<ITblKontrahentRepository> tblKontrahent;
        private Mock<ITblPlikiRepository> tblPliki;
        private ZapotrzebowanieDodajViewModel sut;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            viewService = new Mock<IViewService>();
            dialogService = new Mock<IDialogService>();
            messenger = new Mock<IMessenger>();
            messengerOrg = new Messenger();
            filesManipulation = new Mock<IFilesManipulation>();
            gmailSender = new Mock<IGmailSender>();
            plikiCRUD = new Mock<IPlikiCRUD>();
            fixture = new Fixture();
            outlook = new Mock<IOutlookMailSender>();
            tblZapotrzebowanie = new Mock<ITblZapotrzebowanieRepository>();
            unitOfWork.Setup(u => u.tblZapotrzebowanie).Returns(tblZapotrzebowanie.Object);

            tblZapotrzebowaniePozycje = new Mock<ITblZapotrzebowaniePozycjeRepository>();
            unitOfWork.Setup(s => s.tblZapotrzebowaniePozycje).Returns(tblZapotrzebowaniePozycje.Object);

            tblKontrahent = new Mock<ITblKontrahentRepository>();
            unitOfWork.Setup(s => s.tblKontrahent).Returns(tblKontrahent.Object);

            tblPliki = new Mock<ITblPlikiRepository>();
            unitOfWork.Setup(s => s.tblPliki).Returns(tblPliki.Object);


            sut = PobierzViewModel(messenger.Object);
        }

        private ZapotrzebowanieDodajViewModel PobierzViewModel(IMessenger messenger)
        {
            return new ZapotrzebowanieDodajViewModel(unitOfWork.Object,
                                                            unitOfWorkFactory.Object,
                                                            viewService.Object,
                                                            dialogService.Object,
                                                            filesManipulation.Object,
                                                            gmailSender.Object,
                                                            plikiCRUD.Object,
                                                            outlook.Object,
                                                            messenger);
        }

        private tblZapotrzebowanie StworzZapotrzebowanie()
        {
            return fixture.Build<tblZapotrzebowanie>()
                .Without(w => w.tblUrzadzenia)
                .Without(w => w.tblZapotrzebowaniePozycje)
                .Without(w => w.tblZapotrzebowanieStatus)
                .Without(w => w.tblPliki)
                .With(w => w.tblKlasyfikacjaOgolna, fixture.Build<tblKlasyfikacjaOgolna>()
                                                               .Without(x => x.tblZapotrzebowanie).Create())
                .With(w => w.tblKlasyfikacjaSzczegolowa, fixture.Build<tblKlasyfikacjaSzczegolowa>()
                                                                .Without(x => x.tblZapotrzebowanie).Create())
                .With(w => w.tblKontrahent, fixture.Build<tblKontrahent>()
                                                                .Without(x => x.tblRuchNaglowek)
                                                                .Without(x => x.tblZamowienieHandlowe)
                                                                .Without(x => x.tblZamowienieHandlowe1)
                                                                .Without(x => x.tblZapotrzebowanie)
                                                                .Create())
                .With(w => w.tblPracownikGAT, fixture.Build<tblPracownikGAT>()
                                                                .Without(x => x.tblZapotrzebowanie)
                                                                .Without(x => x.tblRuchNaglowek)
                                                                .Without(x => x.tblWynikiBadanGeowloknin)
                                                                .Without(x => x.tblZamowienieHandlowe)
                                                                .Create())                
                .With(w => w.PracownikOdpZaZakup, fixture.Build<tblPracownikGAT>()
                                                                .Without(x => x.tblZapotrzebowanie)
                                                                .Without(x => x.tblRuchNaglowek)
                                                                .Without(x => x.tblWynikiBadanGeowloknin)
                                                                .Without(x => x.tblZamowienieHandlowe)
                                                                .Create())
                .Create();
        }

        #region ZapiszCommand

        #region CanExecute
        [Test]
        public void ZapiszCommandCanExecute_ZapotrzebowaniIsNotValid_False()
        {
            sut.Zapotrzebowanie.IsValid = false;

            Assert.IsFalse(sut.ZapiszCommand.CanExecute(null));
        }

        [Test]
        public void ZapiszCommandCanExecute_ZapotrzebowaniIsValidKontrahentIs0_False()
        {
            sut.Zapotrzebowanie.IsValid = true;
            sut.Zapotrzebowanie.IDKontrahent = 0;

            Assert.IsFalse(sut.ZapiszCommand.CanExecute(null));
        }
        [Test]
        public void ZapiszCommandCanExecute_ListaPozycjiIsNull_False()
        {
            sut.Zapotrzebowanie.IsValid = true;
            sut.Zapotrzebowanie.IDKontrahent = 1;
            sut.ListaPozycjiZapotrzebowan = null;

            Assert.IsFalse(sut.ZapiszCommand.CanExecute(null));
        }
        [Test]
        public void ZapiszCommandCanExecute_ListaPozycjiIsZero_False()
        {
            sut.Zapotrzebowanie.IsValid = true;
            sut.Zapotrzebowanie.IDKontrahent = 1;
            sut.ListaPozycjiZapotrzebowan = null;

            Assert.IsFalse(sut.ZapiszCommand.CanExecute(null));
        }

        [Test]
        public void ZapiszCommandCanExecute_EverythingIsValidated_True()
        {
            ZapiszCanExecuteTrue();

            Assert.IsTrue(sut.ZapiszCommand.CanExecute(null));
        }
        [Test]
        public void ZapiszCommandExecute_IDZapotrzebowaniIsZero_InvokeSendMessageAsync()
        {
            ZapiszCanExecuteTrue();
            sut.Zapotrzebowanie.IDZapotrzebowanie = 0;

            sut.ZapiszCommand.Execute(null);

            gmailSender.Verify(s => s.SendMessageAsync(It.IsAny<MailMessage>()));
        }

        private void ZapiszCanExecuteTrue()
        {
            sut.Zapotrzebowanie = StworzZapotrzebowanie();
            sut.Zapotrzebowanie.DataZgloszenia = new DateTime(2020, 1, 1);
            sut.Zapotrzebowanie.DataZapotrzebowania = new DateTime(2020, 1, 2);
            sut.ListaPozycjiZapotrzebowan = new ObservableCollection<tblZapotrzebowaniePozycje> { new tblZapotrzebowaniePozycje { IDZapotrzebowanie = 1 } };
        }


        #endregion        

        #region Execute
        [Test]
        public void ZapiszCommandExecute_AddTblsToEntity()
        {
            ZapiszCanExecuteTrue();
            sut.Zapotrzebowanie = new tblZapotrzebowanie
            {
                IDZapotrzebowanie = 0,
                IDPracownikGAT = 4,
                IDKlasyfikacjaOgolna = 1,
                IDKlasyfikacjaSzczegolowa = 1,
                IDFirma = 4,
            };

            sut.Zapotrzebowanie.tblPracownikGAT = new tblPracownikGAT { ID_PracownikGAT = 4 };

            Assert.IsTrue(sut.Zapotrzebowanie.tblPracownikGAT.ID_PracownikGAT == 4);
        }

        [Test]
        public void ZapiszCommandExecute_Whenfinished_IsChanged_False()
        {
            ZapiszCanExecuteTrue();

            sut.ZapiszCommand.Execute(null);

            Assert.IsFalse(sut.IsChanged);
        }

        #region ZmianaStatusu

        [Test]
        public void ZapiszCommandExecute_GdyNieZmienionoStatusu_DialogBoxInfoNieJestPokazanyPrzyZapisie()
        {
            ZapiszCanExecuteTrue();
            sut.ZapotrzebowanieOrg = sut.Zapotrzebowanie;

            sut.ZapiszCommand.Execute(null);

            dialogService.Verify(v => v.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>()),Times.Never);
        }

        [Test]
        public void ZapiszCommandExecute_GdyZmienionoStatus_DialogBoxInfoJestPokazanyPrzyZapisie()
        {
            ZapiszCanExecuteTrue();
            sut.WybranyStatus = new tblZapotrzebowanieStatus { IDZapotrzebowanieStatus = 1 };

            sut.ZapiszCommand.Execute(null);

            dialogService.Verify(v => v.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        [Ignore("test uruchamiac na komputerze z outlookiem")]
        public void ZapiszCommandExecute_GdyZmienionoStatusIDialogBoxTrue_WysylaMailaZOutlooka()
        {
            ZapiszCanExecuteTrue();
            dialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            sut.WybranyStatus = new tblZapotrzebowanieStatus { IDZapotrzebowanieStatus = 1 };

            sut.ZapiszCommand.Execute(null);

            outlook.Verify(v => v.SendAsync(It.IsAny<MailItem>()));
        }

        #endregion
        #endregion
        #endregion

        [Test]
        public void ZamknijOknoCommand_GdyWprowadzonoZmiany_DialogServicePytaOZamkniecie()
        {
            sut.Zapotrzebowanie.CzyZweryfikowano = true;

            sut.ZamknijOknoCommand.Execute(null);

            dialogService.Verify(v => v.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        public void ZamknijOknoCommand_GdyNieWprowadzonoZmian_ZamknijOkno()
        {
            sut.Zapotrzebowanie.CzyZweryfikowano = false;

            sut.ZamknijOknoCommand.Execute(null);

            viewService.Verify(v => v.Close(It.IsAny<string>()));
        }

        [Test]
        public void ZaladujWartosciPoczatkoweCommand_ZapotrzebowanieKlonowane_IsChanged_False()
        {
            sut = PobierzViewModel(messengerOrg);
            sut.ListaPracownikowGAT = new List<tblPracownikGAT> { new tblPracownikGAT { ID_PracownikGAT = 1 } };
            tblZapotrzebowanie.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new db.tblZapotrzebowanie { IDZapotrzebowanie = 1, CzyZweryfikowano = true, IDPracownikGAT = 1 });

            messengerOrg.Send(new vwZapotrzebowanieEwidencja { IDZapotrzebowanie = 1, CzyZweryfikowano = true });

            Assert.IsFalse(sut.IsChanged);
        }

        [Test]
        public void ZamknijOknoCommand_GdyIsChanged_False_DialoServiceNieJestWywolany()
        {
            sut.ListaPracownikowGAT = new List<tblPracownikGAT> { new tblPracownikGAT { ID_PracownikGAT = 1 } };
            tblZapotrzebowanie.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new db.tblZapotrzebowanie { IDZapotrzebowanie = 1, CzyZweryfikowano = true, IDPracownikGAT = 1 });

            sut.ZamknijOknoCommand.Execute(null);

            dialogService.Verify(V => V.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        #region DodajPozycje_IsChanged
        [Test]
        public void MessengerRegisteredTo_Condition_Expectations()
        {
            messenger.Verify(v => v.Register<tblZapotrzebowaniePozycje>(sut, "Dodaj", It.IsAny<Action<tblZapotrzebowaniePozycje>>(), It.IsAny<bool>()));
            messenger.Verify(v => v.Register<tblZapotrzebowaniePozycje>(sut, "Usun", It.IsAny<Action<tblZapotrzebowaniePozycje>>(), It.IsAny<bool>()));
            messenger.Verify(v => v.Register<tblZapotrzebowaniePozycje>(sut, "Edytuj", It.IsAny<Action<tblZapotrzebowaniePozycje>>(), It.IsAny<bool>()));

        }
        #endregion

    }
}
