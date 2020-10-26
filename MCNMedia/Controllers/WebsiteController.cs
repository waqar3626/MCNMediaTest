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
        scheduleDataAccessLayer _scheduleDataAccessLayer = new scheduleDataAccessLayer();
        ChurchDataAccessLayer _churchDataAccessLayer = new ChurchDataAccessLayer();
        public IActionResult Home()
        {
            try
            {

                LoadCountryDDL();
                HttpContext.Session.SetString("UserType", "website");
                List<Church> churches = _churchDataAccessLayer.GetWebsiteChurch().ToList<Church>();
                return View(churches);
            }
            catch (Exception e)
            {
                ShowMesage("Home : "+e.Message);
                throw;
            }
        }

        public IActionResult Schedules()
        {
            try
            {

                List<Schedule> schedules = _scheduleDataAccessLayer.GetWebsiteSchedule().ToList<Schedule>();
                return View(schedules);
            }
            catch (Exception e)
            {
                ShowMesage("Schedules Error : " +e.Message);
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

                List<Church> churches = _churchDataAccessLayer.GetByClientTypeChurch(-1).ToList();
                return View(churches);
            }
            catch (Exception e)
            {
                ShowMesage("Churches : "+e.Message);
                throw;
            }
        }
        public IActionResult Cathedrals()
        {
            try
            {

                LoadCountryDDL();
                List<Church> churches = _churchDataAccessLayer.GetByClientTypeChurch(2).ToList();
                return View(churches);
            }
            catch (Exception e)
            {
                ShowMesage("Cathedrals"+e.Message);
                throw;
            }
        }
        public IActionResult FuneralHomes()
        {
            try
            {
                List<Church> churches = _churchDataAccessLayer.GetByClientTypeChurch(3).ToList();
                return View(churches);
            }
            catch (Exception e)
            {
                ShowMesage("FuneralHomes : "+e.Message);
                throw;
            }
        }

        public JsonResult LoadCountyDDL(int CountryID)
        {
            try
            {

                List<Counties> countyList = _churchDataAccessLayer.GetCounties(CountryID).ToList();

                return Json(countyList);
            }
            catch (Exception e)
            {
                ShowMesage("Load County DropDown : "+e.Message);
                throw;
            }

        }

        public void LoadCountryDDL()
        {
            try
            {

                IEnumerable<Church> countyList = _churchDataAccessLayer.GetCountries();
                List<SelectListItem> selectListItems = new List<SelectListItem>();
                foreach (var item in countyList)
                {
                    selectListItems.Add(new SelectListItem { Text = item.CountryName.ToString(), Value = item.CountryId.ToString() });
                }
                ViewBag.Countries = selectListItems;

            }
            catch (Exception e)
            {
                ShowMesage("Load Country DropDown : "+e.Message);
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

        private void ShowMesage(String exceptionMessage)
        {
            log.Error("Exception : " + exceptionMessage);
        }
    }
}
