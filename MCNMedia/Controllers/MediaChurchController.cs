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
        MediaChurchDataAccessLayer mediaChurchDataAccess = new MediaChurchDataAccessLayer();
        GenericModel gm = new GenericModel();

        private IWebHostEnvironment environment;

        public MediaChurchController(IWebHostEnvironment _environment)
        {
            environment = _environment;
        }

        public IActionResult Index()
        {
            GenericModel gm = new GenericModel();
            // gm.ListMedia = medchurchDataAccess.GetByMediaType();
            return View(gm);
        }

        #region "Picture"

        [HttpPost]
        public JsonResult AddMedia(string mediaType, string AddPicTabName, IFormFile mediaFile)
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

        public JsonResult GetMediaByTypeId(string medType)
        {
            int churchId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));
            List<MediaChurch> medInfo = mediaChurchDataAccess.GetByMediaType(medType, churchId).ToList();
            return Json(medInfo);

        }

        public IActionResult EditMedia(int id)
        {
            GenericModel gm = new GenericModel();
            gm.Media = mediaChurchDataAccess.GetMediaById(id);
            //  return View(gm);
            return PartialView("EditPicture", gm);
        }

        public JsonResult UpdateMedia(String ChurchMediaId, IFormFile mediaFile, string mediaType, String EditPictureTabName)
        {
            MediaChurch mediaupdate = new MediaChurch();
            mediaupdate.ChurchMediaId = Convert.ToInt32(ChurchMediaId);
            mediaupdate.TabName = EditPictureTabName;
            mediaupdate.MediaURL = FileUploadUtility.UploadFile(mediaFile, UploadingAreas.Picture, Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId")));
            mediaupdate.MediaType = mediaType;
            mediaupdate.MediaName = mediaFile.FileName.ToString();
            int res = mediaChurchDataAccess.UpdateMedia(mediaupdate);


            return Json(res);
        }

        public IActionResult DeleteMedia(int id)
        {

            GenericModel gm = new GenericModel();
            bool res = mediaChurchDataAccess.DeleteMedia(id);
            return Json(res);
        }

        #endregion

        #region "Video"

        [HttpPost]
        public IActionResult AddVideo(IFormFile mediaFile, string mediaType, String AddVidTabName)
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

        public JsonResult ListVideo(string medType)
        {
            MediaChurch mdChurch = new MediaChurch();
            int churchId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));

            List<MediaChurch> vidList = mediaChurchDataAccess.GetByMediaType(medType, churchId).ToList();
            return Json(vidList);
        }

        public IActionResult EditVideo(int id)
        {
            GenericModel gm = new GenericModel();
            gm.Media = mediaChurchDataAccess.GetMediaById(id);
            //return View(gm);
            return PartialView("_EditVideo", gm);
        }

        public JsonResult UpdateVideo(int ChurchMediaId, IFormFile mediaFile, string mediaType, String EditVidTabName)
        {
            MediaChurch mediaupdate = new MediaChurch();
            mediaupdate.ChurchMediaId = Convert.ToInt32(ChurchMediaId);
            mediaupdate.TabName = EditVidTabName;
            mediaupdate.MediaURL = FileUploadUtility.UploadFile(mediaFile, UploadingAreas.Video, Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"))); // Church Id would be needed
            mediaupdate.MediaType = mediaType;
            mediaupdate.MediaName = mediaFile.FileName.ToString();
            int res = mediaChurchDataAccess.UpdateMedia(mediaupdate);
            return Json(res);
        }

        public IActionResult DeleteVideo(int id)
        {
            GenericModel gm = new GenericModel();
            bool res = mediaChurchDataAccess.DeleteMedia(id);
            return Json(res);
        }

        #endregion

        #region "Slideshow"

        [HttpPost]
        public JsonResult AddSlide(string mediaType, string AddSlideshowTabName, IFormFile mediaFile)
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

        public JsonResult GetSlideShowByTypeId(string medType)
        {
            int churchId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));
            List<MediaChurch> slideInfo = mediaChurchDataAccess.GetByMediaType(medType, churchId).ToList();
            return Json(slideInfo);

        }

        public IActionResult EditSlide(int id)
        {
            GenericModel gm = new GenericModel();
            gm.Media = mediaChurchDataAccess.GetMediaById(id);
            //  return View(gm);
            return PartialView("_EditSlideShow", gm);
        }

        public JsonResult UpdateSlide(string ChurchMediaId, IFormFile mediaFile, string mediaType, String EditSlideShowTabName)
        {
            MediaChurch mediaupdate = new MediaChurch();
            mediaupdate.ChurchMediaId = Convert.ToInt32(ChurchMediaId);
            mediaupdate.TabName = EditSlideShowTabName;
            mediaupdate.MediaURL = FileUploadUtility.UploadFile(mediaFile, UploadingAreas.SlideShow, Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId")));
            mediaupdate.MediaName = mediaFile.FileName.ToString();
            int res = mediaChurchDataAccess.UpdateMedia(mediaupdate);
            return Json(res);
        }

        public IActionResult DeleteSlide(int id)
        {

            GenericModel gm = new GenericModel();
            bool res = mediaChurchDataAccess.DeleteMedia(id);
            return Json(res);
        }


        #endregion

    }
}
