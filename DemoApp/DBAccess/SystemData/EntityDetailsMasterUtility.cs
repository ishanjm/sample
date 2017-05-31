using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBAccess.SystemData
{
    public class EntityDetailsMasterUtility
    {
        public string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

        public bool SetMaxCodeForTenantTransaction(EntityTransactionType transaction, EntityDetailsCode currentCode)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn;

                switch (transaction)
                {
                    case EntityTransactionType.Orphan:
                        cmd.CommandText = string.Format("update EntityDetailsMaster set OrphanValue={0} ", currentCode.Code);
                        break;
                    default:
                        break;
                }

                conn.Open();
                try
                {
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        public EntityDetailsCode GetMaxCodeForTenantTransaction(EntityTransactionType transaction)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                EntityDetailsCode code;
                SqlCommand command = new SqlCommand();
                string prefixCol = string.Empty;
                string maxValCol = string.Empty;
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;

                switch (transaction)
                {
                    case EntityTransactionType.Orphan:
                        prefixCol = "OrphanPrefix";
                        maxValCol = "OrphanValue";
                        command.CommandText = String.Format("Select {0},{1} from EntityDetailsMaster ", prefixCol, maxValCol);
                        break;
                   
                    default:
                        break;
                }

                try
                {
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (!reader.HasRows)
                        throw new Exception("Tenant entity details configuration is invalid");
                    else
                    {
                        reader.Read();
                        code = new EntityDetailsCode();
                        code.Prefix = reader[prefixCol].ToString();
                        code.Code = Convert.ToInt32(reader[maxValCol]) + 1;

                        //   code = SetMaxLengthOfCode(code, 5);//5 is maxlength for POS codes
                    }
                    conn.Close();
                }
                catch (Exception)
                {
                    conn.Close();
                    throw;
                }

                return code;
            }
        }
        public struct EntityDetailsCode
        {
            public string Prefix { get; set; }
            public int Code { get; set; }
        }

        public enum EntityTransactionType
        {
            Orphan
        }
    }
}
