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
    public class UserLoginController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        UserDataAccessLayer _userDataAccess = new UserDataAccessLayer();
        public ViewResult UserLogin()
        {
            ModelState.Clear();
            HttpContext.Session.Clear();
            return View();
        }

        [HttpPost]
        public IActionResult UserLogin(User user)
        {
            try
            {
                User usr = _userDataAccess.UserLogin(user);
                if (usr.UserId > 0)
                {
                    if (usr.RoleName.ToLower() == "admin")
                    {
                        HttpContext.Session.SetString("UserType", usr.RoleName.ToLower());
                        return View("/Views/Home/Home.cshtml");
                    }
                    else if (usr.RoleName.ToLower() == "client")
                    {
                        HttpContext.Session.SetString("UserType", usr.RoleName.ToLower());

                        HttpContext.Session.SetInt32("UserId", usr.UserId);
                        return RedirectToAction("ChurchInfo", "Client", usr.UserId);
                    }
                    else
                    {
                        return ViewBag.IsSuccess = 2;
                    }
                }
                else
                {
                    ViewBag.IsSuccess = 3;

                    return View();
                }
            }
            catch (Exception e)
            {
                ShowMesage("User Login Errors : " + e.Message);
                throw;
            }
        }

        private void ShowMesage(String exceptionMessage)
        {
            log.Error("Exception : " + exceptionMessage);
        }




    }

   
}
