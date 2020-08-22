using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MCNMedia_Dev.Controllers
{
    public class ScheduleController : Controller
    {
        scheduleDataAccessLayer scheduleDataAccess = new scheduleDataAccessLayer("a");


        [HttpGet]
        public IActionResult AddSchedule()
        {
            LoadDDLChurch();
            return View();
        }

        [HttpGet]
        public IActionResult AdddSchedule()
        {
            LoadDDLChurch();
            return View();
        }


        [HttpPost()]
        public IActionResult AddSchedule(Schedule sch)
        {
            LoadDDLChurch();
            scheduleDataAccess.AddSchedule(sch);
            return RedirectToAction("ListSchedule");
        }

        [HttpGet]
        public IActionResult ListSchedule()
        {
            List<Schedule> sch = scheduleDataAccess.GetAllSchedule().ToList<Schedule>();
            return View(sch);

        }

        [HttpPost]
        public IActionResult Edit(int id, [Bind] Schedule schedule)
        {
            //if (ModelState.IsValid)
            //{
            scheduleDataAccess.UpdateSchedule(schedule);
            return RedirectToAction("ListSchedule");
            //}
            //return View();
        }


        [HttpGet]
        public IActionResult EditSchedule(int id)
        {
            Schedule schedule = scheduleDataAccess.GetScheduleDataBtId(id);
            if (schedule == null)
            {
                return NotFound();
            }
            LoadDDLChurch();
            return View(schedule);
        }


        [HttpPost]
        public IActionResult UpdateSchedule(int id, [Bind] Schedule schedule)
        {

            scheduleDataAccess.UpdateSchedule(schedule);
            return RedirectToAction("ListUser");
          
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            scheduleDataAccess.DeleteSchedule(id);
            return RedirectToAction("ListSchedule");
        }

        public void LoadDDLChurch()
        {
            IEnumerable<DDLChurch> churches = scheduleDataAccess.GetDDLChurch();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (var item in churches)
            {
                selectListItems.Add(new SelectListItem { Text = item.ChurchName.ToString(), Value = item.ChurchId.ToString() });
            }
            ViewBag.State = selectListItems;

        }
    }
}
