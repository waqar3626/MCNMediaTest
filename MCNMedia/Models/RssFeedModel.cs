using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class RssFeedModel
    {
        public int RssFeedId { get; set; }

        public string RssFeedTitle { get; set; }

        public string Description { get; set; }

        public DateTime PublishDate { get; set; }
    }
}
