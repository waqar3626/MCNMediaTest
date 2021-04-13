using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Helper;
using MCNMedia_Dev._Helper;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OtpNet;

namespace MCNMedia_Dev.Controllers
{
    public class UserLoginController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        UserDataAccessLayer _userDataAccess = new UserDataAccessLayer();
        UserAssignChurchesDataAccessLayer userAssignDataAcessLayer = new UserAssignChurchesDataAccessLayer();
        EmailTemplateDataAccessLayer _emailTemDataAccessLayer = new EmailTemplateDataAccessLayer();
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
                    HttpContext.Session.SetString("UserName", usr.FirstName + " " + usr.LastName);
                    if (usr.RoleName.ToLower() == "admin")
                    {
                        HttpContext.Session.SetString("UserType", usr.RoleName.ToLower());
                        return RedirectToAction("Dashboard", "Dashboard");
                    }
                    else if (usr.RoleName.ToLower() == "client")
                    {
                        HttpContext.Session.SetString("UserType", usr.RoleName.ToLower());
                        HttpContext.Session.SetInt32("UserId", usr.UserId);

                        IEnumerable<UserAssignChurches> churchList = userAssignDataAcessLayer.GetSingleUserAssignChurches(usr.UserId);
                        if (churchList.Count() > 0)
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
                ViewBag.ErrorMsg = "Error Occurreds! " + e.Message;
                return View();
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
        public IActionResult ChangeUserPassword(int UserId, string OldPassword, string NewPassword)
        {
            try
            {
                User user = _userDataAccess.GetUserData(UserId);
                Status sts = new Status();
                sts = Hashing.ValidatePassword(OldPassword, user.LoginPassword);
                if (sts.Success)
                {
                    _userDataAccess.ChangeUserPassword(UserId, Hashing.HashPassword(NewPassword), UserId);
                    return Json(new { success = true, responseText = "Password has been updated successfully." });
                }
                else
                {
                    return Json(new { success = false, responseText = sts.Message });
                }
            }
            catch (Exception exp)
            {

                return Json(new { success = false, responseText = exp.Message });
            }
          
        }


        public IActionResult ForgetPassword() {
            try
            {
                return View();
            }
            catch (Exception exp)
            {

                ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                return View();
            }
         
        }

        [HttpPost]
        public IActionResult ForgetPassword(string Email)
        {
            User user = new User();
            string UserName = "";
            try
            {


                user = _userDataAccess.GetUserDataByEmail(Email);
            if (user.UserId > 0)
            {
                EmailHelper emailHelper = new EmailHelper();
                ForgetPassword forgetPassword = new ForgetPassword();
                string secretkey = forgetPassword.GeneratePassword();
                var bytes = Base32Encoding.ToBytes(secretkey);
                var otp = new Totp(bytes);
                var result = otp.ComputeTotp(DateTime.UtcNow);
                EmailTemplate emailTemplate = _emailTemDataAccessLayer.GetEmailTemplateId(1);
                    UserName = user.FirstName + " " + user.LastName;
                string Message = emailTemplate.Subject;
             
                Message = Message.Replace("#First Name#", UserName);
                Message = Message.Replace("#Code#", result);
                Status sts = emailHelper.SendEmail(emailTemplate.FromEmail, Email, UserName, "Password Reset Code", Message);
                if (sts.Success)
                {
                    TempData["Email"] = Email;
                    TempData["OTPCode"] = result;
                    ViewBag.EmailShow = 2;
                    @ViewBag.EmailAddress = Email;
                }
                else
                {
                    ViewBag.Error = 3;
                    TempData["ErrorMessage"] = sts.Message;
                    @ViewBag.EmailAddress = Email;
                    _Helper.Common.SaveToXXX(sts.Message);
                }
                    TempData["UserId"] = user.UserId;
                    TempData["UserName"] = UserName;
                return View("ConfirmOtpCode");
            }
            else
                {
                    TempData["UserId"] = user.UserId;
                    TempData["UserName"] = UserName;
                    ViewBag.Error = 2;
                return View();
            }

            }
            catch (Exception exp)
            {
                TempData["UserName"] = UserName;
                ViewBag.ErrorMsg = "Error occured " + exp.Message;
                return View();
            }

        }
        [HttpPost]
        public IActionResult ConfirmOtpCode(string Input1, string Input2, string Input3, string Input4, string Input5, string Input6)
        {
            try
            {
              
                string Code = Input1 + Input2 + Input3 + Input4 + Input5 + Input6;
                int OtpCode = Convert.ToInt32(Code);
                string Email = TempData["Email"].ToString();
                int ConfirmOtpCode = Convert.ToInt32(TempData["OTPCode"]);
                if (OtpCode == ConfirmOtpCode)
                {
                    ViewBag.Email = Email;

                    TempData["UserName"] = TempData["UserName"];
                    TempData["UserId"] = TempData["UserId"];
                    TempData["Email"] = TempData["Email"];
                    return View("ResetPassword");
                }
                else
                {
                    ViewBag.EmailShow = 1;
                    TempData["Email"] = TempData["Email"];
                    ViewBag.EmailAddress = "aaaaaa";
                    ViewBag.Error = 2;
                    TempData["UserId"] = TempData["UserId"];
                    TempData["UserName"] = TempData["UserName"];
                    return View();
                }


            }
            catch (Exception exp)
            {
                TempData["Email"] = TempData["Email"];
                TempData["UserId"] = TempData["UserId"];
                ViewBag.ErrorMsg = "Error occured " + exp.Message;
                return View();
            }
          


        }
        [HttpPost]
        public IActionResult ResetPassword(string hdnEmail, string newPassword)
        {
            try
            {
                int UserId = Convert.ToInt32(TempData["UserId"]);
                string Email = TempData["Email"].ToString();
                _userDataAccess.ResetUserPassword(UserId, newPassword);
                return View("PasswordResetMessage");
            }
            catch (Exception exp)
            {
                TempData["UserName"] = TempData["UserName"].ToString();
                TempData["UserId"] = TempData["UserId"];
                TempData["Email"] = TempData["Email"];
                ViewBag.ErrorMsg = "Error occured " + exp.Message;
                return View();
            }
            


        }

    }


}
