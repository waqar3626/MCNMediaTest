using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev._Helper;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MCNMedia_Dev.Controllers
{

    public class ChurchDonationController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ChurchDonationDataAccessLayer DonationDataAccessLayer = new ChurchDonationDataAccessLayer();
        ChurchDataAccessLayer chdataAccess = new ChurchDataAccessLayer();
        
        public IActionResult ListDonation()
        {
            GenericModel gm = new GenericModel();

            try
            {
                int id = 0;
                if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
                {
                    id = (int)HttpContext.Session.GetInt32("ChurchId");
                }

                
                gm.Churches = chdataAccess.GetChurchData(id);
                ChurchDonation churchDonation = new ChurchDonation();
                return View(gm);
            }
            catch (Exception)
            {

                return View(gm);
            }
           
            
        }

        public JsonResult UpdateDonation(string ChurchDonationId, IFormFile mediaFile,  string EditWebsiteUrl, string ImageUrlChurch,bool ShowOnWebsite)
        {
            try
            {
                int churchId = Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId"));
                int userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                string churchName = HttpContext.Session.GetString("ChurchName");
                ChurchDonation donation = new ChurchDonation();
                if (mediaFile != null)
                {
                    donation.ImageUrl = FileUploadUtility.UploadFile(mediaFile, UploadingAreas.Donation, churchId, $"{churchName}{System.IO.Path.GetExtension(mediaFile.FileName)}");
                }
                else
                {
                    donation.ImageUrl = ImageUrlChurch;
                }
                donation.DonationId = Convert.ToInt32(ChurchDonationId);
                donation.ChurchId = churchId;
                donation.WebSiteUrl = EditWebsiteUrl;
                donation.ShowOnWebsite = ShowOnWebsite;
                donation.UpdatedBy = userId;
                int res = DonationDataAccessLayer.UpdateDonation(donation);

                string logMessage = $"Donate info for church '{churchName}' updated by {HttpContext.Session.GetString("UserName")} on {DateTime.Now}";
                ActivityLogDataAccessLayer.AddActivityLog(Operation.Update, Categories.Donate_Link, message: logMessage, churchId: churchId, userId: userId);
                return Json(new {success=true, res });
            }
            catch (Exception e)
            {
                return Json(new { success = false, responseText=e.Message });
               
            }
        }

        public IActionResult DeleteDonation(int id)
        {
            try
            {
                ChurchDonation donation = new ChurchDonation();
                int UpdateBy = (int)HttpContext.Session.GetInt32("UserId");
                bool res = DonationDataAccessLayer.DeleteDonation(id, UpdateBy);
                return Json(res);
            }
            catch (Exception e)
            {
                ShowMessage("Delete Donation Error" + e.Message);
                throw;
            }


        }


        private void ShowMessage(string exceptionMessage)
        {
            log.Info("Exception: " + exceptionMessage);
        }
    }
}
