﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class AnalyticsModel
    {
        public int AnalyticsID { get; set; }

        public int ChurchId { get; set; }
        public string IPAddress { get; set; }
        public DateTime DateAndTime { get; set; }
        public string Country { get; set; }

        public int CountryCount { get; set; }
    }
}
