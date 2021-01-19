using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class GenericSubscriber
    {
        public Subscriptions subscriptions { get; set; }
        public IEnumerable<Subscriptions> Lsubscriptions { get; set; }
    }
}
