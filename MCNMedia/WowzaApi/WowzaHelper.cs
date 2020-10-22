using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
        string serverIP = "52.211.229.30";
        string port = "8087";
        string apiVersion = "v2";
        string servers = "_defaultServer_";
        string vhosts = "_defaultVHost_";
        string application = "test";

        public WowzaHelper()
        {

        }

        private string GetBasicUri(int cameraId)
        {
            serverIP = RetrieveCameraServerIP(cameraId);
            string uri = $"http://{serverIP}:{port}/{apiVersion}/servers/{servers}/vhosts/{vhosts}/applications/{application}";
            return uri;
        }

        private string GetStreamFileData(string churchIdentifier, int cameraId, string rtspUrl)
        {
            string streamName = $"{churchIdentifier}_{cameraId}";
            string data = "{\"name\": " + streamName + ", \"serverName\": _defaultServer_, \"uri\": " + rtspUrl + "}";
            abc aa = new abc();
            aa.Name = streamName;
            aa.ServerName = "_defaultServer_";
            aa.Uri = rtspUrl;
            data = JsonConvert.SerializeObject(aa);
            return data;
        }

        private string GetStreamRecorderData(string churchIdentifier, int cameraId)
        {
            string instanceName = $"_{churchIdentifier}_";
            string recorderName = $"{churchIdentifier}_{cameraId}.stream";
            string data = "{\"instanceName\":" + instanceName + ",\"fileVersionDelegateName\": \"\", \"serverName\": \"\", \"recorderName\":" + recorderName + ", \"currentSize\": 0, \"segmentSchedule\": \"\", \"startOnKeyFrame\": true, \"outputPath\": \"\", \"currentFile\": \"\", \"saveFieldList\": [ ], \"recordData\": false, \"applicationName\": \"\", \"moveFirstVideoFrameToZero\": false, \"recorderErrorString\": \"\", \"segmentSize\": 0, \"defaultRecorder\": false, \"splitOnTcDiscontinuity\": false, \"version\": \"\", \"baseFile\": \"\", \"segmentDuration\": 0, \"recordingStartTime\": \"\", \"fileTemplate\": \"\", \"backBufferTime\": 0, \"segmentationType\": \"\", \"currentDuration\": 0, \"fileFormat\": \"\", \"recorderState\": \"\", \"option\": \"\" }";
            return data;
        }

        private bool GetStream(string churchIdentifier, int cameraId)
        {
            string streamName = $"{churchIdentifier}_{cameraId}";
            return GetAsync($"{GetBasicUri(cameraId)}/streamfiles/{streamName}", "");
        }

        private bool Stream_Create(string churchIdentifier, int cameraId, string rtspUrl)
        {
            bool connectStream = false;
            string streamFileName = $"{churchIdentifier}_{cameraId}";
            string createStreamData = GetStreamFileData(churchIdentifier, cameraId, rtspUrl);
            abc aa = new abc();
            aa.Name = streamFileName;
            aa.ServerName = "_defaultServer_";
            aa.Uri = rtspUrl;
            var json = JsonConvert.SerializeObject(aa);
            bool createStreamFile = PostAsync($"{GetBasicUri(cameraId)}/streamfiles", aa);
            if (createStreamFile)
            {
                connectStream = PutAsync($"{GetBasicUri(cameraId)}/streamfiles/{streamFileName}/actions/connect?connectAppName={application}&appInstance=_definst_&mediaCasterType=rtp", "");
            }
            return connectStream;
        }

        private bool GetAsync(string requestUri, string data = "")
        {
            HttpClient client = CreateHttpClientRequest(requestUri);

            // List data response.
            HttpResponseMessage response = client.GetAsync("").Result;
            return response.IsSuccessStatusCode;


        }

        private bool PostAsync(string requestUri, object data)
        {
            HttpClient client = CreateHttpClientRequest(requestUri);

            var json = JsonConvert.SerializeObject(data);
            var data2 = new StringContent(json, Encoding.UTF8, "application/json");

            // List data response.
            HttpResponseMessage response = client.PostAsync(requestUri, data2).Result;
            return response.IsSuccessStatusCode;
        }

        private bool PostAsync(string requestUri, string data)
        {
            HttpClient client = CreateHttpClientRequest(requestUri);
            var data2 = new StringContent(data, Encoding.UTF8, "application/json");

            // List data response.
            HttpResponseMessage response = client.PostAsync(requestUri, data2).Result;
            return response.IsSuccessStatusCode;
        }

        private bool PutAsync(string requestUri, object data)
        {
            HttpClient client = CreateHttpClientRequest(requestUri);

            var json = JsonConvert.SerializeObject(data);
            var data2 = new StringContent(json, Encoding.UTF8, "application/json");

            // List data response.
            HttpResponseMessage response = client.PutAsync(requestUri, data2).Result;
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

        private string RequestAPI(string requestUri, string requestType, string data = "")
        {
            try
            {
                Uri uri = new Uri(requestUri);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest.Credentials = GetCredentials(uri);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Accept = "*/*";
                httpWebRequest.Method = requestType;
                httpWebRequest.ContentLength = data.Length;

                //byte[] bytes = Encoding.UTF8.GetBytes(data);
                //using (Stream stream = httpWebRequest.GetRequestStream())
                //{
                //    stream.Write(bytes, 0, bytes.Length);
                //    stream.Close();
                //}

                //using (HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                //{
                //    using (Stream stream = httpResponse.GetResponseStream())
                //    {
                //        string json = (new StreamReader(stream)).ReadToEnd();
                //        return json;
                //    }
                //}

                WebResponse webResponse = httpWebRequest.GetResponse();

                //Stream webStream = webResponse.GetResponseStream();
                //StreamReader responseReader = new StreamReader(webStream);
                //string response = responseReader.ReadToEnd();

                //var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    string answer = streamReader.ReadToEnd();
                    // string answer = JsonConvert.DeserializeObject<string>(streamReader.ReadToEnd());
                    return answer;
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string RequestCamera(int churchId, int cameraId, string rtspUrl)
        {
            string uniqueIdentifier = RetrieveChurchUniqueIdentifier(churchId);
            if (GetStream(uniqueIdentifier, cameraId))
            {
                // Stream Exists
                bool result = Stream_Create(uniqueIdentifier, cameraId, rtspUrl);
            }
            else
            {
                bool result = Stream_Create(uniqueIdentifier, cameraId, rtspUrl);
                // Create Stream
            }
            return "";
        }

        public string StartRecording(int churchId, int cameraId)
        {
            string uniqueIdentifier = RetrieveChurchUniqueIdentifier(churchId);
            RecordingData recordingData = new RecordingData();
            recordingData.recorderName = uniqueIdentifier + "_" + cameraId.ToString() + ".stream";

            string startRecordingData = GetStreamRecorderData(uniqueIdentifier, cameraId);
            if (GetStream(uniqueIdentifier, cameraId))
            {
                bool startRec = PostAsync($"{GetBasicUri(cameraId)}/instances/_definst_/streamrecorders", recordingData);
                if (startRec)
                {
                    return startRec.ToString();
                }
                return "";
            }
            else
            {
                return "";
            }
        }

        public string StopRecording(int churchId, int cameraId)
        {
            string uniqueIdentifier = RetrieveChurchUniqueIdentifier(churchId);

            if (GetStream(uniqueIdentifier, cameraId))
            {
                bool stopRec = PutAsync($"{GetBasicUri(cameraId)}/instances/_definst_/streamrecorders/{uniqueIdentifier}_{cameraId}.stream/actions/stopRecording", "");
                if (stopRec)
                {
                    return stopRec.ToString();
                }
                return "";
            }
            else
            {
                return "";
            }
        }

        private  string RetrieveChurchUniqueIdentifier(int churchId)
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
            return cam.ServerIP;
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

    class abc
    {
        public string Name { get; set; }
        public string ServerName { get; set; }
        public string Uri { get; set; }
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
        public string outputPath = "";
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
