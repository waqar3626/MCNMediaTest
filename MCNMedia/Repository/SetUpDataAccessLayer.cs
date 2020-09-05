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
            _dc.AddParameter("UserId", 1);
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

        //public Church GetChurchData(int id)
        //{
        //    Church church = new Church();

        //    _dc.ClearParameters();
        //    _dc.AddParameter("ChurchId", id);
        //    DataTable dataTable = _dc.ReturnDataTable("spChurch_GetById");
        //    foreach (DataRow dataRow in dataTable.Rows)
        //    {
        //        church.ChurchId = Convert.ToInt32(dataRow["ChurchId"]);
        //        church.ChurchName = dataRow["ChurchName"].ToString();
        //        church.ClientTypeId = Convert.ToInt32(dataRow["ClientTypeId"]);
        //        church.UniqueChurchId = dataRow["UniqueChurchId"].ToString();
        //        church.Address = dataRow["Address"].ToString();
        //        church.Town = dataRow["Town"].ToString();
        //        church.CountyId = Convert.ToInt32(dataRow["CountyId"]);
        //        church.Website = dataRow["Website"].ToString();
        //        church.EmailAddress = dataRow["EmailAddress"].ToString();
        //        church.Phone = dataRow["Phone"].ToString();
        //        church.ImageURl = dataRow["ImageURL"].ToString();
        //        church.Blurb = dataRow["Blurb"].ToString();
        //        church.Slug = dataRow["Slug"].ToString();
        //        church.Notice = dataRow["Notice"].ToString();
        //        church.Featured = Convert.ToInt32(dataRow["Featured"]);
        //        church.UniqueIdentifier = dataRow["UniqueIdentifier"].ToString();
        //        church.RepeatRecordings = Convert.ToBoolean(dataRow["RepeatRecordings"]);
        //        church.Switch = Convert.ToInt32(dataRow["Switch"]);
        //        church.ShowOnWebsite = Convert.ToBoolean(dataRow["ShowOnWebsite"]);
        //        church.DisplayOrder = Convert.ToInt32(dataRow["DisplayOrder"]);

        //    }
        //    return church;
        //}

    }
}
