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
        [Required(ErrorMessage ="First Name is Required")]
        public string FirstName { get; set; } 
        [Required (ErrorMessage ="Last  Name is Required")]
        public string LastName { get; set; }
        [Required (ErrorMessage ="Email Address is Required")]
        public string EmailAddress { get; set; }
        [Required (ErrorMessage ="Login Password is Required")]
        public string LoginPassword { get; set; }
        [Required (ErrorMessage ="Update By is Required")]
        public int UpdatedBy { get; set; }
        [Required(ErrorMessage ="Select Your Role Id")]
        public int RoleId { get; set; }

    }
}
