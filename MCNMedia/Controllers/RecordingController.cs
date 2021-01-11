using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MCNMedia_Dev._Helper;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace MCNMedia_Dev.Controllers
{
    public class RecordingController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        RecordingDataAccessLayer recordDataAccess = new RecordingDataAccessLayer();
        ChurchDataAccessLayer chdataAccess = new ChurchDataAccessLayer();
        GenericModel gm = new GenericModel();

        [HttpGet]
        public IActionResult AddRecording()
        {
            try
            {
                LoadChurchDDL();
                return View();
            }
            catch (Exception e)
            {
                ShowMessage("Add Recording Error 'Get' " + e.Message);
                throw;
            }

        }

        [HttpGet]
        public IActionResult ListRecording()
        {
            try
            {
                DateTime FromDate = DateTime.Now.AddDays(-1);
                DateTime ToDate = DateTime.Now;
                GenericModel gm = new GenericModel();
                gm.LRecordings = recordDataAccess.RecordingSearch(FromDate, ToDate, -1, "").ToList<Recording>();
                ViewBag.FromDate = FromDate.ToString("dd-MMM-yyyy");
                ViewBag.ToDate = ToDate.ToString("dd-MMM-yyyy");
                ViewBag.ChurchId = -1;
                ViewBag.EventName = "";
                LoadChurchDDL();
                return View(gm);
            }
            catch (Exception e)
            {
                ShowMessage("List Recording Error" + e.Message);
                throw;
            }
        }

        [HttpPost]
        public IActionResult Search(string fromDate, string toDate, int ChurchId, string EventName)
        {
            try
            {
                GenericModel gm = new GenericModel();
                DateTime FromDate = Convert.ToDateTime(fromDate);
                DateTime ToDate = Convert.ToDateTime(toDate);

                gm.LRecordings = recordDataAccess.RecordingSearch(FromDate, ToDate, ChurchId, EventName).ToList<Recording>();
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
                ViewBag.ChurchId = ChurchId;
                ViewBag.EventName = EventName;
                LoadChurchDDL();
                return View("ListRecording", gm);
            }
            catch (Exception e)
            {
                ShowMessage("Search Errors : " + e.Message);
                throw;
            }
        }

      
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {

        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                Recording recording = recordDataAccess.Recording_GetById(id);
                if (recording == null)
                {
                    return NotFound();
                }
                LoadChurchDDL();

                return PartialView("Edit", recording);
            }
            catch (Exception e)
            {
                ShowMessage("Edit Recording Errors 'Get' : " + e.Message);
                throw;
            }
        }

        [HttpGet()]
        public IActionResult GetAllRecording()
        {
            try
            {
                recordDataAccess.Recording_GetAll();
                return View();
            }
            catch (Exception e)
            {
                ShowMessage("Add Recording Error 'Post' " + e.Message);
                throw;
            }

        }

        [HttpPost]
        public IActionResult AddRecording(Recording record)
        {
            try
            {
                record.CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                record.ScheduleId = -1;
                recordDataAccess.AddRecording(record);
                return RedirectToAction("ListRecording");
            }
            catch (Exception e)
            {
                ShowMessage("Add Recording Error 'Post' " + e.Message);
                throw;
            }
        }

        [HttpPost]
        public IActionResult Edit(int id, [Bind] Recording recording)
        {
            try
            {
                if (HttpContext.Session.GetString("UserType") == "admin")
                {
                    recording.UpdatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                    recordDataAccess.UpdateRecording(recording);
                    return RedirectToAction("ListRecording");
                }
                else if (HttpContext.Session.GetString("UserType") == "client")
                {
                    recording.UpdatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                    recordDataAccess.UpdateRecording(recording);
                    return RedirectToAction("Recording", "Client");
                }
                else if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return RedirectToAction("UserLogin", "UserLogin");
                }
                return Json(1);
            }
            catch (Exception e)
            {
                ShowMessage("Edit Recording Errors 'Post' : " + e.Message);
                throw;
            }

        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return RedirectToAction("UserLogin", "UserLogin");
                }
                int UpdateBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                recordDataAccess.DeleteRecording(id, UpdateBy);
                return RedirectToAction("ListRecording");
            }
            catch (Exception e)
            {
                ShowMessage("Delete Recording Error " + e.Message);
                throw;
            }

        }

        [HttpPost]
        public void PublishEvent()
        {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                Recording record = new Recording();
                ScheduleDataAccessLayer scheduleDataAccess = new ScheduleDataAccessLayer();

                var responseReader = reader.ReadToEndAsync();
                ActivityLogDataAccessLayer.AddActivityLog(Operation.Recording_Published_Request, Categories.Schedule, message: responseReader.Result, churchId: -1, userId: -1);
                log.Debug(responseReader.Result);
                var publishRecording = JsonConvert.DeserializeObject<PublishRecording>(responseReader.Result);

                DataTable dt = scheduleDataAccess.spSchedule_NotPublished_GetByCamera(Convert.ToInt32(publishRecording.camera_id));
                if (dt.Rows.Count > 0)
                {
                    record.ChurchId = Convert.ToInt32(dt.Rows[0]["ChurchId"]);
                    record.RecordingTitle = dt.Rows[0]["ScheduleEventName"].ToString();
                    record.ScheduleId = Convert.ToInt32(dt.Rows[0]["ScheduleId"].ToString());
                }
                record.RecordingURl = publishRecording.recording_url;
                record.Date = DateTime.Now;
                record.Time = DateTime.Now;
                record.RecordingTitle = string.IsNullOrEmpty(record.RecordingTitle) ? $"Recording_{DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss")}" : record.RecordingTitle;
                record.CreatedBy = -1; // Created by system so have no user id

                recordDataAccess.AddRecording(record);
                if (record.ScheduleId > 0)
                {
                    int scheduleStatus = 3;
                    if (Convert.ToBoolean(dt.Rows[0]["IsRepeated"]))
                        scheduleStatus = 0;
                    scheduleDataAccess.UpdateScheduleStatus(record.ScheduleId, scheduleStatus);
                    string logMessage = $"Recording published for camera (CameraID: {publishRecording.camera_id}) on {DateTime.Now}";
                    ActivityLogDataAccessLayer.AddActivityLog(Operation.Recording_Published, Categories.Schedule, message: logMessage, churchId: record.ChurchId, userId: -1);

                    scheduleDataAccess.UpdateScheduleLog(record.ScheduleId, 3);
                }
            }
        }


        
        public IActionResult AdminPlayer(int id)
        {
            int recordingPass = 0;
            RecordingDataAccessLayer recordingDataAccessLayer = new RecordingDataAccessLayer();
            if (id == 0)
            {
                id = Convert.ToInt32(HttpContext.Session.GetInt32("RecordingId"));
                recordingPass = Convert.ToInt32(HttpContext.Session.GetInt32("RecordingPass"));
            }
            Recording recording = recordingDataAccessLayer.Recording_GetById(id);
            int pass = recording.Password.Count();
            if (recording.Password.Count() > 0)
            {
                HttpContext.Session.SetString("RecordingPass", recording.Password);
                HttpContext.Session.SetInt32("RecordingId", id);
                if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("UserId").ToString()))
                {
                    int usertype = Convert.ToInt32(HttpContext.Session.GetInt32("UserType"));
                }
                else
                {
                    if (recordingPass == 1)
                    {

                    }
                    else
                    {
                        return RedirectToAction(nameof(RecordingLock));
                    }
                }
            }
            return View(recording);
        }

       
        public IActionResult ClientPlayer(int id)
        {
            int recordingPass = 0;
            GenericModel gm = new GenericModel();
            RecordingDataAccessLayer recordingDataAccessLayer = new RecordingDataAccessLayer();
            if (id == 0)
            {
                id = Convert.ToInt32(HttpContext.Session.GetInt32("RecordingId"));
                recordingPass = Convert.ToInt32(HttpContext.Session.GetInt32("RecordingPass"));
            }
            Recording recording = recordingDataAccessLayer.Recording_GetById(id);
            int churchId = (int)HttpContext.Session.GetInt32("ChurchId");
            gm.Churches = chdataAccess.GetChurchData(churchId);
            int pass = recording.Password.Count();
            if (recording.Password.Count() > 0)
            {
                HttpContext.Session.SetString("RecordingPass", recording.Password);
                HttpContext.Session.SetInt32("RecordingId", id);
                if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("UserId").ToString()))
                {
                    int usertype = Convert.ToInt32(HttpContext.Session.GetInt32("UserType"));
                }
                else
                {
                    if (recordingPass == 1)
                    {

                    }
                    else
                    {
                        return RedirectToAction(nameof(RecordingLock));
                    }
                }
            }
            gm.Recordings = recording;
            return View(gm);
        }

        private void ShowMessage(string exceptionMessage)
        {
            log.Info("Exception: " + exceptionMessage);
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

    }
}
