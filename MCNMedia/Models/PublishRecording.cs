using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class PublishRecording
    {
        public string name { get; set; }
        public string token { get; set; }
        public string churches_id { get; set; }
        public string camera_id { get; set; }
        public string recording_url { get; set; }
        public string event_id { get; set; }
    }
}
