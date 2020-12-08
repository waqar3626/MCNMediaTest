using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static MCNMedia_Dev.Models.Church;

namespace MCNMedia_Dev.Controllers
{
    public class WebsiteController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ScheduleDataAccessLayer _scheduleDataAccessLayer = new ScheduleDataAccessLayer();
        ChurchDataAccessLayer _churchDataAccessLayer = new ChurchDataAccessLayer();
        WebsiteDataAccessLayer _websiteDataAccessLayer = new WebsiteDataAccessLayer();
        PlaceAccessLayer _placeAccessLayer = new PlaceAccessLayer();

        GenericModel gm = new GenericModel();

        public IActionResult Home()
        {
            try
            {
                LoadCountryDDL();
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
                }

                if (!string.IsNullOrEmpty(HttpContext.Request.Query["County"].ToString()))
                {
                    string countyName = Request.Query["County"].ToString().Replace("-", " ");
                    churches = churches.FindAll(x => x.CountyName.ToLower() == countyName.ToLower()).ToList<Church>();
                }

                if (!string.IsNullOrEmpty(HttpContext.Request.Query["Search"].ToString()))
                {
                    string searchFilter = Request.Query["Search"].ToString().ToLower();
                    churches = churches.FindAll(x => x.ChurchName.ToLower().Contains(searchFilter) ||
                    x.Town.ToLower().Contains(searchFilter) ||
                    x.CountryName.ToLower().Contains(searchFilter) ||
                    x.CountyName.ToLower().Contains(searchFilter)).ToList<Church>();
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
        public  IActionResult RecordingLock(RecordingLock recordingLock)
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
            if (id==0)
            {
                id = Convert.ToInt32(HttpContext.Session.GetInt32("RecordingId"));
                recordingPass = Convert.ToInt32(HttpContext.Session.GetInt32("RecordingPass"));
            }
            Recording recording= recordingDataAccessLayer.Recording_GetById(id);
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
            String pass = HttpContext.Session.GetString("ChurchPass").ToString();
            if (churchLock.Password == pass)
            {
                HttpContext.Session.SetInt32("ChurchPass", 1);
                return RedirectToAction(nameof(Profile));
            }
            else
            {
                ViewBag.IsSuccess = 3;
                return View();
            }


        }

        public IActionResult Profile(string id)
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
            if (profileModel.Churches.Password.Count() > 0)
            {
                HttpContext.Session.SetString("ChurchPass", profileModel.Churches.Password);
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

            int churchId = profileModel.Churches.ChurchId;// profileModel.Churches = churchDataAccess.GetChurchData(Convert.ToInt32( churchId));
            List<Announcement> announcementList = announcementDataAccessLayer.GetAnnouncement(churchId).ToList();
            if (announcementList.Count > 0)
                profileModel.Announcement = announcementList.First<Announcement>();
            else
                profileModel.Announcement = new Announcement();

            profileModel.NoticeList = noticeDataAccess.GetAllNotices(churchId).ToList();

            profileModel.CameraList = camDataAccess.GetAllCameras(churchId, "");
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

        private void ShowMesage(String exceptionMessage)
        {
            log.Error("Exception : " + exceptionMessage);
        }
    }
}
