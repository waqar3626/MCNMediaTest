﻿using System;
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
           
            int  id= (int)HttpContext.Session.GetInt32("ChurchId");
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
            LoadServerDDL();
            int id = (int)HttpContext.Session.GetInt32("ChurchId");
            gm.Churches = chdataAccess.GetChurchData(id);
            return View(gm);
        }


       
        #endregion

        #region StreamToFaceBook
        public IActionResult StreamToFaceBook()
        {
            int id = (int)HttpContext.Session.GetInt32("ChurchId");
            gm.Churches = chdataAccess.GetChurchData(id);
            return View(gm);
        }
        #endregion

        #region Schedule
        //public IActionResult ScheduleAndRecording()
        //{
        //    return View();
        //}

        [HttpGet]
        public IActionResult Schedule()
        {
            LoadCameraDDL();
            int id = (int)HttpContext.Session.GetInt32("ChurchId");
            GenericModel gm = new GenericModel();
            //gm.LSchedules = scheduleDataAccess.GetAllSchedule(id);
            //gm.LRecordings = recordingDataAccess.GetAllRecording(id);
            //gm.Churches = chdataAccess.GetChurchData(id);
            gm.LSchedules = previewChurchesDataAccess.GetAllPreviewSchedule(id);
            gm.Churches = chdataAccess.GetChurchData(id);
            //gm.Churches = previewChurchesDataAccess.GetPreviewChurch(id);
            Redirect("Schedule");
            return View(gm);
        
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
            IEnumerable<Server> serverList = camDataAccess.GetServer();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (var item in serverList)
            {
                selectListItems.Add(new SelectListItem { Text = item.ServerName.ToString(), Value = item.ServerId.ToString() });
            }
            ViewBag.Server = selectListItems;

        }

        [HttpPost]
        public IActionResult EditScheduleClient(GenericModel gm)
        {
            
            scheduleDataAccess.UpdateSchedule(gm.Schedules);
            return RedirectToAction("Schedule");
        
        }
        public void LoadChurchesDDL()
        {
            IEnumerable<Recording> RecordList = recordingDataAccess.GetChurches();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (var item in RecordList)
            {
                selectListItems.Add(new SelectListItem { Text = item.ChurchName.ToString(), Value = item.ChurchId.ToString() });
            }
            ViewBag.State = selectListItems;

        }

        public void LoadCameraDDL()
        {
            Church chr = new Church();
            chr.ChurchId = 1;
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
    }
}
