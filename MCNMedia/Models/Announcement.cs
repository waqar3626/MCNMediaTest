using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace MCNMedia_Dev.Models
{
    public class Announcement
    {
        public int ChurchAnnouncementId { get; set; }
        public int ChurchId { get; set; }
        public string AnnouncementTitle { get; set; }
        public int UpdatedBy { get; set; }

        public string AnnouncementText { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime CreatedAt { get; set; }
        public string SysTime { get; set; }
        public int CreatedBy { get; set; }
    }
}
