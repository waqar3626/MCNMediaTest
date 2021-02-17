﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MCNMedia_Dev.Controllers
{
    public class CustomersController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ChurchDataAccessLayer churchDataAccess = new ChurchDataAccessLayer();
        AnnouncementDataAccessLayer announcementDataAccessLayer = new AnnouncementDataAccessLayer();
        CameraDataAccessLayer camDataAccess = new CameraDataAccessLayer();
        RecordingDataAccessLayer recordDataAccess = new RecordingDataAccessLayer();
        ScheduleDataAccessLayer scheduleDataAccess = new ScheduleDataAccessLayer();
        MediaChurchDataAccessLayer mediaChurchDataAccess = new MediaChurchDataAccessLayer();
        NoticeDataAccessLayer noticeDataAccess = new NoticeDataAccessLayer();
        ChurchNewsLetterDataAccessLayer churchNewsLetterDataAccess = new ChurchNewsLetterDataAccessLayer();
        Profile profileModel = new Profile();
        public IActionResult Index()
        {
            return View();
        }


    
        public IActionResult RecordingsList()
        {
            int id = (int)HttpContext.Session.GetInt32("churchId");
            profileModel.RecordingList = recordDataAccess.Recording_GetByChurch(id);

            return View(profileModel);
        }


      
        public JsonResult ListSchedule()
        {
            try
            {
                int churchId = (int)HttpContext.Session.GetInt32("churchId");
                List<Schedule> ChurchSchedule = scheduleDataAccess.GetScheduleByChurch(churchId).ToList();
                return Json(ChurchSchedule);
            }
            catch (Exception e)
            {
                ShowMesage("Add Church Schedule Errors : " + e.Message);
                throw;

            }

        }
        public IActionResult SchedulesList()
        {
            int id = (int)HttpContext.Session.GetInt32("churchId");
           
            return View(profileModel);
        }
        [Route("/iframe/{id?}")]
        public IActionResult CameraLiveStream(string? id)
        {
            string ChurchUniqeIdentity = "";
            if (id != null) {
                
                
                ChurchUniqeIdentity = id;
            }
           
            Church church = churchDataAccess.GetChurchByUniqueIdentifier(ChurchUniqeIdentity);
           
          
            int churchPass = Convert.ToInt32(TempData["ProfileModel"]);
            if (church.Password == HttpContext.Session.GetString("Password"))
            {

            }
            else
            {
                if (church.Password.Count() > 0)
                {
                    if (churchPass == 1)
                    {

                    }
                    else
                    {
                        TempData["ChurchIdentifier"] = ChurchUniqeIdentity;
                        TempData["Password"] = church.Password;
                  
                        return RedirectToAction(nameof(CustomerChurchLock));
                    }

                }
                else
                {

                }
            }

                int churchId = church.ChurchId;
                HttpContext.Session.SetInt32("churchId", churchId);
                profileModel.CameraList = camDataAccess.GetAllCamerasByChurch(churchId);
            profileModel.VideoList = mediaChurchDataAccess.GetByMediaType("Video", churchId).ToList();
            profileModel.PictureList = mediaChurchDataAccess.GetByMediaType("Picture", churchId).ToList();
            profileModel.RecordingList = recordDataAccess.Recording_GetByChurch(churchId);
            return View("CustomerView",profileModel);
            

           
        }

        public IActionResult CustomerChurchLock()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CustomerChurchLock(ChurchLock churchLock)
        {
            try
            {
               
               
                String pass = TempData["Password"].ToString();
                if (churchLock.Password == pass)
                {
                    TempData["ProfileModel"] = 1;
                    HttpContext.Session.SetString("Password", pass);
                    return RedirectToAction(nameof(CameraLiveStream),new {id = TempData["ChurchIdentifier"].ToString() });
                }
                else
                {
                    ViewBag.IsSuccess = 3;
                    TempData["ChurchIdentifier"] = TempData["ChurchIdentifier"].ToString();
                    TempData["Password"] = TempData["Password"].ToString();
                    return View();
                }
            }

            catch (Exception e)
            {
                ShowMesage("ProcessForm Errors : " + e.Message);
                throw;
            }


        }
        private void ShowMesage(string exceptionMessage)
        {
            log.Error("Exception : " + exceptionMessage);
        }
        [HttpGet]
        public IActionResult Player(int id)
        {
            try
            {
                int recordingPass = 0;
                RecordingDataAccessLayer recordingDataAccessLayer = new RecordingDataAccessLayer();
                if (id == 0)
                {
                    TempData["RecordingId"] = id;
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
                            TempData["RecordingPassword"] = recording.Password;
                            return RedirectToAction(nameof(RecordingLock));
                        }
                    }
                }
                return Json(recording);
            }
            catch (Exception exp)
            {

                ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                return View();
            }
        }
        public IActionResult RecordingLock()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RecordingLock(RecordingLock recordingLock)
        {
            try
            {
                String pass = TempData["RecordingPassword"].ToString();
                if (recordingLock.Password == pass)
                {
                    HttpContext.Session.SetInt32("RecordingPass", 1);
                    return RedirectToAction(nameof(Player), new { id = Convert.ToInt32(TempData["RecordingId"]) });
                }
                else
                {
                    ViewBag.IsSuccess = 3;
                    TempData["RecordingId"] = TempData["RecordingId"].ToString();
                    TempData["RecordingPassword"] = TempData["RecordingPassword"].ToString();
                    return View();
                }
            }
            catch (Exception exp)
            {

                ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                return View();
            }
        }
    }
}
