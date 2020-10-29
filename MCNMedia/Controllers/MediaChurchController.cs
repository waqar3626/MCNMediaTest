using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using MCNMedia_Dev._Helper;

namespace MCNMedia_Dev.Controllers
{
    public class MediaChurchController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        MediaChurchDataAccessLayer mediaChurchDataAccess = new MediaChurchDataAccessLayer();
        GenericModel gm = new GenericModel();

        private IWebHostEnvironment environment;

        public MediaChurchController(IWebHostEnvironment _environment)
        {
            environment = _environment;
        }

        public IActionResult Index()
        {
            try
            {
                GenericModel gm = new GenericModel();
                return View(gm);
            }
            catch (Exception e)
            {
                ShowMessage("Index Media Church  Error" + e.Message);
                throw;
            }
           
        }

        #region "Picture"

        [HttpPost]
        public JsonResult AddMedia(string mediaType, string AddPicTabName, IFormFile mediaFile)
        {
            try
            {
                MediaChurch mediaChurch = new MediaChurch();
                if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
                {
                    mediaChurch.ChurchId = (int)HttpContext.Session.GetInt32("ChurchId");
                    mediaChurch.MediaName = Path.GetFileName(mediaFile.FileName);
                    mediaChurch.MediaURL = FileUploadUtility.UploadFile(mediaFile, UploadingAreas.Picture, mediaChurch.ChurchId);
                    mediaChurch.MediaType = mediaType;
                    mediaChurch.TabName = AddPicTabName;
                    int res = mediaChurchDataAccess.AddMedia(mediaChurch);
                }
                return Json(1);
            }
            catch (Exception e)
            {
                ShowMessage("Add Picture Error" + e.Message);
                throw;
            }
           
        }

