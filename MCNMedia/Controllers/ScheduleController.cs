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
        ChurchDataAccessLayer chdataAccess = new ChurchDataAccessLayer();


        [HttpGet]
        public IActionResult AddSchedule()
        {

            return View();
        }


        [HttpPost()]
        public IActionResult AddSchedule(Schedule sch,int eventBy)
        {
            if (eventBy == 1)
            {
                sch.EventDay = sch.EventDate.ToString("dddd");
                sch.IsRepeated = false;

            }
           

                scheduleDataAccess.AddSchedule(sch);
            return RedirectToAction("ListSchedule");
        }

        [HttpGet]
        public IActionResult ListSchedule()
        {
            LoadChurchDDL();
            List<Schedule> sch = scheduleDataAccess.GetAllSchedule().ToList<Schedule>();
            return View(sch);

        }

        [HttpPost]
        public IActionResult Edit(int id, [Bind] Schedule schedule)
        {
            if (schedule.EventDay == null)
            {
                schedule.EventDay = schedule.EventDate.ToString("dddd");

            }
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

        public void LoadChurchDDL()
        {
            Church chr = new Church();
            chr.ChurchId = -1;
            chr.CountyId = -1;
            chr.ClientTypeId = -1;
            chr.ChurchName = "";
            chr.EmailAddress = "";
            chr.Phone = "";

            IEnumerable<Church> ChurchList = chdataAccess.GetAllChurch(chr);

            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (var item in ChurchList)
            {
                selectListItems.Add(new SelectListItem { Text = item.ChurchName.ToString(), Value = item.ChurchId.ToString() });
            }
            ViewBag.Church = selectListItems;
        }

        public IActionResult SearchSchedule(int Church, DateTime EventDate,string EventDay,int eventBy) {
           
            
           
          
            List<Schedule> sch = scheduleDataAccess.GetSearchSchedule(Church, EventDate, EventDay,eventBy).ToList<Schedule>();
            LoadChurchDDL();
            return View("/Views/Schedule/ListSchedule.cshtml",sch);

        }

    }

    
}