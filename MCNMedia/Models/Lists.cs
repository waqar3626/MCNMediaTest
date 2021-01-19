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
        public static List<SelectListItem> GetCountyList(int id)
        {
            PlaceAccessLayer _placeAccessLayer = new PlaceAccessLayer();
            var counties = _placeAccessLayer.GetCounties(id);
            var list = new List<SelectListItem>();
           
            foreach (Place county in counties)
            {
                list.Add(new SelectListItem
                {
                    Text = county.PlaceName,
                    Value = county.PlaceId.ToString()
                });
            }
            return list;
        }

        public static List<SelectListItem> GetCountriesList()
        {
            PlaceAccessLayer _placeAccessLayer = new PlaceAccessLayer();
            var countries = _placeAccessLayer.GetCountries();
            var list = new List<SelectListItem>();
           
            foreach (Place country in countries)
            {
                list.Add(new SelectListItem
                {
                    Text = country.PlaceName,
                    Value = country.PlaceId.ToString()
                });
            }
            return list;
        }

        public static List<SelectListItem> GetClientTypeList()
        {
            ChurchDataAccessLayer churchDataAccess = new ChurchDataAccessLayer();
            var clientTypes = churchDataAccess.GetClientTypeList();
            var list = new List<SelectListItem>();
            
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
        public static List<SelectListItem> GetUsers()
        {
            UserAssignChurchesDataAccessLayer userAssignChurchesDataAccess = new UserAssignChurchesDataAccessLayer();
            var userDDl = userAssignChurchesDataAccess.GetUserDDL();
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem
            {
                Text = "--Select--",
                Value = "0"
            });
            foreach (DataRow dr in userDDl.Rows)
            {
                list.Add(new SelectListItem
                {
                    Text = dr["FirstName"].ToString()+ " " + dr["LastName"].ToString() ,
                    Value = dr["UserId"].ToString(),
                   
                });
            }
            return list;
        }


        public static List<SelectListItem> GetUserAssignChurchListForDDL(int id)
        {
           
            ChurchDataAccessLayer churchDataAccess = new ChurchDataAccessLayer();
            var ChurchDDL = churchDataAccess.GetUserAssignChurchDDL(id);
            var list = new List<SelectListItem>();
            
            foreach (DataRow dr in ChurchDDL.Rows)
            {
                list.Add(new SelectListItem
                {

                Text = dr["ChurchName"].ToString()+", "+ dr["Town"].ToString(),
                 Value = dr["ChurchId"].ToString()

                });
            }
            return list;
        }

        public static List<SelectListItem> GetCameraDDL(int ChurchId)
        {

            ChurchDataAccessLayer churchDataAccess = new ChurchDataAccessLayer();
            var ChurchDDL = churchDataAccess.GetCameraDDL(ChurchId);
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
                    Text = dr["CameraName"].ToString(),
                    Value = dr["CameraId"].ToString()
                });
            }
            return list;
        }
        public static List<SelectListItem> GetPackagesDDL()
        {

            PaymentDataAccessLayer payment= new PaymentDataAccessLayer();
            var PackageDDL = payment.GetPackagesDDL();
            var list = new List<SelectListItem>();

            list.Add(new SelectListItem
            {
                Text = "--Select--",
                Value = "0"
            });
            foreach (DataRow dr in PackageDDL.Rows)
            {

                list.Add(new SelectListItem
                {
                    Text = dr["PackageTitle"].ToString(),
                    Value = dr["PackageId"].ToString()
                });
            }
            return list;
        }
    }
}
