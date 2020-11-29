using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        [HttpGet]
        public IActionResult AddRecording()
        {
            try
            {
                LoadChurchesDDL();
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
                LoadChurchesDDL();
                GenericModel gm = new GenericModel();
                gm.LRecordings = recordDataAccess.Recording_GetAll().ToList<Recording>();
                return View(gm);
            }
            catch (Exception e)
            {
                ShowMessage("List Recording Error" + e.Message);
                throw;
            }
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
                LoadChurchesDDL();

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
                recording.UpdatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                recordDataAccess.UpdateRecording(recording);
                return RedirectToAction("ListRecording");

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
                ActivityLogDataAccessLayer.AddActivityLog("Recording Published Request", category: "Schedule", message: responseReader.Result, churchId: -1, userId: -1);
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
                    ActivityLogDataAccessLayer.AddActivityLog("Recording Published", category: "Schedule", message: logMessage, churchId: record.ChurchId, userId: -1);
                    
                    scheduleDataAccess.UpdateScheduleLog(record.ScheduleId, 3);

                }
            }
        }

        public void LoadChurchesDDL()
        {
            try
            {
                ChurchDataAccessLayer churchDataAccessLayer = new ChurchDataAccessLayer();
                System.Data.DataTable churches = churchDataAccessLayer.GetChurchDDL();
                List<SelectListItem> selectListItems = new List<SelectListItem>();
                foreach (System.Data.DataRow item in churches.Rows)
                {
                    selectListItems.Add(new SelectListItem { Text = item["ChurchName"].ToString(), Value = item["ChurchId"].ToString() });
                }
                ViewBag.State = selectListItems;
            }
            catch (Exception e)
            {
                ShowMessage("Load Church Error in Recording " + e.Message);
                throw;
            }
        }

        private void ShowMessage(string exceptionMessage)
        {
            log.Info("Exception: " + exceptionMessage);
        }
    }
}
