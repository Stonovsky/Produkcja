using System.Net;
using System.Net.Mail;

namespace GAT_Produkcja.Utilities.MailSenders.SmtpClientWrapper
{
    public interface ISmtpClientWrapper
    {
        ICredentialsByHost Credentials { get; set; }
        SmtpDeliveryMethod DeliveryMethod { get; set; }
        bool EnableSsl { get; set; }
        string Host { get; set; }
        int Port { get; set; }
        SmtpClient SmtpClient { get; set; }
        bool UseDefaultCredentials { get; set; }

        void Dispose();
        void Send(MailMessage mailMessage);
    }
}