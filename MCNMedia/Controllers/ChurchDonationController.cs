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
            int id = 0;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
            {
                id = (int)HttpContext.Session.GetInt32("ChurchId");
            }

            GenericModel gm = new GenericModel();
          
            gm.Churches = chdataAccess.GetChurchData(id);
            ChurchDonation churchDonation = new ChurchDonation();
            return View(gm);
        }


        public JsonResult ListDonationJson()
        {
            int ChurchId = (int)HttpContext.Session.GetInt32("ChurchId");
            List<ChurchDonation> churchDonations = DonationDataAccessLayer.GetDonationById(ChurchId).ToList();
            return Json(churchDonations);
        }


        [HttpPost]
        public JsonResult AddDonation(string WebsiteUrl, IFormFile mediaFile)
        {
            try
            {
                ChurchDonation churchDonation = new ChurchDonation();
                if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
                {
                    churchDonation.ChurchId = (int)HttpContext.Session.GetInt32("ChurchId");
                    churchDonation.UpdatedBy = (int)HttpContext.Session.GetInt32("UserId");
                    churchDonation.WebSiteUrl = WebsiteUrl;
                    if (mediaFile!=null)
                    {
                        churchDonation.ImageUrl = FileUploadUtility.UploadFile(mediaFile, UploadingAreas.Picture, churchDonation.ChurchId);

                    }
                    else
                    {
                        churchDonation.ImageUrl = "Uploads/47/Pictures/221a27b6-52ca-4756-b515-3f4c7bc27513.jpg";
                    }

                    int res = DonationDataAccessLayer.AddDonation(churchDonation);
                }
                return Json(1);
            }
            catch (Exception e)
            {
                ShowMessage("Add Donation Error" + e.Message);
                throw;
            }

        }
        public IActionResult _EditDonation(int id)
        {
            try
            {
                ChurchDonation donation = new ChurchDonation();
                donation = DonationDataAccessLayer.GetDonationByDonationId(id);
                return PartialView("_EditDonation", donation);

            }
            catch (Exception e)
            {
                ShowMessage("Edit Donation Error" + e.Message);
                throw;
            }


        }

        public JsonResult UpdateDonation(String ChurchDoonationId, IFormFile mediaFile,  String EditWebsiteUrl, string ImageUrlChurch)
        {
            try
            {
                ChurchDonation churchDonation = new ChurchDonation();


                if (mediaFile != null)
                {
                    churchDonation.ImageUrl = FileUploadUtility.UploadFile(mediaFile, UploadingAreas.Picture, Convert.ToInt32(HttpContext.Session.GetInt32("ChurchId")));
                  
                }
                else
                {
                    churchDonation.ImageUrl = ImageUrlChurch;
           

                }
                churchDonation.DoonationId = Convert.ToInt32(ChurchDoonationId);
                churchDonation.WebSiteUrl = EditWebsiteUrl;

                churchDonation.UpdatedBy = (int)HttpContext.Session.GetInt32("UserId");
                int res = DonationDataAccessLayer.UpdateDonation(churchDonation);


                return Json(res);
            }
            catch (Exception e)
            {
                ShowMessage("Update Donation Error" + e.Message);
                throw;
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
