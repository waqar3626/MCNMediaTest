using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using MaxMind.GeoIP2;
using System.Net.Mail;
using MCNMedia_Dev._Helper;
using MCNMedia_Dev.Models;
using Microsoft.AspNetCore.Hosting;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static MCNMedia_Dev.Models.Church;
using System.IO;
using MaxMind.GeoIP2.Model;

namespace MCNMedia_Dev._Helper
{
    public class Visitor
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool IsLocal { get; set; }
        public string CountryName { get; set; }
        public string IpAddress { get; set; }

        public Visitor(HttpRequest httpRequest, HttpContext httpContext)
        {
            if(IsEnvironmentLocal(httpContext.Connection))
            {
                //Common.SaveToXXX("visited just now -Pak2 " + DateTime.Now.ToString());
                //log.Info($"Visitor from localhost dated: {DateTime.Now.ToString()}" );
                CountryName = "Pakistan";
                IsLocal = true;
            }
            else
            {
                GetVisitorLocation(httpRequest, httpContext);
                IsLocal = false;
            }
        }

        private void GetVisitorLocation(HttpRequest Request,HttpContext httpContext)
        {
            string rootPath = Directory.GetCurrentDirectory();
            using (var reader = new DatabaseReader(Path.Combine(rootPath, "wwwroot/GeoLite2-Country.mmdb")))
            {
                // Determine the IP Address of the request
                IPAddress ipAddress;

                ipAddress = GetApAddress(Request);
                IpAddress = ipAddress.ToString();
                // Get the city from the IP Address
                var countryInfo = reader.Country(ipAddress);
                CountryName = countryInfo.Country.ToString();
                //Common.SaveToXXX($"Visitor IP: {IpAddress} and Country: {CountryName}");
                //log.Info($"Visitor IP: {IpAddress} and Country: {CountryName}");
            }
        }

        private IPAddress GetApAddress(HttpRequest Request)
        {
            IPAddress ipAddress;
            var headers = Request.Headers.ToList();
            if (headers.Exists((kvp) => kvp.Key == "X-Forwarded-For"))
            {
                // when running behind a load balancer you can expect this header
                var header = headers.First((kvp) => kvp.Key == "X-Forwarded-For").Value.ToString();
                ipAddress = IPAddress.Parse(header);
            }
            else
            if (headers.Exists((kvp) => kvp.Key == "REMOTE_ADDR"))
            {
                // when running behind a load balancer you can expect this header
                var header = headers.First((kvp) => kvp.Key == "REMOTE_ADDR").Value.ToString();
                ipAddress = IPAddress.Parse(header);
            }
            else
            {
                // this will always have a value (running locally in development won't have the header)
                ipAddress = Request.HttpContext.Connection.RemoteIpAddress;
            }

            return ipAddress;
        }

        private bool IsEnvironmentLocal(ConnectionInfo connection)
        {
            var remoteAddress = connection.RemoteIpAddress.ToString();
            // if unknown, assume not local
            if (string.IsNullOrEmpty(remoteAddress))
                return false;

            // check if localhost
            if (remoteAddress == "127.0.0.1" || remoteAddress == "::1")
            {
                IpAddress = remoteAddress;
                return true;
            }
            // compare with local address
            if (remoteAddress == connection.LocalIpAddress.ToString())
            {
                IpAddress = remoteAddress;
                return true;
            }

            return false;
        }
    }
}
