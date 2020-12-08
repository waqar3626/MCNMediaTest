﻿using System;
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

        public RecordingDataAccessLayer()
        {
            _dc = new AwesomeDal.DatabaseConnect();
        }
        
        public void AddRecording(Recording recording)
        {
            _dc.ClearParameters();
            _dc.AddParameter("RecordingName", recording.RecordingTitle);
            _dc.AddParameter("RecordingURL", recording.RecordingURl);
            _dc.AddParameter("RecordingDate", recording.Date);
            _dc.AddParameter("RecordingTime", recording.Time);
            _dc.AddParameter("UserId", recording.CreatedBy);
            _dc.AddParameter("ChurchId", recording.ChurchId);
            _dc.AddParameter("SchId", recording.ScheduleId);
            _dc.Execute("spRecording_Add");
        }

        public IEnumerable<Recording> Recording_GetAll()
        {
            return Recording_GetFromDatabase(churchId: -1, recordId: -1, recordName: "");
        }

        public IEnumerable<Recording> Recording_GetByChurch(int churchId)
        {
            return Recording_GetFromDatabase(churchId: churchId, recordId: -1, recordName: "");
        }

        public Recording Recording_GetById(int recordingId)
        {
            Recording recording = new Recording();
            List<Recording> recordings = Recording_GetFromDatabase(churchId: -1, recordId: recordingId, recordName: "").ToList();
            if (recordings.Count > 0)
            {
                recording = recordings.First();
            }
            return recording;
        }

        private IEnumerable<Recording> Recording_GetFromDatabase(int churchId,int recordId,string recordName)
        {
            List<Recording> recordingList = new List<Recording>();
            _dc.ClearParameters();
            _dc.AddParameter("chrchId", churchId);
            _dc.AddParameter("recordId", recordId); ;
            _dc.AddParameter("recordName", recordName);
            DataTable dataTable = _dc.ReturnDataTable("spRecording_Get");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                Recording recording = BindRecording(dataRow);
                recordingList.Add(recording);
            }
            return recordingList;
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
            _dc.AddParameter("ChrhId", recording.ChurchId);
            _dc.Execute("spRecording_Update");
        }

        public void DeleteRecording(int id, int UpdateBy)
        {
            _dc.ClearParameters();
            _dc.AddParameter("RecId", id);
            _dc.AddParameter("UpdateBy", UpdateBy);
            _dc.Execute("spRecording_Delete");
        }

        private static Recording BindRecording(DataRow dataRow)
        {
            Recording recording = new Recording();
            recording.RecordingId = Convert.ToInt32(dataRow["RecordingId"]);
            recording.RecordingTitle = dataRow["RecordingName"].ToString();
            recording.RecordingURl = dataRow["RecordingURL"].ToString();
            recording.Date = Convert.ToDateTime(dataRow["RecordingDate"].ToString());
            recording.Time = Convert.ToDateTime(dataRow["RecordingTime"].ToString());
            recording.CreatedAt = Convert.ToDateTime(dataRow["CreatedAtandUpdateDate"].ToString());
            recording.ChurchId = Convert.ToInt32(dataRow["ChurchId"]);
            recording.ChurchName = dataRow["ChurchName"].ToString();
            return recording;
        }

    }
}
