﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCNMedia_Dev._Helper;
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
        ScheduleDataAccessLayer scheduleDataAccess = new ScheduleDataAccessLayer();
        RecordingDataAccessLayer recordingDataAccess = new RecordingDataAccessLayer();
        //PreviewChurchesDataAccessLayer previewChurchesDataAccess = new PreviewChurchesDataAccessLayer();
        CameraDataAccessLayer camDataAccess = new CameraDataAccessLayer();
        ChurchNewsLetterDataAccessLayer churchNewsLetterDataAccess = new ChurchNewsLetterDataAccessLayer();
        MediaChurchDataAccessLayer mediaChurchDataAccess = new MediaChurchDataAccessLayer();
        GenericModel gm = new GenericModel();


        #region church info



        [HttpGet]
        public IActionResult ChurchInfo()
        {
            try
            {
                int churchId = 0;
                if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
                {
                    churchId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));
                }


                Church church = chdataAccess.GetChurchData(churchId);
                if (church == null)
                {
                    return NotFound();
                }
                HttpContext.Session.SetString("ctabId", "/Client/ChurchInfo");
                return View(church);
            }
            catch (Exception e)
            {
                ShowMesage("Church Info Errors : " + e.Message);
                throw;
            }

        }

        [HttpPost]
        public IActionResult UpdateChurchInfo(Church church, IFormFile ImageUrl, string hdnImageUrl)
        {

            string fileName = "";
            if (ImageUrl != null)
            {
                fileName = Path.GetFileName(ImageUrl.FileName);
                church.ImageURl = FileUploadUtility.UploadFile(ImageUrl, UploadingAreas.ChurchProfileImage); // Path.Combine(dirPath, fileName).Replace(@"\", @"/"); 
            }
            else
            {
                int pos = hdnImageUrl.IndexOf("Upload");
                if (pos >= 0)
                {
                    // String after founder  

                    // Remove everything before url but include Upload 
                    string beforeFounder = hdnImageUrl.Remove(0, pos);
                    church.ImageURl = beforeFounder;
                }

            }
            church.UpdateBy = (int)HttpContext.Session.GetInt32("UserId");
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
                int id = 0;
                if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
                {
                    id = (int)HttpContext.Session.GetInt32("ChurchId");
                }

                GenericModel gm = new GenericModel();
                gm.LAnnouncement = AnnouncementDataAccessLayer.GetAnnouncement(id);
                gm.Churches = chdataAccess.GetChurchData(id);
                HttpContext.Session.SetString("ctabId", "/Client/Announcement");
                return View(gm);
            }
            catch (Exception e)
            {
                ShowMesage("Announcement Errors :" + e.Message);
                throw;
            }
        }
        [HttpGet]
        public IActionResult EditAnnouncement(int id)
        {
            try
            {
                GenericModel gm = new GenericModel();
                gm.Announcement = AnnouncementDataAccessLayer.GetAnnouncementById(id);
                return PartialView("_EditAnnouncement", gm);

            }
            catch (Exception e)
            {
                ShowMesage("Edit Announcement Error" + e.Message);
                throw;
            }




        }

        public IActionResult ListMobileCamera()
        {
            GenericModel gm = new GenericModel();
            int CameraId = Convert.ToInt32(TempData["CameraId"]);
            if (CameraId > 0)
            {
                ViewBag.NewCamera = 2;
                Camera camera = camDataAccess.GetCameraById(CameraId, "");
                gm.Cameras = camera;
            }
            else
            {
                ViewBag.NewCamera = 1;
            }

            int id = (int)HttpContext.Session.GetInt32("ChurchId");


            gm.LCameras = camDataAccess.GetAllCameras(id, "ClientCamera");

            gm.Churches = chdataAccess.GetChurchData(id);

            HttpContext.Session.SetString("ctabId", "/Client/ListMobileCamera");


            return View(gm);
        }

        [HttpPost]
        public IActionResult AddMobileCamera(string CameraName)
        {
            GenericModel gm = new GenericModel();
            int id = (int)HttpContext.Session.GetInt32("ChurchId");
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            if (userId != 0 && id != 0)
            {
                int CameraId = camDataAccess.AddMobileCamera(CameraName, id, userId);
                if (CameraId > 0)
                {
                    TempData["CameraId"] = CameraId;
                    return RedirectToAction(nameof(ListMobileCamera));
                }
                else
                {
                    return RedirectToAction(nameof(ListMobileCamera));
                }

            }
            return Json(2);

        }

        public IActionResult DeleteCamera(int id)
        {
            try
            {
                GenericModel gm = new GenericModel();
                bool res = camDataAccess.DeleteCamera(id);
                return Json(res);
            }
            catch (Exception e)
            {
                ShowMessage("Delete Camera Error" + e.Message);
                throw;
            }

        }

        #region "Facebook Section"
        [HttpPost]
        public JsonResult GetCamerasInfo()
        {
            int id = (int)HttpContext.Session.GetInt32("ChurchId");
            GenericModel gm = new GenericModel();
            gm.LCameras = camDataAccess.GetAllCameras(id, "");
            return Json(gm.LCameras);
        }

        [HttpPost]
        public JsonResult SaveUserInfo(string accessToken, string tokenExpiry, string liveStatus)
        {
            //Save churchId as UserId in FB details
            int churchId = (int)HttpContext.Session.GetInt32("ChurchId");
            camDataAccess.SaveUserInfo(churchId, accessToken, tokenExpiry, liveStatus);
            return Json("");
        }

        [HttpPost]
        public JsonResult SaveSettings(string cameraId, string pageId, string pageName, string pageAccessToken, string description)
        {
            int churchId = (int)HttpContext.Session.GetInt32("ChurchId");
            camDataAccess.SaveSettings(churchId, pageAccessToken, pageId, pageName, description, cameraId);
            return Json("");
        }

        [HttpPost]
        public JsonResult GetFbDetailsByUser()
        {

            int churchId = (int)HttpContext.Session.GetInt32("ChurchId");
            FBLoginDetails fBDetails  = camDataAccess.GetFbDetailsByUser(churchId);

            return Json(fBDetails);
           
        }
        #endregion
        [HttpPost]
        public JsonResult UpdateAnnouncement(int churchAnnounceId, string editAnnounceTitle, string editAnnounceText)
        {
            try
            {
                Announcement announcement = new Announcement();
                announcement.ChurchAnnouncementId = Convert.ToInt32(churchAnnounceId);
                announcement.AnnouncementTitle = editAnnounceTitle;
                announcement.AnnouncementText = editAnnounceText;
                announcement.UpdatedBy = (int)HttpContext.Session.GetInt32("UserId");
                int res = AnnouncementDataAccessLayer.UpdateAnnouncement(announcement);


                return Json(res);
            }
            catch (Exception e)
            {


                ShowMesage("Update Announcement Error" + e.Message);
                throw;
            }

        }
        #endregion



        #region Newsletter

        public IActionResult ChurchNewsLetter()
        {

            try
            {

                int id = (int)HttpContext.Session.GetInt32("ChurchId");
                gm.Churches = chdataAccess.GetChurchData(id);
                HttpContext.Session.SetString("ctabId", "/Client/ChurchNewsLetter");
                return View(gm);
            }
            catch (Exception e)
            {
                ShowMesage("Church NewsLetter Errors : " + e.Message);
                throw;
            }
        }

        #endregion


        #region Camera
        public IActionResult CameraDetail()

        {
            try
            {
                int id = 0;
                if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
                {
                    id = (int)HttpContext.Session.GetInt32("ChurchId");

                }
                LoadServerDDL();

                gm.Churches = chdataAccess.GetChurchData(id);
                gm.LCameras = camDataAccess.GetAllCameras(id, "ClientCamera").ToList();
                HttpContext.Session.SetString("ctabId", "/Client/CameraDetail");
                return View(gm);
            }
            catch (Exception e)
            {
                ShowMesage("Camera Detail Errors : " + e.Message);
                throw;
            }
        }

        public IActionResult Schedule()

        {
            try
            {
                LoadServerDDL();
                int id = (int)HttpContext.Session.GetInt32("ChurchId");
                gm.Churches = chdataAccess.GetChurchData(id);
                HttpContext.Session.SetString("ctabId", "/Client/Schedule");
                return View(gm);
            }
            catch (Exception e)
            {
                ShowMesage("Schedule Detail Errors : " + e.Message);
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
                ShowMesage("Stream To FaceBook : " + e.Message);
                throw;
            }
        }
        #endregion



        #region Recording


        [HttpGet]
        public IActionResult Recording()

        {
            DateTime FromDate = DateTime.Now.AddDays(-7);
            DateTime ToDate = DateTime.Now;
            GenericModel gm = new GenericModel();

            LoadChurchDDL();
            int churchId = (int)HttpContext.Session.GetInt32("ChurchId");

            gm.Churches = chdataAccess.GetChurchData(churchId);
            HttpContext.Session.SetString("ctabId", "/Client/Recording");
            gm.LRecordings = recordingDataAccess.RecordingSearch(FromDate, ToDate, churchId, "").ToList<Recording>();
            ViewBag.FromDate = FromDate.ToString("dd-MMM-yyyy");
            ViewBag.ToDate = ToDate.ToString("dd-MMM-yyyy");
            ViewBag.EventName = "";

            return View(gm);
        }

        [HttpPost]
        public IActionResult SearchRecording(string fromDate, string toDate, string EventName)
        {
            try
            {
                GenericModel gm = new GenericModel();
                DateTime FromDate = Convert.ToDateTime(fromDate);
                DateTime ToDate = Convert.ToDateTime(toDate);
                int churchId = (int)HttpContext.Session.GetInt32("ChurchId");
                gm.Churches = chdataAccess.GetChurchData(churchId);
                gm.LRecordings = recordingDataAccess.RecordingSearch(FromDate, ToDate, churchId, EventName).ToList<Recording>();
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
                ViewBag.EventName = EventName;
                LoadChurchDDL();
                return View("Recording", gm);
            }
            catch (Exception e)
            {
                ShowMessage("Client Search Recording  Errors : " + e.Message);
                throw;
            }
        }

        [HttpGet]
        public IActionResult EditRecordingClient(int id)
        {
            try
            {
                int id1 = (int)HttpContext.Session.GetInt32("ChurchId");
                GenericModel gm = new GenericModel();

                gm.Churches = chdataAccess.GetChurchData(id1);
                gm.Recordings = recordingDataAccess.Recording_GetById(id);
                if (gm.Recordings == null)
                {
                    return NotFound();
                }
                LoadChurchDDL();

                return View(gm);
            }
            catch (Exception e)
            {
                ShowMesage("Edit Recording Client Errors : " + e.Message);
                throw;
            }
        }


        [HttpPost]
        public IActionResult EditRecordingClient(GenericModel gm)
        {
            gm.Recordings.UpdatedBy = (int)HttpContext.Session.GetInt32("UserId");
            recordingDataAccess.UpdateRecording(gm.Recordings);
            return RedirectToAction("Recording");

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
                ShowMesage("Load Server Dropdown Errors : " + e.Message);
                throw;
            }

        }

        //[HttpPost]
        //public IActionResult EditScheduleClient(GenericModel gm)
        //{
        //    try
        //    {

        //        if (gm.Schedules.EventDay == null)
        //        {
        //            gm.Schedules.EventDay = gm.Schedules.EventDate.ToString("dddd");

        //        }
        //        gm.Schedules.UpdatedBy = (int)HttpContext.Session.GetInt32("UserId");
        //        scheduleDataAccess.UpdateSchedule(gm.Schedules);
        //        return RedirectToAction("Schedule");

        //    }
        //    catch (Exception e)
        //    {
        //        ShowMesage("Edit Schedule Client Errors : " + e.Message);
        //        throw;
        //    }
        //}
        //public void LoadChurchesDDL()
        //{
        //    try
        //    {
        //        ChurchDataAccessLayer churchDataAccessLayer = new ChurchDataAccessLayer();
        //        System.Data.DataTable churches = churchDataAccessLayer.GetChurchDDL();
        //        List<SelectListItem> selectListItems = new List<SelectListItem>();
        //        foreach (System.Data.DataRow item in churches.Rows)
        //        {
        //            selectListItems.Add(new SelectListItem { Text = item["ChurchName"].ToString(), Value = item["ChurchId"].ToString() });
        //        }
        //        ViewBag.State = selectListItems;

        //    }
        //    catch (Exception e)
        //    {
        //        ShowMesage("Load churches DropDown Errors : " + e.Message);
        //        throw;
        //    }
        //}

        public IActionResult ClientPlayer(int id)
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

        #region Media Detail
        public JsonResult ListPictureClient()
        {
            GenericModel gm = new GenericModel();
            int ChrId = (int)HttpContext.Session.GetInt32("ChurchId");
            gm.Pictures = mediaChurchDataAccess.GetByMediaType("Picture", ChrId);
            return Json(gm.Pictures);
        }
        public JsonResult _ListVideoClient()
        {
            GenericModel gm = new GenericModel();
            int ChrId = (int)HttpContext.Session.GetInt32("ChurchId");
            gm.Videos = mediaChurchDataAccess.GetByMediaType("Video", ChrId);
            return Json(gm.Videos);

        }

        public JsonResult _ListSlideShowClient()
        {
            GenericModel gm = new GenericModel();
            int ChrId = (int)HttpContext.Session.GetInt32("ChurchId");
            gm.SlideShow = mediaChurchDataAccess.GetByMediaType("SlideShow", ChrId);
            return Json(gm.SlideShow);

        }

       

        public IActionResult MediaDetail()
        {

            LoadChurchDDL();
            int ChrId = (int)HttpContext.Session.GetInt32("ChurchId");
            gm.Pictures = mediaChurchDataAccess.GetByMediaType("Picture", ChrId);

            gm.Videos = mediaChurchDataAccess.GetByMediaType("Video", ChrId);
            gm.SlideShow = mediaChurchDataAccess.GetByMediaType("SlideShow", ChrId);
            gm.Churches = chdataAccess.GetChurchData(ChrId);
            HttpContext.Session.SetString("ctabId", "/Client/MediaDetail");
            Redirect("MediaDetail");
            return View(gm);
        }
        #endregion

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
                    selectListItems.Add(new SelectListItem { Text = item.ChurchName.ToString(), Value = item.ChurchId.ToString() });
                }
                ViewBag.Church = selectListItems;
            }
            catch (Exception e)
            {
                ShowMessage("LoadChurch DropDown Errors : " + e.Message);
                throw;
            }
        }
        private void ShowMesage(String exceptionMessage)
        {
            log.Error("Exception : " + exceptionMessage);
        }

        private void ShowMessage(string exceptionMessage)
        {
            log.Info("Exception: " + exceptionMessage);
        }
    }
}
