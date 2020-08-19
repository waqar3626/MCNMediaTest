using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using Microsoft.AspNetCore.Mvc;

namespace MCNMedia_Dev.Controllers
{
    public class CameraController : Controller
    {
        public IActionResult Index()
        {
            GenericModel gm = new GenericModel();
            return View(gm);
        }
        public IActionResult AddCamera()
        {
            GenericModel gm = new GenericModel();
            return View(gm);
        }
    }
}
