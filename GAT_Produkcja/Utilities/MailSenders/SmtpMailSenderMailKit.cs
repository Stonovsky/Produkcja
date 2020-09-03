using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace GAT_Produkcja.Utilities.MailSenders
{
    public class SmtpMailSenderMailKit
    {
        private const string MAIL_ADDRESS_FROM = "produkcja.gat@gmail.com";
        private const string MAIL_ADDRESS_TO = "tomasz.straczek@gmail.com";//"oleg.bielakow@alians-trade.eu";
        private const string PASSWORD = "Wyzwolenia367A";
        private const string SMTP_GMAIL = "smtp.gmail.com";
        private const int SMTP_CHANNEL = 587;
        private readonly IMessenger messenger;
        private MimeMessage message;
        private vwZapotrzebowanieEwidencja zapotrzebowanie;

        public SmtpMailSenderMailKit(IMessenger messenger)
        {
            this.messenger = messenger;

            this.messenger.Register<vwZapotrzebowanieEwidencja>(this, GdyPrzeslanoZapotrzebowanie);
        }

        private void GdyPrzeslanoZapotrzebowanie(vwZapotrzebowanieEwidencja obj)
        {
            zapotrzebowanie = obj;
        }

        private void CreateMessage()
        {
            message = new MimeMessage();

            message.Subject = "ZAPOTRZEBOWANIE NR: " + zapotrzebowanie.Nr +
                                  ", ZGŁASZAJĄCY: " + zapotrzebowanie.OsobaZglZap +
                                  ", DATA ZAPOTRZEBOWANIA: " + zapotrzebowanie.DataZapotrzebowania;
            message.Body = new TextPart
            {
                Text = "Zgłoszone zostało zapotrzebowanie nr: " + zapotrzebowanie.Nr +
                                    "\r\nData zapotrzebowania: " + zapotrzebowanie.DataZapotrzebowania +
                                    "\r\nOsoba zgłaszająca: " + zapotrzebowanie.OsobaZglZap +
                                    "\r\nOpis: " + zapotrzebowanie.Opis +
                                    "\r\nZakup w firmie: " + zapotrzebowanie.ZakupOd+
                                    "\r\nKlasyfikacja ogólna: " + zapotrzebowanie.KlasyfikacjaOgolna+
                                    "\r\nKlasyfikacja szczegółowa: " + zapotrzebowanie.KlasyfikacjaSzczegolowa+
                                    "\r\nDotyczy urządzenia: " + zapotrzebowanie.Urzadzenie
            };

            message.From.Add(new MailboxAddress(zapotrzebowanie.OsobaZglZap, MAIL_ADDRESS_FROM));
            message.To.Add(new MailboxAddress("Zapotrzebowanie", MAIL_ADDRESS_TO));


        }

        public async Task SendMessageAsync()
        {
            if (zapotrzebowanie == null)
            {
                return;
            }

            CreateMessage();
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(SMTP_GMAIL, SMTP_CHANNEL, SecureSocketOptions.StartTlsWhenAvailable).ConfigureAwait(false);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(MAIL_ADDRESS_FROM, PASSWORD).ConfigureAwait(false);
                    await client.SendAsync(message).ConfigureAwait(false);
                    await client.DisconnectAsync(true).ConfigureAwait(false);
                }
                catch (Exception e)
                {

                    throw;
                }

            }
        }
    }
}
