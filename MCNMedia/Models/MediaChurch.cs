using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class MediaChurch
    {
        public int ChurchMediaId { get; set; }
        public string TabName { get; set; }
        public string MediaType { get; set; }
        public string MediaURL { get; set; }
        public string MediaName { get; set; }
        public string CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public string UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }

    }

}
