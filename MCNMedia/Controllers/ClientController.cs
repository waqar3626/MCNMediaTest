using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Crmf;

namespace MCNMedia_Dev.Controllers
{
    public class ClientController : Controller
    {
        ChurchDataAccessLayer chdataAccess = new ChurchDataAccessLayer();
        AnnouncementDataAccessLayer AnnouncementDataAccessLayer = new AnnouncementDataAccessLayer();
        scheduleDataAccessLayer scheduleDataAccess = new scheduleDataAccessLayer();

        #region church info

        //[HttpGet]
        //public IActionResult ChurchInfo(int churchId)
        //{
        //    Church church = chdataAccess.GetChurchData(1);
        //    if (church == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(church);
        //} 
        [HttpGet]
        public IActionResult ChurchInfo(int churchId)
        {
            Church church = chdataAccess.GetChurchData(churchId);
            if (church == null)
            {
                return NotFound();
            }
            return View(church);
        }

        public IActionResult UpdateChurchInfo([Bind] Church church)
        {

            chdataAccess.UpdateChurch(church);
            return RedirectToAction("Client");

        }
        #endregion

        #region Announcement
        public IActionResult Announcement(int id)
        {
            GenericModel gm = new GenericModel();
            gm.Announcement = AnnouncementDataAccessLayer.GetAnnouncementById(1);
            gm.Churches = chdataAccess.GetChurchData(1);
            return View(gm);
        }

        public IActionResult UpdateAnnouncement([Bind] Announcement announcement)
        {

            AnnouncementDataAccessLayer.UpdateAnnouncement(announcement);
            return RedirectToAction("Announcement");

        }
        #endregion

        #region Camera
        public IActionResult CameraDetail()
        {
            return View();
        }
        #endregion

        #region StreamToFaceBook
        public IActionResult StreamToFaceBook()
        {
            return View();
        }
        #endregion

        #region ScheduleAndRecording
        //public IActionResult ScheduleAndRecording()
        //{
        //    return View();
        //}

        [HttpGet]
        public IActionResult ScheduleAndRecording()
        {
            GenericModel gm = new GenericModel();
            gm.LSchedules = scheduleDataAccess.GetAllSchedule();
            gm.Churches = chdataAccess.GetChurchData(1);
            Redirect("ScheduleAndRecording");
            return View(gm);
        }

        #endregion
    }
}
