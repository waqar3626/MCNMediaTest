using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev._Helper
{
    public class ForgetPassword
    {

        public string GeneratePassword()
        {
            string PasswordLength = "15";
            string NewPassword = "";

            string allowedChars = "";

            allowedChars = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";
            allowedChars += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";


            char[] sep = { ',' };
            string[] arr = allowedChars.Split(sep);
            string IDString = "";
            string temp = "";
            Random rand = new Random();
            for (int i = 0; i < Convert.ToInt32(PasswordLength); i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                IDString += temp;
                NewPassword = IDString;
            }
            return NewPassword;
        }

        public string EncodeDataToBase64(string Data)
        {
           
                byte[] encData_byte = new byte[Data.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(Data);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
           
        } //this function Convert to Decord your Password
        public string DecodeDataFrom64(string encodedData)
        {
            
                if (encodedData.Contains("="))
                {
                    System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                    System.Text.Decoder utf8Decode = encoder.GetDecoder();
                    byte[] todecode_byte = Convert.FromBase64String(encodedData);
                    int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                    char[] decoded_char = new char[charCount];
                    utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                    string result = new String(decoded_char);
                    return result;
                }
                else
                {
                    return encodedData;
                }
           
        }

    }
}
