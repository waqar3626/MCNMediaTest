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

           
            
            return View(selectListItems);
            
        }

        public ActionResult Index()

        {
            List<UserAssignChurches> data = new List<UserAssignChurches>();
            List<SelectListItem> items = new List<SelectListItem>();
            IEnumerable<UserAssignChurches> churchList = userAssignDataAcessLayer.GetChurchList();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (var item in churchList)
            {
                selectListItems.Add(new SelectListItem { Text = item.ChurchName.ToString(), Value = item.ChurchId.ToString() });
            }
           
            
            return View(selectListItems);
        }

        [HttpPost]
        public ActionResult Index(List<SelectListItem> items)
        {
            ViewBag.Message = "Selected Items:\\n";
            foreach (SelectListItem item in items)
            {
                if (item.Selected)
                {
                    ViewBag.Message += string.Format("{0}\\n", item.Text);
                }
            }
            return View(items);

        }
            //public void LoadUser()
            //{
            //    IEnumerable<UserAssignChurches> userList = userAssignDataAcessLayer.GetUsers();
            //    List<SelectListItem> selectListItems = new List<SelectListItem>();
            //    foreach (var item in userList)
            //    {
            //        selectListItems.Add(new SelectListItem { Text = item.FirstName.ToString(), Value = item.UserId.ToString() });
            //    }
            //    ViewBag.UserList = selectListItems;

            //}
            //public void LoadChruchList()
            //{
            //    IEnumerable<UserAssignChurches> churchList = userAssignDataAcessLayer.GetChurchList();
            //    List<SelectListItem> selectListItems = new List<SelectListItem>();
            //    foreach (var item in churchList)
            //    {
            //        selectListItems.Add(new SelectListItem { Text = item.ChurchName.ToString(), Value = item.ChurchId.ToString() });
            //    }
            //    ViewBag.ChurchList = selectListItems;

            //}

        }
}
