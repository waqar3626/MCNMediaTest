using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bCrypt = BCrypt.Net.BCrypt;

namespace _Helper
{
    public class Hashing
    {
        private static string GetRandomSalt()
        {
            return bCrypt.GenerateSalt(12);
        }

        public static string HashPassword(string password)
        {
            return bCrypt.HashPassword(password, GetRandomSalt());
        }

        public static Status ValidatePassword(string password, string correctHash)
        {
            Status sts = new Status();
            try
            {
                if (bCrypt.Verify(password, correctHash))
                {
                    sts.Success = true;
                }
                else
                {
                    sts.Success = false;
                    sts.Message = string.Format("Invalid Login Information");
                }
                return sts;
            }
            catch (Exception ex)
            {
                sts.Success = false;
                sts.Message = string.Format("Invalid Login Information");
                return sts;
            }
        }
    }
}
