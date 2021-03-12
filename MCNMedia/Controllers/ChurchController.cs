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
using cloudscribe.Pagination.Models;
namespace MCNMedia_Dev.Controllers
{

    public class ChurchController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        ChurchDataAccessLayer churchDataAccess = new ChurchDataAccessLayer();
        CameraDataAccessLayer camDataAccess = new CameraDataAccessLayer();
        PlaceAccessLayer _placeAccessLayer = new PlaceAccessLayer();

    
        private IWebHostEnvironment environment;

        public ChurchController(IWebHostEnvironment _environment)
        {
            environment = _environment;
        }
        GenericModel gm = new GenericModel();

        [HttpGet]
        public IActionResult View_Load()
        {
            try
            {
                LoadClientDDL();
                LoadCountyDDL();
                return View();
            }
            catch (Exception e)
            {
                ViewBag.ErrorMsg = "Error Occurreds! " + e.Message;
                return View();
            }
        }

        [HttpGet]
        public IActionResult Listchurch()
        {
            List<Church> church = new List<Church>();
            try
            {
                LoadServerDDL();
                LoadClientDDL();
                LoadCountyDDL();
                LoadCountryDDL();
           
                Church chr = new Church();
                HttpContext.Session.SetInt32("ClientType", 0);
                HttpContext.Session.SetInt32("County", 0);
                chr.ChurchId = 1;
                chr.CountyId = -1;
                chr.ClientTypeId = -1;
                chr.ChurchName = "";
                chr.EmailAddress = "";
                chr.Phone = "";
                chr.Town = ""; 
                chr.CountryId = -1;
                 church = churchDataAccess.GetAllChurch(chr).ToList<Church>();
                if (Convert.ToInt32(TempData["SuccessMsg"]) == 1)
                {
                    ViewBag.SuccessMsg = "Church Deleted SuccessFully";
                }
                if (Convert.ToInt32(TempData["ErrorBit"]) == 1)
                {
                    ViewBag.ErrorMsg= "Error Occurreds! " + TempData["ErrorMsg"];
                    return View(church);
                }
                if (Convert.ToInt32(TempData["addSuccessMsg"]) == 1)
                {
                    ViewBag.SuccessMsg = "Church Added Successfully";
                    
                }
               
                return View(church);
            }
            catch (Exception exp)
            {
                LoadServerDDL();
                LoadClientDDL();
                LoadCountyDDL();
                LoadCountryDDL();

                ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                return View(church);
            }
        }

