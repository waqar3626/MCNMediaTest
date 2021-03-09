using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MCNMedia_Dev.Controllers
{
    public class NoticeController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        NoticeDataAccessLayer noticeDataAccess = new NoticeDataAccessLayer();
        GenericModel gm = new GenericModel();

        public IActionResult Index()
        {
            try
            {
                GenericModel gm = new GenericModel();
                return View(gm);
            }
            catch (Exception e)
            {
                ShowMesage("Index Errors : " + e.Message);
                throw;
            }
        }

        [HttpPost]
        public JsonResult AddNotice(string NoticeTitle, string NoticeName)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return Json(-1);
                }
                Notice notice = new Notice();
                notice.ChurchId = (int)HttpContext.Session.GetInt32("ChurchId");
                notice.UpdatedBy = (int)HttpContext.Session.GetInt32("UserId");
                notice.NoticeTitle = NoticeTitle;
                notice.NoticeName = NoticeName;


                if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
                {
    
                    int res = noticeDataAccess.AddNotice(notice);
                    return Json(new { success = true, responseText = "The attached file is not supported." });
                }

                return Json(1);
            }
            catch (Exception exp)
            {
                return Json(new { success = false, responseText = exp.Message });
            }
        }

        public JsonResult GetAllNotice()
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return Json(-1);
                }
                Notice notice = new Notice();

                int churchId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));
                List<Notice> noticeInfo = noticeDataAccess.GetAllNotices(churchId).ToList();
                return Json(noticeInfo);

            }
            catch (Exception e)
            {
                ShowMesage("Get All Notice Errors : " + e.Message);
                throw;
            }
        }
        public IActionResult EditNotice(int id)
        {
            try
            {

                GenericModel gm = new GenericModel();
                gm.Notices = noticeDataAccess.GetNoticeById(id);
                return PartialView("_EditNotice", gm);
            }
            catch (Exception e)
            {
                ShowMesage("Edit Notice Errors : " + e.Message);
                throw;
            }
        }
        
        public IActionResult DeleteNotice(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    //return RedirectToAction("UserLogin", "UserLogin");
                    return Json(-1);
                }

                GenericModel gm = new GenericModel();
                int UpdatedBy = (int)HttpContext.Session.GetInt32("UserId");
                bool res = noticeDataAccess.DeleteNotice(id, UpdatedBy);
                return Json(res);
            }
            catch (Exception e)
            {
                ShowMesage("Delete Notice Errors :" + e.Message);
                throw;
            }
        }
        public JsonResult UpdateNotice(int ChurchNoticeId, string NoticeTitle, string NoticeName)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return Json(-1);
                }

                Notice notice = new Notice();
                notice.UpdatedBy = (int)HttpContext.Session.GetInt32("UserId");
                notice.ChurchNoticeId = Convert.ToInt32(ChurchNoticeId);
                notice.NoticeTitle = NoticeTitle;
                notice.NoticeName = NoticeName;
              
                int res = noticeDataAccess.UpdateNotice(notice);
                return Json(new { success = true, responseText = "The attached file is not supported." });
            }

            catch (Exception e)
            {
                return Json(new { success = false, responseText = e.Message });
            }

        }

        private void ShowMesage(String exceptionMessage)
        {
            log.Error("Exception : " + exceptionMessage);
        }

    }
}
