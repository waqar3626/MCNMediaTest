using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Mvc;

namespace MCNMedia_Dev.Controllers
{
    public class MediaChurchController : Controller
    {
        MediaChurchDataAccessLayer medchurchDataAccess = new MediaChurchDataAccessLayer();
        public IActionResult AddPicture()
        {
            return View();
        }

        public IActionResult EditPicture()
        {
            return View();
        }
    }
}
