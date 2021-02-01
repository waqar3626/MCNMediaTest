using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

using System.Collections;
using System.Data;

namespace MCNMedia_Dev._Helper
{
    public class Smtp
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Smtp"/> class.
        /// </summary>
        public Smtp()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            _smtpClient = Load();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Smtp"/> class.
        /// </summary>
        /// <param name="userName">
        /// Name of the user.</param>
        /// <param name="password">
        /// The password.</param>
        /// <param name="smtpServer">
        /// The SMTP server.</param>
        /// <param name="port">
        /// The port. Is defaulted to 587 If not supplied.</param>
        public Smtp(string userName ,string password,string smtpServer , int port  = 587)
        {
            _smtpClient = new SmtpClient();
            {
                _Helper.Common.SaveToXXX("Email - Smtp - Begin1");
                var withBlock = _smtpClient;
                withBlock.Credentials = new NetworkCredential(userName, password);
                _Helper.Common.SaveToXXX("Email - Smtp - Begin2");
                withBlock.DeliveryMethod = SmtpDeliveryMethod.Network;
                withBlock.EnableSsl = true;
                withBlock.Host = smtpServer;
                withBlock.Port = port;
                _Helper.Common.SaveToXXX("Email - Smtp - End");
            }
        }

        /// <summary>
        /// The SMTP Client
        /// </summary>
        private readonly SmtpClient _smtpClient;

        /// <summary>
        /// Sends the specified mail messages.
        /// All <see cref="MailMessage" />s are set to <c>.IsBodyHtml = True</c>.
        /// </summary>
        /// <param name="mailMessages">
        /// The mail messages (see <see Cref="MailMessage" />.
        /// </param>
        public void Send(params MailMessage[] mailMessages)
        {
            foreach (MailMessage msg in mailMessages)
            {
                _Helper.Common.SaveToXXX("Email - Smtp-Send - Begin1");
                msg.IsBodyHtml = true;
                _smtpClient.Send(msg);
                _Helper.Common.SaveToXXX("Email - Smtp-Send - End");

            }
        }

        /// <summary>
        /// Sends the specified email messages.
        /// <paramref name="emailMessages" /> are converted to <see cref="MailMessage" />
        /// before sending and are set to <c>.IsBodyHtml = True</c>.
        /// </summary>
        /// <param name="emailMessages">
        /// The email messages (see <see cref="EmailMessage" />).
        /// </param>
        public void Send(params EmailMessage[] emailMessages)
        {
            List<MailMessage> messages = new List<MailMessage>();

            foreach (EmailMessage msg in emailMessages)
            {
                _Helper.Common.SaveToXXX("Email - Smtp-Send-WithBody - Begin1");
                MailMessage message = new MailMessage();
                {
                    var withBlock = message;
                    _Helper.Common.SaveToXXX("Email - Smtp-Send-WithBody - Begin2");
                    withBlock.To.Add(new MailAddress(msg.To.Email, msg.To.Name));
                    withBlock.Subject = msg.Subject;
                    withBlock.Body = msg.Body;
                    withBlock.From = new MailAddress(msg.From.Email, msg.From.Name);
                    _Helper.Common.SaveToXXX("Email - Smtp-Send-WithBody - Begin2");
                }
                _Helper.Common.SaveToXXX("Email - Smtp-Send-WithBody - Begin3");
                messages.Add(message);
                _Helper.Common.SaveToXXX("Email - Smtp-Send-WithBody - Begin4");

            }
            _Helper.Common.SaveToXXX("Email - Smtp-Send-WithBody - Begin5");

            Send(messages.ToArray());
            _Helper.Common.SaveToXXX("Email - Smtp-Send-WithBody - Begin5");

        }

        /// <summary>
        /// Loads an instance of <see cref="Smtp" /> from the database.
        /// The values are retrieved from the table 'SMTP'.
        /// </summary>
        /// <returns>
        /// A populated instance of <see cref="Smtp" />.
        /// </returns>
        /// <exception cref="Exception">
        /// An exception is thrown if there is no data-row or empty values in the table 'SMTP'.
        /// </exception>
        private SmtpClient Load()
        {
              AwesomeDal.DatabaseConnect _dc;
            DataTable dt = new DataTable();

            // Gets values from the database
            _dc = new AwesomeDal.DatabaseConnect();
            _dc.ClearParameters();
            dt = _dc.ReturnDataTable("spSmtp_Get");
            
            // Instantiates variables
            DataRow dr = dt.Rows[0];
            string userName = dr["UserName"].ToString();
            string password = dr["password"].ToString();
            string smtpServer = dr["SmtpServer"].ToString();
            int port = System.Convert.ToInt32(dr["Port"]);

            // Checks values exist
            if (userName.Trim().Length == 0 || password.Trim().Length == 0 || smtpServer.Trim().Length == 0 || port <= 0)
                throw new Exception("SMTP has not been fully configured in this system");

            // Creates SMTP
            SmtpClient remoteSmtp = new SmtpClient();
            {
                var withBlock = remoteSmtp;
                withBlock.Credentials = new NetworkCredential(userName, password);
                withBlock.DeliveryMethod = SmtpDeliveryMethod.Network;
                withBlock.EnableSsl = true;
                withBlock.Host = smtpServer;
                withBlock.Port = port;
            }

            return remoteSmtp;
        }

    }

    public class EmailMessage
    {
        public EmailMessage() 
        {
                this.To = new EmailAddress();
                this.From = new EmailAddress();
        }

        private EmailMessage(EmailAddress _to, EmailAddress _from)
        {
            if (_to == null)
                this.To = new EmailAddress();
            else
                this.To = _to;
            if (_from == null)
                this.From = new EmailAddress();
            else
                this.From = _from;
        }

        public EmailAddress To { get; set; }
        public EmailAddress From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }

    public class EmailAddress
    {
        public EmailAddress()
        {
            Email = string.Empty;
            Name = string.Empty;
        }

        public EmailAddress(string email, string name)
        {
            this.Email = email;
            this.Name = name;
        }

        public string Email { get; set; }

        public string Name { get; set; }
    }
}