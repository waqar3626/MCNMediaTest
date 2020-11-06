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
    public class PreviewController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        PreviewChurchesDataAccessLayer previewChurchesDataAccessLayer = new PreviewChurchesDataAccessLayer();
        NoticeDataAccessLayer NoticeDataAccessLayer = new NoticeDataAccessLayer();
        AnnouncementDataAccessLayer AnnouncementDataAccessLayer = new AnnouncementDataAccessLayer();
        GenericModel gm = new GenericModel();
        public IActionResult Preview(int chId)
        {
            try
            {
                gm.Churches = previewChurchesDataAccessLayer.GetPreviewChurch(chId);
                gm.LCameras = previewChurchesDataAccessLayer.GetAllPreviewCameras(chId);
                gm.LSchedules = previewChurchesDataAccessLayer.GetSchedulePreviewWeekly(chId);
                gm.LRecordings = previewChurchesDataAccessLayer.GetAllPreviewRecording(chId);
                gm.ListNotice = NoticeDataAccessLayer.GetAllNotices(chId);
                gm.LAnnouncement = AnnouncementDataAccessLayer.GetAnnouncement(chId);
                return View(gm);
            }
            catch (Exception e)
            {
                ShowMesage("Preview Errors : " + e.Message);
                throw;
            }
        
            
        }
        private void ShowMesage(String exceptionMessage)
        {
            log.Error("Exception : " + exceptionMessage);
        }
    }
}

