using System;
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
                GenericModel gm1 = new GenericModel();
                 int ChrId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));
                gm1.LDashBoardClients = dashboardData.GetDashboardClientInfo(ChrId);
                gm1.Dashboards = dashboardData.GetDashboardClientInfoCount(ChrId);
                gm1.DashBoardClients = dashboardData.GetCountClientDashBoard(ChrId);
                int usrId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                int UsrAssignChurchId = dashboardData.GetUserAssignTopChurchId(usrId);
                int ChurchId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));
                if (ChurchId > 0) {
                    
                    gm1.Churches = churchDataAccessLayer.GetChurchData(ChurchId);
                }
                else
                {
                    HttpContext.Session.SetInt32("ChurchId", UsrAssignChurchId);
                    gm1.Churches = churchDataAccessLayer.GetChurchData(UsrAssignChurchId);

                }

               
                HttpContext.Session.SetString("ctabId", "/DashBoardClient/DashBoardClient");
                ViewBag.ChurchId = ChrId.ToString();
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
