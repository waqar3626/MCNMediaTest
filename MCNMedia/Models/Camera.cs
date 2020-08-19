using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class Camera
    {
        public int CameraId { get; set; }
        public string TabName { get; set; }
        public string CameraRtsp { get; set; }
        public string HttpPort { get; set; }
        public string RtspPort { get; set; }
    }
}
