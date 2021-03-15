using MCNMedia_Dev.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Repository
{
    public class DashboardDataAccessLayer
    {
        AwesomeDal.DatabaseConnect _dc;

    public DashboardDataAccessLayer()
    {
        _dc = new AwesomeDal.DatabaseConnect();
    }

        public Dashboard GetDashboardInfo()
        {
            Dashboard dashboard = new Dashboard();

            _dc.ClearParameters();
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
                dashboard.CameraCount =Convert.ToInt32(dataRow["TotalCameras"]);
            }
            return dashboard;
        }

        

        public IEnumerable<Dashboard> GetDashboardCountry_Churches()
        {
            List<Dashboard> dashboards = new List<Dashboard>();
           

            _dc.ClearParameters();
            DataTable dataTable = _dc.ReturnDataTable("spDashboard_Country_Churches");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                Dashboard dashboard = new Dashboard();
                dashboard.CountryName = dataRow["CountryName"].ToString();
                dashboard.ChurchCount = Convert.ToInt32(dataRow["Churches"]);
                dashboard.CathedralsCount = Convert.ToInt32(dataRow["Cathedrals"]);
                dashboard.FuneralsHomeCount = Convert.ToInt32(dataRow["FuneralHomes"]);
                dashboards.Add(dashboard);


            }
            return dashboards;
        }

    }
}
