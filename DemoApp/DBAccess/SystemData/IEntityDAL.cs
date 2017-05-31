using Model.SystemModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBAccess.SystemData
{
    interface IEntityDAL<T>
    {
        string Create(T obj);
        bool Delete(T obj);
        T Fetch(string ID);
        void Update(T obj);
        Dictionary<string, T> Search(SearchCondition Condition);
    }
}
