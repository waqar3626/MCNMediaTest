using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using MCNMedia_Dev._Helper;
using Microsoft.AspNetCore.Mvc;

namespace MCNMedia_Dev.Controllers
{
    public class RokuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public XElement GetCategories()
        {
            Roku roku = new Roku();
            HttpContext.Response.Headers.Add("Content-Type", "text/xml");
            return XElement.Parse( roku.GetCounty());
        }

        [HttpGet]
        public XElement GetRoku()
        {
            Roku roku = new Roku();
            HttpContext.Response.Headers.Add("Content-Type", "application/rss+xml");
            return XElement.Parse(roku.RokuRSS());
        }

        [HttpGet]
        public XElement GetCamByCategory()
        {
            string slug = HttpContext.Request.Query["id"].ToString();
            Roku roku = new Roku();
            HttpContext.Response.Headers.Add("Content-Type", "text/xml");
            return XElement.Parse(roku.GetCameraByCounty(slug));
        }
    }
}
