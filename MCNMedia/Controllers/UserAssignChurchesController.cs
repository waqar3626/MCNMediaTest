using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
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
                ShowMesage("Assign Church to User Errors  'Get' : " + e.Message);
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
                    UserAssignChurches userAssignChurches = new UserAssignChurches();

                    foreach (SelectListItem item in Data)
                    {
                        if (item.Selected)
                        {
                            userAssignChurches.ChurchId = Convert.ToInt32(item.Value);
                            userAssignChurches.UserId = UserId;
                            userAssignChurches.CreatedBy = 1;
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
                ShowMesage("Assign Churches To User Errors  'Post' : " + e.Message);
                throw;
            }


        }

        public IActionResult GetUserAssignChurches()
        {
            try
            {
                IEnumerable<UserAssignChurches> userAssignChurchesList = userAssignDataAcessLayer.GetUserAssignChurchesList();
                return View(userAssignChurchesList);

            }
            catch (Exception e)
            {
                ShowMesage("Get User Assign Churches Errors : " + e.Message);
                throw;
            } 
        }

        [HttpGet]
        public IActionResult EditAssignChurchesUser(int id)

        {
            try
            {
                ViewBag.Churches = userAssignDataAcessLayer.GetSingleUserAssignChurches(id);
                UserAssignChurches uAChurches = new UserAssignChurches();
                List<SelectListItem> items = new List<SelectListItem>();
                IEnumerable<UserAssignChurches> churchList = userAssignDataAcessLayer.GetUserAssignedChurches(id);
                List<SelectListItem> selectListItems = new List<SelectListItem>();
                foreach (var item in churchList)
                {
                    selectListItems.Add(new SelectListItem { Text = item.ChurchName.ToString(), Value = item.ChurchId.ToString(), Selected = item.Assigned });
                    ViewBag.UserId = item.UserId;
                }

                return View(selectListItems);
            }
            catch (Exception e)
            {
                ShowMesage("Edit Assign Churches User Errors : " + e.Message);
                throw;
            }
        }

        [HttpPost]
        public IActionResult EditAssignChurchesUser(List<SelectListItem> Data, int UserId)
        {
            try
            {

                UserAssignChurches userAssignChurches = new UserAssignChurches();
                userAssignDataAcessLayer.DeleteUserChurches(UserId);
                foreach (SelectListItem item in Data)
                {
                    if (item.Selected)
                    {
                        userAssignChurches.ChurchId = Convert.ToInt32(item.Value);
                        userAssignChurches.UserId = UserId;
                        userAssignChurches.CreatedBy = 1;
                        userAssignDataAcessLayer.AddUserChurch(userAssignChurches);

                    }

                }
                return RedirectToAction(nameof(ListUser));

            }
            catch (Exception e)
            {
                ShowMesage("Edit Assign Churches User Errors  'Post' : " + e.Message);
                throw;
            }
        }
        [HttpGet]
        public IActionResult ListUser()
        {
            try
            {
                User user = new User();
                List<User> usr = userDataAccess.GetAllUser(user).ToList<User>();
                return View("/Views/User/ListUser.cshtml", usr);
            }
            catch (Exception e)
            {
                ShowMesage("List User Errors  'Get' : " + e.Message);
                throw;
            }
        }


        private void ShowMesage(String exceptionMessage)
        {
            log.Error("Exception : " + exceptionMessage);
        }

    }
}
