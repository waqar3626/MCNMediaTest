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
    public class MediaChurchDataAccessLayer
    {
        AwesomeDal.DatabaseConnect _dc;

        private readonly string AWS_S3_BUCKET_URI;
        private readonly string SYSTEM_MODE;

        public MediaChurchDataAccessLayer()
        {
            _dc = new AwesomeDal.DatabaseConnect();
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            var root = builder.Build();
            var awsS3bucket = root.GetSection("S3BucketConfiguration");
            var sysConfig = root.GetSection("SystemConfiguration");
            AWS_S3_BUCKET_URI = $"{awsS3bucket["aws_bucket_url"]}/{sysConfig["system_mode"]}";
        }

        

        
        public int AddMedia(MediaChurch media)
        {
            _dc.ClearParameters();
            _dc.AddParameter("UserId", media.UpdatedBy);
            _dc.AddParameter("MedType", media.MediaType);
            _dc.AddParameter("MedTabName", media.TabName);
            _dc.AddParameter("MedURL", media.MediaURL);
            _dc.AddParameter("MedName",media.MediaName);
            _dc.AddParameter("ChurchId2", media.ChurchId);
            return _dc.Execute("spChurchMedia_Add");
        }
        
        public IEnumerable<MediaChurch> GetByMediaType(string medType,int ChrId)
        {
            List<MediaChurch> Balobj = new List<MediaChurch>();

            _dc.ClearParameters();
            _dc.AddParameter("chrId", ChrId);
            _dc.AddParameter("MedType", medType);
            DataTable dataTable = _dc.ReturnDataTable("spChurchMedia_GetByMediaType");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                MediaChurch mdChurch = new MediaChurch();
                mdChurch.ChurchMediaId = Convert.ToInt32(dataRow["ChurchMediaId"].ToString());
                mdChurch.TabName= dataRow["TabName"].ToString();
                mdChurch.MediaName= dataRow["MediaName"].ToString();
                mdChurch.CreatedAt = Convert.ToDateTime( dataRow["CreatedAt"].ToString());
                mdChurch.CreatedBy = dataRow["FirstName"].ToString();
                Balobj.Add(mdChurch);
            }
            return Balobj;
        }
       
        public MediaChurch GetMediaById(int mediaId)
        {
            MediaChurch mediaChurch = new MediaChurch();

            _dc.ClearParameters();
            _dc.AddParameter("mediaId", mediaId);
            DataTable dataTable = _dc.ReturnDataTable("spChurchMedia_GetById");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                mediaChurch.ChurchMediaId = Convert.ToInt32(dataRow["ChurchMediaId"]);
                mediaChurch.TabName = dataRow["TabName"].ToString();
                mediaChurch.MediaType = dataRow["MediaType"].ToString();
                mediaChurch.MediaURL = $"{AWS_S3_BUCKET_URI}/{dataRow["MediaURL"]}"; 
                mediaChurch.MediaName = dataRow["MediaName"].ToString();

            }
            return mediaChurch;
        }


        public int UpdateMedia(MediaChurch med)
        {
            _dc.ClearParameters();
            _dc.AddParameter("MediaId", med.ChurchMediaId);
            _dc.AddParameter("MedTabName", med.TabName);
            _dc.AddParameter("MedURL", med.MediaURL);
            _dc.AddParameter("MedName", med.MediaName);
            _dc.AddParameter("UserId", med.UpdatedBy);

            return _dc.Execute("spChurchMedia_Update");
        }

        public bool DeleteMedia(int chMediaId,int UpdateBy)
        {
            _dc.ClearParameters();
            _dc.AddParameter("MediaId", chMediaId);
            _dc.AddParameter("UserId", UpdateBy);
            return _dc.ReturnBool("spChurchMedia_Delete");
        }
    }
}