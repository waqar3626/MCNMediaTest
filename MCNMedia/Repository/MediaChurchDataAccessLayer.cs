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
            _dc.CloseAndDispose();
            _dc.ClearParameters();
            _dc.AddParameter("UserId", media.UpdatedBy);
            _dc.AddParameter("MedType", media.MediaType);
            _dc.AddParameter("MedTabName", media.TabName);
            _dc.AddParameter("MedURL", media.MediaURL);
            _dc.AddParameter("MedName", media.MediaName);
            _dc.AddParameter("ChurchId2", media.ChurchId);
            return _dc.ReturnInt("spChurchMedia_Add");
        }

        public int AddSlideShowImages(int ChurchMediaId, string MediaUrl, int DisplayOrder, int CreatedBy)
        {
            _dc.CloseAndDispose();
            _dc.ClearParameters();
            _dc.AddParameter("churchMediaId", ChurchMediaId);
            _dc.AddParameter("mediaURL", MediaUrl);
            _dc.AddParameter("displayOrder", DisplayOrder);
            _dc.AddParameter("createdBy", CreatedBy);
            return _dc.ReturnInt("spSlideShowImages_Add");
        }

        public IEnumerable<MediaChurch> GetByMediaType(string medType, int ChrId)
        {
            List<MediaChurch> Balobj = new List<MediaChurch>();
            _dc.CloseAndDispose();
            _dc.ClearParameters();
            _dc.AddParameter("chrId", ChrId);
            _dc.AddParameter("MedType", medType);
            DataTable dataTable = _dc.ReturnDataTable("spChurchMedia_GetByMediaType");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                MediaChurch mdChurch = new MediaChurch();
                mdChurch.ChurchMediaId = Convert.ToInt32(dataRow["ChurchMediaId"].ToString());
                mdChurch.TabName = dataRow["TabName"].ToString();
                mdChurch.MediaName = dataRow["MediaName"].ToString();
                mdChurch.CreatedAt = Convert.ToDateTime(dataRow["CreatedAt"].ToString());
                mdChurch.SysTime = Convert.ToDateTime(dataRow["CreatedAt"]).ToString("dd-MMM-yyyy");
                mdChurch.CreatedBy = dataRow["FirstName"].ToString();
                mdChurch.MediaURL = $"{AWS_S3_BUCKET_URI}/{dataRow["MediaURL"]}";
                Balobj.Add(mdChurch);
            }
            return Balobj;
        }

        public MediaChurch GetMediaById(int mediaId)
        {
            MediaChurch mediaChurch = new MediaChurch();
            _dc.CloseAndDispose();
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
                mediaChurch.ChurchName = dataRow["ChurchName"].ToString();
            }
            return mediaChurch;
        }

        public int UpdateMedia(MediaChurch med)
        {
            _dc.CloseAndDispose();
            _dc.ClearParameters();
            _dc.AddParameter("MediaId", med.ChurchMediaId);
            _dc.AddParameter("MedTabName", med.TabName);
            //_dc.AddParameter("MedURL", med.MediaURL);
            _dc.AddParameter("MedName", med.MediaName);
            _dc.AddParameter("UserId", med.UpdatedBy);

            return _dc.Execute("spChurchMedia_Update");
        }

        public int DeleteMedia(int chMediaId, int UpdateBy)
        {
            _dc.CloseAndDispose();
            _dc.ClearParameters();
            _dc.AddParameter("MediaId", chMediaId);
            _dc.AddParameter("UserId", UpdateBy);
            _dc.ReturnBool("spChurchMedia_Delete");
            return 1;
        }
        public int DeleteSlideShowImages(int chMediaId, int UpdateBy)
        {
            _dc.CloseAndDispose();
            _dc.ClearParameters();
            _dc.AddParameter("MediaId", chMediaId);
            _dc.AddParameter("UserId", UpdateBy);
            _dc.ReturnBool("spSlideShowImage_Delete");
            return 1;
        }

        public IEnumerable<MediaChurch> SlideShowImaeGetAll(int chrId)
        {
            List<MediaChurch> Balobj = new List<MediaChurch>();
            _dc.CloseAndDispose();
            _dc.ClearParameters();
            _dc.AddParameter("chrId", chrId);
            DataTable dataTable = _dc.ReturnDataTable("spChurchSlideshowImage_GetAll");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                MediaChurch mdChurch = new MediaChurch();
                mdChurch.ChurchMediaId = Convert.ToInt32(dataRow["ChurchMediaId"].ToString());
                mdChurch.TabName = dataRow["TabName"].ToString();
                mdChurch.MediaName = dataRow["MediaDetail"].ToString();
                mdChurch.MediaURL = $"{AWS_S3_BUCKET_URI}/";
                Balobj.Add(mdChurch);
            }
            return Balobj;
        }

        public IEnumerable<MediaChurch> SlideShowImaeGetByMediaId(int chrId)
        {
            List<MediaChurch> Balobj = new List<MediaChurch>();
            _dc.CloseAndDispose();
            _dc.ClearParameters();
            _dc.AddParameter("churchMediaId", chrId);
            DataTable dataTable = _dc.ReturnDataTable("spGetSlideShowImagesByMediaId");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                MediaChurch mdChurch = new MediaChurch();
                mdChurch.ChurchMediaId = Convert.ToInt32(dataRow["ChurchMediaId"].ToString());
                mdChurch.TabName = dataRow["TabName"].ToString();
                mdChurch.ChurchId = Convert.ToInt32(dataRow["ChurchId"].ToString());
                mdChurch.ImageId = Convert.ToInt32(dataRow["ImageId"].ToString());
                mdChurch.DisplayOrder = Convert.ToInt32(dataRow["DisplayOrder"].ToString());
                mdChurch.MediaURL = $"{AWS_S3_BUCKET_URI}/{dataRow["MediaURL"]}";
                Balobj.Add(mdChurch);
            }
            return Balobj;
        }

        public IEnumerable<MediaChurch> spSlideShowImagesGetByChurch(int chrId)
        {
            List<MediaChurch> Balobj = new List<MediaChurch>();
            _dc.CloseAndDispose();
            _dc.ClearParameters();
            _dc.AddParameter("chrId", chrId);
            DataTable dataTable = _dc.ReturnDataTable("spSlideShowImages_GetByChurch");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                MediaChurch mdChurch = new MediaChurch();
                mdChurch.ChurchMediaId = Convert.ToInt32(dataRow["ChurchMediaId"].ToString());
                mdChurch.TabName = dataRow["TabName"].ToString();
                mdChurch.ChurchId = Convert.ToInt32(dataRow["ChurchId"].ToString());
                mdChurch.ImageId = Convert.ToInt32(dataRow["ImageId"].ToString());
                mdChurch.MediaType = dataRow["MediaType"].ToString();
                mdChurch.DisplayOrder = Convert.ToInt32(dataRow["DisplayOrder"].ToString());
                mdChurch.MediaURL = $"{AWS_S3_BUCKET_URI}/{dataRow["MediaURL"]}";
                Balobj.Add(mdChurch);
            }
            return Balobj;
        }
        public int DeleteSlideShowSingleImage(int chMediaId, int UpdateBy)
        {
            _dc.CloseAndDispose();
            _dc.ClearParameters();
            _dc.AddParameter("MediaId", chMediaId);
            _dc.AddParameter("UserId", UpdateBy);
            _dc.ReturnBool("spSlideShowDeleteSingleImage");
            return 1;
        }

        public bool ChangeSlideShowImageOrder(int ImageId, int chMediaId, int DisplayOrder, int UpdateBy)
        {
            _dc.CloseAndDispose();
            _dc.ClearParameters();
            _dc.AddParameter("imgId", ImageId);
            _dc.AddParameter("medChrId", chMediaId);
            _dc.AddParameter("disOrder", DisplayOrder);
            _dc.AddParameter("CrBy", UpdateBy);
            return _dc.ReturnBool("spChangeSlideShowImageOrder");

        }
        public string fbAppKeyget()
        {

            DataTable dataTable = _dc.ReturnDataTable("sp_fbAppKeyget");

            int id = 0;
            string FacebookAppKey = "";
            foreach (DataRow dataRow in dataTable.Rows)
            {

                id = Convert.ToInt32(dataRow["Id"]);
                FacebookAppKey = dataRow["FacebookAppKey"].ToString();


            }
            return FacebookAppKey;
        }
    }
}