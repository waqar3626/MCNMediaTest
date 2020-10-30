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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        SetUpDataAccessLayer setupDataAccess = new SetUpDataAccessLayer();
        GenericModel gm = new GenericModel();

        public IActionResult InsertSetUp()
        {

            return View();

        }

        [HttpPost]
        public IActionResult InsertSetUp(SetUp ChurchSetUp)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
                {
                    int churchId = (int)HttpContext.Session.GetInt32("ChurchId");
                    ChurchSetUp.ChurchId = churchId;
                    ChurchSetUp.UpdatedBy = (int)HttpContext.Session.GetInt32("UserId");
                    setupDataAccess.InsertSetUp(ChurchSetUp);
                }
                return RedirectToAction("Listchurch", "Church");

            }
            catch (Exception e)
            {
                ShowMesage("InsertSetup Errors : " + e.Message);
                throw;
            }
        }

        private void ShowMesage(String exceptionMessage)
        {
            log.Error("Exception : " + exceptionMessage);
        }



    }
}
