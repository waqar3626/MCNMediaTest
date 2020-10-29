using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class Dashboard
    {
        public int ChurchId { get; set; }
        public int ChurchCount { get; set; }
        public string ChurchName { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int CountryCount { get; set; }
        public int CountyId { get; set; }
        public string CountyName { get; set; }
        public int CountyCount { get; set; }

        public int CameraId { get; set; }
        public string CameraName { get; set; }
        public int CameraCount { get; set; }
        public int CathedralsCount { get; set; }
        public int FuneralsHomeCount { get; set; }
        public int CanadaCount { get; set; }
        public int USACount { get; set; }
        public int EnglandCount { get; set; }
        public int RepublicOfIrelandCount { get; set; }
        public int NorthrenIrelandCount { get; set; }
        public int WalesCount { get; set; }
        public int ISEOfManCount { get; set; }
        public int ScotlandCount { get; set; }
        public int UserId { get; set; }
        public int UserCount { get; set; }
        public int AdminCount { get; set; }
        public int ClientCount { get; set; }
        public int ScheduleId { get; set; }
        public int TodayScheduleCount { get; set; }
        public int ScheduleCount { get; set; }

        public int RecordingId { get; set; }
        public String RecordingTitle { get; set; }
        public int RecordingCount{ get; set; }
        public int TodayRecording { get; set; }


    }
}
