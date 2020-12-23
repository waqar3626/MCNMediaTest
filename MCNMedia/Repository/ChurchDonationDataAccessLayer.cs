using MCNMedia_Dev.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Repository
{
    public class ChurchDonationDataAccessLayer
    {
        AwesomeDal.DatabaseConnect _dc;

        private readonly string AWS_S3_BUCKET_URI;

        public ChurchDonationDataAccessLayer()
        {
            _dc = new AwesomeDal.DatabaseConnect();
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            var root = builder.Build();
            var awsS3bucket = root.GetSection("S3BucketConfiguration");
            var sysConfig = root.GetSection("SystemConfiguration");
            AWS_S3_BUCKET_URI = $"{awsS3bucket["aws_bucket_url"]}/{sysConfig["system_mode"]}";
        }

        public int AddDonation(ChurchDonation donation)
        {
            _dc.ClearParameters();
            _dc.AddParameter("UserId", donation.UpdatedBy);
            _dc.AddParameter("WebsiteUrl", donation.WebSiteUrl);
            _dc.AddParameter("ImageUrl",  donation.ImageUrl);
            _dc.AddParameter("ChurchId", donation.ChurchId);
            return _dc.Execute("spChurchDonation_Insert");
        }

        public IEnumerable<ChurchDonation> GetDonationById(int ChurchId)
        {
            List<ChurchDonation> donation = new List<ChurchDonation>();

            _dc.ClearParameters();
            _dc.AddParameter("Church_Id", ChurchId);
            DataTable dataTable = _dc.ReturnDataTable("spChurchDonation_GetByChurchId");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                ChurchDonation churchDonation = new ChurchDonation();
                churchDonation.DoonationId = Convert.ToInt32(dataRow["DonationId"]);
                churchDonation.ChurchId = Convert.ToInt32(dataRow["ChurchId"]);
                churchDonation.ImageUrl = $"{AWS_S3_BUCKET_URI}/{dataRow["ImageUrl"]}";
                churchDonation.WebSiteUrl = dataRow["WebsiteUrl"].ToString();
                //churchDonation.ChurchName = dataRow["ChurchName"].ToString();
                donation.Add(churchDonation);
            }
            return donation;
        }
        public ChurchDonation GetDonationByDonationId(int donationId)
        {
            ChurchDonation donation = new ChurchDonation();

            _dc.ClearParameters();
            _dc.AddParameter("Donation_Id", donationId);
            DataTable dataTable = _dc.ReturnDataTable("spChurchDonationByDonationId");
            foreach (DataRow dataRow in dataTable.Rows)
            {

                donation.DoonationId = Convert.ToInt32(dataRow["DonationId"]);
                donation.ChurchId = Convert.ToInt32(dataRow["ChurchId"]);
                donation.ImageUrl = $"{AWS_S3_BUCKET_URI}/{dataRow["ImageUrl"]}";
                donation.WebSiteUrl = dataRow["WebsiteUrl"].ToString();
                //churchDonation.ChurchName = dataRow["ChurchName"].ToString();
                
            }
            return donation;
        }

        public int UpdateDonation(ChurchDonation churchDonation)
        {
            _dc.ClearParameters();
            _dc.AddParameter("DoonationId", churchDonation.DoonationId);
            _dc.AddParameter("Website_Url", churchDonation.WebSiteUrl);
            _dc.AddParameter("Image_Url", churchDonation.ImageUrl);
            _dc.AddParameter("UsrId", churchDonation.UpdatedBy);

            return _dc.Execute("spChurchDonation_Update");
        }


        public bool DeleteDonation(int DonationId, int UpdateBy)
        {
            _dc.ClearParameters();
            _dc.AddParameter("Donation_Id", DonationId);
            _dc.AddParameter("UserId", UpdateBy);
            return _dc.ReturnBool("spChurchDonation_Delete");
        }

    }
}
