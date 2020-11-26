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
        UserAssignChurchesDataAccessLayer userAssignDataAcessLayer = new UserAssignChurchesDataAccessLayer();
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
                    HttpContext.Session.SetInt32("UserId", usr.UserId);
                    HttpContext.Session.SetString("UserName", usr.FirstName+" " + usr.LastName);
                   
                    if (usr.RoleName.ToLower() == "admin")
                    {
                        HttpContext.Session.SetString("UserType", usr.RoleName.ToLower());
                        return RedirectToAction("Dashboard","Dashboard");
                    }
                    else if (usr.RoleName.ToLower() == "client")
                    {
                        HttpContext.Session.SetString("UserType", usr.RoleName.ToLower());

                        HttpContext.Session.SetInt32("UserId", usr.UserId);
              
                        IEnumerable<UserAssignChurches> churchList = userAssignDataAcessLayer.GetSingleUserAssignChurches(usr.UserId);
                        if (churchList.Count()>0)
                        {
                            return RedirectToAction("DashBoardClient", "DashBoardClient");
                        }
                        else
                        {
                            //ViewBag.IsSuccess = 4;
                            //return View();
                            return RedirectToAction("UserMessage", "UserLogin");
                        }
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

        public IActionResult UserMessage()
        {
            return View();
        }
        private void ShowMesage(String exceptionMessage)
        {
            log.Error("Exception : " + exceptionMessage);
        }

        [HttpPost]
        public IActionResult ChangeUserPassword(int UserId ,string OldPassword,string NewPassword)
        {
            User user = _userDataAccess.GetUserData(UserId);
            if (user.LoginPassword == OldPassword) {
                _userDataAccess.ChangeUserPassword(UserId, NewPassword, UserId);
                return Json(new { success = true, responseText = "The attached file is not supported." });
            }
            else {
                return Json(new { success = false, responseText = "The attached file is not supported." });

            }
        }

    }

   
}
