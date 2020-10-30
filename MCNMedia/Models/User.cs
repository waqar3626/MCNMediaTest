using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MCNMedia_Dev.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public string FirstName { get; set; } 
        public string LastName { get; set; }


        public string EmailAddress { get; set; }

        public string LoginPassword { get; set; }

        public int UpdatedBy { get; set; }

        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public int ChurchId { get; set; }

        public string ChurchName { get; set; }

        public string UserName { get; set; }

        public int ID { get; internal set; }
    }
}
