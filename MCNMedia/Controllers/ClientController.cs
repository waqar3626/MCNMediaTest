using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Org.BouncyCastle.Asn1.Crmf;

namespace MCNMedia_Dev.Controllers
{
    
    public class ClientController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        ChurchDataAccessLayer chdataAccess = new ChurchDataAccessLayer();
        AnnouncementDataAccessLayer AnnouncementDataAccessLayer = new AnnouncementDataAccessLayer();
        scheduleDataAccessLayer scheduleDataAccess = new scheduleDataAccessLayer();
        RecordingDataAccessLayer recordingDataAccess = new RecordingDataAccessLayer();
        PreviewChurchesDataAccessLayer previewChurchesDataAccess = new PreviewChurchesDataAccessLayer();
        CameraDataAccessLayer camDataAccess = new CameraDataAccessLayer();
        GenericModel gm = new GenericModel();


        #region church info
      
       

        [HttpGet]
        public IActionResult ChurchInfo(int churchId)
        {
            try
            {
                Church church = chdataAccess.GetChurchData(churchId);
                if (church == null)
                {
                    return NotFound();
                }

                return View(church);
            }
            catch (Exception e)
            {
                ShowMesage("Church Info Errors : " + e.Message);
                throw;
            }
           
        }
        
        public JsonResult GetChurchInfoById(int churchId)
        {
            Church church = chdataAccess.GetChurchData(churchId);
            HttpContext.Session.SetInt32("ChurchId", churchId);
           
            List<Church> churchInfo = new List<Church>();
            churchInfo.Add(church);
            return Json(churchInfo);
            // return Json(new { data = cameraInfo });

        }

        public IActionResult PreviewClient(int chId)
        {
            gm.Churches = previewChurchesDataAccess.GetPreviewChurch(chId);
            return View(gm);
        }

        [HttpPost]
        public IActionResult UpdateChurchInfo([Bind] Church church)
        {

            chdataAccess.UpdateChurch(church);
            return RedirectToAction("ChurchInfo");

        }
        #endregion



        #region Announcement
        [HttpGet]
        public IActionResult Announcement()
        {
            try
            {

                int id = (int)HttpContext.Session.GetInt32("ChurchId");
                GenericModel gm = new GenericModel();
                gm.LAnnouncement = AnnouncementDataAccessLayer.GetAnnouncement(id);
                gm.Churches = chdataAccess.GetChurchData(id);
                return View(gm);
            }
            catch (Exception e)
            {
                ShowMesage("Announcement Errors :" + e.Message);
                throw;
            }
        }

        public IActionResult UpdateAnnouncement([Bind] Announcement announcement)
        {
            try
            {

                AnnouncementDataAccessLayer.UpdateAnnouncement(announcement);
                return RedirectToAction("Announcement");

            }
            catch (Exception e)
            {
                ShowMesage("Update Announcement : "+e.Message);
                throw;
            }
        }
        #endregion



        #region Camera
        public IActionResult CameraDetail()

        {
            try
            {
                LoadServerDDL();
                int id = (int)HttpContext.Session.GetInt32("ChurchId");
                gm.Churches = chdataAccess.GetChurchData(id);
                return View(gm);
            }
            catch (Exception e)
            {
                ShowMesage("Camera Detail Errors : " + e.Message);
                throw;
            }
        }


       
        #endregion



        #region StreamToFaceBook
        public IActionResult StreamToFaceBook()
        {
            try
            {
                int id = (int)HttpContext.Session.GetInt32("ChurchId");
                gm.Churches = chdataAccess.GetChurchData(id);
                return View(gm);
            }
            catch (Exception e)
            {
                ShowMesage("Stream To FaceBook : "+ e.Message);
                throw;
            }
        }
        #endregion



        #region Schedule

