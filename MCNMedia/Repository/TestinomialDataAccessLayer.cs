using MCNMedia_Dev.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Repository
{
    public class TestinomialDataAccessLayer
    {
        AwesomeDal.DatabaseConnect _dc;

        public TestinomialDataAccessLayer()
        {
            _dc = new AwesomeDal.DatabaseConnect();
        }

        //public IEnumerable<Testinomial> GetTestinomials()
        //{
        //    List<Testinomial> testinomials = new List<Testinomial>();
        //    _dc.ClearParameters();
        //    DataTable dataTable = _dc.ReturnDataTable("spTestinomial_Select_forWebsite");
        //    foreach (DataRow dataRow in dataTable.Rows)
        //    {
        //        Testinomial testinomial = new Testinomial();
        //    }

        //    return testinomials;
        //} 
    }
}
