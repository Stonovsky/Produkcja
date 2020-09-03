using GAT_Produkcja.db;
using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace GAT_Produkcja.Utilities.MailSenders.MailMessages
{
    public class ZapotrzebowanieMailItem
    {
        private readonly tblZapotrzebowanie zapotrzebowanie;
        private readonly List<string> mailAddressesTo;

        public ZapotrzebowanieMailItem(tblZapotrzebowanie zapotrzebowanie, List<string> mailAddressesTo)
        {
            this.zapotrzebowanie = zapotrzebowanie;
            this.mailAddressesTo = mailAddressesTo;
        }

        /// <summary>
        /// Creates a mail item for Zapotrzebowanie
        /// </summary>
        /// <returns></returns>
        public MailItem Create()
        {
            if (zapotrzebowanie == null)
                return null;

            var app = new Outlook.Application();

            MailItem mailItem = app.CreateItem(Outlook.OlItemType.olMailItem);

            mailItem.Subject = zapotrzebowanie.tblZapotrzebowanieStatus.StatusZapotrzebowania +
                    " dla zap. nr: " + zapotrzebowanie.Nr +
                    ", opis: " + zapotrzebowanie.Opis;
            mailItem.To = CombineAddressesTo(); //"tomasz.straczek@gmail.com"
            mailItem.Body = "Zmieniono status na: " + zapotrzebowanie.tblZapotrzebowanieStatus?.StatusZapotrzebowania +
                            "\r\n" +
                            "Dla zapotrzebowania nr: " + zapotrzebowanie?.Nr + ", dotyczącego: " + zapotrzebowanie?.Opis +
                            "\r\n" +
                            "Zgłoszonego przez: " + zapotrzebowanie.tblPracownikGAT?.ImieINazwiskoGAT;
            mailItem.Importance = Outlook.OlImportance.olImportanceHigh;
            //mailItem.Display(false);

            return mailItem;
        }

        /// <summary>
        /// Combine array of mail addresses to one string of addressess
        /// </summary>
        /// <returns></returns>
        public string CombineAddressesTo()
        {
            if (mailAddressesTo is null) return string.Empty;

            var distinctMailAddressesTo = mailAddressesTo.Distinct();
            string mailList = string.Empty;

            foreach (var mail in distinctMailAddressesTo)
            {
                if (string.IsNullOrWhiteSpace(mail)) continue;

                //sprawdza czy mail wystepuje na pierwszej pozycji w liscie
                if (mailAddressesTo.IndexOf(mail) == 0)
                    mailList += $"{mail}";
                else
                    mailList += $"; {mail}";
            }

            return mailList;
        }
    }
}
