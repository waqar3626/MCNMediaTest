using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MCNMedia_Dev._Helper
{
    public class FacebookHelperNew
    {


        /// <summary>
        /// Get current status of camera i.e Online or Offline. Also checking the recording status i.e Recording is ON or OFF
        /// </summary>
        /// <param name="churchId">The id of <see cref="Church" />.</param>
        /// <param name="cameraId">The id of <see cref="Camera"/></param>
        /// <returns>Return A <see cref="CameraStream"/></returns>


        public bool FacebookLiveStream(int cameraId,string StreamKey,int churchId)
        {
            try
            {
                string UniqueIdentifier = RetrieveChurchUniqueIdentifier(churchId);
                FacebookParameter facebookParameter = new FacebookParameter();
                facebookParameter.camera_id = UniqueIdentifier+"_"+cameraId.ToString();
                facebookParameter.client_name = "definst";
                facebookParameter.type = "rtmp";
                facebookParameter.stream_url = "rtmp://live-api-s.facebook.com:80/rtmp/";
                facebookParameter.secure_stream_url = "rtmps://live-api-s.facebook.com:443/rtmp/";
                facebookParameter.stream_key = StreamKey;
                //string uniqueIdentifier = RetrieveChurchUniqueIdentifier(churchId);
                //string streamName = $"{uniqueIdentifier}_{cameraId}";
                return PostAsync("http://54.217.38.80:8182/api/v1/start-fb-live", facebookParameter);
                //HttpClient client = CreateHttpClientRequest($"http://52.51.59.126:8182/api/v1/start-fb-live");
                //// List data response.
                //HttpResponseMessage response = client.GetAsync("").Result;
                //if (response.IsSuccessStatusCode)
                //{
                //    var responseBody = response.Content.ReadAsStringAsync().Result;
                //    try
                //    {
                //        cameraStream = JsonConvert.DeserializeObject<CameraStream>(responseBody);
                //    }
                //    catch (JsonReaderException ex)
                //    {
                //    }
                //}
                //return cameraStream;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool StopFacebookLiveStreaming(int cameraId,  int churchId)
        {
            try
            {
                string UniqueIdentifier = RetrieveChurchUniqueIdentifier(churchId);
                FacebookParameter facebookParameter = new FacebookParameter();
                facebookParameter.camera_id = UniqueIdentifier + "_" + cameraId.ToString();
                facebookParameter.client_name = "definst";
                facebookParameter.type = "rtmp";
                facebookParameter.stream_url = "rtmp://live-api-s.facebook.com:80/rtmp/";
                facebookParameter.secure_stream_url = "rtmps://live-api-s.facebook.com:443/rtmp/";
         
                return PostAsync("http://54.217.38.80:8182/api/v1/stop-fb-live", facebookParameter);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool PostAsync(string requestUri, object data)
        {
            
            HttpClient client = CreateHttpClientRequest(requestUri);

            var jsonData = JsonConvert.SerializeObject(data);
         
            var encodeData = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // List data response.
            HttpResponseMessage response = client.PostAsync(requestUri, encodeData).Result;
          
            return response.IsSuccessStatusCode;
        }

        private HttpClient CreateHttpClientRequest(string requestUri)
        {
            Uri uri = new Uri(requestUri);
            var HttpHandler = new HttpClientHandler();
            //HttpHandler.Credentials = GetCredentials(uri);

            HttpClient client = new HttpClient(HttpHandler);
            client.BaseAddress = new Uri(requestUri);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        private string RetrieveChurchUniqueIdentifier(int churchId)
        {
            Church church = new Church();
            ChurchDataAccessLayer churchDataAccessLayer = new ChurchDataAccessLayer();
            church = churchDataAccessLayer.GetChurchData(churchId);
            return church.UniqueIdentifier;
        }

        public class FacebookParameter
        {
            public string camera_id { get; set; }
            public string client_name { get; set; }

            public string type { get; set; }

            public string stream_url { get; set; }
            public string secure_stream_url { get; set; }

            public string stream_key { get; set; }
           

        }


    }
}
