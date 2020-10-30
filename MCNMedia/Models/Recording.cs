﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class Recording
    {
        [Key]
        public int RecordingId { get; set; }
        [Required(ErrorMessage = "Recording Title is Required")]
        public String RecordingTitle { get; set; }
        [Required(ErrorMessage = "Recording URL is Required")]
        public string RecordingURl { get; set; }
        [Required(ErrorMessage = "UpdateBy is Required")]
        public int UpdatedBy { get; set; }
        public int CreatedBy { get; set; }

        public int ChurchId { get; set; }
        [Required(ErrorMessage = "Church Name is Required")]
        public string ChurchName { get; set; }

        [Required(ErrorMessage = "Recording Date is Required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM/dd/yyyy}")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Recording Time is Required")]
        [DataType(DataType.Time)]
        public DateTime Time { get; set; }
       
    }
  
}
