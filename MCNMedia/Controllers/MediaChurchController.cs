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

namespace MCNMedia_Dev.Controllers
{
    public class MediaChurchController : Controller
    {
        MediaChurchDataAccessLayer medchurchDataAccess = new MediaChurchDataAccessLayer();
        GenericModel gm = new GenericModel();

        private IHostingEnvironment environment;

        public MediaChurchController(IHostingEnvironment _environment)
        {
            environment = _environment;
        }
        private string MediaReturn(IFormFile MediaURl3)
        {
            string contentPath = this.environment.ContentRootPath;
            string dirPath = Path.Combine("Uploads", "Pictures");
            string path = Path.Combine(contentPath, dirPath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName = Guid.NewGuid().ToString() +
            System.IO.Path.GetExtension(MediaURl3.FileName);
            //string fileName = Path.GetFileName(imageURl2.FileName);
            using (var stream = System.IO.File.Create(Path.Combine(path, fileName)))
            {
                MediaURl3.CopyTo(stream);
                ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
            }
            return Path.Combine(dirPath, fileName);
        }

        public IActionResult Index()
        {

            GenericModel gm = new GenericModel();
           // gm.ListMedia = medchurchDataAccess.GetByMediaType();
            return View(gm);
        }

        public JsonResult ListVideo(string medType)
        {
            MediaChurch mdChurch = new MediaChurch();
            int churchId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));

            List<MediaChurch> vidList = medchurchDataAccess.GetByMediaType(medType, churchId).ToList();
            return Json(vidList);
        }
        public IActionResult EditVideo(int id)
        {

            GenericModel gm = new GenericModel();
            gm.Media = medchurchDataAccess.GetMediaById(id);
            //return View(gm);
            return PartialView("_EditVideo", gm);


        }
        [HttpPost]
        public IActionResult AddVideo(IFormFile mediaFile, string mediaType, String AddVidTabName)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
            {
                MediaChurch media = new MediaChurch();
                media.ChurchId = (int)HttpContext.Session.GetInt32("ChurchId");
                media.MediaURL = MediaReturn(mediaFile);
                media.MediaType = mediaType;
                media.MediaName = mediaFile.FileName.ToString();
                media.TabName = AddVidTabName;


                int res = medchurchDataAccess.AddMedia(media);


                return Json(res);
            }
            return RedirectToAction("Listchurch", "Church");
        }
        public IActionResult DeleteVideo(int id)
        {

            GenericModel gm = new GenericModel();
            bool res = medchurchDataAccess.DeleteMedia(id);
            return Json(res);
        }

        public JsonResult UpdateVideo(int ChurchMediaId, IFormFile mediaFile, string mediaType, String EditVidTabName)
        {
            MediaChurch mediaupdate = new MediaChurch();
            mediaupdate.ChurchMediaId = Convert.ToInt32(ChurchMediaId);
            mediaupdate.TabName = EditVidTabName;
            mediaupdate.MediaURL = MediaReturn(mediaFile);
            mediaupdate.MediaType = mediaType;
            mediaupdate.MediaName = mediaFile.FileName.ToString();
            int res = medchurchDataAccess.UpdateMedia(mediaupdate);


            return Json(res);
        }

        [HttpPost]
        public JsonResult AddMedia(string mediaType, string AddPicTabName, IFormFile mediaFile)
        {
            MediaChurch mediaChurch = new MediaChurch();

            mediaChurch.ChurchId = (int)HttpContext.Session.GetInt32("ChurchId");
            mediaChurch.MediaName = Path.GetFileName(mediaFile.FileName);
            mediaChurch.MediaURL = MediaReturn(mediaFile);
            mediaChurch.MediaType = mediaType;
            mediaChurch.TabName = AddPicTabName;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
            {
                int res = medchurchDataAccess.AddMedia(mediaChurch);
            }
            return Json(1);
        }
        [HttpPost]
        public JsonResult AddSlide(string mediaType, string AddSlideshowTabName, IFormFile mediaFile)
        {
            MediaChurch mediaChurch = new MediaChurch();
            mediaChurch.ChurchId = (int)HttpContext.Session.GetInt32("ChurchId");
            mediaChurch.MediaName = Path.GetFileName(mediaFile.FileName);
            mediaChurch.MediaURL = MediaReturn(mediaFile);
            mediaChurch.MediaType = mediaType;
            mediaChurch.TabName = AddSlideshowTabName;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
            {
                int res = medchurchDataAccess.AddMedia(mediaChurch);
            }
            return Json(1);
        }
        public IActionResult EditMedia(int id)
        {
            GenericModel gm = new GenericModel();
            gm.Media = medchurchDataAccess.GetMediaById(id);
            //  return View(gm);
            return PartialView("EditPicture", gm);
        }

        public IActionResult EditSlide(int id)
        {
            GenericModel gm = new GenericModel();
            gm.Media = medchurchDataAccess.GetMediaById(id);
            //  return View(gm);
            return PartialView("_EditSlideShow", gm);
        }
        public IActionResult DeleteMedia(int id)
        {

            GenericModel gm = new GenericModel();
            bool res = medchurchDataAccess.DeleteMedia(id);
            return Json(res);
        }
        public IActionResult DeleteSlide(int id)
        {

            GenericModel gm = new GenericModel();
            bool res = medchurchDataAccess.DeleteMedia(id);
            return Json(res);
        }

        public JsonResult GetMediaByTypeId(string medType)
        {
            int churchId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));
            List<MediaChurch> medInfo = medchurchDataAccess.GetByMediaType(medType, churchId).ToList();
            return Json(medInfo);

        }
        public JsonResult GetSlideShowByTypeId(string medType)
        {
            int churchId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));
            List<MediaChurch> slideInfo = medchurchDataAccess.GetByMediaType(medType, churchId).ToList();
            return Json(slideInfo);

        }

        public JsonResult UpdateMedia(String ChurchMediaId, IFormFile mediaFile, string mediaType, String EditPictureTabName)
        {
            MediaChurch mediaupdate = new MediaChurch();
            mediaupdate.ChurchMediaId = Convert.ToInt32(ChurchMediaId);
            mediaupdate.TabName = EditPictureTabName;
            mediaupdate.MediaURL = MediaReturn(mediaFile);
            mediaupdate.MediaType = mediaType;
            mediaupdate.MediaName = mediaFile.FileName.ToString();
            int res = medchurchDataAccess.UpdateMedia(mediaupdate);


            return Json(res);
        }
        public JsonResult UpdateSlide(string ChurchMediaId, IFormFile mediaFile, string mediaType, String EditSlideShowTabName)
        {
            MediaChurch mediaupdate = new MediaChurch();
            mediaupdate.ChurchMediaId = Convert.ToInt32(ChurchMediaId);
            mediaupdate.TabName = EditSlideShowTabName;
            mediaupdate.MediaURL = MediaReturn(mediaFile);
            mediaupdate.MediaType = mediaType;
            mediaupdate.MediaName = mediaFile.FileName.ToString();
            int res = medchurchDataAccess.UpdateMedia(mediaupdate);


            return Json(res);
        }
    }
}
