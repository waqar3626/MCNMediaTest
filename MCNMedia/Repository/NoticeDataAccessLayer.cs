using MCNMedia_Dev.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Repository
{
    public class NoticeDataAccessLayer
    {
        AwesomeDal.DatabaseConnect _dc;

        public NoticeDataAccessLayer()
        {
            _dc = new AwesomeDal.DatabaseConnect();
        }

      
        public IEnumerable<Notice> GetAllNotices(int ChrId)
        {
            List<Notice> listnotice = new List<Notice>();
            _dc.ClearParameters();
            _dc.AddParameter("chrId", ChrId);
            DataTable dataTable = _dc.ReturnDataTable("spChurchNotice_GetByChurchId");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                Notice notice = new Notice();
                notice.ChurchNoticeId = Convert.ToInt32(dataRow["ChurchNoticeId"]);
                notice.NoticeTitle = dataRow["NoticeTitle"].ToString();
                notice.NoticeName = dataRow["Notice"].ToString();
                notice.CreatedAt = Convert.ToDateTime(dataRow["CreatedAt"].ToString());
                notice.SysTime = Convert.ToDateTime(dataRow["CreatedAt"]).ToString("dd-MMM-yyyy");
                notice.CreatedBy = dataRow["FirstName"].ToString();
                listnotice.Add(notice);
            }
            return listnotice;
        }


        public int AddNotice(Notice notice)
        {
            _dc.ClearParameters();
            _dc.AddParameter("UserId", notice.UpdatedBy);
            _dc.AddParameter("NotTitle", notice.NoticeTitle);
            _dc.AddParameter("Notice2", notice.NoticeName);
            _dc.AddParameter("ChurchId1", notice.ChurchId);
            return _dc.Execute("spChurchNotice_Add");
           
        }

        public Notice GetNoticeById(int ChNoticeId)
        {
            Notice notice = new Notice();

            _dc.ClearParameters();
            _dc.AddParameter("ChNoticeId", ChNoticeId);
            DataTable dataTable = _dc.ReturnDataTable("spChurchNotice_GetById");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                notice.ChurchNoticeId = Convert.ToInt32(dataRow["ChurchNoticeId"]);
                notice.NoticeTitle = dataRow["NoticeTitle"].ToString();
                notice.NoticeName = dataRow["Notice"].ToString();
               
            }
            return notice;
        }


        public int UpdateNotice(Notice not)
        {
            _dc.ClearParameters();
            _dc.AddParameter("NoticeId", not.ChurchNoticeId);
            _dc.AddParameter("NotTitle", not.NoticeTitle);
            _dc.AddParameter("Notice1", not.NoticeName);
            _dc.AddParameter("UserId", not.UpdatedBy);

            return _dc.Execute("spChurchNotice_Update");
        }

        public bool DeleteNotice(int chnoticeId, int UpdateBy)
        {
            _dc.ClearParameters();
            _dc.AddParameter("ChNoticeId", chnoticeId);
            _dc.AddParameter("UserId", UpdateBy);
            return _dc.ReturnBool("spChurchNotice_Delete");
        }

    }
}
