using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev._Helper
{
    public class Wowza
    {
        ScheduleDataAccessLayer scheduleDataAccess = new ScheduleDataAccessLayer();
        CameraDataAccessLayer camDataAccess = new CameraDataAccessLayer();

        #region "Stop Recording"

        public void StopRecording_ReadyToStop()
        {
            DataTable dt = scheduleDataAccess.GetScheduleReadyToStop();
            foreach (DataRow dr in dt.Rows)
            {
               StopRecording(Convert.ToInt32(dr["CameraId"]), Convert.ToInt32(dr["ChurchId"]), Convert.ToInt32(dr["ScheduleId"]));
              //  StopRecordingNew(Convert.ToInt32(dr["CameraId"]), Convert.ToInt32(dr["ChurchId"]), Convert.ToInt32(dr["ScheduleId"]), dr["UniqueIdentifier"].ToString()); 

            }
        }

        public bool StopRecordingBySchedule(int scheduleId)
        {
            Schedule schedule;
            schedule = scheduleDataAccess.GetScheduleById(scheduleId);
            return StopRecording(schedule.CameraId, schedule.ChurchId, schedule.ScheduleId);
        }

        private bool StopRecording(int cameraId, int churchId, int scheduleId)
        {
            _Helper.Common.SaveToXXX($"Stop Recording (ScheduleId: {scheduleId}) on Camera: {cameraId}");
            WowzaApi.WowzaHelper wowzaHelper = new WowzaApi.WowzaHelper();

            bool res = wowzaHelper.StopRecording(churchId, cameraId);
            if (res)
            {
                scheduleDataAccess.UpdateScheduleStatus(scheduleId: scheduleId, scheduleStatus: 2);
                string logMessage = $"Recording (ScheduleId: {scheduleId}) stopped for camera (CameraID: {cameraId}) on {DateTime.Now}";
                ActivityLogDataAccessLayer.AddActivityLog(Operation.Recording_Stopped, Categories.Schedule, message: logMessage, churchId: churchId, userId: -1);
                scheduleDataAccess.UpdateScheduleLog(scheduleId: scheduleId, scheduleStatus: 2);
            }
            return res;
        }

        private bool StopRecordingNew(int cameraId, int churchId, int scheduleId,string UniqueIdentifier)
        {
            _Helper.Common.SaveToXXX($"Stop Recording (ScheduleId: {scheduleId}) on Camera: {cameraId}");
            WowzaApi.WowzaHelper2 wowzaHelper = new WowzaApi.WowzaHelper2();

            bool res = wowzaHelper.StopRecording(churchId, cameraId, UniqueIdentifier);
            if (res)
            {
                scheduleDataAccess.UpdateScheduleStatus(scheduleId: scheduleId, scheduleStatus: 2);
                scheduleDataAccess.UpdateScheduleLog(scheduleId: scheduleId, scheduleStatus: 2);
                string logMessage = $"Recording (ScheduleId: {scheduleId}) stopped for camera (CameraID: {cameraId}) on {DateTime.Now}";
                ActivityLogDataAccessLayer.AddActivityLog(Operation.Recording_Stopped, Categories.Schedule, message: logMessage, churchId: churchId, userId: -1);
                
            }
            return res;
        }

        #endregion

        #region "Start Recording"

        public void StartRecording_ReadyToStart()
        {
            DataTable dt = scheduleDataAccess.GetScheduleReadyToStart();
            foreach (DataRow dr in dt.Rows)
            {
                _Helper.Common.SaveToXXX($"Start Recording Camera: {dr["CameraId"]} - ScheduleId: {dr["ScheduleId"]} - Cam Status: {dr["IsCameraLive"]} - Stream Status: {dr["IsCameraStreaming"]}");
                if (dr["CameraDeleted"].ToString() == "1")
                {
                    scheduleDataAccess.InsertScheduleLog(scheduleId: Convert.ToInt32(dr["ScheduleId"]), scheduleStatus: 6);
                }
                else if (dr["IsCameraLive"].ToString() == "0")
                {
                    scheduleDataAccess.InsertScheduleLog(scheduleId: Convert.ToInt32(dr["ScheduleId"]), scheduleStatus: 4);
                }
                else if (dr["IsCameraStreaming"].ToString() == "0")
                {
                    scheduleDataAccess.InsertScheduleLog(scheduleId: Convert.ToInt32(dr["ScheduleId"]), scheduleStatus: 5);
                }
                else
                {
                   // StartRecordingNew(Convert.ToInt32(dr["CameraId"]), Convert.ToInt32(dr["ChurchId"]), Convert.ToInt32(dr["ScheduleId"]), dr["UniqueIdentifier"].ToString(), dr["ScheduleEventName"].ToString());
                    StartRecording(Convert.ToInt32(dr["CameraId"]), Convert.ToInt32(dr["ChurchId"]), Convert.ToInt32(dr["ScheduleId"]));
                }
            }
        }

        public bool StartRecordingBySchedule(int scheduleId)
        {
            Schedule schedule;
            schedule = scheduleDataAccess.GetScheduleById(scheduleId);
            return StartRecording(schedule.CameraId, schedule.ChurchId, schedule.ScheduleId);
        }

        private bool StartRecording(int cameraId, int churchId, int scheduleId)
        {
            WowzaApi.WowzaHelper wowzaHelper = new WowzaApi.WowzaHelper();
            
            bool res = wowzaHelper.StartRecording(churchId, cameraId);
           
            if (res)
            {
                scheduleDataAccess.UpdateScheduleStatus(scheduleId: scheduleId, scheduleStatus: 1);
                scheduleDataAccess.InsertScheduleLog(scheduleId: scheduleId, scheduleStatus: 1);
                string logMessage = $"Recording (ScheduleId: {scheduleId}) started for camera (CameraID: {cameraId}) on {DateTime.Now}";
                ActivityLogDataAccessLayer.AddActivityLog(Operation.Recording_Started, Categories.Schedule, message: logMessage, churchId: churchId, userId: -1);
                
            }
            return res;
        }

        private bool StartRecordingNew(int cameraId, int churchId, int scheduleId,string UniqueIdentifier,string EventName)
        {
            WowzaApi.WowzaHelper2 wowzaHelper = new WowzaApi.WowzaHelper2();

            bool res = wowzaHelper.StartRecording(churchId, cameraId,UniqueIdentifier,EventName,scheduleId);

            if (res)
            {
                scheduleDataAccess.UpdateScheduleStatus(scheduleId: scheduleId, scheduleStatus: 1);
                string logMessage = $"Recording (ScheduleId: {scheduleId}) started for camera (CameraID: {cameraId}) on {DateTime.Now}";
                ActivityLogDataAccessLayer.AddActivityLog(Operation.Recording_Started, Categories.Schedule, message: logMessage, churchId: churchId, userId: -1);
                scheduleDataAccess.InsertScheduleLog(scheduleId: scheduleId, scheduleStatus: 1);
            }
            return res;
        }

        #endregion

        #region "Sync Camera"

        /// <summary>
        /// Synchronizing camera status with wowza streaming engine of all installed cameras.
        /// 1. Fetch all admin cameras
        /// 2. Fetch status of camera stream from wowza
        /// 3. Update camera status in database
        /// </summary>
        public void SyncAllCamerasWithWowza()
        {
            CameraStream cameraStream = new CameraStream();
            List<Camera> cameraList = camDataAccess.GetAllCameras();
            foreach (Camera cam in cameraList)
            {
                SyncCameraWithWowza(cam);
            }
        }

        /// <summary>
        /// Synchronizing camera status with wowza streaming engine.
        /// </summary>
        /// <param name="cameraId">integer</param>
        /// <returns></returns>
        public bool SyncCamerasWithWowzaById(int cameraId)
        {
            Camera camera = new Camera();
            CameraDataAccessLayer cameraDataAccessLayer = new CameraDataAccessLayer();
            camera = cameraDataAccessLayer.GetCameraById(cameraId);
            return SyncCameraWithWowza(camera);
        }

        private bool SyncCameraWithWowza(Camera camera)
        {
            CameraStream cameraStream;
            WowzaApi.WowzaHelper wowzaHelper = new WowzaApi.WowzaHelper();
            cameraStream = wowzaHelper.RequestCameraStatus(camera.ChurchId, camera.CameraId);
            if (camera.IsCameraStreaming != cameraStream.isConnected)
            {
                camDataAccess.UpdateCameraStreamingStatus(camera.CameraId, cameraStream.isConnected);
            }
            return cameraStream.isConnected;
        }

        #endregion
    }
}
