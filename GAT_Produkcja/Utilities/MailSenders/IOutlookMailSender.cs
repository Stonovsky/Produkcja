using System.Collections.Generic;
using System.Threading.Tasks;
using GAT_Produkcja.db;
using GAT_Produkcja.Utilities.MailSenders.MailMessages;
using Microsoft.Office.Interop.Outlook;

namespace GAT_Produkcja.Utilities.MailSenders
{
    public interface IOutlookMailSender
    {
        Task SendAsync(MailItem mailItem);
        ZapotrzebowanieMailItem StworzZapotrzebowanieMailItem(tblZapotrzebowanie zapotrzebowanie, List<string> adresyMailowe);
        Task WyslijMailZZapotrzebowaniemAsync(tblZapotrzebowanie zapotrzebowanie, List<string> adresyMailowe);

    }
}