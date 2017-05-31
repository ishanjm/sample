
using Model.SystemModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MasterData
{
    public class UserModel 
    {
        public UserModel()
        {
            OrphanList = new List<OrphanModel>();
        }
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserGroup UserGroup { get; set; }
        public string Organization { get; set; }
        public string ContactPerson { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime ServiceStartDate { get; set; }
        public Status Status { get; set; }
        public List<OrphanModel> OrphanList { get; set; }
        public string ProfileImage { get; set; }
        public string OrphanPrefix { get; set; }
        public int  OrphanCount { get; set; }

        public string FullName { get; set; }

        public string DisplayName { get; set; }
        public string ID { get; set; }
    }
}
