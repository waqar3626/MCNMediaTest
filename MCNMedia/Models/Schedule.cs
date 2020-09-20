using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class Schedule
    {
        public int ScheduleId { get; set; }
        public int ChurchId { get; set; }
        public string ChurchName { get; set; }
        [Required]
        public String EventName { get; set; }
        //[DisplayFormat(DataFormatString = "{0:yyyy/MMM/dd}")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM/dd/yyyy}")]
        public DateTime EventDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:ddd}")]
        public String EventDay { get; set; }
        [EnumDataType(typeof(DayOfWeek))]
        public EventDay Days { get; set; }
      //  [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm}")]
       [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm tt}")]
        public DateTime EventTime { get; set; }

        public Boolean IsRepeated { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        //[DataType(DataType.Date)]
        //[Display(Name = "Required delivery date")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        //public Nullable<DateTime> DelDt { get; set; }

    }
    public enum EventDay
    {

        [Description("Monday")]
        Monday,
        [Description("Tuesday")]
        Tuesday,
        [Description("Wednesday")]
        Wednesday,
        [Description("Thursday")]
        Thursday,
        [Description("Friday")]
        Friday,
        [Description("Saturday")]
        Saturday,
        [Description("Sunday")]
        Sunday
        
    }
}
