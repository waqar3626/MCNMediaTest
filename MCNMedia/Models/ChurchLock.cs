using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class ChurchLock
    {
        public string Password { get; set; }
        //for the locked churches
        public string Slug { get; set; }
        public int churchPass { get; set; }

        public int ChurchId { get; set; }

        public string ChurchName { get; set; }
    }
}
