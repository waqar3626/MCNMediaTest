using MCNMedia_Dev._Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev.Repository
{
    public static class ActivityLogDataAccessLayer
    {
        private static AwesomeDal.DatabaseConnect _dc;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// This method used to log an activity or operation performed against an object.
        /// </summary>
        /// <param name="operation">Name of an operation</param>
        /// <param name="category">Name of category/object aginst activity or operation performed</param>
        /// <param name="message">Message to be logged</param>
        /// <param name="churchId">Church ID</param>
        /// <param name="userId">User ID</param>
        public static void AddActivityLog(Operation operation, Categories category, string message, int churchId, int userId)
        {
            _dc = new AwesomeDal.DatabaseConnect();
            _dc.ClearParameters();
            _dc.AddParameter("Operation", operation.ToString());
            _dc.AddParameter("Category", category.ToString());
            _dc.AddParameter("Message", message);
            _dc.AddParameter("ChurchId", churchId);
            _dc.AddParameter("UserId", userId);
            _dc.Execute("spActivityLog_Insert");
        }
    }
}
