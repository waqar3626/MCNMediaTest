using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static MCNMedia_Dev.Models.Church;
using Microsoft.AspNetCore.Http;

namespace MCNMedia_Dev.Controllers
{
    public class ChurchController : Controller
    {

        ChurchDataAccessLayer churchDataAccess = new ChurchDataAccessLayer("a");
        CameraDataAccessLayer camDataAccess = new CameraDataAccessLayer("a");

        [HttpGet]
        public IActionResult AddChurch()
        {
            LoadClientDDL();
            LoadCountyDDL();
            return View();
        }

        [HttpGet]
        public IActionResult Listchurch()
        {
            List<Church> church = churchDataAccess.GetAllChurch().ToList<Church>();
            return View(church);

        }
        [HttpGet()]
        public IActionResult GetAllChurch()
        {

            churchDataAccess.GetAllChurch();
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Church church = churchDataAccess.GetChurchData(id);
            if (church == null)
            {
                return NotFound();
            }

            LoadClientDDL();
            LoadCountyDDL();
            return View(church);
        }

        [HttpPost()]
        public IActionResult AddChurch(Church church)
        {
            churchDataAccess.AddChurch(church);
            return RedirectToAction("Listchurch");
        }

        [HttpPost]
        public IActionResult Edit(int id, [Bind] Church church)
        {
            if (id != church.ChurchId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                churchDataAccess.UpdateChurch(church);
                return RedirectToAction("Listchurch");
            }
            return View(church);
        }

        public IActionResult Delete(int id)
        {
            churchDataAccess.DeleteChurch(id);
            return RedirectToAction("Listchurch");
        }


        [HttpGet]
        public IActionResult ChurchDetails()
        {
            GenericModel gm = new GenericModel();
            if (!string.IsNullOrEmpty(HttpContext.Request.Query["chId"].ToString()))
            {
                int churchId = Convert.ToInt32(HttpContext.Request.Query["chId"].ToString());

                HttpContext.Session.SetInt32("ChurchId", churchId);

                Church church = churchDataAccess.GetChurchData(churchId);

                gm.Churches = church;
                if (church == null)
                {
                    return NotFound();
                }
                gm.LCameras = camDataAccess.GetAllCameras();
                return View(gm);
            }
            return RedirectToAction("Listchurch", "Church");
        }

        public void LoadClientDDL()
        {
            IEnumerable<ClientType> clientList = churchDataAccess.GetClients();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (var item in clientList)
            {
                selectListItems.Add(new SelectListItem { Text = item.ClientTypeTitle.ToString(), Value = item.ClientTypeId.ToString() });
            }
            ViewBag.ClientTypes = selectListItems;
        }

            public void LoadCountyDDL()
        {
            IEnumerable<Counties> countyList = churchDataAccess.GetCounties();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (var item in countyList)
            {
                selectListItems.Add(new SelectListItem { Text = item.CountyName.ToString(), Value = item.CountyId.ToString() });
            }
            ViewBag.Counties = selectListItems;

        }


        [HttpPost]

        public IActionResult UpdateChurch(int ChurchId, [Bind] GenericModel church)
        {
            //if (ChurchId != church.Churches.ChurchId)
            //{
            //    return NotFound();
            //}
            ChurchId = church.Churches.ChurchId;

            if (ModelState.IsValid)
            {
                churchDataAccess.UpdateChurch(church.Churches);

                var queryString = new { chId = ChurchId };
                return RedirectToAction("ChurchDetails", "Church", queryString);
            }
            return View(church);
        }
    }
}
