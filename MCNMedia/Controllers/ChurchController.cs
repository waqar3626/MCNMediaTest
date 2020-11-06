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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            try
            {
                LoadClientDDL();
                LoadCountyDDL();
                return View();
            }
            catch (Exception e)
            {
                ShowMessage("Add Church Error" + e.Message);
                throw;

            }
           
           
        }

        [HttpGet]
        public IActionResult Listchurch()
        {
            try {
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

            catch(Exception e)
            {
                    ShowMessage("List Church Error" + e.Message);
                    throw;
            }
         

        }
        [HttpGet()]
        public IActionResult GetAllChurch()
        {
            try
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
            catch (Exception e)
            {
                    ShowMessage(" All Churches Error" + e.Message);
                    throw;
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            try {
                    Church church = churchDataAccess.GetChurchData(id);
                    if (church == null)
                    {
                        return NotFound();
                    }

                    LoadClientDDL();
                    LoadCountyDDL();
                    return View(church);
            }
            catch (Exception e)
            {
                ShowMessage(" Edit Church Error" + e.Message);
                throw;
            }

        }

        [HttpPost]
        public IActionResult AddChurch(Church church, IFormFile imageURl2)
        {
            try
            {
                church.CreateBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                string fileName = Path.GetFileName(imageURl2.FileName);
                church.ImageURl = FileUploadUtility.UploadFile(imageURl2, UploadingAreas.ChurchProfileImage); // Path.Combine(dirPath, fileName).Replace(@"\",@"/");
                church.CreateBy = (int)HttpContext.Session.GetInt32("UserId");
                churchDataAccess.AddChurch(church);
                ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                return RedirectToAction("Listchurch");
            }
            catch (Exception e)
            {
                ShowMessage(" Add Church Error" + e.Message);
                throw;
            }

        }
       
        public IActionResult Delete(int id)
        {
            try
            {
                int UpdateBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                churchDataAccess.DeleteChurch(id, UpdateBy);
                return RedirectToAction("Listchurch");
            }
            catch (Exception e)
            {
                ShowMessage(" Delete Church Error" + e.Message);
                throw;
            }
        }

        [HttpGet]
        public IActionResult ChurchDetails()
        {
            try
            {
                GenericModel gm = new GenericModel();
                if (!string.IsNullOrEmpty(HttpContext.Request.Query["ch"].ToString()))
                {
                    LoadServerDDL();
                    int churchId = Convert.ToInt32(HttpContext.Request.Query["ch"].ToString());

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
            catch (Exception e)
            {
                ShowMessage(" Church Detail Error" + e.Message);
                throw;
            }

        }

       

        public void LoadClientDDL()
        {
            try
            {
                IEnumerable<ClientType> clientList = churchDataAccess.GetClients();
                List<SelectListItem> selectListItems = new List<SelectListItem>();
                foreach (var item in clientList)
                {
                    selectListItems.Add(new SelectListItem { Text = item.ClientTypeTitle.ToString(), Value = item.ClientTypeId.ToString() });
                }
                ViewBag.ClientTypes = selectListItems;
            }
            catch (Exception e)
            {
                ShowMessage(" Client Type Can't load Error" + e.Message);
                throw;
            }
           
        }

        public void LoadCountyDDL()
        {
            try
            {
                IEnumerable<Counties> countyList = churchDataAccess.GetCounties(-1);
                List<SelectListItem> selectListItems = new List<SelectListItem>();
                foreach (var item in countyList)
                {
                    selectListItems.Add(new SelectListItem { Text = item.CountyName.ToString(), Value = item.CountyId.ToString() });
                }
                ViewBag.Counties = selectListItems;

            }
            catch (Exception e)
            {
                ShowMessage(" County Can't Load Error" + e.Message);
                throw;
            }
           

        }

        

        [HttpPost]
        public IActionResult UpdateChurch(int ChurchId, [Bind] GenericModel church, IFormFile imageURl2, string ImageUrl)
        {
            try
            {
                string fileName = "";
                if (imageURl2 != null)
                {
                    fileName = Path.GetFileName(imageURl2.FileName);
                    church.Churches.ImageURl = FileUploadUtility.UploadFile(imageURl2, UploadingAreas.ChurchProfileImage); // Path.Combine(dirPath, fileName).Replace(@"\", @"/"); 
                }
                else
                {
                    church.Churches.ImageURl = ImageUrl;
                }
                ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                ChurchId = church.Churches.ChurchId;

                if (ModelState.IsValid)
                {
                    church.Churches.UpdateBy = (int)HttpContext.Session.GetInt32("UserId");
                    churchDataAccess.UpdateChurch(church.Churches);
                    var queryString = new { chId = ChurchId };
                    return RedirectToAction("ChurchDetails", "Church", queryString);
                }
                return View("_ChurchInfo",church);
            }
            catch (Exception e)
            {
                ShowMessage("Update Church  Error" + e.Message);
                throw;
            }
           
        }

        public IActionResult Search(string ChurchName, int ClientType, string EmailAddress, int County, string PhoneNo)
        {
            try
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
            catch (Exception e)
            {
                ShowMessage("Search Church  Error" + e.Message);
                throw;
            }
           
            



        }

        public void LoadServerDDL()
        {
            try
            {
                IEnumerable<Server> serverList = camDataAccess.GetServer();
                List<SelectListItem> selectListItems = new List<SelectListItem>();
                foreach (var item in serverList)
                {
                    selectListItems.Add(new SelectListItem { Text = item.ServerName.ToString(), Value = item.ServerId.ToString() });
                }
                ViewBag.Server = selectListItems;

            }
            catch (Exception e)
            {
                ShowMessage("Server Can't Load  Error" + e.Message);
                throw;
            }
           

        }
        private void ShowMessage(string exceptionMessage)
        {
            log.Info("Exception: " + exceptionMessage);
        }
    }
}
