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
        scheduleDataAccessLayer SchDataAccess = new scheduleDataAccessLayer();
        GenericModel gm = new GenericModel();
        public JsonResult ListSchedule()
        {
            int churchId = (int)HttpContext.Session.GetInt32("ChurchId");
            List<Schedule> ChurchSchedule = SchDataAccess.GetAllChurchSchedule(churchId).ToList();
            return Json(ChurchSchedule);
        }
        
    }
}
