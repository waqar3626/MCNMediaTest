using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Analytics.SDK.Core;
using Google.Analytics.SDK.Core.Extensions;
using Google.Analytics.SDK.Core.Services.Interfaces;
using MCNMedia_Dev._Helper;
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
        GoogleAnalytics googleantics = new GoogleAnalytics();

        public IActionResult Dashboard(int chrid, DateTime eventDate)
        {
            try
            {
                List<AnalyticsModel> analyticsList = churchDataAccess.GetAllChurchForAnalytics(eventDate, eventDate).ToList();
                if (eventDate == Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                {
                    ViewBag.SchDate = DateTime.Now.ToString("dd-MMM-yyyy");
                    ViewBag.TotalCountriesCount = analyticsList.Sum(item => item.CountryCount);
                    eventDate = DateTime.Now;
                }
                else
                {
                    ViewBag.SchDate = eventDate.ToString("dd-MMM-yyyy");
                    ViewBag.TotalCountriesCount = analyticsList.Sum(item => item.CountryCount);
                }
                GenericModel gm = new GenericModel();
                gm.Dashboards = dashboardData.GetDashboardInfo();
                gm.ListDashboards2 = dashboardData.GetDashboardCountry_Churches();
                gm.LDashBoardClients = clientdashboardData.GetDashboardClientInfo(-1);
               gm.AnalyticsList = churchDataAccess.GetAllChurchForAnalytics(eventDate, eventDate);
                googleantics.Authenticate();
                gm.googleAnalytics = googleantics.QueryDataPer(eventDate);
                ViewBag.TotalCountriesCount = gm.googleAnalytics.Sum(item => item.Count);


                return View(gm);
            }
            catch (Exception e)
            {
                ShowMessage("Dashboard Error" + e.Message);
                
                 throw;
            }
            
        }

         //Google Analytics Start

        //class Program
        //{
        //    private const string DemoDataApplicationWebProplerty = "UA-53766825-1";
        //    private const string Applicationname = "QuickStart core";
        //    private const string ApplicationVersion = "1.0";
        //    private const string ApplicationId = "1.0.0";

        //    public static async Task Main(string[] args)
        //    {
        //        Console.WriteLine("Hello Google Analatics SDK!");
        //        var tracker = TrackerBuilder.BuildMobileTracker(DemoDataApplicationWebProplerty, Applicationname, ApplicationVersion, ApplicationId);
        //        if (!await ScreenViewHitHelper.SendAsync(tracker, "QuickStartMain"))
        //        {
        //            Console.WriteLine("Send Hit Failed");
        //            return 1;
        //        }

        //        Console.WriteLine("Hit Sent");
        //        return 0;
        //    }
        //}


        //public static async Task SendAsync(ITracker tracker, string screenName)
        //{
        //    var hit = new ScreenViewHit(screenName)
        //    {

        //        DataSource = "app",
        //    };

        //    // create the hit request.
        //    var request = (HitRequestBase)tracker.CreateHitRequest(hit);

        //    // Run a debug check to ensure its valid.
        //    var debugResponse = await request.ExecuteDebugAsync();
        //    if (!((DebugResult)debugResponse).IsValid()) 
        //    {
        //        return false;
        //    }
        //    // Send hit.
        //    var collectRequest = await request.ExecuteCollectAsync();
        //    Console.Write(collectRequest.RawResponse);
        //    return true;
        //}

        //Google Analytics END
        private void ShowMessage(string exceptionMessage)
        {
            log.Info("Exception: " + exceptionMessage);
        }
    }
}
