using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Models
{
    public static class Lists
    {
        public static List<SelectListItem> GetCountyList()
        {
            ChurchDataAccessLayer churchDataAccess = new ChurchDataAccessLayer();
            var counties = churchDataAccess.GetCountyList();
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem
            {
                Text = "--Select--",
                Value = "0"
            });
            foreach (DataRow dr in counties.Rows)
            {
                list.Add(new SelectListItem
                {
                    Text = dr["CountyName"].ToString(),
                    Value = dr["CountyId"].ToString()
                });
            }
            return list;
        }

        public static List<SelectListItem> GetClientTypeList()
        {
            ChurchDataAccessLayer churchDataAccess = new ChurchDataAccessLayer();
            var clientTypes = churchDataAccess.GetClientTypeList();
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem
            {
                Text = "--Select--",
                Value = "0"
            });
            foreach (DataRow dr in clientTypes.Rows)
            {
                list.Add(new SelectListItem
                {
                    Text = dr["ClientTypeTitle"].ToString(),
                    Value = dr["ClientTypeId"].ToString()
                });
            }
            return list;
        }

        public static List<SelectListItem> GetChurchListForDDL()
        {
            ChurchDataAccessLayer churchDataAccess = new ChurchDataAccessLayer();
            var ChurchDDL = churchDataAccess.GetChurchDDL();
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem
            {
                Text = "--Select--",
                Value = "0"
            });
            foreach (DataRow dr in ChurchDDL.Rows)
            {
                list.Add(new SelectListItem
                {
                    Text = dr["ChurchName"].ToString(),
                    Value = dr["ChurchId"].ToString()
                });
            }
            return list;
        }
    }
}
