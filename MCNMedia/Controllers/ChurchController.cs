using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static MCNMedia_Dev.Models.Church;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using MCNMedia_Dev.Repository;
using System.IO;
using MCNMedia_Dev._Helper;

namespace MCNMedia_Dev.Controllers
{
    public class ChurchController : Controller
    {

        ChurchDataAccessLayer churchDataAccess = new ChurchDataAccessLayer();
        CameraDataAccessLayer camDataAccess = new CameraDataAccessLayer();

        private IWebHostEnvironment environment;

        public ChurchController(IWebHostEnvironment _environment)
        {
            environment = _environment;
        }



        [HttpGet]
        public IActionResult AddChurch()
        {
           
            LoadClientDDL();
            LoadCountyDDL();
            return View();
        }

        [HttpGet]
        public IActionResult Listchurch()
        {
            LoadServerDDL();
            LoadClientDDL();
            LoadCountyDDL();
           
            Church chr = new Church();

            HttpContext.Session.SetInt32("ClientType", 0);
            HttpContext.Session.SetInt32("County", 0);
            chr.ChurchId = 1;
            chr.CountyId = -1;
            chr.ClientTypeId = -1;
            chr.ChurchName = "";
            chr.EmailAddress = "";
            chr.Phone = "";
            List<Church> church = churchDataAccess.GetAllChurch(chr).ToList<Church>();
            return View(church);

        }
        [HttpGet()]
        public IActionResult GetAllChurch()
        {
            Church chr = new Church();
            chr.ChurchId = 1;
            chr.CountyId = -1;
            chr.ClientTypeId = -1;
            chr.ChurchName = "";
            chr.EmailAddress = "";
            chr.Phone = "";
            churchDataAccess.GetAllChurch(chr);
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Church church = churchDataAccess.GetChurchData(id);
            if (church == null)
            {
                return NotFound();
            }

            LoadClientDDL();
            LoadCountyDDL();
            return View(church);
        }

        [HttpPost]
        public IActionResult AddChurch(Church church, IFormFile imageURl2)
        {
            string fileName = Path.GetFileName(imageURl2.FileName);
            church.ImageURl = FileUploadUtility.UploadFile(imageURl2, UploadingAreas.ChurchProfileImage); // Path.Combine(dirPath, fileName).Replace(@"\",@"/");
            churchDataAccess.AddChurch(church);
            ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
            return RedirectToAction("Listchurch");
        }

        [HttpPost]
        public IActionResult Edit(int id, [Bind] Church church)
        {
            if (id != church.ChurchId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                churchDataAccess.UpdateChurch(church);
                return RedirectToAction("Listchurch");
            }
            return View(church);
        }

        public IActionResult Delete(int id)
        {
            churchDataAccess.DeleteChurch(id);
            return RedirectToAction("Listchurch");
        }


        [HttpGet]
        public IActionResult ChurchDetails()
        {
            GenericModel gm = new GenericModel();
            if (!string.IsNullOrEmpty(HttpContext.Request.Query["chId"].ToString()))
            {
                LoadServerDDL();
                int churchId = Convert.ToInt32(HttpContext.Request.Query["chId"].ToString());

                HttpContext.Session.SetInt32("ChurchId", churchId);

                Church church = churchDataAccess.GetChurchData(churchId);

                gm.Churches = church;
                if (church == null)
                {
                    return NotFound();
                }
                gm.LCameras = camDataAccess.GetAllCameras(churchId);
                SetUpDataAccessLayer setupdataaccess = new SetUpDataAccessLayer();
                gm.ChurchSetUp = setupdataaccess.getSetUp(churchId);
                return View(gm);
            }
            return RedirectToAction("Listchurch", "Church");
        }

        public void LoadClientDDL()
        {
            IEnumerable<ClientType> clientList = churchDataAccess.GetClients();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (var item in clientList)
            {
                selectListItems.Add(new SelectListItem { Text = item.ClientTypeTitle.ToString(), Value = item.ClientTypeId.ToString()});
            }
            ViewBag.ClientTypes = selectListItems;
        }

        public void LoadCountyDDL()
        {
            IEnumerable<Counties> countyList = churchDataAccess.GetCounties(-1);
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (var item in countyList)
            {
                selectListItems.Add(new SelectListItem { Text = item.CountyName.ToString(), Value = item.CountyId.ToString() });
            }
            ViewBag.Counties = selectListItems;

        }

        

        [HttpPost]

        public IActionResult UpdateChurch(int ChurchId, [Bind] GenericModel church, IFormFile imageURl2, string ImageUrl)
        {
            string fileName = "";
            if (imageURl2 != null)
            {
                 fileName = Path.GetFileName(imageURl2.FileName);
                church.Churches.ImageURl = FileUploadUtility.UploadFile(imageURl2, UploadingAreas.ChurchProfileImage); // Path.Combine(dirPath, fileName).Replace(@"\", @"/"); 
            }
            else {
                church.Churches.ImageURl = ImageUrl;
            }
                ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
            ChurchId = church.Churches.ChurchId;

            if (ModelState.IsValid)
            {
                churchDataAccess.UpdateChurch(church.Churches);
                var queryString = new { chId = ChurchId };
                return RedirectToAction("ChurchDetails", "Church", queryString);
            }
            return View(church);
        }

        public IActionResult Search(string ChurchName, int ClientType, string EmailAddress, int County, string PhoneNo)
        {
            LoadClientDDL();
            LoadCountyDDL();

           
         HttpContext.Session.SetInt32("ClientType", ClientType);
            HttpContext.Session.SetInt32("County", County);
            Church chr = new Church();
            chr.ChurchId = 1;
            chr.ChurchName = ChurchName;
            chr.ClientTypeId = ClientType;
            chr.EmailAddress = EmailAddress;
            chr.CountyId = County;
            chr.Phone = PhoneNo;
            List<Church> church = churchDataAccess.GetAllChurch(chr).ToList<Church>();
            return View("/Views/Church/Listchurch.cshtml", church);

            



        }

        public void LoadServerDDL()
        {
            IEnumerable<Server> serverList = camDataAccess.GetServer();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (var item in serverList)
            {
                selectListItems.Add(new SelectListItem { Text = item.ServerName.ToString(), Value = item.ServerId.ToString() });
            }
            ViewBag.Server = selectListItems;

        }
    }
}
