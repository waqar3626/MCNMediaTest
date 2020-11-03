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
                ShowMessage("Add Recording Error 'Get' " + e.Message);
                throw;
            }

        }
        [HttpGet]
        public IActionResult ListRecording()
        {
            try
            {
                LoadChurchesDDL();
                GenericModel gm = new GenericModel();
                gm.LRecordings = recordDataAccess.GetAllRecording().ToList<Recording>();
                return View(gm);
            }
            catch(Exception e)
            {
                ShowMessage ("List Recording Error" + e.Message);
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

                return PartialView("Edit", recording);
            }
            catch (Exception e)
            {
                ShowMessage("Edit Recording Errors 'Get' : " + e.Message);
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
                ShowMessage("Add Recording Error 'Post' " + e.Message);
                throw;
            }
           
        }

        [HttpPost]
        public IActionResult AddRecording(Recording record)
        {
            try
            {
                recordDataAccess.AddRecording(record);
                return RedirectToAction("ListRecording");
            }
            catch (Exception e)
            {
                ShowMessage("Add Recording Error 'Post' " + e.Message);
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
                ShowMessage("Edit Recording Errors 'Post' : " + e.Message);
                throw;
            }
            
        }

        [HttpGet]

        public IActionResult Delete(int id, int UpdateBy)
        {
            try
            {
                recordDataAccess.DeleteRecording(id, UpdateBy);
                return RedirectToAction("ListRecording");
            }
            catch (Exception e)
            {
                ShowMessage("Delete Recording Error " + e.Message);
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
                ShowMessage("Load Church Error in Recording " + e.Message);
                throw;
            }
           

        }

        private void ShowMessage(string exceptionMessage)
        {
            log.Info("Exception: "+exceptionMessage);
        }



    }
}
