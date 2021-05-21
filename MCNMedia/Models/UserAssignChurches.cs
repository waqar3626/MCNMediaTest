using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class UserAssignChurches
    {
        public int UserAssignChurchId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ChurchId { get; set; }
        public string ChurchName { get; set; }
        public string Town { get; set; }
        public int CreatedBy  { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UpdateBy { get; set; }
        public DateTime UpdateAt { get; set; }

        public bool Assigned { get; set; }

    }
}
