using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class Server
    {
        public int ServerId { get; set; }
        public string ServerName { get; set; }
        public string ServerIP { get; set; }
        public string ServerPort { get; set; }
        public string ServerUser { get; set; }
        public string ServerPassword { get; set; }
    }
}
