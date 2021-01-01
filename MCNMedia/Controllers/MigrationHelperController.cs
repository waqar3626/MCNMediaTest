using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MCNMedia_Dev.Controllers
{
    public class MigrationHelperController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public void MigrateCameras()
        {
            CameraDataAccessLayer camDataAccess = new CameraDataAccessLayer();
            List<Camera> cameraList = camDataAccess.GetAllAdminCameras().ToList<Camera>();

            MCNMedia_Dev.WowzaApi.WowzaHelper wowzaHelper = new WowzaApi.WowzaHelper();
            int counter = 0;
            foreach (Camera cam in cameraList)
            {
                counter++;
                if (counter <= 1000)
                {
                    wowzaHelper = new WowzaApi.WowzaHelper();
                    wowzaHelper.RequestCamera(cam.ChurchId, cam.CameraId, cam.CameraUrl);
                    Thread.Sleep(1000);
                }

            }
        }

        public void CopyNewsletter()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            var root = builder.Build();
            var awsS3bucket = root.GetSection("S3BucketConfiguration");
            var sysConfig = root.GetSection("SystemConfiguration");

            RegionEndpoint bucketRegion = RegionEndpoint.EUWest1;
            IAmazonS3 client = new AmazonS3Client(awsS3bucket["aws_access_key"], awsS3bucket["aws_secret_key"], bucketRegion);

            ChurchNewsLetterDataAccessLayer churchNewsLetterDataAccessLayer = new ChurchNewsLetterDataAccessLayer();
            List<NewsLetter> newsLetters = churchNewsLetterDataAccessLayer.GetAllNewsLetter().ToList();
            foreach (NewsLetter ns in newsLetters)
            {
                try
                {
                    CopyObjectRequest requestCopy = new CopyObjectRequest
                    {
                        SourceBucket = awsS3bucket["aws_bucket_name"],
                        SourceKey = $"Uploads/newsletter/{ns.NewsLetterName}",
                        DestinationBucket = $"mcnmedia/{sysConfig["system_mode"]}",
                        DestinationKey = $"Uploads/{ns.ChurchId}/NewsLetter/{ns.NewsLetterName}"
                    };
                    CopyObjectResponse response = client.CopyObjectAsync(requestCopy).Result;
                }
                catch (Exception e)
                { }
            }
        }

        public void CopyProfileImage()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            var root = builder.Build();
            var awsS3bucket = root.GetSection("S3BucketConfiguration");
            var sysConfig = root.GetSection("SystemConfiguration");

            RegionEndpoint bucketRegion = RegionEndpoint.EUWest1;
            IAmazonS3 client = new AmazonS3Client(awsS3bucket["aws_access_key"], awsS3bucket["aws_secret_key"], bucketRegion);

            ChurchDataAccessLayer churchData = new ChurchDataAccessLayer();
            List<Church> churches = churchData.GetByClientTypeChurch(-1).ToList();
            foreach (Church chrch in churches)
            {
                if (chrch.ImageURl.Contains("/"))
                {
                    string imageName = chrch.ImageURl.Split("/").Last();
                    try
                    {
                        CopyObjectRequest requestCopy = new CopyObjectRequest
                        {
                            SourceBucket = "mcnmedia-app",
                            SourceKey = $"Uploads/ProfileImages/{imageName}",
                            DestinationBucket = $"mcnmedia/{sysConfig["system_mode"]}",
                            DestinationKey = $"Uploads/ProfileImages/{imageName}"
                        };
                        CopyObjectResponse response = client.CopyObjectAsync(requestCopy).Result;
                    }
                    catch (Exception e)
                    { }
                }
            }
        }



    }
}
