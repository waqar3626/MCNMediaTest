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
        DashBoardClientDataAccessLayer clientdashboardData = new DashBoardClientDataAccessLayer();
        ChurchDataAccessLayer churchDataAccess = new ChurchDataAccessLayer();
        public IActionResult Dashboard(int chrid, DateTime eventDate)
        {
            try
            {
                if (eventDate == Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                {
                    ViewBag.SchDate = DateTime.Now.ToString("dd-MMM-yyyy");
                    eventDate = DateTime.Now;
                }
                else
                {
                    ViewBag.SchDate = eventDate.ToString("dd-MMM-yyyy");
                }
                GenericModel gm = new GenericModel();

                gm.Dashboards = dashboardData.GetDashboardInfo();
                gm.ListDashboards2 = dashboardData.GetDashboardCountry_Churches();
                gm.LDashBoardClients = clientdashboardData.GetDashboardClientInfo(-1);
               gm.AnalyticsList = churchDataAccess.GetAllChurchForAnalytics(eventDate, eventDate);
             
                
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
