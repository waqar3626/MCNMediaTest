using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Crmf;

namespace MCNMedia_Dev.Controllers
{
    public class ClientController : Controller
    {
        ChurchDataAccessLayer chdataAccess = new ChurchDataAccessLayer();
        AnnouncementDataAccessLayer AnnouncementDataAccessLayer = new AnnouncementDataAccessLayer();
        scheduleDataAccessLayer scheduleDataAccess = new scheduleDataAccessLayer();
        RecordingDataAccessLayer recordingDataAccess = new RecordingDataAccessLayer();
        PreviewChurchesDataAccessLayer previewChurchesDataAccess = new PreviewChurchesDataAccessLayer();
        GenericModel gm = new GenericModel();

        #region church info
      
       

        [HttpGet]
        public IActionResult ChurchInfo(int churchId)
        {
            Church church = chdataAccess.GetChurchData(churchId);
            if (church == null)
            {
                return NotFound();
            }
            return View(church);
           
        }
        
        public JsonResult GetChurchInfoById(int churchId)
        {
            Church church = chdataAccess.GetChurchData(churchId);
            HttpContext.Session.SetInt32("ChurchId1", churchId);
           
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
           
            int  id= (int)HttpContext.Session.GetInt32("ChurchId1");
            GenericModel gm = new GenericModel();
            gm.LAnnouncement = AnnouncementDataAccessLayer.GetAnnouncement(id);
            gm.Churches= chdataAccess.GetChurchData(id);
            //List<Announcement> announcementsList = AnnouncementDataAccessLayer.GetAnnouncement(id).ToList();
            //gm.Churches = chdataAccess.GetChurchData(1);

            return View(gm);
        }

        public IActionResult UpdateAnnouncement([Bind] Announcement announcement)
        {

            AnnouncementDataAccessLayer.UpdateAnnouncement(announcement);
            return RedirectToAction("Announcement");

        }
        #endregion

        #region Camera
        public IActionResult CameraDetail()
        {
            int id = (int)HttpContext.Session.GetInt32("ChurchId1");
            gm.Churches = chdataAccess.GetChurchData(id);
            return View(gm);
        }
        #endregion

        #region StreamToFaceBook
        public IActionResult StreamToFaceBook()
        {
            int id = (int)HttpContext.Session.GetInt32("ChurchId1");
            gm.Churches = chdataAccess.GetChurchData(id);
            return View(gm);
        }
        #endregion

        #region ScheduleAndRecording
        //public IActionResult ScheduleAndRecording()
        //{
        //    return View();
        //}

        [HttpGet]
        public IActionResult ScheduleAndRecording()
        {
            int id = (int)HttpContext.Session.GetInt32("ChurchId1");
            GenericModel gm = new GenericModel();
            //gm.LSchedules = scheduleDataAccess.GetAllSchedule(id);
            //gm.LRecordings = recordingDataAccess.GetAllRecording(id);
            //gm.Churches = chdataAccess.GetChurchData(id);
            gm.LSchedules = previewChurchesDataAccess.GetAllPreviewSchedule(id);
            gm.LRecordings = previewChurchesDataAccess.GetAllPreviewRecording(id);
            gm.Churches = chdataAccess.GetChurchData(id);
            //gm.Churches = previewChurchesDataAccess.GetPreviewChurch(id);
            Redirect("ScheduleAndRecording");
            return View(gm);
        }

        #endregion
    }
}
