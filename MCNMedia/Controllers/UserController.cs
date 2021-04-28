using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using _Helper;
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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        UserDataAccessLayer userDataAccess = new UserDataAccessLayer();
        //GenericModel gm = new GenericModel();

        [HttpGet]
        public IActionResult AddUser()
        {
            try
            {

                LoadDDL();
                return View();

            }

            catch (Exception e)
            {
                ViewBag.ErrorMsg = "Error Occurreds! " + e.Message;
                return View();
              
            }
        }
        

        [HttpGet]
        public IActionResult ListUser(User user)
        {
            LoadDDL();
            user.RoleId = -1;
            ViewBag.UserType = -1;
            GenericModel gm = new GenericModel();
            try
            {

                   
                gm.LUsers = UserList(user);
                if (Convert.ToInt32(TempData["addUserSuccess"]) == 1){
                    ViewBag.SuccessMsg = "User Added SuccessFully";
                }
                if (Convert.ToInt32(TempData["deleteUserMsg"]) == 1)
                {
                    ViewBag.SuccessMsg = "User Deleted SuccessFully";
                }
                ViewBag.PartalBit = "none";
         
                   return View(gm);

            }

            catch (Exception e)
            {
                ViewBag.ErrorMsg = "Error Occurreds! " + e.Message;
                return View(gm);
            }
        }

        private List<User> UserList(User user)
        {
            List<User> ListUser = userDataAccess.GetAllUser(user).ToList<User>();
            return ListUser;


        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            LoadDDL();
            User user = new User();
            try {

                   user = userDataAccess.GetUserData(id);
                    if (user == null)
                    {
                        return NotFound();
                    }
                
                return PartialView("Edit",user);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMsg = "Error Occurreds! " + e.Message;
                return PartialView("Edit", user);
            }

        }
        

        [HttpPost]
        public IActionResult AddUser(User usr)
        {
            GenericModel gm=new GenericModel();
            LoadDDL();
            try {
                
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return RedirectToAction("UserLogin", "UserLogin");
                }
                if (ModelState.IsValid)
                {
                    usr.UpdatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                    gm.LUsers = UserList(usr);
                    gm.Users = usr;
                    userDataAccess.AddUser(usr);
                    TempData["addUserSuccess"] = 1;
                    return RedirectToAction("ListUser");

                 
                }
                else
                {


                    return View();
                }
            }
            catch (Exception exp)
            {
               
                ViewBag.PartalBit = "-1";
                ViewBag.ErrorMsgPartial = "Error Occurreds! " + exp.Message;
                return View("ListUser", gm);
           
            }

          
          
        }

        [HttpGet]
        public IActionResult GetAlluser()
        {
            try
            {

                User usr = new User();
                userDataAccess.GetAllUser(usr);
                return View();

            }

            catch (Exception e)
            {
                ViewBag.ErrorMsg = "Error Occurreds! " + e.Message;
                return View();
            }



        }

        [HttpPost]
        
        public IActionResult UpdateUser([Bind] User user, string NewPass)
        {
            GenericModel gm = new GenericModel();
            User user1 = new User();
            user1.RoleId = -1;
            user1.UserId = -1;
            user1.EmailAddress = "";
            user1.FirstName = "";
            user1.LastName = "";

            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return Json(new { Url = "UserLogin" });
                }
                if (ModelState.IsValid) {
                    user.UpdatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                    if (!string.IsNullOrEmpty(NewPass))
                    {
                        user.NewPassword = Hashing.HashPassword(NewPass);
                    }
                    else
                    {
                        user.NewPassword = user.LoginPassword;
                    }
                    userDataAccess.UpdateUser(user);
                    gm.LUsers = UserList(user1);
                    gm.Users = user;
                    return RedirectToAction("ListUser");
                }
                else
                {
                    LoadDDL();
                    return PartialView("Edit");
                }
            }

            catch (Exception exp)
            {
                LoadDDL();
                gm.LUsers = UserList(user1);
                ViewBag.PartalBit = "-2";
                ViewBag.ErrorMsgPartial = "Error Occurreds! " + exp.Message;
                return View("ListUser", gm);

            }


        }

        [HttpGet]
       
        public IActionResult Delete(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return Json(new { Url = "UserLogin" });
                }
                int UpdateBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

                userDataAccess.DeleteUser(id, UpdateBy);
                TempData["deleteUserMsg"] = 1;
                return RedirectToAction("ListUser");
            }

            catch (Exception e)
            {
                ViewBag.PartalBit = "-1";
                ViewBag.ErrorMsgPartial = "Error Occurreds! " + e.Message;
                return RedirectToAction("ListUser");
            }
        }

        [HttpPost]
        public IActionResult Search(string FirstName, string LastName, string EmailAddress, int Role) 
        {
            LoadDDL();
            User usr = new User();
            usr.FirstName = FirstName;
            usr.LastName = LastName;
            usr.EmailAddress = EmailAddress;
            usr.RoleId = Role;
            ViewBag.UserType = Role;
            //List<User> user = userDataAccess.GetAllUser(usr).ToList<User>();
            GenericModel gm = new GenericModel();

            try
            {
              
                gm.LUsers = userDataAccess.GetAllUser(usr).ToList<User>();
                return View("/Views/User/ListUser.cshtml", gm);

            }

           catch (Exception e)
            {
                ViewBag.ErrorMsg = "Error Occurreds! " + e.Message;
                return View("/Views/User/ListUser.cshtml", gm);
            }
        }

        public void LoadDDL()
            
        {
                IEnumerable<UserRoles> RoleList = userDataAccess.GetRoles();
                List<SelectListItem> selectListItems = new List<SelectListItem>();
                foreach (var item in RoleList)
                {
                    selectListItems.Add(new SelectListItem { Text = item.RoleName.ToString(), Value = item.RoleId.ToString() });
                }
                ViewBag.State = selectListItems;
            
          

        }

        



    }
}
