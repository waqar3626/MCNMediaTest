using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using Microsoft.AspNetCore.Mvc;

namespace MCNMedia_Dev.Controllers
{
    public class UserController : Controller
    {
        UserDataAccessLayer userDataAccess = new UserDataAccessLayer("a");

        [HttpGet]
        public IActionResult AddUser()
        {
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
            return View(user);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {

            if (id == null)
            {
                return NotFound();
            }
            User user = userDataAccess.GetUserData(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

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
            if (id != user.UserId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                userDataAccess.UpdateUser(user);
                return RedirectToAction("ListUser");
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
       
        public IActionResult DeleteConfirmed(int id)
        {
            userDataAccess.DeleteUser(id);
            return RedirectToAction("ListUser");
        }
    }
}
