using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class WebsiteMedia
    {
        public int MediaId { get; set; }
        public int ChurchId { get; set; }
        public string ChurchName { get; set; }
        public string UniqueIdentifier { get; set; }
        public string TabName { get; set; }
        public string MediaURL { get; set; }
        public bool IsCameraLive { get; set; }
        public bool IsCameraStreaming { get; internal set; }
        public string MediaType { get; set; }
        public string ServerUrl { get; set; }
        public string CreatedAt { get; set; }
    }
}
