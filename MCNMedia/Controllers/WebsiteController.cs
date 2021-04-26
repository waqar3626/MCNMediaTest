using System;
using System.Collections.Generic;
using System.Linq;
using MaxMind.GeoIP2;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using MCNMedia_Dev._Helper;
using MCNMedia_Dev.Models;
using Microsoft.AspNetCore.Hosting;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static MCNMedia_Dev.Models.Church;
using System.IO;
using Microsoft.AspNetCore.Diagnostics;
using MaxMind.GeoIP2.Model;
using Microsoft.AspNetCore.Http.Extensions;
using DNTCaptcha.Core;
using Microsoft.Extensions.Options;
using System.Web;

namespace MCNMedia_Dev.Controllers
{
    public class WebsiteController : Controller
    {
        
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IDNTCaptchaValidatorService _validatorService;
        private readonly DNTCaptchaOptions _captchaOptions;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ScheduleDataAccessLayer _scheduleDataAccessLayer = new ScheduleDataAccessLayer();
        ChurchDataAccessLayer _churchDataAccessLayer = new ChurchDataAccessLayer();
        RecordingDataAccessLayer _recordDataAccess = new RecordingDataAccessLayer();
        WebsiteDataAccessLayer _websiteDataAccessLayer = new WebsiteDataAccessLayer();
        //SubscriptionDataAccessLayer subDataAccess = new SubscriptionDataAccessLayer();
        PlaceAccessLayer _placeAccessLayer = new PlaceAccessLayer();
        TestinomialDataAccessLayer testinomialDataAccess = new TestinomialDataAccessLayer();
        ChurchDonationDataAccessLayer churchDonationDataAccessLayes = new ChurchDonationDataAccessLayer();
        GenericModel gm = new GenericModel();
        Website website1 = new Website();
        //CookieOptions cookieOptions = new CookieOptions();
        public WebsiteController(IHostingEnvironment hostingEnvironment, IDNTCaptchaValidatorService validatorService, IOptions<DNTCaptchaOptions> captchaOptions)
        {
            _hostingEnvironment = hostingEnvironment;
            _validatorService = validatorService;
            _captchaOptions = captchaOptions==null?throw new ArgumentNullException(nameof(captchaOptions)):captchaOptions.Value;
        }
        public IActionResult index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ContactUs(Website web)

        {
            try
            {
                

                bool containsHTML = (web.ContactName != HttpUtility.HtmlEncode(web.ContactName));
                bool containsHTML2 = (web.ContactEmail != HttpUtility.HtmlEncode(web.ContactEmail));
                bool containsHTML3 = (web.ContactSubject != HttpUtility.HtmlEncode(web.ContactSubject));
                bool containsHTML4 = (web.Message != HttpUtility.HtmlEncode(web.Message));
                if (containsHTML || containsHTML2 || containsHTML3 || containsHTML4)
                {
                    throw new Exception("Data is invalid kindly provide a valid data");

                }
                else
                {
                    EmailHelper em = new EmailHelper();
                //em.SendEmail(string fromEmail, string toEmail, string toName, string subject, string body);
                em.SendEmail(web.ContactEmail, "mcnmedia9@gmail.com", web.ContactName, web.ContactSubject, web.Message);
                _websiteDataAccessLayer.AddContactForm(web);
                ModelState.Clear();
                ViewBag.SuccessMsg = "Email Send Successfully";
                return View();
                }
            }

            catch (Exception exp)
            {

                ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                return View();
            }
        }
        public IActionResult Home()
        {
            try
            {
                
                HttpContext.Session.SetString("Password", "");
                HttpContext.Session.SetString("UserType", "website");
                gm.ChurchList = _churchDataAccessLayer.GetWebsiteChurch().ToList<Church>();
                gm.LSchedulesInHour = UpComingSchedulesInCommingHour();
                gm.CountryList = _placeAccessLayer.GetCountries();
                //gm.testinomial = testinomialDataAccess.GetTestinomials().ToList<Testinomial>();

                List<SelectListItem> listCoutryDDL = LoadCountryDDL();

                ViewBag.Countries = listCoutryDDL;
                if (listCoutryDDL == null)
                {
                    throw new Exception("Country List is Empty");
                }
              
                return View(gm);
            }

            catch (Exception exp)
            {

                ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                return View(gm);
            }
        }
       

