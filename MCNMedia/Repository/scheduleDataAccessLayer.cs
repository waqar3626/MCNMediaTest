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
            _dc.AddParameter("ScheduleEventDay", Enum.Parse(typeof(EventDay), schedules.EventDay).ToString());
            _dc.AddParameter("ScheduleEventTime", schedules.EventTime);
            _dc.AddParameter("ChurchId", schedules.ChurchId);
            _dc.AddParameter("CreatedBy", 1);
            //_dc.AddParameter("RoleId", schedules.RoleId);
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
                //user.UpdatedBy = Convert.ToInt32(rdr["UpdatedBy"]);
                //schedule.RoleId = Convert.ToInt32(dataRow["RoleId"]);
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
            _dc.AddParameter("SchDay", Enum.Parse(typeof(EventDay), schedule.EventDay).ToString());
            _dc.AddParameter("SchTime", schedule.EventTime);
            _dc.AddParameter("UpdateBy", schedule.UpdatedBy);
            //_dc.AddParameter("RoleId", schedule.RoleId);
            _dc.Execute("spSchedule_Update");
        }

        //To Delete the record on a particular User 
        public void DeleteSchedule(int id)
        {
            _dc.ClearParameters();
            _dc.AddParameter("SchId", id);
            _dc.AddParameter("UpdatedBy", 1);
            _dc.ReturnBool("spSchedule_Delete");
        }

    }
}
