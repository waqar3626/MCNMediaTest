using MCNMedia_Dev.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Repository
{
   
    public class InboxDataAccessLayes
    {

        AwesomeDal.DatabaseConnect _dc;

        public InboxDataAccessLayes()
        {
            _dc = new AwesomeDal.DatabaseConnect();
        }
        public int GetUnreadEmail()
        {
            _dc.ClearParameters();
            int Count = _dc.ReturnInt("spMailUnreadGet");
            return Count;
        }
        public IEnumerable<Inbox> GetAllEmails()
        {
            List<Inbox> Balobj = new List<Inbox>();
                        _dc.ClearParameters();
            
            DataTable dataTable = _dc.ReturnDataTable("spEmails_GetAll");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                Inbox inbox = new Inbox();
                inbox.ContactId = Convert.ToInt32(dataRow["ContactId"].ToString());
                inbox.ContactMail = dataRow["ContactMail"].ToString();
                inbox.ContactName = dataRow["ContactName"].ToString();
                inbox.ContactSubject = dataRow["ContactSubject"].ToString();
                inbox.Message = dataRow["Message"].ToString();
                inbox.Status = Convert.ToInt32(dataRow["Status"].ToString());
                inbox.EmailDate = Convert.ToDateTime(dataRow["EmailDate"].ToString());
                inbox.SysTime = Convert.ToDateTime(dataRow["EmailDate"]).ToString("dd-MMM-yyyy");
                Balobj.Add(inbox);
            }
            return Balobj;
        }
        public bool UpdateUnreadMailStatus()
        {
            _dc.ClearParameters();
          return _dc.ReturnBool("spUnreadMailStatus_Update");
        }
        public bool ChangeMailStatus(int MessageId,int Status)
        {
            _dc.ClearParameters();
            _dc.AddParameter("messageId", MessageId);
            _dc.AddParameter("status", Status);
            return _dc.ReturnBool("spMailStatus_Update");
        }

        public Inbox GetMailDataById(int id)
        {
            Inbox inbox = new Inbox();

            _dc.ClearParameters();
            _dc.AddParameter("messageId", id);
            DataTable dataTable = _dc.ReturnDataTable("spEmailGetById");
            foreach (DataRow dataRow in dataTable.Rows)
            {
               
                inbox.ContactId = Convert.ToInt32(dataRow["ContactId"].ToString());
                inbox.ContactMail = dataRow["ContactMail"].ToString();
                inbox.ContactName = dataRow["ContactName"].ToString();
                inbox.ContactSubject = dataRow["ContactSubject"].ToString();
                inbox.Message = dataRow["Message"].ToString();
                inbox.Status = Convert.ToInt32(dataRow["Status"].ToString());
                inbox.EmailDate = Convert.ToDateTime(dataRow["EmailDate"].ToString());
                inbox.SysTime = Convert.ToDateTime(dataRow["EmailDate"]).ToString("dd-MMM-yyyy");
            }
            return inbox;
        }
        public bool DeleteMail(int id)
        {
            _dc.ClearParameters();
            _dc.AddParameter("messageId", id);
            
         return   _dc.ReturnBool("spMail_Delete");
        }
    }
}
