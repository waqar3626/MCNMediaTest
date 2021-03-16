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
        public IActionResult ReadMessage(int MessageId)
        {
            Inbox inbox = new Inbox();
            try
            {
                inboxDataAccessLayer.ChangeMailStatus(MessageId,3);
                inbox = inboxDataAccessLayer.GetMailDataById(MessageId);
                return View(inbox);
            }
            catch (Exception exp)
            {
                ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                return View(inbox);
            }

        }
        public IActionResult DeleteMail(int MessageId)
        {
           
            List<Inbox> inbox = new List<Inbox>();
            try
            {
                bool res = inboxDataAccessLayer.DeleteMail(MessageId);

                inbox = inboxDataAccessLayer.GetAllEmails().ToList();
                ViewBag.SuccessMsg = "Mail Deleted Successfully";
                return View("Inbox",inbox);

            }
            catch (Exception exp)
            {
                ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                return View("Inbox",inbox);
            }

        }



    }
}
