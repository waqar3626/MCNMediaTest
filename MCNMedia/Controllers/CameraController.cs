using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MCNMedia_Dev.Controllers
{
    public class CameraController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        CameraDataAccessLayer camDataAccess = new CameraDataAccessLayer();
        GenericModel gm = new GenericModel();
        public IActionResult Index()
        {
            try
            {               
                GenericModel gm = new GenericModel();
                int churchId = (int)HttpContext.Session.GetInt32("ChurchId");
                gm.LCameras = camDataAccess.GetAdminCameraByChurch(churchId);
                return View(gm);
            }
            catch (Exception e)
            {
                ShowMessage("Index Camera Error" + e.Message);
                throw;
            }
        }

        [HttpPost]
        public IActionResult AddCamera(string CameraName, string CameraUrl, string HttpPort, string RtspPort, int ServerID)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return RedirectToAction("UserLogin", "UserLogin");
                }
                Camera camera = new Camera();
                if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
                {
                    int churchId = (int)HttpContext.Session.GetInt32("ChurchId");
                    camera.ChurchId = churchId;
                    camera.CameraName = CameraName;
                    camera.CameraUrl = CameraUrl;
                    camera.HttpPort = HttpPort;
                    camera.RtspPort = RtspPort;
                    camera.CameraUrl = CameraUrl;
                    camera.ServerId = ServerID;
                    camera.CreatedBy = (int)HttpContext.Session.GetInt32("UserId");
                    if (HttpContext.Session.GetString("UserType") == "admin")
                    {
                        camera.CameraType = _Helper.CameraType.AdminCamera;
                    }
                    else if (HttpContext.Session.GetString("UserType") == "client")
                    {
                        camera.CameraType = _Helper.CameraType.ClientCamera;
                    }
                    int cameraId = camDataAccess.AddCamera(camera);
                    if (cameraId > 0)
                    {
                        MCNMedia_Dev.WowzaApi.WowzaHelper wowzaHelper = new WowzaApi.WowzaHelper();
                        wowzaHelper.RequestCamera(churchId, cameraId, camera.CameraUrl);

                        gm.ResultMessage = "Camera Added Sucessfully";
                        gm.LCameras = camDataAccess.GetAdminCameraByChurch(churchId);

                        HttpContext.Session.SetString("TabName", "Camera");
                        var queryString = new { chId = churchId };
                        return Json(new { success = true, responseText = "The attached file is not supported." });
                    }
                }
                return RedirectToAction("Listchurch", "Church");
            }
            catch (Exception e)
            {
                ShowMessage("Add Camera Error" + e.Message);
                throw;
            }
        }

        public IActionResult EditCamera(int id)
        {
            try
            {
                GenericModel gm = new GenericModel();
                gm.Cameras = camDataAccess.GetCameraById(id);
                return PartialView("_EditCamera", gm);
            }
            catch (Exception e)
            {
                ShowMessage("Edit Camera Error" + e.Message);
                throw;
            }
        }

        public IActionResult DeleteCamera(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return Json(new { Url = "UserLogin" });
                }
                GenericModel gm = new GenericModel();
                bool res = camDataAccess.DeleteCamera(id);
                return Json(res);
            }
            catch (Exception e)
            {
                ShowMessage("Delete Camera Error" + e.Message);
                throw;
            }

        }

        public IActionResult GetAllCameras()
        {
            try
            {    
                Camera camera = new Camera();
                if (HttpContext.Session.GetString("UserType") == "admin")
                {

                    int churchId = (int)HttpContext.Session.GetInt32("ChurchId");
                    //List<Camera> cameraInfo = camDataAccess.GetAllCameras(churchId, "AdminCamera").ToList();
                    gm.LCameras = camDataAccess.GetAdminCameraByChurch(churchId);
                    return View("ChurchDetails", gm);
                }
                else if (HttpContext.Session.GetString("UserType") == "client")
                {

                    int churchId = (int)HttpContext.Session.GetInt32("ChurchId");
                    gm.LCameras = camDataAccess.GetMobileCameraByChurch(churchId);

                    return View("Views/Client/CameraDetail", gm);
                }

                return Json(1);
            }
            catch (Exception e)
            {
                ShowMessage("All Cameras Error" + e.Message);
                throw;
            }

        }

        public JsonResult UpdateCamera(string CameraId, string CameraName, string CameraUrl, string HttpPort, string RtspPort)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return Json(new { Url="UserLogin"});
                }
                Camera CamUpdate = new Camera();
                CamUpdate.CameraId = Convert.ToInt32(CameraId);
                CamUpdate.CameraName = CameraName;
                CamUpdate.CameraUrl = CameraUrl;
                CamUpdate.HttpPort = HttpPort;
                CamUpdate.RtspPort = RtspPort;
                CamUpdate.UpdatedBy = (int)HttpContext.Session.GetInt32("UserId");
                int res = camDataAccess.Updatecamera(CamUpdate);
                int churchId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));
                return Json(res);
            }
            catch (Exception e)
            {
                ShowMessage("Update Cameras Error" + e.Message);
                throw;
            }

        }

        public void SyncCameraWithWowza()
        {
            CameraStream cameraStream = new CameraStream();
            List<Camera> cameraList = camDataAccess.GetAllAdminCameras();
            foreach (Camera cam in cameraList)
            {
                WowzaApi.WowzaHelper wowzaHelper = new WowzaApi.WowzaHelper();
                int churchId =cam.ChurchId;
                int cameraId = cam.CameraId;
                cameraStream = wowzaHelper.RequestCameraStatus(churchId, cameraId);
                if(cameraStream.isConnected) // Camera Status
                {

                }
            }
        }

        private void ShowMessage(string exceptionMessage)
        {
            log.Info("Exception: " + exceptionMessage);
        }
    }
}