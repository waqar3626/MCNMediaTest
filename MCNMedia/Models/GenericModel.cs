using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class GenericModel
    {
        public Church Churches { get; set; }
        public Camera Cameras { get; set; }
        public MediaChurch Media { get; set; }
        public Announcement Announcement { get; set; }
        public IEnumerable<MediaChurch> ListMedia { get; set; }
        public IEnumerable<Camera> LCameras { get; set; }

        public Notice Notices { get; set; }
        public IEnumerable<Notice> ListNotice { get; set; }
        public SetUp ChurchSetUp { get; set; }

        public string ResultMessage { get; set; } = "";
    }
}
