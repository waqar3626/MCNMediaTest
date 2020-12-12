using MCNMedia_Dev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Repository
{
    public class WebsiteDataAccessLayer

    {
        AwesomeDal.DatabaseConnect _dc;
        public WebsiteDataAccessLayer()
        {

            _dc = new AwesomeDal.DatabaseConnect();
        }
        public void AddContactForm(Website website)
        {
            _dc.ClearParameters();
            _dc.AddParameter("CntName", website.ContactName);
            _dc.AddParameter("CntMail", website.ContactEmail);
            _dc.AddParameter("CntSubject", website.ContactSubject);
            _dc.AddParameter("CntMessage", website.Message);
            _dc.Execute("spContactUs_Add");
        }

      }
}
