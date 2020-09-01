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

        public IEnumerable<MediaChurch> GetPictureByMediaType()
        {
            List<MediaChurch> Balobj = new List<MediaChurch>();

            _dc.ClearParameters();
            _dc.AddParameter("MedType", "Picture");
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
        public MediaChurch GetPictureById(int ChMediaId)
        {
            MediaChurch picture = new MediaChurch();

            _dc.ClearParameters();
            _dc.AddParameter("ChMediaId", ChMediaId);
            DataTable dataTable = _dc.ReturnDataTable("spChurchMedia_GetById");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                picture.ChurchMediaId = Convert.ToInt32(dataRow["ChurchMediaId"]);
                picture.TabName = dataRow["TabName"].ToString();
                picture.MediaType = dataRow["MediaType"].ToString();
                picture.MediaURL = dataRow["MediaURL"].ToString();
                picture.MediaName = dataRow["MediaName"].ToString();

            }
            return picture;
        }


        public int UpdatePicture(MediaChurch picture)
        {
            _dc.ClearParameters();
            _dc.AddParameter("MediaId", picture.ChurchMediaId);
            _dc.AddParameter("MedTabName", picture.TabName);
            _dc.AddParameter("MedURL", picture.MediaURL);
            //_dc.AddParameter("MedName", picture.MediaName);
            _dc.AddParameter("UserId", picture.UpdatedBy);

            return _dc.Execute("spChurchMedia_Update");
        }
        public bool DeletePicture(int ChMediaId)
        {
            _dc.ClearParameters();
            _dc.AddParameter("MediaId", ChMediaId);
            _dc.AddParameter("UserId",1);
            return _dc.ReturnBool("spChurchMedia_Delete");
        }
    }
}