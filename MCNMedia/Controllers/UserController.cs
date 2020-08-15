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
        UserDataAccessLayer userDataAccess = new UserDataAccessLayer("a");

        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }


        [HttpGet]
        public IActionResult ListUser()
        {
             List<User> usr = userDataAccess.GetAllUser().ToList<User>();
             return View(usr);
        }



        [HttpPost()]
        public IActionResult AddUser(User usr)
        {

            //User usr = new User();
            //UpdateModel();
            userDataAccess.AddUser(usr);
            return RedirectToAction("ListUser");
           // return View();
        }

        [HttpGet()]
        public IActionResult GetAlluser()
        {

            //User usr = new User();
            //UpdateModel();
            userDataAccess.GetAllUser();
            return View();
        }
    }
}
