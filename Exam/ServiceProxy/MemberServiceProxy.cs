using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ServiceProxy
{
    public static class MemberServiceProxy
    {
        public static string CreateMember(Member MomberModel)
        {
            string ID = string.Empty;
            try
            {
                TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted });
                using (transaction)
                {
                    MemberDAL DAL = new MemberDAL();
                    ID = DAL.CreateMember(MomberModel);
                    transaction.Complete();
                }
            }
            catch (DbException dbexception)
            {
                throw new Exception("Exception while Inserting Data", dbexception);
            }

            catch (ArgumentException argumentException)
            {
                throw new Exception("Exception while Inserting Data", argumentException);
            }
            return ID;
        }

        public static List<Member> GetMemberList()
        {
            MemberDAL DAL = new MemberDAL();
            return DAL.GetMemberList();
        }
    }
}
