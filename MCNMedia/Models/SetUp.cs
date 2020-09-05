using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class SetUp
    {
        public int ChurchSetUpId { get; set; }
        public String SelectServer { get; set; }
        public String InternetProvider { get; set; }
        public String RouterModel { get; set; }
        public String RouterUserName { get; set; }
        public String RouterPassword { get; set; }
        public Boolean Aerials { get; set; }
        public Boolean Switch { get; set; }
        public Boolean HomePlugs { get; set; }
        public Boolean DirectWire { get; set; }
        public int ChurchId { get; set; }
        public string ChurchName { get; set; }
        public DateTime CreatedAt { get; set; }
        public String CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }

    }
}
