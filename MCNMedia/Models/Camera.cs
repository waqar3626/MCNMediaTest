using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class Camera
    {
        public int CameraId { get; set; }
        public string CameraName { get; set; }
        public string CameraUrl { get; set; }
        public string RtspPort { get; set; }
        public string HttpPort { get; set; }
        public string CameraSlug { get; set; }
        public string CameraType { get; set; }
        public int ChurchId { get; set; }
        public string ChurchName { get; set; }
        public int ServerId { get; set; }
        public string ServerName { get; set; }
        public string ServerIP { get; set; }

        public string App { get; set; }
        public string CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public string UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }

        public string StreamingProtocol { get; set; }
        public string LiveStreamUrl { get; internal set; }
    }


    public class Server {

        public int ServerId { get; set; }
        public string ServerName { get; set; }
        public string ServerIP { get; set; }



    }
} 
