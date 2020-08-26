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
        public void AddPictures(MediaChurch picture)
        {
            _dc.ClearParameters();
            _dc.AddParameter("UserId", picture.CreatedBy);
            _dc.AddParameter("MedType", picture.MediaType);
            _dc.AddParameter("MedTabName", picture.TabName);
            _dc.AddParameter("MedURL", picture.MediaURL);
            _dc.AddParameter("MedName", picture.MediaName);
            _dc.Execute("spCamera_Add");
        }

        public MediaChurch GetPicturesByMediaType(int camId)
        {
            MediaChurch picture = new MediaChurch();

            _dc.ClearParameters();
            _dc.AddParameter("MedType", camId);
            DataTable dataTable = _dc.ReturnDataTable("spChurchMedia_GetByMediaType");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                
                picture.TabName= dataRow["TabName"].ToString();
                picture.MediaURL = dataRow["MediaURL"].ToString();
                picture.MediaName = dataRow["MediaName"].ToString();
                

            }
            return picture;
        }

        public int UpdatePictures(MediaChurch picture)
        {
            _dc.ClearParameters();
            _dc.AddParameter("MediaId", picture.ChurchMediaId);
            _dc.AddParameter("MedTabName", picture.TabName);
            _dc.AddParameter("MedURL", picture.MediaURL);
            _dc.AddParameter("MedName", picture.MediaName);
            _dc.AddParameter("UserId", picture.UpdatedBy);

            return _dc.Execute("spChurchMedia_Update");
        }
        public bool DeletePictures(int camId)
        {
            _dc.ClearParameters();
            _dc.AddParameter("MediaId", camId);
            _dc.AddParameter("UserId",1);
            return _dc.ReturnBool("spChurchMedia_Delete");
        }
    }
}