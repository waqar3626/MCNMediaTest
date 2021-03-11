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
            _dc.AddParameter("CameraType", camera.CameraType.ToString());
            _dc.AddParameter("StreamingProtocol", camera.StreamingProtocol);
            _dc.AddParameter("ChurchId", camera.ChurchId);
            _dc.AddParameter("ServerId", camera.ServerId);
            _dc.AddParameter("App", camera.App);


            return _dc.ReturnInt("spCamera_Add");
        }

        public List<Camera> GetAllCameras()
        {
            List<Camera> Balobj = new List<Camera>();
            DataTable dataTable = GetCamera(churchId: -1, cameraId: -1, camType: _Helper.CameraType.All);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                Camera camera = BindingCamera(dataRow);
                Balobj.Add(camera);
            }
            return Balobj;
        }

        public List<Camera> GetAllCamerasByChurch(int churchId)
        {
            List<Camera> Balobj = new List<Camera>();
            DataTable dataTable = GetCamera(churchId: churchId, cameraId: -1, camType: _Helper.CameraType.All);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                Camera camera = BindingCamera(dataRow);
                Balobj.Add(camera);
            }
            return Balobj;
        }

        public List<Camera> GetAllAdminCameras()
        {
            List<Camera> Balobj = new List<Camera>();
            DataTable dataTable = GetCamera(churchId: -1, cameraId: -1, camType: _Helper.CameraType.AdminCamera);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                Camera camera = BindingCamera(dataRow);
                Balobj.Add(camera);
            }
            return Balobj;
        }

        public List<Camera> GetAdminCameraByChurch(int churchId)
        {
            ScheduleDataAccessLayer scheduleDataAccessLayer = new ScheduleDataAccessLayer();
            List<Camera> Balobj = new List<Camera>();
            DataTable dataTable = GetCamera(churchId: churchId, cameraId: -1, camType: _Helper.CameraType.AdminCamera);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                Camera camera = BindingCamera(dataRow);
                Balobj.Add(camera);
            }
            return Balobj;
        }

        public List<Camera> GetMobileCameraByChurch(int churchId)
        {
            List<Camera> Balobj = new List<Camera>();
            DataTable dataTable = GetCamera(churchId: churchId, cameraId: -1, camType: _Helper.CameraType.ClientCamera);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                Camera camera = BindingCamera(dataRow);
                Balobj.Add(camera);
            }
            return Balobj;
        }

        public List<Camera> GetActiveCameraByChurch(int churchId)
        {
            List<Camera> camList = new List<Camera>();
            DataTable dataTable = GetCamera(churchId: churchId, cameraId: -1, camType: _Helper.CameraType.All);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (Convert.ToBoolean(dataRow["IsCameraLive"]))
                {
                    Camera camera = BindingCamera(dataRow);
                    camList.Add(camera);
                }
            }
            return camList;
        }

        public List<Camera> GetCameraByCounty(string countySlug)
        {
            _dc.ClearParameters();
            _dc.AddParameter("CountySlug", countySlug);
            DataTable dataTable = _dc.ReturnDataTable("spCamera_GetByCounty");

            List<Camera> camList = new List<Camera>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (Convert.ToBoolean(dataRow["IsCameraLive"]))
                {
                    Camera camera = BindingCamera(dataRow);
                    camList.Add(camera);
                }
            }
            return camList;
        }

        public Camera GetCameraById(int camId)
        {
            Camera camera = new Camera();
            DataTable dataTable = GetCamera(churchId: -1, cameraId: camId, camType: _Helper.CameraType.All);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                camera = BindingCamera(dataRow);
            }
            return camera;
        }

        private DataTable GetCamera(int churchId, int cameraId, _Helper.CameraType camType)
        {
            _dc.ClearParameters();
            _dc.AddParameter("chrchId", churchId);
            _dc.AddParameter("camId", cameraId);
            _dc.AddParameter("camtype", camType.ToString());
            DataTable dataTable = _dc.ReturnDataTable("spCamera_Get");
            return dataTable;
        }

        private Camera BindingCamera(DataRow dataRow)
        {
            Camera camera = new Camera();
            camera.CameraId = Convert.ToInt32(dataRow["CameraId"]);
            camera.CameraName = dataRow["CameraName"].ToString();
            camera.CameraType = (_Helper.CameraType)Enum.Parse(typeof(_Helper.CameraType), dataRow["CameraType"].ToString());
            camera.HttpPort = dataRow["HttpPort"].ToString();
            camera.CameraUrl = dataRow["CameraUrl"].ToString();
            camera.RtspPort = dataRow["RtspPort"].ToString();
            camera.ChurchId = Convert.ToInt32(dataRow["ChurchId"].ToString());
            camera.ChurchName = dataRow["ChurchName"].ToString();
            camera.ServerId = Convert.ToInt32(dataRow["ServerId"]);
            camera.ServerName = dataRow["ServerName"].ToString();
            camera.ServerIP = dataRow["ServerIP"].ToString();
            camera.IsCameraLive = Convert.ToBoolean(dataRow["IsCameraLive"]);
            camera.IsCameraStreaming = Convert.ToBoolean(dataRow["IsCameraStreaming"]);
            if (camera.CameraType == _Helper.CameraType.AdminCamera)
            {
                camera.LiveStreamUrl = $"https://{dataRow["ServerURL"]}/live/{dataRow["UniqueIdentifier"]}_{dataRow["CameraId"]}.stream/playlist.m3u8";
            }
            else if (camera.CameraType == _Helper.CameraType.ClientCamera)
            {
                string cameraUniqueIdentifier = dataRow["CameraUrl"].ToString().Split("_")[0];
                camera.LiveStreamUrl = $"https://{dataRow["ServerURL"]}/live/_{cameraUniqueIdentifier}_/{dataRow["CameraUrl"]}.stream/playlist.m3u8";
            }
            camera.StreamingProtocol = dataRow["StreamingProtocol"].ToString().Trim();
            camera.ChurchUniqueIdentifier = dataRow["UniqueIdentifier"].ToString().Trim();
            //camera.LiveStreamUrl = "https://1502594353.rsc.cdn77.org/live/_23b079cbd1f93615a4e57355415b9a67c1c5e9c8_/23b079cbd1f93615a4e57355415b9a67c1c5e9c8_4.stream/playlist.m3u8";
            return camera;
        }

        public int UpdateCamera(Camera camera)
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

        public int UpdateCameraStatus(int cameraId, bool cameraStatus, int UpdatedBy)
        {
            _dc.ClearParameters();
            _dc.AddParameter("CamId", cameraId);
            _dc.AddParameter("cameraStatus", cameraStatus);
            _dc.AddParameter("UpdateBy", UpdatedBy);

            return _dc.Execute("spCamera_UpdateStatus");
        }
        public int UpdateMobileCameraStatus(int cameraId, bool cameraStatus, int UpdatedBy)
        {
            _dc.ClearParameters();
            _dc.AddParameter("CamId", cameraId);
            _dc.AddParameter("cameraStatus", cameraStatus);
            _dc.AddParameter("UpdateBy", UpdatedBy);

            return _dc.Execute("spMobileCamera_UpdateStatus");
        }

        public int UpdateCameraStreamingStatus(int cameraId, bool isStreaming)
        {
            _dc.ClearParameters();
            _dc.AddParameter("CamId", cameraId);
            _dc.AddParameter("isStreaming", isStreaming);
            return _dc.Execute("spCamera_StreamingStatus_Update");
        }

        public int AddMobileCamera(string CameraName, int ChurchId, int UserId)
        {
            _dc.ClearParameters();
            _dc.AddParameter("cameraName", CameraName);
            _dc.AddParameter("churchId", ChurchId);
            _dc.AddParameter("userId", UserId);
            return _dc.ReturnInt("spClientMobileCamera_Add");
        }

        #region "Facebook Section"
        public int SaveUserInfo(int churchId, string UserAccessToken, string TokenExpiry, string LiveStatus)
        {
            //Save churchId as UserId in FB details
            _dc.ClearParameters();
            _dc.AddParameter("UsrId", churchId);
            _dc.AddParameter("Access_Token", UserAccessToken);
            _dc.AddParameter("Token_Expiry", TokenExpiry);
            _dc.AddParameter("Live_Status", LiveStatus);

            return _dc.ReturnInt("spFbLogin_Insert");
        }

        public int SaveSettings(int UserId, string PageAccessToken, string PageId, string PageName, string Message, string CamId)
        {
            // churchId as UserId in FB details
            _dc.ClearParameters();
            _dc.AddParameter("Usr_Id", UserId);
            _dc.AddParameter("Page_Access_Token", PageAccessToken);
            _dc.AddParameter("Page_Id", PageId);
            _dc.AddParameter("Page_Name", PageName);
            _dc.AddParameter("Message", Message);
            _dc.AddParameter("Camera_Id", CamId);

            return _dc.ReturnInt("spFblogin_Update");
        }

        public FBLoginDetails GetFbDetailsByUser(int UserId)
        {
            // churchId as UserId in FB details

            _dc.ClearParameters();
            _dc.AddParameter("User_Id", UserId);
            DataRow dataInfo = _dc.ReturnDataRow("sp_fblogin_GetByUser");

            FBLoginDetails fbInfo = new FBLoginDetails();
            fbInfo.FbLoginId = dataInfo["FbLoginId"].ToString();
            fbInfo.UserId = dataInfo["UserId"].ToString();
            fbInfo.Page_Access_Token = dataInfo["Page_Access_Token"].ToString();
            fbInfo.Token_Expiry = dataInfo["Token_Expiry"].ToString();
            fbInfo.Page_Id = dataInfo["Page_Id"].ToString();
            fbInfo.Page_Name = dataInfo["Page_Name"].ToString();

            fbInfo.Message = dataInfo["Message"].ToString();
            fbInfo.Live_Status = dataInfo["Live_Status"].ToString();
            fbInfo.Camera_Id = dataInfo["Camera_Id"].ToString();
            fbInfo.Access_Token = dataInfo["Access_Token"].ToString();

            return fbInfo;

        }
        #endregion
    }



}