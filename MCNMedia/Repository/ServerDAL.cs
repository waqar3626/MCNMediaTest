using MCNMedia_Dev.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Repository
{
    public class ServerDAL
    {
        AwesomeDal.DatabaseConnect _dc;

        public ServerDAL()
        {
            _dc = new AwesomeDal.DatabaseConnect();
        }

        public List<Server> GetServer()
        {
            List<Server> Balobj = new List<Server>();
            _dc.ClearParameters();
            DataTable dataTable = _dc.ReturnDataTable("spServer_Get");

            foreach (DataRow dataRow in dataTable.Rows)
            {
                Server server = new Server();
                server.ServerId = Convert.ToInt32(dataRow["ServerId"]);
                server.ServerName = dataRow["ServerName"].ToString();
                server.ServerIP = dataRow["ServerIP"].ToString();

                Balobj.Add(server);
            }
            return Balobj;
        }
    }
}
