using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using MCNMedia_Dev._Helper;
using Microsoft.AspNetCore.Hosting;

namespace MCNMedia_Dev.Controllers
{
    public class DashBoardClientController : Controller
    {
        private IWebHostEnvironment environment;

        public DashBoardClientController(IWebHostEnvironment _environment)
        {
            environment = _environment;
        }

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        DashBoardClientDataAccessLayer dashboardData = new DashBoardClientDataAccessLayer();
        DashboardDataAccessLayer dashboardDataAccessLayer = new DashboardDataAccessLayer();
        ChurchDataAccessLayer churchDataAccessLayer = new ChurchDataAccessLayer();
        CameraDataAccessLayer camDataAccess = new CameraDataAccessLayer();

        [HttpGet]
        public IActionResult DashBoardClient(DateTime eventDate)
        {
            int ChurchId1 = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));
            List<AnalyticsModel> analyticsList = churchDataAccessLayer.GetbyChurch(ChurchId1, eventDate, eventDate).ToList();
            if (eventDate==Convert.ToDateTime("1/1/0001 12:00:00 AM"))
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
            GenericModel gm1 = new GenericModel();
            ViewBag.TotalCountriesCount = 0;
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return RedirectToAction("UserLogin", "UserLogin");
                }

                int ChurchId = 0;
                
             
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
                gm1.LCameras = camDataAccess.GetAdminCameraByChurch(ChurchId).ToList();
                gm1.DashBoardClients = dashboardData.GetCountClientDashBoard(ChurchId);
                gm1.Churches = churchDataAccessLayer.GetChurchData(ChurchId);
                

                GoogleAnalytics googleantics = new GoogleAnalytics(environment);
                List<GoogleAnalyticsProperty> googleAnalytics = googleantics.GoogleAnalytics_GetByChurch(eventDate); 
                gm1.googleAnalytics = googleAnalytics;
                if (googleAnalytics.Count > 0)
                {
                    gm1.googleAnalytics = googleAnalytics.FindAll(x => x.PageTitle.Contains(gm1.Churches.Slug + " - MCN"));
                    ViewBag.TotalCountriesCount = gm1.googleAnalytics.Sum(item => item.Count);
                }
               
                ViewBag.ChurchId = ChurchId.ToString();
                return View(gm1);
            }
            catch (Exception exp)
            {
                HttpContext.Session.SetString("ChurchName", gm1.Churches.ChurchName);
                HttpContext.Session.SetString("ctabId", "/DashBoardClient/DashBoardClient");
                ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                return View(gm1);
            }
        }

        public JsonResult ReloadPage(int chrid)
        {
            HttpContext.Session.SetInt32("ChurchId", chrid);
            return Json(chrid);
        }

        [HttpPost]
        public JsonResult ChangeCameraStatus(int cameraId, bool cameraStatus)
        {
            try
            {
                int UserId= Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                camDataAccess.UpdateCameraStatus(cameraId,cameraStatus,UserId);
                return Json(new { success = true, responseText = "The attached file is not supported." });
            }
            catch (Exception e)
            {
                return Json(new { success = false, responseText = e.Message });

            }
        }

        private void ShowMessage(string exceptionMessage)
        {
            _Helper.Common.SaveToXXX(exceptionMessage);
            log.Info("Exception: " + exceptionMessage);
        }
    }
}
