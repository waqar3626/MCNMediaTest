using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class ChurchDonation
    {
        public int DoonationId { get; set; }
        public int ChurchId { get; set; }
        public String ImageUrl { get; set; }
        public String WebSiteUrl { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public Boolean mDelete { get; set; }
    }
}
