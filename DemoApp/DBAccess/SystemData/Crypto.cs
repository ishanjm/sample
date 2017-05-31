using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DBAccess.SystemData
{
    public class Crypto
    {
        private Byte[] lbtVector = { 240, 3, 45, 29, 0, 76, 173, 59 };
        private String lscryptoKey = "m4N6iC";

        public  string psDecrypt(string sQueryString)
        {
            Byte[] buffer;
            TripleDESCryptoServiceProvider loCryptoClass = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider loCryptoProvider = new MD5CryptoServiceProvider();
            try
            {
                buffer = Convert.FromBase64String(sQueryString);
                loCryptoClass.Key = loCryptoProvider.ComputeHash(ASCIIEncoding.ASCII.GetBytes(lscryptoKey));
                loCryptoClass.IV = lbtVector;
                return Encoding.ASCII.GetString(loCryptoClass.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                loCryptoClass.Clear();
                loCryptoProvider.Clear();
                loCryptoClass = null;
                loCryptoProvider = null;
            }
        }

        public string psEncrypt(string sInputVal)
        {
            TripleDESCryptoServiceProvider loCryptoClass = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider loCryptoProvider = new MD5CryptoServiceProvider();
            Byte[] lbtBuffer;
            try
            {
                lbtBuffer = System.Text.Encoding.ASCII.GetBytes(sInputVal);
                loCryptoClass.Key = loCryptoProvider.ComputeHash(ASCIIEncoding.ASCII.GetBytes(lscryptoKey));
                loCryptoClass.IV = lbtVector;
                sInputVal = Convert.ToBase64String(loCryptoClass.CreateEncryptor().TransformFinalBlock(lbtBuffer, 0, lbtBuffer.Length));
                return sInputVal;
            }
            catch (CryptographicException e)
            {
                throw e;
            }
            catch (FormatException fex)
            {
                throw fex;
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                loCryptoClass.Clear();
                loCryptoProvider.Clear();
                loCryptoClass = null;
                loCryptoProvider = null;
            }
        }
    }
}
