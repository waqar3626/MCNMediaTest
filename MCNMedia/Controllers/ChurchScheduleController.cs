using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MCNMedia_Dev.Controllers
{
    public class ChurchScheduleController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        scheduleDataAccessLayer SchDataAccess = new scheduleDataAccessLayer();
        GenericModel gm = new GenericModel();
        public JsonResult ListSchedule()
        {
            try
            {
                int churchId = (int)HttpContext.Session.GetInt32("ChurchId");
                List<Schedule> ChurchSchedule = SchDataAccess.GetAllChurchSchedule(churchId).ToList();
                return Json(ChurchSchedule);
            }
            catch (Exception e)
            {
                ShowMessage("Add Church Schedule Errors : " + e.Message);
                throw;
                throw;
            }
           
        }

        [HttpPost]
        public JsonResult AddSchedule(bool ToggleRecord, Schedule sch)
        {
            try
            {
                if (sch.IsRepeated == false)
                {
                    sch.EventDay = sch.EventDate.ToString("dddd");
                }
                else
                {
                    sch.EventDate = Convert.ToDateTime("1900-01-01 00:00:00");
                }

                sch.Record = ToggleRecord;
                
                if (!string.IsNullOrEmpty(HttpContext.Session.GetInt32("ChurchId").ToString()))
                {
                   sch.ChurchId= Convert.ToInt32( HttpContext.Session.GetInt32("ChurchId"));
                    int res = SchDataAccess.AddSchedule(sch);
                }
                return Json(1);

            }
            catch (Exception e)
            {
                ShowMessage("Add Schedule Errors : " + e.Message);
                throw;
            }
        }

      

        private void ShowMessage(String exceptionMessage) 
        {
            log.Error("Exception : " + exceptionMessage);
        }
    }
}
