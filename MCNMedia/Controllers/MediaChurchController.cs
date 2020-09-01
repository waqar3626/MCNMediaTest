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
        private string ImageReturn(IFormFile ImageURl3)
        {
            string contentPath = this.environment.ContentRootPath;
            string dirPath = Path.Combine("Uploads", "Pictures");
            string path = Path.Combine(contentPath, dirPath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName = Guid.NewGuid().ToString() +
            System.IO.Path.GetExtension(ImageURl3.FileName);
            //string fileName = Path.GetFileName(imageURl2.FileName);
            using (var stream = System.IO.File.Create(Path.Combine(path, fileName)))
            {
                ImageURl3.CopyTo(stream);
                ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
            }
            return Path.Combine(dirPath, fileName);
        }

        public IActionResult Index()
        {

            GenericModel gm = new GenericModel();
            gm.ListPicture = medchurchDataAccess.GetPictureByMediaType();
            return View(gm);
        }

        [HttpPost]
        public IActionResult AddMedia(string mediaType, string AddPicTabName, IFormFile mediaFile)
        {
            MediaChurch mediaChurch = new MediaChurch();
            mediaChurch.MediaName = Path.GetFileName(mediaFile.FileName);
            mediaChurch.MediaURL = ImageReturn(mediaFile);
            mediaChurch.MediaType = mediaType;
            mediaChurch.TabName = AddPicTabName;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
            {
                int res = medchurchDataAccess.AddMedia(mediaChurch);
            }
            return RedirectToAction("Listchurch", "Church");
        }

        public IActionResult EditPicture(int id)
        {
            GenericModel gm = new GenericModel();
            gm.Pictures = medchurchDataAccess.GetPictureById(id);
            //  return View(gm);
            return PartialView("EditPicture", gm);
        }
        public IActionResult DeletePicture(int id)
        {

            GenericModel gm = new GenericModel();
            bool res = medchurchDataAccess.DeletePicture(id);
            return Json(res);
        }

        public JsonResult GetPictureByTypeId()
        {
            List<MediaChurch> pictureInfo = medchurchDataAccess.GetPictureByMediaType().ToList();
            return Json(pictureInfo);
            // return Json(new { data = cameraInfo });

        }

        public JsonResult UpdatePicture(string ChurchMediaId, string TabName, string MediaURL, IFormFile ImageURl3)
        {
            //pic.Pictures.ImageURl = ImageReturn(ImageURl3); 
            MediaChurch pictureUpdate = new MediaChurch();
            pictureUpdate.ChurchMediaId = Convert.ToInt32(ChurchMediaId);
            //pictureUpdate.MediaName = MediaName;
            pictureUpdate.TabName = TabName;
            pictureUpdate.MediaURL = MediaURL;
            //pictureUpdate.MediaType = MediaType;

            int res = medchurchDataAccess.UpdatePicture(pictureUpdate);


            return Json(res);
        }
    }
}
