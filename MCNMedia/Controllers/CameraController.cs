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
       
    }
}
