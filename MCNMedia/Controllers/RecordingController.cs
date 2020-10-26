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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        RecordingDataAccessLayer recordDataAccess = new RecordingDataAccessLayer();
        [HttpGet]
        public IActionResult AddRecording()
        {
            try
            {
                LoadChurchesDDL();
                return View();
            }
            catch (Exception e)
            {
                ShowMesage("Add Recording View : "+e.Message);
                throw;
            } }
        [HttpGet]
        public IActionResult ListRecording()
        {
            try
            {

                List<Recording> recording = recordDataAccess.GetAllRecording().ToList<Recording>();
                return View(recording);

            }
            catch (Exception e)
            {
                ShowMesage("Recording List : "+e.Message);
                throw;
            }
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {

                Recording recording = recordDataAccess.GetRecordingData(id);
                if (recording == null)
                {
                    return NotFound();
                }
                LoadChurchesDDL();

                return View(recording);
            }
            catch (Exception e)
            {
                ShowMesage("Get edit recording data : "+e.Message);
                throw;
            }
        }

        [HttpGet()]
        public IActionResult GetAllRecording()
        {
            try
            {
                recordDataAccess.GetAllRecording();
                return View();
            }
            catch (Exception e)
            {
                ShowMesage("Get All Recording : " + e.Message);
                throw;
            }
            
        }

        [HttpPost]
        public IActionResult AddRecording(Recording recording)
        {
            try
            {
                recordDataAccess.AddRecording(recording);
                return RedirectToAction("ListRecording");

            }
            catch (Exception e)
            {
                ShowMesage("Add Recording Error :" + e.Message);
                throw;

            }
        }

        [HttpPost]

        public IActionResult Edit(int id, [Bind] Recording recording)
        {
            try
            {

                recordDataAccess.UpdateRecording(recording);
                return RedirectToAction("ListRecording");

            }
            catch (Exception e)
            {
                ShowMesage("Edit recording error : " + e.Message);
                throw;
            }

        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {

                recordDataAccess.DeleteRecording(id);
                return RedirectToAction("ListRecording");

            }
            catch (Exception e)
            {
                ShowMesage("Delete Recording Error : " + e.Message);
                throw;
            }
        }

        public void LoadChurchesDDL()
        {
            try
            {

                IEnumerable<Recording> RecordList = recordDataAccess.GetChurches();
                List<SelectListItem> selectListItems = new List<SelectListItem>();
                foreach (var item in RecordList)
                {
                    selectListItems.Add(new SelectListItem { Text = item.ChurchName.ToString(), Value = item.ChurchId.ToString() });
                }
                ViewBag.State = selectListItems;

            }
            catch (Exception e)
            {
                ShowMesage("LoadChurch DropDown Error : "+e.Message);
                throw;
            }
        }

        private void ShowMesage(String exceptionMessage)
        {
            log.Error("Exception : " + exceptionMessage);
        }



    }
}
