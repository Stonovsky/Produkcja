using System.Net.Mail;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.MailSenders
{
    public interface IGmailSender
    {
        void SendMessage(MailMessage message);
        Task SendMessageAsync(MailMessage message);
    }
}