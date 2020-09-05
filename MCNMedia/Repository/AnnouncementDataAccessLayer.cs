using MCNMedia_Dev.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Repository
{
    public class AnnouncementDataAccessLayer
    {
        AwesomeDal.DatabaseConnect _dc;

        public AnnouncementDataAccessLayer()
        {
            _dc = new AwesomeDal.DatabaseConnect();
        }




        public int AddAnnouncement(Announcement announcement)
        {
            _dc.ClearParameters();
            _dc.AddParameter("UserId", 1);
            _dc.AddParameter("ChurchId", announcement.ChurchId);
            _dc.AddParameter("AnnouncementTitle", announcement.AnnouncementTitle);
            _dc.AddParameter("AnnouncementText", announcement.AnnouncementText);
           
            return _dc.Execute("spAnnouncement_Add");
        }
        public IEnumerable<Announcement> GetAnnouncement( int ChrId)
        {
            List<Announcement> Balobj = new List<Announcement>();

            Announcement announcement = new Announcement();

            _dc.ClearParameters();
            _dc.AddParameter("chrId", ChrId);
            DataTable dataTable = _dc.ReturnDataTable("spAnnouncement_GetByChurchId");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                announcement.ChurchAnnouncementId = Convert.ToInt32(dataRow["ChurchAnnouncementId"].ToString());
                //announcement.ChurchId = Convert.ToInt32(dataRow["TabName"].ToString());
                announcement.AnnouncementTitle = dataRow["AnnouncementTitle"].ToString();
                announcement.AnnouncementText = dataRow["Announcement"].ToString();
                announcement.CreatedAt = Convert.ToDateTime(dataRow["CreatedAt"].ToString());
                Balobj.Add(announcement);
            }
            return Balobj;
        }
        public bool DeleteAnnouncement(int ChAnnoaId)
        {
            _dc.ClearParameters();
            _dc.AddParameter("AnnouceId", ChAnnoaId);
            _dc.AddParameter("UpdatedBy", 1);
            return _dc.ReturnBool("spAnnouncement_Delete");
        }

        public Announcement GetAnnouncementById(int announcId)
        {
            Announcement announcement = new Announcement();

            _dc.ClearParameters();
            _dc.AddParameter("ChurchAnnounceId", announcId);
            DataTable dataTable = _dc.ReturnDataTable("spAnnouncement_ById");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                announcement.ChurchAnnouncementId = Convert.ToInt32(dataRow["ChurchAnnouncementId"]);
                announcement.AnnouncementTitle = dataRow["AnnouncementTitle"].ToString();
                announcement.AnnouncementText = dataRow["Announcement"].ToString();

            }
            return announcement;
        }


        public int UpdateAnnouncement(Announcement announcement)
        {
            _dc.ClearParameters();
            _dc.AddParameter("ChurchAnnounId", announcement.ChurchAnnouncementId);
            _dc.AddParameter("AnnouncementTitle", announcement.AnnouncementTitle);
            _dc.AddParameter("AnnouncementText", announcement.AnnouncementText);
            _dc.AddParameter("UpdateBy", 1);

            return _dc.Execute("spAnnouncement_Update");
        }
    }
}
