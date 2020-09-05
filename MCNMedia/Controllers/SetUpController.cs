using System;
using System.Collections.Generic;
using System.Linq;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MCNMedia_Dev.Controllers
{
    public class SetUpController : Controller
    {
        SetUpDataAccessLayer setupDataAccess = new SetUpDataAccessLayer();
        GenericModel gm = new GenericModel();

        public IActionResult InsertSetUp()
        {
           
            return View();

        }

        [HttpPost]
        public IActionResult InsertSetUp(SetUp ChurchSetUp)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
            {
                int churchId = (int)HttpContext.Session.GetInt32("ChurchId");
                ChurchSetUp.ChurchId = churchId;

                setupDataAccess.InsertSetUp(ChurchSetUp);
              }
            return RedirectToAction("Listchurch", "Church");
        }



    }
}
