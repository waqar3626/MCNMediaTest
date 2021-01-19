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

        public void UpdateSubscriptionByAdmin(Subscriptions sub)
        {
            _dc.ClearParameters();
            _dc.AddParameter("Subscriber_Id", sub.SubscriberId);
            _dc.AddParameter("Subscriber_Name", sub.Name);
            _dc.AddParameter("Email_Address", sub.EmailAddress);
            _dc.AddParameter("Country_Id", sub.CountryId);
            _dc.AddParameter("Subscriber_Password", sub.Password);
            _dc.AddParameter("Updaed_By", sub.UpdatedBy);
            _dc.Execute("spSubscriber_Update");
        }
        public IEnumerable<Subscriptions> GetAllSubscribersList(DateTime FromDate, DateTime ToDate, string EmailAddress)
        {
            List<Subscriptions> Balobj = new List<Subscriptions>();
            _dc.ClearParameters();
            _dc.AddParameter("EmailAdd", EmailAddress);
            _dc.AddParameter("fromdate", FromDate);
            _dc.AddParameter("Todate", ToDate);

            DataTable dataTable = _dc.ReturnDataTable("spSubscriber_GetAllSubscriberListK");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                Subscriptions subscription = new Subscriptions();
                subscription.SubscriberId = Convert.ToInt32(dataRow["SubscriberId"].ToString());
                subscription.Name = dataRow["SubscriberName"].ToString();
                subscription.EmailAddress = dataRow["EmailAddress"].ToString();
                subscription.ChurchName = dataRow["ChurchName"].ToString();
                subscription.PackageTitle = dataRow["PackageTitle"].ToString();
                subscription.DurationUnit =Convert.ToInt32(dataRow["Duration"].ToString())+ dataRow["DurationUnit"].ToString();
                subscription.PackageCharge = Convert.ToDecimal(dataRow["PackageCharge"].ToString());
                subscription.PaidAmount = Convert.ToDecimal(dataRow["PaidAmount"].ToString());
                subscription.Orderdate = Convert.ToDateTime(dataRow["OrderDate"].ToString());
                subscription.Expiredate = Convert.ToDateTime(dataRow["ExpiryDate"].ToString());
                subscription.CreatedAt = Convert.ToDateTime(dataRow["CreatedAt"].ToString());
                Balobj.Add(subscription);
            }
            return Balobj;
        }
        public IEnumerable<Subscriptions> GetSingleSubscribersList(int subscriberId)
         {
            List<Subscriptions> Balobj = new List<Subscriptions>();
            _dc.ClearParameters();
            _dc.AddParameter("subscriberId", subscriberId);

            DataTable dataTable = _dc.ReturnDataTable("spSubscriberOrder_Get");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                Subscriptions subscription = new Subscriptions();
                subscription.SubscriberId = Convert.ToInt32(dataRow["SubscriberId"]);
                subscription.Name = dataRow["SubscriberName"].ToString();
                subscription.EmailAddress = dataRow["EmailAddress"].ToString();
                subscription.PackageTitle = dataRow["PackageTitle"].ToString();
                subscription.DurationUnit = Convert.ToInt32(dataRow["Duration"].ToString()) + dataRow["DurationUnit"].ToString();
                subscription.PackageCharge = Convert.ToDecimal(dataRow["PackageCharge"].ToString());
                subscription.PaidAmount = Convert.ToDecimal(dataRow["PaidAmount"].ToString());
                subscription.Orderdate = Convert.ToDateTime(dataRow["OrderDate"].ToString());
                subscription.Expiredate = Convert.ToDateTime(dataRow["ExpiryDate"].ToString());
                subscription.CreatedAt = Convert.ToDateTime(dataRow["CreatedAt"].ToString());
                Balobj.Add(subscription);
            }
            return Balobj;
        }
        public Subscriptions GetSubscriberById(int subscriberId)
        {
            Subscriptions subscription = new Subscriptions();
            _dc.ClearParameters();
            _dc.AddParameter("subscribe_Id", subscriberId);
            DataTable dataTable = _dc.ReturnDataTable("spSubscriber_GetById");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                
                subscription.SubscriberId = Convert.ToInt32(dataRow["SubscriberId"]);
                subscription.Name = dataRow["SubscriberName"].ToString();
                subscription.EmailAddress = dataRow["EmailAddress"].ToString();
                subscription.CreatedAt = Convert.ToDateTime(dataRow["CreatedAt"].ToString());
                subscription.Expiredate = Convert.ToDateTime(dataRow["ExpiryDate"].ToString());
                subscription.CountryId = Convert.ToInt32(dataRow["CountryId"].ToString());
                subscription.ChurchId = Convert.ToInt32(dataRow["ChurchId"].ToString());
               
            }
            return subscription;
        }
        public void UpdateSubscriberPassword(int Subscriber_Id,string newPassword)
        {
            _dc.ClearParameters();
            _dc.AddParameter("Subscriber_Id", Subscriber_Id);
            _dc.AddParameter("Subscriber_Password", newPassword);
            _dc.Execute("spSubscriber_PassChange");
           
        }
        public void ResetSubscriberPassword(string SubscriberEmail, string newPassword)
        {
            _dc.ClearParameters();
            _dc.AddParameter("Subscriber_EmailAdd", SubscriberEmail);
            _dc.AddParameter("Subscriber_Password", newPassword);
            _dc.Execute("spSubscriber_ResetPassword");

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
            _dc.AddParameter("IsAddedByAdmin", Sub.IsAddedByAdmin);


            return _dc.ReturnInt("spSubscriberPayment_AddKk");

         
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

        public Subscriptions SubscriberEmailCheck(string EmailAddress)
        {
            Subscriptions user = new Subscriptions();

            _dc.ClearParameters();
            _dc.AddParameter("EmailAdd", EmailAddress);
           

            DataTable dataTable = _dc.ReturnDataTable("spSubscriber_EmailCheck");
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

        public int ChurchRegionCheck(int ChurchId, string RegionName)
        {
            
           // List<Church> Balobj = new List<Church>();
            _dc.ClearParameters();
            _dc.AddParameter("ChId", ChurchId);
            _dc.AddParameter("RegName", RegionName);

            int result = 0;
            DataTable dataTable = _dc.ReturnDataTable("spChurch_ForRegionCheck");
            if (dataTable.Rows.Count > 0)
            {
                result = Convert.ToInt32(dataTable.Rows[0][0].ToString());
            }
            return result;
        }

        public IEnumerable<Subscriptions> GetRevenueInfo(DateTime FromDate, DateTime ToDate)
        {
            List<Subscriptions> Balobj = new List<Subscriptions>();
            _dc.ClearParameters();
            _dc.AddParameter("fromdate", FromDate);
            _dc.AddParameter("Todate", ToDate);

            DataTable dataTable = _dc.ReturnDataTable("spSubscriber_GetRevenueInfo");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                Subscriptions subscription = new Subscriptions();

                subscription.PackageTitle = dataRow["PackageTitle"].ToString();
                subscription.PaidAmount = Convert.ToDecimal(dataRow["PaidAmount"].ToString());

                subscription.NoOfPackages = Convert.ToInt32(dataRow["NoOfPackages"].ToString());

                Balobj.Add(subscription);
            }
            return Balobj;
        }
    }
}
