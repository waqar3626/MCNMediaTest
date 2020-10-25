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
        CameraDataAccessLayer camDataAccess = new CameraDataAccessLayer();

       


        [HttpGet]
        public IActionResult AddSchedule()
        {

            return View();
        }


        [HttpPost()]
        public IActionResult AddSchedule(Schedule sch)
        {
            if (sch.IsRepeated == false)
            {
                sch.EventDay = sch.EventDate.ToString("dddd");
               

            }
            else 
            {
                sch.EventDate = Convert.ToDateTime("0001-01-01 00:00:00");
            }


           
            scheduleDataAccess.AddSchedule(sch);
            return RedirectToAction("ListSchedule");
        }

        [HttpGet]
        public JsonResult ListSch()
        {
           
            LoadChurchDDL();
            List<Schedule> Sch = scheduleDataAccess.GetAllSchedule().ToList<Schedule>();
            return Json   (Sch);

        }

        [HttpGet]
        public ViewResult ListSchedule()
        {
            GenericModel gm = new GenericModel();

            LoadChurchDDL();
            gm.LSchedules = scheduleDataAccess.GetAllSchedule().ToList<Schedule>();
            return View(gm);

        }

        [HttpPost]
        public IActionResult Edit(int id, [Bind] Schedule schedule)
        {
            if (schedule.IsRepeated == false)
            {
                schedule.EventDay = schedule.EventDate.ToString("dddd");
               

            }
            else if (schedule.IsRepeated == true)
            {
                schedule.EventDate = Convert.ToDateTime("0001-01-01 00:00:00");
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
            GenericModel gm = new GenericModel();
             gm.Schedules = scheduleDataAccess.GetScheduleDataBtId(id);

            if (gm.Schedules == null)
            {
                return NotFound();
            }

            return View(gm);
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

            GenericModel gm = new GenericModel();         
          
            List<Schedule> sch = scheduleDataAccess.GetSearchSchedule(Church, EventDate, EventDay,eventBy).ToList<Schedule>();
            gm.LSchedules = sch;
            LoadChurchDDL();
            return View("/Views/Schedule/ListSchedule.cshtml",gm);

        }

        public JsonResult LoadCameraDDL(int ChurchId)
        {
            List<Camera> countyList = camDataAccess.GetAllCameras(ChurchId).ToList();

            return Json(countyList);

        }

    }



}