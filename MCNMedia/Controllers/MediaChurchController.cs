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

        [HttpPost]
        public JsonResult AddMedia(string mediaType, string AddPicTabName, IFormFile mediaFile)
        {
            MediaChurch mediaChurch = new MediaChurch();
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
        public IActionResult DeletePicture(int id)
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
            List<MediaChurch> medInfo = medchurchDataAccess.GetByMediaType(medType).ToList();
            return Json(medInfo);

        }
        public JsonResult GetSlideShowByTypeId(string medType)
        {
            List<MediaChurch> slideInfo = medchurchDataAccess.GetByMediaType(medType).ToList();
            return Json(slideInfo);

        }

        //public JsonResult UpdateMedia(string ChurchMediaId, string TabName, string MediaURL, IFormFile ImageURl3)
        //{
        //    //pic.Pictures.ImageURl = ImageReturn(ImageURl3); 
        //    MediaChurch medUpdate = new MediaChurch();
        //    medUpdate.ChurchMediaId = Convert.ToInt32(ChurchMediaId);
        //    //pictureUpdate.MediaName = MediaName;
        //    medUpdate.TabName = TabName;
        //    medUpdate.MediaURL = MediaURL;
        //    //pictureUpdate.MediaType = MediaType;

        //    int res = medchurchDataAccess.UpdateMedia(medUpdate);


        //    return Json(res);
        //}

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
