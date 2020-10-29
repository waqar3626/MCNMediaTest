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

        public User Users { get; set; }
        public MediaChurch Media { get; set; }

        public Schedule Schedules { get; set; }

        public Recording Recordings { get; set; }

        public Announcement Announcement { get; set; }
        public IEnumerable<Announcement> LAnnouncement { get; set; }
        public Dashboard Dashboards { get; set; }
        public IEnumerable<Dashboard> LDashboards { get; set; }
        public IEnumerable<MediaChurch> ListMedia { get; set; }
        public IEnumerable<Camera> LCameras { get; set; }

        public IEnumerable<User> LUsers { get; set; }

        public IEnumerable<Schedule> LSchedules { get; set; }

        public IEnumerable<Recording> LRecordings { get; set; }
        public PreviewChurch PreviewChurches { get; set; }
        public IEnumerable<PreviewChurch> LPreviewChurches { get; set; }

        public Notice Notices { get; set; }
        public IEnumerable<Notice> ListNotice { get; set; }
        public SetUp ChurchSetUp { get; set; }

        public string ResultMessage { get; set; } = "";

       
    }
}
