using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace MCNMedia_Dev.Controllers
{
    public class UserAssignChurchesController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        UserDataAccessLayer userDataAccess = new UserDataAccessLayer();
        UserAssignChurchesDataAccessLayer userAssignDataAcessLayer = new UserAssignChurchesDataAccessLayer();

      
      [HttpGet]
        public IActionResult AssignChurchesToUser()
        {
            try
            {
                UserAssignChurches uAChurches = new UserAssignChurches();
                List<SelectListItem> items = new List<SelectListItem>();
                IEnumerable<UserAssignChurches> churchList = userAssignDataAcessLayer.GetChurchList();
                List<SelectListItem> selectListItems = new List<SelectListItem>();
                foreach (var item in churchList)
                {
                    selectListItems.Add(new SelectListItem { Text = item.ChurchName.ToString(), Value = item.ChurchId.ToString() });
                }


                ViewBag.ErrorMessage = "Error";
                return View(selectListItems);
            }
            catch (Exception e)
            {
                ShowMessage("Assign Church to User Errors  'Get' : " + e.Message);
                throw;
            }
           
            
        }
        [HttpPost]
        public IActionResult AssignChurchesToUser(List<SelectListItem> Data, int UserId)
        {
            try
            {
                try
                {
                    if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                    {
                        return RedirectToAction("UserLogin", "UserLogin");
                    }
                    UserAssignChurches userAssignChurches = new UserAssignChurches();

                    foreach (SelectListItem item in Data)
                    {
                        if (item.Selected)
                        {
                            userAssignChurches.ChurchId = Convert.ToInt32(item.Value);
                            userAssignChurches.UserId = UserId;
                            userAssignChurches.CreatedBy = (int)HttpContext.Session.GetInt32("UserId");
                            userAssignDataAcessLayer.AddUserChurch(userAssignChurches);

                        }

                    }
                    return RedirectToAction("GetUserAssignChurches");
                }
                catch (Exception)
                {

                    ViewBag.ErrorMessage = "ErrorMessage";
                    return RedirectToAction("AssignChurchesToUser");
                }
            }
            catch (Exception e)
            {
                ShowMessage("Assign Churches To User Errors  'Post' : " + e.Message);
                throw;
            }
          
 

        }

        public IActionResult GetUserAssignChurches() {

            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return RedirectToAction("UserLogin", "UserLogin");
                }
                IEnumerable<UserAssignChurches> userAssignChurchesList = userAssignDataAcessLayer.GetUserAssignChurchesList();
                return View(userAssignChurchesList);
            }
            catch (Exception e)
            {
                ShowMessage("Get User Assign Churches Errors : " + e.Message);
                throw;
            }
            
        }

        [HttpGet]
        public IActionResult EditAssignChurchesUser(int id)

        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return RedirectToAction("UserLogin", "UserLogin");
                }
                ViewBag.Churches = userAssignDataAcessLayer.GetSingleUserAssignChurches(id);
                UserAssignChurches uAChurches = new UserAssignChurches();
                List<SelectListItem> items = new List<SelectListItem>();
                IEnumerable<UserAssignChurches> churchList = userAssignDataAcessLayer.GetUserAssignedChurches(id);
                List<SelectListItem> selectListItems = new List<SelectListItem>();
                foreach (var item in churchList)
                {
                    selectListItems.Add(new SelectListItem { Text = item.ChurchName.ToString()+ " , " + item.Town.ToString(), Value = item.ChurchId.ToString(), Selected = item.Assigned });
                    ViewBag.UserId = item.UserId;
                }

                return View(selectListItems);
            }
            catch (Exception e)
            {
                ShowMessage("Edit Assign Churches User Errors 'Get' : " + e.Message);
                throw;
            }
           
            }

            [HttpPost]
        public IActionResult EditAssignChurchesUser(List<SelectListItem> Data, int UserId)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return RedirectToAction("UserLogin", "UserLogin");
                }
                UserAssignChurches userAssignChurches = new UserAssignChurches();
                int UpdatedBy = (int)HttpContext.Session.GetInt32("UserId");
                userAssignDataAcessLayer.DeleteUserChurches(UserId,UpdatedBy);

                foreach (SelectListItem item in Data)
                {
                    if (item.Selected)
                    {
                        userAssignChurches.ChurchId = Convert.ToInt32(item.Value);
                        userAssignChurches.UserId = UserId;
                        userAssignChurches.CreatedBy = (int)HttpContext.Session.GetInt32("UserId");
                        userAssignDataAcessLayer.AddUserChurch(userAssignChurches);

                    }

                }
                return RedirectToAction("ListUser", "User");
            }

            catch (Exception e)
            {
                ShowMessage("Assign Churches To User Errors  'Post' : " + e.Message);
                throw;
            }
           

        }


            private void ShowMessage(string exceptionMessage)
            {
                log.Info("Exception: " + exceptionMessage);
            }



    }


}
