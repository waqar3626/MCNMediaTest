using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MCNMedia_Dev._Helper
{
    public class Roku
    {
        public string RokuRSS()
        {

            CameraDataAccessLayer camDataAccess = new CameraDataAccessLayer();
            List<Camera> cameraList = camDataAccess.GetAllAdminCameras().ToList<Camera>();

            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.Append("<rss xmlns:media=\"http://search.yahoo.com/mrss/\" version=\"2.0\">");
            sb.Append("<channel>");
            sb.Append("<title>Developer Sample Library - No Genre</title>");
            sb.Append("<link />");
            sb.Append("<description />");
            sb.Append("<language>en-us</language>");
            sb.Append("<pubDate>Sat, 09 Jul 2016 00:30:47 GMT</pubDate>");
            foreach (var cam in cameraList)
            {
                if (cam.IsCameraLive && cam.IsCameraStreaming)
                {
                    sb.Append("<item>");
                    sb.Append("<title>" + cam.ChurchName.Replace("&", "&#38;") + "</title>");
                    sb.Append("<link>" + cam.LiveStreamUrl + "</link>");
                    sb.Append("<description>With the Twitch channel, you can watch the most popular broadcasts of the day, browse live broadcasts by the games you love and follow your favorite Twitch broadcasters.</description>");
                    sb.Append("<pubDate>Thu, 11 Jun 2015 16:51:07 GMT</pubDate>");
                    sb.Append("<guid isPermaLink=\"false\">" + cam.ChurchUniqueIdentifier + "</guid>");
                    sb.Append("<media:content  bitrate=\"1328.0\"  fileSize=\"8731706\" framerate=\"23.976\" height=\"720\" type=\"video/mp4\" width=\"1280\" duration=\"74.74\" isDefault=\"true\" url=\"" + cam.LiveStreamUrl + "\">");
                    sb.Append("<media:description>With the Twitch channel, you can watch the most popular broadcasts of the day, browse live broadcasts by the games you love and follow your favorite Twitch broadcasters.</media:description>");
                    sb.Append("<media:keywords>episode 21, roku recommends, twitch</media:keywords>");
                    sb.Append("<media:thumbnail url=\"http://mcnuat.us-east-1.elasticbeanstalk.com/Images/missing-image2.jpg\" />");
                    sb.Append("<media:title>Live Gaming</media:title>");
                    sb.Append("</media:content>");
                    sb.Append("</item>");
                }
            }
            sb.Append("</channel>");
            sb.Append("</rss>");

            return sb.ToString();
        }

        public string GetCounty()
        {
            PlaceAccessLayer _placeAccessLayer = new PlaceAccessLayer();
            IEnumerable<Place> countryList = _placeAccessLayer.GetCountries();
            IEnumerable<Place> countyList;

            StringBuilder sb = new StringBuilder();
            sb.Append("<categories>"); //http://rokudev.roku.com/rokudev/examples/videoplayer/images/missing.png
            sb.Append("<banner_ad sd_img=\"http://mcnuat.us-east-1.elasticbeanstalk.com/Images/missing-image.jpg\" hd_img=\"http://mcnuat.us-east-1.elasticbeanstalk.com/Images/missing-image.jpg\"/>");
            foreach (var item in countryList)
            { //http://philipmcn.co.uk/roku/xml/live.png
                sb.Append("<category title=\"" + item.PlaceName + "\" description=\"\" sd_img=\"http://mcnuat.us-east-1.elasticbeanstalk.com/Images/missing-image.jpg\" hd_img=\"http://mcnuat.us-east-1.elasticbeanstalk.com/Images/missing-image.jpg\">");

                countyList = _placeAccessLayer.GetCounties(item.PlaceId);
                foreach (var county in countyList)
                { //http://justdaz.com/roku/{county.PlaceSlug}.xml
                    sb.Append("<categoryLeaf title=\"" + county.PlaceName + "\" description=\"\" feed=\"http://mcnuat.us-east-1.elasticbeanstalk.com/roku/" + county.PlaceSlug + ".xml\"/>");
                }
                sb.Append("</category>");
            }
            sb.Append("</categories>");
            return sb.ToString();
        }

        public string GetCameraByCounty(string countySlug)
        {
            CameraDataAccessLayer camDataAccess = new CameraDataAccessLayer();
            ChurchDataAccessLayer churchDataAccess = new ChurchDataAccessLayer();
            Church church;
            List<Camera> cameraList = camDataAccess.GetCameraByCounty(countySlug);

            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.Append("<feed>");

            foreach (var cam in cameraList)
            {
                if (cam.IsCameraLive && cam.IsCameraStreaming)
                {
                    church = churchDataAccess.GetChurchData(cam.ChurchId);
                    sb.Append("<item sdImg=\"" + church.ImageURl + "\" hdImg=\"" + church.ImageURl + "\">"); // Church image
                    sb.Append("<title>" + cam.ChurchName.Replace("&", "&#38;") + ", " + church.Town + "</title>");
                    sb.Append("<contentId>10001</contentId>");
                    sb.Append("<contentType>" + church.Address + "</contentType>");
                    sb.Append("<contentQuality>SD</contentQuality>");
                    sb.Append("<streamFormat>hls</streamFormat>");
                    sb.Append("<media>");
                    sb.Append("<streamQuality>SD</streamQuality>");
                    sb.Append("<streamBitrate>365</streamBitrate>");
                    sb.Append("<streamUrl>" + cam.LiveStreamUrl + "</streamUrl>");
                    sb.Append("</media>");
                    sb.Append("<synopsis>test2</synopsis>");
                    sb.Append("<genres>test3</genres>");
                    sb.Append("</item>");
                }
            }
            sb.Append("</feed>");

            return sb.ToString();
        }

        public string GetCamera()
        {
            CameraDataAccessLayer camDataAccess = new CameraDataAccessLayer();
            List<Camera> cameraList = camDataAccess.GetAllAdminCameras().ToList<Camera>();

            var shortFormVids = new List<Object>();

            foreach (var cam in cameraList)
            {
                //if (cam.IsCameraLive && cam.IsCameraStreaming)
                //{
                ChurchDataAccessLayer churchDataAccessLayer = new ChurchDataAccessLayer();
                //list<Church> church = new Church(); 
                Church church = churchDataAccessLayer.GetChurchData(cam.ChurchId);
                string url;
                if (string.IsNullOrEmpty(church.ImageURl))
                {
                    url = "https://mcnmedia.s3-eu-west-1.amazonaws.com/live/Roku/Uploads/ProfileImages/missing-image.jpg"; 
                }
                else
                {
                    url = church.ImageURl.Replace("live", "live/Roku");
                }
                
                var ob = new
                {

                    id = $"Cam_{cam.CameraId}",
                    title = cam.ChurchName.Replace("&", "&#38;"),
                    shortDescription = "Faith based Community.",
                    //thumbnail = "http://mcnuat.us-east-1.elasticbeanstalk.com/Images/missing-image2.jpg",
                    thumbnail = url,
                    genres = new List<string>() { "faith" }.ToArray(),
                    tags = new List<string>() { "uk" }.ToArray(),
                    releaseDate = "2021-01-15",
                    content = new
                    {
                        dateAdded = "2021-01-15T04:14:54.431Z",
                        captions = new List<string>() { "" }.ToArray(),
                        duration = 150000,
                        videos = new List<Object>()
                            {
                                new
                                {
                                    url=cam.LiveStreamUrl,
                                    quality="HD",
                                    videoType="hls"
                                }
                            }
                    }
                };
                shortFormVids.Add(ob);
                //}
            }


            var playlists = new List<Object>();
            PlaceAccessLayer _placeAccessLayer = new PlaceAccessLayer();
            IEnumerable<Place> countyList = _placeAccessLayer.GetCounties(countryId: -1);
            List<string> camIds = new List<string>();
            foreach (var county in countyList)
            {
                cameraList = new List<Camera>();
                cameraList = camDataAccess.GetCameraByCounty(county.PlaceSlug);
                camIds = new List<string>();
                foreach (var cam in cameraList)
                {
                    if (cam.IsCameraLive && cam.IsCameraStreaming)
                    {
                        camIds.Add($"Cam_{cam.CameraId}");
                    }
                }
                if (camIds.Count > 0)
                {
                    var playlist = new
                    {
                        name = county.PlaceName,
                        itemIds = camIds.ToArray()
                    };
                    playlists.Add(playlist);
                }
            }

            List<Object> list = playlists;

            var categories = new List<Object>();
            foreach (var playlist in playlists)
            {
                var cat = new
                {
                    name = playlist.GetType().GetProperty("name").GetValue(playlist, null),
                    playlistName = playlist.GetType().GetProperty("name").GetValue(playlist, null),
                    order = "manual"
                };
                categories.Add(cat);
            };





            var my_jsondata = new
            {
                providerName = "MCNMedia.tv",
                language = "en-US",
                lastUpdated = "2016-10-06T18:12:32.125Z",
                shortFormVideos = shortFormVids.ToArray(),
                playlists = playlists.ToArray(),
                categories = categories.ToArray()
            };

            //Tranform it to Json object
            string json_data = JsonConvert.SerializeObject(my_jsondata);
            return json_data;

        }
    }

}
