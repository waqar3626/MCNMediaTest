using MCNMedia_Dev.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Repository
{
    public class MediaChurchDataAccessLayer
    {
        AwesomeDal.DatabaseConnect _dc;

        public MediaChurchDataAccessLayer()
        {
            _dc = new AwesomeDal.DatabaseConnect();
        }

        

        //public IEnumerable<Picture> GetAllPictures()
        //{
        //    List<Picture> Balobj = new List<Picture>();
        //    _dc.ClearParameters();
        //    _dc.AddParameter("CamName", "");
        //    DataTable dataTable = _dc.ReturnDataTable("Sp_ProcedureName");
        //    foreach (DataRow dataRow in dataTable.Rows)
        //    {
        //        Picture picture = new Picture();
        //        Picture.ChurchMediaId = Convert.ToInt32(dataRow["CameraId"]);
        //        Picture.MediaType = dataRow["CameraName"].ToString();
        //        Picture.HttpPort = dataRow["HttpPort"].ToString();
        //        Picture.CameraUrl = dataRow["CameraUrl"].ToString();
        //        Picture.RtspPort = dataRow["RtspPort"].ToString();
        //        Picture.ChurchId = Convert.ToInt32(dataRow["ChurchId"].ToString());
        //        Picture.ChurchName = dataRow["ChurchName"].ToString();
        //        Balobj.Add(picture);
        //    }
        //    return Balobj;
        //}
        public int AddMedia(MediaChurch media)
        {
            _dc.ClearParameters();
            _dc.AddParameter("UserId", media.CreatedBy);
            _dc.AddParameter("MedType", media.MediaType);
            _dc.AddParameter("MedTabName", media.TabName);
            _dc.AddParameter("MedURL", media.MediaURL);
            _dc.AddParameter("MedName",media.MediaName);
            _dc.AddParameter("ChurchId2", media.ChurchId);
            return _dc.Execute("spChurchMedia_Add");
        }
        
        public IEnumerable<MediaChurch> GetByMediaType(string medType)
        {
            List<MediaChurch> Balobj = new List<MediaChurch>();

            _dc.ClearParameters();
            _dc.AddParameter("MedType", medType);
            DataTable dataTable = _dc.ReturnDataTable("spChurchMedia_GetByMediaType");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                MediaChurch mdChurch = new MediaChurch();
                mdChurch.ChurchMediaId = Convert.ToInt32(dataRow["ChurchMediaId"].ToString());
                mdChurch.TabName= dataRow["TabName"].ToString();
                mdChurch.CreatedAt = Convert.ToDateTime( dataRow["CreatedAt"].ToString());
                mdChurch.CreatedBy =Convert.ToInt32(dataRow["CreatedBy"].ToString());
                Balobj.Add(mdChurch);
            }
            return Balobj;
        }
        public MediaChurch GetMediaById(int ChMediaId)
        {
            MediaChurch mediaChurch = new MediaChurch();

            _dc.ClearParameters();
            _dc.AddParameter("ChMediaId", ChMediaId);
            DataTable dataTable = _dc.ReturnDataTable("spChurchMedia_GetById");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                mediaChurch.ChurchMediaId = Convert.ToInt32(dataRow["ChurchMediaId"]);
                mediaChurch.TabName = dataRow["TabName"].ToString();
                mediaChurch.MediaType = dataRow["MediaType"].ToString();
                mediaChurch.MediaURL = dataRow["MediaURL"].ToString();
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

        public bool DeleteMedia(int ChMediaId)
        {
            _dc.ClearParameters();
            _dc.AddParameter("MediaId", ChMediaId);
            _dc.AddParameter("UserId",1);
            return _dc.ReturnBool("spChurchMedia_Delete");
        }
    }
}