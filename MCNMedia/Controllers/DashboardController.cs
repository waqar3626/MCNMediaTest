using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Mvc;

namespace MCNMedia_Dev.Controllers
{
    public class DashboardController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        DashboardDataAccessLayer dashboardData = new DashboardDataAccessLayer();
        public IActionResult Dashboard()
        {
            try
            {
                GenericModel gm = new GenericModel();
                gm.Dashboards = dashboardData.GetDashboardInfo();
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
