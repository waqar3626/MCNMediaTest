using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev._Helper;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
 

namespace MCNMedia_Dev.Controllers
{
    public class ChurchNewsLetterController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ChurchNewsLetterDataAccessLayer churchNewsLetterDataAccess = new ChurchNewsLetterDataAccessLayer();

        GenericModel gm = new GenericModel();

        private IWebHostEnvironment environment;

        public ChurchNewsLetterController(IWebHostEnvironment _environment)
        {
            environment = _environment;
        }
        [HttpPost]
       
       
        [RequestFormLimits(MultipartBodyLengthLimit  = 209715200)]
        public JsonResult AddNewsLetter(string AddChurchNewsLetterTitle, IFormFile mediaFile, bool ShowOnWebsite)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
                {
                    NewsLetter chnewsLetter = new NewsLetter();
                    chnewsLetter.ChurchId = (int)HttpContext.Session.GetInt32("ChurchId");
                    chnewsLetter.UpdatedBy = (int)HttpContext.Session.GetInt32("UserId");
                    chnewsLetter.NewsLetterName = Path.GetFileName(mediaFile.FileName);
                    chnewsLetter.NewsLetterUrl = FileUploadUtility.UploadFile(mediaFile, UploadingAreas.NewsLetter, Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId")));
                    chnewsLetter.NewsLetterTitle = AddChurchNewsLetterTitle;
                    chnewsLetter.ShowOnWebsite = ShowOnWebsite;


                    int res = churchNewsLetterDataAccess.AddNewsLetter(chnewsLetter);
                }
                return Json(1);
            }
            catch (Exception e)
            {
                ShowMessage("Add Church NewsLetter Error" + e.Message);
                throw;
            }
        }

        [HttpGet]
        public JsonResult NewsletterList()
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    //return Json(new { redirecturl = Url.Action("UserLogin") });
                    //return Json({ redirectUrl = Url.Action("Index", "Home"), isRedirect = true });
                    //return Json(new
                    //{
                    //    redirectUrl = Url.Action("~/Admin/Churches"),
                    //    isRedirect = true
                    //});
                    //return Json( RedirectToAction("Listchurch", "Church"));
                    return Json(new
                    {
                        redirectUrl = Url.Action("~/UserLogin"),
                        isRedirect = true
                    });
                    //    RedirectToAction("UserLogin", "UserLogin");
                    //    return Json(-1);
                }
                int churchId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));
                List<NewsLetter> slideInfo = churchNewsLetterDataAccess.GetNewsLetterByChurch(churchId).ToList();
                return Json(slideInfo);
            }
            catch (Exception e)
            {
                ShowMessage("Get Specific NewsLetter Error" + e.Message);
                throw;
            }
        }
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        public IActionResult EditNewsLetter(int id)
        {
            try
            {
                GenericModel gm = new GenericModel();
                gm.ChurchNewsLetter = churchNewsLetterDataAccess.GetNewsLetterById(id);
                return PartialView("_EditChurchNewsLetter", gm);
            }
            catch (Exception e)
            {
                ShowMessage("Edit NewsLetter Show Error" + e.Message);
                throw;
            }
        }

        public JsonResult UpdateNewsLetter(string ChurchNewsLetterId, IFormFile mediaFile, bool ShowOnWebsite, string EditNewsLetterTitle, string NewsLetterUrl, string NewsLetterName)
        {
            try
            {
                NewsLetter chnewsLetter = new NewsLetter();
                if (mediaFile != null)
                {
                    chnewsLetter.NewsLetterUrl = FileUploadUtility.UploadFile(mediaFile, UploadingAreas.NewsLetter, Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId")));
                    chnewsLetter.NewsLetterName = mediaFile.FileName.ToString();
                }
                else
                {
                    chnewsLetter.NewsLetterUrl = NewsLetterUrl;
                    chnewsLetter.NewsLetterName = NewsLetterName;
                }

                chnewsLetter.ChurchNewsLetterId = Convert.ToInt32(ChurchNewsLetterId);
                chnewsLetter.NewsLetterTitle = EditNewsLetterTitle;
                chnewsLetter.ShowOnWebsite = ShowOnWebsite;
                chnewsLetter.UpdatedBy = (int)HttpContext.Session.GetInt32("UserId");
                int res = churchNewsLetterDataAccess.UpdateNewsLetter(chnewsLetter);
                return Json(res);
            }
            catch (Exception e)
            {
                ShowMessage("Update NewsLetter Error" + e.Message);
                throw;
            }
        }

        public IActionResult DeleteNewsLetter(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return Json(-1);
                }
                GenericModel gm = new GenericModel();
                int UpdateBy = (int)HttpContext.Session.GetInt32("UserId");
                bool res = churchNewsLetterDataAccess.DeleteNewsLetter(id, UpdateBy);
                return Json(res);
            }
            catch (Exception e)
            {
                ShowMessage("Delete Slide Show Error" + e.Message);
                throw;
            }
        }

        private void ShowMessage(String exceptionMessage)
        {
            log.Error("Exception : " + exceptionMessage);
        }
    }
}
