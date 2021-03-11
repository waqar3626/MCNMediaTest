using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MCNMedia_Dev._Helper;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Cors;
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
        ChurchDonationDataAccessLayer DonationDataAccessLayer = new ChurchDonationDataAccessLayer();


        #region church info

        private GenericModel InfoChurchAndDonation()
        {
           

            int churchId = 0;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
            {
                churchId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));
            }

            Church church = chdataAccess.GetChurchData(churchId);
            if (church == null)
            {
                return new GenericModel();
            }
            HttpContext.Session.SetString("ctabId", "/Client/ChurchInfo");


            ChurchDonation churchDonation = DonationDataAccessLayer.GetDonationByChurch(churchId);
            gm.Churches = church;
            gm.ChurchDonations = churchDonation;
            return gm;
        }

        [HttpGet]
        public IActionResult ChurchInfo()
        {
            GenericModel gm = new GenericModel();
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return RedirectToAction("UserLogin", "UserLogin");
                }
                gm = InfoChurchAndDonation();
                return View(gm);
            }
            catch (Exception exp)
            {
                HttpContext.Session.SetString("ctabId", "/Client/ChurchInfo");
                ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                return View(gm);
            }
        }

        [HttpPost]
        public IActionResult UpdateChurchInfo([Bind] GenericModel church, IFormFile ImageUrl, string hdnImageUrl)
        {
            GenericModel gm = new GenericModel();
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return RedirectToAction("UserLogin", "UserLogin");
                }

                string fileName = "";
                if (ImageUrl != null)
                {
                    fileName = Path.GetFileName(ImageUrl.FileName);
                    church.Churches.ImageURl = FileUploadUtility.UploadFile(ImageUrl, UploadingAreas.ChurchProfileImage); // Path.Combine(dirPath, fileName).Replace(@"\", @"/"); 
                }
                else
                {
                    int pos = hdnImageUrl.IndexOf("Upload");
                    if (pos >= 0)
                    {
                        // String after founder  

                        // Remove everything before url but include Upload 
                        string beforeFounder = hdnImageUrl.Remove(0, pos);
                        church.Churches.ImageURl = beforeFounder;
                    }

                }
                HttpContext.Session.SetString("ctabId", "/Client/ChurchInfo");
                church.Churches.UpdateBy = (int)HttpContext.Session.GetInt32("UserId");
                chdataAccess.UpdateChurch(church.Churches);
                gm = InfoChurchAndDonation();
                ViewBag.SuccessMsg = "Church Info Updated Successfully!";
                return View("ChurchInfo",gm);
            }
            catch (Exception exp)
            {
                HttpContext.Session.SetString("ctabId", "/Client/ChurchInfo");
                ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                return View("ChurchInfo", gm);
            }
            

        }
        #endregion

        
       
        #region Announcement
        [HttpGet]
        public IActionResult Announcement()
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return RedirectToAction("UserLogin", "UserLogin");
                }
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
                ShowMessage("Announcement Errors :" + e.Message);
                throw;
            }
        }
        [HttpGet]
        public IActionResult EditAnnouncement(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return RedirectToAction("UserLogin", "UserLogin");
                }
                GenericModel gm = new GenericModel();
                gm.Announcement = AnnouncementDataAccessLayer.GetAnnouncementById(id);
                return PartialView("_EditAnnouncement", gm);

            }
            catch (Exception e)
            {
                ShowMessage("Edit Announcement Error" + e.Message);
                throw;
            }




        }

        public IActionResult ListMobileCamera()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
            {
                return RedirectToAction("UserLogin", "UserLogin");
            }
            GenericModel gm = new GenericModel();
            int CameraId = Convert.ToInt32(TempData["CameraId"]);
            if (CameraId > 0)
            {
                 ViewBag.NewCamera = 2;
                Camera camera = camDataAccess.GetCameraById(CameraId);
                gm.Cameras = camera;
            }
            else
            {
                ViewBag.NewCamera = 1;
            }

            int id = (int)HttpContext.Session.GetInt32("ChurchId");


            gm.LCameras = camDataAccess.GetMobileCameraByChurch(id);

            gm.Churches = chdataAccess.GetChurchData(id);

            HttpContext.Session.SetString("ctabId", "/Client/ListMobileCamera");


            return View(gm);
        }

        [HttpPost]
        public IActionResult AddMobileCamera(string CameraName)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
            {
                return RedirectToAction("UserLogin", "UserLogin");
            }
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
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return RedirectToAction("UserLogin", "UserLogin");
                }
                GenericModel gm = new GenericModel();
                bool res = camDataAccess.DeleteCamera(id);
                return Json(new {success=true, res });
            }
            catch (Exception e)
            {
                return Json(new { success = false, responseText=e.Message });

            }

        }

        #region "Facebook Section

        [EnableCors("_myAllowSpecificOrigins")]
        [HttpPost]
        public JsonResult LiveStreamToFacebook(string jsonData, string CamId)
        {
            int Church_Id = (int)HttpContext.Session.GetInt32("ChurchId");
            int CameraId = Convert.ToInt32(CamId);
            WowzaApi.WowzaHelper wowza = new WowzaApi.WowzaHelper();
            if (wowza.CheckCameraBeforeStreaming(Church_Id, CameraId))
            {
                FacebookHelper facebookHelper = new FacebookHelper();
                return Json(facebookHelper.FacebookLiveStream(jsonData));
            }
            else
            {
                return Json(false);
            }
            
        }

        [EnableCors("_myAllowSpecificOrigins")]
        [HttpPost]
        public JsonResult StopLiveStreamToFacebook(string jsonData)
        {
            FacebookHelper facebookHelper = new FacebookHelper();
            return Json(facebookHelper.StopFacebookLiveStreaming(jsonData));
        }

        public void StreamtoFacebook()
        {
            string returnData = String.Empty;
            string url = "http://54.217.38.80:8087/v2/servers/_defaultServer_/vhosts/_defaultVHost_/applications/live/pushpublish/mapentries/facebook_timeline_target";

            var webRequest = WebRequest.Create(url) as HttpWebRequest;
            if (webRequest != null)
            {
                webRequest.Accept = "*/*";
                webRequest.UserAgent = ".NET";
                webRequest.Method = WebRequestMethods.Http.Post;
                webRequest.ContentType = "application/json";
                webRequest.Host = "";

                var whc = new WebHeaderCollection
                {
                    "application: " + "live",
                    "enabled: " + true,
                    "entryName: " + "facebook_timeline_target",
                    "facebook.accessToken: " + "EAAVj2VeRPDoBAAJt2dgSyOHLfl37QH8zAVscaQoDPi6yILvQ5OzIUCMC2QDqZAWiw2gIfqmJORP9bl1yQkjbbgAZCqWwPQ28bZAPdGyUIaBXyZAAvftx4ZCmXAx1A4HiiNHiCQ6sZAD5tLh2CHTMSQPrZA5ZBViDhR9OZCbiyTYL4kgZDZD",
                    "facebook.description: " + "This is a stream from Wowza Streaming Engine.",
                    "facebook.destId: " + "3695099897240804",
                    "facebook.destName: " + "My Timeline",
                    "facebook.destType: " + "timeline",
                    "facebook.privacy: " + "onlyMe",
                    "facebook.title: " + "Live Test",
                    "facebook.useAppSecret: " + "false",
                    "profile: " + "rtmp-facebook",
                    "sourceStreamName: " + "mob6aa78cb_934.stream"
                };
                webRequest.Headers = whc;

                using (WebResponse response = webRequest.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        returnData = reader.ReadToEnd();
                    }
                }
            }


        }
        [HttpPost]
        public JsonResult GetCamerasInfo()
        {
            int id = (int)HttpContext.Session.GetInt32("ChurchId");
            GenericModel gm = new GenericModel();
            gm.LCameras = camDataAccess.GetAllCamerasByChurch(id);
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
        public JsonResult GetStreamParams(string cameraId, string pageId, string pageName, string pageAccessToken, string description, string userId, string userAccessToken)
        {
            int churchId = (int)HttpContext.Session.GetInt32("ChurchId");
            WowzaApi.WowzaHelper wowza = new WowzaApi.WowzaHelper();
            bool resu = wowza.RequestFacebookStreaming(churchId, Convert.ToInt32(cameraId), userAccessToken);
            // camDataAccess.SaveSettings(churchId, pageAccessToken, pageId, pageName, description, cameraId);
            return Json(resu.ToString());
        }

        [HttpPost]
        public JsonResult GetFbDetailsByUser()
        {

            int churchId = (int)HttpContext.Session.GetInt32("ChurchId");
            FBLoginDetails fBDetails = camDataAccess.GetFbDetailsByUser(churchId);

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


                ShowMessage("Update Announcement Error" + e.Message);
                throw;
            }

        }
        #endregion



        #region Newsletter

        public IActionResult ChurchNewsLetter()
        {

            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return RedirectToAction("UserLogin", "UserLogin");
                }

                int id = (int)HttpContext.Session.GetInt32("ChurchId");
                gm.Churches = chdataAccess.GetChurchData(id);
                HttpContext.Session.SetString("ctabId", "/Client/ChurchNewsLetter");
                return View(gm);
            }
            catch (Exception e)
            {
                ShowMessage("Church NewsLetter Errors : " + e.Message);
                throw;
            }
        }

        #endregion


        #region Camera
        public IActionResult CameraDetail()

        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
            {
                return RedirectToAction("UserLogin", "UserLogin");
            }
            try
            {
                int id = 0;
                if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
                {
                    id = (int)HttpContext.Session.GetInt32("ChurchId");

                }
                LoadServerDDL();

                gm.Churches = chdataAccess.GetChurchData(id);
                gm.LCameras = camDataAccess.GetMobileCameraByChurch(id).ToList();
                HttpContext.Session.SetString("ctabId", "/Client/CameraDetail");
                return View(gm);
            }
            catch (Exception e)
            {
                ShowMessage("Camera Detail Errors : " + e.Message);
                throw;
            }
        }

        public IActionResult Schedule()

        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return RedirectToAction("UserLogin", "UserLogin");
                }
                LoadServerDDL();
                int id = (int)HttpContext.Session.GetInt32("ChurchId");
                gm.Churches = chdataAccess.GetChurchData(id);
                HttpContext.Session.SetString("ctabId", "/Client/Schedule");
                return View(gm);
            }
            catch (Exception e)
            {
                ShowMessage("Schedule Detail Errors : " + e.Message);
                throw;
            }
        }




        #endregion



        #region StreamToFaceBook
        public IActionResult StreamToFaceBook()
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return RedirectToAction("UserLogin", "UserLogin");
                }
                int id = (int)HttpContext.Session.GetInt32("ChurchId");
                gm.Churches = chdataAccess.GetChurchData(id);
                return View(gm);
            }
            catch (Exception e)
            {
                ShowMessage("Stream To FaceBook : " + e.Message);
                throw;
            }
        }
        #endregion



        #region Recording


        [HttpGet]
        public IActionResult Recording()

        {
            GenericModel gm = new GenericModel();
            DateTime FromDate = DateTime.Now.AddDays(-7);
            DateTime ToDate = DateTime.Now;
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return RedirectToAction("UserLogin", "UserLogin");
                }
               


                LoadChurchDDL();
                int churchId = (int)HttpContext.Session.GetInt32("ChurchId");

                gm.Churches = chdataAccess.GetChurchData(churchId);
                HttpContext.Session.SetString("ctabId", "/Client/Recording");
                gm.LRecordings = recordingDataAccess.RecordingSearch(FromDate, ToDate, "", churchId, "").ToList<Recording>();
                ViewBag.FromDate = FromDate.ToString("dd-MMM-yyyy");
                ViewBag.ToDate = ToDate.ToString("dd-MMM-yyyy");
                ViewBag.EventName = "";
                
                return View(gm);
            }
            catch (Exception exp)
            {
                LoadChurchDDL();
                ViewBag.FromDate = FromDate.ToString("dd-MMM-yyyy");
                ViewBag.ToDate = ToDate.ToString("dd-MMM-yyyy");
                ViewBag.EventName = "";
                ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                return View(gm);
            }

           
        }

        [HttpPost]
        public IActionResult SearchRecording(string fromDate, string toDate, string EventName)
        {
            GenericModel gm = new GenericModel();
            DateTime FromDate = Convert.ToDateTime(fromDate);
            DateTime ToDate = Convert.ToDateTime(toDate);
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return RedirectToAction("UserLogin", "UserLogin");
                }
               
                int churchId = (int)HttpContext.Session.GetInt32("ChurchId");
                gm.Churches = chdataAccess.GetChurchData(churchId);
                gm.LRecordings = recordingDataAccess.RecordingSearch(FromDate, ToDate,"", churchId, EventName).ToList<Recording>();
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
                ViewBag.EventName = EventName;
               
                LoadChurchDDL();
                return View("Recording", gm);
            }
            catch (Exception exp)
            {
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
                ViewBag.EventName = EventName;
                LoadChurchDDL();
                ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                return View("Recording", gm);
            }
        }

        

        [HttpGet]
        public IActionResult _Edit(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return RedirectToAction("UserLogin", "UserLogin");
                }
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
                ShowMessage("Edit Recording Client Errors : " + e.Message);
                throw;
            }
        }


        [HttpPost]
        public IActionResult _Edit(GenericModel gm)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
            {
                return RedirectToAction("UserLogin", "UserLogin");
            }
            gm.Recordings.UpdatedBy = (int)HttpContext.Session.GetInt32("UserId");
            recordingDataAccess.UpdateRecording(gm.Recordings);
            return RedirectToAction("Recording");

        }




        #endregion



        public void LoadServerDDL()
        {
            try
            {
                ServerDAL serverDAL = new ServerDAL();
                List<Server> serverList = serverDAL.GetServer();
                List<SelectListItem> selectListItems = new List<SelectListItem>();
                foreach (var item in serverList)
                {
                    selectListItems.Add(new SelectListItem { Text = item.ServerName.ToString(), Value = item.ServerId.ToString() });
                }
                ViewBag.Server = selectListItems;
            }
            catch (Exception e)
            {
                ShowMessage("Load Server Dropdown Errors : " + e.Message);
                throw;
            }

        }

        public IActionResult ClientVideoPlayer(int id)
        {
            
            GenericModel gm = new GenericModel();
            int churchId = (int)HttpContext.Session.GetInt32("ChurchId");
            gm.Media = mediaChurchDataAccess.GetMediaById(id);
            gm.Churches = chdataAccess.GetChurchData(churchId);
            return View(gm);
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
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
            {
                return RedirectToAction("UserLogin", "UserLogin");
            }
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
                chr.Town = "";
                chr.CountryId = -1;
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

        public IActionResult _VideoClosedCaption()
        {

            return View();
        }
       

        [HttpGet]
        public IActionResult EditClientRecording(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return RedirectToAction("UserLogin", "UserLogin");
                }
                Recording recording = recordingDataAccess.Recording_GetById(id);
                if (recording == null)
                {
                    return NotFound();
                }
                LoadChurchDDL();

                return PartialView("_EditRecordingClient", recording);
            }
            catch (Exception e)
            {
                ShowMessage("Edit Recording Errors 'Get' : " + e.Message);
                throw;
            }
        }


        public JsonResult ChangeMobileCameraStatus(int cameraId, bool cameraStatus)
        {
            try
            {
                int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                camDataAccess.UpdateMobileCameraStatus(cameraId, cameraStatus, UserId);
                return Json(new { success = true, responseText = "The attached file is not supported." });
            }
            catch (Exception e)
            {
                return Json(new { success = false, responseText = e.Message });

            }
        }


        private void ShowMessage(string exceptionMessage)
        {
            log.Info("Exception: " + exceptionMessage);
        }
    }
}
