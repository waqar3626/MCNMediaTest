using System;
using System.Collections.Generic;
using System.Linq;
using MaxMind.GeoIP2;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using MCNMedia_Dev._Helper;
using MCNMedia_Dev.Models;
using MCNMedia_Dev._Helper;
using Microsoft.AspNetCore.Hosting;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static MCNMedia_Dev.Models.Church;
using System.IO;
using MaxMind.GeoIP2.Model;

namespace MCNMedia_Dev.Controllers
{
    public class WebsiteController : Controller
    {

        private readonly IHostingEnvironment _hostingEnvironment;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ScheduleDataAccessLayer _scheduleDataAccessLayer = new ScheduleDataAccessLayer();
        ChurchDataAccessLayer _churchDataAccessLayer = new ChurchDataAccessLayer();
        WebsiteDataAccessLayer _websiteDataAccessLayer = new WebsiteDataAccessLayer();
        SubscriptionDataAccessLayer subDataAccess = new SubscriptionDataAccessLayer();
        PlaceAccessLayer _placeAccessLayer = new PlaceAccessLayer();

        GenericModel gm = new GenericModel();
        public WebsiteController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ContactUs(Website web)

        {
            try
            {
                EmailHelper em = new EmailHelper();
                //em.SendEmail(string fromEmail, string toEmail, string toName, string subject, string body);
                em.SendEmail(web.ContactEmail, "mcnmedia9@gmail.com", web.ContactName, web.ContactSubject, web.Message);
                //string name = web.ContactName;
                //string email = web.ContactEmail;
                //string subject = web.ContactSubject;
                //string massage = web.Message;
                //MailMessage mailmassage = new MailMessage();
                //mailmassage.To.Add("mcnmedia9@gmail.com ");
                //mailmassage.Subject = subject;
                //mailmassage.Body = massage;
                //mailmassage.From = new MailAddress(email);
                //mailmassage.IsBodyHtml = false;
                //SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                //smtp.Port = 25;
                //smtp.UseDefaultCredentials = true;
                //smtp.EnableSsl = true;
                //smtp.Credentials = new NetworkCredential("mcnmedia9@gmail.com ", "M3di@mcN");
                //smtp.Send(mailmassage);
                //ViewBag.message = "The Mail has been send by " + web.ContactName + " Successfully....!," + mailmassage.To + ".";
                _websiteDataAccessLayer.AddContactForm(web);
                ModelState.Clear();
                return View();
            }

            catch (Exception e)
            {
                ShowMesage("Contactus Email Sending Failed : " + e.Message);
                throw;
            }
        }
        public IActionResult Home()
        {
            try
            {
                LoadCountryDDL();
                HttpContext.Session.SetString("Password", "");
                HttpContext.Session.SetString("UserType", "website");
                gm.ChurchList = _churchDataAccessLayer.GetWebsiteChurch().ToList<Church>();
                gm.LSchedules = UpComingSchedules();
                gm.CountryList = _placeAccessLayer.GetCountries();
                return View(gm);
            }
            catch (Exception e)
            {
                ShowMesage("Home : " + e.Message);
                throw;
            }
        }

        public IActionResult Schedules()
        {
            try
            {
                GenericSchedule gmSch = new GenericSchedule();
                gmSch.TodaySchedule = _scheduleDataAccessLayer.GetWebsiteSchedule_TodaySchedules().ToList<Schedule>();
                gmSch.UpcomingSchedule = UpComingSchedules();
                gmSch.CurrentSchedule = Schedules_WhatsOnNow();
                return View(gmSch);
            }
            catch (Exception e)
            {
                ShowMesage("Schedules Error : " + e.Message);
                throw;
            }
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

                ShowMesage("Schedules Error : " + e.Message);
                throw;
            }
        }

        private IEnumerable<Schedule> UpComingSchedules()
        {
            try
            {

                List<Schedule> schedules = _scheduleDataAccessLayer.GetWebsiteSchedule_UpComingSchedules().ToList<Schedule>();
                return schedules;
            }
            catch (Exception e)
            {

                ShowMesage("Schedules Error : " + e.Message);
                throw;
            }
        }

       
        public IActionResult ContactUs()
        {
            try
            {
                

                return View();
            }
            catch (Exception e)
            {
                ShowMesage("Contact Us : " + e.Message);
                throw;
            }
        }

        public IActionResult Churches()
        {
            try
            {
                gm.ChurchList = _churchDataAccessLayer.GetByClientTypeChurch(1).ToList();
                gm.CountryList = _placeAccessLayer.GetCountries();
                return View(gm);
            }
            catch (Exception e)
            {
                ShowMesage("Churches : " + e.Message);
                throw;
            }
        }

