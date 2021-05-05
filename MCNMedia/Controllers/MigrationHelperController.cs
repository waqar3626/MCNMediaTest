using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Threading;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

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
                //if (cam.IsCameraStreaming == false)
                //{
                //    wowzaHelper.RequestCameraStatus(cam.ChurchId, cam.CameraId);
                //}
                counter++;
                if (counter <= 1000)
                {
                    wowzaHelper = new WowzaApi.WowzaHelper();
                    wowzaHelper.RequestCamera(cam.ChurchId, cam.CameraId, cam.CameraUrl);
                    Thread.Sleep(100);
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

        public void CopyFiles()
        {
            ChurchDataAccessLayer churchData = new ChurchDataAccessLayer();
            List<Church> churches = churchData.GetByClientTypeChurch(-1).ToList();
            foreach (Church chrch in churches)
            {
                if (chrch.ImageURl.Contains("/"))
                {
                    string imageName = chrch.ImageURl.Split("/").Last();
                    string sourcePath = @"D:\aws\" + imageName;
                    string destinationPath = @"D:\aws\abc\" + imageName;
                    if (System.IO.File.Exists(sourcePath))
                    {
                        if (System.IO.File.Exists(destinationPath))
                        { }
                        else
                        {
                            System.IO.File.Copy(sourcePath, destinationPath);
                        }
                    }
                }
            }

        }

        public void RequestCameraStatus()
        {

            RequestCameraStatusa("52.51.59.126");

            RequestCameraStatusa("52.211.229.30");

        }

        private DataTable RequestCameraStatusa(string ip)
        {
            string error = "";
            DataTable dt = GetTable();
            StreamFiles streamFiles = new StreamFiles();
            StreamFile cameraStream = new StreamFile();
            try
            {
                string requestUrl = $"http://{ip}:8087/v2/servers/_defaultServer_/vhosts/_defaultVHost_/applications/live/streamfiles";
                HttpClient client = CreateHttpClientRequest(requestUrl);
                // List data response.
                HttpResponseMessage response = client.GetAsync("").Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = response.Content.ReadAsStringAsync().Result;
                    try
                    {
                        streamFiles = JsonConvert.DeserializeObject<StreamFiles>(responseBody);

                        foreach (var streamFile in streamFiles.streamFiles)
                        {

                            HttpClient clienta = CreateHttpClientRequest($"{requestUrl}/{streamFile.id}");
                            // List data response.
                            HttpResponseMessage responsea = clienta.GetAsync("").Result;
                            if (responsea.IsSuccessStatusCode)
                            {
                                var responseBodya = responsea.Content.ReadAsStringAsync().Result;
                                try
                                {
                                    cameraStream = JsonConvert.DeserializeObject<StreamFile>(responseBodya);
                                    int cameraId = 0;
                                    if (streamFile.id.Contains("_"))
                                    {
                                        string camId = streamFile.id.Split("_").Last();
                                        if (Microsoft.VisualBasic.Information.IsNumeric(camId))
                                        {
                                            cameraId = Convert.ToInt32(camId);
                                        }
                                    }
                                    dt.Rows.Add(cameraId, cameraStream.serverName, cameraStream.name, cameraStream.uri, ip, streamFile.id);
                                    CameraDataAccessLayer cameraDataAccessLayer = new CameraDataAccessLayer();
                                    cameraDataAccessLayer.WowzaUrl_Old_Add(cameraId, cameraStream.serverName, cameraStream.uri, ip, streamFile.id);
                                }
                                catch (Exception ex)
                                {
                                    error += ex.Message;
                                    _Helper.Common.SaveToXXX(ex.Message);
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        error += ex.Message;
                        _Helper.Common.SaveToXXX(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                error += ex.Message;
                _Helper.Common.SaveToXXX(ex.Message);
            }
            dt.Rows.Add(-1, "", "", "", "", error);
            return dt;
        }

        public static bool IsNumeric(object Expression)
        {
            double retNum;

            bool isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }
        public DataTable GetTable()
        {
            DataTable table = new DataTable();

            //' Create four typed columns in the DataTable.
            table.Columns.Add("CameraId", typeof(int));
            table.Columns.Add("serverName", typeof(string));
            table.Columns.Add("name", typeof(string));
            table.Columns.Add("uri", typeof(string));
            table.Columns.Add("version", typeof(string));
            table.Columns.Add("streamName", typeof(string));
            return table;
        }

        public void GetWowzaStreamURL()
        {
            StreamFile cameraStream = new StreamFile();
            CameraDataAccessLayer camDAL = new CameraDataAccessLayer();
            List<Camera> camList = new List<Camera>();
            camList = camDAL.GetAllAdminCameras();
            foreach(Camera cam in camList)
            {
                string serverIP;
                if (cam.ServerId == 1)
                    serverIP = "52.51.59.126";
                else
                    serverIP = "52.211.229.30";

                string requestUrl = $"http://{serverIP}:8087/v2/servers/_defaultServer_/vhosts/_defaultVHost_/applications/live/streamfiles";

                HttpClient clienta = CreateHttpClientRequest($"{requestUrl}/{cam.ChurchUniqueIdentifier}_{cam.CameraId}");
                // List data response.
                HttpResponseMessage responsea = clienta.GetAsync("").Result;
                if (responsea.IsSuccessStatusCode)
                {
                    var responseBodya = responsea.Content.ReadAsStringAsync().Result;
                    try
                    {
                        cameraStream = JsonConvert.DeserializeObject<StreamFile>(responseBodya);
                        camDAL.WowzaUrl_Required_Add(cam.CameraId, cameraStream.serverName, cameraStream.uri, serverIP, $"{cam.ChurchUniqueIdentifier}_{cam.CameraId}");
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
            }
        }

        private HttpClient CreateHttpClientRequest(string requestUri)
        {
            Uri uri = new Uri(requestUri);
            var HttpHandler = new HttpClientHandler();
            HttpHandler.Credentials = GetCredentials(uri);

            HttpClient client = new HttpClient(HttpHandler);
            client.BaseAddress = new Uri(requestUri);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        private CredentialCache GetCredentials(Uri uri)
        {
            var credentialCache = new CredentialCache();
            credentialCache.Add(
            new Uri(uri.GetLeftPart(UriPartial.Authority)), // request url's host
            "Digest", // authentication type 
            new NetworkCredential("mcnmedia", "ykdEHGc6XH6e35") // credentials 
            );
            return credentialCache;
        }
    }

    public class StreamFiles
    {
        public string serverName { get; set; }
        public List<StreamFileInfo> streamFiles { get; set; }
    }

    [DataContract]
    public class StreamFileInfo
    {
        [DataMember]
        public string id { get; set; }

        [DataMember]
        public string href { get; set; }
    }
    public class StreamFile
    {
        public string version { get; set; }
        public string serverName { get; set; }
        public string name { get; set; }
        public string uri { get; set; }
    }
}
