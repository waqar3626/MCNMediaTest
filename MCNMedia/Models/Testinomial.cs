using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class Testinomial
    {
        public int TestinomialId { get; set; }
        public int ChurchId { get; set; }
        public string ChurchName { get; set; }
        public string Title { get; set; }
        public String Description { get; set; }
        public int DisplayOrder { get; set; }
        public int ShowOnWebsite { get; set; }
        public String eMail { get; set; }
        public String Name { get; set; }
    }
}
