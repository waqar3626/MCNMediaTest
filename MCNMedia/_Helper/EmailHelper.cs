using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MCNMedia_Dev._Helper
{
    public class EmailHelper
    {
        public bool SendEmail(string fromEmail, string toEmail, string toName, string subject, string body)
        {
            _Helper.Common.SaveToXXX("EmailHelper - SendEmail(first) - Begin");

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            EmailMessage email = new EmailMessage();
            email = ReturnAsEmailMessage(fromEmail, toEmail, toName, subject, body);
            SendEmail(email);
            _Helper.Common.SaveToXXX("EmailHelper - SendEmail(first) - End");
            return true;
            
        }

        private void SendEmail(EmailMessage email)
        {
            _Helper.Common.SaveToXXX("EmailHelper - SendEmail(second) - Begin");
            Smtp smtpDb = new Smtp();
            smtpDb.Send(email);
            _Helper.Common.SaveToXXX("EmailHelper - SendEmail(second) - Begin");
        }

        private EmailMessage ReturnAsEmailMessage(string fromEmail, string toEmail, string toName, string subject, string body)
        {
            EmailMessage email = new EmailMessage();

            email.From.Name = null;
            email.From.Email = fromEmail;

            email.To.Name = toName;
            email.To.Email = toEmail;

            email.Subject = subject;
            email.Body = body;

            return email;
        }
    }
}