        public IActionResult Cathedrals()
        {
            try
            {
                gm.ChurchList = _churchDataAccessLayer.GetByClientTypeChurch(2).ToList();
                gm.CountryList = _placeAccessLayer.GetCountries();
                return View(gm);
            }
            catch (Exception e)
            {
                ShowMesage("Cathedrals" + e.Message);
                throw;
            }
        }

        public IActionResult FuneralHomes()
        {
            try
            {
                //int churchId = gm.Churches.ChurchId;
                gm.ChurchList = _churchDataAccessLayer.GetByClientTypeChurch(3).ToList();
                gm.CountryList = _placeAccessLayer.GetCountries();
                return View(gm);
            }
            catch (Exception e)
            {
                ShowMesage("FuneralHomes : " + e.Message);
                throw;
            }
        }

        public IActionResult Cameras()
        {
            try
            {
                List<Church> churches = _churchDataAccessLayer.GetByClientTypeChurch(clientTypeId: -1).ToList<Church>();

                if (!string.IsNullOrEmpty(HttpContext.Request.Query["Country"].ToString()))
                {
                    string countryName = Request.Query["Country"].ToString().Replace("-", " ");
                    churches = churches.FindAll(x => x.CountryName.ToLower() == countryName.ToLower()).ToList<Church>();
                    ViewBag.countryName = Request.Query["Country"].ToString();

                    if (!string.IsNullOrEmpty(HttpContext.Request.Query["County"].ToString()))
                    {
                        string countyName = Request.Query["County"].ToString().Replace("-", " ");
                        churches = churches.FindAll(x => x.CountyName.ToLower() == countyName.ToLower()).ToList<Church>();
                        ViewBag.CountyList = 0;
                        ViewBag.countyName = Request.Query["County"].ToString();
                    }

                    else
                    {
                        ViewBag.CountyList = 1;
                        string countryName1 = Request.Query["Country"].ToString();

                        List<Place> countyList = _placeAccessLayer.GetCountiesByCountryName(countryName).ToList();

                        gm.CountyList = _placeAccessLayer.GetCountiesByCountryName(countryName);
                        gm.CountryList = _placeAccessLayer.GetCountries();
                        return View(gm);
                        
                    }
                   
                }

                if (!string.IsNullOrEmpty(HttpContext.Request.Query["County"].ToString()))
                {

                    string searchFilter = Request.Query["County"].ToString().ToLower();
                    churches = churches.FindAll(x => x.CountyName.ToLower().Contains(searchFilter)).ToList<Church>();
                    ViewBag.CountyList = 0;
                    ViewBag.countyName = Request.Query["County"].ToString();
                    gm.CountryList = _placeAccessLayer.GetCountries();
                    gm.ChurchList = churches;
                    return View(gm);
                }
                if (!string.IsNullOrEmpty(HttpContext.Request.Query["Search"].ToString()))
                {
                    string searchFilter = Request.Query["Search"].ToString().ToLower();
                    //string searchFilter = Request.Query["Search"].ToString().ToLower();
                    string input = new String(searchFilter.Where(c => c != '-' && (c < '0' || c > '9')).ToArray());
                    input.ToLower();
                    string last = input.Split(' ').Last();
                    churches = churches.FindAll(x => x.ChurchName.ToLower().Contains(input) ||
                    x.Town.ToLower().Contains(last) ||
                    x.CountryName.ToLower().Contains(last) ||
                    x.CountyName.ToLower().Contains(last)).ToList<Church>();
                    ViewBag.CountyList = 0;
                }

                gm.ChurchList = churches;
                gm.CountryList = _placeAccessLayer.GetCountries();
                return View(gm);

            }
            catch (Exception e)
            {
                ShowMesage("Camera : " + e.Message);
                throw;
            }
        }

