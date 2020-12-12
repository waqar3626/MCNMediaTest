using System;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Configuration;
using System.IO;
using Amazon.S3.Model.Internal.MarshallTransformations;
using Stripe;
using Microsoft.AspNetCore.Mvc.Rendering;
using MCNMedia_Dev._Helper;
namespace MCNMedia_Dev.Controllers
{

    public class SubscriptionController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        PlaceAccessLayer _placeAccessLayer = new PlaceAccessLayer();
        ChurchDataAccessLayer _churchDataAccessLayer = new ChurchDataAccessLayer();
        WebsiteDataAccessLayer _websiteDataAccessLayer = new WebsiteDataAccessLayer();
        SubscriptionDataAccessLayer subDataAccess = new SubscriptionDataAccessLayer();
        ScheduleDataAccessLayer _scheduleDataAccessLayer = new ScheduleDataAccessLayer();


        public ViewResult Subscribe()
        {
            try
            {



                return View();
            }
            catch (Exception e)
            {
                ShowMessage("Processing Errors : " + e.Message);
                throw;
            }
        }

        [HttpPost]
        public IActionResult Processing(int PaymentLogId, string Description, string stripeToken, string stripeEmail, decimal PackageAmount)
        {
            try
            { 
            Subscriptions subscription = new Subscriptions();
            Dictionary<string, string> Metadata = new Dictionary<string, string>();
            Metadata.Add("Package Description", Description);
            StripeConfiguration.ApiKey = "sk_test_51HsMgeBcBMLkoLiDqppjmP6GfFXnDOjuwsEFPvi2SM0WJZp3yh9zlQbsVDv3lcyUv2FA8aUDuDqJP8bJ6hUwNf1L00RC9aOiHS";
            var options = new ChargeCreateOptions
            {
                Amount = (long)PackageAmount * 100,
                Currency = "GBP",
                Description = Description,
                Source = stripeToken,
                ReceiptEmail = stripeEmail,
                Metadata = Metadata,
            };
            var service = new ChargeService();
            Charge charge = service.Create(options);
            switch (charge.Status)
            {
                case "succeeded":
                    subscription.ChurchId = (int)HttpContext.Session.GetInt32("chrId");
                    subscription.SubscriberId = (int)HttpContext.Session.GetInt32("SubscriberId");
                    subscription.OrderId = charge.Id;
                    subscription.PackageId = (int)HttpContext.Session.GetInt32("packageId");
                    subscription.OrderAmount = (decimal)PackageAmount;
                    subscription.PaidAmount = (decimal)PackageAmount;
                    subscription.TokenId = stripeToken;
                    int paymentLogId = subDataAccess.UpdateSubscriberpaymentLog(PaymentLogId, true, charge.Id, stripeToken);
                    int paymentId = subDataAccess.AddSubscriberpayment(subscription);
                    TempData["paymentId"] = paymentId;
                    //EmailHelper emailHelper = new EmailHelper();
                    //emailHelper.SendEmail("mcnmedia9@gmail.com", "toEmail", "Name", "Subject:Payment", "Body");
                    return RedirectToAction("Profile", "Website");
                    break;
                case "failed":
                    int paymentLogId2 = subDataAccess.UpdateSubscriberpaymentLog(PaymentLogId, false, charge.Id, stripeToken);
                    break;
            }
            return RedirectToAction(nameof(Subscribe));
            }
            catch (Exception e)
            {
                ShowMessage("Processing Errors : " + e.Message);
                throw;
            }
        }

        public IActionResult Packages()
        {
            try
            { 
            if (HttpContext.Request.Cookies["SubscriberId"] != null)
            {
                string SubscriberId = DecodeDataFrom64(HttpContext.Request.Cookies["SubscriberId"]);
                int chrId = (int)HttpContext.Session.GetInt32("chrId");
                int subscriberId = Convert.ToInt32(SubscriberId);
                Subscriptions sub = subDataAccess.SubscriberCheck(chrId, subscriberId);
                if (sub.PaymentId > 0)
                {
                    //Subscription is active
                    TempData["paymentId"] = sub.PaymentId;
                    return RedirectToAction("Profile", "Website");
                }
                else
                {
                    CookieOptions cookieOptions = new CookieOptions();
                    cookieOptions.Expires = new DateTimeOffset(DateTime.Now.AddDays(1));
                    string encodeCookieId = EncodeDataToBase64(SubscriberId.ToString());
                    HttpContext.Response.Cookies.Append("SubscriberId", encodeCookieId, cookieOptions);
                    HttpContext.Session.SetInt32("SubscriberId", subscriberId);
                    TempData["SubscriberId"] = subscriberId;
                    return RedirectToAction(nameof(Packages2));
                }
            }
            else
                {
                ViewBag.Error = -1;
                ViewBag.Button = 1;
                ViewBag.LoginButton = 1;
                List<Subscriptions> subscription = subDataAccess.GetAllSubscription().ToList();
                ViewBag.SubscriberId = -1;
                return View(subscription);
            }
            }
            catch (Exception e)
            {
                ShowMessage("Processing Errors : " + e.Message);
                throw;
            }
        }

