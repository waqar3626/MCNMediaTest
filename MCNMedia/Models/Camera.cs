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
        public _Helper.CameraType CameraType { get; set; }
        public bool IsCameraLive { get; set; }
        public bool IsCameraStreaming { get; internal set; }
        public string StreamingProtocol { get; set; }
        public string LiveStreamUrl { get; internal set; }
        public Boolean CameraScheduleIsRepeated { get; set; }
        public string App { get; set; }
        public string CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public string UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }

        public int ChurchId { get; set; }
        public string ChurchName { get; set; }
        public string ChurchUniqueIdentifier { get; internal set; }
    
        public int ServerId { get; set; }
        public string ServerName { get; set; }
        public string ServerIP { get; set; }
        public string ServerPort { get; set; }
        public string ServerUser { get; set; }
        public string ServerPassword { get; set; }
    }
} 
