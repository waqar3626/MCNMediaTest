using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MCNMedia_Dev.Controllers
{
    public class CameraController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
