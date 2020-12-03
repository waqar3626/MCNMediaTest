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
            return RedirectToAction("Packages", "Subscription");

        }

        private void ShowMesage(String exceptionMessage)
        {
            log.Error("Exception : " + exceptionMessage);
        }
    }
}
