using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace MCNMedia_Dev.Controllers
{
    public class CameraController : Controller
    {
        CameraDataAccessLayer camDataAccess = new CameraDataAccessLayer("a");
        GenericModel gm = new GenericModel();
        public IActionResult Index()
        {

            GenericModel gm = new GenericModel();
            gm.LCameras = camDataAccess.GetAllCameras();
            return View(gm);
        }


        [HttpPost]
        public IActionResult AddCamera(GenericModel cam)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
            {
                int churchId = (int)HttpContext.Session.GetInt32("ChurchId");
                cam.Cameras.ChurchId = churchId;

                camDataAccess.AddCamera(cam.Cameras);
                gm.ResultMessage = "Camera Added Sucessfully";

                gm.LCameras = camDataAccess.GetAllCameras();
                HttpContext.Session.SetString("TabName", "Camera");
                var queryString = new { chId = churchId };


                return RedirectToAction("ChurchDetails", "Church", queryString);
            }
            return RedirectToAction("Listchurch", "Church");
        }


        public IActionResult EditCamera(int id)
        {

            GenericModel gm = new GenericModel();
            gm.Cameras = camDataAccess.GetCameraById(id);
            //  return View(gm);
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
            List<Camera> cameraInfo = camDataAccess.GetAllCameras().ToList();
            return Json(cameraInfo);
            // return Json(new { data = cameraInfo });

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


            return Json(res);
        }
    }
}
