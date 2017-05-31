using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.MasterData;
using Model.SystemModel;
using DBAccess.MasterDataDAL;

namespace Service.MasterDataService
{
    public class UserService
    {
        public static Dictionary<string,UserModel> Search(SearchCondition condition)
        {
            UsersDAL DAL = new UsersDAL();
            Dictionary<string, UserModel> UserList = DAL.Search(condition);
            return UserList;
        }

        public static List<UserModel> SearchSponserList(string v)
        {
            throw new NotImplementedException();
        }

        public static void Update(UserModel user)
        {
            throw new NotImplementedException();
        }

        public static void Create(UserModel user)
        {
            throw new NotImplementedException();
        }

        public static void Delete(UserModel user)
        {
            throw new NotImplementedException();
        }

        public static UserModel Fetch(string id)
        {
            throw new NotImplementedException();
        }
    }
}
