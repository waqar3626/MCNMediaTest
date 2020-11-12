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

        public IEnumerable<DashBoardClient> GetDashboardClientInfo()
        {
            List<DashBoardClient> dashBoardClients = new List<DashBoardClient>();

            _dc.ClearParameters();
            _dc.AddParameter("ChrId", 522);
            _dc.AddParameter("CurrentDay", "1900-01-01");
            DataTable dataTable = _dc.ReturnDataTable("sp_ClientEventlist");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                DashBoardClient dashBoardClient = new DashBoardClient();

                dashBoardClient.ChurchId = Convert.ToInt32(dataRow["ChurchId"]);
                dashBoardClient.CameraId = Convert.ToInt32(dataRow["CameraId"]);
                dashBoardClient.ScheduleEventName = dataRow["ScheduleEventName"].ToString();
                dashBoardClient.ScheduleEventTime = dataRow["ScheduleEventTime"].ToString();
                dashBoardClient.ScheduleEventDate =Convert.ToDateTime( dataRow["ScheduleEventDate"]);
                dashBoardClient.ScheduleStatus = dataRow["ScheduleStatus"].ToString();
                dashBoardClient.IsRepeated = Convert.ToInt32(dataRow["IsRepeated"]);
                dashBoardClient.RecordingDuration = Convert.ToInt32(dataRow["RecordingDuration"]);
                dashBoardClients.Add(dashBoardClient);
            }
            return dashBoardClients;
        }

    }
}