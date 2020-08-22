using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySqlConnector;

namespace MCNMedia_Dev.Controllers
{
    public class DDLController : Controller
    {
        public ActionResult Index()
        {
            String conn = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            String query = "select UserId,FirstName from tblusers";
            MySqlCommand mySqlCommand = new MySqlCommand(query, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
            DataSet dataSet = new DataSet();
            mySqlDataAdapter.Fill(dataSet);
            ViewBag.roleid = dataSet.Tables[0];
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.roleid.Rows)
            {
                selectListItems.Add(new SelectListItem { Text = @dr["FirstName"].ToString(), Value = @dr["UserId"].ToString() });
            }
            ViewBag.State = selectListItems;
            mySqlConnection.Close();
            return View();
        }
    }
}
