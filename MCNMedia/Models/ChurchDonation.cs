using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class ChurchDonation
    {
        public int DonationId { get; set; }
        public int ChurchId { get; set; }
        public string ImageUrl { get; set; }
        public string WebSiteUrl { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool ShowOnWebsite { get; set; }
    }
}
