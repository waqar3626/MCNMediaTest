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
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ListDonation()
        {
            ChurchDonation donation = new ChurchDonation();
            donation.ChurchId = (int)HttpContext.Session.GetInt32("ChurchId");
            List<ChurchDonation> churchDonations = DonationDataAccessLayer.GetDonationById(donation).ToList();
            return View(churchDonations);
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
                    churchDonation.ImageUrl = FileUploadUtility.UploadFile(mediaFile, UploadingAreas.Picture, churchDonation.ChurchId);

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
        public IActionResult EditDonation(int id)
        {
            try
            {
                ChurchDonation donation = new ChurchDonation();
                donation.ChurchId=(int)HttpContext.Session.GetInt32("ChurchId");
                //donation = DonationDataAccessLayer.GetDonationById(donation.ChurchId);
                return PartialView("EditPicture", donation);
            }
            catch (Exception e)
            {
                ShowMessage("Edit Donation Error" + e.Message);
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
