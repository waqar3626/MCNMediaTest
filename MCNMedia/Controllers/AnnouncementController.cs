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
    public class AnnouncementController : Controller
    {
        AnnouncementDataAccessLayer AnnouncementDataAccessLayer = new AnnouncementDataAccessLayer();
        public IActionResult Index()
        {
            return View();
        }
        public JsonResult ListAnnouncement()
        {
            Announcement announcement = new Announcement();
            int churchId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));

            List<Announcement> announcementsList = AnnouncementDataAccessLayer.GetAnnouncement(churchId).ToList();
            return Json(announcementsList);
        }
        [HttpPost]
        public IActionResult AddAnnouncement(string announceTittle, String announceText)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
            {
                Announcement announcement = new Announcement();
                announcement.ChurchId = (int)HttpContext.Session.GetInt32("ChurchId");

                announcement.AnnouncementTitle = announceTittle;
                announcement.AnnouncementText = announceText;


                int res = AnnouncementDataAccessLayer.AddAnnouncement(announcement);


                return Json(res);
            }
            return RedirectToAction("Listchurch", "Church");
        }


        public IActionResult EditAnnouncement(int id)
        {

            GenericModel gm = new GenericModel();
            gm.Announcement = AnnouncementDataAccessLayer.GetAnnouncementById(id);
            return PartialView("_EditAnnouncement", gm);


        }

        public IActionResult DeleteAnnouncement(int id)
        {

            GenericModel gm = new GenericModel();
            bool res = AnnouncementDataAccessLayer.DeleteAnnouncement(id);
            return Json(res);
        }

        public JsonResult UpdateAnnouncement(int churchAnnounceId, string editAnnounceTitle, string editAnnounceText)
        {
            Announcement announcement = new Announcement();
            announcement.ChurchAnnouncementId = Convert.ToInt32(churchAnnounceId);
            announcement.AnnouncementTitle = editAnnounceTitle;
            announcement.AnnouncementText = editAnnounceText;

            int res = AnnouncementDataAccessLayer.UpdateAnnouncement(announcement);


            return Json(res);
        }

  
    }
}
