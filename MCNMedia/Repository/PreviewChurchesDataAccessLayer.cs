using MCNMedia_Dev.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Repository
{
    public class PreviewChurchesDataAccessLayer
    {
        AwesomeDal.DatabaseConnect _dc;
        public PreviewChurchesDataAccessLayer()
        {

            _dc = new AwesomeDal.DatabaseConnect();
        }
        public Church GetPreviewChurch(int ChId)
        {
           Church pchurches = new Church();
            _dc.ClearParameters();
            _dc.AddParameter("chId", ChId);

            DataTable dataTable = _dc.ReturnDataTable("spChurches_Preview");
            foreach (DataRow dataRow in dataTable.Rows)
            {

                pchurches.ChurchId = Convert.ToInt32(dataRow["ChurchId"]);
                pchurches.ChurchName = dataRow["ChurchName"].ToString();
                pchurches.Address = dataRow["Address"].ToString();
                pchurches.Town = dataRow["Town"].ToString();
                pchurches.Website = dataRow["Website"].ToString();
                pchurches.EmailAddress = dataRow["EmailAddress"].ToString();
                pchurches.Phone = dataRow["Phone"].ToString();
                pchurches.ImageURl = "/" + dataRow["ImageURL"].ToString();
                pchurches.Notice = dataRow["Notice"].ToString();
                pchurches.Featured = Convert.ToInt32(dataRow["Featured"]);
             
            }
            return pchurches;
        }
        public IEnumerable<Camera> GetAllPreviewCameras(int chId )
        {
            List<Camera> Balobj = new List<Camera>();
            _dc.ClearParameters();
            _dc.AddParameter("chId",chId );
            DataTable dataTable = _dc.ReturnDataTable("spCamera_PreviewChurches");
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

        public IEnumerable<Schedule> GetAllPreviewSchedule(int chId)
        {
            List<Schedule> Balobj = new List<Schedule>();
            _dc.ClearParameters();
      
            _dc.AddParameter("ChrId", chId);
            //_dc.AddParameter("EmailAdd", "");
            DataTable dataTable = _dc.ReturnDataTable("spSchedule_PreviewChurches");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                Schedule schedule = new Schedule();
                schedule.ScheduleId = Convert.ToInt32(dataRow["ScheduleId"]);
                schedule.ChurchName = dataRow["ChurchName"].ToString();
                schedule.EventName = dataRow["ScheduleEventName"].ToString();
                schedule.EventDate = Convert.ToDateTime(dataRow["ScheduleEventDate"].ToString());
                schedule.EventDay = dataRow["ScheduleEventDay"].ToString();
                schedule.EventTime = Convert.ToDateTime(dataRow["ScheduleEventTime"].ToString());
                schedule.CreatedAt = Convert.ToDateTime(dataRow["CreatedAt"].ToString());
                //user.UpdatedBy = Convert.ToInt32(rdr["UpdatedBy"]);
                //schedule.RoleName = dataRow["RoleName"].ToString();


                Balobj.Add(schedule);
            }
            return Balobj;
        }

        public IEnumerable<Recording> GetAllPreviewRecording(int chId)
        {
            List<Recording> Balobj = new List<Recording>();
            _dc.ClearParameters();
            _dc.AddParameter("chId", chId);
           
            DataTable dataTable = _dc.ReturnDataTable("spRecording_PreviewChurches");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                Recording recording = new Recording();
                recording.RecordingId = Convert.ToInt32(dataRow["RecordingId"]);
                recording.RecordingTitle = dataRow["RecordingName"].ToString();
                recording.RecordingURl = dataRow["RecordingURL"].ToString();
                recording.Date = Convert.ToDateTime(dataRow["RecordingDate"].ToString());
                recording.Time = Convert.ToDateTime(dataRow["RecordingTime"].ToString());
                recording.ChurchId = Convert.ToInt32(dataRow["ChurchId"]);
                recording.ChurchName = dataRow["ChurchName"].ToString();


                Balobj.Add(recording);
            }
            return Balobj;
        }

    }
}
