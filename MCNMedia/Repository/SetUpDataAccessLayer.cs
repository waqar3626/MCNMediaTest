using MCNMedia_Dev.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Repository
{
    public class SetUpDataAccessLayer
    {

        AwesomeDal.DatabaseConnect _dc;

        public SetUpDataAccessLayer()
        {
            _dc = new AwesomeDal.DatabaseConnect();
        }

        public void InsertSetUp(SetUp setup)
        {
            _dc.ClearParameters();
            _dc.AddParameter("UserId", setup.UpdatedBy);
            _dc.AddParameter("SetUpSelectServer", setup.SelectServer);
            _dc.AddParameter("SetUpInternetProvider", setup.InternetProvider);
            _dc.AddParameter("SetUpRouterModel", setup.RouterModel);
            _dc.AddParameter("SetUpRouterUserName", setup.RouterUserName);
            _dc.AddParameter("SetUpRouterPassword", setup.RouterPassword);
            _dc.AddParameter("ChurchId2", setup.ChurchId);
            _dc.AddParameter("SetupAerials", setup.Aerials);
            _dc.AddParameter("SetUpSwitch", setup.Switch);
            _dc.AddParameter("SetUpHomePlugs", setup.HomePlugs);
            _dc.AddParameter("SetUpDirectWire", setup.DirectWire);
            _dc.Execute("spChurchSetUp_Insertion_And_Updation");

        }
        
        public SetUp getSetUp(int id)
        {
            SetUp setup = new SetUp();

            _dc.ClearParameters();
            _dc.AddParameter("ChSetUpId", id);
            DataTable dataTable = _dc.ReturnDataTable("spChurchSetup_GetById");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                setup.ChurchSetUpId = Convert.ToInt32(dataRow["ChurchSetUpId"]);
                setup.SelectServer = dataRow["SelectServer"].ToString();
                setup.InternetProvider = dataRow["InternetProvider"].ToString();
                setup.RouterModel = dataRow["RouterModel"].ToString();
                setup.RouterUserName = dataRow["RouterUserName"].ToString();
                setup.RouterPassword= dataRow["RouterPassword"].ToString();
                setup.Aerials = Convert.ToBoolean(dataRow["Aerials"]);
                setup.Switch = Convert.ToBoolean(dataRow["Switch"]);
                setup.HomePlugs = Convert.ToBoolean(dataRow["HomePlugs"]);
                setup.DirectWire = Convert.ToBoolean(dataRow["DirectWire"]);
                setup.ChurchId = Convert.ToInt32(dataRow["ChurchId"]);

            }
            return setup;
        }

        

    }
}
