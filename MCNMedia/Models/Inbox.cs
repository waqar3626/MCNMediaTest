using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class Inbox
    {

        public int ContactId { get; set; }

        public string ContactMail { get; set; }
        public string ContactName { get; set; }

        public string ContactSubject { get; set; }
        public string Message { get; set; }

        public int Status { get; set; }

        public DateTime EmailDate { get; set; }

        public string SysTime { get; set; }


        public Inbox()
        {
            ContactId = 0;
            ContactMail = "";
            ContactName = "";
            ContactSubject = "";
            Message = "";
            Status = 0;
            SysTime = "";

        }
    }


}
