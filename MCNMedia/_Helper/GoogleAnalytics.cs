using Google.Analytics.SDK.Core;
using Google.Analytics.SDK.Core.Extensions;
using Google.Analytics.SDK.Core.Services.Interfaces;
using Google.Apis.Analytics.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using MCNMedia_Dev.Controllers;
using MCNMedia_Dev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace MCNMedia_Dev._Helper
{


    public class GoogleAnalytics
    {
        private string keyFilePath = @"C:\Users\pc\Downloads\mcn-project-303512-fe457b99555b.p12";
        //private string keyFilePath = "WebsiteAssets/mcn-project-303512-c459210865b0.p12";
        private string serviceAccountEmail = "mcn-console-api-service-acct@mcn-project-303512.iam.gserviceaccount.com";
        private string keyPassword = "notasecret";
        private string websiteCode = "236800193";
        private AnalyticsService service = null;
        private List<ChartRecord> visitsData = new List<ChartRecord>();

        public void Authenticate()
        {
            //loading the Key file
            var certificate = new X509Certificate2(keyFilePath, keyPassword, X509KeyStorageFlags.Exportable);

            var scopes =
          new string[] {
             AnalyticsService.Scope.Analytics,              // view and manage your analytics data
             AnalyticsService.Scope.AnalyticsEdit,          // edit management actives
             AnalyticsService.Scope.AnalyticsManageUsers,   // manage users
             AnalyticsService.Scope.AnalyticsReadonly};     // View analytics data       

            var credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(serviceAccountEmail)
            {
                Scopes = scopes
            }.FromCertificate(certificate));

            service = new AnalyticsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });
        }

        //public IEnumerable<GoogleAnalyticsProperty> QueryData(DateTime dateTime)
        //{
        //    List<GoogleAnalyticsProperty> Balobj = new List<GoogleAnalyticsProperty>();

        //    DataResource.GaResource.GetRequest request = service.Data.Ga.Get(
        //       "ga:" + websiteCode,
        //       //DateTime.Today.AddDays(-15).ToString("yyyy-MM-dd"),
        //       //DateTime.Today.ToString("yyyy-MM-dd"),
        //       dateTime.ToString("yyyy-MM-dd"),
        //       dateTime.ToString("yyyy-MM-dd"),
        //       //"ga:st-john-the-baptist-church-clontarf-dublin",
        //       "ga:sessions");
        //    request.Dimensions = "ga:year,ga:month,ga:day,ga:country";
        //    request.Dimensions = "ga:country";
        //    var data = request.Execute();
        //    //checked if no hits occures
        //    GoogleAnalyticsProperty googleAnalytics = new GoogleAnalyticsProperty();
        //    if (data.Rows == null)
        //    {


        //            googleAnalytics.CountryName = "";
        //            googleAnalytics.Count = 0;
        //            Balobj.Add(googleAnalytics);

        //        return Balobj;
        //    }
        //    else
        //    {
        //        foreach (var row in data.Rows)
        //        {
        //            googleAnalytics.CountryName = row[0].ToString();
        //            googleAnalytics.Count = int.Parse(row[1]);
        //            //googleAnalytics.PageTitle = (row[5]).ToString();
        //            Balobj.Add(googleAnalytics);
        //            //visitsData.Add(new ChartRecord(new DateTime(int.Parse(row[0]), int.Parse(row[1]), int.Parse(row[2])).ToString("MM-dd-yyyy"), row[3].ToString(), int.Parse(row[4])));
        //        }
        //        return Balobj;
        //    }
        //}


        public IEnumerable<GoogleAnalyticsProperty> QueryDataPer(DateTime dateTime)
        {
            List<GoogleAnalyticsProperty> Balobj = new List<GoogleAnalyticsProperty>();

            DataResource.GaResource.GetRequest request = service.Data.Ga.Get(
               "ga:" + websiteCode,
               dateTime.ToString("yyyy-MM-dd"),
               dateTime.ToString("yyyy-MM-dd"),
               //DateTime.Today.AddDays(-15).ToString("yyyy-MM-dd"),
               //DateTime.Today.AddDays(-15).ToString("yyyy-MM-dd"),
               //DateTime.Today.ToString("yyyy-MM-dd"),
               //dateTime.ToString("yyyy-MM-dd"),
               //dateTime.ToString("yyyy-MM-dd"),
               //"ga:st-john-the-baptist-church-clontarf-dublin",
               "ga:pageviews");
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
                    //googleAnalytics.PageTitle = (row[4]).ToString();
                    Balobj.Add(googleAnalytics);
                    //visitsData.Add(new ChartRecord(new DateTime(int.Parse(row[0]), int.Parse(row[1]), int.Parse(row[2])).ToString("MM-dd-yyyy"), row[3].ToString(), int.Parse(row[4])));
                }
                return Balobj;
            }
        }
        public IEnumerable<GoogleAnalyticsProperty> QueryDataPerChurch(DateTime dateTime)
        {
            List<GoogleAnalyticsProperty> Balobj = new List<GoogleAnalyticsProperty>();

            DataResource.GaResource.GetRequest request = service.Data.Ga.Get(
               "ga:" + websiteCode,
               DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd"),
               DateTime.Today.ToString("yyyy-MM-dd"),
               //dateTime.ToString("yyyy-MM-dd"),
               //dateTime.ToString("yyyy-MM-dd"),
               //"ga:st-john-the-baptist-church-clontarf-dublin",
               //"ga:sessions"
               "ga:pageviews"
               );
            request.Dimensions = "ga:year,ga:month,ga:day,ga:country,ga:pageTitle";
            //request.Dimensions = "ga:pageviews";
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
                    googleAnalytics.Count = int.Parse(row[2]);
                    googleAnalytics.PageTitle = (row[4]).ToString();
                    Balobj.Add(googleAnalytics);
                    //visitsData.Add(new ChartRecord(new DateTime(int.Parse(row[0]), int.Parse(row[1]), int.Parse(row[2])).ToString("MM-dd-yyyy"), row[3].ToString(), int.Parse(row[4])));
                }
                return Balobj;
            }
        }

    }

}

