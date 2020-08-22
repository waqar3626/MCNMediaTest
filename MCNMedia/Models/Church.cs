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

        private string _UniqueChurchId = string.Empty;
        public string UniqueChurchId
        {
            get { return _UniqueChurchId; }
            set => _UniqueChurchId = Guid.NewGuid().ToString();

        }

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

        public int UpdateBy { get; set; }
        public string ImageURl { get; set; }
        public string Blurb { get; set; }

        private string _Slug = string.Empty;
        public string Slug
        {
            get { return _Slug; }
            set => _Slug = Guid.NewGuid().ToString();
        }

        public string Notice { get; set;}

        public int Featured { get; set; }
        
        private string _UniqueIdentifier = string.Empty;
        public string UniqueIdentifier
        {
            get { return _UniqueIdentifier; }
            set => _UniqueIdentifier = Guid.NewGuid().ToString();
        }
        public Boolean RepeatRecordings { get; set; }
       
        public int Switch { get; set; }
     
        public Boolean ShowOnWebsite { get; set; }
     
        public int DisplayOrder { get; set; }

        public string ClientTypeTitle { get; set; }

        public string CountyName { get; set; }

        public class ClientType
        {
            public int ClientTypeId { get; set; }
            public String ClientTypeTitle { get; set; }
        }

        public class Counties
        {
            public int CountyId { get; set; }
            public String CountyName { get; set; }
        }



    }
}
