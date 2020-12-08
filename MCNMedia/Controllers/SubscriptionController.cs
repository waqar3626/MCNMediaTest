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


        public ViewResult Subscribe() {

           
            return View();
        }

        [HttpPost]
        public IActionResult Processing(int PaymentLogId, string Description,string stripeToken, string stripeEmail,decimal PackageAmount)
        {
            Subscriptions subscription = new Subscriptions();
            Dictionary<string, string> Metadata = new Dictionary<string, string>();
            Metadata.Add("Product", "RubberDuck");
            Metadata.Add("Quantity", "10");
            StripeConfiguration.ApiKey = "sk_test_51HsMgeBcBMLkoLiDqppjmP6GfFXnDOjuwsEFPvi2SM0WJZp3yh9zlQbsVDv3lcyUv2FA8aUDuDqJP8bJ6hUwNf1L00RC9aOiHS";
            var options = new ChargeCreateOptions
            {
                Amount = (long)PackageAmount*100,
                Currency = "EUR",
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
                    subscription.SubscriberId=(int)HttpContext.Session.GetInt32("SubscriberId");
                    subscription.OrderId = charge.Id;
                    subscription.PackageId= (int)HttpContext.Session.GetInt32("packageId");
                    subscription.OrderAmount = (decimal)PackageAmount;
                    subscription.PaidAmount = (decimal)PackageAmount;
                    subscription.TokenId = stripeToken;
                    int paymentLogId = subDataAccess.UpdateSubscriberpaymentLog(PaymentLogId,true, charge.Id, stripeToken);
                    int paymentId = subDataAccess.AddSubscriberpayment(subscription);
                    return RedirectToAction(nameof(Profile));
                    break;
                case "failed":

                    int paymentLogId2 = subDataAccess.UpdateSubscriberpaymentLog(PaymentLogId, false, charge.Id, stripeToken);
                    break;
            }
            return RedirectToAction(nameof(Subscribe));
        }

        public IActionResult Packages() {
            HttpContext.Session.SetInt32("chrId", 44);
            ViewBag.Error = -1;
            ViewBag.Button = 1;
            ViewBag.LoginButton = 1;
            List<Subscriptions> subscription = subDataAccess.GetAllSubscription().ToList();
            ViewBag.SubscriberId = -1;
            return View(subscription);
        }

        public IActionResult Packages2()
        {
            HttpContext.Session.SetInt32("chrId", 44);
            int SubscriberId = Convert.ToInt32(TempData["SubscriberId"]);
            ViewBag.Error = 3;
            ViewBag.LoginButton = -1;
            ViewBag.Button = 2;
            ViewBag.SubscriberId = SubscriberId;
            List<Subscriptions> subscription = subDataAccess.GetAllSubscription().ToList();
            return View("Packages",subscription);
        }

        public IActionResult ListSubscription()
        {
            Subscriptions subscription = new Subscriptions();
            List<Subscriptions> subs = subDataAccess.GetAllSubscribersList().ToList<Subscriptions>();
            return View(subs);

        }

        public IActionResult PackageRenewal(int PackageId,int SubscriberId)
        {
            
            int ChurchId = (int)HttpContext.Session.GetInt32("chrId");
            if (SubscriberId > 0)
            {
                HttpContext.Session.SetInt32("SubscriberId", SubscriberId);
                HttpContext.Session.SetInt32("packageId", PackageId);
                Subscriptions subscriberinfo = subDataAccess.GetSubscriberById(SubscriberId);
                Subscriptions subscription = subDataAccess.GetpackagesById(PackageId);
                subscription.ChurchId = ChurchId;
                subscription.SubscriberId = SubscriberId;
                subscription.OrderId = "-";
                subscription.PackageId = PackageId;
                subscription.EmailAddress = subscriberinfo.EmailAddress;
                decimal PakageAmount = subscription.PackageCharge;
                int paymentLogId = subDataAccess.AddSubscriberpaymentLog(PackageId, SubscriberId, PakageAmount, "-", ChurchId,"-");
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
        public IActionResult SubscriptionUserRegistration(int PackageId) 
        {
            LoadCountryDDL();
            ViewBag.ErrorMsg = 1;
            HttpContext.Session.SetInt32("packageId", PackageId);
            
            return View();
        }

     [HttpPost]
        public IActionResult UserRegistration(Subscriptions subscriptions)
        {
                int SubscriberId = subDataAccess.AddSubscriber(subscriptions);
                int ChurchId= (int)HttpContext.Session.GetInt32("chrId");
                if (SubscriberId > 0) {
                HttpContext.Session.SetInt32("SubscriberId", SubscriberId);
                int PackageId = (int)HttpContext.Session.GetInt32("packageId");
                Subscriptions subscriberinfo = subDataAccess.GetSubscriberById(SubscriberId);
                Subscriptions subscription = subDataAccess.GetpackagesById(PackageId);
                subscription.ChurchId = ChurchId;
                subscription.SubscriberId = SubscriberId;
                subscription.OrderId = "-";
                subscription.PackageId = PackageId;
                subscription.EmailAddress = subscriberinfo.EmailAddress;
                decimal PakageAmount = subscription.PackageCharge;
                int paymentLogId = subDataAccess.AddSubscriberpaymentLog(PackageId, SubscriberId, PakageAmount, "-",ChurchId,"-");
                if (paymentLogId > 0) { 
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
              return View("SubscriptionUserRegistration",subscriptions);

            }
        }


        public IActionResult SubscriptionUserLogin(int subscriptionId)
        {
            

            return View();
        }

        [HttpPost]
        public IActionResult UserLogin(string Email, string Password) 
        {

            Subscriptions subscription = subDataAccess.SubscriberLogin(Email, Password);
            if (subscription.SubscriberId>0) {
                int chrId = (int)HttpContext.Session.GetInt32("chrId");
                int subscriberId= (int)subscription.SubscriberId;
                Subscriptions sub = subDataAccess.SubscriberCheck(chrId, subscriberId);
                if (sub.PaymentId > 0)
                {
                    return RedirectToAction(nameof(Profile));
                }
                else
                {
                    HttpContext.Session.SetInt32("SubscriberId", subscription.SubscriberId);
                    TempData["SubscriberId"] = subscription.SubscriberId;
                    return RedirectToAction(nameof(Packages2));
                }
                            
            }
            else
            {
                ViewBag.Error = 2;
                return View("SubscriptionUserLogin");
            }

        
        }
        public void LoadCountryDDL()
        {
            try
            {

                IEnumerable<Place> countryList = _placeAccessLayer.GetCountries();
                List<SelectListItem> selectListItems = new List<SelectListItem>();
                foreach (var item in countryList)
                {
                    selectListItems.Add(new SelectListItem { Text = item.PlaceName.ToString(), Value = item.PlaceId.ToString() });
                }
                ViewBag.Countries = selectListItems;

            }
            catch (Exception e)
            {
              
                throw;
            }


                    }


        public IActionResult Profile()
        {
            int id = (int)HttpContext.Session.GetInt32("chrId");
            ChurchDataAccessLayer churchDataAccess = new ChurchDataAccessLayer();
            AnnouncementDataAccessLayer announcementDataAccessLayer = new AnnouncementDataAccessLayer();
            CameraDataAccessLayer camDataAccess = new CameraDataAccessLayer();
            RecordingDataAccessLayer recordDataAccess = new RecordingDataAccessLayer();
            ScheduleDataAccessLayer scheduleDataAccess = new ScheduleDataAccessLayer();
            MediaChurchDataAccessLayer mediaChurchDataAccess = new MediaChurchDataAccessLayer();
            NoticeDataAccessLayer noticeDataAccess = new NoticeDataAccessLayer();
            //PreviewChurchesDataAccessLayer previewChurchesDataAccessLayer = new PreviewChurchesDataAccessLayer();
            ChurchNewsLetterDataAccessLayer churchNewsLetterDataAccess = new ChurchNewsLetterDataAccessLayer();


            Profile profileModel = new Profile();
            string churchSlug = id.ToString();
            profileModel.Churches = churchDataAccess.GetChurchData(Convert.ToInt32(id));
            List<Announcement> announcementList = announcementDataAccessLayer.GetAnnouncement(id).ToList();
            if (announcementList.Count > 0)
                profileModel.Announcement = announcementList.First<Announcement>();
            else
                profileModel.Announcement = new Announcement();

            profileModel.NoticeList = noticeDataAccess.GetAllNotices(id).ToList();

            profileModel.CameraList = camDataAccess.GetAllCameras(id, "");
            profileModel.VideoList = mediaChurchDataAccess.GetByMediaType("Video", id).ToList();
            profileModel.SlideshowList = mediaChurchDataAccess.GetByMediaType("SlideShow", id).ToList();
            profileModel.PictureList = mediaChurchDataAccess.GetByMediaType("Picture", id).ToList();
            profileModel.newsletter = churchNewsLetterDataAccess.GetLetestNewsletterByChurch(id);

            //profileModel.Cameras = camDataAccess.GetCameraById(1,"");
            //  profileModel.Media = "";
            profileModel.RecordingList = recordDataAccess.Recording_GetByChurch(id);
            profileModel.ScheduleList = scheduleDataAccess.GetSearchSchedule(id, DateTime.Now, DateTime.Now.ToString("dddd"), -1).ToList<Schedule>();

            profileModel.NowScheduleList = Schedules_WhatsOnNow();

            profileModel.ScheduleListDay0 = scheduleDataAccess.GetSearchSchedule(id, System.DateTime.Now, System.DateTime.Now.ToString("dddd"), -1);
            profileModel.ScheduleListDay1 = scheduleDataAccess.GetSearchSchedule(id, System.DateTime.Now.AddDays(1), System.DateTime.Now.AddDays(1).ToString("dddd"), -1);
            profileModel.ScheduleListDay2 = scheduleDataAccess.GetSearchSchedule(id, System.DateTime.Now.AddDays(2), System.DateTime.Now.AddDays(2).ToString("dddd"), -1);
            profileModel.ScheduleListDay3 = scheduleDataAccess.GetSearchSchedule(id, System.DateTime.Now.AddDays(3), System.DateTime.Now.AddDays(3).ToString("dddd"), -1);
            profileModel.ScheduleListDay4 = scheduleDataAccess.GetSearchSchedule(id, System.DateTime.Now.AddDays(4), System.DateTime.Now.AddDays(4).ToString("dddd"), -1);
            profileModel.ScheduleListDay5 = scheduleDataAccess.GetSearchSchedule(id, System.DateTime.Now.AddDays(5), System.DateTime.Now.AddDays(5).ToString("dddd"), -1);
            profileModel.ScheduleListDay6 = scheduleDataAccess.GetSearchSchedule(id, System.DateTime.Now.AddDays(6), System.DateTime.Now.AddDays(6).ToString("dddd"), -1);

            return View(profileModel);
        }

        private IEnumerable<Schedule> Schedules_WhatsOnNow()
        {
            try
            {

                List<Schedule> schedules = _scheduleDataAccessLayer.GetWebsiteSchedule_WhatsOnNow().ToList<Schedule>();
                return schedules;
            }
            catch (Exception e)
            {

                
                throw;
            }
        }


    }
}