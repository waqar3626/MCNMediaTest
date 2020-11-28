using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;

namespace MCNMedia_Dev.Repository
{
    public class PaymentDataAccessLayer
    {
        AwesomeDal.DatabaseConnect _dc;

        public PaymentDataAccessLayer()
        {
            _dc = new AwesomeDal.DatabaseConnect();
        }
        public int AddOnlinePaymentLog(StripePayment spayment)
        {
            _dc.ClearParameters();
            _dc.AddParameter("SubscriberId", spayment.SubscriberId);
            _dc.AddParameter("OrderId", spayment.OrderId);
            _dc.AddParameter("Amount", spayment.Amount);
            _dc.AddParameter("AmountInCents", spayment.AmountInCents);
            _dc.AddParameter("OrderDate", spayment.OrderDate);
            _dc.AddParameter("IsSuccess", spayment.IsSuccess);
            return _dc.ReturnInt("spLogOnline_Add");
        }

        public int UpdateOnlinePaymentLog(StripePayment spayment)
        {
            _dc.ClearParameters();
            _dc.AddParameter("SubscriberId", spayment.SubscriberId);
            _dc.AddParameter("OrderId", spayment.OrderId);
            _dc.AddParameter("IsSuccess", spayment.IsSuccess);
            return _dc.ReturnInt("spLogOnline_Update");
        }
    }
}
