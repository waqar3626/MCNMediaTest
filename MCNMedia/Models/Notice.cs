using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class Notice
    {
        public int ChurchNoticeId { get; set; }
        public string NoticeTitle { get; set; }
        public string NoticeName { get; set; }
        public int ChurchId { get; set; }
        public string ChurchName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }
    }
}
