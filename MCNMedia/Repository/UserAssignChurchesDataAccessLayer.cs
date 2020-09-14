using MCNMedia_Dev.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Repository
{
    public class UserAssignChurchesDataAccessLayer
    {
        AwesomeDal.DatabaseConnect _dc;
        public UserAssignChurchesDataAccessLayer()
        {
            _dc = new AwesomeDal.DatabaseConnect();
        }

        public IEnumerable<UserAssignChurches> GetChurchList()
        {
            List<UserAssignChurches> Balobj = new List<UserAssignChurches>();
            _dc.ClearParameters();
            DataTable dataTable = _dc.ReturnDataTable("spChurchList_Get");

            foreach (DataRow dataRow in dataTable.Rows)
            {
                UserAssignChurches userAssignChurches = new UserAssignChurches();
                userAssignChurches.ChurchId = Convert.ToInt32(dataRow["ChurchId"]);
                userAssignChurches.ChurchName = dataRow["ChurchName"].ToString();
                Balobj.Add(userAssignChurches);
            }
            return Balobj;
        }

        public DataTable GetUserDDL()
        {
            _dc.ClearParameters();
            return _dc.ReturnDataTable("spUser_Get");

        }

        public void AddUserChurch(UserAssignChurches uas)
        {
            _dc.ClearParameters();
            _dc.AddParameter("UsrId", uas.UserId);
            _dc.AddParameter("ChId", uas.ChurchId);
            _dc.AddParameter("CreatedBy", uas.CreatedBy);
            _dc.Execute("spUser_Churches_Add");
        }
        public IEnumerable<UserAssignChurches> GetUserAssignChurchesList()
        {

            List<UserAssignChurches> Balobj = new List<UserAssignChurches>();
            _dc.ClearParameters();
            DataTable dataTable =  _dc.ReturnDataTable("spUser_Churches_Get");

            foreach (DataRow dataRow in dataTable.Rows)
            {
                UserAssignChurches userAssignChurches = new UserAssignChurches();
                //userAssignChurches.UserAssignChurchId= Convert.ToInt32(dataRow["UserChurchId"]);
                userAssignChurches.UserId= Convert.ToInt32(dataRow["UserId"]);
               // userAssignChurches.ChurchId = Convert.ToInt32(dataRow["ChurchId"]);
                userAssignChurches.ChurchName = dataRow["ChurchName"].ToString();
                userAssignChurches.FirstName= dataRow["fullname"].ToString();
             

              Balobj.Add(userAssignChurches);
            }
              return Balobj;
        }
    
    public IEnumerable<UserAssignChurches> GetSingleUserAssignChurches(int UserId)
    {
        List<UserAssignChurches> Balobj = new List<UserAssignChurches>();
        _dc.ClearParameters();
            _dc.AddParameter("UsrId", UserId);
            DataTable dataTable = _dc.ReturnDataTable("spUser_Churches_GetSingleUser");

        foreach (DataRow dataRow in dataTable.Rows)
        {
            UserAssignChurches userAssignChurches = new UserAssignChurches();
            userAssignChurches.UserAssignChurchId = Convert.ToInt32(dataRow["UserChurchId"]);
            userAssignChurches.ChurchId = Convert.ToInt32(dataRow["ChurchId"]);
            userAssignChurches.UserId = Convert.ToInt32(dataRow["UserId"]);
           


            Balobj.Add(userAssignChurches);
        }
        return Balobj;
    }

       

        //public void UpdateUserChurch(UserAssignChurches uas)
        //{
        //    _dc.ClearParameters();
        //    _dc.AddParameter("UsrId", uas.UserId);
        //    _dc.AddParameter("ChId", uas.ChurchId);
        //    _dc.AddParameter("UpdateBy", uas.UpdateBy);
        //    _dc.Execute("spUser_Church_Update");
        //}
        public void DeleteUserChurches(int id)
        {
            _dc.ClearParameters();
            _dc.AddParameter("UsrId", id);
            _dc.AddParameter("UpdateBy", 1);
            _dc.ReturnBool("spUser_Churches_Delete");
        }

        public IEnumerable<UserAssignChurches> GetUserAssignedChurches(int UserId)
        {
            List<UserAssignChurches> Balobj = new List<UserAssignChurches>();
            _dc.ClearParameters();
            _dc.AddParameter("UsrId", UserId);
            DataTable dataTable = _dc.ReturnDataTable("spUserChurch_Assigned_GetByUserId");

            foreach (DataRow dataRow in dataTable.Rows)
            {
                UserAssignChurches userAssignChurches = new UserAssignChurches();
                userAssignChurches.UserId = UserId;
                userAssignChurches.ChurchId = Convert.ToInt32(dataRow["ChurchId"]);
                userAssignChurches.ChurchName = dataRow["ChurchName"].ToString();
                userAssignChurches.Assigned = Convert.ToBoolean(dataRow["Assigned"]);
                Balobj.Add(userAssignChurches);
            }
            return Balobj;
        }
    }
}

    

