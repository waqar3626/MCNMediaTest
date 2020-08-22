using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;

namespace MCNMedia_Dev.Repository
{
    public class RecordingDataAccessLayer
    {
        AwesomeDal.DatabaseConnect _dc;
        private string v;

        public RecordingDataAccessLayer()
        {
            _dc = new AwesomeDal.DatabaseConnect();
        }

        public RecordingDataAccessLayer(string v)
        {
            this.v = v;
            _dc = new AwesomeDal.DatabaseConnect();
        }
        //To View all Recordings details
        public IEnumerable<Recording> GetAllRecording()
        {
            List<Recording> Balobj = new List<Recording>();
            _dc.ClearParameters();
            _dc.AddParameter("RecId", -1);
            _dc.AddParameter("RecName", "");
           DataTable dataTable = _dc.ReturnDataTable("spRecording_Search");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                Recording recording = new Recording();
                recording.RecordingId = Convert.ToInt32(dataRow["RecordingId"]);
                recording.RecordingTitle = dataRow["RecordingName"].ToString();
                recording.RecordingURl = dataRow["RecordingURL"].ToString();
                recording.Date = Convert.ToDateTime(dataRow["RecordingDate"].ToString());
                recording.Time = Convert.ToDateTime(dataRow["RecordingTime"].ToString());
                recording.ChurchId = Convert.ToInt32(dataRow["ChurchId"]);
                recording.ChurchName = dataRow["ChurchName"].ToString();


                Balobj.Add(recording);
            }
            return Balobj;
        }
        public IEnumerable<Recording> GetChurches()
        {
            List<Recording> Balobj = new List<Recording>();
            _dc.ClearParameters();
            DataTable dataTable = _dc.ReturnDataTable("spchurches_Get");

            foreach (DataRow dataRow in dataTable.Rows)
            {
                Recording recording = new Recording();
                recording.ChurchId = Convert.ToInt32(dataRow["ChurchId"]);
                recording.ChurchName = dataRow["ChurchName"].ToString();
                Balobj.Add(recording);
            }
            return Balobj;
        }

        //To Add new Recording record
        public void AddRecording(Recording recording)
        {
            _dc.ClearParameters();
            _dc.AddParameter("RecordingName", recording.RecordingTitle);
            _dc.AddParameter("RecordingURL", recording.RecordingURl);
            _dc.AddParameter("RecordingDate", recording.Date);
            _dc.AddParameter("RecordingTime", recording.Time);
            _dc.AddParameter("UserId", recording.UpdatedBy);
            _dc.AddParameter("ChurchId", recording.ChurchId);
            _dc.Execute("spRecording_Add");
        }

        //To Update the records of a particluar Recording
        public void UpdateRecording(Recording recording)
        {
            _dc.ClearParameters();
            _dc.AddParameter("RecId", recording.RecordingId);
            _dc.AddParameter("RecName", recording.RecordingTitle);
            _dc.AddParameter("RecURL", recording.RecordingURl);
            _dc.AddParameter("RecDate", recording.Date);
            _dc.AddParameter("RecTime", recording.Time);
            _dc.AddParameter("UpdateBy", recording.UpdatedBy);
            _dc.AddParameter("ChurchId", recording.ChurchId);
            _dc.Execute("spRecording_Update");
        }

        //Get the details of a particular Recording
        public Recording GetRecordingData(int id)
        {
            Recording recording = new Recording();

            _dc.ClearParameters();
            _dc.AddParameter("RecId", id);
            DataTable dataTable = _dc.ReturnDataTable("spRecording_GetById");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                recording.RecordingId = Convert.ToInt32(dataRow["RecordingId"]);
                recording.RecordingTitle = dataRow["RecordingName"].ToString();
                recording.RecordingURl = dataRow["RecordingURL"].ToString();
                recording.Date = Convert.ToDateTime(dataRow["RecordingDate"].ToString());
                recording.Time = Convert.ToDateTime(dataRow["RecordingTime"].ToString());
                recording.ChurchId = Convert.ToInt32(dataRow["ChurchId"]);

            
            }
            return recording;
        }
        //To Delete the record on a particular Recording
        public void DeleteRecording(int id)
            {
                _dc.ClearParameters();
                _dc.AddParameter("RecId", id);
                _dc.AddParameter("UpdateBy", 1);
                _dc.Execute("spRecording_Delete");
            }

        }
}
