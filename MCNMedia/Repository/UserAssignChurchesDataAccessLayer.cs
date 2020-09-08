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

        //public IEnumerable<UserAssignChurches> GetUsers()
        //{
        //    List<UserAssignChurches> Balobj = new List<UserAssignChurches>();
        //    _dc.ClearParameters();
        //    DataTable dataTable = _dc.ReturnDataTable("spUser_Get");

        //    foreach (DataRow dataRow in dataTable.Rows)
        //    {
        //        UserAssignChurches usr = new UserAssignChurches();
        //        usr.UserId = Convert.ToInt32(dataRow["UserId"]);
        //        usr.FirstName = dataRow["FirstName"].ToString();
        //        Balobj.Add(usr);
        //    }
        //    return Balobj;
        //}

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
    }
}
