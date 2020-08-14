using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MCNMedia_Dev.Controllers
{
    public class UserController : Controller
    {
        public IActionResult AddUser()
        {
            return View();
        }
    }
}
