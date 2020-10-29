using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class PreviewChurch
    {
   
        public int ChurchId { get; set; }

       
        public string ChurchName { get; set; }

  
        public int ClientTypeId { get; set; }

        private string _UniqueChurchId = string.Empty;
        public string UniqueChurchId
        {
            get { return _UniqueChurchId; }
            set => _UniqueChurchId = Guid.NewGuid().ToString();

        }

      
        public string Address { get; set; }

        
        public string Town { get; set; }

      
        public int CountyId { get; set; }

      
        public string Website { get; set; }

       
        public string EmailAddress { get; set; }

       
        public string Phone { get; set; }

        public int UpdateBy { get; set; }
        public string ImageURl { get; set; }
        public string Blurb { get; set; }

        private string _Slug = string.Empty;
        public string Slug
        {
            get { return _Slug; }
            set => _Slug = Guid.NewGuid().ToString();
        }

        public string Notice { get; set; }

        public int Featured { get; set; }

        private string _UniqueIdentifier = string.Empty;
        public string UniqueIdentifier
        {
            get { return _UniqueIdentifier; }
            set => _UniqueIdentifier = Guid.NewGuid().ToString();
        }
        public Boolean RepeatRecordings { get; set; }

        public int Switch { get; set; }

        public Boolean ShowOnWebsite { get; set; }

        public int DisplayOrder { get; set; }

        public string ClientTypeTitle { get; set; }

        public string CountyName { get; set; }

        public class ClientType
        {
            public int ClientTypeId { get; set; }
            public string ClientTypeTitle { get; set; }
        }

        public class Counties
        {
            public int CountyId { get; set; }
            public string CountyName { get; set; }
        }
        public int CameraId { get; set; }
        public string CameraName { get; set; }
        public string CameraUrl { get; set; }
        public string RtspPort { get; set; }
        public string HttpPort { get; set; }
        public string CameraSlug { get; set; }
        public string CameraType { get; set; }
         public int ServerId { get; set; }
        public string App { get; set; }
        public string CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public string UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }

        public string StreamingProtocol { get; set; }

        public int ScheduleId { get; set; }
      
       
        public String EventName { get; set; }
        //[DisplayFormat(DataFormatString = "{0:yyyy/MMM/dd}")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM/dd/yyyy}")]
        public DateTime EventDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:ddd}")]
        public String EventDay { get; set; }
        [EnumDataType(typeof(DayOfWeek))]
        public EventDay Days { get; set; }
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm}")]
        [DataType(DataType.Time)]
        public DateTime EventTime { get; set; }
      
        public int RecordingId { get; set; }
       
        public String RecordingTitle { get; set; }
      
        public string RecordingURl { get; set; }
       
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM/dd/yyyy}")]
        public DateTime Date { get; set; }

       
        [DataType(DataType.Time)]
        public DateTime Time { get; set; }

        public int ChrId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string FeedBack { get; set; }




    }
}
