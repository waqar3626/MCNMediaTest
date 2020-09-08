using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class UserAssignChurches
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public int ChurchId { get; set; }
        public string ChurchName { get; set; }
        public string CreatedBy  { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
