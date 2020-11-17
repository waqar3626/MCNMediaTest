﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class Profile
    {
        public Church Churches { get; set; }
        public Camera Cameras { get; set; }
        //public MediaChurch Media { get; set; }
        //public Schedule Schedules { get; set; }
        //public Recording Recordings { get; set; }
        public Announcement Announcement { get; set; }

        // Lists
        public IEnumerable<Camera> CameraList { get; set; }
        public IEnumerable<MediaChurch> VideoList { get; set; }
        public IEnumerable<MediaChurch> SlideshowList { get; set; }
        public IEnumerable<MediaChurch> PictureList { get; set; }

        public IEnumerable<Schedule> ScheduleList { get; set; }
        public IEnumerable<Recording> RecordingList { get; set; }

    }
}