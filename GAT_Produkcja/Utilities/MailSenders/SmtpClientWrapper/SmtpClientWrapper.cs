using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.MailSenders.SmtpClientWrapper
{
    public class SmtpClientWrapper : ISmtpClientWrapper
    {
        #region Properties
        public int Port
        {
            get { return SmtpClient.Port; }
            set { SmtpClient.Port = value; }
        }

        public SmtpDeliveryMethod DeliveryMethod
        {
            get { return SmtpClient.DeliveryMethod; }
            set { SmtpClient.DeliveryMethod = value; }
        }

        public bool UseDefaultCredentials
        {
            get { return SmtpClient.UseDefaultCredentials; }
            set { SmtpClient.UseDefaultCredentials = value; }
        }

        public string Host
        {
            get { return SmtpClient.Host; }
            set { SmtpClient.Host = value; }
        }

        public bool EnableSsl
        {
            get { return SmtpClient.EnableSsl; }
            set { SmtpClient.EnableSsl = value; }
        }

        public ICredentialsByHost Credentials
        {
            get { return SmtpClient.Credentials; }
            set { SmtpClient.Credentials = value; }
        }

        public SmtpClient SmtpClient { get; set; }


        #endregion

        #region CTOR

        public SmtpClientWrapper(string host, int port)
        {
            SmtpClient = new SmtpClient(host, port);
        }
        public SmtpClientWrapper()
        {
            SmtpClient = new SmtpClient();
        }

        #endregion

        public void Send(MailMessage mailMessage)
        {
            SmtpClient.Send(mailMessage);
        }

        public void Dispose()
        {
            SmtpClient.Dispose();
        }

    }
}
