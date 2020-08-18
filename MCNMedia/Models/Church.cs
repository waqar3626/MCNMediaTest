using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class Church
    {
        [Key]
        public int ChurchId { get; set; }

        [Required(ErrorMessage ="Church Name is Required")]
        public string ChurchName { get; set; }

        [Required(ErrorMessage = "Select the Client Type Id")]
        public int ClientTypeId { get; set; }

        [Required(ErrorMessage = "Select the Unique church Id")]
        public string UniqueChurchId { get; set; }

        [Required(ErrorMessage = "Address is Required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Town Name is Required")]
        public string Town { get; set; }

        [Required(ErrorMessage = "Select the County Id")]
        public int CountyId { get; set; }

        [Required(ErrorMessage = "Website Name is Required")]
        public string Website { get; set; }

        [Required(ErrorMessage = "Email Address is Required")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Phone Number is Required")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Image URL is Required")]
        public string ImageURl { get; set; }

        [Required(ErrorMessage = "Blurb is Required")]
        public string Blurb { get; set; }

        [Required(ErrorMessage = "Slug is Required")]
        public string Slug { get; set; }

        [Required(ErrorMessage = "Notice is Required")]
        public string Notice { get; set;}

        [Required(ErrorMessage = "Featured is Required")]
        public int Featured { get; set; }

        [Required(ErrorMessage = "Unique Identifier is Required")]
        public string UniqueIdentifier { get; set; }

        [Required(ErrorMessage = "Repeat Recordings is Required")]
        public Boolean RepeatRecordings { get; set; }
        [Required(ErrorMessage = "Switch is Required")]
        public int Switch { get; set; }
        [Required(ErrorMessage = "ShowOnWebsite is Required")]
        public Boolean ShowOnWebsite { get; set; }
        [Required(ErrorMessage = "DisplayOrder is Required")]
        public int DisplayOrder { get; set; }

    }
}
