using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MCNMedia_Dev.Controllers
{
    public class ScheduleController : Controller
    {
        scheduleDataAccessLayer scheduleDataAccess = new scheduleDataAccessLayer();


        [HttpGet]
        public IActionResult AddSchedule()
        {
            
            return View();
        }


        [HttpPost()]
        public IActionResult AddSchedule(Schedule sch)
        {
            
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
    }
}
