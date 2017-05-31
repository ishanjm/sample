using DBAccess.SystemData;
using Model.MasterData;
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

namespace DBAccess.MasterDataDAL
{
    public class UsersDAL : IEntityDAL<UserModel>
    {
        public string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        public string Create(UserModel obj)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            Crypto crypt = new Crypto();
            string ID = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                if (!string.IsNullOrEmpty(obj.ProfileImage))
                {
                    cmd.CommandText = "INSERT INTO Users (OrphanCount,OrphanPrefix,FirstName,LastName,UserGroup,Organization,ContactPerson,Country,Address,Telephone,Email,ServiceStartDate,Status,Password,ProfileImage)OUTPUT Inserted.UserID " +
                                   " VALUES (@OrphanCount,@OrphanPrefix,@FirstName,@LastName,@UserGroup,@Organization,@ContactPerson,@Country,@Address,@Telephone,@Email,@ServiceStartDate,@Status,@Password,@ProfileImage)";

                }
                else
                {
                    cmd.CommandText = "INSERT INTO Users (OrphanCount,OrphanPrefix,FirstName,LastName,UserGroup,Organization,ContactPerson,Country,Address,Telephone,Email,ServiceStartDate,Status,Password)OUTPUT Inserted.UserID " +
                                                      " VALUES (@OrphanCount,@OrphanPrefix,@FirstName,@LastName,@UserGroup,@Organization,@ContactPerson,@Country,@Address,@Telephone,@Email,@ServiceStartDate,@Status,@Password)";
                }
                cmd.Parameters.AddWithValue("@OrphanCount", obj.OrphanCount);
                if (!string.IsNullOrEmpty(obj.OrphanPrefix))
                {
                    cmd.Parameters.AddWithValue("@OrphanPrefix", obj.OrphanPrefix.ToUpper());
                }
                else
                {
                    cmd.Parameters.AddWithValue("@OrphanPrefix", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(obj.FirstName))
                {
                    cmd.Parameters.AddWithValue("@FirstName", obj.FirstName);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@FirstName", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(obj.LastName))
                {
                    cmd.Parameters.AddWithValue("@LastName", obj.LastName);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@LastName", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(obj.Organization))
                {
                    cmd.Parameters.AddWithValue("@Organization", obj.Organization);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Organization", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(obj.ContactPerson))
                {
                    cmd.Parameters.AddWithValue("@ContactPerson", obj.ContactPerson);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ContactPerson", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(obj.Country))
                {
                    cmd.Parameters.AddWithValue("@Country", obj.Country);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Country", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(obj.Address))
                {
                    cmd.Parameters.AddWithValue("@Address", obj.Address);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Address", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(obj.Telephone))
                {
                    cmd.Parameters.AddWithValue("@Telephone", obj.Telephone);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Telephone", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(obj.Email))
                {
                    cmd.Parameters.AddWithValue("@Email", obj.Email);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(obj.ProfileImage))
                {
                    cmd.Parameters.AddWithValue("@ProfileImage", obj.ProfileImage);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ProfileImage", DBNull.Value);
                }

                cmd.Parameters.AddWithValue("@UserGroup", obj.UserGroup.ToString());
                cmd.Parameters.AddWithValue("@ServiceStartDate", obj.ServiceStartDate);
                cmd.Parameters.AddWithValue("@Status", obj.Status.ToString());
                cmd.Parameters.AddWithValue("@Password", crypt.psEncrypt(obj.Password));

                con.Open();
                ID = cmd.ExecuteScalar().ToString();
            }
            catch (DbException dbException)
            {
                throw new Exception("Exception while getiing Data", dbException);
            }
            catch (ArgumentException argumentException)
            {
                throw new Exception("Exception while getiing Data", argumentException);
            }
            finally
            {
                con.Close();
            }
            return ID;
        }

        public bool Delete(UserModel obj)
        {
            throw new NotImplementedException();
        }

        public UserModel Fetch(string ID)
        {
            UserModel User = new UserModel();
            IDataReader datareader = null;
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            try
            {
                cmd.CommandText = string.Format(@"select * from Users where UserID='{0}'", ID);
                con.Open();
                datareader = cmd.ExecuteReader();
                if (datareader.Read())
                {
                    User = ReadUserMaster(datareader);
                    con.Close();
                   // ReadOrphan(User);
                }
            }
            catch (DbException dbException)
            {
                throw new Exception("Exception while Connecting to DB", dbException);
            }
            catch (ArgumentException Exception)
            {
                throw new Exception("Exception while getting  Data", Exception);
            }
            finally
            {
                con.Close();
            }
            return User;
        }

        //private void ReadOrphan(UserModel User)
        //{
        //    IDataReader datareader = null;
        //    SqlConnection con = new SqlConnection(ConnectionString);
        //    SqlCommand cmd = new SqlCommand();
        //    OrphanDAL orphandata = new OrphanDAL();
        //    cmd.Connection = con;
        //    try
        //    {
        //        if (User.UserGroup==UserGroup.StaffMember)
        //        {
        //            cmd.CommandText = string.Format(@"select Orphan.*,CONCAT(Users.FirstName,' ',Users.LastName) as SupervisorName from Orphan left join Users on Users.UserID=Orphan.SupervisorID where SupervisorID='{0}' and Orphan.Status !='Deleted' ", User.UserID);
        //        }
        //        else if (User.UserGroup==UserGroup.Supervisor)
        //        {
        //            cmd.CommandText = string.Format(@"select Orphan.*,CONCAT(Users.FirstName,' ',Users.LastName) as SupervisorName from Orphan left join Users on Users.UserID=Orphan.SupervisorID where SupervisorID='{0}' and Orphan.Status !='Deleted' ", User.UserID);
        //        }
        //        else
        //        {
        //            cmd.CommandText = string.Format(@"select Orphan.*,CONCAT(Users.FirstName,' ',Users.LastName) as SupervisorName from Orphan left join Users on Users.UserID=Orphan.SupervisorID where SponserID='{0}' and Orphan.Status !='Deleted' ", User.UserID);
        //        }
        //        con.Open();
        //        datareader = cmd.ExecuteReader();
        //        while (datareader.Read())
        //        {
        //           OrphanModel Orphan = orphandata.ReadOrphanMaster(datareader);
        //           User.OrphanList.Add(Orphan);
        //        }
        //    }
        //    catch (DbException dbException)
        //    {
        //        throw new Exception("Exception while Connecting to DB", dbException);
        //    }
        //    catch (ArgumentException Exception)
        //    {
        //        throw new Exception("Exception while getting  Data", Exception);
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //}

        public void Update(UserModel obj)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            try
            {
                if (obj.ProfileImage != null)
                {
                    cmd.CommandText = string.Format(@"UPDATE Users SET "
                                                 + "FirstName= @FirstName"
                                                 + ",LastName= @LastName"
                                                 + ",UserGroup= @UserGroup "
                                                 + ",Organization= @Organization "
                                                 + ",ContactPerson= @ContactPerson "
                                                 + ",Country= @Country "
                                                 + ",Address= @Address "
                                                 + ",Telephone= @Telephone "
                                                 + ",Email= @Email "
                                                 + ",ServiceStartDate= @ServiceStartDate "
                                                 + ",Status= @Status "
                                                 + ",OrphanPrefix= @OrphanPrefix "
                                                 + ",ProfileImage= @ProfileImage "
                                                 + ",OrphanCount= @OrphanCount "
                                                 + "WHERE UserID=@UserID");

                    if (obj.ProfileImage != null)
                    {
                        cmd.Parameters.AddWithValue("@ProfileImage", obj.ProfileImage);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ProfileImage", DBNull.Value);
                    }
                }
                else
                {
                    cmd.CommandText = string.Format(@"UPDATE Users SET "
                                                  + "FirstName= @FirstName"
                                                  + ",LastName= @LastName"
                                                  + ",UserGroup= @UserGroup "
                                                  + ",Organization= @Organization "
                                                  + ",ContactPerson= @ContactPerson "
                                                  + ",Country= @Country "
                                                  + ",Address= @Address "
                                                  + ",Telephone= @Telephone "
                                                  + ",Email= @Email "
                                                  + ",ServiceStartDate= @ServiceStartDate "
                                                  + ",OrphanPrefix= @OrphanPrefix "
                                                  + ",OrphanCount= @OrphanCount "
                                                  + ",Status= @Status "
                                                  + "WHERE UserID=@UserID");

                }
                cmd.Parameters.AddWithValue("@OrphanCount", obj.OrphanCount);
                if (!string.IsNullOrEmpty(obj.OrphanPrefix))
                {
                    cmd.Parameters.AddWithValue("@OrphanPrefix", obj.OrphanPrefix.ToUpper());
                }
                else
                {
                    cmd.Parameters.AddWithValue("@OrphanPrefix", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(obj.FirstName))
                {
                    cmd.Parameters.AddWithValue("@FirstName", obj.FirstName);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@FirstName", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(obj.LastName))
                {
                    cmd.Parameters.AddWithValue("@LastName", obj.LastName);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@LastName", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(obj.Organization))
                {
                    cmd.Parameters.AddWithValue("@Organization", obj.Organization);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Organization", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(obj.ContactPerson))
                {
                    cmd.Parameters.AddWithValue("@ContactPerson", obj.ContactPerson);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ContactPerson", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(obj.Country))
                {
                    cmd.Parameters.AddWithValue("@Country", obj.Country);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Country", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(obj.Address))
                {
                    cmd.Parameters.AddWithValue("@Address", obj.Address);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Address", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(obj.Telephone))
                {
                    cmd.Parameters.AddWithValue("@Telephone", obj.Telephone);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Telephone", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(obj.Email))
                {
                    cmd.Parameters.AddWithValue("@Email", obj.Email);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                }

                cmd.Parameters.AddWithValue("@UserGroup", obj.UserGroup.ToString());
                cmd.Parameters.AddWithValue("@ServiceStartDate", obj.ServiceStartDate);
                cmd.Parameters.AddWithValue("@UserID", obj.UserID);
                cmd.Parameters.AddWithValue("@Status", obj.Status.ToString());
                con.Open();
                cmd.ExecuteNonQuery();

            }
            catch (DbException dbException)
            {
                throw new Exception("Exception while Connecting to DB", dbException);
            }
            catch (ArgumentException Exception)
            {
                throw new Exception("Exception while inserting  Data", Exception);
            }
            finally
            {
                con.Close();
            }
        }

        public Dictionary<string, UserModel> Search(SearchCondition Condition)
        {
            Dictionary<string, UserModel> UserList = new Dictionary<string, UserModel>();
            IDataReader datareader = null;
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            try
            {
                string orderStr = "order by Name";
                StringBuilder innerFilter = new StringBuilder();
                StringBuilder outerFilter = new StringBuilder();

                if (Condition != null)
                {
                    if ((Condition.RecordEnd > 0 && Condition.RecordStart >= 0) && (Condition.RecordStart < Condition.RecordEnd))
                    {
                        if (!string.IsNullOrEmpty(Condition.searchCond))
                        {
                            outerFilter.AppendFormat(Condition.searchCond + " AND ");
                        }
                        outerFilter.AppendFormat("RowNumber between {0} and {1}", Condition.RecordStart, Condition.RecordEnd);
                    }
                    if (!string.IsNullOrEmpty(Condition.searchCondColFilter))
                    {
                        innerFilter.Append("WHERE " + Condition.searchCondColFilter);
                    }
                    if (!string.IsNullOrEmpty(Condition.SortExpression))
                    {
                        orderStr = Condition.SortExpression;
                    }
                }

                cmd.CommandText = string.Format(@"WITH CTEUsers AS " +
                                                "( " +
                                                    " SELECT *,concat(FirstName,' ',LastName) as Name, row_number() OVER ({0}) AS RowNumber " +
                                                    " FROM Users {1} " +
                                                " ) " +
                                                " SELECT *,(select MAX(RowNumber) from CTEUsers) as TotalRows FROM CTEUsers WHERE {2} ", orderStr, innerFilter, outerFilter);
                con.Open();
                datareader = cmd.ExecuteReader();
                while (datareader.Read())
                {
                    Condition.TotalCount = Convert.ToInt32(datareader["TotalRows"]);
                    UserModel User = ReadUserMaster(datareader);
                    UserList.Add(User.UserID, User);
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
            return UserList;
        }

        private UserModel ReadUserMaster(IDataReader datareader)
        {
            UserModel User = new UserModel();
            try
            {
                User.DisplayName = datareader["DisplayName"].ToString();
            }
            catch (Exception)
            {
               
            }
            if (datareader["Address"] != DBNull.Value)
            {
                User.Address = datareader["Address"].ToString();
            }
            if (datareader["OrphanCount"] != DBNull.Value)
            {
                User.OrphanCount = Convert.ToInt32(datareader["OrphanCount"]);
            }
            if (datareader["OrphanPrefix"] != DBNull.Value)
            {
                User.OrphanPrefix = datareader["OrphanPrefix"].ToString();
            }
            if (datareader["ContactPerson"] != DBNull.Value)
            {
                User.ContactPerson = datareader["ContactPerson"].ToString();
            }
            if (datareader["Country"] != DBNull.Value)
            {
                User.Country = datareader["Country"].ToString();
            }
            if (datareader["Email"] != DBNull.Value)
            {
                User.Email = datareader["Email"].ToString();
            }
            if (datareader["FirstName"] != DBNull.Value)
            {
                User.FirstName = datareader["FirstName"].ToString();
            }
            if (datareader["FirstName"] != DBNull.Value && datareader["LastName"] != DBNull.Value)
            {
                User.FullName = datareader["FirstName"].ToString() + " " + datareader["LastName"].ToString();
            }
            if (datareader["FirstName"] != DBNull.Value && datareader["LastName"] == DBNull.Value)
            {
                User.FullName = datareader["FirstName"].ToString();
            }
            if (datareader["LastName"] != DBNull.Value)
            {
                User.LastName = datareader["LastName"].ToString();
            }
            if (datareader["Organization"] != DBNull.Value)
            {
                User.Organization = datareader["Organization"].ToString();
            }
            if (datareader["ServiceStartDate"] != DBNull.Value)
            {
                User.ServiceStartDate = Convert.ToDateTime(datareader["ServiceStartDate"]);
            }
            if (datareader["Telephone"] != DBNull.Value)
            {
                User.Telephone = datareader["Telephone"].ToString();
            }
            if (datareader["UserGroup"] != DBNull.Value)
            {
                User.UserGroup = (UserGroup)Enum.Parse(typeof(UserGroup), datareader["UserGroup"].ToString(), true);
            }
            if (datareader["UserID"] != DBNull.Value)
            {
                User.UserID = datareader["UserID"].ToString();
            }
            if (datareader["ProfileImage"] != DBNull.Value)
            {
                User.ProfileImage = datareader["ProfileImage"].ToString();
            }
            if (datareader["Status"] != DBNull.Value)
            {
                User.Status = (Status)Enum.Parse(typeof(Status), datareader["Status"].ToString(), true);
            }
            return User;
        }

        public List<UserModel> SearchSponser(string Query)
        {
            List<UserModel> UserList = new List<UserModel>();
            IDataReader datareader = null;
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            try
            {
                cmd.CommandText = string.Format(@"select * from Users where FirstName like '%{0}%' 
                                                    or LastName like '%{0}%'
                                                    or UserGroup like '%{0}%'
                                                    or Organization like '%{0}%'
                                                    or ContactPerson like '%{0}%'
                                                    or Country like '%{0}%'
                                                    or Address like '%{0}%'
                                                    or Telephone like '%{0}%'
                                                    or Email like '%{0}%'
                                                    or ServiceStartDate like '%{0}%' ", Query);
                con.Open();
                datareader = cmd.ExecuteReader();
                while (datareader.Read())
                {
                    UserModel User = ReadUserMaster(datareader);
                    UserList.Add(User);
                }
            }
            catch (DbException dbException)
            {
                throw new Exception("Exception while Connecting to DB", dbException);
            }
            catch (ArgumentException Exception)
            {
                throw new Exception("Exception while getting  Data", Exception);
            }
            finally
            {
                con.Close();
            }
            return UserList;
        }

        public List<OrphanModel> SearchOrphan(string Query)
        {
            List<OrphanModel> OrphanList = new List<OrphanModel>();
            IDataReader datareader = null;
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            try
            {
                cmd.CommandText = string.Format(@"select Orphan.*,CONCAT(Users.FirstName,' ',Users.LastName) as SupervisorName from Orphan inner join Users on Users.UserID=Orphan.SupervisorID where FullName like '%{0}%'
                                                    or Gender like '%{0}%'
                                                    or DOB like '%{0}%'
                                                    or BirthCertificateNo like '%{0}%'
                                                    or Orphan.Address like '%{0}%'
                                                    or PostalCode like '%{0}%'
                                                    or Province like '%{0}%'
                                                    or Religion like '%{0}%'
                                                    or FatherName like '%{0}%'
                                                    or FatherDeathDate like '%{0}%'
                                                    or FatherDeathReason like '%{0}%'
                                                    or FatherProfession like '%{0}%'
                                                    or MotherName like '%{0}%'
                                                    or MotherDeathDate like '%{0}%'
                                                    or MotherDeathReason like '%{0}%'
                                                    or MotherProfession like '%{0}%'
                                                    or GuardianName like '%{0}%'
                                                    or GuardianProfession like '%{0}%'
                                                    or GuardianRelationship like '%{0}%'
                                                    or GuardianAddress like '%{0}%'
                                                    or EducationalStatus like '%{0}%'
                                                    or Sports like '%{0}%'
                                                    or ExtActivities like '%{0}%'
                                                    or Achievemnts like '%{0}%'
                                                    or ACNO like '%{0}%'
                                                    or ACCode like '%{0}%'
                                                    or BankName like '%{0}%'
                                                    or Amount like '%{0}%'
                                                    or SponserID like '%{0}%'
                                                    or Orphan.Status like '%{0}%'
                                                    or FamilyName like '%{0}%'
                                                    or GuardianNIC like '%{0}%' ", Query);
                con.Open();
                datareader = cmd.ExecuteReader();
                while (datareader.Read())
                {
                   // OrphanDAL OrphanDAL = new OrphanDAL();
                   // OrphanModel User =OrphanDAL.ReadOrphanMaster(datareader);
                   // OrphanList.Add(User);
                }
            }
            catch (DbException dbException)
            {
                throw new Exception("Exception while Connecting to DB", dbException);
            }
            catch (ArgumentException Exception)
            {
                throw new Exception("Exception while getting  Data", Exception);
            }
            finally
            {
                con.Close();
            }
            return OrphanList;
        }

        public List<UserModel> SearchSponserList(string Query)
        {
            List<UserModel> UserData = new List<UserModel>();
            IDataReader datareader = null;
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            try
            {
                cmd.CommandText = string.Format(@"select *,CONCAT(FirstName,' ',LastName) as DisplayName from Users where {0}", Query);
                con.Open();
                datareader = cmd.ExecuteReader();
                while (datareader.Read())
                {
                    UserModel User = ReadUserMaster(datareader);
                    UserData.Add(User);
                }
            }
            catch (DbException dbException)
            {
                throw new Exception("Exception while Connecting to DB", dbException);
            }
            catch (ArgumentException Exception)
            {
                throw new Exception("Exception while getting  Data", Exception);
            }
            finally
            {
                con.Close();
            }
            return UserData;
        }

        public List<OrphanModel> SearchOrphanList(string Query)
        {
            List<OrphanModel> OrpanData = new List<OrphanModel>();
            IDataReader datareader = null;
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            try
            {
                cmd.CommandText = string.Format(@"select Orphan.*,CONCAT(Users.FirstName,' ',Users.LastName) as SupervisorName from Orphan inner join Users on Users.UserID=Orphan.SupervisorID where {0}", Query);
                con.Open();
                datareader = cmd.ExecuteReader();
                while (datareader.Read())
                {
                   // OrphanDAL OrphanDAL = new OrphanDAL();
                   // OrphanModel User = OrphanDAL.ReadOrphanMaster(datareader);
                   // OrpanData.Add(User);
                }
            }
            catch (DbException dbException)
            {
                throw new Exception("Exception while Connecting to DB", dbException);
            }
            catch (ArgumentException Exception)
            {
                throw new Exception("Exception while getting  Data", Exception);
            }
            finally
            {
                con.Close();
            }
            return OrpanData;
        }
    }
}
