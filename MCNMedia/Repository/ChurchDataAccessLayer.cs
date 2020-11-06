﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static MCNMedia_Dev.Models.Church;
using MCNMedia_Dev.Models;

namespace MCNMedia_Dev.Repository
{
    public class ChurchDataAccessLayer
    {
        AwesomeDal.DatabaseConnect _dc;

        public ChurchDataAccessLayer()
        {
            _dc = new AwesomeDal.DatabaseConnect();

        }

        //To View all Churches details 
        public IEnumerable<Church> GetAllChurch(Church chr)
        {
            List<Church> Balobj = new List<Church>();
            _dc.ClearParameters();
            _dc.AddParameter("ChurchId", chr.ChurchId);
            _dc.AddParameter("ChurchName", chr.ChurchName);
            _dc.AddParameter("ClientType", chr.ClientTypeId);
            _dc.AddParameter("Country", chr.CountyId);
            _dc.AddParameter("EmailAdd", chr.EmailAddress);
            _dc.AddParameter("PhoneNo", chr.Phone);


            DataTable dataTable = _dc.ReturnDataTable("spChurch_Search");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                Church church = new Church();
                church.ChurchId = Convert.ToInt32(dataRow["ChurchId"]);
                church.ChurchName = dataRow["ChurchName"].ToString();
                church.ClientTypeTitle = dataRow["ClientTypeTitle"].ToString();
                church.UniqueChurchId = dataRow["UniqueChurchId"].ToString();
                church.Address = dataRow["Address"].ToString();
                church.Town = dataRow["Town"].ToString();
                church.CountyName = dataRow["CountyName"].ToString();
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
                church.Password= dataRow["Password"].ToString();

                Balobj.Add(church);
            }
            return Balobj;
        }

        public IEnumerable<ClientType> GetClients()
        {
            List<ClientType> Balobj = new List<ClientType>();
            _dc.ClearParameters();
            DataTable dataTable = _dc.ReturnDataTable("spClientTypes_Get");

            foreach (DataRow dataRow in dataTable.Rows)
            {
                ClientType client = new ClientType();
                client.ClientTypeId = Convert.ToInt32(dataRow["ClientTypeId"]);
                client.ClientTypeTitle = dataRow["ClientTypeTitle"].ToString();
                Balobj.Add(client);
            }
            return Balobj;
        }

        public IEnumerable<Counties> GetCounties(int country)
        {
            List<Counties> Balobj = new List<Counties>();
            _dc.ClearParameters();
            _dc.AddParameter("cntryId", country);
            DataTable dataTable = _dc.ReturnDataTable("spCounties_Get");

            foreach (DataRow dataRow in dataTable.Rows)
            {
                Counties county = new Counties();
                county.CountyId = Convert.ToInt32(dataRow["CountyId"]);
                county.CountyName = dataRow["CountyName"].ToString();
                Balobj.Add(county);
            }
            return Balobj;
        }


        public IEnumerable<Church> GetCountries()
        {
            List<Church> Balobj = new List<Church>();
            _dc.ClearParameters();
            DataTable dataTable = _dc.ReturnDataTable("spCountries_Get");

            foreach (DataRow dataRow in dataTable.Rows)
            {
                Church country = new Church();
                country.CountryId = Convert.ToInt32(dataRow["CountryId"]);
                country.CountryName = dataRow["CountryName"].ToString();
                Balobj.Add(country);
            }
            return Balobj;
        }
        //To Add new Church record    
        public void AddChurch(Church church)
        {
            _dc.ClearParameters();
            _dc.AddParameter("UserId", church.CreateBy);
            _dc.AddParameter("ChurchName", church.ChurchName);
            _dc.AddParameter("ClientTypeId", church.ClientTypeId);
            _dc.AddParameter("UniqueChurchId", Guid.NewGuid().ToString());
            _dc.AddParameter("Address", church.Address);
            _dc.AddParameter("Town", church.Town);
            _dc.AddParameter("CountyId", church.CountyId);
            _dc.AddParameter("Website", church.Website);
            _dc.AddParameter("EmailAddress", church.EmailAddress);
            _dc.AddParameter("Phone", church.Phone);
            _dc.AddParameter("ImageURL", church.ImageURl);
            _dc.AddParameter("Blurb", church.Blurb);
            _dc.AddParameter("Slug", Guid.NewGuid().ToString());
            _dc.AddParameter("Notice", church.Notice);
            _dc.AddParameter("Featured", church.Featured);
            _dc.AddParameter("UniqueIdentifier", Guid.NewGuid().ToString());
            _dc.AddParameter("RepeatRecordings", church.RepeatRecordings);
            _dc.AddParameter("Switch", church.Switch);
            _dc.AddParameter("ShowOnWebsite", church.ShowOnWebsite);
            _dc.AddParameter("DisplayOrder", church.DisplayOrder);
            _dc.AddParameter("ChurchPassword", church.Password);
            _dc.Execute("spChurch_Add");
        }

