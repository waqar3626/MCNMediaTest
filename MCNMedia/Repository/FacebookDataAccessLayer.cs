using MCNMedia_Dev.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Repository
{
    public class FacebookDataAccessLayer
    {
        AwesomeDal.DatabaseConnect _dc;

        public FacebookDataAccessLayer()
        {
            _dc = new AwesomeDal.DatabaseConnect();
        }




        public int FacebookCameraStreamingAdd(int chuchId, int cameraId,string streamKey , int createdBy)
        {
            _dc.CloseAndDispose();
            _dc.ClearParameters();
            _dc.AddParameter("chrId", chuchId);
            _dc.AddParameter("camId", cameraId);
            _dc.AddParameter("stramKey",streamKey);
            _dc.AddParameter("createdBy", createdBy);

            return _dc.Execute("spFacebook_CameraStreamingAdd");

        }
       
        public bool FacebookCameraStreamingDelete(int chuchId, int cameraId)
        {
            _dc.CloseAndDispose();
            _dc.ClearParameters();
            _dc.AddParameter("chrId", chuchId);
            _dc.AddParameter("camId", cameraId);
          return _dc.ReturnBool("spFacebook_CameraStreamingDelete");
        }

       public FbStreaming FacebookStreamingGetByCameraId(int cameraId)
        {
            FbStreaming fbStreaming = new FbStreaming();
            _dc.CloseAndDispose();
            _dc.ClearParameters();
            
            _dc.AddParameter("camId", cameraId);
          
            DataTable dataTable = _dc.ReturnDataTable("spFacebookStreaming_GetByCameraId"); ;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                fbStreaming.StreamId = Convert.ToInt32(dataRow["StreamId"]);
                fbStreaming.StreamKey = dataRow["StreamKey"].ToString();
                fbStreaming.CameraId = Convert.ToInt32(dataRow["CameraId"]);
                fbStreaming.ChurchId = Convert.ToInt32(dataRow["ChurchId"]);
                fbStreaming.Status = Convert.ToInt32(dataRow["Status"]);

            }

            return fbStreaming;
        }






    }
}
