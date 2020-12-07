using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;

namespace MCNMedia_Dev.Repository
{
    public class SubscriptionDataAccessLayer
    {
        AwesomeDal.DatabaseConnect _dc;

        public SubscriptionDataAccessLayer()
        {
            _dc = new AwesomeDal.DatabaseConnect();
        }

        public IEnumerable<Subscriptions> GetAllSubscription()
        {
            List<Subscriptions> Balobj = new List<Subscriptions>();
            DataTable dataTable = GetSubscriptionPackages(PackageId: -1);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                Subscriptions subscription = BindSubscriptionPackages(dataRow);
                Balobj.Add(subscription);
            }
            return Balobj;
        }

        public Subscriptions GetpackagesById(int PackageId)
        {
            Subscriptions subscription = new Subscriptions();
            DataTable dataTable = GetSubscriptionPackages(PackageId: PackageId);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                subscription = BindSubscriptionPackages(dataRow);
            }
            return subscription;
        }
        public DataTable GetSubscriptionPackages(int PackageId)
        {
            List<Subscriptions> Balobj = new List<Subscriptions>();
            _dc.ClearParameters();
            _dc.AddParameter("PackageId", PackageId);

            DataTable dataTable = _dc.ReturnDataTable("spPackages_Get");
            return dataTable;
               
        }

            private Subscriptions BindSubscriptionPackages(DataRow dataRow)
            {

                Subscriptions subscription = new Subscriptions();
                subscription.PackageId = Convert.ToInt32(dataRow["PackageId"]);
                subscription.Duration = Convert.ToInt32(dataRow["Duration"]);
                subscription.PackageTitle = dataRow["PackageTitle"].ToString();
                subscription.DurationUnit = dataRow["DurationUnit"].ToString();
                subscription.Currency = dataRow["Currency"].ToString();
                subscription.PackageCharge = Convert.ToDecimal(dataRow["PackageCharge"].ToString());
                return subscription;
        }
        public int AddSubscriber(Subscriptions Sub)
        {
            _dc.ClearParameters();
            _dc.AddParameter("Subname", Sub.Name);
            _dc.AddParameter("countryId", Sub.CountryId);
            _dc.AddParameter("Email", Sub.EmailAddress);
            _dc.AddParameter("Subpassword", Sub.Password);

            int SubscriberId = _dc.ReturnInt("spSubscribers_Add");

            return SubscriberId;
        }

        public Subscriptions GetSubscriberById(int subscriberId)
        {
            Subscriptions subscription = new Subscriptions();
            _dc.ClearParameters();
            _dc.AddParameter("subscribeId", subscriberId);
            DataTable dataTable = _dc.ReturnDataTable("spSubscriber_GetById");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                
                subscription.SubscriberId = Convert.ToInt32(dataRow["SubscriberId"]);
                subscription.Name = dataRow["SubscriberName"].ToString();
                subscription.EmailAddress = dataRow["EmailAddress"].ToString();
               
            }
            return subscription;
        }
        public int AddSubscriberpayment(Subscriptions Sub)
        {
            _dc.ClearParameters();
            _dc.AddParameter("PackageId", Sub.PackageId);
            _dc.AddParameter("SubscriberId", Sub.SubscriberId);
            _dc.AddParameter("OrderId", Sub.OrderId);
            _dc.AddParameter("OrderAmount", Sub.OrderAmount);
            _dc.AddParameter("PaidAmount", Sub.PaidAmount);
            _dc.AddParameter("ChurchId", Sub.ChurchId);
            _dc.AddParameter("tokenId", Sub.TokenId);


            return _dc.ReturnInt("spSubscriberPayment_Add");

         
        }

        public int  AddSubscriberpaymentLog(int PackageId,int SubscriberId,decimal OrderAmount, string OrderId,int ChurchId,string tokenId)
        {
            _dc.ClearParameters();
            _dc.AddParameter("PackageId", PackageId);
            _dc.AddParameter("SubscriberId", SubscriberId);
            _dc.AddParameter("OrderId", OrderId);
            _dc.AddParameter("OrderAmount", OrderAmount);
            _dc.AddParameter("ChurchId", ChurchId);
            _dc.AddParameter("tokenId", tokenId);

            int PaymentLogId= _dc.ReturnInt("spSubscriberPaymentLog_Add");

            return PaymentLogId;

        }
        public int UpdateSubscriberpaymentLog(int paymentLogId, bool IsSuccess, string OrderId, string tokenId)
        {
            _dc.ClearParameters();
            _dc.AddParameter("PayLogId", paymentLogId);
            _dc.AddParameter("OrderId", OrderId);
            _dc.AddParameter("IsSuccess", IsSuccess);
            _dc.AddParameter("tokenId", tokenId);


            return _dc.ReturnInt("spSubscriberPaymentLog_Update");

           



        }

        public Subscriptions SubscriberLogin(string EmailAddress, string LoginPassword)
        {
            Subscriptions user = new Subscriptions();

            _dc.ClearParameters();
            _dc.AddParameter("EmailAdd", EmailAddress);
            _dc.AddParameter("logPassword", LoginPassword);

            DataTable dataTable = _dc.ReturnDataTable("spSubscriber_Login");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                user.SubscriberId = Convert.ToInt32(dataRow["SubscriberId"]);
                user.Name = dataRow["SubscriberName"].ToString();
                user.EmailAddress = dataRow["EmailAddress"].ToString();
                user.CountryId = Convert.ToInt32(dataRow["CountryId"]);
                user.Password = dataRow["SubscriberPassword"].ToString();
               
            }
            return user;
        }
        public Subscriptions SubscriberCheck(int ChurchId , int SubscriberId)
        {
            Subscriptions user = new Subscriptions();

            _dc.ClearParameters();
            _dc.AddParameter("chrchId", ChurchId);
            _dc.AddParameter("SubId", SubscriberId);

            DataTable dataTable = _dc.ReturnDataTable("spSubscription_Check");
            if (dataTable.Rows.Count > 0) { 
            foreach (DataRow dataRow in dataTable.Rows)
            {
                user.PaymentId = Convert.ToInt32(dataRow["PaymentId"]);
                user.PackageId = Convert.ToInt32(dataRow["PackageId"]);
                user.SubscriberId = Convert.ToInt32(dataRow["SubscriberId"]);
                user.ChurchId = Convert.ToInt32(dataRow["ChurchId"]);
                user.OrderId= dataRow["OrderId"].ToString();
                user.OrderAmount = Convert.ToDecimal(dataRow["OrderAmount"].ToString());
                user.PaidAmount = Convert.ToDecimal(dataRow["PaidAmount"].ToString());
                 }
            }
            return user;
        }
    }
}
