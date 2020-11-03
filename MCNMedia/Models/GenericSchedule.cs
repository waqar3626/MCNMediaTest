using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class GenericSchedule
    {
        public IEnumerable<Schedule> List1 { get; set; }
        public IEnumerable<Schedule> List2 { get; set; }

        public IEnumerable<Schedule> List3 { get; set; }
    }
}
