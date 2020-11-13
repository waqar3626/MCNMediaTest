using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MCNMedia_Dev.Controllers
{
    public class DashBoardClientController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        DashBoardClientDataAccessLayer dashboardData = new DashBoardClientDataAccessLayer();
        DashboardDataAccessLayer DashboardDataAccessLayer = new DashboardDataAccessLayer();
        public IActionResult DashBoardClient()
        {
            try
            {
                GenericModel gm = new GenericModel();
                int ChrId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));
                gm.LDashBoardClients = dashboardData.GetDashboardClientInfo(ChrId);
                gm.Dashboards = dashboardData.GetDashboardClientInfoCount(ChrId);
                gm.DashBoardClients = dashboardData.GetCountClientDashBoard(ChrId);
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
