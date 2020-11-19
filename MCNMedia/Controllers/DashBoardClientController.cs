﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MCNMedia_Dev.Controllers
{
    public class DashBoardClientController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        DashBoardClientDataAccessLayer dashboardData = new DashBoardClientDataAccessLayer();
        DashboardDataAccessLayer dashboardDataAccessLayer = new DashboardDataAccessLayer();
        ChurchDataAccessLayer churchDataAccessLayer = new ChurchDataAccessLayer();
        [HttpGet]
        public IActionResult DashBoardClient()
        {
            try
            {
                int ChurchId = 0;
                GenericModel gm1 = new GenericModel();
                int usrId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                int UsrAssignChurchId = dashboardData.GetUserAssignTopChurchId(usrId);
               
                if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
                {
                    ChurchId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));
                    
                }
                else
                {
                    ChurchId = UsrAssignChurchId;
                    HttpContext.Session.SetInt32("ChurchId", ChurchId);
                }

                gm1.LDashBoardClients = dashboardData.GetDashboardClientInfo(ChurchId);
                gm1.Dashboards = dashboardData.GetDashboardClientInfoCount(ChurchId);
                gm1.DashBoardClients = dashboardData.GetCountClientDashBoard(ChurchId);
                gm1.Churches = churchDataAccessLayer.GetChurchData(ChurchId);
               

               
                HttpContext.Session.SetString("ctabId", "/DashBoardClient/DashBoardClient");
                ViewBag.ChurchId = ChurchId.ToString();
                return View(gm1);
            }
            catch (Exception e)
            {
                ShowMessage("Dashboard Error" + e.Message);
                throw;
            }

        }

        public JsonResult ReloadPage(int chrid)
        {
            HttpContext.Session.SetInt32("ChurchId", chrid);
            return Json(chrid);
        }

        private void ShowMessage(string exceptionMessage)
        {
            log.Info("Exception: " + exceptionMessage);
        }
    }
}
