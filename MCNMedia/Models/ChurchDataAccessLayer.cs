using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class ChurchDataAccessLayer
    {
        AwesomeDal.DatabaseConnect _dc;
        private string v;

        public ChurchDataAccessLayer()
        {
            _dc = new AwesomeDal.DatabaseConnect();

        }

        public ChurchDataAccessLayer(string v)
        {
            this.v = v;
            _dc = new AwesomeDal.DatabaseConnect();
        }

        //To View all Churches details 
        public IEnumerable<Church> GetAllChurch()
        {
            List<Church> Balobj = new List<Church>();
            _dc.ClearParameters();
            _dc.AddParameter("ChurchId", 1);
            _dc.AddParameter("ChurchName", "");
            DataTable dataTable = _dc.ReturnDataTable("spChurch_Search");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                Church church = new Church();
                church.ChurchId = Convert.ToInt32(dataRow["ChurchId"]);
                church.ChurchName = dataRow["ChurchName"].ToString();
                church.ClientTypeId = Convert.ToInt32(dataRow["ClientTypeId"]);
                church.UniqueChurchId = dataRow["UniqueChurchId"].ToString();
                church.Address = dataRow["Address"].ToString();
                church.Town = dataRow["Town"].ToString();
                church.CountyId = Convert.ToInt32(dataRow["CountyId"]);
                church.Website = dataRow["Website"].ToString();
                church.EmailAddress = dataRow["EmailAddress"].ToString();
                church.Phone = dataRow["Phone"].ToString();
                church.ImageURl = dataRow["ImageURL"].ToString();
                church.Blurb = dataRow["Blurb"].ToString();
                church.Slug = dataRow["Slug"].ToString();
                church.Notice = dataRow["Notice"].ToString();
                church.Featured = Convert.ToInt32(dataRow["Featured"]);
                church.UniqueIdentifier = dataRow["UniqueIdentifier"].ToString();
                church.RepeatRecordings = Convert.ToBoolean(dataRow["RepeatRecordings"]);
                church.Switch = Convert.ToInt32(dataRow["Switch"]);
                church.ShowOnWebsite = Convert.ToBoolean(dataRow["ShowOnWebsite"]);
                church.DisplayOrder = Convert.ToInt32(dataRow["DisplayOrder"]);


                Balobj.Add(church);
            }
            return Balobj;
        }

        //To Add new Church record    
        public void AddChurch(Church church)
        {
            _dc.ClearParameters();
            _dc.AddParameter("UserId", 1);
            _dc.AddParameter("ChurchName", church.ChurchName);
            _dc.AddParameter("ClientTypeId", church.ClientTypeId);
            _dc.AddParameter("UniqueChurchId", church.UniqueChurchId);
            _dc.AddParameter("Address", church.Address);
            _dc.AddParameter("Town", church.Town);
            _dc.AddParameter("CountyId", church.CountyId);
            _dc.AddParameter("Website", church.Website);
            _dc.AddParameter("EmailAddress", church.EmailAddress);
            _dc.AddParameter("Phone", church.Phone);
            _dc.AddParameter("ImageURL", church.ImageURl);
            _dc.AddParameter("Blurb", church.Blurb);
            _dc.AddParameter("Slug", church.Slug);
            _dc.AddParameter("Notice", church.Notice);
            _dc.AddParameter("Featured", church.Featured);
            _dc.AddParameter("UniqueIdentifier", church.UniqueIdentifier);
            _dc.AddParameter("RepeatRecordings", church.RepeatRecordings);
            _dc.AddParameter("Switch", church.Switch);
            _dc.AddParameter("ShowOnWebsite", church.ShowOnWebsite);
            _dc.AddParameter("DisplayOrder", church.DisplayOrder);
            _dc.Execute("spChurch_Add");
        }

        //To Update the records of a particluar Church
        public void UpdateChurch(Church church)
        {
            _dc.ClearParameters();
            _dc.AddParameter("ChurchId", church.ChurchId);
            _dc.AddParameter("UserId", 1);
            _dc.AddParameter("ChurchName", church.ChurchName);
            _dc.AddParameter("ClientTypeId", church.ClientTypeId);
            _dc.AddParameter("UniqueChurchId", church.UniqueChurchId);
            _dc.AddParameter("Address", church.Address);
            _dc.AddParameter("Town", church.Town);
            _dc.AddParameter("CountyId", church.CountyId);
            _dc.AddParameter("Website", church.Website);
            _dc.AddParameter("EmailAddress", church.EmailAddress);
            _dc.AddParameter("Phone", church.Phone);
            _dc.AddParameter("ImageURL", church.ImageURl);
            _dc.AddParameter("Blurb", church.Blurb);
            _dc.AddParameter("Slug", church.Slug);
            _dc.AddParameter("Notice", church.Notice);
            _dc.AddParameter("Featured", church.Featured);
            _dc.AddParameter("UniqueIdentifier", church.UniqueIdentifier);
            _dc.AddParameter("RepeatRecordings", church.RepeatRecordings);
            _dc.AddParameter("Switch", church.Switch);
            _dc.AddParameter("ShowOnWebsite", church.ShowOnWebsite);
            _dc.AddParameter("DisplayOrder", church.DisplayOrder);
            _dc.Execute("spChurch_Update");
        }

        //Get the details of a particular Church
        public Church GetChurchData(int id)
        {
            Church church = new Church();

            _dc.ClearParameters();
            _dc.AddParameter("ChurchId", id);
            DataTable dataTable = _dc.ReturnDataTable("spChurch_GetById");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                church.ChurchId = Convert.ToInt32(dataRow["ChurchId"]);
                church.ChurchName = dataRow["ChurchName"].ToString();
                church.ClientTypeId = Convert.ToInt32(dataRow["ClientTypeId"]);
                church.UniqueChurchId = dataRow["UniqueChurchId"].ToString();
                church.Address = dataRow["Address"].ToString();
                church.Town = dataRow["Town"].ToString();
                church.CountyId = Convert.ToInt32(dataRow["CountyId"]);
                church.Website = dataRow["Website"].ToString();
                church.EmailAddress = dataRow["EmailAddress"].ToString();
                church.Phone = dataRow["Phone"].ToString();
                church.ImageURl = dataRow["ImageURL"].ToString();
                church.Blurb = dataRow["Blurb"].ToString();
                church.Slug = dataRow["Slug"].ToString();
                church.Notice = dataRow["Notice"].ToString();
                church.Featured = Convert.ToInt32(dataRow["Featured"]);
                church.UniqueIdentifier = dataRow["UniqueIdentifier"].ToString();
                church.RepeatRecordings = Convert.ToBoolean(dataRow["RepeatRecordings"]);
                church.Switch = Convert.ToInt32(dataRow["Switch"]);
                church.ShowOnWebsite = Convert.ToBoolean(dataRow["ShowOnWebsite"]);
                church.DisplayOrder = Convert.ToInt32(dataRow["DisplayOrder"]);

            }
            return church;
        }

        public void DeleteChurch(int id)
        {
            _dc.ClearParameters();
            _dc.AddParameter("ChurchId", id);
            _dc.AddParameter("UserId", 1);
            _dc.ReturnBool("spChurch_Delete");
        }
    }
}
