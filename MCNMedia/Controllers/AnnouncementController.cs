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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        AnnouncementDataAccessLayer AnnouncementDataAccessLayer = new AnnouncementDataAccessLayer();
        public IActionResult Index()
        {
            return View();
        }
        public JsonResult ListAnnouncement()
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return Json(-1);
                }
                Announcement announcement = new Announcement();
                int churchId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));

                List<Announcement> announcementsList = AnnouncementDataAccessLayer.GetAnnouncement(churchId).ToList();
                return Json(announcementsList);

            }
            catch (Exception e)
            {
                ShowMessage("List Announcement Error" + e.Message);
                throw;
            }
          
        }
        [HttpPost]
        public IActionResult AddAnnouncement(string announceTittle, String announceText)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
                {
                    Announcement announcement = new Announcement();
                    announcement.ChurchId = (int)HttpContext.Session.GetInt32("ChurchId");

                    announcement.AnnouncementTitle = announceTittle;
                    announcement.AnnouncementText = announceText;
                    announcement.CreatedBy = (int)HttpContext.Session.GetInt32("UserId");
                    int res = AnnouncementDataAccessLayer.AddAnnouncement(announcement);


                    return Json(res);
                }
                return RedirectToAction("UserLogin", "UserLogin");
            }
            catch (Exception e)
            {
                ShowMessage("Add Announcement Error" + e.Message);

                throw;
            }
        
        }


        public IActionResult EditAnnouncement(int id)
        {
            try
            {
                GenericModel gm = new GenericModel();
                gm.Announcement = AnnouncementDataAccessLayer.GetAnnouncementById(id);
                return PartialView("_EditAnnouncement", gm);

            }
            catch (Exception e)
            {
                ShowMessage("Edit Announcement Error" + e.Message);
                throw;
            }

         


        }

        public IActionResult DeleteAnnouncement(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return Json(-1);
                }
                GenericModel gm = new GenericModel();
                int UpdateBy = (int)HttpContext.Session.GetInt32("UserId");
                bool res = AnnouncementDataAccessLayer.DeleteAnnouncement(id,UpdateBy);
                return Json(res);
            }
            catch (Exception e)
            {
              
                ShowMessage("Delete Announcement Error" + e.Message);
                throw;
            }

        }

        public JsonResult UpdateAnnouncement(int churchAnnounceId, string editAnnounceTitle, string editAnnounceText)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return Json(-1);
                }
                Announcement announcement = new Announcement();
                announcement.ChurchAnnouncementId = Convert.ToInt32(churchAnnounceId);
                announcement.AnnouncementTitle = editAnnounceTitle;
                announcement.AnnouncementText = editAnnounceText;
            announcement.UpdatedBy = (int)HttpContext.Session.GetInt32("UserId");
                int res = AnnouncementDataAccessLayer.UpdateAnnouncement(announcement);


                return Json(res);
            }
            catch (Exception e)
            {


                ShowMessage("Update Announcement Error" + e.Message);
                throw;
            }
            
        }

        private void ShowMessage(string exceptionMessage)
        {
            log.Info("Exception: " + exceptionMessage);
        }

    }
}
