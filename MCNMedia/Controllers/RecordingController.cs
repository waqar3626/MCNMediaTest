using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MCNMedia_Dev.Controllers
{
    public class RecordingController : Controller
    {
        RecordingDataAccessLayer recordDataAccess = new RecordingDataAccessLayer();
        [HttpGet]
        public IActionResult AddRecording()
        {
            LoadChurchesDDL();
            return View();
        }
        [HttpGet]
       public IActionResult ListRecording()
        {
            List<Recording> recording = recordDataAccess.GetAllRecording().ToList<Recording>();
            return View(recording);

        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Recording recording = recordDataAccess.GetRecordingData(id);
            if (recording == null)
            {
                return NotFound();
            }
            LoadChurchesDDL();

            return View(recording);
        }

        [HttpGet()]
        public IActionResult GetAllRecording()
        {

            recordDataAccess.GetAllRecording();
            return View();
        }

        [HttpPost]
        public IActionResult AddRecording(Recording recording)
        {
            recordDataAccess.AddRecording(recording);
            return RedirectToAction("ListRecording");
        }

        [HttpPost]

        public IActionResult Edit(int id, [Bind] Recording recording)
        {
            
            recordDataAccess.UpdateRecording(recording);
            return RedirectToAction("ListRecording");
            
        }

        [HttpGet]

        public IActionResult Delete(int id)
        {
            recordDataAccess.DeleteRecording(id);
            return RedirectToAction("ListRecording");
        }

        public void LoadChurchesDDL()
        {
            IEnumerable<Recording> RecordList = recordDataAccess.GetChurches();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (var item in RecordList)
            {
                selectListItems.Add(new SelectListItem { Text = item.ChurchName.ToString(), Value = item.ChurchId.ToString() });
            }
            ViewBag.State = selectListItems;

        }


    }
}
