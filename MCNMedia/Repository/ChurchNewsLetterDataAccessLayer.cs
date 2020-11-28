using MCNMedia_Dev.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Repository
{
    public class ChurchNewsLetterDataAccessLayer
    {
        AwesomeDal.DatabaseConnect _dc;
        private readonly string AWS_S3_BUCKET_URI;

        public ChurchNewsLetterDataAccessLayer()
        {
            _dc = new AwesomeDal.DatabaseConnect();
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            var root = builder.Build();
            var awsS3bucket = root.GetSection("S3BucketConfiguration");
            var sysConfig = root.GetSection("SystemConfiguration");
            AWS_S3_BUCKET_URI = $"{awsS3bucket["aws_bucket_url"]}/{sysConfig["system_mode"]}";
        }

        public int AddNewsLetter(NewsLetter newsletter)
        {
            _dc.ClearParameters();
            _dc.AddParameter("UserId", newsletter.UpdatedBy);
            _dc.AddParameter("NewsTitle", newsletter.NewsLetterTitle);
            _dc.AddParameter("ShowOnWeb", newsletter.ShowOnWebsite);
            _dc.AddParameter("NewsUrl", newsletter.NewsLetterUrl);
            _dc.AddParameter("NewsName", newsletter.NewsLetterName);
            _dc.AddParameter("ChurchId2", newsletter.ChurchId);
            return _dc.Execute("spChurch_Newsletter_Add");
        }

        public IEnumerable<NewsLetter> GetNewsLetterByChurchId(int ChrId)
        {
            List<NewsLetter> newsLetters = new List<NewsLetter>();

            _dc.ClearParameters();
          _dc.AddParameter("chrId", ChrId);     
            DataTable dataTable = _dc.ReturnDataTable("spChurch_NewsLetter_GetByChurchId");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                NewsLetter chnewsLetter = new NewsLetter();
                chnewsLetter.ChurchNewsLetterId = Convert.ToInt32(dataRow["ChurchNewsLetterId"].ToString());
                chnewsLetter.NewsLetterTitle = dataRow["NewsLetterTitle"].ToString();
                chnewsLetter.NewsLetterName = dataRow["NewsLetterName"].ToString();
                chnewsLetter.ShowOnWebsite = Convert.ToBoolean(dataRow["ShowOnWebsite"]);
                chnewsLetter.CreatedAt = Convert.ToDateTime(dataRow["CreatedAt"].ToString());
                chnewsLetter.CreatedBy = dataRow["FirstName"].ToString();
                chnewsLetter.NewsLetterUrl = $"{AWS_S3_BUCKET_URI}/{dataRow["NewsLetterUrl"]}";
                newsLetters.Add(chnewsLetter);
            }
            return newsLetters;
        }

        public IEnumerable<NewsLetter> GetNewsLetterByChurch(int churchId)
        {
            return GetNewsletterFromDatabase(churchId: churchId, newsletterId: -1);
        }

        public NewsLetter GetNewsLetterById(int newsletterId)
        {
            NewsLetter newsLetter = new NewsLetter();
            List<NewsLetter> newsLetters = GetNewsletterFromDatabase(churchId: -1, newsletterId: newsletterId).ToList();
            if(newsLetters.Count>0)
            {
                newsLetter = newsLetters.First();
            }
            return newsLetter;
        }

        private IEnumerable<NewsLetter> GetNewsletterFromDatabase(int churchId, int newsletterId)
        {
            List<NewsLetter> newsLetters = new List<NewsLetter>();

            _dc.ClearParameters();
            _dc.AddParameter("ChrchId", churchId);
            _dc.AddParameter("NewsletterId", newsletterId);
            DataTable dataTable = _dc.ReturnDataTable("spChurch_Newsletter_Get");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                NewsLetter newsletter = BindingNewsletter(dataRow);
                newsLetters.Add(newsletter);
            }
            return newsLetters;
        }

        public NewsLetter GetLetestNewsletterByChurch(int churchId)
        {
            NewsLetter newsletter = new NewsLetter();
            _dc.ClearParameters();
            _dc.AddParameter("ChrchId", churchId);
            DataTable dataTable = _dc.ReturnDataTable("spChurch_Newsletter_GetLatestByChurch");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                 newsletter = BindingNewsletter(dataRow);
            }
            return newsletter;
        }

        public int UpdateNewsLetter(NewsLetter newsletter)
        {
            _dc.ClearParameters();
            _dc.AddParameter("ChNewsLetterId", newsletter.ChurchNewsLetterId);
            _dc.AddParameter("NewsTitle", newsletter.NewsLetterTitle);
            _dc.AddParameter("NewsUrl", newsletter.NewsLetterUrl);
            _dc.AddParameter("NewsName", newsletter.NewsLetterName);
            _dc.AddParameter("ShowOnWeb", newsletter.ShowOnWebsite);
            _dc.AddParameter("UserId", newsletter.UpdatedBy);

            return _dc.Execute("spChurch_NewsLetter_Update");
        }

        public bool DeleteNewsLetter(int chNewsLetterId, int UpdateBy)
        {
            _dc.ClearParameters();
            _dc.AddParameter("ChNewsLetterId", chNewsLetterId);
            _dc.AddParameter("UserId", UpdateBy);
            return _dc.ReturnBool("spChurch_NewsLetter_Delete");
        }

        private NewsLetter BindingNewsletter(DataRow dataRow)
        {
            NewsLetter newsletter = new NewsLetter();
            newsletter.ChurchNewsLetterId = Convert.ToInt32(dataRow["ChurchNewsLetterId"].ToString());
            newsletter.NewsLetterTitle = dataRow["NewsLetterTitle"].ToString();
            newsletter.NewsLetterName = dataRow["NewsLetterName"].ToString();
            newsletter.ShowOnWebsite = Convert.ToBoolean(dataRow["ShowOnWebsite"]);
            newsletter.CreatedAt = Convert.ToDateTime(dataRow["CreatedAt"].ToString());
            newsletter.CreatedBy = $"{dataRow["FirstName"]} {dataRow["LastName"]}";
            newsletter.NewsLetterUrl = $"{AWS_S3_BUCKET_URI}/{dataRow["NewsLetterUrl"]}";
            return newsletter;
        }
    }
}
