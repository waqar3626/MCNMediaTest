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
        scheduleDataAccessLayer _scheduleDataAccessLayer = new scheduleDataAccessLayer();
        ChurchDataAccessLayer _churchDataAccessLayer = new ChurchDataAccessLayer();
        public IActionResult Home()
        {
            LoadCountryDDL();

            HttpContext.Session.SetString("UserType", "website");
            List<Church> churches = _churchDataAccessLayer.GetWebsiteChurch().ToList<Church>();
            return View(churches);
        }
       
        public IActionResult Schedules()
        {
            List<Schedule> schedules = _scheduleDataAccessLayer.GetWebsiteSchedule().ToList<Schedule>() ;
            return View(schedules);
        }
        
        public IActionResult ContactUs()
        {
            return View();
        }
        public IActionResult Churches()
        {
            List<Church> churches = _churchDataAccessLayer.GetByClientTypeChurch(-1).ToList();

            return View(churches);
        }
        public IActionResult Cathedrals()
        {
            LoadCountryDDL();
            List<Church> churches = _churchDataAccessLayer.GetByClientTypeChurch(2).ToList();
            return View(churches);
        }
        public IActionResult FuneralHomes()
        {
            List<Church> churches = _churchDataAccessLayer.GetByClientTypeChurch(3).ToList();
            return View(churches);
        }

        public JsonResult LoadCountyDDL(int CountryID)
        {
            List<Counties> countyList = _churchDataAccessLayer.GetCounties(CountryID).ToList();
           
            return Json( countyList);

        }

        public void LoadCountryDDL()
        {
            IEnumerable<Church> countyList = _churchDataAccessLayer.GetCountries();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (var item in countyList)
            {
                selectListItems.Add(new SelectListItem { Text = item.CountryName.ToString(), Value = item.CountryId.ToString() });
            }
            ViewBag.Countries = selectListItems;

        }
       
        public IActionResult ProcessForm()
        {
            LoadCountryDDL();
            return View("Home");
        }
    }

}
