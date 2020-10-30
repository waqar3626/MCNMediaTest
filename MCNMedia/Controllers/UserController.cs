using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Http;
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
            User user = new User();
             List<User> usr = userDataAccess.GetAllUser(user).ToList<User>();
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
            return PartialView(user);
        }
        

        [HttpPost()]
        public IActionResult AddUser(User usr)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("UserId").ToString()))
            {
                usr.UserId = (int)HttpContext.Session.GetInt32("UserId");
                userDataAccess.AddUser(usr);
            }
            return RedirectToAction("ListUser");
        }

        [HttpGet()]
        public IActionResult GetAlluser()
        {

           
            User usr = new User();
            userDataAccess.GetAllUser(usr);
            return View();
        }

        [HttpPost]
        
        public IActionResult Edit(int id, [Bind] User user)
        {
            
            user.UserId = id;
                userDataAccess.UpdateUser(user); 
                return RedirectToAction("ListUser");
          
        }

        [HttpGet]
       
        public IActionResult Delete(int id)
        {
            userDataAccess.DeleteUser(id);
            return RedirectToAction("ListUser");
        }

        [HttpPost]
        public IActionResult Search(string FirstName, string LastName, string EmailAddress, string Role) 
        {
            
            User usr = new User();
            usr.FirstName = FirstName;
            usr.LastName = LastName;
            usr.EmailAddress = EmailAddress;
            usr.RoleName = Role;
            List<User> user = userDataAccess.GetAllUser(usr).ToList<User>();
            return View("/Views/User/ListUser.cshtml",user);
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