        public IActionResult Packages2()
        {
            try
            { 
            int SubscriberId = Convert.ToInt32(TempData["SubscriberId"]);
            ViewBag.Error = 3;
            ViewBag.LoginButton = -1;
            ViewBag.Button = 2;
            ViewBag.SubscriberId = SubscriberId;
            List<Subscriptions> subscription = subDataAccess.GetAllSubscription().ToList();
            return View("Packages", subscription);
            }
            catch (Exception e)
            {
                ShowMessage("Processing Errors : " + e.Message);
                throw;
            }
        }

        public IActionResult ListSubscription()
        {
            try
            { 
            DateTime fromDate = DateTime.Now.AddDays(-30);
            DateTime toDate = DateTime.Now;
            string emailAddress = "";

            List<Subscriptions> subs = subDataAccess.GetAllSubscribersList(fromDate, toDate, emailAddress).ToList<Subscriptions>();
            ViewBag.FromDate = fromDate.ToString("dd-MMM-yyyy");
            ViewBag.ToDate = toDate.ToString("dd-MMM-yyyy");
            return View(subs);
            }
            catch (Exception e)
            {
                ShowMessage("Processing Errors : " + e.Message);
                throw;
            }
        }

        [HttpPost]
        public IActionResult Search(string fromDate, string toDate, string EmailAddress)
        {
            try
            { 
            DateTime FromDate = Convert.ToDateTime(fromDate);
            DateTime ToDate = Convert.ToDateTime(toDate);

            List<Subscriptions> subs = subDataAccess.GetAllSubscribersList(FromDate, ToDate, EmailAddress).ToList<Subscriptions>();
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            return View("ListSubscription", subs);
            }
            catch (Exception e)
            {
                ShowMessage("Processing Errors : " + e.Message);
                throw;
            }
        }
       
        public IActionResult PackageRenewal(int PackageId, int SubscriberId)
        {
            try
            { 
            int ChurchId = (int)HttpContext.Session.GetInt32("chrId");
            if (SubscriberId > 0)
            {
                HttpContext.Session.SetInt32("packageId", PackageId);

                HttpContext.Session.SetInt32("SubscriberId", SubscriberId);
                Subscriptions subscriberinfo = subDataAccess.GetSubscriberById(SubscriberId);
                Subscriptions subscription = subDataAccess.GetpackagesById(PackageId);
                subscription.ChurchId = ChurchId;
                subscription.SubscriberId = SubscriberId;
                subscription.OrderId = "-";
                subscription.PackageId = PackageId;
                subscription.EmailAddress = subscriberinfo.EmailAddress;
                decimal PakageAmount = subscription.PackageCharge;
                int paymentLogId = subDataAccess.AddSubscriberpaymentLog(PackageId, SubscriberId, PakageAmount, "-", ChurchId, "-");
                //EmailHelper emailHelper = new EmailHelper();
                //emailHelper.SendEmail("mcnmedia9@gmail.com", subscription.EmailAddress, "", "Subject:Renewal", "you have been Renew your mcnMedia subscription");
                if (paymentLogId > 0)
                {
                    HttpContext.Session.SetInt32("paymentLogId", paymentLogId);
                    subscription.PaymentId = paymentLogId;
                    return View("Subscribe", subscription);
                }
                else
                {
                    TempData["SubscriberId"] = SubscriberId;
                    return RedirectToAction(nameof(Packages2));
                }
            }
            else
            {
                TempData["SubscriberId"] = SubscriberId;
                return RedirectToAction(nameof(Packages));
            }
            }
            catch (Exception e)
            {
                ShowMessage("Processing Errors : " + e.Message);
                throw;
            }
        }

