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
                ShowMessage("Add User Error" + e.Message);
                throw;
            }
        }
        

        [HttpGet]
        public IActionResult ListUser(User user)
        {

            try
            {

                    LoadDDL();
                user.RoleId = -1;
                ViewBag.UserType = -1;
                GenericModel gm = new GenericModel();
                gm.LUsers = UserList(user);
                //ViewBag.IsSuccess = Convert.ToInt32(TempData["IsSuccess"]);
                ViewBag.PartalBit = "none";
                ViewBag.Message = TempData["Message"];
                //gm.LUsers = userDataAccess.GetAllUser(user).ToList<User>();
                   return View(gm);

            }

            catch (Exception e)
            {
                    ShowMessage("List User Error" + e.Message);
                    throw;
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
            try {

                    User user = userDataAccess.GetUserData(id);
                    if (user == null)
                    {
                        return NotFound();
                    }
                LoadDDL();
                return PartialView("Edit",user);
            }
            catch (Exception e)
            {
                    ShowMessage("Edit User Error" + e.Message);
                    throw;
            }

        }
        

        [HttpPost]
        public IActionResult AddUser(User usr)
        {
            GenericModel gm=new GenericModel();
            try {
                LoadDDL();
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return RedirectToAction("UserLogin", "UserLogin");
                }
                if (ModelState.IsValid)
                {
                    usr.UpdatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                    userDataAccess.AddUser(usr);
                    gm.LUsers = UserList(usr);
                    gm.Users = usr;

                    //TempData["IsSuccess"] = 1;
                    //TempData["Message"] = "User Added Successfully";
                    //ViewBag.SuccessMsg = "Email Send Successfully";

                    //throw new Exception();
                    return View("ListUser", gm);

                    //before
                    //throw new Exception();
                    //return RedirectToAction("ListUser");
                }
                else
                {


                    return View();
                }
            }
            catch (Exception exp)
            {
                LoadDDL();
                ViewBag.PartalBit = "-1";
                ViewBag.ErrorMsgPartial = "Error Occurreds! " + exp.Message;
                return View("ListUser", gm);
                //ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                //return View();
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
                ShowMessage("All Users Error" + e.Message);
                throw;
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
                    //throw new Exception();
                    return RedirectToAction("ListUser", gm);
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
                return RedirectToAction("ListUser");
            }

            catch (Exception e)
            {
                ShowMessage("Delete User Error" + e.Message);
                throw;
            }
        }

        [HttpPost]
        public IActionResult Search(string FirstName, string LastName, string EmailAddress, int Role) 
        {

            try
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
                gm.LUsers = userDataAccess.GetAllUser(usr).ToList<User>();
                return View("/Views/User/ListUser.cshtml", gm);

            }

           catch (Exception e)
            {
                ShowMessage("Search User Error" + e.Message);
                throw;
            }
        }

        public void LoadDDL()
            
        {
            try
            {
                IEnumerable<UserRoles> RoleList = userDataAccess.GetRoles();
                List<SelectListItem> selectListItems = new List<SelectListItem>();
                foreach (var item in RoleList)
                {
                    selectListItems.Add(new SelectListItem { Text = item.RoleName.ToString(), Value = item.RoleId.ToString() });
                }
                ViewBag.State = selectListItems;
            }
            catch (Exception e)
            {
                ShowMessage("User Role Error" + e.Message);

                throw;
            }

        }

        private void ShowMessage(string exceptionMessage)
        {
            log.Info("Exception: " + exceptionMessage);
        }



    }
}
