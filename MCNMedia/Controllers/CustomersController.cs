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
                List<Schedule> ChurchSchedule = scheduleDataAccess.GetAllChurchSchedule(churchId).ToList();
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
                HttpContext.Session.SetString("churchUniqueIdentity", id);
                
                ChurchUniqeIdentity = id;
            }
            else
            {
                ChurchUniqeIdentity = HttpContext.Session.GetString("churchUniqueIdentity");
            }
            Church church = churchDataAccess.GetChurchByUniqueIdentifier(ChurchUniqeIdentity);
            HttpContext.Session.SetString("ChurchPass", church.Password);
          
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
                        return RedirectToAction(nameof(CustomerChurchLock));
                    }

                }
                else
                {

                }
            }

                int churchId = church.ChurchId;
                HttpContext.Session.SetInt32("churchId", churchId);
                profileModel.CameraList = camDataAccess.GetAllCameras(churchId, "");
            profileModel.VideoList = mediaChurchDataAccess.GetByMediaType("Video", churchId).ToList();
            profileModel.PictureList = mediaChurchDataAccess.GetByMediaType("Picture", churchId).ToList();
            return View(profileModel);
            

           
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
                String pass = HttpContext.Session.GetString("ChurchPass").ToString();
                if (churchLock.Password == pass)
                {
                    TempData["ProfileModel"] = 1;
                    HttpContext.Session.SetString("Password", pass);
                    return RedirectToAction(nameof(CameraLiveStream));
                }
                else
                {
                    ViewBag.IsSuccess = 3;
                    //ViewData["Message"] = "Incorrect Password";
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
    }
}