        public JsonResult LoadCountyDDL(int countryId)
        {
            try
            {
                List<Place> countyList = _placeAccessLayer.GetCounties(countryId).ToList();
                return Json(countyList);
            }
            catch (Exception e)
            {
                ShowMesage("Load County DropDown : " + e.Message);
                throw;
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
                ShowMesage("Load Country DropDown : " + e.Message);
                throw;
            }
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
                ShowMesage("ProcessForm Errors : " + e.Message);
                throw;
            }
        }

        [HttpPost]
        public IActionResult AddContactForm(Website website)
        {
            _websiteDataAccessLayer.AddContactForm(website);
            return RedirectToAction("Home");
        }

        public ViewResult Terms()
        {
            return View();
        }

        public ViewResult Privacy()
        {
            return View();
        }

        public IActionResult ChurchLock()
        {
            return View();
        }
        public IActionResult RecordingLock()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RecordingLock(RecordingLock recordingLock)
        {
            String pass = HttpContext.Session.GetString("RecordingPass").ToString();
            if (recordingLock.Password == pass)
            {
                HttpContext.Session.SetInt32("RecordingPass", 1);
                return RedirectToAction(nameof(Player));
            }
            else
            {
                ViewBag.IsSuccess = 3;
                return View();
            }

        }

        public IActionResult Player(int id)
        {
            int recordingPass = 0;
            RecordingDataAccessLayer recordingDataAccessLayer = new RecordingDataAccessLayer();
            if (id == 0)
            {
                id = Convert.ToInt32(HttpContext.Session.GetInt32("RecordingId"));
                recordingPass = Convert.ToInt32(HttpContext.Session.GetInt32("RecordingPass"));
            }
            Recording recording = recordingDataAccessLayer.Recording_GetById(id);
            int pass = recording.Password.Count();
            if (recording.Password.Count() > 0)
            {
                HttpContext.Session.SetString("RecordingPass", recording.Password);
                HttpContext.Session.SetInt32("RecordingId", id);
                if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("UserId").ToString()))
                {
                    int usertype = Convert.ToInt32(HttpContext.Session.GetInt32("UserType"));
                }
                else
                {
                    if (recordingPass == 1)
                    {

                    }
                    else
                    {

                        return RedirectToAction(nameof(RecordingLock));
                    }
                }
            }
            return View(recording);

        }


        [HttpPost]
        public IActionResult ChurchLock(ChurchLock churchLock)
        {
            try
            {
                String pass = HttpContext.Session.GetString("ChurchPassword").ToString();
                if (churchLock.Password == pass)
                {
                    HttpContext.Session.SetInt32("ChurchPass", 1);
                    return RedirectToAction(nameof(Profile));
                }
                else
                {
                    ViewBag.IsSuccess = 3;
                    //ViewData["Message"] = "Incorrect Password";
                    return View();
                }
            }
           
            catch (Exception e)
            {
                ShowMesage("ProcessForm Errors : " + e.Message);
                throw;
            }


        }



        public IActionResult Profile(string id)
        {
            try
            {

                int churchPass = 0;
                if (id == null)
                {
                    id = HttpContext.Session.GetString("slug");
                    churchPass = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchPass"));
                }
                ViewData["Title"] = id;
                ChurchDataAccessLayer churchDataAccess = new ChurchDataAccessLayer();
                AnnouncementDataAccessLayer announcementDataAccessLayer = new AnnouncementDataAccessLayer();
                CameraDataAccessLayer camDataAccess = new CameraDataAccessLayer();
                RecordingDataAccessLayer recordDataAccess = new RecordingDataAccessLayer();
                ScheduleDataAccessLayer scheduleDataAccess = new ScheduleDataAccessLayer();
                MediaChurchDataAccessLayer mediaChurchDataAccess = new MediaChurchDataAccessLayer();
                NoticeDataAccessLayer noticeDataAccess = new NoticeDataAccessLayer();
                ChurchNewsLetterDataAccessLayer churchNewsLetterDataAccess = new ChurchNewsLetterDataAccessLayer();
                Profile profileModel = new Profile();
                profileModel.Churches = churchDataAccess.GetChurchDataBySlug(id);
                HttpContext.Session.SetString("slug", id);
                HttpContext.Session.SetInt32("chrId", profileModel.Churches.ChurchId);
                if (profileModel.Churches.Password.Count() > 0)
                {
                    HttpContext.Session.SetString("ChurchPassword", profileModel.Churches.Password);
                    HttpContext.Session.SetString("slug", id);
                    if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("UserId").ToString()))
                    {
                        int usertype = Convert.ToInt32(HttpContext.Session.GetInt32("UserType"));
                    }
                    else
                    {
                        if (churchPass == 1)
                        {

                        }
                        else
                        {

                            return RedirectToAction(nameof(ChurchLock));
                        }
                    }

                }



                int SubscriberPaid = Convert.ToInt32(TempData["paymentId"]);
                string visitorLocation = CheckVisitorLocation();
                if (visitorLocation == "United Kingdom" || visitorLocation == "Ireland" || SubscriberPaid > 0)
                {
                    int churchId = profileModel.Churches.ChurchId;// profileModel.Churches = churchDataAccess.GetChurchData(Convert.ToInt32( churchId));
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


                    profileModel.CameraList = camDataAccess.GetActiveCameraByChurch(churchId);
                    profileModel.VideoList = mediaChurchDataAccess.GetByMediaType("Video", churchId).ToList();
                    profileModel.SlideshowList = mediaChurchDataAccess.GetByMediaType("SlideShow", churchId).ToList();
                    profileModel.PictureList = mediaChurchDataAccess.GetByMediaType("Picture", churchId).ToList();
                    profileModel.newsletter = churchNewsLetterDataAccess.GetLetestNewsletterByChurch(churchId);

                    profileModel.RecordingList = recordDataAccess.Recording_GetByChurch(churchId);
                    profileModel.ScheduleList = scheduleDataAccess.GetSearchSchedule(churchId, DateTime.Now, DateTime.Now.ToString("dddd"), -1).ToList<Schedule>();

                    profileModel.NowScheduleList = Schedules_WhatsOnNow();

                    profileModel.ScheduleListDay0 = scheduleDataAccess.GetSearchSchedule(churchId, System.DateTime.Now, System.DateTime.Now.ToString("dddd"), -1);
                    profileModel.ScheduleListDay1 = scheduleDataAccess.GetSearchSchedule(churchId, System.DateTime.Now.AddDays(1), System.DateTime.Now.AddDays(1).ToString("dddd"), -1);
                    profileModel.ScheduleListDay2 = scheduleDataAccess.GetSearchSchedule(churchId, System.DateTime.Now.AddDays(2), System.DateTime.Now.AddDays(2).ToString("dddd"), -1);
                    profileModel.ScheduleListDay3 = scheduleDataAccess.GetSearchSchedule(churchId, System.DateTime.Now.AddDays(3), System.DateTime.Now.AddDays(3).ToString("dddd"), -1);
                    profileModel.ScheduleListDay4 = scheduleDataAccess.GetSearchSchedule(churchId, System.DateTime.Now.AddDays(4), System.DateTime.Now.AddDays(4).ToString("dddd"), -1);
                    profileModel.ScheduleListDay5 = scheduleDataAccess.GetSearchSchedule(churchId, System.DateTime.Now.AddDays(5), System.DateTime.Now.AddDays(5).ToString("dddd"), -1);
                    profileModel.ScheduleListDay6 = scheduleDataAccess.GetSearchSchedule(churchId, System.DateTime.Now.AddDays(6), System.DateTime.Now.AddDays(6).ToString("dddd"), -1);
                    return View(profileModel);
                }
                else
                {
                    HttpContext.Session.SetInt32("chrId", profileModel.Churches.ChurchId);
                    ViewBag.ChurchId = profileModel.Churches.ChurchId;
                    churchPass = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchPass"));
                    return RedirectToAction("Packages", "Subscription");
                }
            }

            catch (Exception e)
            {
                ShowMesage("Profile Errors : " + e.Message);
                throw;
            }


        }

        private void ShowMesage(string exceptionMessage)
        {
            log.Error("Exception : " + exceptionMessage);
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
                //var ipAddress = HttpContext.Connection.RemoteIpAddress; // IPAddress.Parse("119.159.146.215"); //
                //var ida = Request.HttpContext.Connection.RemoteIpAddress;
                IPAddress ipAddress;
                var headers = Request.Headers.ToList();
                //Common.SaveToXXX("Headers test");
                //foreach (var item in headers)
                //{
                //    Common.SaveToXXX(item.Key.ToString() + "--" + item.Value.ToString());
                //}
                 
                //Common.SaveToXXX("IP Address" + Request.HttpContext.Connection.RemoteIpAddress.ToString());
                if (headers.Exists((kvp) => kvp.Key == "X-Forwarded-For"))
                {
                    // when running behind a load balancer you can expect this header
                    var header = headers.First((kvp) => kvp.Key == "X-Forwarded-For").Value.ToString();
                    ipAddress = IPAddress.Parse(header);
                }
                else
                if(headers.Exists((kvp) => kvp.Key == "REMOTE_ADDR"))
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
                return true;

            // compare with local address
            if (remoteAddress == connection.LocalIpAddress.ToString())
                return true;

            return false;
        }

        public IActionResult SubscriberLogout()
        {
            foreach (var cookieKey in Request.Cookies.Keys)
            {
                if(cookieKey== "SubscriberId" || cookieKey == "SubscriberName")
                {
                    Response.Cookies.Delete(cookieKey);
                }
                
            }
            return RedirectToAction(nameof(Home));
        }
       
    }
}
