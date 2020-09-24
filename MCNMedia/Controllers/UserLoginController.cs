using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Mvc;

namespace MCNMedia_Dev.Controllers
{
    public class UserLoginController : Controller
    {
        UserDataAccessLayer _userDataAccess = new UserDataAccessLayer();
        public ViewResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UserLogin(User user)
        {
            User usr = _userDataAccess.UserLogin(user);
            if (usr.UserId > 0)
            {

                return View("/Views/Home/Home.cshtml");
            }
            else {
                ViewBag.IsSuccess = 3;
              
            return View();
            }
        }
    }
}