        [HttpGet]
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
                chr.Town = "";
                chr.CountryId = -1;
                churchDataAccess.GetAllChurch(chr);
                return View();
            }
            catch (Exception exp)
            {

                ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                return View();
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Church church = new Church();
            try
            {
                 church = churchDataAccess.GetChurchData(id);
                if (church == null)
                {
                    return NotFound();
                }

                LoadClientDDL();
                LoadCountyDDL();
                return View(church);
            }
            catch (Exception exp)
            {
                LoadClientDDL();
                LoadCountyDDL();
                ViewBag.ErrorMsgPartial = "Error Occurreds! " + exp.Message;
                return View(church);
            }
        }

        private List<Church> UserList(Church church)
        {
            List<Church> ListChurch = churchDataAccess.GetAllChurch(church).ToList<Church>();
            return ListChurch;


        }

        [HttpPost]
        public IActionResult AddChurch(Church church, IFormFile imageURl2)
        {
            try
            {
               
                string fileName = "missing-image.jpg";
                church.ImageURl = "Uploads/ProfileImages/missing-image.jpg";
                church.CreateBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                if (imageURl2 != null)
                {
                    fileName = Path.GetFileName(imageURl2.FileName);
                    church.ImageURl = FileUploadUtility.UploadFile(imageURl2, UploadingAreas.ChurchProfileImage); // Path.Combine(dirPath, fileName).Replace(@"\",@"/");
                }
                church.CreateBy = (int)HttpContext.Session.GetInt32("UserId");
                church.Slug = ((church.ChurchName + "-" + church.Town).ToLower()).Replace(" ", "-").Replace("'", "").Replace("_", "-").Replace("(", "-").Replace(".", "");
                churchDataAccess.AddChurch(church);
                List<Church> ListChurch = churchDataAccess.GetAllChurch(church).ToList<Church>();
                ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                TempData["addSuccessMsg"] = 1;
                return RedirectToAction("Listchurch", ListChurch);
            }
            catch (Exception exp)
            {
                Church chr = new Church();
                chr.ChurchId = -1;
                chr.CountyId = -1;
                chr.ClientTypeId = -1;
                chr.ChurchName = "";
                chr.EmailAddress = "";
                chr.Phone = "";
                chr.Town = "";
                chr.CountryId = -1;

                LoadServerDDL();
                LoadClientDDL();
                LoadCountyDDL();
                LoadCountryDDL();
                List<Church> ListChurch = churchDataAccess.GetAllChurch(chr).ToList<Church>();
                ViewBag.PartalBit = "-1";
                ViewBag.ErrorMsgPartial = "Error Occurreds! " + exp.Message;
                return View("Listchurch", ListChurch);
            }

        }
      [HttpGet]
        public IActionResult DeleteChurch(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return RedirectToAction("Listchurch", "Church");
                }
             
                int UpdateBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                churchDataAccess.DeleteChurch(id, UpdateBy);
                TempData["SuccessMsg"] = 1;
                return RedirectToAction("Listchurch");
                
            }
            catch (Exception e)
            {
                TempData["ErrorBit"] = 1;
                TempData["ErrorMsg"] = e.Message;
                return RedirectToAction("Listchurch");
            }
        }

        [HttpGet]
        public JsonResult Analytics(DateTime analyticDate)
        {
            try
            {
                string date = analyticDate.ToString("dd/mm/yyyy");
                if (analyticDate.ToString() == "1/1/0001 12:00:00 AM" || date == "01-00-0001")
                {
                    analyticDate = DateTime.Now;
                }
                GenericModel gm1 = new GenericModel();
                int churchId = (int)HttpContext.Session.GetInt32("ChurchId");

                gm1.Churches = churchDataAccess.GetChurchData(churchId);
                GoogleAnalytics googleantics = new GoogleAnalytics(environment);
                List<GoogleAnalyticsProperty> googleAnalytics = googleantics.GoogleAnalytics_GetByChurch(analyticDate);
                if (googleAnalytics.Count > 0)
                {
                    gm1.googleAnalytics = googleAnalytics.FindAll(x => x.PageTitle.Contains(gm1.Churches.Slug + " - MCN"));
                }
                ViewBag.ChurchId = churchId.ToString();
                ViewBag.SchDate = analyticDate;

                return Json(new {success=true, gm1.googleAnalytics });
            }
            catch (Exception exp)
            {
                return Json(new { success = false, exp.Message });
            }
            
        }

        [HttpGet]
        public IActionResult ChurchDetails(DateTime eventDate)
        {
            try
            {
                if (eventDate == Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                {
                    ViewBag.SchDate = DateTime.Now.ToString("dd-MMM-yyyy");
                    eventDate = DateTime.Now;
                }
                else
                {
                    ViewBag.SchDate = eventDate.ToString("dd-MMM-yyyy");
                }
                if (!string.IsNullOrEmpty(HttpContext.Request.Query["ch"].ToString()))
                {
                    LoadServerDDL();
                    int churchId = Convert.ToInt32(HttpContext.Request.Query["ch"].ToString());

                    HttpContext.Session.SetInt32("ChurchId", churchId);

                    Church church = churchDataAccess.GetChurchData(churchId);
                
                    gm.AnalyticsList = churchDataAccess.GetbyChurch(churchId, eventDate, eventDate).ToList();
                    gm.Churches = church;
                    if (church == null)
                    {
                        return NotFound();
                    }
                    gm.LCameras = camDataAccess.GetAdminCameraByChurch(churchId);
                    SetUpDataAccessLayer setupdataaccess = new SetUpDataAccessLayer();
                    gm.ChurchSetUp = setupdataaccess.getSetUp(churchId);
                    return View(gm);
                }
                return RedirectToAction("Listchurch", "Church");
            }
            catch (Exception e)
            {
                TempData["ErrorBit"] = 1;
                TempData["ErrorMsg"] = e.Message;
                return RedirectToAction("Listchurch");
            }

        }

        public void LoadClientDDL()
        {
            
                IEnumerable<ClientType> clientList = churchDataAccess.GetClients();
                List<SelectListItem> selectListItems = new List<SelectListItem>();
                foreach (var item in clientList)
                {
                    selectListItems.Add(new SelectListItem { Text = item.ClientTypeTitle.ToString(), Value = item.ClientTypeId.ToString() });
                }
                ViewBag.ClientTypes = selectListItems;
            
          
        }

        public void LoadCountryDDL()
        {
            
                IEnumerable<Place> countryList = _placeAccessLayer.GetCountries();
                List<SelectListItem> selectListItems = new List<SelectListItem>();
                foreach (var item in countryList)
                {
                    selectListItems.Add(new SelectListItem { Text = item.PlaceName.ToString(), Value = item.PlaceId.ToString() });
                }
                ViewBag.Countries = selectListItems;
          
        }


        public void LoadCountyDDL()
        {
            
                IEnumerable<Place> countyList = _placeAccessLayer.GetCounties(-1);
                List<SelectListItem> selectListItems = new List<SelectListItem>();
                foreach (var item in countyList)
                {
                    selectListItems.Add(new SelectListItem { Text = item.PlaceName.ToString(), Value = item.PlaceId.ToString() });
                }
                ViewBag.Counties = selectListItems;
           
        }

        [HttpPost]
        public IActionResult UpdateChurch(int ChurchId, [Bind] GenericModel church, IFormFile imageURl2, string ImageUrl, string InstallationDate)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserType")))
                {
                    return RedirectToAction("Listchurch", "Church");
                }
                string fileName = "";
                if (imageURl2 != null)
                {
                    fileName = Path.GetFileName(imageURl2.FileName);
                    church.Churches.ImageURl = FileUploadUtility.UploadFile(imageURl2, UploadingAreas.ChurchProfileImage); // Path.Combine(dirPath, fileName).Replace(@"\", @"/"); 
                }
                else
                {
                    int pos = ImageUrl.IndexOf("Upload");
                    if (pos >= 0)
                    {
                        // Remove everything before url but include Upload 
                        string beforeFounder = ImageUrl.Remove(0, pos);
                        church.Churches.ImageURl = beforeFounder;
                    }
                }
                ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                ChurchId = church.Churches.ChurchId;
                church.Churches.InstallationDate = Convert.ToDateTime(InstallationDate);
                if (ModelState.IsValid)
                {
                    church.Churches.UpdateBy = (int)HttpContext.Session.GetInt32("UserId");
                    churchDataAccess.UpdateChurch(church.Churches);
                    var queryString = new { chId = ChurchId };
                    return RedirectToAction("ChurchDetails", "Church", queryString);
                }
                else
                {
                    return View("_ChurchInfo", church);
                }

            }
            catch (Exception e)
            {
                TempData["ErrorBit"] = 1;
                TempData["ErrorMsg"] = e.Message;
                return RedirectToAction("Listchurch");
            }

        }
        [HttpPost]
        public IActionResult SearchChurch(string ChurchName, int ClientType, string EmailAddress, int County, int Country, string town, string PhoneNo)
        {
            List<Church> church = new List<Church>();
            try
            {
            
                LoadClientDDL();
                LoadCountyDDL();
                LoadCountryDDL();
                HttpContext.Session.SetInt32("ClientType", ClientType);
                HttpContext.Session.SetInt32("County", County);
                HttpContext.Session.SetInt32("countryId", Country);
                Church chr = new Church();

                chr.ChurchName = ChurchName;
                chr.ClientTypeId = ClientType;
                chr.EmailAddress = EmailAddress;
                chr.CountyId = County;
                chr.Town = town;
                chr.CountryId = Country;
                chr.Phone = PhoneNo;
                church = churchDataAccess.GetAllChurch(chr).ToList<Church>();
   
                return View("/Views/Church/Listchurch.cshtml", church);
            }
            catch (Exception exp)
            {

                LoadClientDDL();
                LoadCountyDDL();
                LoadCountryDDL();
                ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                return View("/Views/Church/Listchurch.cshtml", church);
            }
        }

        public void LoadServerDDL()
        {
            
                ServerDAL serverDAL = new ServerDAL();
                List<Server> serverList = serverDAL.GetServer();
                List<SelectListItem> selectListItems = new List<SelectListItem>();
                foreach (var item in serverList)
                {
                    selectListItems.Add(new SelectListItem { Text = item.ServerName.ToString(), Value = item.ServerId.ToString() });
                }
                ViewBag.Server = selectListItems;

           
        }

      
    }
}