        [HttpGet]
        public IActionResult Schedule()
        {
            try
            {

                int id = (int)HttpContext.Session.GetInt32("ChurchId");
                GenericModel gm = new GenericModel();
                gm.LSchedules = previewChurchesDataAccess.GetAllPreviewSchedule(id);
                gm.Churches = chdataAccess.GetChurchData(id);
                Redirect("Schedule");
                return View(gm);

            }
            catch (Exception e)
            {
                ShowMesage("Schedule Errors : " + e.Message);
                throw;
            }
        }
        [HttpGet]
        public IActionResult Recording()
        {
            int id = (int)HttpContext.Session.GetInt32("ChurchId");
            GenericModel gm = new GenericModel();
            gm.LRecordings = previewChurchesDataAccess.GetAllPreviewRecording(id);
            gm.Churches = chdataAccess.GetChurchData(id);
            Redirect("Recording");
            return View(gm);

        }


        [HttpGet]
        public IActionResult EditRecordingClient(int id)
        {
            try
            {
                int id1 = (int)HttpContext.Session.GetInt32("ChurchId");
                GenericModel gm = new GenericModel();

                gm.Churches = chdataAccess.GetChurchData(id1);
                gm.Recordings = recordingDataAccess.GetRecordingData(id);
                if (gm.Recordings == null)
                {
                    return NotFound();
                }
                LoadChurchesDDL();

                return View(gm);
            }
            catch (Exception e)
            {
                ShowMesage("Edit Recording Client Errors : " +e.Message);
                throw;
            }
            
        }

        
        [HttpPost]
        public IActionResult EditRecordingClient(GenericModel gm)
        {

            recordingDataAccess.UpdateRecording(gm.Recordings);
            return RedirectToAction("Recording");

        }

      
        
        [HttpGet]
        public IActionResult EditScheduleClient(int id)
        {
            int id2 = (int)HttpContext.Session.GetInt32("ChurchId");
            GenericModel gm = new GenericModel();
            gm.Churches = chdataAccess.GetChurchData(id2);
            gm.Schedules= scheduleDataAccess.GetScheduleDataBtId(id);
            if (gm.Schedules == null)
            {
                return NotFound();
            }
            LoadChurchesDDL();
            return View(gm);
        }
        #endregion



        public void LoadServerDDL()
        {
            try
            {
                IEnumerable<Server> serverList = camDataAccess.GetServer();
                List<SelectListItem> selectListItems = new List<SelectListItem>();
                foreach (var item in serverList)
                {
                    selectListItems.Add(new SelectListItem { Text = item.ServerName.ToString(), Value = item.ServerId.ToString() });
                }
                ViewBag.Server = selectListItems;
            }
            catch (Exception e)
            {
                ShowMesage("Load Server Dropdown Errors : " + e.Message );
                throw;
            }

        }

        [HttpPost]
        public IActionResult EditScheduleClient(GenericModel gm)
        {
            try
            {

                if (gm.Schedules.EventDay == null)
                {
                    gm.Schedules.EventDay = gm.Schedules.EventDate.ToString("dddd");

                }
                scheduleDataAccess.UpdateSchedule(gm.Schedules);
                return RedirectToAction("Schedule");

            }
            catch (Exception e)
            {
                ShowMesage("Edit Schedule Client Errors : " + e.Message);
                throw;
            }
        }
        public void LoadChurchesDDL()
        {
            try
            {

                IEnumerable<Recording> RecordList = recordingDataAccess.GetChurches();
                List<SelectListItem> selectListItems = new List<SelectListItem>();
                foreach (var item in RecordList)
                {
                    selectListItems.Add(new SelectListItem { Text = item.ChurchName.ToString(), Value = item.ChurchId.ToString() });
                }
                ViewBag.State = selectListItems;

            }
            catch (Exception e)
            {
                ShowMesage("Load churches DropDown Errors : " + e.Message);
                throw;
            }
        }

        private void ShowMesage(String exceptionMessage)
        {
            log.Error("Exception : " + exceptionMessage);
        }


    }
}
