using MCNMedia_Dev._Helper;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json; 
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Controllers
{
    public class CameraController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private HttpClient client;
        CameraDataAccessLayer camDataAccess = new CameraDataAccessLayer();
        GenericModel gm = new GenericModel();

        public CameraController()
        {
            client = new HttpClient();
        }
        public IActionResult Index()
        {
            GenericModel gm = new GenericModel();
            try
            {

                int churchId = (int)HttpContext.Session.GetInt32("ChurchId");
                gm.LCameras = camDataAccess.GetAdminCameraByChurch(churchId);
                return View(gm);
            }
            catch (Exception e)
            {
                return View(gm);
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
            catch (Exception exp)
            {
                return Json(new { success = false, responseText = exp.Message });
            }
        }

        public IActionResult EditCamera(int id)
        {
            GenericModel gm = new GenericModel();
            try
            {

                gm.Cameras = camDataAccess.GetCameraById(id);
                return PartialView("_EditCamera", gm);
            }
            catch (Exception e)
            {
                ViewBag.PartalBit = "-2";
                ViewBag.ErrorMsgPartial = "Error Occurreds! " + e.Message;
                return PartialView("_EditCamera", gm);
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
                
                    return Json(new { success = true, res });
            }
            catch (Exception e)
            {
                return Json(new { success = false, responseText = e.Message });
            }

        }

        public IActionResult GetAllCameras()
        {
            Camera camera = new Camera();
            try
            {

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
                if (HttpContext.Session.GetString("UserType") == "admin")
                {
                    ViewBag.ErrorMsg = "Error Occurreds! " + e.Message;
                    return View("ChurchDetails", gm);
                }
                else if (HttpContext.Session.GetString("UserType") == "client")
                {
                    ViewBag.ErrorMsg = "Error Occurreds! " + e.Message;
                    return View("Views/Client/CameraDetail", gm);
                }
                return Json(new { success = false, responseText = e.Message });
            }

        }

        public JsonResult UpdateCamera(string CameraId, string CameraName, string CameraUrl, string HttpPort, string RtspPort)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return Json(new { Url = "UserLogin" });
                }
                Camera CamUpdate = new Camera();
                CamUpdate.CameraId = Convert.ToInt32(CameraId);
                CamUpdate.CameraName = CameraName;
                CamUpdate.CameraUrl = CameraUrl;
                CamUpdate.HttpPort = HttpPort;
                CamUpdate.RtspPort = RtspPort;
                CamUpdate.UpdatedBy = (int)HttpContext.Session.GetInt32("UserId");
                int res = camDataAccess.UpdateCamera(CamUpdate);

                int churchId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));
                return Json(new { success = true, responseText = "The attached file is not supported." });
            }
            catch (Exception exp)
            {
                return Json(new { success = false, responseText = exp.Message });
            }
        }
        [HttpPost]
        public JsonResult RevokeCamera(int cameraId)
        {
            try
            {
                Wowza wowza = new Wowza();
                return Json(wowza.SyncCamerasWithWowzaById(cameraId));
            }
            catch (Exception e)
            {

                return Json(new { success = false, responseText = e.Message });
            }

        }

        public async Task<Camera> GetCameras(int id)
        {
            var response = await client.GetAsync(
                "http://michalbialeckicomnetcoreweb20180417060938.azurewebsites.net/api/users/" + id)
                .ConfigureAwait(false);
            var user = JsonConvert.DeserializeObject<Camera>(await response.Content.ReadAsStringAsync());

            return user;
        }
        public async Task<IEnumerable<Camera>> GetUsers(IEnumerable<int> ids)
        {
            var response = await client
                .PostAsync(
                    "http://michalbialeckicomnetcoreweb20180417060938.azurewebsites.net/api/users/GetMany",
                    new StringContent(JsonConvert.SerializeObject(ids), Encoding.UTF8, "application/json"))
                .ConfigureAwait(false);

            var users = JsonConvert.DeserializeObject<IEnumerable<Camera>>(await response.Content.ReadAsStringAsync());

            return users;
        }
        public async Task<IEnumerable<Camera>> GetUsersInParallelInWithBatches(IEnumerable<int> cameraId)
        {
            var tasks = new List<Task<IEnumerable<Camera>>>();
            var batchSize = 100;
            int numberOfBatches = (int)Math.Ceiling((double)cameraId.Count() / batchSize);

            for (int i = 0; i < numberOfBatches; i++)
            {
                var currentIds = cameraId.Skip(i * batchSize).Take(batchSize);
                //tasks.Add(client.GetCameras(currentIds));
            }

            return (await Task.WhenAll(tasks)).SelectMany(u => u);
        }
    }
}