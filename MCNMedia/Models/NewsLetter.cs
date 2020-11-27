using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class NewsLetter
    {
        public int ChurchNewsLetterId { get; set; }
        public string NewsLetterTitle { get; set; }
        public string NewsLetterName { get; set; }
        public string NewsLetterUrl { get; set; }
        public int ChurchId { get; set; }
        public bool ShowOnWebsite { get; set; }
        public string ChurchName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }
    }
}
