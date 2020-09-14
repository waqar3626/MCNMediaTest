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
        UserAssignChurchesDataAccessLayer userAssignDataAcessLayer = new UserAssignChurchesDataAccessLayer();

      
      [HttpGet]
        public IActionResult AssignChurchesToUser()
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
        [HttpPost]
        public IActionResult AssignChurchesToUser(List<SelectListItem> Data, int UserId)
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

        public IActionResult GetUserAssignChurches() {

            IEnumerable<UserAssignChurches> userAssignChurchesList =  userAssignDataAcessLayer.GetUserAssignChurchesList();
            return View(userAssignChurchesList);
        }

        [HttpGet]
        public IActionResult EditAssignChurchesUser(int id)

        {
            ViewBag.Churches = userAssignDataAcessLayer.GetSingleUserAssignChurches(id);
            UserAssignChurches uAChurches = new UserAssignChurches();
            List<SelectListItem> items = new List<SelectListItem>();
            IEnumerable<UserAssignChurches> churchList = userAssignDataAcessLayer.GetUserAssignedChurches(id);
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (var item in churchList)
            {
                selectListItems.Add(new SelectListItem { Text = item.ChurchName.ToString(), Value = item.ChurchId.ToString(), Selected = item.Assigned  });
                ViewBag.UserId = item.UserId;
            }
                     
               return View(selectListItems);
            }

            [HttpPost]
        public IActionResult EditAssignChurchesUser(List<SelectListItem> Data, int UserId)
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
            return RedirectToAction("GetUserAssignChurches");

        }
    }

}
