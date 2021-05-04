using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MCNMedia_Dev.WowzaApi
{
    public class WowzaHelper2
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Readonly Properties

        private readonly string PORT;
        private readonly string API_VERSION;
        private readonly string SERVER;
        private readonly string VHOST;
        private readonly string APPLICATION;
        private readonly string USER_NAME;
        private readonly string SECURITY_KEY;

        #endregion

        #region Constructor

        public WowzaHelper2()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            var root = builder.Build();
            var wowzaConfig = root.GetSection("WowzaConfiguration");

            PORT = wowzaConfig["wowza_port"];
            API_VERSION = wowzaConfig["wowza_apiVersion"];
            SERVER = wowzaConfig["wowza_servers"];
            VHOST = wowzaConfig["wowza_vhosts"];
            APPLICATION = wowzaConfig["wowza_application"];
            USER_NAME = wowzaConfig["wowza_user"];
            SECURITY_KEY = wowzaConfig["wowza_secretKey"];
        }

        #endregion



        #region Public Method
        public bool RemoveCamera(int cameraId,int churchId)
        {
            try
            {
                string uniqueIdentifier = RetrieveChurchUniqueIdentifier(churchId);
              


                CameraDetail cameraDetail = new CameraDetail();
                cameraDetail.camera_id = uniqueIdentifier + "_" + cameraId.ToString();
                cameraDetail.client_name = "definst";

                cameraDetail.camera_slug = uniqueIdentifier;
                if (cameraDetail.camera_slug == "mobile_camera")
                {
                    cameraDetail.instance = "definst";
                }
                else
                {
                    cameraDetail.instance = "definst";
                }

              
                bool createStreamFile = PostAsync($"http://54.217.38.80:8182/api/v1/removeCamera", cameraDetail);
                return createStreamFile;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Error Occured - Request Camera. ", ex.Message);
                return false;
            }
        }

        public bool RequestCamera(int churchId, int cameraId, string rtspUrl)
        {
            try
            {
                log.InfoFormat("Request Camera for ChurchId: {0}, CameraId: {1} and RTSP: {2} - Start", churchId, cameraId, rtspUrl);
                string uniqueIdentifier = RetrieveChurchUniqueIdentifier(churchId);
                if (GetStream(uniqueIdentifier, cameraId))
                {
                    // Stream Exists
                    log.Info("Camera already registered.");
                    return false;
                }
                else
                {
                    // Create Stream
                    bool result = Stream_Create(uniqueIdentifier, cameraId, rtspUrl);
                    return result;
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Error Occured - Request Camera. ", ex.Message);
                return false;
            }
        }

       
     
        #endregion

        #region Helper Data Methods

        private string GetUniqueIdentifier(int churchId, int cameraId)
        {
            string uniqueIdentifier;
            Camera cam = new Camera();
            CameraDataAccessLayer cameraDataAccessLayer = new CameraDataAccessLayer();
            cam = cameraDataAccessLayer.GetCameraById(cameraId);
            if (cam.CameraType == _Helper.CameraType.AdminCamera)
            {
                uniqueIdentifier = cam.ChurchUniqueIdentifier;
            }
            else
            {
                uniqueIdentifier = cam.CameraUrl.Split("_")[0].ToString();
            }

            return uniqueIdentifier;
        }
        public bool StartRecording(int churchId, int cameraId,string UniqueIdentifier,string EventName,int EventId)
        {
            try
            {
                RecordingDetail recordingDetail = new RecordingDetail();
                recordingDetail.type = "rtmp";
                recordingDetail.camera_id = UniqueIdentifier.ToString()+"_"+ cameraId.ToString();
                recordingDetail.client_name = "definst";
                recordingDetail.recording_title = EventName;
                recordingDetail.event_id = EventId.ToString();

                bool startRec = PostAsync($"http://54.217.38.80:8182/api/v1/startRecording", recordingDetail);
                return startRec;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Error Occured - StartRecording. ", ex.Message);
                return false;
            }
        }
        public bool StopRecording(int churchId, int cameraId,string UniqueIdentifier)
        {
            try
            {
                log.InfoFormat("Recording Stopped for ChurchId: {0} and CameraId: {1} - Start", churchId, cameraId);
                RecordingDetail recordingDetail = new RecordingDetail();
                recordingDetail.type = "rtmp";
                recordingDetail.camera_id = UniqueIdentifier.ToString()+"_"+cameraId.ToString();
                recordingDetail.client_name ="definst" ;
             

                bool stopRec = PostAsync($"http://54.217.38.80:8182/api/v1/stopRecording", recordingDetail);
                return stopRec;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Error Occured - StopRecording. ", ex.Message);
                return false;
            }
        }

        private string RetrieveChurchUniqueIdentifier(int churchId)
        {
            Church church = new Church();
            ChurchDataAccessLayer churchDataAccessLayer = new ChurchDataAccessLayer();
            church = churchDataAccessLayer.GetChurchData(churchId);
            return church.UniqueIdentifier;
        }

        private string RetrieveCameraServerIP(int cameraId)
        {
            Camera cam = new Camera();
            CameraDataAccessLayer cameraDataAccessLayer = new CameraDataAccessLayer();
            cam = cameraDataAccessLayer.GetCameraById(cameraId);
            if (cam.ServerIP is null)
                cam.ServerIP = "54.217.38.80";
            return cam.ServerIP;
        }

        #endregion

        #region Helper Supporting Methods

        private bool GetStream(string churchIdentifier, int cameraId)
        {
            string streamName = $"{churchIdentifier}_{cameraId}";
            return GetAsync($"{GetApplicationUrl(cameraId)}/streamfiles/{streamName}", "");
        }

        private bool Stream_Create(string churchIdentifier, int cameraId, string rtspUrl)
        {
            bool connectStream = false;
            string streamFileName = $"{churchIdentifier}_{cameraId}";
            log.InfoFormat("Add Camera(CameraID: {0}) on wowza having stream file name: {1} - Start", cameraId, streamFileName);

            CameraDetail cameraDetail = new CameraDetail();
            cameraDetail.camera_id = churchIdentifier+"_"+cameraId.ToString();
            cameraDetail.client_name = "definst";
            cameraDetail.rtsp_url = rtspUrl;
            cameraDetail.camera_slug = churchIdentifier;
            if (cameraDetail.camera_slug == "mobile_camera") {
                cameraDetail.instance = "definst";
            }
            else
            {
                cameraDetail.instance = cameraDetail.camera_slug;
            }

            bool createStreamFile = PostAsync($"http://54.217.38.80:8182/api/v1/addCamera", cameraDetail);
            
            return connectStream;
        }

        private bool AddStreamToStartup(int cameraId, string streamName)
        {
            log.InfoFormat("Add Camera Stream(CameraID: {0}) to Startup.XML on wowza having stream file name: {1} - Start", cameraId, streamName);

            StartupStreamFile startupStreamFile = new StartupStreamFile();
            startupStreamFile.appName = APPLICATION;
            startupStreamFile.instance = "_definst_";
            startupStreamFile.mediaCasterType = "rtp";
            startupStreamFile.serverName = SERVER;
            startupStreamFile.streamName = streamName + ".stream";
            return PostAsync($"{GetBasicUri(cameraId)}/startupstreams", startupStreamFile);
        }

        private CredentialCache GetCredentials(Uri uri)
        {
            var credentialCache = new CredentialCache();
            credentialCache.Add(
            new Uri(uri.GetLeftPart(UriPartial.Authority)), // request url's host
            "Digest", // authentication type 
            new NetworkCredential(USER_NAME, SECURITY_KEY) // credentials 
            );
            return credentialCache;
        }

        private CredentialCache GetCredentials_Server3(Uri uri)
        {
            var credentialCache = new CredentialCache();
            credentialCache.Add(
            new Uri(uri.GetLeftPart(UriPartial.Authority)), // request url's host
            "Digest", // authentication type 
            new NetworkCredential(USER_NAME, "ykdEHGc6XH6e35") // credentials 
            );
            return credentialCache;
        }

        private string GetApplicationUrl(int cameraId)
        {
            string uri = $"{GetBasicUri(cameraId)}/applications/{APPLICATION}";
            return uri;
        }

        private string GetBasicUri(int cameraId)
        {
            string serverIP = RetrieveCameraServerIP(cameraId);
            string uri = $"http://{serverIP}:{PORT}/{API_VERSION}/servers/{SERVER}/vhosts/{VHOST}";
            return uri;
        }

        private string UpdateUriBasedOnServer(string uri, string uniqueIdentifier)
        {
            string instance = "_definst_";
            if (uri.Contains("52.51.59.126"))
            {
                instance = $"_{uniqueIdentifier}_";
            }
            return uri.Replace("#instance#", instance);
        }

        #endregion

        #region Helper Wowza Api Communication

        private bool PostAsync(string requestUri, object data)
        {
            log.Info("Wowza API - Post Event - Start");
            log.DebugFormat("Wowza Request URL: {0}", requestUri);
            HttpClient client = CreateHttpClientRequest(requestUri);

            var jsonData = JsonConvert.SerializeObject(data);
            log.DebugFormat("Wowza Sending Data: {0}", jsonData);
            var encodeData = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // List data response.
            HttpResponseMessage response = client.PostAsync(requestUri, encodeData).Result;
            AddLogsToLog4Net(response);
            log.Info("Wowza API - Post Event - End");
            return response.IsSuccessStatusCode;
        }

        private bool GetAsync(string requestUri, string data = "")
        {
            log.Info("Wowza API - Get Event - Start");
            log.DebugFormat("Wowza Request URL: {0}", requestUri);
            HttpClient client = CreateHttpClientRequest(requestUri);

            log.DebugFormat("Wowza Sending Data: {0}", data);
            // List data response.
            HttpResponseMessage response = client.GetAsync(data).Result;
            AddLogsToLog4Net(response);
            log.Info("Wowza API - Get Event - End");
            return response.IsSuccessStatusCode;
        }

        private bool PutAsync(string requestUri, object data)
        {
            log.Info("Wowza API - Update Event - Start");
            log.DebugFormat("Wowza Request URL: {0}", requestUri);
            HttpClient client = CreateHttpClientRequest(requestUri);

            var jsonData = JsonConvert.SerializeObject(data);
            log.DebugFormat("Wowza Sending Data: {0}", jsonData);
            var encodedData = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // List data response.
            HttpResponseMessage response = client.PutAsync(requestUri, encodedData).Result;
            AddLogsToLog4Net(response);
            log.Info("Wowza API - Update Event - End");
            return response.IsSuccessStatusCode;
        }

        private HttpClient CreateHttpClientRequest(string requestUri)
        {
            Uri uri = new Uri(requestUri);
            var HttpHandler = new HttpClientHandler();
            if (requestUri.Contains("52.51.59.126"))
            {
                HttpHandler.Credentials = GetCredentials_Server3(uri);
            }
            else
            {
                HttpHandler.Credentials = GetCredentials(uri);
            }
            HttpClient client = new HttpClient(HttpHandler);
            client.BaseAddress = new Uri(requestUri);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        #endregion

        private static void AddLogsToLog4Net(HttpResponseMessage response)
        {
            log.DebugFormat("Wowza - Raw Response: {0}", JsonConvert.SerializeObject(response));
            log.DebugFormat("Wowza - Response - Status: {0}", response.StatusCode);
            log.DebugFormat("Wowza - Response - Message: {0}", response.ReasonPhrase);
            log.DebugFormat("Wowza - Response - Request Message: {0}", response.RequestMessage);
            log.DebugFormat("Wowza - Response - IsSuccess: {0}", response.IsSuccessStatusCode);
        }
    }

   public class CameraDetail
    {
        public string camera_id { get; set; }
        public string client_name { get; set; }

        public string rtsp_url { get; set; }

        public string instance { get; set; }
        public string camera_slug { get; set; }

    }

    public class RecordingDetail
    {
        public string camera_id { get; set; }
        public string client_name { get; set; }

        public string type { get; set; }

        public string recording_title  { get; set; }
        public string event_id { get; set; }

    }
}
