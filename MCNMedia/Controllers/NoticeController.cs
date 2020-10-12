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
        NoticeDataAccessLayer noticeDataAccess = new NoticeDataAccessLayer();
        GenericModel gm = new GenericModel();

        public IActionResult Index()
        {
            GenericModel gm = new GenericModel();
            return View(gm);
        }

        [HttpPost]
        public JsonResult AddNotice(string NoticeTitle, string NoticeName)
        {

            Notice notice = new Notice();
            notice.ChurchId = (int)HttpContext.Session.GetInt32("ChurchId");
            notice.NoticeTitle = NoticeTitle;
            notice.NoticeName = NoticeName;


            if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
            {
                int res = noticeDataAccess.AddNotice(notice);
            }
            return Json(1);
        }

        public JsonResult GetAllNotice()
        {
            Notice notice = new Notice();

            int churchId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));
            List<Notice> noticeInfo = noticeDataAccess.GetAllNotices(churchId).ToList();
            return Json(noticeInfo);
           
        }
        public IActionResult EditNotice(int id)
        {
            GenericModel gm = new GenericModel();
            gm.Notices = noticeDataAccess.GetNoticeById(id);
            return PartialView("_EditNotice", gm);
        }
        
        public IActionResult DeleteNotice(int id)
        {

            GenericModel gm = new GenericModel();
            bool res = noticeDataAccess.DeleteNotice(id);
            return Json(res);
        }
        public JsonResult UpdateNotice(int ChurchNoticeId, string NoticeTitle, string NoticeName)
        {
            Notice notice = new Notice();
            notice.ChurchNoticeId = Convert.ToInt32(ChurchNoticeId);
            notice.NoticeTitle = NoticeTitle;
            notice.NoticeName = NoticeName;
            int res = noticeDataAccess.UpdateNotice(notice);


            return Json(res);
        }
    }
}
