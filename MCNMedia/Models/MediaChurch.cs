using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class MediaChurch
    {
        public int ChurchMediaId { get; set; }
        public int ChurchId { get; set; }
        public string ChurchName { get; set; }
        public string TabName { get; set; }
        public string MediaType { get; set; }
        public string MediaURL { get; set; }
        public string MediaName { get; set; }
        public DateTime CreatedAt { get; set; }
        public String CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }

    }

}
