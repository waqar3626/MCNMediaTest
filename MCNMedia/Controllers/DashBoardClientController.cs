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


namespace MCNMedia_Dev.Controllers
{
    public class DashBoardClientController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        DashBoardClientDataAccessLayer dashboardData = new DashBoardClientDataAccessLayer();
        DashboardDataAccessLayer dashboardDataAccessLayer = new DashboardDataAccessLayer();
        ChurchDataAccessLayer churchDataAccessLayer = new ChurchDataAccessLayer();
        CameraDataAccessLayer camDataAccess = new CameraDataAccessLayer();

        [HttpGet]
        public IActionResult DashBoardClient()
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return RedirectToAction("UserLogin", "UserLogin");
                }

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
                gm1.LCameras = camDataAccess.GetAllCameras(ChurchId,"AdminCamera").ToList();
                gm1.DashBoardClients = dashboardData.GetCountClientDashBoard(ChurchId);
                gm1.Churches = churchDataAccessLayer.GetChurchData(ChurchId);


                HttpContext.Session.SetString("ChurchName", gm1.Churches.ChurchName);
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

        [HttpPost]
        public JsonResult ChangeCameraStatus(int cameraId, bool cameraStatus)
        {

            try
            {
                int UserId= Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                camDataAccess.UpdatecameraStatus(cameraId,cameraStatus,UserId);
                return Json(new { success = true, responseText = "The attached file is not supported." });
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
