using Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MemberDAL
    {
        private string _strConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        public string CreateMember(Member member)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = _strConnectionString;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            try
            {

                cmd.CommandText = string.Format(@"Insert into Member " +
                    "(Name,Address,Dob)OUTPUT Inserted.ID " +
                    "values(@Name,@Address,@Dob)");

                if (!string.IsNullOrEmpty(member.Address))
                {
                    cmd.Parameters.AddWithValue("@Address", member.Address);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Address", DBNull.Value);
                }

                if (!string.IsNullOrEmpty(member.Name))
                {
                    cmd.Parameters.AddWithValue("@Name", member.Name);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                }

                cmd.Parameters.AddWithValue("@Dob", member.Dob);

                con.Open();
                string ID = cmd.ExecuteScalar().ToString();
                return ID;
            }
            catch (DbException dbException)
            {
                throw new Exception("Exception while Connecting to DB", dbException);
            }
            catch (ArgumentException Exception)
            {
                throw new Exception("Exception while inserting LoginDetails Data", Exception);
            }
            finally
            {
                con.Close();
            }
        }

        public List<Member> GetMemberList()
        {
            List<Member> MemberList = new List<Member>();
            SqlConnection con = new SqlConnection(_strConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = string.Format(@"Select * from Member ");
            try
            {
                con.Open();
                IDataReader datareader = cmd.ExecuteReader();
                while (datareader.Read())
                {
                    Member pc = ReadMember(datareader);
                    MemberList.Add(pc);
                }

            }
            catch (Exception e)
            {

            }
            finally
            {
                con.Close();
            }
            return MemberList;
        }

        private Member ReadMember(IDataReader datareader)
        {
            Member MemberObj = new Member();
            if (datareader["ID"] != DBNull.Value)
            {
                MemberObj.ID = datareader["ID"].ToString();
            }
            if (datareader["Name"] != DBNull.Value)
            {
                MemberObj.Name = datareader["Name"].ToString();
            }
            if (datareader["Address"] != DBNull.Value)
            {
                MemberObj.Address = datareader["Address"].ToString();
            }
            if (datareader["Address"] != DBNull.Value)
            {
                MemberObj.Dob =Convert.ToDateTime(datareader["Dob"]);
            }
            return MemberObj;
        }
    }
}
