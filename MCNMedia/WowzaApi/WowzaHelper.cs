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
    public class WowzaHelper
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

        public WowzaHelper()
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

        public bool StartRecording(int churchId, int cameraId)
        {
            try
            {
                log.InfoFormat("StartRecording Event Called for ChurchId: {0} and CameraId: {1} - Start", churchId, cameraId);
                string uniqueIdentifier = RetrieveChurchUniqueIdentifier(churchId);
                log.DebugFormat("Church Unique Identifier: {0}", uniqueIdentifier);
                RecordingData recordingData = new RecordingData();

                recordingData.recorderName = $"{uniqueIdentifier}_{ cameraId}.stream";

                if (GetStream(uniqueIdentifier, cameraId))
                {
                    bool startRec = PostAsync($"{GetApplicationUrl(cameraId)}/instances/_definst_/streamrecorders", recordingData);
                    return startRec;
                }
                else
                {
                    log.Error($"Stream for recording not exists. (ChurchId: {churchId}, CameraId: {cameraId}, Unique Id: {uniqueIdentifier})");
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Error Occured - StartRecording. ", ex.Message);
                return false;
            }
        }

        public bool StopRecording(int churchId, int cameraId)
        {
            try
            {
                log.InfoFormat("Recording Stopped for ChurchId: {0} and CameraId: {1} - Start", churchId, cameraId);
                string uniqueIdentifier = RetrieveChurchUniqueIdentifier(churchId);
                log.DebugFormat("Church Unique Identifier: {0}", uniqueIdentifier);
                if (GetStream(uniqueIdentifier, cameraId))
                {
                    bool stopRec = PutAsync($"{GetApplicationUrl(cameraId)}/instances/_definst_/streamrecorders/{uniqueIdentifier}_{cameraId}.stream/actions/stopRecording", "");
                    return stopRec;
                }
                else
                {
                    log.Error($"Stream for recording not exists. (ChurchId: {churchId}, CameraId: {cameraId}, Unique Id: {uniqueIdentifier})");
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Error Occured - StopRecording. ", ex.Message);
                return false;
            }
        }

        #endregion

        #region Helper Data Methods

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

            StreamFile streamFile = new StreamFile();
            streamFile.name = streamFileName;
            streamFile.serverName = SERVER;
            streamFile.uri = rtspUrl;

            bool createStreamFile = PostAsync($"{GetApplicationUrl(cameraId)}/streamfiles", streamFile);
            if (createStreamFile)
            {
                log.Info("Camera added on wowza");
                connectStream = PutAsync($"{GetApplicationUrl(cameraId)}/streamfiles/{streamFileName}/actions/connect?connectAppName={APPLICATION}&appInstance=_definst_&mediaCasterType=rtp", "");
                if (connectStream)
                {
                    AddStreamToStartup(cameraId, streamFileName);
                    log.Info("Camera connected on wowza successfully");
                }
                else
                    log.Error("Somthing went wrong, camera didn't connect sucessfully");
            }
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
            log.DebugFormat("Wowza - Raw Response: {0}", JsonConvert.SerializeObject(response));
            log.DebugFormat("Wowza - Response - Status: {0}", response.StatusCode);
            log.DebugFormat("Wowza - Response - Message: {0}", response.ReasonPhrase);
            log.DebugFormat("Wowza - Response - Request Message: {0}", response.RequestMessage);
            log.DebugFormat("Wowza - Response - IsSuccess: {0}", response.IsSuccessStatusCode);
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
            log.DebugFormat("Wowza - Raw Response: {0}", JsonConvert.SerializeObject(response));
            log.DebugFormat("Wowza - Response - Status: {0}", response.StatusCode);
            log.DebugFormat("Wowza - Response - Message: {0}", response.ReasonPhrase);
            log.DebugFormat("Wowza - Response - Request Message: {0}", response.RequestMessage);
            log.DebugFormat("Wowza - Response - IsSuccess: {0}", response.IsSuccessStatusCode);
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
            log.DebugFormat("Wowza - Raw Response: {0}", JsonConvert.SerializeObject(response));
            log.DebugFormat("Wowza - Response - Status: {0}", response.StatusCode);
            log.DebugFormat("Wowza - Response - Message: {0}", response.ReasonPhrase);
            log.DebugFormat("Wowza - Response - Request Message: {0}", response.RequestMessage);
            log.DebugFormat("Wowza - Response - IsSuccess: {0}", response.IsSuccessStatusCode);
            log.Info("Wowza API - Update Event - End");
            return response.IsSuccessStatusCode;
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

        #endregion

    }

    class StreamFile
    {
        public string name { get; set; }
        public string serverName { get; set; }
        public string uri { get; set; }
    }

    class StartupStreamFile
    {
        public string mediaCasterType { get; set; }
        public string instance { get; set; }
        public string appName { get; set; }
        public string serverName { get; set; }
        public string streamName { get; set; }
    }

    class RecordingData
    {
        public string instanceName = "_definst_"; // _6aa78cb93bf19affb2d70f728a50e7f7b355e954_";
        public string fileVersionDelegateName = "";
        public string serverName = "";
        public string recorderName { get; set; }
        public int currentSize = 0;
        public string segmentSchedule = "";
        public bool startOnKeyFrame = true;
        public string outputPath = "/usr/local/WowzaStreamingEngine/content/recording";
        public string currentFile = "";
        public string[] saveFieldList = new string[] { };
        public bool recordData = false;
        public string applicationName = "";
        public bool moveFirstVideoFrameToZero = false;
        public string recorderErrorString = "";
        public int segmentSize = 0;
        public bool defaultRecorder = false;
        public bool splitOnTcDiscontinuity = false;
        public string version = "";
        public string baseFile = "";
        public int segmentDuration = 0;
        public string recordingStartTime = "";
        public string fileTemplate = "";
        public int backBufferTime = 0;
        public string segmentationType = "";
        public int currentDuration = 0;
        public string fileFormat = "";
        public string recorderState = "";
        public string option = "";
    }
}
