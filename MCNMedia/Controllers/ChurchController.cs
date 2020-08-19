using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MCNMedia_Dev.Controllers
{
    public class ChurchController : Controller
    {

        ChurchDataAccessLayer churchDataAccess = new ChurchDataAccessLayer("a");

        [HttpGet]
        public IActionResult AddChurch()
        {
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
            return View(church);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {

            if (id == null)
            {
                return NotFound();
            }
            Church church = churchDataAccess.GetChurchData(id);
            if (church == null)
            {
                return NotFound();
            }
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
        [HttpPost, ActionName("Delete")]

        public IActionResult DeleteConfirmed(int id)
        {
            churchDataAccess.DeleteChurch(id);
            return RedirectToAction("Listchurch");
        }


        [HttpGet]
        public IActionResult ChurchDetails()
        {
            int churchId = Convert.ToInt32(HttpContext.Request.Query["chId"].ToString());
            Church church = churchDataAccess.GetChurchData(churchId);
            if (church == null)
            {
                return NotFound();
            }
            return View(church);
        }

        
        [HttpPost]

        public IActionResult UpdateChurch(int ChurchId, [Bind] Church church)
        {
            if (ChurchId != church.ChurchId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                churchDataAccess.UpdateChurch(church);
             
                var queryString = new { chId = ChurchId };
                return RedirectToAction("ChurchDetails","Church",queryString);
            }
            return View(church);
        }

    }
}
