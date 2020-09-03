using System.Threading.Tasks;
using Outlook = Microsoft.Office.Interop.Outlook;
using GAT_Produkcja.db;
using GAT_Produkcja.Utilities.MailSenders.MailMessages;
using System.Collections.Generic;

namespace GAT_Produkcja.Utilities.MailSenders
{
    public class OutlookMailSender : IOutlookMailSender
    {
        private tblZapotrzebowanie zapotrzebowanie;
        private Outlook.Application app;
        private dynamic mailItem;
        private ZapotrzebowanieMailItem zapotrzebowanieMailItem;

        public OutlookMailSender()
        {
        }

        public async Task SendAsync(Outlook.MailItem mailItem)
        {
            Outlook.MailItem mail = mailItem;

            await Task.Run(() => mail.Send());

        }

        public ZapotrzebowanieMailItem StworzZapotrzebowanieMailItem(tblZapotrzebowanie zapotrzebowanie, List<string> adresyMailowe)
        {
            return new ZapotrzebowanieMailItem(zapotrzebowanie, adresyMailowe);
        }

        public async Task WyslijMailZZapotrzebowaniemAsync(tblZapotrzebowanie zapotrzebowanie, List<string> adresyMailowe)
        {
            zapotrzebowanieMailItem = new ZapotrzebowanieMailItem(zapotrzebowanie, adresyMailowe);
            
            Outlook.MailItem mail = zapotrzebowanieMailItem.Create();

            await Task.Run(() => mail.Send());
        }
    }
}