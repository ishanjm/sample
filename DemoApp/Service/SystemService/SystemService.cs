using DBAccess.SystemData;
using Model.SystemModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.SystemService
{
    public class SystemService
    {
        public static bool LogIn(string UserName, String Password)
        {
            bool Status = false;

            SystemServiceDAL SystemDAL = new SystemServiceDAL();
            Status = SystemDAL.LogIn(UserName, Password);
            return Status;
        }
        public static bool IsLoggedIn(string CookieValue)
        {
           
            bool islogin = false;
            if (!string.IsNullOrEmpty(UserIdentity.UserID) && CookieValue == UserIdentity.UserID)
            {
                islogin = true;
            }
            return islogin;
        }
        public static void InitiateLogin(string CookieValue)
        {
            SystemServiceDAL UserDAL = new SystemServiceDAL();
            UserDAL.LogIn(CookieValue);
        }
    }
}
