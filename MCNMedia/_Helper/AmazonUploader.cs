using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev._Helper
{
    public class AmazonUploader
    {
        public static void SendMyFileToS3(Stream localFileStream,string subDirectoryInBucket, string fileNameInS3 )
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            var root = builder.Build();
            var awsS3bucket = root.GetSection("S3BucketConfiguration");
            var sysConfig = root.GetSection("SystemConfiguration");

            RegionEndpoint bucketRegion = RegionEndpoint.EUWest1;
            IAmazonS3 client = new AmazonS3Client(awsS3bucket["aws_access_key"], awsS3bucket["aws_secret_key"], bucketRegion);

            TransferUtility utility = new TransferUtility(client);
            TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();

            request.BucketName = $"{awsS3bucket["aws_bucket_name"]}/{sysConfig["system_mode"]}/{subDirectoryInBucket.Replace("\\", "/")}";
            request.Key = fileNameInS3; //file name up in S3  
            request.InputStream = localFileStream;
            utility.Upload(request); //commensing the transfer  
        }

    }
}
