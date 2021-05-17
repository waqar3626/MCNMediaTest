using MCNMedia_Dev.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace MCNMedia_Dev.Repository
{
    public class EmailTemplateDataAccessLayer
    {
        AwesomeDal.DatabaseConnect _dc;
        public EmailTemplateDataAccessLayer()
        {
            _dc = new AwesomeDal.DatabaseConnect();
        }

        public EmailTemplate GetEmailTemplateId(int MessageId)
        {
            EmailTemplate emailTemplate = new EmailTemplate();
            _dc.CloseAndDispose();
            _dc.ClearParameters();
            _dc.AddParameter("MsgId", MessageId);
            DataTable dataTable = _dc.ReturnDataTable("spEmailMessage_Get");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                emailTemplate.MessageId = Convert.ToInt32(dataRow["MessageId"]);
                emailTemplate.MessageFor = dataRow["MessageFor"].ToString();
                emailTemplate.Subject = dataRow["Subject"].ToString();
                emailTemplate.FromEmail = dataRow["From_Email"].ToString();
                emailTemplate.FromName = dataRow["From_Name"].ToString();

            }
            return emailTemplate;
        }
    }
}
