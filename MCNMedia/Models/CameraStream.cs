using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class CameraStream
    {
        public string serverName { get; set; }
        public string applicationInstance { get; set; }
        public string name { get; set; }
        public string sourceIp { get; set; }
        public bool isRecordingSet { get; set; }
        public bool isStreamManagerStream { get; set; }
        public bool isPublishedToVOD { get; set; }
        public bool isConnected { get; set; }
        public bool isPTZEnabled { get; set; }
        public int ptzPollingInterval { get; set; }
        public int ptzPollingIntervalMinimum { get; set; }
    }
}
