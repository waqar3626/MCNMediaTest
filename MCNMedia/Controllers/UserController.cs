using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySql.Data.MySqlClient;

namespace MCNMedia_Dev.Controllers
{
    public class UserController : Controller
    {
        UserDataAccessLayer userDataAccess = new UserDataAccessLayer();

        [HttpGet]
        public IActionResult AddUser()
        {
            LoadDDL();
            return View();
        }
        

        [HttpGet]
        public IActionResult ListUser()
        {
             List<User> usr = userDataAccess.GetAllUser().ToList<User>();
             return View(usr);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        { 
            User user = userDataAccess.GetUserData(id);
            if (user == null)
            {
                return NotFound();
            }
            LoadDDL();
            return View(user);
        }
        //[HttpGet]
        //public IActionResult Delete(int id)
        //{

        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    User user = userDataAccess.GetUserData(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(user);
        //}

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

        [HttpPost]
        
        public IActionResult Edit(int id, [Bind] User user)
        {
            //if (ModelState.IsValid)
            //{
                userDataAccess.UpdateUser(user); 
                return RedirectToAction("ListUser");
            //}
            //return View();
        }

        [HttpGet]
       
        public IActionResult Delete(int id)
        {
            userDataAccess.DeleteUser(id);
            return RedirectToAction("ListUser");
        }

        public void LoadDDL()
        {
            IEnumerable<UserRoles> RoleList= userDataAccess.GetRoles();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (var item in RoleList)
            {
                selectListItems.Add(new SelectListItem { Text = item.RoleName.ToString(), Value = item.RoleId.ToString() });
            }
            ViewBag.State = selectListItems;
            
        }
    }
}
