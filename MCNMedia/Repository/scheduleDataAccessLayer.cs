using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;

namespace MCNMedia_Dev.Repository
{
    public class scheduleDataAccessLayer
    {
        AwesomeDal.DatabaseConnect _dc;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public scheduleDataAccessLayer()
        {
            _dc = new AwesomeDal.DatabaseConnect();
        }
        //To Add new Schedule record    
        public void AddSchedule(Schedule schedules)
        {
            _dc.ClearParameters();
            _dc.AddParameter("ScheduleEventName", schedules.EventName);
            _dc.AddParameter("ScheduleEventDate", schedules.EventDate);
            _dc.AddParameter("ScheduleEventDay", schedules.EventDay);
            _dc.AddParameter("ScheduleEventTime", schedules.EventTime);
            _dc.AddParameter("ScheduleLength", schedules.RecordDuration);
            _dc.AddParameter("SchRecord", schedules.Record);
            _dc.AddParameter("ChurchId", schedules.ChurchId);
            _dc.AddParameter("IsRepeated", schedules.IsRepeated);
            _dc.AddParameter("SchPassword", schedules.Password);
            _dc.AddParameter("SchCameraId", schedules.CameraId); 
            _dc.AddParameter("CreatedBy", schedules.CreatedBy);
            _dc.Execute("spschedule_Add");
        }

		//To View all Users details 
        public IEnumerable<Schedule> GetAllSchedule()
        {
            List<Schedule> Balobj = new List<Schedule>();
            _dc.ClearParameters();
            _dc.AddParameter("SchId", -1);
            _dc.AddParameter("SchName", "");
            _dc.AddParameter("ChrId", -1);
            //_dc.AddParameter("EmailAdd", "");
            DataTable dataTable = _dc.ReturnDataTable("spSchedule_GetAll");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                try
                {
                    Schedule schedule = new Schedule();
                    schedule.ScheduleId = Convert.ToInt32(dataRow["ScheduleId"]);
                    schedule.ChurchName = dataRow["ChurchName"].ToString();
                    schedule.EventName = dataRow["ScheduleEventName"].ToString();
                    schedule.EventDate = Convert.ToDateTime(dataRow["ScheduleEventDate"].ToString());
                    schedule.EventDay = dataRow["ScheduleEventDay"].ToString();
                    schedule.EventTime = Convert.ToDateTime(dataRow["ScheduleEventTime"].ToString());
                    schedule.CreatedAt = Convert.ToDateTime(dataRow["CreatedAt"].ToString());
                    schedule.Password = dataRow["Password"].ToString();
                    schedule.IsRepeated = Convert.ToBoolean(dataRow["IsRepeated"]);
                    schedule.RecordDuration = Convert.ToInt32(dataRow["RecordingDuration"]);
                  
                    schedule.CameraId = Convert.ToInt32(dataRow["CameraId"]);
                    schedule.CameraName = dataRow["CameraName"].ToString();

                    Balobj.Add(schedule);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
            return Balobj;
        }



        public IEnumerable<Schedule> GetSearchSchedule(int ChurchId, DateTime EventDate,string EventDay,int isRecord)
        {
            List<Schedule> Balobj = new List<Schedule>();
            _dc.ClearParameters();
            _dc.AddParameter("ChrId", ChurchId);
            _dc.AddParameter("SchDate", EventDate.ToString("yyyy-MM-dd"));
            _dc.AddParameter("SchDay", EventDay);
            _dc.AddParameter("isRecord", isRecord);

            DataTable dataTable = _dc.ReturnDataTable("spSchedule_Search");
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
                schedule.Password = dataRow["Password"].ToString();
                schedule.IsRepeated = Convert.ToBoolean(dataRow["IsRepeated"]);
                schedule.RecordDuration = Convert.ToInt32(dataRow["RecordingDuration"]);
                
                schedule.CameraId = Convert.ToInt32(dataRow["CameraId"]);
                schedule.CameraName = dataRow["CameraName"].ToString();
                //user.UpdatedBy = Convert.ToInt32(rdr["UpdatedBy"]);
                //schedule.RoleName = dataRow["RoleName"].ToString();


                Balobj.Add(schedule);
            }
            return Balobj;
        }

        //Get the details of a particular User
        public Schedule GetScheduleDataBtId(int id)
        {
            Schedule schedule = new Schedule();

            _dc.ClearParameters();
            _dc.AddParameter("SchId", id);
            DataTable dataTable = _dc.ReturnDataTable("spSchedule_GetById");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                schedule.ScheduleId = Convert.ToInt32(dataRow["ScheduleId"]);
                schedule.ChurchId = Convert.ToInt32(dataRow["ChurchId"]);
                schedule.EventName = dataRow["ScheduleEventName"].ToString();
                schedule.EventDate = Convert.ToDateTime(dataRow["ScheduleEventDate"].ToString());
                schedule.EventDay = (dataRow["ScheduleEventDay"].ToString());
                schedule.EventTime = Convert.ToDateTime(dataRow["ScheduleEventTime"].ToString());
                schedule.RecordDuration = Convert.ToInt32(dataRow["RecordingDuration"]);
                schedule.Record = Convert.ToBoolean(dataRow["Record"]);
                schedule.Password = dataRow["Password"].ToString();
                schedule.IsRepeated = Convert.ToBoolean(dataRow["IsRepeated"]);
                schedule.CameraId = Convert.ToInt32(dataRow["CameraId"]);
               
            }
            return schedule;
        }

        //To Update the records of a particluar User
        public void UpdateSchedule(Schedule schedule)
        {
            _dc.ClearParameters();
            _dc.AddParameter("SchId", schedule.ScheduleId);
            _dc.AddParameter("SchName", schedule.EventName);
            _dc.AddParameter("SchDate", schedule.EventDate);
            _dc.AddParameter("SchDay", schedule.EventDay);
            _dc.AddParameter("SchTime", schedule.EventTime);
             _dc.AddParameter("recordingDuration", schedule.RecordDuration);
            _dc.AddParameter("record", schedule.Record);
            _dc.AddParameter("UpdateBy", schedule.UpdatedBy);
            _dc.AddParameter("SchPassword", schedule.Password);
            _dc.AddParameter("IsRepeated", schedule.IsRepeated);
            _dc.AddParameter("SchCameraId", schedule.CameraId);

            _dc.Execute("spSchedule_Update");
        }

        //To Delete the record on a particular User 
        public void DeleteSchedule(int id,int UpdateBy)
        {
            _dc.ClearParameters();
            _dc.AddParameter("SchId", id);
            _dc.AddParameter("UpdatedBy", UpdateBy);
            _dc.ReturnBool("spSchedule_Delete");
        }

        public IEnumerable<Schedule> GetWebsiteSchedule()
        {
            List<Schedule> Balobj = new List<Schedule>();
            _dc.ClearParameters();
   

            //_dc.AddParameter("EmailAdd", "");
            DataTable dataTable = _dc.ReturnDataTable("spWebsite_ScheduleList");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                Schedule schedule = new Schedule();
                schedule.ScheduleId = Convert.ToInt32(dataRow["ScheduleId"]);
                schedule.ChurchName = dataRow["ChurchName"].ToString();
                schedule.EventName = dataRow["ScheduleEventName"].ToString();
                schedule.EventDate = Convert.ToDateTime(dataRow["ScheduleEventDate"].ToString());
                schedule.EventDay = dataRow["ScheduleEventDay"].ToString();
                schedule.EventTime = Convert.ToDateTime(dataRow["ScheduleEventTime"].ToString());
                Balobj.Add(schedule);
            }
            return Balobj;
        }


    }
}
