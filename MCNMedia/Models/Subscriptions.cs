using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class Subscriptions
    {
        public int PackageId { get; set; }
        public string PackageTitle { get; set; }
        public int Duration { get; set; }
        public string DurationUnit { get; set; }
        public string Currency { get; set; }
        public decimal PackageCharge { get; set; }

        public int SubscriberId { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public int CountryId { get; set; }
        public string Password { get; set; }
        public String  NewPassword { get; set; }

        public int PaymentId { get; set; }
        public string OrderId { get; set; }
        public decimal OrderAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public string ChurchName { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy} {0:hh:mm tt}")]
        public DateTime Orderdate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy} {0:hh:mm tt}")]
        public DateTime Expiredate { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ChurchId { get; set; }
        public string TokenId { get; set; }

        public decimal FullAmount { get; set; }

        public int NoOfPackages { get; set; }


    }
}
