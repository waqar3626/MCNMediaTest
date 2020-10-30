using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MCNMedia_Dev.Controllers
{
    public class ScheduleController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        scheduleDataAccessLayer scheduleDataAccess = new scheduleDataAccessLayer();
        ChurchDataAccessLayer chdataAccess = new ChurchDataAccessLayer();
        CameraDataAccessLayer camDataAccess = new CameraDataAccessLayer();

       
 

        [HttpGet]
        public IActionResult AddSchedule()
        {
            try
            {
                return View();

            }
            catch (Exception e)
            {

                ShowMesage("Schedule View Errors : " + e.Message);
                throw;
            }

        }


        [HttpPost()]
        public IActionResult AddSchedule(Schedule sch)
        {
            try
            {

                if (sch.IsRepeated == false)
                {
                    sch.EventDay = sch.EventDate.ToString("dddd");


                }
                else
                {
                    sch.EventDate = Convert.ToDateTime("0001-01-01 00:00:00");
                }


                sch.CreatedBy = (int)HttpContext.Session.GetInt32("UserId");
                scheduleDataAccess.AddSchedule(sch);
                return RedirectToAction("ListSchedule");

            }
            catch (Exception e)
            {
                ShowMesage("Add Schedule Errors : " + e.Message);
                throw;
            }}

        [HttpGet]
        public JsonResult ListSch()
        {

            try
            {
                LoadChurchDDL();
                List<Schedule> Sch = scheduleDataAccess.GetAllSchedule().ToList<Schedule>();
                return Json(Sch);
            }
            catch (Exception e)
            {
                ShowMesage("List Schedule Errors : " + e.Message);
                throw;
            }

        }

        [HttpGet]
        public ViewResult ListSchedule()
        {
            try
            {

                GenericModel gm = new GenericModel();
                LoadChurchDDL();
                gm.LSchedules = scheduleDataAccess.GetAllSchedule().ToList<Schedule>();
                return View(gm);

            }
            catch (Exception e)
            {
               
                ShowMesage("List Schedule Errors : " + e.Message);
                throw;

            }
        }

        [HttpPost]
        public IActionResult Edit(int id, [Bind] Schedule schedule)
        {
            try
            {

                if (schedule.IsRepeated == false)
                {
                    schedule.EventDay = schedule.EventDate.ToString("dddd");


                }
                else if (schedule.IsRepeated == true)
                {
                    schedule.EventDate = Convert.ToDateTime("0001-01-01 00:00:00");
                }
                schedule.UpdatedBy = (int)HttpContext.Session.GetInt32("UserId");
                scheduleDataAccess.UpdateSchedule(schedule);
                return RedirectToAction("ListSchedule");
            }
            catch (Exception e)
            {
                ShowMesage("Get edit Schedule Data Errors : " + e.Message);
                throw;
            } 
        }


        [HttpGet]
        public IActionResult EditSchedule(int id)
        {
            try
            {

                Schedule Schedules = scheduleDataAccess.GetScheduleDataBtId(id);

                if (Schedules == null)
                {
                    return NotFound();
                }

                return View(Schedules);

            }
            catch (Exception e)
            {
                ShowMesage("Edit Schedule Errors : " + e.Message);
                throw;
            }
        }


        [HttpPost]
        public IActionResult UpdateSchedule(int id, [Bind] Schedule schedule)
        {
            try
            {
                schedule.UpdatedBy = (int)HttpContext.Session.GetInt32("UserId");
                scheduleDataAccess.UpdateSchedule(schedule);
                return RedirectToAction("ListUser");

            }
            catch (Exception e)
            {
                ShowMesage("Update Schedule Errors : " + e.Message);
                throw;
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {

                scheduleDataAccess.DeleteSchedule(id);
                return RedirectToAction("ListSchedule");
            }
            catch (Exception e)
            {
                ShowMesage("Delete Schedule Errors :" + e.Message);
                throw;
            }
        }

        public void LoadChurchDDL()
        {
            try
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
            catch (Exception e)
            {
                ShowMesage("LoadChurch DropDown Errors : " + e.Message);
                throw;
            }
        }

        public IActionResult SearchSchedule(int Church, DateTime EventDate,string EventDay,int eventBy) {
            try
            {

                GenericModel gm = new GenericModel();

                List<Schedule> sch = scheduleDataAccess.GetSearchSchedule(Church, EventDate, EventDay, eventBy).ToList<Schedule>();
                gm.LSchedules = sch;
                LoadChurchDDL();
                return View("/Views/Schedule/ListSchedule.cshtml", gm);

            }
            catch (Exception e)
            {
                ShowMesage("Search Schedule Errors : " + e.Message);
                throw;
            }
        }

        public JsonResult LoadCameraDDL(int ChurchId)
        {
            try
            {

                List<Camera> countyList = camDataAccess.GetAllCameras(ChurchId).ToList();

                return Json(countyList);

            }
            catch (Exception e)
            {
                ShowMesage("Load Camera DropDown  Errors : " + e.Message);
                throw;
            }
        }

        private void ShowMesage(String exceptionMessage)
        {
            log.Error("Exception : " + exceptionMessage);
        }
    }
}