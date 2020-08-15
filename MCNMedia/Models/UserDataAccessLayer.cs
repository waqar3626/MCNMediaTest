using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public class UserDataAccessLayer
    {
        string connectionString = "server=localhost;port=3306;user=root;password=Khalid123;database=mcnmedia_dev";
        private string v;

        public UserDataAccessLayer(string v)
        {
            this.v = v;
        }


        //To View all Users details 
        public IEnumerable<User> GetAllUser()
        {
            List<User> Balobj = new List<User>();

            using (MySqlConnection  con = new MySqlConnection(connectionString))
            {
                MySqlCommand Cmd = new MySqlCommand("spUser_GetAll", con);
                Cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                MySqlDataReader rdr = Cmd.ExecuteReader();
                while (rdr.Read())
                {
                    User user = new User();
                    //user.UserId = Convert.ToInt32(rdr["UserId"]);
                    user.FirstName = rdr["FirstName"].ToString();
                    user.LastName = rdr["LastName"].ToString();
                    user.EmailAddress = rdr["EmailAddress"].ToString();
                    user.LoginPassword = rdr["LoginPassword"].ToString();
                    //user.UpdatedBy = Convert.ToInt32(rdr["UpdatedBy"]);
                    user.RoleId = Convert.ToInt32(rdr["RoleId"]);


                    Balobj.Add(user);
                }
                con.Close();
            }
            return Balobj;
        }



        //To Add new User record    
        public void AddUser(User user)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand("spUser_Add", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("FirstName", user.FirstName);
                cmd.Parameters.AddWithValue("LastName", user.LastName);
                cmd.Parameters.AddWithValue("EmailAddress", user.EmailAddress);
                cmd.Parameters.AddWithValue("LoginPassword", user.LoginPassword);
                cmd.Parameters.AddWithValue("UserId", user.UpdatedBy);
                cmd.Parameters.AddWithValue("RoleId", user.RoleId);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        //To Update the records of a particluar User
        public void UpdateUser(User user)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand("spUpdateEmployee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", user.UserId);
                cmd.Parameters.AddWithValue("@FName", user.FirstName);
                cmd.Parameters.AddWithValue("@LName", user.LastName);
                cmd.Parameters.AddWithValue("@Email", user.EmailAddress);
                cmd.Parameters.AddWithValue("@Password", user.LoginPassword);
                cmd.Parameters.AddWithValue("@UpdateBy", user.UpdatedBy);
                cmd.Parameters.AddWithValue("@roleId", user.RoleId);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        //Get the details of a particular User  
        public User GetUserData(int? id)
        {
            User user = new User();
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                string sqlQuery = "SELECT * FROM tblusers WHERE UserId= " + id;
                MySqlCommand cmd = new MySqlCommand(sqlQuery, con);
                con.Open();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    user.UserId = Convert.ToInt32(rdr["UserId"]);
                    user.FirstName = rdr["FName"].ToString();
                    user.LastName = rdr["LName"].ToString();
                    user.EmailAddress = rdr["Email"].ToString();
                    user.LoginPassword = rdr["Password"].ToString();
                    user.UpdatedBy = Convert.ToInt32(rdr["Updateby"]);
                    user.RoleId = Convert.ToInt32(rdr["RoleId"]);
                }
            }
            return user;
        }
        //To Delete the record on a particular User 
        public void DeleteUser(int? id)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand("ProcedureName", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("UserId", id);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}
