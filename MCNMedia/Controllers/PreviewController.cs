using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Mvc;

namespace MCNMedia_Dev.Controllers
{
    public class PreviewController : Controller
    {
        PreviewChurchesDataAccessLayer previewChurchesDataAccessLayer = new PreviewChurchesDataAccessLayer();
        GenericModel gm = new GenericModel();
        public IActionResult Preview(int chId)
        {
        
            gm.Churches = previewChurchesDataAccessLayer.GetPreviewChurch(chId);
            gm.LCameras = previewChurchesDataAccessLayer.GetAllPreviewCameras(chId);
            gm.LSchedules = previewChurchesDataAccessLayer.GetAllPreviewSchedule(chId);
            gm.LRecordings = previewChurchesDataAccessLayer.GetAllPreviewRecording(chId);
            return View(gm);
        }
        public IActionResult Preview2()
        {
            return View();
        }
    }
}
