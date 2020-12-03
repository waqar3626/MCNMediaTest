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

        public IActionResult Profile(string id)
        {
            Church church = _churchDataAccessLayer.GetChurchDataBySlug(id);
            HttpContext.Session.SetInt32("chrId", church.ChurchId);

            int id1 = church.ChurchId;
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
            profileModel.Churches = churchDataAccess.GetChurchData(Convert.ToInt32(id1));
            List<Announcement> announcementList = announcementDataAccessLayer.GetAnnouncement(id1).ToList();
            if (announcementList.Count > 0)
                profileModel.Announcement = announcementList.First<Announcement>();
            else
                profileModel.Announcement = new Announcement();

            profileModel.NoticeList = noticeDataAccess.GetAllNotices(id1).ToList();

            profileModel.CameraList = camDataAccess.GetAllCameras(id1, "");
            profileModel.VideoList = mediaChurchDataAccess.GetByMediaType("Video", id1).ToList();
            profileModel.SlideshowList = mediaChurchDataAccess.GetByMediaType("SlideShow", id1).ToList();
            profileModel.PictureList = mediaChurchDataAccess.GetByMediaType("Picture", id1).ToList();
            profileModel.newsletter = churchNewsLetterDataAccess.GetLetestNewsletterByChurch(id1);

            //profileModel.Cameras = camDataAccess.GetCameraById(1,"");
            //  profileModel.Media = "";
            profileModel.RecordingList = recordDataAccess.Recording_GetByChurch(id1);
            profileModel.ScheduleList = scheduleDataAccess.GetSearchSchedule(id1, DateTime.Now, DateTime.Now.ToString("dddd"), -1).ToList<Schedule>();

            profileModel.NowScheduleList = Schedules_WhatsOnNow();

            profileModel.ScheduleListDay0 = scheduleDataAccess.GetSearchSchedule(id1, System.DateTime.Now, System.DateTime.Now.ToString("dddd"), -1);
            profileModel.ScheduleListDay1 = scheduleDataAccess.GetSearchSchedule(id1, System.DateTime.Now.AddDays(1), System.DateTime.Now.AddDays(1).ToString("dddd"), -1);
            profileModel.ScheduleListDay2 = scheduleDataAccess.GetSearchSchedule(id1, System.DateTime.Now.AddDays(2), System.DateTime.Now.AddDays(2).ToString("dddd"), -1);
            profileModel.ScheduleListDay3 = scheduleDataAccess.GetSearchSchedule(id1, System.DateTime.Now.AddDays(3), System.DateTime.Now.AddDays(3).ToString("dddd"), -1);
            profileModel.ScheduleListDay4 = scheduleDataAccess.GetSearchSchedule(id1, System.DateTime.Now.AddDays(4), System.DateTime.Now.AddDays(4).ToString("dddd"), -1);
            profileModel.ScheduleListDay5 = scheduleDataAccess.GetSearchSchedule(id1, System.DateTime.Now.AddDays(5), System.DateTime.Now.AddDays(5).ToString("dddd"), -1);
            profileModel.ScheduleListDay6 = scheduleDataAccess.GetSearchSchedule(id1, System.DateTime.Now.AddDays(6), System.DateTime.Now.AddDays(6).ToString("dddd"), -1);

            return View(profileModel);
           

        }

        private void ShowMesage(String exceptionMessage)
        {
            log.Error("Exception : " + exceptionMessage);
        }
    }
}
