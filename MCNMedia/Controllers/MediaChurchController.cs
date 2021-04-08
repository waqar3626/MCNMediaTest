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
using BitMiracle.Docotic.Pdf;
using Microsoft.Extensions.Configuration;

namespace MCNMedia_Dev.Controllers
{
    public class MediaChurchController : Controller
    {
        private readonly string AWS_S3_BUCKET_URI;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        MediaChurchDataAccessLayer mediaChurchDataAccess = new MediaChurchDataAccessLayer();
        GenericModel gm = new GenericModel();

        private IWebHostEnvironment environment;

        public MediaChurchController(IWebHostEnvironment _environment)
        {
            environment = _environment;
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            var root = builder.Build();
            var awsS3bucket = root.GetSection("S3BucketConfiguration");
            var sysConfig = root.GetSection("SystemConfiguration");
            AWS_S3_BUCKET_URI = $"{awsS3bucket["aws_bucket_url"]}/{sysConfig["system_mode"]}";
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
                return View(gm);
            }

        }

        #region "Picture"

        [HttpPost]
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = 15728640)]
        public JsonResult AddMedia(string mediaType, string AddPicTabName, IFormFile mediaFile)
        {
            try
            {


                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return Json(-1);
                }
                MediaChurch mediaChurch = new MediaChurch();
                if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
                {
                    mediaChurch.ChurchId = (int)HttpContext.Session.GetInt32("ChurchId");
                    mediaChurch.UpdatedBy = (int)HttpContext.Session.GetInt32("UserId");
                    mediaChurch.MediaName = Path.GetFileName(mediaFile.FileName);
                    mediaChurch.MediaURL = FileUploadUtility.UploadFile(mediaFile, UploadingAreas.Picture, mediaChurch.ChurchId);
                    mediaChurch.MediaType = mediaType;
                    mediaChurch.TabName = AddPicTabName;

                    int res = mediaChurchDataAccess.AddMedia(mediaChurch);
                    return Json(new { success = true });
                }

                return Json(1);
            }
            catch (Exception exp)
            {
                return Json(new { success = false, responseText = exp.Message });
            }
        }


        public JsonResult GetMediaByTypeId(string medType)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return Json(-1);
                }
                int churchId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));
                List<MediaChurch> medInfo = mediaChurchDataAccess.GetByMediaType(medType, churchId).ToList();
                return Json(medInfo);
            }
            catch (Exception e)
            {
                return Json(new { success = false, responseText = e.Message });
            }


        }

        public IActionResult EditMedia(int id)
        {
            GenericModel gm = new GenericModel();
            try
            {

                gm.Media = mediaChurchDataAccess.GetMediaById(id);
                return PartialView("EditPicture", gm);
            }
            catch (Exception exp)
            {
                return PartialView("EditPicture", gm);
            }


        }
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = 15728640)]

        public JsonResult UpdateMedia(String ChurchMediaId, string mediaType, String EditPictureTabName, string MediaUrl, string MediaName)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return Json(-1);
                }
                //if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                //{
                //    return Json(new { Url = "UserLogin" });
                //}
                MediaChurch mediaupdate = new MediaChurch();


                mediaupdate.MediaURL = MediaUrl;
                mediaupdate.MediaName = MediaName;


                mediaupdate.ChurchMediaId = Convert.ToInt32(ChurchMediaId);
                mediaupdate.TabName = EditPictureTabName;
                mediaupdate.MediaType = mediaType;

                int res = 0;
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    mediaupdate.UpdatedBy = (int)HttpContext.Session.GetInt32("UserId");
                    res = mediaChurchDataAccess.UpdateMedia(mediaupdate);
                }

                return Json(new { success = true, responseText = "The attached file is not supported." });

            }
            catch (Exception e)
            {
                return Json(new { success = false, responseText = e.Message });
            }
        }

        public JsonResult DeleteMedia(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return Json(new { Url = "UserLogin" });
                }
                GenericModel gm = new GenericModel();
                int UpdateBy = (int)HttpContext.Session.GetInt32("UserId");
                int res = mediaChurchDataAccess.DeleteMedia(id, UpdateBy);
                return Json(new { success = true });
            }
            catch (Exception e)
            {
                return Json(new { success = false, responseText = e.Message });
            }


        }

        #endregion

        #region "Video"

        [HttpPost]
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = 2147483647)]

        public IActionResult AddVideo(IFormFile mediaFile, string mediaType, String AddVidTabName)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return RedirectToAction("Listchurch", "Church");
                }
                if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
                {
                    MediaChurch media = new MediaChurch();
                    media.ChurchId = (int)HttpContext.Session.GetInt32("ChurchId");
                    media.UpdatedBy = (int)HttpContext.Session.GetInt32("UserId");
                    media.MediaURL = FileUploadUtility.UploadFile(mediaFile, UploadingAreas.Video, media.ChurchId);
                    media.MediaType = mediaType;
                    media.MediaName = mediaFile.FileName.ToString();
                    media.TabName = AddVidTabName;

                    int res = mediaChurchDataAccess.AddMedia(media);

                    return Json(new { success = true, responseText = "The attached file is not supported." });
                }
                return RedirectToAction("Listchurch", "Church");
            }
            catch (Exception e)
            {
                return Json(new { success = false, responseText = e.Message });
            }
        }

        public JsonResult ListVideo(string medType)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return Json(-1);
                }
                MediaChurch mdChurch = new MediaChurch();
                int churchId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));

                List<MediaChurch> vidList = mediaChurchDataAccess.GetByMediaType(medType, churchId).ToList();
                return Json(vidList);
            }
            catch (Exception exp)
            {
                return Json(new { success = false, responseText = exp.Message });
            }


        }

        public IActionResult EditVideo(int id)
        {
            GenericModel gm = new GenericModel();
            try
            {

                gm.Media = mediaChurchDataAccess.GetMediaById(id);
                return PartialView("_EditVideo", gm);
            }
            catch (Exception e)
            {
                return PartialView("_EditVideo", gm);
            }


        }
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = 2147483647)]
        public JsonResult UpdateVideo(int ChurchMediaId, string mediaType, String EditVidTabName, string MediaUrl, string MediaName)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return Json(-1);
                }
                MediaChurch mediaupdate = new MediaChurch();

                mediaupdate.MediaURL = MediaUrl;
                mediaupdate.MediaName = MediaName;

                mediaupdate.ChurchMediaId = Convert.ToInt32(ChurchMediaId);
                mediaupdate.TabName = EditVidTabName;
                mediaupdate.MediaType = mediaType;
                mediaupdate.UpdatedBy = (int)HttpContext.Session.GetInt32("UserId");
                int res = mediaChurchDataAccess.UpdateMedia(mediaupdate);
                return Json(new
                {
                    success = true,
                    res
                });
            }
            catch (Exception exp)
            {
                return Json(new { success = false, responseText = exp.Message });
            }
        }


        public IActionResult DeleteVideo(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return Json(new { Url = "UserLogin" });
                }
                GenericModel gm = new GenericModel();
                int UpdateBy = (int)HttpContext.Session.GetInt32("UserId");
                int res = mediaChurchDataAccess.DeleteMedia(id, UpdateBy);
                return Json(new { success = true, res });
            }
            catch (Exception e)
            {
                return Json(new { success = false, responseText = e.Message });
            }

        }

        #endregion

        #region "Slideshow"

        [HttpPost]

        public JsonResult AddSlide(string mediaType, string AddSlideshowTabName)
        {


            try
            {

                var UploadedFiles = HttpContext.Request.Form.Files;

                if (UploadedFiles.Count != 0) { 
                foreach (var file in UploadedFiles)
                {
                    string Extension = Path.GetExtension(file.FileName);

                    if (Extension.ToLower() == ".jpg" || Extension.ToLower() == ".png" || Extension.ToLower() == ".jpeg" || Extension.ToLower() == ".bmp")
                    {

                    }
                    else
                    {
                        throw new Exception("Invalid File Type Kindly Upload Image only in Format [.jpg , .jpeg , .png , .bmp]");
                    }

                }
                }else
                {
                    throw new Exception("Please select atleast one Image only in Format [.jpg , .jpeg , .png , .bmp]");
                }
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return Json(-1);
                }
                MediaChurch mediaChurch = new MediaChurch();
                mediaChurch.ChurchId = (int)HttpContext.Session.GetInt32("ChurchId");
                mediaChurch.UpdatedBy = (int)HttpContext.Session.GetInt32("UserId");
                mediaChurch.MediaName = "";
                mediaChurch.MediaURL = "";
                mediaChurch.MediaType = mediaType;
                mediaChurch.TabName = AddSlideshowTabName;

                if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
                {

                    int res = mediaChurchDataAccess.AddMedia(mediaChurch);
                    var OrderBy = 1;

                    foreach (var file in UploadedFiles)
                    {
                        string Url = FileUploadUtility.UploadFile(file, UploadingAreas.SlideShowPic, Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId")));
                        int userId = (int)HttpContext.Session.GetInt32("UserId");
                        mediaChurchDataAccess.AddSlideShowImages(res, Url, OrderBy, userId);
                        OrderBy++;
                    }
                    return Json(new { success = true, res });


                }

                return Json(1);
            }
            catch (Exception exp)
            {

                return Json(new { success = false, responseText = exp.Message });
            }
        }

        public JsonResult GetSlideShowImages(string medType)
        {
            try
           
            {
                int churchId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));
                List<MediaChurch> slideInfo = mediaChurchDataAccess.SlideShowImaeGetAll(churchId).ToList();
                return Json(slideInfo);
            }
            catch (Exception exp)
            {
                return Json(new { success = false, responseText = exp.Message });
            }


        }

        [HttpPost]
        public JsonResult ChangeDisplayOrder(int DisplayOrder,int ChurchMediaId,int ImageId)
        {
            try
            {
                int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId").ToString());
                bool res = mediaChurchDataAccess.ChangeSlideShowImageOrder(ImageId, ChurchMediaId, DisplayOrder, UserId);
                return Json(new { success = true,res });
            }
            catch (Exception exp)
            {

                return Json(new { success = false, responseText = exp.Message });
            }


        }

        public IActionResult SlideShowImages(int id)
        {
            List<MediaChurch> mediaChurch = new List<MediaChurch>();
            try
            {

                mediaChurch = mediaChurchDataAccess.SlideShowImaeGetByMediaId(id).ToList();
                return PartialView("_SlideShowImages", mediaChurch);
            }
            catch (Exception e)
            {
                return PartialView("_SlideShowImages", mediaChurch);

            }

        }


        public JsonResult GetSlideShowImagesJson(int churchMediaId)
        {
            try
            {
                int churchId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));
                List<MediaChurch> slideInfo = mediaChurchDataAccess.SlideShowImaeGetByMediaId(churchMediaId).ToList();
                return Json(slideInfo);
            }
            catch (Exception exp)
            {
                return Json(new { success = false, responseText = exp.Message });
            }


        }

        public IActionResult EditSlide(int id)
        {
            GenericModel gm = new GenericModel();
            try
            {

                gm.Media = mediaChurchDataAccess.GetMediaById(id);
                return PartialView("_EditSlideShow", gm);
            }
            catch (Exception e)
            {
                return PartialView("_EditSlideShow", gm);

            }

        }
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        public JsonResult UpdateSlide(string ChurchMediaId, string mediaType, String EditSlideShowTabName, string MediaUrl, string MediaName)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return Json(-1);
                }
                MediaChurch mediaupdate = new MediaChurch();


                mediaupdate.MediaURL = MediaUrl;
                mediaupdate.MediaName = MediaName;

                mediaupdate.ChurchMediaId = Convert.ToInt32(ChurchMediaId);
                mediaupdate.TabName = EditSlideShowTabName;
                mediaupdate.UpdatedBy = (int)HttpContext.Session.GetInt32("UserId");
                int res = mediaChurchDataAccess.UpdateMedia(mediaupdate);
                return Json(new { success = true, responseText = "The attached file is not supported." });

            }
            catch (Exception e)
            {
                return Json(new { success = false, responseText = e.Message });
            }
        }

        public IActionResult DeleteSlide(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return RedirectToAction("Listchurch", "Church");
                }
                GenericModel gm = new GenericModel();
                int UpdateBy = (int)HttpContext.Session.GetInt32("UserId");
                int res = mediaChurchDataAccess.DeleteMedia(id, UpdateBy);
                if (res > 0)
                {

                    int res2 = mediaChurchDataAccess.DeleteSlideShowImages(id, UpdateBy);

                    if (res2 > 0)
                    {
                        return Json(new { success = true, res2 });
                    }
                }

                return Json(1);

            }
            catch (Exception e)
            {
                return Json(new { success = false, responseText = e.Message });
            }
        }

        public IActionResult DeleteSlideshowSingleImage(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return RedirectToAction("Listchurch", "Church");
                }
                GenericModel gm = new GenericModel();
                int UpdateBy = (int)HttpContext.Session.GetInt32("UserId");


                int res2 = mediaChurchDataAccess.DeleteSlideShowSingleImage(id, UpdateBy);


                return Json(new { success = true, res2 });

            }
            catch (Exception e)
            {
                return Json(new { success = false, responseText = e.Message
    });
            }
        }


        [HttpPost]
            public JsonResult AddSingleImageSlideShows(IFormFile mediaFile,int mediaChurchId,int OrderBy)
        {
            try
            {


                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return Json(-1);
                }
                MediaChurch mediaChurch = new MediaChurch();
                if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
                {
                    mediaChurch.ChurchId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId").ToString());
                    mediaChurch.MediaName = Path.GetFileName(mediaFile.FileName);
                    mediaChurch.MediaURL = FileUploadUtility.UploadFile(mediaFile, UploadingAreas.SlideShowPic, mediaChurch.ChurchId);
                    mediaChurch.MediaType = "SlideShow";
                    mediaChurch.TabName = "";
                    int userId = (int)HttpContext.Session.GetInt32("UserId");
                    MediaChurch mdchr= mediaChurchDataAccess.SlideShowImaeGetByMediaId(mediaChurchId).ToList().Last();
                    OrderBy = mdchr.DisplayOrder+1;
                    mediaChurchDataAccess.AddSlideShowImages(mediaChurchId, mediaChurch.MediaURL, OrderBy, userId);
                    return Json(new { success = true });
                }

                return Json(1);
            }
            catch (Exception exp)
            {
                return Json(new { success = false, responseText = exp.Message });
            }
        }


    }


    #endregion


}
