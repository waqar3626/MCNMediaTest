using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime CreatedAt { get; set; }
        public String CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }

        public string SysTime { get; set; }
    }
}