        public IActionResult SubscriptionUserRegistration(int PackageId)
        {
            try
            { 
            LoadCountryDDL();
            ViewBag.ErrorMsg = 1;
            HttpContext.Session.SetInt32("packageId", PackageId);
            return View();
            }
            catch (Exception e)
            {
                ShowMessage("Processing Errors : " + e.Message);
                throw;
            }
        }

        [HttpPost]
        public IActionResult UserRegistration(Subscriptions subscriptions)
        {
            try 
            { 
            int SubscriberId = subDataAccess.AddSubscriber(subscriptions);
                int ChurchId = 0;
                if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("chrId").ToString()))
                {
                    ChurchId = (int)HttpContext.Session.GetInt32("chrId");
                }
            
            if (SubscriberId > 0)
            {
                    Subscriptions subscriberName = subDataAccess.GetSubscriberById(SubscriberId);
                string subscriberId = EncodeDataToBase64(SubscriberId.ToString());
                CookieOptions cookieOptions = new CookieOptions();
                cookieOptions.Expires = new DateTimeOffset(DateTime.Now.AddDays(1));
                HttpContext.Response.Cookies.Append("SubscriberId", subscriberId, cookieOptions);
                HttpContext.Session.SetInt32("SubscriberId", SubscriberId);
                     HttpContext.Response.Cookies.Append("SubscriberName", subscriberName.Name, cookieOptions);
                int PackageId = (int)HttpContext.Session.GetInt32("packageId");
                Subscriptions subscriberinfo = subDataAccess.GetSubscriberById(SubscriberId);
                Subscriptions subscription = subDataAccess.GetpackagesById(PackageId);
                subscription.ChurchId = ChurchId;
                subscription.SubscriberId = SubscriberId;
                subscription.OrderId = "-";
                subscription.PackageId = PackageId;
                subscription.EmailAddress = subscriberinfo.EmailAddress;
                decimal PakageAmount = subscription.PackageCharge;
                int paymentLogId = subDataAccess.AddSubscriberpaymentLog(PackageId, SubscriberId, PakageAmount, "-", ChurchId, "-");
                //EmailHelper emailHelper = new EmailHelper();
                //emailHelper.SendEmail("mcnmedia9@gmail.com", subscription.EmailAddress, subscription.Name, "Subject: sign Up", "Body");
                if (paymentLogId > 0)
                {
                    HttpContext.Session.SetInt32("paymentLogId", paymentLogId);
                    subscription.PaymentId = paymentLogId;
                    return View("Subscribe", subscription);
                }
                else
                {
                    LoadCountryDDL();
                    return View("SubscriptionUserRegistration", subscriptions);
                }
            }
            else
            {
                LoadCountryDDL();
                ViewBag.ErrorMsg = 2;
                return View("SubscriptionUserRegistration", subscriptions);
            }
            }
            catch (Exception e)
            {
                ShowMessage("Processing Errors : " + e.Message);
                throw;
            }
        }

        [Route("/Subscription/SubscriptionUserLogin/{Id?}")]
        public IActionResult SubscriptionUserLogin(string? Id)
        {
            try 
            {
                ViewBag.LoginLocation = Id;
            return View();
            }
            catch (Exception e)
            {
                ShowMessage("Processing Errors : " + e.Message);
                throw;
            }
        }

        [HttpPost]
        public IActionResult UserLogin(string Email, string Password)
        {
            try
            {
                int chrId = 0;
            Subscriptions subscription = subDataAccess.SubscriberLogin(Email, Password);
            if (subscription.SubscriberId > 0)
            {   
                string SubId = EncodeDataToBase64(subscription.SubscriberId.ToString());
                CookieOptions cookieOptions = new CookieOptions();
                cookieOptions.Expires = new DateTimeOffset(DateTime.Now.AddDays(1));
                HttpContext.Response.Cookies.Append("SubscriberId", SubId, cookieOptions);
                HttpContext.Response.Cookies.Append("SubscriberName", subscription.Name, cookieOptions);
                    if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("chrId").ToString()))
                    {
                        chrId = (int)HttpContext.Session.GetInt32("chrId");
                    }
                int subscriberId = (int)subscription.SubscriberId;
                Subscriptions sub = subDataAccess.SubscriberCheck(chrId, subscriberId);
                if (sub.PaymentId > 0)
                {
                    //Subscription is active
                    TempData["paymentId"] = sub.PaymentId;
                    //EmailHelper emailHelper = new EmailHelper();
                    //emailHelper.SendEmail("mcnmedia9@gmail.com", Email, "", "Subject:Login", "you have been login to mcnMedia");
                    return RedirectToAction("Profile", "Website");
                }
                else
                {      
                    HttpContext.Response.Cookies.Append("SubscriberId", SubId, cookieOptions);
                    HttpContext.Session.SetInt32("SubscriberId", subscription.SubscriberId);
                    TempData["SubscriberId"] = subscription.SubscriberId;
                    return RedirectToAction(nameof(Packages2));
                }
            }
            else
            {
                ViewBag.Error = 2;
                    ViewBag.LoginLocation = "Packages";
                return View("SubscriptionUserLogin");
            }
            }
            catch (Exception e)
            {
                ShowMessage("Processing Errors : " + e.Message);
                throw;
            }
        }

        [HttpPost]
        public IActionResult HomeSubscriberLogin(string Email, string Password)
        {
            try
            {
                
                Subscriptions subscription = subDataAccess.SubscriberLogin(Email, Password);
                if (subscription.SubscriberId > 0)
                {
                    string SubId = EncodeDataToBase64(subscription.SubscriberId.ToString());
                    CookieOptions cookieOptions = new CookieOptions();
                    cookieOptions.Expires = new DateTimeOffset(DateTime.Now.AddDays(1));
                    HttpContext.Response.Cookies.Append("SubscriberId", SubId, cookieOptions);
                    HttpContext.Response.Cookies.Append("SubscriberName", subscription.Name, cookieOptions);
                    int subscriberId = (int)subscription.SubscriberId;
                    
                        //Subscription is active
                       
                        //EmailHelper emailHelper = new EmailHelper();
                        //emailHelper.SendEmail("mcnmedia9@gmail.com", Email, "", "Subject:Login", "you have been login to mcnMedia");
                        return RedirectToAction("Home", "Website");
                    
                    
                }
                else
                {
                    ViewBag.Error = 2;
                    ViewBag.LoginLocation = "Home";
                    return View("SubscriptionUserLogin");
                }
            }
            catch (Exception e)
            {
                ShowMessage("Processing Errors : " + e.Message);
                throw;
            }

        }
        public void LoadCountryDDL()
        {
            try
            {
                IEnumerable<Place> countryList = _placeAccessLayer.GetISOCountries();
                List<SelectListItem> selectListItems = new List<SelectListItem>();
                foreach (var item in countryList)
                {
                    selectListItems.Add(new SelectListItem { Text = item.PlaceName.ToString(), Value = item.PlaceId.ToString() });
                }
                ViewBag.Countries = selectListItems;
            }
            catch (Exception e)
            {
                ShowMessage("Processing Errors : " + e.Message);
                throw;
            }
        }

        public string EncodeDataToBase64(string Data)
        {
            try
            {
                byte[] encData_byte = new byte[Data.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(Data);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception e)
            {
                ShowMessage("Processing Errors : " + e.Message);
                throw;
            }
        } //this function Convert to Decord your Password
        public string DecodeDataFrom64(string encodedData)
        {
            try
            {
                if (encodedData.Contains("=")) {
                    System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
                }
                else
                {
                    return encodedData;
                }
            }
            catch (Exception e)
            {
                ShowMessage("Processing Errors : " + e.Message);
                throw;
            }
        }

        private void ShowMessage(String exceptionMessage)
        {
            log.Error("Exception : " + exceptionMessage);
        }

        public IActionResult PasswordChange()
        {
            return View();
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        public IActionResult SubscriperProfile()
        {
            try
            {
                string SubscriberId = DecodeDataFrom64(HttpContext.Request.Cookies["SubscriberId"]);
                int SubId = Convert.ToInt32(SubscriberId);

                List < Subscriptions > subs = subDataAccess.GetSingleSubscribersList(SubId).ToList<Subscriptions>();

                return View(subs);
            }
            catch (Exception e)
            {
                ShowMessage("Processing Errors : " + e.Message);
                throw;
            }
        }
    }
}