using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Microsoft.AspNetCore.Http;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Data;

namespace MCNMedia_Dev.Controllers
{
    public class CameraController : Controller
    {
        CameraDataAccessLayer camDataAccess = new CameraDataAccessLayer();
        GenericModel gm = new GenericModel();
        public IActionResult Index()
        {

            GenericModel gm = new GenericModel();
            int churchId = (int)HttpContext.Session.GetInt32("ChurchId");
            gm.LCameras = camDataAccess.GetAllCameras(churchId);
            return View(gm);
        }

        [HttpPost]
        public IActionResult AddCamera(GenericModel cam)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
            {

                int churchId = (int)HttpContext.Session.GetInt32("ChurchId");
                cam.Cameras.ChurchId = churchId;

                int cameraId = camDataAccess.AddCamera(cam.Cameras);
                if (cameraId > 0)
                {
                    MCNMedia_Dev.WowzaApi.WowzaHelper wowzaHelper = new WowzaApi.WowzaHelper();
                    wowzaHelper.RequestCamera(churchId, cameraId, cam.Cameras.CameraUrl);

                    gm.ResultMessage = "Camera Added Sucessfully";

                    gm.LCameras = camDataAccess.GetAllCameras(churchId);
                    HttpContext.Session.SetString("TabName", "Camera");
                    var queryString = new { chId = churchId };

                    if (HttpContext.Session.GetString("UserType") == "admin")
                    {
                        return RedirectToAction("ChurchDetails", "Church", queryString);
                    }
                    else if (HttpContext.Session.GetString("UserType") == "client")
                    {
                        return RedirectToAction("CameraDetail", "client", queryString);
                    }
                }
                else
                {
                    gm.ResultMessage = "Error occured please try later";
                }
            }
            return RedirectToAction("Listchurch", "Church");
        }

        public IActionResult EditCamera(int id)
        {
            GenericModel gm = new GenericModel();
            gm.Cameras = camDataAccess.GetCameraById(id);
            // return View(gm);
            return PartialView("_EditCamera", gm);
        }

        public IActionResult DeleteCamera(int id)
        {
            GenericModel gm = new GenericModel();
            bool res = camDataAccess.DeleteCamera(id);
            return Json(res);
        }

        public JsonResult GetAllCameras()
        {
            int churchId = (int)HttpContext.Session.GetInt32("ChurchId");
            List<Camera> cameraInfo = camDataAccess.GetAllCameras(churchId).ToList();
            return Json(cameraInfo);
        }

        public JsonResult UpdateCamera(string CameraId, string CameraName, string CameraUrl, string HttpPort, string RtspPort)
        {
            Camera CamUpdate = new Camera();
            CamUpdate.CameraId = Convert.ToInt32(CameraId);
            CamUpdate.CameraName = CameraName;
            CamUpdate.CameraUrl = CameraUrl;
            CamUpdate.HttpPort = HttpPort;
            CamUpdate.RtspPort = RtspPort;

            int res = camDataAccess.Updatecamera(CamUpdate);
            int churchId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));
            WowzaApi.WowzaHelper api = new WowzaApi.WowzaHelper();
            api.RequestCamera(Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId")), CamUpdate.CameraId, CamUpdate.CameraUrl);
            //api.StartRecording(churchId, CamUpdate.CameraId);
            //api.StopRecording(churchId, CamUpdate.CameraId);
            return Json(res);
        }
    }
}