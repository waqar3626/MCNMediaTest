using Google.Apis.Analytics.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using MCNMedia_Dev.Controllers;
using MCNMedia_Dev.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace MCNMedia_Dev._Helper
{
    public class GoogleAnalytics
    {
        private string keyFilePath = @"/WebsiteAssets/mcnservice-6f2c92e8dee2.p12"; // .p12 file of MCNLive Trafic
        private string serviceAccountEmail = "mcn-802@mcnservice.iam.gserviceaccount.com"; //php
        private string keyPassword = "notasecret";
        private string websiteCode = "237777915"; // view id of MCNLive Trafic

        private AnalyticsService analyticsservice = null;
        private IWebHostEnvironment environment;

        public GoogleAnalytics(IWebHostEnvironment _environment)
        {
            environment = _environment;
        }

        private void Authenticate()
        {
            try
            {
                string webRootPath = environment.WebRootPath + keyFilePath;
                //loading the Key file
                var certificate = new X509Certificate2(webRootPath, keyPassword, X509KeyStorageFlags.Exportable);
                var scopes =
                    new string[] {
                        AnalyticsService.Scope.Analytics,              // view and manage your analytics data
                        AnalyticsService.Scope.AnalyticsEdit,          // edit management actives
                        AnalyticsService.Scope.AnalyticsManageUsers,   // manage users
                        AnalyticsService.Scope.AnalyticsReadonly
                    };     // View analytics data       

                var credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(serviceAccountEmail)
                {
                    Scopes = scopes
                }.FromCertificate(certificate));

                analyticsservice = new AnalyticsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential
                });
            }
            catch (Exception ex)
            {
                _Helper.Common.SaveToXXX(ex.StackTrace);
            }
        }

        public IEnumerable<GoogleAnalyticsProperty> QueryDataPer(DateTime dateTime)
        {
            List<GoogleAnalyticsProperty> Balobj = new List<GoogleAnalyticsProperty>();
            DataResource.GaResource.GetRequest request = analyticsservice.Data.Ga.Get(
               "ga:" + websiteCode,
               dateTime.ToString("yyyy-MM-dd"),
               dateTime.ToString("yyyy-MM-dd"),
               "ga:pageviews");
            //DataResource.GaResource.GetRequest request = analyticsservice.Data.Ga.Get(
            //   "ga:" + websiteCode,
            //   //DateTime.Now.AddDays(-7).ToString(),
            //   //dateTime.ToString("yyyy-MM-dd"),
            //   "2020-12-01",
            //   "2021-03-01",
            //   "ga:sessions");
            request.Dimensions = "ga:year,ga:month,ga:day,ga:country";
            //request.Dimensions = "ga:country";
            var data = request.Execute();
            //checked if no hits occures
            if (data.Rows == null)
            {
                GoogleAnalyticsProperty googleAnalytics = new GoogleAnalyticsProperty();
                googleAnalytics.CountryName = "";
                googleAnalytics.Count = 0;
                Balobj.Add(googleAnalytics);

                return Balobj;
            }
            else
            {
                foreach (var row in data.Rows)
                {
                    GoogleAnalyticsProperty googleAnalytics = new GoogleAnalyticsProperty();
                    googleAnalytics.CountryName = row[3].ToString();
                    googleAnalytics.Count = int.Parse(row[4]);
                    Balobj.Add(googleAnalytics);
                    //visitsData.Add(new ChartRecord(new DateTime(int.Parse(row[0]), int.Parse(row[1]), int.Parse(row[2])).ToString("MM-dd-yyyy"), row[3].ToString(), int.Parse(row[4])));
                }
                return Balobj;
            }
        }

        public IEnumerable<GoogleAnalyticsProperty> QueryDataPerChurch(DateTime dateTime)
        {
            List<GoogleAnalyticsProperty> Balobj = new List<GoogleAnalyticsProperty>();
            DataResource.GaResource.GetRequest request = analyticsservice.Data.Ga.Get(
               "ga:" + websiteCode,
               dateTime.ToString("yyyy-MM-dd"),
               dateTime.ToString("yyyy-MM-dd"),
               //"ga:sessions"
               "ga:pageviews"
               );
            request.Dimensions = "ga:year,ga:month,ga:day,ga:country,ga:pageTitle";
            var data = request.Execute();
            //checked if no hits occures
            if (data.Rows == null)
            {
                GoogleAnalyticsProperty googleAnalytics = new GoogleAnalyticsProperty();
                googleAnalytics.CountryName = "";
                googleAnalytics.Count = 0;
                Balobj.Add(googleAnalytics);
                return Balobj;
            }
            else
            {
                foreach (var row in data.Rows)
                {
                    GoogleAnalyticsProperty googleAnalytics = new GoogleAnalyticsProperty();
                    googleAnalytics.CountryName = row[3].ToString();
                    googleAnalytics.Count = int.Parse(row[5]);
                    googleAnalytics.PageTitle = (row[4]).ToString();
                    Balobj.Add(googleAnalytics);
                    //visitsData.Add(new ChartRecord(new DateTime(int.Parse(row[0]), int.Parse(row[1]), int.Parse(row[2])).ToString("MM-dd-yyyy"), row[3].ToString(), int.Parse(row[4])));
                }
                return Balobj;
            }
        }

        public List<GoogleAnalyticsProperty> GoogleAnalytics_GetAll(DateTime dateTime)
        {
            //Google.Apis.Analytics.v3.Data.GaData data = GoogleAnalytics_Get(dateTime, "ga:year,ga:month,ga:day,ga:country");
            Google.Apis.Analytics.v3.Data.GaData data = GoogleAnalytics_Get(dateTime, "ga:userType,ga:month,ga:day,ga:country");
            List<GoogleAnalyticsProperty> googleAnalyticsProperties = new List<GoogleAnalyticsProperty>();
            if (data.Rows != null)
            {
                foreach (var row in data.Rows)
                {
                    GoogleAnalyticsProperty googleAnalytics = new GoogleAnalyticsProperty();
                    googleAnalytics.CountryName = row[3].ToString();
                    googleAnalytics.Count = int.Parse(row[4]);
                    googleAnalyticsProperties.Add(googleAnalytics);
                }
            }
            return googleAnalyticsProperties;
        }

        public List<GoogleAnalyticsProperty> GoogleAnalytics_GetByChurch(DateTime dateTime)
        {
            string dimensions = "ga:year,ga:month,ga:day,ga:country,ga:pageTitle";
            Google.Apis.Analytics.v3.Data.GaData data = GoogleAnalytics_Get(dateTime, dimensions);
            List<GoogleAnalyticsProperty> googleAnalyticsProperties = new List<GoogleAnalyticsProperty>();
            if (data.Rows != null)
            {
                foreach (var row in data.Rows)
                {
                    GoogleAnalyticsProperty googleAnalytics = new GoogleAnalyticsProperty();
                    googleAnalytics.CountryName = row[3].ToString();
                    googleAnalytics.Count = int.Parse(row[5]);
                    googleAnalytics.PageTitle = (row[4]).ToString();
                    googleAnalyticsProperties.Add(googleAnalytics);
                }
            }
            return googleAnalyticsProperties;
        }

        private Google.Apis.Analytics.v3.Data.GaData GoogleAnalytics_Get(DateTime dateTime,string dimensions)
        {
            Authenticate();
            DataResource.GaResource.GetRequest request = GetGoogleAnalyticRequest(dateTime);
            request.Dimensions =dimensions;
            var data = request.Execute();
            return data;
        }

       

        private DataResource.GaResource.GetRequest GetGoogleAnalyticRequest(DateTime dateTime)
        {
            return analyticsservice.Data.Ga.Get(
               "ga:" + websiteCode,
               dateTime.ToString("yyyy-MM-dd"),
               dateTime.ToString("yyyy-MM-dd"),
               "ga:users");
               //"ga:pageviews");
        }

    }

}

