using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static MCNMedia_Dev.Models.Church;
using MCNMedia_Dev.Models;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MCNMedia_Dev.Repository
{
    public class ChurchDataAccessLayer
    {
        AwesomeDal.DatabaseConnect _dc;

        private readonly string AWS_S3_BUCKET_URI;

        public ChurchDataAccessLayer()
        {
            _dc = new AwesomeDal.DatabaseConnect();

            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            var root = builder.Build();
            var awsS3bucket = root.GetSection("S3BucketConfiguration");
            var sysConfig = root.GetSection("SystemConfiguration");
            AWS_S3_BUCKET_URI = $"{awsS3bucket["aws_bucket_url"]}/{sysConfig["system_mode"]}";
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
                church.ImageURl = $"{AWS_S3_BUCKET_URI}/{dataRow["ImageURL"]}";
                church.Slug = dataRow["Slug"].ToString();
                church.Featured = Convert.ToInt32(dataRow["Featured"]);
                church.UniqueIdentifier = dataRow["UniqueIdentifier"].ToString();
                church.Switch = Convert.ToInt32(dataRow["Switch"]);
                church.ShowOnWebsite = Convert.ToBoolean(dataRow["ShowOnWebsite"]);
                church.DisplayOrder = Convert.ToInt32(dataRow["DisplayOrder"]);
                church.Password = dataRow["Password"].ToString();

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
            _dc.AddParameter("Slug", church.Slug);
            _dc.AddParameter("Featured", church.Featured);
            _dc.AddParameter("UniqueIdentifier", Guid.NewGuid().ToString());
            _dc.AddParameter("Switch", church.Switch);
            _dc.AddParameter("ShowOnWebsite", church.ShowOnWebsite);
            _dc.AddParameter("DisplayOrder", church.DisplayOrder);
            _dc.AddParameter("ChurchPassword", church.Password);
            _dc.AddParameter("installationDate", church.InstallationDate);
            _dc.AddParameter("staticIP", church.StaticIP);
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
            _dc.AddParameter("installationDate", church.InstallationDate);
            _dc.AddParameter("staticIP", church.StaticIP);
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
                church.ImageURl = $"{AWS_S3_BUCKET_URI}/{dataRow["ImageURL"].ToString().Replace("//", "/")}";
                church.Slug = dataRow["Slug"].ToString();
                church.Password = dataRow["Password"].ToString();
                church.Featured = Convert.ToInt32(dataRow["Featured"]);
                church.UniqueIdentifier = dataRow["UniqueIdentifier"].ToString();
                church.Switch = Convert.ToInt32(dataRow["Switch"]);
                church.ShowOnWebsite = Convert.ToBoolean(dataRow["ShowOnWebsite"]);
                church.DisplayOrder = Convert.ToInt32(dataRow["DisplayOrder"]);
                church.InstallationDate = Convert.ToDateTime(dataRow["InstallationDate"].ToString());
                church.StaticIP= dataRow["StaticIP"].ToString();
            }
            return church;
        }
        public Church GetChurchDataBySlug(string chSlug)
        {
            Church church = new Church();

            _dc.ClearParameters();
            _dc.AddParameter("churchslug", chSlug);
            DataTable dataTable = _dc.ReturnDataTable("spChurch_GetBySlug");
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
                church.ImageURl = $"{AWS_S3_BUCKET_URI}/{dataRow["ImageURL"].ToString().Replace("//", "/")}";
                church.Slug = dataRow["Slug"].ToString();
                church.Password = dataRow["Password"].ToString();
                church.Featured = Convert.ToInt32(dataRow["Featured"]);
                church.UniqueIdentifier = dataRow["UniqueIdentifier"].ToString();
                church.Switch = Convert.ToInt32(dataRow["Switch"]);
                church.ShowOnWebsite = Convert.ToBoolean(dataRow["ShowOnWebsite"]);
                church.DisplayOrder = Convert.ToInt32(dataRow["DisplayOrder"]);
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

        public IEnumerable<Church> GetWebsiteChurch()
        {
            List<Church> churchList = new List<Church>();
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
                church.ImageURl = $"{AWS_S3_BUCKET_URI}/{dataRow["ImageURL"]}";
                church.Slug = dataRow["Slug"].ToString();
                churchList.Add(church);
            }
            return churchList;
        }

        public IEnumerable<Church> GetByClientTypeChurch(int clientTypeId)
        {
            List<Church> churchList = new List<Church>();

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
                church.ImageURl = $"{AWS_S3_BUCKET_URI}/{dataRow["ImageURL"]}";
                church.CountyId = Convert.ToInt32(dataRow["CountyId"].ToString());
                church.CountyName = dataRow["CountyName"].ToString();
                church.CountryId = Convert.ToInt32(dataRow["CountryId"].ToString());
                church.CountryName = dataRow["CountryName"].ToString();
                church.Slug = dataRow["Slug"].ToString();
                churchList.Add(church);
            }
            return churchList;
        }

    }
}
