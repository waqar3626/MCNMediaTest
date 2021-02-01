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

        #region "Stop Recording"

        public void StopRecording_ReadyToStop()
        {
            DataTable dt = scheduleDataAccess.GetScheduleReadyToStop();
            foreach (DataRow dr in dt.Rows)
            {
                StopRecording(Convert.ToInt32(dr["CameraId"]), Convert.ToInt32(dr["ChurchId"]), Convert.ToInt32(dr["ScheduleId"]));
            }
        }

        //public bool StopRecordingByCamera(int cameraId)
        //{

        //}

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
                    StartRecording(Convert.ToInt32(dr["CameraId"]), Convert.ToInt32(dr["ChurchId"]), Convert.ToInt32(dr["ScheduleId"]));
                }
            }
        }

        public void StartRecordingByCamera(int cameraId)
        {

        }

        public void StartRecordingBySchedule(int scheduleId)
        {

        }

        private void StartRecording(int cameraId, int churchId, int scheduleId)
        {
            WowzaApi.WowzaHelper wowzaHelper = new WowzaApi.WowzaHelper();
            bool res = wowzaHelper.StartRecording(churchId, cameraId);
            if (res)
            {
                scheduleDataAccess.UpdateScheduleStatus(scheduleId: scheduleId, scheduleStatus: 1);
                string logMessage = $"Recording (ScheduleId: {scheduleId}) started for camera (CameraID: {cameraId}) on {DateTime.Now}";
                ActivityLogDataAccessLayer.AddActivityLog(Operation.Recording_Started, Categories.Schedule, message: logMessage, churchId: churchId, userId: -1);
                scheduleDataAccess.InsertScheduleLog(scheduleId: scheduleId, scheduleStatus: 1);
            }
        }

        #endregion
    }
}
