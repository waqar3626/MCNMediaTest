using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace MCNMedia_Dev.Controllers
{
    public class DashBoardClientController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        DashBoardClientDataAccessLayer dashboardData = new DashBoardClientDataAccessLayer();
        DashboardDataAccessLayer dashboardDataAccessLayer = new DashboardDataAccessLayer();
        public IActionResult DashBoardClient(int chrid)
        {
            try
            {
                GenericModel gm = new GenericModel();
                int ChrId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));
                gm.LDashBoardClients = dashboardData.GetDashboardClientInfo(chrid);
                gm.Dashboards = dashboardData.GetDashboardClientInfoCount(chrid);
                gm.DashBoardClients = dashboardData.GetCountClientDashBoard(chrid);
                return View(gm);
            }
            catch (Exception e)
            {
                ShowMessage("Dashboard Error" + e.Message);
                throw;
            }

        }

        private void ShowMessage(string exceptionMessage)
        {
            log.Info("Exception: " + exceptionMessage);
        }
    }
}
