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
        
        public ChurchDonation GetDonationByChurch(int churchId)
        {
            ChurchDonation donation = new ChurchDonation();
            List<ChurchDonation> donationList = GetDonationFromDatabase(churchId, donationId: -1);
            if (donationList.Count > 0)
            {
                donation =  donationList.First();
            }
            return donation;
        }

        private List<ChurchDonation> GetDonationFromDatabase(int churchId,int donationId)
        {
            List<ChurchDonation> donations = new List<ChurchDonation>();
            _dc.CloseAndDispose();
            _dc.ClearParameters();
            _dc.AddParameter("Church_Id", churchId);
            _dc.AddParameter("Donation_Id", donationId);
            DataTable dataTable = _dc.ReturnDataTable("spDonation_Get");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                donations.Add(BindDonation(dataRow));
            }
            return donations;
        }

        private ChurchDonation BindDonation(DataRow dataRow)
        {
            ChurchDonation churchDonation = new ChurchDonation();
            churchDonation.DonationId = Convert.ToInt32(dataRow["DonationId"]);
            churchDonation.ChurchId = Convert.ToInt32(dataRow["ChurchId"]);
            churchDonation.ImageUrl = $"{AWS_S3_BUCKET_URI}/{dataRow["ImageUrl"]}";
            churchDonation.WebSiteUrl = dataRow["WebsiteUrl"].ToString();
            churchDonation.ShowOnWebsite = Convert.ToBoolean(dataRow["ShowOnWebsite"]);
            return churchDonation;
        }

        public int UpdateDonation(ChurchDonation donation)
        {
            _dc.CloseAndDispose();
            _dc.ClearParameters();
            _dc.AddParameter("Church_Id", donation.ChurchId);
            _dc.AddParameter("Image_Url", donation.ImageUrl);
            _dc.AddParameter("Website_Url", donation.WebSiteUrl);
            _dc.AddParameter("ShowOnWebsite", donation.ShowOnWebsite);
            _dc.AddParameter("UsrId", donation.UpdatedBy);

            return _dc.Execute("spDonation_Update");
        }

        public bool DeleteDonation(int DonationId, int UpdateBy)
        {
            _dc.CloseAndDispose();
            _dc.ClearParameters();
            _dc.AddParameter("Donation_Id", DonationId);
            _dc.AddParameter("UserId", UpdateBy);
            return _dc.ReturnBool("spDonation_Delete");
        }

    }
}
