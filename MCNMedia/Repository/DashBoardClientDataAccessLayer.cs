using MCNMedia_Dev.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Repository
{
    public class DashBoardClientDataAccessLayer
    {
        AwesomeDal.DatabaseConnect _dc;

        public DashBoardClientDataAccessLayer()
        {
            _dc = new AwesomeDal.DatabaseConnect();
        }
        public Dashboard GetDashboardClientInfoCount(int chrid)
        {
            Dashboard dashboard = new Dashboard();
            _dc.CloseAndDispose();
            _dc.ClearParameters();
            _dc.AddParameter("ChrId", chrid);
            DataTable dataTable = _dc.ReturnDataTable("spDashboard_CountData");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                dashboard.ChurchCount = Convert.ToInt32(dataRow["NumberOfChurches"]);
                dashboard.CathedralsCount = Convert.ToInt32(dataRow["Cathedrals"]);
                dashboard.FuneralsHomeCount = Convert.ToInt32(dataRow["FuneralHomes"]);
                dashboard.CountryCount = Convert.ToInt32(dataRow["NumberOfCountries"]);
                dashboard.CountyCount = Convert.ToInt32(dataRow["NumberOfCounties"]);
                dashboard.RepublicOfIrelandCount = Convert.ToInt32(dataRow["RepublicOfIreland"]);
                dashboard.NorthrenIrelandCount = Convert.ToInt32(dataRow["NorthernIreland"]);
                dashboard.EnglandCount = Convert.ToInt32(dataRow["England"]);
                dashboard.ScotlandCount = Convert.ToInt32(dataRow["Scotland"]);
                dashboard.USACount = Convert.ToInt32(dataRow["USA"]);
                dashboard.CanadaCount = Convert.ToInt32(dataRow["Canada"]);
                dashboard.ISEOfManCount = Convert.ToInt32(dataRow["IseOfMan"]);
                dashboard.WalesCount = Convert.ToInt32(dataRow["Wales"]);
                dashboard.UserCount = Convert.ToInt32(dataRow["NumberOfUsers"]);
                dashboard.AdminCount = Convert.ToInt32(dataRow["Admins"]);
                dashboard.ClientCount = Convert.ToInt32(dataRow["Clients"]);
                dashboard.ScheduleCount = Convert.ToInt32(dataRow["Schedules"]);
                dashboard.TodayScheduleCount = Convert.ToInt32(dataRow["TodaysSchedules"]);
                dashboard.RecordingCount = Convert.ToInt32(dataRow["TotalRecordings"]);
                dashboard.TodayRecording = Convert.ToInt32(dataRow["TodaysRecordings"]);
                dashboard.CameraCount = Convert.ToInt32(dataRow["TotalCameras"]);
            }
            return dashboard;
        }

        public IEnumerable<DashBoardClient> GetDashboardClientInfo(int chrid)
        {
            List<DashBoardClient> dashBoardClients = new List<DashBoardClient>();


            DataTable dataTable = GetRecordReport(chrid: chrid,dateTime: DateTime.Now);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                DashBoardClient dashBoard = BindRecordedEvents(dataRow);
                dashBoardClients.Add(dashBoard);
            }
            return dashBoardClients;
        } 
       
        public IEnumerable<DashBoardClient> GetRecordingReport(int chrid,DateTime date)
        {
            List<DashBoardClient> dashBoardClients = new List<DashBoardClient>();


            DataTable dataTable = GetRecordReport(chrid: chrid,dateTime: date);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                DashBoardClient dashBoard = BindRecordedEvents(dataRow);
                dashBoardClients.Add(dashBoard);
            }
            return dashBoardClients;
        } 
       
        
        public DashBoardClient GetCountClientDashBoard(int chrid)
        {
            _dc.CloseAndDispose();
            _dc.ClearParameters();
            _dc.AddParameter("ChrId", chrid);
            _dc.AddParameter("CurrentDay", "2020-11-13");
            DataTable dataTable = _dc.ReturnDataTable("spDashBoardClient_CountData");
            DashBoardClient dashBoardClient = new DashBoardClient();

            foreach (DataRow dataRow in dataTable.Rows)
            {

                dashBoardClient.NumberOfPictures = Convert.ToInt32(dataRow["NumberOfPictures"]);
                dashBoardClient.NumberOfVideo = Convert.ToInt32(dataRow["NumberOfVideo"]);
                dashBoardClient.NumberOfSlideShow = Convert.ToInt32(dataRow["NumberOfSlideShow"]);
                dashBoardClient.NumberOfCameras = Convert.ToInt32(dataRow["NumberOfCameras"]);
                dashBoardClient.TodaysSchedules = Convert.ToInt32(dataRow["TodaysSchedules"]);
                dashBoardClient.TotalRecordings = Convert.ToInt32(dataRow["TotalRecordings"]);
                dashBoardClient.TodaysRecordings = Convert.ToInt32(dataRow["TodaysRecordings"]);
                dashBoardClient.TodaysEventsWithOutRecordings = Convert.ToInt32(dataRow["TodaysEventsWithOutRecordings"]);
                dashBoardClient.TodayRecordingDone = Convert.ToInt32(dataRow["TodayRecordingDone"]);
            }
            return dashBoardClient;
        }
        public int GetUserAssignTopChurchId(int UsrId)
        {
            _dc.CloseAndDispose();
            _dc.ClearParameters();
            _dc.AddParameter("usrId", UsrId);
            int ChurchId = _dc.ReturnInt("spUserAssignChurch_GetTopChurchId");

            return ChurchId;
        }

        private DataTable GetRecordReport(int chrid,DateTime dateTime)
        {
            _dc.CloseAndDispose();
            _dc.ClearParameters();
            _dc.AddParameter("ChrId", chrid);
            _dc.AddParameter("CurrentDay", dateTime);
            DataTable dataTable = _dc.ReturnDataTable("sp_ClientEventlist");
            return dataTable;
        }

        private DashBoardClient BindRecordedEvents(DataRow dataRow)
        {
            DashBoardClient dashBoardClient = new DashBoardClient();
            dashBoardClient.ScheduleId = Convert.ToInt32(dataRow["ScheduleId"]);
            dashBoardClient.ChurchId = Convert.ToInt32(dataRow["ChurchId"]);
            dashBoardClient.ChurchName = dataRow["ChurchName"].ToString();
            dashBoardClient.CameraId = Convert.ToInt32(dataRow["CameraId"]);
            dashBoardClient.ScheduleEventName = dataRow["ScheduleEventName"].ToString();
            dashBoardClient.ScheduleEventTime = dataRow["ScheduleEventTime"].ToString();
            dashBoardClient.ScheduleEventDate = Convert.ToDateTime(dataRow["ScheduleEventDate"]);
            dashBoardClient.ScheduleStatus = dataRow["StatusTitle"].ToString();
            dashBoardClient.IsRepeated = Convert.ToInt32(dataRow["IsRepeated"]);
            dashBoardClient.RecordingDuration = Convert.ToInt32(dataRow["RecordingDuration"]);
            return dashBoardClient;
        }

    }
}