using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class GenericSchedule
    {
        /// <summary>The TodaySchedule property represents the events scheduled for current date.</summary>
        /// <value>The TodaySchedule property gets/sets the list of scheduled events.</value>
        public IEnumerable<Schedule> TodaySchedule { get; set; }

        /// <summary>The UpcomingSchedule property represents the today's scheduled events which will be started shortly.</summary>
        /// <value>The UpcomingSchedule property gets/sets the list of today's coming soon events.</value>
        public IEnumerable<Schedule> UpcomingSchedule { get; set; }

        /// <summary>The CurrentSchedule property represents the scheduled events currently in progress.</summary>
        /// <value>The CurrentSchedule property gets/sets the list of inprogress events schedule.</value>
        public IEnumerable<Schedule> CurrentSchedule { get; set; }
    }
}
