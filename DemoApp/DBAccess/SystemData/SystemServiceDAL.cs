using Model.SystemModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBAccess.SystemData
{
     public class SystemServiceDAL
    {
         public string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
         public  bool LogIn(string UserName, string Password="Inite")
         {
             Crypto crypt = new Crypto();
             bool IsLogin = false;

             IDataReader datareader = null;
             SqlConnection con = new SqlConnection(ConnectionString);
             SqlCommand cmd = new SqlCommand();
             cmd.Connection = con;

             try
             {
                
                 cmd.CommandText = string.Format(@"SELECT * FROM Users where Email='{0}' AND Password='{1}' and Status='Active'", UserName, crypt.psEncrypt(Password));
                 
                 con.Open();
                 datareader = cmd.ExecuteReader();
                 if (datareader.Read())
                 {
                     IsLogin = true;
                     UserIdentity.UserID = datareader["UserID"].ToString();
                     UserIdentity.Address = datareader["Address"].ToString();
                     UserIdentity.ContactPerson = datareader["ContactPerson"].ToString();
                     UserIdentity.Country = datareader["Country"].ToString();
                     UserIdentity.Email = datareader["Email"].ToString();
                     UserIdentity.FirstName = datareader["FirstName"].ToString();
                     UserIdentity.LastName = datareader["LastName"].ToString();
                     UserIdentity.Organization = datareader["Organization"].ToString();
                     UserIdentity.ServiceStartDate =Convert.ToDateTime(datareader["ServiceStartDate"]);
                     UserIdentity.Status = (Status)Enum.Parse(typeof(Status), datareader["Status"].ToString(), true);
                     UserIdentity.Telephone = datareader["Telephone"].ToString();
                     UserIdentity.UserGroup = (UserGroup)Enum.Parse(typeof(UserGroup), datareader["UserGroup"].ToString(), true);
                     UserIdentity.ProfileImage = datareader["ProfileImage"].ToString();
                 }
             }
             catch (DbException dbException)
             {
                 throw new Exception("Exception while Connecting to DB", dbException);
             }
             catch (ArgumentException Exception)
             {
                 throw new Exception("Exception while getting Data", Exception);
             }
             finally
             {
                 con.Close();
             }

             return IsLogin;
         }
    }
}
