using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev._Helper
{
    public class Common
    {

        static AwesomeDal.DatabaseConnect _dc;

        public static bool SaveToXXX(string message)
        {
            //_dc = new AwesomeDal.DatabaseConnect();
            //_dc.ClearParameters();
            //_dc.AddParameter("msg", message);
            //return _dc.ReturnBool("spLog_Add");
            return true;
        }



    }
}
