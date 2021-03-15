using MCNMedia_Dev._Helper;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace MCNMedia_Dev.Controllers
{
    public class InboxController : Controller
    {
        InboxDataAccessLayes inboxDataAccessLayer = new InboxDataAccessLayes();
        public IActionResult Inbox()
        {
            List<Inbox> inbox = new List<Inbox>();
            try
            {
                inboxDataAccessLayer.UpdateUnreadMailStatus();
               inbox = inboxDataAccessLayer.GetAllEmails().ToList();
                return View(inbox);
            }
            catch (Exception exp)
            {
                ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                return View(inbox);
            }
          
        }

     
    }
}
