using _Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MCNMedia_Dev._Helper
{
    public class EmailHelper
    {
        public Status SendEmail(string fromEmail, string toEmail, string toName, string subject, string body)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            EmailMessage email =  ReturnAsEmailMessage(fromEmail, toEmail, toName, subject, body);
            return Send(email);
        }

        private Status Send(EmailMessage email)
        {
            Smtp smtpDb = new Smtp();
            return smtpDb.Send(email);
        }

        private EmailMessage ReturnAsEmailMessage(string fromEmail, string toEmail, string toName, string subject, string body)
        {
            EmailMessage email = new EmailMessage();

            email.From.Name = "MCN Media";
            email.From.Email = fromEmail;

            email.To.Name = toName;
            email.To.Email = toEmail;

            email.Subject = subject;
            email.Body = body;

            return email;
        }
    }
}
