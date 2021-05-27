using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class FbStreaming
    {
        public int StreamId { get; set; }
        public string StreamKey { get; set; }
        public int ChurchId { get; set; }
        public int CameraId { get; set; }
        public int Status { get; set; }
        public FbStreaming()
        {
            StreamId = 0;
            StreamKey = string.Empty;
            ChurchId = 0;
            CameraId = 0;
            Status = 0;
        }
    }

   
}