        public JsonResult GetMediaByTypeId(string medType)
        {
            try
            {
                int churchId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));
                List<MediaChurch> medInfo = mediaChurchDataAccess.GetByMediaType(medType, churchId).ToList();
                return Json(medInfo);
            }
            catch (Exception e)
            {
                ShowMessage("Get Specific Picture Error" + e.Message);
                throw;
            }
           

        }

        public IActionResult EditMedia(int id)
        {
            try
            {
                GenericModel gm = new GenericModel();
                gm.Media = mediaChurchDataAccess.GetMediaById(id);
                return PartialView("EditPicture", gm);
            }
            catch (Exception e)
            {
                ShowMessage("Edit Picture Error" + e.Message);
                throw;
            }
            
       
    }
    public JsonResult UpdateMedia(String ChurchMediaId, IFormFile mediaFile, string mediaType, String EditPictureTabName, string MediaUrl, string MediaName)
        {
            try
            {
                MediaChurch mediaupdate = new MediaChurch();


                if (mediaFile != null)
                {
                    mediaupdate.MediaURL = FileUploadUtility.UploadFile(mediaFile, UploadingAreas.Picture, Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId")));
                    mediaupdate.MediaName = mediaFile.FileName.ToString();
                }
                else
                {
                    mediaupdate.MediaURL = MediaUrl;
                    mediaupdate.MediaName = MediaName;

                }
                mediaupdate.ChurchMediaId = Convert.ToInt32(ChurchMediaId);
                mediaupdate.TabName = EditPictureTabName;
                mediaupdate.MediaType = mediaType;
                int res = mediaChurchDataAccess.UpdateMedia(mediaupdate);


                return Json(res);
            }
            catch (Exception e)
            {
                ShowMessage("Update Picture Error" + e.Message);
                throw;
            }
           
        }

        public IActionResult DeleteMedia(int id)
        {
            try
            {
                GenericModel gm = new GenericModel();
                bool res = mediaChurchDataAccess.DeleteMedia(id);
                return Json(res);
            }
            catch (Exception e)
            {
                ShowMessage("Delete Picture Error" + e.Message);
                throw;
            }

           
        }

        #endregion

        #region "Video"

        [HttpPost]
        public IActionResult AddVideo(IFormFile mediaFile, string mediaType, String AddVidTabName)
        {
            try
                {
                    if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
                    {
                        MediaChurch media = new MediaChurch();
                        media.ChurchId = (int)HttpContext.Session.GetInt32("ChurchId");
                        media.MediaURL = FileUploadUtility.UploadFile(mediaFile, UploadingAreas.Video, media.ChurchId);
                        media.MediaType = mediaType;
                        media.MediaName = mediaFile.FileName.ToString();
                        media.TabName = AddVidTabName;
                        int res = mediaChurchDataAccess.AddMedia(media);
                        return Json(res);
                    }
                    return RedirectToAction("Listchurch", "Church");
            }

            catch (Exception e)
            {
                ShowMessage("Add Video Error" + e.Message);
                throw;
            }
           
        }

        public JsonResult ListVideo(string medType)
        {
            try
            {
                MediaChurch mdChurch = new MediaChurch();
                int churchId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));

                List<MediaChurch> vidList = mediaChurchDataAccess.GetByMediaType(medType, churchId).ToList();
                return Json(vidList);
            }
            catch (Exception e)
            {
                ShowMessage("List Video Error" + e.Message);
                throw;
            }

            
        }

        public IActionResult EditVideo(int id)
        {
            try
            {
                GenericModel gm = new GenericModel();
                gm.Media = mediaChurchDataAccess.GetMediaById(id);
                return PartialView("_EditVideo", gm);
            }
            catch (Exception e)
            {
                ShowMessage("Edit Video Error" + e.Message);
                throw;
            }

            
        }

        public JsonResult UpdateVideo(int ChurchMediaId, IFormFile mediaFile, string mediaType, String EditVidTabName, string MediaUrl, string MediaName)
        {
            try
            {
                MediaChurch mediaupdate = new MediaChurch();

                if (mediaFile != null)
                {
                    mediaupdate.MediaURL = FileUploadUtility.UploadFile(mediaFile, UploadingAreas.Video, Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"))); // Church Id would be needed

                    mediaupdate.MediaName = mediaFile.FileName.ToString();
                }
                else
                {
                    mediaupdate.MediaURL = MediaUrl;
                    mediaupdate.MediaName = MediaName;

                }

                mediaupdate.ChurchMediaId = Convert.ToInt32(ChurchMediaId);
                mediaupdate.TabName = EditVidTabName;
                mediaupdate.MediaType = mediaType;

                int res = mediaChurchDataAccess.UpdateMedia(mediaupdate);
                return Json(res);
            }
            catch (Exception e)
            {
                ShowMessage("Update Video Error" + e.Message);
                throw;
            }

            
        }

        public IActionResult DeleteVideo(int id)
        {
            try
            {
                GenericModel gm = new GenericModel();
                bool res = mediaChurchDataAccess.DeleteMedia(id);
                return Json(res);
            }
            catch (Exception e)
            {
                ShowMessage("Delete Video Error" + e.Message);
                throw;
            }
           
        }

        #endregion

        #region "Slideshow"

        [HttpPost]
        public JsonResult AddSlide(string mediaType, string AddSlideshowTabName, IFormFile mediaFile)
        {
            try
            {
                MediaChurch mediaChurch = new MediaChurch();
                mediaChurch.ChurchId = (int)HttpContext.Session.GetInt32("ChurchId");
                mediaChurch.MediaName = Path.GetFileName(mediaFile.FileName);
                mediaChurch.MediaURL = FileUploadUtility.UploadFile(mediaFile, UploadingAreas.SlideShow, Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId")));
                mediaChurch.MediaType = mediaType;
                mediaChurch.TabName = AddSlideshowTabName;

                if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
                {

                    int res = mediaChurchDataAccess.AddMedia(mediaChurch);
                }
                return Json(1);
            }
            catch (Exception e)
            {
                ShowMessage("Add Slide Show Error" + e.Message);
                throw;
            }
          
        }

        public JsonResult GetSlideShowByTypeId(string medType)
        {
            try
            {
                int churchId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));
                List<MediaChurch> slideInfo = mediaChurchDataAccess.GetByMediaType(medType, churchId).ToList();
                return Json(slideInfo);
            }
            catch (Exception e)
            {
                ShowMessage("Get Specific Slide Show Error" + e.Message);
                throw;
            }
            

        }

        public IActionResult EditSlide(int id)
        {
            try
            {
                GenericModel gm = new GenericModel();
                gm.Media = mediaChurchDataAccess.GetMediaById(id);
                return PartialView("_EditSlideShow", gm);
            }
            catch (Exception e)
            {
                ShowMessage("Edit Slide Show Error" + e.Message);
                throw;
            }
           
        }

        public JsonResult UpdateSlide(string ChurchMediaId, IFormFile mediaFile, string mediaType, String EditSlideShowTabName, string MediaUrl, string MediaName)
        {
            try
            {
                MediaChurch mediaupdate = new MediaChurch();
                if (mediaFile != null)
                {
                    mediaupdate.MediaURL = FileUploadUtility.UploadFile(mediaFile, UploadingAreas.SlideShow, Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId")));
                    mediaupdate.MediaName = mediaFile.FileName.ToString();
                }
                else
                {
                    mediaupdate.MediaURL = MediaUrl;
                    mediaupdate.MediaName = MediaName;

                }

                mediaupdate.ChurchMediaId = Convert.ToInt32(ChurchMediaId);
                mediaupdate.TabName = EditSlideShowTabName;

                int res = mediaChurchDataAccess.UpdateMedia(mediaupdate);
                return Json(res);
            }
            catch (Exception e)
            {
                ShowMessage("Update Slide Show Error" + e.Message);
                throw;
            }
            
        }

        public IActionResult DeleteSlide(int id)
        {
            try
            {
                GenericModel gm = new GenericModel();
                bool res = mediaChurchDataAccess.DeleteMedia(id);
                return Json(res);
            }
            catch (Exception e)
            {
                ShowMessage("Delete Slide Show Error" + e.Message);
                throw;
            }
            
        }


        #endregion
        private void ShowMessage(string exceptionMessage)
        {
            log.Info("Exception: " + exceptionMessage);
        }
    }
}
