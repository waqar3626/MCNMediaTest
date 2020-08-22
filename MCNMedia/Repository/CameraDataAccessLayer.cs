using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;

namespace MCNMedia_Dev.Repository
{
    public class CameraDataAccessLayer
    {
        AwesomeDal.DatabaseConnect _dc;
        private string v;

        public CameraDataAccessLayer()
        {
            _dc = new AwesomeDal.DatabaseConnect();
        }

        public CameraDataAccessLayer(string v)
        {
            this.v = v;
            _dc = new AwesomeDal.DatabaseConnect();
        }

        public IEnumerable<Camera> GetAllCameras()
        {
            List<Camera> Balobj = new List<Camera>();
            _dc.ClearParameters();
            _dc.AddParameter("CamName", "");
            DataTable dataTable = _dc.ReturnDataTable("spCamera_Search");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                Camera camera = new Camera();
                camera.CameraId = Convert.ToInt32(dataRow["CameraId"]);
                camera.CameraName = dataRow["CameraName"].ToString();
                camera.HttpPort = dataRow["HttpPort"].ToString(); 
                camera.CameraUrl = dataRow["CameraUrl"].ToString();
                camera.RtspPort = dataRow["RtspPort"].ToString();
                camera.ChurchId = Convert.ToInt32(dataRow["ChurchId"].ToString());
                camera.ChurchName = dataRow["ChurchName"].ToString();
                Balobj.Add(camera);
            }
            return Balobj;
        }

        public void AddCamera(Camera camera)
        {
            _dc.ClearParameters();
            _dc.AddParameter("CreatedBy", 1);
            _dc.AddParameter("CameraName", camera.CameraName);
            _dc.AddParameter("CameraUrl", camera.CameraUrl);
            _dc.AddParameter("RtspPort", camera.RtspPort);
            _dc.AddParameter("HttpPort", camera.HttpPort);
            _dc.AddParameter("CameraSlug", camera.CameraSlug);
            _dc.AddParameter("CameraType", camera.CameraType);
            _dc.AddParameter("StreamingProtocol", camera.StreamingProtocol);
            _dc.AddParameter("ChurchId", camera.ChurchId);
            _dc.AddParameter("ServerId", camera.ServerId);
            _dc.AddParameter("App", camera.App);
            _dc.Execute("spCamera_Add");
        }

        public Camera GetCameraById(int camId)
        {
            Camera camera = new Camera();

            _dc.ClearParameters();
            _dc.AddParameter("CamId", camId);
            DataTable dataTable = _dc.ReturnDataTable("spCamera_GetById");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                camera.CameraId = Convert.ToInt32(dataRow["CameraId"]);
                camera.CameraName = dataRow["CameraName"].ToString();
                camera.CameraUrl = dataRow["CameraUrl"].ToString();
                camera.HttpPort = dataRow["HttpPort"].ToString();
                camera.RtspPort = dataRow["RtspPort"].ToString();
                //camera.Town = dataRow["Town"].ToString();
                //camera.CountyId = Convert.ToInt32(dataRow["CountyId"]);
                //camera.Website = dataRow["Website"].ToString();
                //camera.EmailAddress = dataRow["EmailAddress"].ToString();
                //camera.Phone = dataRow["Phone"].ToString();
                //camera.ImageURl = dataRow["ImageURL"].ToString();
                //camera.Blurb = dataRow["Blurb"].ToString();
                //camera.Slug = dataRow["Slug"].ToString();
                //camera.Notice = dataRow["Notice"].ToString();
                //camera.Featured = Convert.ToInt32(dataRow["Featured"]);
                //camera.UniqueIdentifier = dataRow["UniqueIdentifier"].ToString();
                //camera.RepeatRecordings = Convert.ToBoolean(dataRow["RepeatRecordings"]);
                //camera.Switch = Convert.ToInt32(dataRow["Switch"]);
                //camera.ShowOnWebsite = Convert.ToBoolean(dataRow["ShowOnWebsite"]);
                //camera.DisplayOrder = Convert.ToInt32(dataRow["DisplayOrder"]);

            }
            return camera;
        }

        public int Updatecamera(Camera camera)
        {
            _dc.ClearParameters();
            _dc.AddParameter("CamId", camera.CameraId);
            _dc.AddParameter("CamName", camera.CameraName);
            _dc.AddParameter("CamRtsp", camera.RtspPort);
            _dc.AddParameter("CamHttp", camera.HttpPort);
            _dc.AddParameter("UpdateBy", 1);

            return _dc.Execute("spcamera_Update");
        }
        public bool DeleteCamera(int camId)
        {
            _dc.ClearParameters();
            _dc.AddParameter("CamId", camId);
            _dc.AddParameter("UpdatedBy", 1);
            return _dc.ReturnBool("spCamera_Delete");
        }
    }

    
}
