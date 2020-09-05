using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Mvc;

namespace MCNMedia_Dev.Controllers
{
    public class ClientController : Controller
    {
        ChurchDataAccessLayer dataAccess = new ChurchDataAccessLayer();
        public IActionResult Client()
        {
            return View();
        }
        public IActionResult Announcement()
        {
            return View();
        }
        public IActionResult CameraDetail()
        {
            return View();
        }
        public IActionResult StreamToFaceBook()
        {
            return View();
        }
        public IActionResult ScheduleAndRecording()
        {
            return View();
        }
    }
}