        private void Exception()
        {
            throw new NotImplementedException();
        }

        public JsonResult RecordingList()
        {
            try
            {
                int churchId = (int)HttpContext.Session.GetInt32("chrId");
                List<Recording> RecordingList = _recordDataAccess.Recording_GetByChurch(churchId).ToList();
                return Json(RecordingList);
            }
            catch (Exception e)
            {
                return Json(-1);
                //return Json(new { redirecturl = "../Views/Website/Error.cshtml" }, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }
        }

        public IActionResult Schedules()
        {
            GenericSchedule gmSch = new GenericSchedule();
            try
            {
                
                gmSch.TodaySchedule = _scheduleDataAccessLayer.GetSchedule_Today().ToList<Schedule>();
                gmSch.UpcomingSchedule = UpComingSchedules();
                gmSch.CurrentSchedule = Schedules_WhatsOnNow();
       
                return View(gmSch);
            }
            catch (Exception exp)
            {

                ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                return View(gmSch);
            }
        }

        private IEnumerable<Schedule> Schedules_WhatsOnNow()
        {
            try
            {
                List<Schedule> schedules = _scheduleDataAccessLayer.GetSchedule_WhatsOnNow().ToList<Schedule>();
                return schedules;
            }
            catch (Exception e)
            {
                ShowMesage("Schedule Whats On Now Errors : " + e.Message);
                throw;
            }
        }

        private IEnumerable<Schedule> UpComingSchedulesForChurch(int churchId)
        {
            try
            {
                List<Schedule> schedules = _scheduleDataAccessLayer.GetSchedule_UpComingByChurch(churchId).ToList<Schedule>();
                return schedules;
            }
            catch (Exception e)
            {
                ShowMesage("UpComing Schedules for Church Error : " + e.Message);
                throw;
            }
        }

        private IEnumerable<Schedule> UpComingSchedules()
        {
            try
            {
                List<Schedule> schedules = _scheduleDataAccessLayer.GetSchedule_UpComing_All().ToList<Schedule>();
                return schedules;
            }
            catch (Exception e)
            {
                ShowMesage("UpComing Schedules Error : " + e.Message);
                throw;
            }
        }
        private IEnumerable<Schedule> UpComingSchedulesInCommingHour()
        {
            try
            {
                List<Schedule> schedules = _scheduleDataAccessLayer.GetSchedule_UpComingHour().ToList<Schedule>();
                return schedules;
            }
            catch (Exception e)
            {
                ShowMesage("UpComing Schedules Error : " + e.Message);
                throw;
            }
        }

        public IActionResult ContactUs()
        {
            try
            {
                return View();
            }
            catch (Exception exp)
            {

                ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                return View();
            }
        }

        public IActionResult Churches()
        {
            List<SelectListItem> listCoutryDDL = new List<SelectListItem>();
            try
            {
                gm.ChurchList = _churchDataAccessLayer.GetByClientTypeChurch(1).ToList();
                gm.CountryList = _placeAccessLayer.GetCountries();
                listCoutryDDL = LoadCountryDDL();

                ViewBag.Countries = listCoutryDDL;
                if (listCoutryDDL == null)
                {
                    throw new Exception("Country List is Empty");
                }
               
                return View(gm);
            }
            catch (Exception exp)
            {
                ViewBag.Countries = listCoutryDDL;
                ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                return View(gm);
            }
        }

        public IActionResult Cathedrals()
        {
            List<SelectListItem> listCoutryDDL = new List<SelectListItem>();
            try
            {
                gm.ChurchList = _churchDataAccessLayer.GetByClientTypeChurch(2).ToList();
                gm.CountryList = _placeAccessLayer.GetCountries();
                listCoutryDDL = LoadCountryDDL();

                ViewBag.Countries = listCoutryDDL;
                if (listCoutryDDL == null)
                {
                    throw new Exception("Country List is Empty");
                }
               
                return View(gm);
            }
            catch (Exception exp)
            {
                ViewBag.Countries = listCoutryDDL;
                ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                return View(gm);
            }
        }

        public IActionResult FuneralHomes()
        {
            List<SelectListItem> listCoutryDDL = new List<SelectListItem>();
            try
            {
                //int churchId = gm.Churches.ChurchId;
                gm.ChurchList = _churchDataAccessLayer.GetByClientTypeChurch(3).ToList();
                gm.CountryList = _placeAccessLayer.GetCountries();
                listCoutryDDL = LoadCountryDDL();

                ViewBag.Countries = listCoutryDDL;
                if (listCoutryDDL == null)
                {
                    throw new Exception("Country List is Empty");
                }
            
                return View(gm);
            }
            catch (Exception exp)
            {
                ViewBag.Countries = listCoutryDDL;
                ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                return View(gm);
            }
        }

        public IActionResult Cameras()
        {
            List<SelectListItem> listCoutryDDL = new List<SelectListItem>();
            try
            {
                listCoutryDDL = LoadCountryDDL();

                ViewBag.Countries = listCoutryDDL;
                if (listCoutryDDL == null)
                {
                    throw new Exception("Country List is Empty");
                }
                List<Church> churches = _churchDataAccessLayer.GetByClientTypeChurch(clientTypeId: -1).ToList<Church>();
                ViewBag.NoChurch = 0;
                if (!string.IsNullOrEmpty(HttpContext.Request.Query["Country"].ToString()))
                {
                    string countryName = Request.Query["Country"].ToString().Replace("-", " ");
                    ViewBag.CountryID = -1;
                    if (countryName != "all")
                    {

                        churches = churches.FindAll(x => x.CountryName.ToLower() == countryName.ToLower()).ToList<Church>();
                        if (churches.Count > 0)
                        {
                            ViewBag.CountryID = churches.First().CountryId;
                        }
                        else
                        {
                            ViewBag.SearchFilter = $"Country = {countryName}";

                            
                        }
                    }
                    ViewBag.countryName = Request.Query["Country"].ToString();
                    HttpContext.Session.SetString("Country", Request.Query["Country"].ToString());

                    if (!string.IsNullOrEmpty(HttpContext.Request.Query["County"].ToString()))
                    {
                        string countyName = Request.Query["County"].ToString().Replace("-", " ");
                        ViewBag.CountyID = -1;
                        TempData["CountyID"] = -1;
                        if (countyName != "all")
                        {
                            churches = churches.FindAll(x => x.CountyName.ToLower() == countyName.ToLower()).ToList<Church>();
                            if (churches.Count() > 0) {
                               ViewBag.CountyID = churches.First().CountyId;
                            }
                            else
                            {
                                ViewBag.CountyID = -1;
                            }

                        }
                        ViewBag.CountyList = 0;
                        countyName = countyName.Replace("-", " ");
                        ViewBag.SearchFilter = $"County = {countyName}";
                        if (churches.Count() == 0)
                            ViewBag.NoChurch = 1;
                    }
                    else
                    {
                        ViewBag.CountyList = 1;
                        string countryName1 = Request.Query["Country"].ToString();

                        List<Place> countyList = _placeAccessLayer.GetCountiesByCountryName(countryName).ToList();

                        gm.CountyList = _placeAccessLayer.GetCountiesByCountryName(countryName);
                        gm.CountryList = _placeAccessLayer.GetCountries();
                        if (gm.CountyList.Count() == 0)
                            ViewBag.NoChurch = 2;
                        ViewBag.SearchFilter = $"Country = {countryName1}";
                       
                        return View(gm);
                    }
                }

                if (!string.IsNullOrEmpty(HttpContext.Request.Query["County"].ToString()))
                {
                    string searchFilter = Request.Query["County"].ToString().ToLower();
                    searchFilter = searchFilter.Replace("-", " ");
                    churches = churches.FindAll(x => x.CountyName.ToLower().StartsWith(searchFilter)).ToList<Church>();
                    ViewBag.CountyList = 0;
                    ViewBag.countyName = HttpContext.Session.GetString("Country") + "  / " + Request.Query["County"].ToString().Replace("-", " "); //Request.Query["Country"].ToString();
                    gm.CountryList = _placeAccessLayer.GetCountries();
                    gm.ChurchList = churches;
                    searchFilter = searchFilter.Replace("-", " ");
                    ViewBag.SearchFilter = $"County = {searchFilter}";
                    
                    return View(gm);
                }
                if (!string.IsNullOrEmpty(HttpContext.Request.Query["Search"].ToString()))
                {
                    //string searchFilter = Request.Query["Search"].ToString().ToLower();
                    string searchFilter = Request.Query["Search"].ToString().ToLower();
                    //string input = new String(searchFilter.Where(c => c != '-' && (c < '0' || c > '9')).ToArray());
                    //input.ToLower();
                    searchFilter.ToLower();
                    string first = searchFilter.Split(' ').First();
                    churches = churches.FindAll(x => x.ChurchName.ToLower().Contains(searchFilter) ||
                    x.Slug.ToLower().Contains(searchFilter) ||
                    x.Town.ToLower().Contains(first) ||
                    x.CountryName.ToLower().Contains(searchFilter) ||
                    x.CountyName.ToLower().Contains(first)).ToList<Church>();
                    ViewBag.CountyList = 0;
                    ViewBag.SearchFilter = Request.Query["Search"].ToString();
                }
                gm.ChurchList = churches;
                if (gm.ChurchList.Count() == 0)
                    ViewBag.NoChurch = 1;
                gm.CountryList = _placeAccessLayer.GetCountries();
             
                return View(gm);
            }
            catch (Exception exp)
            {
                ViewBag.Countries = listCoutryDDL;
                ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                return View(gm);
            }
        }

        public JsonResult LoadCountyDDL(int countryId)
        {
            try
            {

                List<Place> countyList = _placeAccessLayer.GetCounties(countryId).ToList();

                return Json(countyList);

            }
            catch (Exception exp)
            {
                return Json(exp.Message);
                //return Json(new { redirecturl = "../Views/Website/Error.cshtml" }, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }

        }

        public List<SelectListItem> LoadCountryDDL()
        {

            IEnumerable<Place> countryList = _placeAccessLayer.GetCountries();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (var item in countryList)
            {
                selectListItems.Add(new SelectListItem { Text = item.PlaceName.ToString(), Value = item.PlaceId.ToString() });
            }

            return selectListItems;






        }

        public IActionResult ProcessForm()
        {
            try
            {
                LoadCountryDDL();
                return View("Home");
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult AddContactForm(Website website)
        {
            try
            {
                _websiteDataAccessLayer.AddContactForm(website);
                return RedirectToAction("Home");
            }
            catch (Exception exp)
            {

                ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                return View();
            }
        }

        public ViewResult Terms()
        {
            try
            {
                return View();
            }
            catch (Exception exp)
            {
                ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                return View();

            }
        
        }

        public ViewResult Privacy()
        {
            try
            {
                return View();
            }
            catch (Exception exp)
            {
                ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                return View();
            }
            
        }


        public IActionResult RecordingLock()
        {
            try
            {
                string Password = Convert.ToString(TempData["RecordingPassword"].ToString());
                ViewData["RecordingId"] = TempData["RecordingId"];
                ViewData["RecordingPassword"] = Password;
                return View();
            }
            catch (Exception exp)
            {
                ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                return View();
            }
            //here
           
        }
        [HttpPost]
        public IActionResult RecordingLock(string RecordingPass, string Password, int recodingId)
        {
            try
            {
                if (RecordingPass == null)
                {
                    RecordingPass = Convert.ToString(TempData["RecordingPassword"]);
                    recodingId = Convert.ToInt32(TempData["RecordingId"]);
                }
                if (RecordingPass == Password)
                {
                    TempData["RecordingPass"] = 1;
                    TempData["RecordingId"] = Convert.ToInt32(recodingId);
                    //http://localhost:56963/Website/Player/67
                    return RedirectToAction(nameof(Player), new { id = recodingId });
                }
                else
                {
                    ViewBag.ErrorMsg = "Password Incorrect ! Please Enter correct password.";
                    return View();
                }
            }
            catch (Exception exp)
            {

                ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                return View();
            }
        }

        public IActionResult Player(int id)
        {
            Recording recording = new Recording();
            try
            {
                int recordingPass = 0;
                RecordingDataAccessLayer recordingDataAccessLayer = new RecordingDataAccessLayer();
                if (id == 0)
                {
                    id = Convert.ToInt32(TempData["RecordingId"]);
                    recordingPass = Convert.ToInt32(TempData["RecordingPass"]);// Convert.ToInt32(HttpContext.Session.GetInt32("RecordingPass"));
                }
                 recording = recordingDataAccessLayer.Recording_GetById(id);
                int pass = recording.Password.Count();
                if (recording.Password.Count() > 0)
                {
                    TempData["RecordingPassword"] = recording.Password;
                    TempData["RecordingId"] = id;
                    ViewData["RecordingId"] = id;
                    if (!string.IsNullOrEmpty(Convert.ToString(TempData["RecordingPass"])))
                    {
                        recordingPass = 1;
                    }

                    if (recordingPass == 1)
                    {

                    }
                    else
                    {
                        ViewData["RecordingPassword1"] = recording.Password;
                        return RedirectToAction(nameof(RecordingLock));
                    }
                }
                return View(recording);
            }
            catch (Exception exp)
            {

                ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                return View(recording);
            }
        }

        [HttpPost]
        public IActionResult ChurchLock(string Password)
        {
            Church church = new Church();
            try
            {
                string Slug = TempData["Slug"].ToString();

                church = _churchDataAccessLayer.GetChurchDataBySlug(Slug);


                if (church.Password == Password)
                {
                    TempData["ChurchPass"] = "true";
                    return RedirectToAction(nameof(Profile), new { id = Slug });
                }
                else
                {
                    ViewBag.ErrorMsg = "Password Incorrect ! Please Enter correct password.";
                        TempData["Slug"] = TempData["Slug"].ToString();
                    TempData["ChrName"] = church.ChurchName;
                    return View();
                }
               
            }
            catch (Exception exp)
            {
                TempData["ChrName"] = church.ChurchName;
                ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                return View();
            }
        }

        public IActionResult Profile(string id)
        {
            string originalPath = new Uri(HttpContext.Request.GetDisplayUrl()).AbsoluteUri;
            string slug = originalPath.Split('/').Last();
            Profile profileModel = new Profile();
            try
            {
                string churchPass = "false";
                string originalPath1 = id;

                if (!string.IsNullOrEmpty(Convert.ToString(TempData["ChurchPass"])))
                {
                    churchPass = TempData["ChurchPass"].ToString();
                }
                ViewData["Title"] = id;
                if (string.IsNullOrEmpty(id))
                {
                    return RedirectToAction(nameof(Cameras));
                }
                ChurchDataAccessLayer churchDataAccess = new ChurchDataAccessLayer();
                AnnouncementDataAccessLayer announcementDataAccessLayer = new AnnouncementDataAccessLayer();
                CameraDataAccessLayer camDataAccess = new CameraDataAccessLayer();
                RecordingDataAccessLayer recordDataAccess = new RecordingDataAccessLayer();
                ScheduleDataAccessLayer scheduleDataAccess = new ScheduleDataAccessLayer();
                MediaChurchDataAccessLayer mediaChurchDataAccess = new MediaChurchDataAccessLayer();
                NoticeDataAccessLayer noticeDataAccess = new NoticeDataAccessLayer();
                ChurchNewsLetterDataAccessLayer churchNewsLetterDataAccess = new ChurchNewsLetterDataAccessLayer();
               
                profileModel.Churches = churchDataAccess.GetChurchDataBySlug(id);
                TempData["ChurchPasswordToChurchLock"] = profileModel.Churches.Password;
               if(profileModel.Churches.ChurchId != 0) { 
                
                if (profileModel.Churches.Password.Count() > 0)
                {
                    if (churchPass == "true")
                    {
                        // Password provided
                    }
                    else
                    {
                        TempData["Slug"] = id;
                        TempData["ChrName"] = profileModel.Churches.ChurchName;
                        return View("ChurchLock");
                    }
                }
                }
                else
                {
                    profileModel.Churches.ChurchId=0;
                    TempData["ErrorMsgBit"] = "404";
                    ViewBag.ErrorMsg = "No church found against your action";

                    return RedirectToAction(nameof(Home));

                }
                // it for paper view Know we dont need it
                //int subscriberPaid = Convert.ToInt32(TempData["paymentId"]);
                //Visitor visitor = new Visitor(Request, HttpContext);
                ////string visitorLocation = CheckVisitorLocation();
                //int allowToProfile = subDataAccess.ChurchRegionCheck(profileModel.Churches.ChurchId, visitor.CountryName);
                ////if (allowToProfile == 1 || subscriberPaid > 0)
                //if (true)
                //{
                //HttpContext.Session.SetInt32("chrId", profileModel.Churches.ChurchId);
                int churchId = profileModel.Churches.ChurchId;// profileModel.Churches = churchDataAccess.GetChurchData(Convert.ToInt32( churchId));
                String ip = website1.IP;
                //_websiteDataAccessLayer.Analytics(churchId, visitor.IpAddress, visitor.CountryName);
                List<Announcement> announcementList = announcementDataAccessLayer.GetAnnouncement(churchId).ToList();
                if (announcementList.Count > 0)
                    profileModel.Announcement = announcementList.First<Announcement>();
                else
                    profileModel.Announcement = new Announcement();
                List<Notice> noticeList = noticeDataAccess.GetAllNotices(churchId).ToList();
                if (noticeList.Count > 0)
                    profileModel.notice = noticeList.First<Notice>();
                else
                    profileModel.notice = new Notice();

                profileModel.mediaList = camDataAccess.GetWebsiteMedia(churchId);
                profileModel.CameraList = camDataAccess.GetActiveCameraByChurch(churchId);
                profileModel.VideoList = mediaChurchDataAccess.GetByMediaType("Video", churchId).ToList();
                profileModel.SlideshowList = mediaChurchDataAccess.GetByMediaType("SlideShow", churchId).ToList();
                profileModel.PictureList = mediaChurchDataAccess.GetByMediaType("Picture", churchId).ToList();
                profileModel.newsletter = churchNewsLetterDataAccess.GetLetestNewsletterByChurch(churchId);

                profileModel.UpcomingSchedule = UpComingSchedulesForChurch(churchId);

                profileModel.RecordingList = recordDataAccess.Recording_GetByChurch(churchId);
                profileModel.ScheduleList = scheduleDataAccess.GetSearchSchedule(churchId, DateTime.Now, DateTime.Now.ToString("dddd"), -1).ToList<Schedule>();

                profileModel.NowScheduleList = Schedules_WhatsOnNow();
                profileModel.churchDonation = churchDonationDataAccessLayes.GetDonationByChurch(churchId);
                profileModel.ScheduleListDay0 = scheduleDataAccess.GetSearchSchedule(churchId, System.DateTime.Now, System.DateTime.Now.ToString("dddd"), -1);
                profileModel.ScheduleListDay1 = scheduleDataAccess.GetSearchSchedule(churchId, System.DateTime.Now.AddDays(1), System.DateTime.Now.AddDays(1).ToString("dddd"), -1);
                profileModel.ScheduleListDay2 = scheduleDataAccess.GetSearchSchedule(churchId, System.DateTime.Now.AddDays(2), System.DateTime.Now.AddDays(2).ToString("dddd"), -1);
                profileModel.ScheduleListDay3 = scheduleDataAccess.GetSearchSchedule(churchId, System.DateTime.Now.AddDays(3), System.DateTime.Now.AddDays(3).ToString("dddd"), -1);
                profileModel.ScheduleListDay4 = scheduleDataAccess.GetSearchSchedule(churchId, System.DateTime.Now.AddDays(4), System.DateTime.Now.AddDays(4).ToString("dddd"), -1);
                profileModel.ScheduleListDay5 = scheduleDataAccess.GetSearchSchedule(churchId, System.DateTime.Now.AddDays(5), System.DateTime.Now.AddDays(5).ToString("dddd"), -1);
                profileModel.ScheduleListDay6 = scheduleDataAccess.GetSearchSchedule(churchId, System.DateTime.Now.AddDays(6), System.DateTime.Now.AddDays(6).ToString("dddd"), -1);
                profileModel.MediaChurchList = mediaChurchDataAccess.spSlideShowImagesGetByChurch(churchId).ToList();
                List<Schedule> nextEventSchedule = new List<Schedule>();
                foreach (Schedule schedule in profileModel.ScheduleListDay0)
                {
                    if (Convert.ToDateTime(schedule.EventTime) > DateTime.Now)
                    {
                        schedule.EventDay = "Today";
                        nextEventSchedule.Add(schedule);
                        profileModel.NextScheduleList = nextEventSchedule;
                    }
                }
                if (nextEventSchedule.Count == 0 && profileModel.ScheduleListDay1.Count() > 0)
                {
                    foreach (Schedule schedule in profileModel.ScheduleListDay1)
                    {
                        nextEventSchedule.Add(schedule);
                        profileModel.NextScheduleList = nextEventSchedule;
                    }
                }
                else if (nextEventSchedule.Count == 0 && profileModel.ScheduleListDay2.Count() > 0)
                {
                    foreach (Schedule schedule in profileModel.ScheduleListDay2)
                    {
                        nextEventSchedule.Add(schedule);
                        profileModel.NextScheduleList = nextEventSchedule;
                    }
                }
                else if (nextEventSchedule.Count == 0 && profileModel.ScheduleListDay3.Count() > 0)
                {
                    foreach (Schedule schedule in profileModel.ScheduleListDay3)
                    {
                        nextEventSchedule.Add(schedule);
                        profileModel.NextScheduleList = nextEventSchedule;
                    }
                }
                else if (nextEventSchedule.Count == 0 && profileModel.ScheduleListDay4.Count() > 0)
                {
                    foreach (Schedule schedule in profileModel.ScheduleListDay4)
                    {
                        nextEventSchedule.Add(schedule);
                        profileModel.NextScheduleList = nextEventSchedule;
                    }
                }
                else if (nextEventSchedule.Count == 0 && profileModel.ScheduleListDay5.Count() > 0)
                {
                    foreach (Schedule schedule in profileModel.ScheduleListDay5)
                    {
                        nextEventSchedule.Add(schedule);
                        profileModel.NextScheduleList = nextEventSchedule;
                    }
                }
                else if (nextEventSchedule.Count == 0 && profileModel.ScheduleListDay6.Count() > 0)
                {
                    foreach (Schedule schedule in profileModel.ScheduleListDay6)
                    {
                        nextEventSchedule.Add(schedule);
                        profileModel.NextScheduleList = nextEventSchedule;
                    }
                }
                else
                {
                    profileModel.NextScheduleList = nextEventSchedule;
                }
                // http://localhost:56963/Camera
                Uri uri = Request.GetTypedHeaders().Referer;
                ViewData["slugForlockPage"] = id;

                //Response.Redirect(originalPath+"/"+id);
                return View(profileModel);
                //}
                //else
                //{
                //    HttpContext.Session.SetInt32("chrId", profileModel.Churches.ChurchId);
                //    ViewBag.ChurchId = profileModel.Churches.ChurchId;
                //    churchPass = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchPass"));
                //    return RedirectToAction("Packages", "Subscription");
                //}
            }
            catch (Exception exp)
            {

                ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                return View(profileModel);
            }
        }

        private string CheckVisitorLocation()
        {
            if (IsLocal(HttpContext.Connection))
            {
                return "Pakistan";
            }
            string rootPath = Directory.GetCurrentDirectory();
            using (var reader = new DatabaseReader(Path.Combine(rootPath, "wwwroot/GeoLite2-Country.mmdb")))
            {
                // Determine the IP Address of the request
                IPAddress ipAddress;
                var headers = Request.Headers.ToList();
                if (headers.Exists((kvp) => kvp.Key == "X-Forwarded-For"))
                {
                    // when running behind a load balancer you can expect this header
                    var header = headers.First((kvp) => kvp.Key == "X-Forwarded-For").Value.ToString();
                    ipAddress = IPAddress.Parse(header);
                }
                else
                if (headers.Exists((kvp) => kvp.Key == "REMOTE_ADDR"))
                {
                    // when running behind a load balancer you can expect this header
                    var header = headers.First((kvp) => kvp.Key == "REMOTE_ADDR").Value.ToString();
                    ipAddress = IPAddress.Parse(header);
                }
                else
                {
                    // this will always have a value (running locally in development won't have the header)
                    ipAddress = Request.HttpContext.Connection.RemoteIpAddress;
                }
                website1.IP = ipAddress.ToString();
                // Get the city from the IP Address
                var countryInfo = reader.Country(ipAddress);
                var countryname = countryInfo.Country.ToString();
                return countryname;
            }
        }

        private bool IsLocal(ConnectionInfo connection)
        {
            var remoteAddress = connection.RemoteIpAddress.ToString();
            // if unknown, assume not local
            if (String.IsNullOrEmpty(remoteAddress))
                return false;

            // check if localhost
            if (remoteAddress == "127.0.0.1" || remoteAddress == "::1")
            {
                website1.IP = remoteAddress;
                return true;
            }
            // compare with local address
            if (remoteAddress == connection.LocalIpAddress.ToString())
            {
                website1.IP = remoteAddress;
                return true;
            }
            return false;
        }

        [HttpGet]
        public JsonResult SearchRecording(string EventName, string FromDate, string ToDate, string Slug)
        {
            string url = HttpContext.Request.Query.ToString();
            try
            {
                if (Slug == null)
                {
                    Slug = "^^^^^^_____++++++++++";
                }
                if (true)
                {
                    url = Convert.ToString(HttpContext.Request.Query["Camera/{id?}"].ToString());
                }
                DateTime fromDate = Convert.ToDateTime(FromDate);
                DateTime toDate = Convert.ToDateTime(ToDate);
                List<Recording> listRecording = _recordDataAccess.RecordingSearch(fromDate, toDate, Slug, -1, EventName).ToList();
                ViewData["slugForlockPage"] = Slug;
              
                return Json(listRecording);
            }
            catch (Exception exp)
            {
                
                return Json("-1"+"^"+exp.Message);
            }
        }

        public IActionResult SubscriberLogout()
        {
            try
            {
                foreach (var cookieKey in Request.Cookies.Keys)
                {
                    if (cookieKey == "SubscriberId" || cookieKey == "SubscriberName")
                    {
                        Response.Cookies.Delete(cookieKey);
                    }
                }
                return RedirectToAction(nameof(Home));
            }
            catch (Exception exp)
            {

                ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                return View();
            }
        }

        public IActionResult RssFeed()
        {
            return View();
        }


        public IActionResult Error()
        {

            return View();
        }

        //[Route("Error")]
        //public IActionResult Error()
        //{
        //    var exceptionHandler = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        //    ViewBag.exceptionPath = exceptionHandler.Path;
        //    ViewBag.ExceptionMessage = exceptionHandler.Error.Message;
        //    ViewBag.Stacktrace = exceptionHandler.Error.StackTrace;
        //    return View();
        //}
        private void ShowMesage(string exceptionMessage)
        {
            log.Error("Exception : " + exceptionMessage);
        }

    }
}
