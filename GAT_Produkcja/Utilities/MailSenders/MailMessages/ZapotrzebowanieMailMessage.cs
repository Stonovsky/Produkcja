using GAT_Produkcja.db;
using GAT_Produkcja.Utilities.MailSenders.MailAddresses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.MailSenders.MailMessages
{
    public class ZapotrzebowanieMailMessage : MailMessage
    {
        private readonly tblZapotrzebowanie zapotrzebowanie;
        private readonly MailAddressesFromTo mailAddressesFromTo;
        private MailMessage message;

        public ZapotrzebowanieMailMessage(tblZapotrzebowanie zapotrzebowanie, MailAddressesFromTo mailAddressesFromTo)
        {
            this.zapotrzebowanie = zapotrzebowanie;
            this.mailAddressesFromTo = mailAddressesFromTo;
        }

        public MailMessage Create()
        {
            var urzadzenie = zapotrzebowanie.tblUrzadzenia != null ? zapotrzebowanie.tblUrzadzenia.Nazwa : "Brak";

            message = new MailMessage();
            message.Subject = $"ZAPOTRZEBOWANIE NR: {zapotrzebowanie.Nr}" +
                               ", ZGŁASZAJĄCY: " + zapotrzebowanie.tblPracownikGAT?.ImieINazwiskoGAT +
                                  ", DATA ZAPOTRZEBOWANIA: " + zapotrzebowanie.DataZapotrzebowania.Date.ToString("dd/MM/yyyy");

            message.Body = $"Zgłoszone zostało zapotrzebowanie nr: {zapotrzebowanie.Nr} \r\n" +
                           $"Data zapotrzebowania: {zapotrzebowanie.DataZapotrzebowania.Date} \r\n" +
                           $"Osoba zgłaszająca: {zapotrzebowanie.tblPracownikGAT?.ImieINazwiskoGAT}" +
                            "\r\n" +
                            "\r\nOpis: " + zapotrzebowanie?.Opis +
                            "\r\n" +
                            "\r\nZakup w firmie: " + zapotrzebowanie.tblKontrahent?.Nazwa +
                            "\r\n" +
                            "\r\nKlasyfikacja ogólna: " + zapotrzebowanie.tblKlasyfikacjaOgolna?.Nazwa +
                            "\r\nKlasyfikacja szczegółowa: " + zapotrzebowanie.tblKlasyfikacjaSzczegolowa?.Nazwa +
                            "\r\nDotyczy urządzenia: " + urzadzenie; // + zapotrzebowanie.tblUrzadzenia!=null? zapotrzebowanie.tblUrzadzenia.Nazwa : "Brak";

            message.From = (new MailAddress(mailAddressesFromTo.gmailAddressFrom));

            if (mailAddressesFromTo.mailAddressesTo.Count() == 0)
                throw new ArgumentNullException();

            foreach (var email in mailAddressesFromTo.mailAddressesTo)
                message.To.Add(new MailAddress(email));

            return message;
        }
    }
}
