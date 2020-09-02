﻿using MCNMedia_Dev.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Repository
{
    public class UserDataAccessLayer
    {
        AwesomeDal.DatabaseConnect _dc;
        public UserDataAccessLayer()
        {
            _dc = new AwesomeDal.DatabaseConnect();
        }

        //To View all Users details 
        public IEnumerable<User> GetAllUser()
        {
            List<User> Balobj = new List<User>();
            _dc.ClearParameters();
            _dc.AddParameter("UsrId", -1);
            _dc.AddParameter("FName", "");
            _dc.AddParameter("LName", "");
            _dc.AddParameter("EmailAdd", "");
            DataTable dataTable = _dc.ReturnDataTable("spUser_Search");
            foreach(DataRow dataRow in dataTable.Rows)
            {
                User user = new User();
                user.UserId = Convert.ToInt32(dataRow["UserId"]);
                user.FirstName = dataRow["FirstName"].ToString();
                user.LastName = dataRow["LastName"].ToString();
                user.EmailAddress = dataRow["EmailAddress"].ToString();
                user.LoginPassword = dataRow["LoginPassword"].ToString();
                //user.UpdatedBy = Convert.ToInt32(rdr["UpdatedBy"]);
                user.RoleName = dataRow["RoleName"].ToString();


                Balobj.Add(user);
            }
            return Balobj;
        }

        // get user role
        public IEnumerable<UserRoles> GetRoles()
        {
            List<UserRoles> Balobj = new List<UserRoles>();
            _dc.ClearParameters();
            DataTable dataTable = _dc.ReturnDataTable("spRoles_Get");

            foreach (DataRow dataRow in dataTable.Rows)
            {
                UserRoles role = new UserRoles();
                role.RoleId = Convert.ToInt32(dataRow["RoleId"]);
                role.RoleName = dataRow["RoleName"].ToString();
                Balobj.Add(role);
            }
            return Balobj;
        }


        //To Add new User record    
        public void AddUser(User user)
        {
            _dc.ClearParameters();
            _dc.AddParameter("FirstName", user.FirstName);
            _dc.AddParameter("LastName", user.LastName);
            _dc.AddParameter("EmailAddress", user.EmailAddress);
            _dc.AddParameter("LoginPassword", user.LoginPassword);
            _dc.AddParameter("UserId", user.UpdatedBy);
            _dc.AddParameter("RoleId", user.RoleId);
            _dc.Execute("spUser_Add");
        }
        //To Update the records of a particluar User
        public void UpdateUser(User user)
        {
            _dc.ClearParameters();
            _dc.AddParameter("UsrId", user.UserId);
            _dc.AddParameter("FName", user.FirstName);
            _dc.AddParameter("LName", user.LastName);
            _dc.AddParameter("EmailAdd", user.EmailAddress);
            //_dc.AddParameter("@Password", user.LoginPassword);
            _dc.AddParameter("UpdateBy", user.UpdatedBy);
            _dc.AddParameter("RoleId", user.RoleId);
            _dc.Execute("spUser_Update");
        }

        //Get the details of a particular User
        public User GetUserData(int id)
        {
            User user = new User();

            _dc.ClearParameters();
            _dc.AddParameter("UsrId", id);
            DataTable dataTable = _dc.ReturnDataTable("spUser_GetbyId");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                user.UserId = Convert.ToInt32(dataRow["UserId"]);
                user.FirstName = dataRow["FirstName"].ToString();
                user.LastName = dataRow["LastName"].ToString();
                user.EmailAddress = dataRow["EmailAddress"].ToString();
                user.LoginPassword = dataRow["LoginPassword"].ToString();
                //user.UpdatedBy = Convert.ToInt32(rdr["UpdatedBy"]);
                user.RoleId = Convert.ToInt32(dataRow["RoleId"]);
            }

            //            using (MySqlConnection con = new MySqlConnection(connectionString))
            //{
            //    string sqlQuery = "SELECT * FROM tblusers WHERE UserId= " + id;
            //    MySqlCommand cmd = new MySqlCommand(sqlQuery, con);
            //    con.Open();
            //    MySqlDataReader rdr = cmd.ExecuteReader();
            //    while (rdr.Read())
            //    {
            //        user.UserId = Convert.ToInt32(rdr["UserId"]);
            //        user.FirstName = rdr["FirstName"].ToString();
            //        user.LastName = rdr["LastName"].ToString();
            //        user.EmailAddress = rdr["EmailAddress"].ToString();
            //        user.LoginPassword = rdr["LoginPassword"].ToString();
            //        //user.UpdatedBy = Convert.ToInt32(rdr["Updatedby"]);
            //        user.RoleId = Convert.ToInt32(rdr["RoleId"]);
            //    }
            //}
            return user;
        }

        //To Delete the record on a particular User 
        public void DeleteUser(int id)
        {
            _dc.ClearParameters();
            _dc.AddParameter("UsrId", id);
            _dc.AddParameter("UpdateBy", 1);
            _dc.ReturnBool("spUser_Delete");
        }
    }
}