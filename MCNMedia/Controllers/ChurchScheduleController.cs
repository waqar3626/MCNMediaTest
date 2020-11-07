using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MCNMedia_Dev.Controllers
{
    public class ChurchScheduleController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        ScheduleDataAccessLayer SchDataAccess = new ScheduleDataAccessLayer();
        GenericModel gm = new GenericModel();
        public JsonResult ListSchedule()
        {
            try
            {
                int churchId = (int)HttpContext.Session.GetInt32("ChurchId");
                List<Schedule> ChurchSchedule = SchDataAccess.GetAllChurchSchedule(churchId).ToList();
                return Json(ChurchSchedule);
            }
            catch (Exception e)
            {
                ShowMessage("Add Church Schedule Errors : " + e.Message);
                throw;
               
            }

        }

        [HttpPost]
        public IActionResult AddChurchSchedule(string eventName, bool isRepeat,DateTime eventDate,string eventDay,string eventTime,bool recordtoggle,int cameraId,int recordDuration,bool passwordtoggle,string password)
        {
          
            try
            {
               

                Schedule sch = new Schedule();
                 sch.IsRepeated = isRepeat;
                sch.EventTime = Convert.ToDateTime(eventTime);
                if (sch.IsRepeated == false)
                {
                    sch.EventDate = eventDate;
                    sch.EventDay = sch.EventDate.ToString("dddd");
                }
                else
                {
                    sch.EventDay = eventDay;
                    sch.EventDate = Convert.ToDateTime("1900-01-01 00:00:00");
                }
                sch.EventName = eventName;
                sch.Record = recordtoggle;
            
                if (sch.Record == false)
                {
                    sch.CameraId = 0;
                    sch.RecordDuration = 0;
                }
                else {
                    sch.CameraId = cameraId;
                    sch.RecordDuration = recordDuration;
                }
                if (passwordtoggle == false)
                {
                    sch.Password = string.Empty;
                }
                else {
                    sch.Password = password;
                }
                sch.CreatedBy= Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
                {
                    sch.ChurchId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));
                 int   res = SchDataAccess.AddSchedule(sch);
                    return Json(new { success = true, responseText = "The attached file is not supported." });
                    
                }
                

                    return RedirectToAction("Listchurch", "Church");

                
               
                
            }
            catch (Exception e)
            {
                ShowMessage("Add Schedule Errors : " + e.Message);
                throw;
            }
        }

        [HttpGet]
        public IActionResult EditSchedule(int id)
        {
            try
            {
                Schedule Schedules = SchDataAccess.GetScheduleDataBtId(id);
                if (Schedules == null)
                {
                    return NotFound();
                }

                return PartialView("_EditSchedule",Schedules);
            }
            catch (Exception e)
            {
                ShowMessage("Edit Schedule Errors : " + e.Message);
                throw;
            }
        }


        [HttpPost]
        public IActionResult EditSchedule(int id, bool ToggleRecord1, [Bind] Schedule schedule)
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
                schedule.ScheduleId = id;
                schedule.Record = ToggleRecord1;
                schedule.UpdatedBy = (int)HttpContext.Session.GetInt32("UserId");
                SchDataAccess.UpdateSchedule(schedule);
                return RedirectToAction("ListSchedule");
            }
            catch (Exception e)
            {
                ShowMessage("Get edit Schedule Data Errors : " + e.Message);
                throw;
            }
        }


        


        [HttpPost]
        public IActionResult UpdateSchedule(int id, [Bind] Schedule schedule)
        {
            try
            {
                schedule.UpdatedBy = (int)HttpContext.Session.GetInt32("UserId");
                SchDataAccess.UpdateSchedule(schedule);
                return RedirectToAction("ListUser");

            }
            catch (Exception e)
            {
                ShowMessage("Update Schedule Errors : " + e.Message);
                throw;
            }
        }

       
        public IActionResult DeleteSchedule(int id)
        {
            try
            {

                GenericModel gm = new GenericModel();
                int UpdatedBy = (int)HttpContext.Session.GetInt32("UserId");
                bool res = SchDataAccess.DeleteSchedule(id, UpdatedBy);
                return Json(res);
            }
            catch (Exception e)
            {
                ShowMessage("Delete Notice Errors :" + e.Message);
                throw;
            }
        }


        private void ShowMessage(String exceptionMessage)
        {
            log.Error("Exception : " + exceptionMessage);
        }
    }
}