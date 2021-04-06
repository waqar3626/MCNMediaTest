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
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return Json(new { Url = "UserLogin" });
                }
                int churchId = (int)HttpContext.Session.GetInt32("ChurchId");
                List<Schedule> ChurchSchedule = SchDataAccess.GetScheduleByChurch(churchId).ToList();
                return Json(ChurchSchedule);
            }
            catch (Exception e)
            {
                return Json(new {success=false, e.Message});
            }
        }

        [HttpPost]
        public JsonResult AddChurchScheduleByJson(string eventName, bool isRepeat, DateTime eventDate, string eventDay, string eventTime, bool recordtoggle, int cameraId, int recordDuration, bool passwordtoggle, string password)
        {
            try
            {
               
                Schedule sch = new Schedule();
                sch.IsRepeated = isRepeat;
                sch.EventTime = Convert.ToDateTime(eventTime);
                if (sch.IsRepeated == false)
                {
                    if (eventDate.ToString("yyyy-MMM-dd")== "1900-Jan-01")
                    {
                        throw new Exception("Invalid Date!" + eventDate + "Kindly Provide a valid date");
                    }
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
                else
                {
                    sch.CameraId = cameraId;
                    sch.RecordDuration = recordDuration;
                }
                if (passwordtoggle == false)
                {
                    sch.Password = string.Empty;
                }
                else
                {
                    sch.Password = password;
                }
                sch.CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
                {
                  
                    sch.ChurchId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));
                    int res = SchDataAccess.AddSchedule(sch);
                     
                        return Json(new { success = true, responseText = "The attached file is not supported." });
                   
                }
                return Json("Listchurch");
            }
            
            catch (Exception e)
            {
                return Json(new { success = false, responseText = e.Message });
            }
        }

        [HttpGet]
        public IActionResult EditSchedule(int id)
        {
            try
            {
                Schedule Schedules = SchDataAccess.GetScheduleById(id);
                if (Schedules == null)
                {
                    return NotFound();
                }
                return PartialView("_EditSchedule", Schedules);
            }
            catch (Exception e)
            {
                ShowMessage("Edit Schedule Errors : " + e.Message);
                throw;
            }
        }

        [HttpPost]
        public IActionResult UpdateChurchSchedule(string eventName, bool isRepeat, DateTime eventDate, string eventDay, string eventTime, bool recordtoggle, int cameraId, int recordDuration, bool passwordtoggle, string password, int scheduleId)
        {
            try
            {
                Schedule sch = new Schedule();
                sch.IsRepeated = isRepeat;
                sch.ScheduleId = scheduleId;
                sch.EventTime = Convert.ToDateTime(eventTime);
                if (sch.IsRepeated == false)
                {
                    if (eventDate.ToString("yyyy-MMM-dd") == "1900-Jan-01")
                    {
                        throw new Exception("Invalid Date!" + eventDate + "Kindly Provide a valid date");
                    }
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
                else
                {
                    sch.CameraId = cameraId;
                    sch.RecordDuration = recordDuration;
                }
                if (passwordtoggle == false)
                {
                    sch.Password = string.Empty;
                }
                else
                {
                    sch.Password = password;
                }
                sch.UpdatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
                    {
                        sch.ChurchId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));
                        int res = SchDataAccess.UpdateSchedule(sch);
                        return Json(new { success = true, responseText = "The attached file is not supported." });
                    }
                }
                return RedirectToAction("UserLogin", "UserLogin");
            }
            catch (Exception e)
            {
                return Json(new { success = false, responseText = e.Message });
            }
        }

        [HttpPost]
        public IActionResult UpdateSchedule(int id, [Bind] Schedule schedule)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return Json(new { Url = "UserLogin" });
                }
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
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return Json(new { Url = "UserLogin" });
                }
                GenericModel gm = new GenericModel();
                int UpdatedBy = (int)HttpContext.Session.GetInt32("UserId");
                bool res = SchDataAccess.DeleteSchedule(id, UpdatedBy);
                return Json(new {success=true, res });
            }
            catch (Exception e)
            {
                return Json(new { success = false, responseText=e.Message });
               
            }
        }

        private void ShowMessage(String exceptionMessage)
        {
            log.Error("Exception : " + exceptionMessage);
        }
    }
}