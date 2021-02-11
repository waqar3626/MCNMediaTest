﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MCNMedia_Dev._Helper;
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

        ScheduleDataAccessLayer scheduleDataAccess = new ScheduleDataAccessLayer();
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
                ShowMessage("Schedule View Errors : " + e.Message);
                throw;
            }
        }
        [HttpPost]
        public IActionResult AddScheduleNew(int churchId,string eventName, bool isRepeated, DateTime eventDate, string eventDay, string eventTime, bool isRecording, int cameraId, int recordDuration, bool isPassword, string password)
        {

            try
            {


                Schedule sch = new Schedule();
                sch.IsRepeated = isRepeated;
                sch.EventTime = Convert.ToDateTime(eventTime);
                sch.ChurchId = churchId;
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
                sch.Record = isRecording;

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
                if (isPassword == false)
                {
                    sch.Password = string.Empty;
                }
                else
                {
                    sch.Password = password;
                }
                sch.CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    
                    int res = scheduleDataAccess.AddSchedule(sch);
                    return Json(new { success = true, responseText = "The attached file is not supported." });

                }


                return RedirectToAction("Listchurch", "Church");




            }
            catch (Exception e)
            {
                return Json(new { success = false, responseText = e.Message });
            }
        }


       
        [HttpGet]
        public ViewResult ListSchedule()
        {
            try
            {
                int churchId = -1;
                int record = -1;
                DateTime eventDate = DateTime.Now;
                string eventDay = DateTime.Now.ToString("dddd");
                ViewBag.SchDate = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewBag.SchChurchId = churchId;
                ViewBag.Schrecord = record;


                LoadChurchDDL();
                GenericModel gm = new GenericModel();

                gm.LSchedules = SearchSchedules(churchId, eventDay, eventDate, record);
                return View(gm);
            }
            catch (Exception e)
            {
                ShowMessage("List Schedule Errors : " + e.Message);
                throw;
            }
        }

        [HttpPost]
        public IActionResult Edit(int id, bool ToggleRecord1, [Bind] Schedule schedule)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return RedirectToAction("UserLogin", "UserLogin");
                }

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
                scheduleDataAccess.UpdateSchedule(schedule);
                return RedirectToAction("ListSchedule");
            }
            catch (Exception e)
            {
                ShowMessage("Get edit Schedule Data Errors : " + e.Message);
                throw;
            }
        }


        [HttpGet]
        public IActionResult EditSchedule(int id)
        {
            try
            {
                Schedule Schedules = scheduleDataAccess.GetScheduleById(id);
                if (Schedules == null)
                {
                    return NotFound();
                }

                return PartialView("EditSchedule",Schedules);
            }
            catch (Exception e)
            {
                ShowMessage("Edit Schedule Errors : " + e.Message);
                throw;
            }
        }


        [HttpPost]
        public IActionResult UpdateSchedule(string eventName, bool isRepeat, DateTime eventDate, string eventDay, string eventTime, bool recordtoggle, int cameraId, int recordDuration, bool passwordtoggle, string password, int scheduleId,int churchId)
        {

            try
            {


                Schedule sch = new Schedule();
                sch.IsRepeated = isRepeat;
                sch.ScheduleId = scheduleId;
                sch.ChurchId = churchId;
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

                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    sch.UpdatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));


                    int res = scheduleDataAccess.UpdateSchedule(sch);
                    return Json(new { success = true, responseText = "The attached file is not supported." });
                }

                return RedirectToAction("Listchurch", "Church");




            }
            catch (Exception e)
            {
                return Json(new { success = false, responseText = e.Message });
            }
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return Json(new { Url = "UserLogin" });
                }
                int UpdatedBy = (int)HttpContext.Session.GetInt32("UserId");
                scheduleDataAccess.DeleteSchedule(id, UpdatedBy);
                return RedirectToAction("ListSchedule");
            }
            catch (Exception e)
            {
                ShowMessage("Delete Schedule Errors :" + e.Message);
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
                chr.Town = "";
                chr.CountryId = -1;

                IEnumerable<Church> ChurchList = chdataAccess.GetAllChurch(chr);

                List<SelectListItem> selectListItems = new List<SelectListItem>();
                foreach (var item in ChurchList)
                {
                    string ChurchNameTown = item.ChurchName + " , " + item.Town;
                    selectListItems.Add(new SelectListItem { Text = ChurchNameTown, Value = item.ChurchId.ToString() });
                }
                ViewBag.Church = selectListItems;
            }
            catch (Exception e)
            {
                ShowMessage("LoadChurch DropDown Errors : " + e.Message);
                throw;
            }
        }

        public JsonResult LoadCameraDDL(int ChurchId)
        {
            try
            {
                List<Camera> countyList = camDataAccess.GetAllCamerasByChurch(ChurchId).ToList();
                return Json(countyList);
            }
            catch (Exception e)
            {
                ShowMessage("Load Camera DropDown  Errors : " + e.Message);
                throw;
            }
        }

        /// <summary>
        /// This method is used to get <b>Schedule List</b> basis on provided parameters.
        /// </summary>
        /// <param name="churchId">Church Id</param>
        /// <param name="eventDay">Event Scheduled Day</param>
        /// <param name="eventDate">Event Scheduled Date</param>
        /// <param name="record">Event marked for recording</param>
        /// <returns></returns>
        public IActionResult Search(int churchId, DateTime eventDate, int recordDt)
        {
            try
            {
                LoadChurchDDL();
                string eventDay = eventDate.ToString("dddd");
                GenericModel gm = new GenericModel();
                gm.LSchedules = SearchSchedules(churchId, eventDay, eventDate, recordDt);
                return View("/Views/Schedule/ListSchedule.cshtml", gm);
            }
            catch (Exception e)
            {
                ShowMessage("Search Church  Error" + e.Message);
                throw;
            }
        }

        private List<Schedule> SearchSchedules(int churchId, string eventDay, DateTime eventDate, int record)
        {
            ViewBag.SchDate = eventDate.ToString("dd-MMM-yyyy");
            ViewBag.SchChurchId = churchId;
            ViewBag.Schrecord = record;
            HttpContext.Session.SetInt32("ChurchId", churchId);
            GenericModel gm = new GenericModel();
            return scheduleDataAccess.GetSearchSchedule(churchId, eventDate, eventDay, record).ToList<Schedule>();
        }

        [HttpPost]
        public JsonResult StartRecordingSchedule(int scheduleId) {

            Wowza wowza = new Wowza();
            wowza.StartRecordingBySchedule(scheduleId);
            return Json(1);
        }

        [HttpPost]
        public JsonResult StopRecordingSchedule(int scheduleId)
        
        {
            Wowza wowza = new Wowza();
            wowza.StopRecordingBySchedule(scheduleId);
            return Json(1);
        }

        private void ShowMessage(string exceptionMessage)
        {
            log.Error("Exception : " + exceptionMessage);
        }
    }
}