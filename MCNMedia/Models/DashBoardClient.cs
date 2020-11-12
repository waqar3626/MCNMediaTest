using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class DashBoardClient
    {
        public int ChurchId { get; set; }
        public int CameraId { get; set; }
        public String ScheduleEventName { get; set; }
        public String ScheduleEventTime { get; set; }
        public DateTime ScheduleEventDate { get; set; }
        public string ScheduleStatus { get; set; }
        public int IsRepeated { get; set; }
        public int RecordingDuration { get; set; }
    }
}
