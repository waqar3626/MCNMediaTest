using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Mvc;

namespace MCNMedia_Dev.Controllers
{
    public class RecordingReportController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DashBoardClientDataAccessLayer clientdashboardData = new DashBoardClientDataAccessLayer();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RecordingReportList()
        {
            try
            {
                GenericModel gm = new GenericModel();

                ViewBag.SchDate = DateTime.Now.ToString("dd-MMM-yyyy");

                DateTime date = DateTime.Now;
                gm.LDashBoardClients = clientdashboardData.GetRecordingReport(-1, date);
                return View(gm);


            }
            catch (Exception e)
            {
                ShowMessage("Dashboard Error" + e.Message);

                throw;
            }
        }

        public IActionResult Search(DateTime eventDate)
        {
            try
            {

                ViewBag.SchDate = eventDate.ToString("dd-MMM-yyyy");
                GenericModel gm = new GenericModel();
                //DateTime date = genericModel.DashBoardClients.ScheduleEventDate;
                gm.LDashBoardClients = clientdashboardData.GetRecordingReport(-1, eventDate);
                return View("/Views/RecordingReport/RecordingReportList.cshtml", gm);
            }
            catch (Exception e)
            {
                ShowMessage("Search Church  Error" + e.Message);
                throw;
            }
        }

        private void ShowMessage(string exceptionMessage)
        {
            log.Info("Exception: " + exceptionMessage);
        }
    }
}
