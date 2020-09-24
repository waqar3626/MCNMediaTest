using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MCNMedia_Dev.Models
{
    public class Announcement
    {
        public int ChurchAnnouncementId { get; set; }
        public int ChurchId { get; set; }
        public string AnnouncementTitle { get; set; }
        
      
        public string AnnouncementText { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
