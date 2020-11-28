using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class StripePayment
    {
        public int PaymentLogId { get; set; }
        public int SubscriberId { get; set; }
        public string OrderId { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountInCents { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsSuccess { get; set; } 
  
    }
}