        //To Update the records of a particluar Church
        public void UpdateChurch(Church church)
        {
            _dc.ClearParameters();
            _dc.AddParameter("ChurchID1", church.ChurchId);
            _dc.AddParameter("UserId", church.UpdateBy);
            _dc.AddParameter("ChurchName", church.ChurchName);
            _dc.AddParameter("ClientTypeId", church.ClientTypeId);
            _dc.AddParameter("Address", church.Address);
            _dc.AddParameter("Town", church.Town);
            _dc.AddParameter("CountyId", church.CountyId);
            _dc.AddParameter("Website", church.Website);
            _dc.AddParameter("EmailAddress", church.EmailAddress);
            _dc.AddParameter("Phone", church.Phone);
            _dc.AddParameter("ImageURL", church.ImageURl);
            _dc.AddParameter("ShowOnWebsite", church.ShowOnWebsite);
            _dc.AddParameter("DisplayOrder", church.DisplayOrder);
            _dc.AddParameter("ChurchPassword", church.Password);
            _dc.Execute("spChurch_Update");
        }

        public void DeleteChurch(int id, int UpdateBy)
        {
            _dc.ClearParameters();
            _dc.AddParameter("ChrId", id);
            _dc.AddParameter("UserId", UpdateBy);
            _dc.Execute("spChurch_Delete");
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
                //string res = dataRow["ImageURL"].ToString();
                church.ImageURl  = ("../" + dataRow["ImageURL"].ToString()).Replace("//", "/");
                church.Blurb = dataRow["Blurb"].ToString();
                church.Slug = dataRow["Slug"].ToString();
                church.Notice = dataRow["Notice"].ToString();
                church.Password = dataRow["Password"].ToString();
                church.Featured = Convert.ToInt32(dataRow["Featured"]);
                church.UniqueIdentifier = dataRow["UniqueIdentifier"].ToString();
                church.RepeatRecordings = Convert.ToBoolean(dataRow["RepeatRecordings"]);
                church.Switch = Convert.ToInt32(dataRow["Switch"]);
                church.ShowOnWebsite = Convert.ToBoolean(dataRow["ShowOnWebsite"]);
                church.DisplayOrder = Convert.ToInt32(dataRow["DisplayOrder"]);

                //string res2 = res.Substring(0, 1);
                //if (res2 != "/")
                //{
                //    church.ImageURl = "../" + res;
                //}
                //else
                //{
                //    church.ImageURl = ".." + res;
                //}

            }
            return church;
        }

       

        public DataTable GetChurchDDL()
        {
            _dc.ClearParameters();
            return _dc.ReturnDataTable("spchurches_Get");

        }

       

        public DataTable GetUserAssignChurchDDL(int id)
        {
            _dc.ClearParameters();
            _dc.AddParameter("usrId", id);
            return _dc.ReturnDataTable("spuserChurches_Get");

        }

        public DataTable GetCameraDDL(int id)
        {
            _dc.ClearParameters();
            _dc.AddParameter("chId", id);
            return _dc.ReturnDataTable("spCameraddl_Get");

        }


        public DataTable GetClientTypeList()
        {
            _dc.ClearParameters();
            return _dc.ReturnDataTable("spClientTypes_Get");

        }

        public DataTable GetCountyList(int country)
        {
            _dc.ClearParameters();
            _dc.AddParameter("cntryId", country);
            return  _dc.ReturnDataTable("spCounties_Get");
           

        }
        public DataTable GetCountriesList()
        {
            _dc.ClearParameters();
            return _dc.ReturnDataTable("spCountries_Get");

        }

        public IEnumerable<Church> GetWebsiteChurch()
        {
            List<Church> Balobj = new List<Church>();
       


            DataTable dataTable = _dc.ReturnDataTable("spWebsite_GetRandom_Churches");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                Church church = new Church();
                church.ChurchId = Convert.ToInt32(dataRow["ChurchId"]);
                church.ChurchName = dataRow["ChurchName"].ToString();
             
                church.Address = dataRow["Address"].ToString();
                church.Town = dataRow["Town"].ToString();
                church.CountyName = dataRow["CountyName"].ToString();
                church.Website = dataRow["Website"].ToString();
                church.EmailAddress = dataRow["EmailAddress"].ToString();
                church.Phone = dataRow["Phone"].ToString();
                string res = dataRow["ImageURL"].ToString();
                   string res2 = res.Substring(0, 1);
                if (res2 != "/")
                {
                    church.ImageURl = "/" + res;
                }
                else
                {
                    church.ImageURl = res;
                }

                Balobj.Add(church);
            }
            return Balobj;
        }

        public IEnumerable<Church> GetByClientTypeChurch(int clientTypeId)
        {
            List<Church> Balobj = new List<Church>();

            _dc.ClearParameters();
            _dc.AddParameter("cTypeId", clientTypeId);

            DataTable dataTable = _dc.ReturnDataTable("spChurch_GetByClientType");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                Church church = new Church();
                church.ChurchId = Convert.ToInt32(dataRow["ChurchId"]);
                church.ChurchName = dataRow["ChurchName"].ToString();
                church.ClientTypeId = Convert.ToInt32(dataRow["ClientTypeId"]);
                church.Address = dataRow["Address"].ToString();
                church.Town = dataRow["Town"].ToString();
                church.Website = dataRow["Website"].ToString();
                church.EmailAddress = dataRow["EmailAddress"].ToString();
                church.Phone = dataRow["Phone"].ToString();
                string res = dataRow["ImageURL"].ToString();
                string res2 = res.Substring(0, 1);
                if (res2 != "/")
                {
                    church.ImageURl = "/" + res;
                }
                else
                {
                    church.ImageURl = res;
                }

                Balobj.Add(church);
            }
            return Balobj;
        }

    }
}
    