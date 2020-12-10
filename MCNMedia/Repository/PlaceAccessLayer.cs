using MCNMedia_Dev.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Repository
{
    /// <summary>
    /// The base class for fetching locations like Country and County.
    /// Containing all methods for getting locations
    /// </summary>
    /// <remarks>
    /// This class can provide a list of countries and counties in a country.
    /// </remarks>
    public class PlaceAccessLayer
    {
        AwesomeDal.DatabaseConnect _dc;

        public PlaceAccessLayer()
        {
            _dc = new AwesomeDal.DatabaseConnect();
        }

        /// <summary>
        /// Get the list of countries
        /// </summary>
        /// <returns>The list of countries</returns>
        public IEnumerable<Place> GetCountries()
        {
            List<Place> countryLst = new List<Place>();
            _dc.ClearParameters();
            DataTable dataTable = _dc.ReturnDataTable("spCountries_Get");

            foreach (DataRow dataRow in dataTable.Rows)
            {
                Place country = new Place();
                country.PlaceId = Convert.ToInt32(dataRow["CountryId"]);
                country.PlaceName = dataRow["CountryName"].ToString();
                countryLst.Add(country);
            }
            return countryLst;
        } public IEnumerable<Place> GetISOCountries()
        {
            List<Place> countryLst = new List<Place>();
            _dc.ClearParameters();
            DataTable dataTable = _dc.ReturnDataTable("spISOCountry_Get");

            foreach (DataRow dataRow in dataTable.Rows)
            {
                Place country = new Place();
                country.PlaceId = Convert.ToInt32(dataRow["ISOCountryID"]);
                country.PlaceName = dataRow["ISOCountry"].ToString();
                countryLst.Add(country);
            }
            return countryLst;
        }

        /// <summary>
        /// Get the list of counties in a country
        /// </summary>
        /// <param name="country">Integer: the country id.</param>
        /// <returns>The list of counties</returns>
        public IEnumerable<Place> GetCounties(int country)
        {
            List<Place> countyLst = new List<Place>();
            _dc.ClearParameters();
            _dc.AddParameter("cntryId", country);
            DataTable dataTable = _dc.ReturnDataTable("spCounties_Get");

            foreach (DataRow dataRow in dataTable.Rows)
            {
                Place county = new Place();
                county.PlaceId = Convert.ToInt32(dataRow["CountyId"]);
                county.PlaceName = dataRow["CountyName"].ToString();
                countyLst.Add(county);
            }
            return countyLst;
        }

    }
}
