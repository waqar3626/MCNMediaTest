using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        PreviewChurchesDataAccessLayer previewChurchesDataAccess = new PreviewChurchesDataAccessLayer();
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
        
        
      

        public IActionResult PreviewClient(int chId)
        {
            gm.Churches = previewChurchesDataAccess.GetPreviewChurch(chId);
            return View(gm);
        }

        [HttpPost]
        public IActionResult UpdateChurchInfo(Church church, IFormFile imageUrlMain, string ImageUrl)
        {

            string fileName = "";
            if (imageUrlMain != null)
            {
                fileName = Path.GetFileName(imageUrlMain.FileName);
                church.ImageURl = FileUploadUtility.UploadFile(imageUrlMain, UploadingAreas.ChurchProfileImage); // Path.Combine(dirPath, fileName).Replace(@"\", @"/"); 
            }
            else
            {
                int pos = ImageUrl.IndexOf("Upload");
                if (pos >= 0)
                {
                    // String after founder  
                    
                    // Remove everything before url but include Upload 
                    string beforeFounder = ImageUrl.Remove(0, pos);
                    church.ImageURl = beforeFounder;
                }
              
            }
            church.UpdateBy = (int)HttpContext.Session.GetInt32("UserId");
            chdataAccess.UpdateChurch(church);
            return RedirectToAction("ChurchInfo");

        }
        #endregion

        #region Media Detail
        public IActionResult MediaDetail()
        {
           int ChrId = (int)HttpContext.Session.GetInt32("ChurchId");
            gm.Pictures = mediaChurchDataAccess.GetByMediaType("Picture",ChrId);
            gm.Videos = mediaChurchDataAccess.GetByMediaType("Video", ChrId);
            gm.SlideShow = mediaChurchDataAccess.GetByMediaType("SlideShow", ChrId);
            return View(gm);
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
                HttpContext.Session.SetString("ctabId", "/Client/ChNewsLetter");
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
                gm.LCameras = camDataAccess.GetAllCameras(id).ToList();
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
                ShowMesage("Stream To FaceBook : "+ e.Message);
                throw;
            }
        }
        #endregion



        #region Schedule

        //[HttpGet]
        //public IActionResult Schedule()
        //{
        //    try
        //    {

        //        int id = (int)HttpContext.Session.GetInt32("ChurchId");
        //        GenericModel gm = new GenericModel();
        //        gm.LSchedules = previewChurchesDataAccess.GetAllPreviewSchedule(id);
        //        gm.Churches = chdataAccess.GetChurchData(id);
        //        Redirect("Schedule");
        //        return View(gm);

        //    }
        //    catch (Exception e)
        //    {
        //        ShowMesage("Schedule Errors : " + e.Message);
        //        throw;
        //    }
       // }
        [HttpGet]
        public IActionResult Recording()
        {
            int id = (int)HttpContext.Session.GetInt32("ChurchId");
            GenericModel gm = new GenericModel();
            gm.LRecordings = previewChurchesDataAccess.GetAllPreviewRecording(id);
            gm.Churches = chdataAccess.GetChurchData(id);
            HttpContext.Session.SetString("ctabId", "/Client/Recording");
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
            gm.Recordings.UpdatedBy = (int)HttpContext.Session.GetInt32("UserId");
            recordingDataAccess.UpdateRecording(gm.Recordings);
            return RedirectToAction("Recording");

        }

      
        
        //[HttpGet]
        //public IActionResult EditScheduleClient(int id)
        //{
        //    int id2 = (int)HttpContext.Session.GetInt32("ChurchId");
        //    GenericModel gm = new GenericModel();
        //    gm.Churches = chdataAccess.GetChurchData(id2);
        //    gm.Schedules= scheduleDataAccess.GetScheduleDataBtId(id);
        //    if (gm.Schedules == null)
        //    {
        //        return NotFound();
        //    }
        //    LoadChurchesDDL();
        //    return View(gm);
        //}
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
