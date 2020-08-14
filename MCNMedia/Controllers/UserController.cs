using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using Microsoft.AspNetCore.Mvc;

namespace MCNMedia_Dev.Controllers
{
    public class UserController : Controller
    {
        UserDataAccessLayer aa = new UserDataAccessLayer("a");

        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }


        [HttpPost()]
        public IActionResult AddUser(User usr)
        {

            //User usr = new User();
            //UpdateModel();


            aa.AddUser(usr);



            return View();
        }
    }
}
