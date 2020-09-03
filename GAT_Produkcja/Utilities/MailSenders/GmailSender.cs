using GAT_Produkcja.Utilities.MailSenders.SmtpClientWrapper;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.MailSenders
{
    public class GmailSender : IGmailSender
    {
        private const string gmailAddressFrom = "produkcja.gat@gmail.com";
        private const string gmailPasswordFrom = "Wyzwolenia367A";
        private const string SMTP_GMAIL = "smtp.gmail.com";
        private const int SMTP_PORT = 587;
        private readonly ISmtpClientWrapper smtpClientWrapper;
        private MailMessage message;

        public GmailSender(ISmtpClientWrapper smtpClientWrapper)
        {
            this.smtpClientWrapper = smtpClientWrapper;
        }
        public async Task SendMessageAsync(MailMessage message)
        {
            await Task.Run(() => SendMessage(message));
        }

        public void SendMessage(MailMessage message)
        {
            try
            {
                smtpClientWrapper.Port = SMTP_PORT;
                smtpClientWrapper.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClientWrapper.UseDefaultCredentials = false;
                smtpClientWrapper.Host = SMTP_GMAIL;
                smtpClientWrapper.EnableSsl = true;
                smtpClientWrapper.Credentials = new NetworkCredential(gmailAddressFrom, gmailPasswordFrom);

                smtpClientWrapper.Send(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                smtpClientWrapper.Dispose();
            }
        }
    }
}
