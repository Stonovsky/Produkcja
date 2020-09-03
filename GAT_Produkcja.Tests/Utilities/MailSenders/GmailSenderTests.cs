using AutoFixture;
using GAT_Produkcja.db;
using GAT_Produkcja.Utilities.MailSenders;
using GAT_Produkcja.Utilities.MailSenders.MailAddresses;
using GAT_Produkcja.Utilities.MailSenders.MailMessages;
using GAT_Produkcja.Utilities.MailSenders.SmtpClientWrapper;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.Utilities.MailSenders
{
    [TestFixture]
    public class GmailSenderTests
    {
        private const string gmailAddresFrom = "produkcja.gat@gmail.com";
        private const string gmailPassword = "Wyzwolenia367A";
        private Mock<ISmtpClientWrapper> smtpClientWrapper;
        private Fixture fixture;
        private GmailSender sut;
        private tblZapotrzebowanie zapotrzebowanie;
        private ZapotrzebowanieMailMessage mailMessage;
        private List<string> mailAddressesTo;

        [SetUp]
        public void SetUp()
        {
            smtpClientWrapper = new Mock<ISmtpClientWrapper>();
            fixture = new Fixture();


            zapotrzebowanie = fixture.Build<tblZapotrzebowanie>()
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
                .With(w => w.PracownikOdpZaZakup, fixture.Build<tblPracownikGAT>()
                                                                .Without(x => x.tblZapotrzebowanie)
                                                                .Without(x => x.tblRuchNaglowek)
                                                                .Without(x => x.tblWynikiBadanGeowloknin)
                                                                .Without(x => x.tblZamowienieHandlowe)
                                                                .Create())
                .With(w => w.tblPracownikGAT, fixture.Build<tblPracownikGAT>()
                                                                .Without(x => x.tblZapotrzebowanie)
                                                                .Without(x => x.tblRuchNaglowek)
                                                                .Without(x => x.tblWynikiBadanGeowloknin)
                                                                .Without(x => x.tblZamowienieHandlowe)
                                                                .Create())
                .Create();
            mailMessage = new ZapotrzebowanieMailMessage(zapotrzebowanie, new MailAddressesFromTo()
            {
                gmailAddressFrom = gmailAddresFrom,
                mailAddressesTo = new List<string> { "tomasz.straczek@gmail.com" }
            });



            sut = CreateSut(smtpClientWrapper.Object);
        }

        private GmailSender CreateSut(ISmtpClientWrapper smtpClientWrapper)
        {
            return new GmailSender(smtpClientWrapper);
        }

        [Test]
        public async Task SendMessage_GdyArgumentyOK_MailZostajeWyslany()
        {
            await sut.SendMessageAsync(mailMessage);

            smtpClientWrapper.Verify(v => v.Send(It.IsAny<MailMessage>()));
        }

        [Ignore("Metoda testuje faktyczne wysyłanie maila, włącz ją wtedy kiedy musisz sprawdzić czy klasa wysyła maila")]
        [Test]
        public async Task SendMessage_SprawdzaCzyRZeczywiscieWysylaMaila()
        {
            sut = CreateSut(new SmtpClientWrapper());

            await sut.SendMessageAsync(mailMessage.Create());
        }
    }
}

