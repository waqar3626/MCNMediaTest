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

        public CameraDataAccessLayer()
        {
            _dc = new AwesomeDal.DatabaseConnect();
        }

        public int AddCamera(Camera camera)
        {
            _dc.ClearParameters();
            _dc.AddParameter("CreatedBy", camera.CreatedBy);
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
            _dc.ReturnInt("spCamera_Add");
            return 1;
        }

        public IEnumerable<Camera> GetAllCameras(int ChurchId)
        {
            List<Camera> Balobj = new List<Camera>();
            DataTable dataTable = GetCamera(churchId: ChurchId, cameraId: -1);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                Camera camera = BindingCamera(dataRow);
                Balobj.Add(camera);
            }
            return Balobj;
        }

        public Camera GetCameraById(int camId)
        {
            Camera camera = new Camera();
            DataTable dataTable = GetCamera(churchId: -1, cameraId: camId);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                camera = BindingCamera(dataRow);
            }
            return camera;
        }

        private DataTable GetCamera(int churchId, int cameraId)
        {
            _dc.ClearParameters();
            _dc.AddParameter("chrchId", churchId);
            _dc.AddParameter("camId", cameraId);
            DataTable dataTable = _dc.ReturnDataTable("spCamera_Get");
            return dataTable;
        }

        private Camera BindingCamera(DataRow dataRow)
        {
            Camera camera = new Camera();
            camera.CameraId = Convert.ToInt32(dataRow["CameraId"]);
            camera.CameraName = dataRow["CameraName"].ToString();
            camera.HttpPort = dataRow["HttpPort"].ToString();
            camera.CameraUrl = dataRow["CameraUrl"].ToString();
            camera.RtspPort = dataRow["RtspPort"].ToString();
            camera.ChurchId = Convert.ToInt32(dataRow["ChurchId"].ToString());
            camera.ChurchName = dataRow["ChurchName"].ToString();
            camera.ServerId = Convert.ToInt32(dataRow["ServerId"]);
            camera.ServerName = dataRow["ServerName"].ToString();
            camera.ServerIP = dataRow["ServerIP"].ToString();
            camera.LiveStreamUrl = $"https://{dataRow["ServerIP"]}/live/_{dataRow["UniqueIdentifier"]}_/{dataRow["UniqueIdentifier"]}_{dataRow["CameraId"]}.stream/playlist.m3u8";
            camera.LiveStreamUrl = "https://1502594353.rsc.cdn77.org/live/_23b079cbd1f93615a4e57355415b9a67c1c5e9c8_/23b079cbd1f93615a4e57355415b9a67c1c5e9c8_4.stream/playlist.m3u8";
            return camera;
        }

        public int Updatecamera(Camera camera)
        {
            _dc.ClearParameters();
            _dc.AddParameter("CamId", camera.CameraId);
            _dc.AddParameter("CamName", camera.CameraName);
            _dc.AddParameter("CamRtsp", camera.RtspPort);
            _dc.AddParameter("CamHttp", camera.HttpPort);
            _dc.AddParameter("UpdateBy", camera.UpdatedBy);

            return _dc.Execute("spcamera_Update");
        }

        public bool DeleteCamera(int camId)
        {
            _dc.ClearParameters();
            _dc.AddParameter("CamId", camId);
            _dc.AddParameter("UpdatedBy", 1);
            return _dc.ReturnBool("spCamera_Delete");
        }

        public IEnumerable<Server> GetServer()
        {
            List<Server> Balobj = new List<Server>();
            _dc.ClearParameters();
            DataTable dataTable = _dc.ReturnDataTable("spServer_Get");

            foreach (DataRow dataRow in dataTable.Rows)
            {
                Server server = new Server();
                server.ServerId = Convert.ToInt32(dataRow["ServerId"]);
                server.ServerName = dataRow["ServerName"].ToString();
                server.ServerIP = dataRow["ServerIP"].ToString();

                Balobj.Add(server);
            }
            return Balobj;
        }

    }



}