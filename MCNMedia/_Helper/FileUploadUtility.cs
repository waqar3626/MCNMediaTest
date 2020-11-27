using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace MCNMedia_Dev._Helper
{
    public static class FileUploadUtility
    {
        /// <summary>
        /// File Upload Utility
        /// </summary>
        /// <param name="fileToUpload"></param>
        /// <returns></returns>
        public static string UploadFile(IFormFile fileToUpload, UploadingAreas uploadingArea)
        {
            string contentDirectory = ContentDirectory(uploadingArea, churchId: "");
            string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(fileToUpload.FileName);
            using (var stream = new MemoryStream())
            {
                fileToUpload.CopyTo(stream);
                AmazonUploader.SendMyFileToS3(stream, contentDirectory, fileName);
                //UploadFileToS3Bucket(contentDirectory, fileName, stream);
            }
            return Path.Combine(contentDirectory, fileName).Replace("\\","/");
        }

        public static string UploadFile(IFormFile fileToUpload, UploadingAreas uploadingArea, int churchId)
        {
            string contentDirectory = ContentDirectory(uploadingArea, churchId.ToString());
            string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(fileToUpload.FileName);
            using (var stream = new MemoryStream())
            {
                fileToUpload.CopyTo(stream);
                AmazonUploader.SendMyFileToS3(stream, contentDirectory, fileName);
                //UploadFileToS3Bucket(contentDirectory, fileName, stream);
            }
            return Path.Combine(contentDirectory, fileName).Replace("\\", "/");
        }

        public static string UploadFile(IFormFile fileToUpload, UploadingAreas uploadingArea, string fileName)
        {
            string contentDirectory = ContentDirectory(uploadingArea, churchId: "");
            using (var stream = new MemoryStream())
            {
                fileToUpload.CopyTo(stream);
                AmazonUploader.SendMyFileToS3(stream, contentDirectory, fileName);
            }
            return Path.Combine(contentDirectory, fileName).Replace("\\", "/");
        }

        public static string UploadFile(IFormFile fileToUpload, UploadingAreas uploadingArea, int churchId ,string fileName)
        {
            string contentDirectory = ContentDirectory(uploadingArea,churchId.ToString());
            using (var stream = new MemoryStream())
            {
                fileToUpload.CopyTo(stream);
                AmazonUploader.SendMyFileToS3(stream, contentDirectory, fileName);
                //UploadFileToS3Bucket(contentDirectory, fileName, stream);
            }
            return Path.Combine(contentDirectory, fileName).Replace("\\", "/");
        }

        private static string ContentDirectory(UploadingAreas uploadingAreas, string churchId)
        {
            string rootDirectory = "Uploads";
            switch (uploadingAreas)
            {
                case UploadingAreas.ChurchProfileImage:
                    return Path.Combine(rootDirectory, churchId, "ProfileImages");
                case UploadingAreas.Picture:
                    return Path.Combine(rootDirectory, churchId, "Pictures");
                case UploadingAreas.Video:
                    return Path.Combine(rootDirectory, churchId, "Videos");
                case UploadingAreas.SlideShow:
                    return Path.Combine(rootDirectory, churchId, "SlideShows");
                case UploadingAreas.UserProfileImage:
                    return Path.Combine(rootDirectory, churchId, "UserProfileImages");
                case UploadingAreas.NewsLetter:
                    return Path.Combine(rootDirectory, churchId, "NewsLetter");
                default:
                    return rootDirectory;
            }
        }

        private static void UploadFileToS3Bucket(string contentDirectory, string fileName, Stream stream)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            var root = builder.Build();
            var awsS3bucket = root.GetSection("S3BucketConfiguration");
            var sysConfig = root.GetSection("SystemConfiguration");

            RegionEndpoint bucketRegion = RegionEndpoint.EUWest1;
            string awsAccessKey = awsS3bucket["aws_access_key"];
            string awsSecretKey = awsS3bucket["aws_secret_key"];
            IAmazonS3 client = new AmazonS3Client(awsAccessKey, awsSecretKey, bucketRegion);

            string key = $"{sysConfig["system_mode"]}/{contentDirectory.Replace("\\", "/")}/{fileName}";

            TransferUtility utility = new TransferUtility(client);
            TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();

            request.BucketName = awsS3bucket["aws_bucket_name"];
            request.Key = key; //file name up in S3  
            request.InputStream = stream;
            utility.Upload(request); //commensing the transfer  
        }
    }

}
