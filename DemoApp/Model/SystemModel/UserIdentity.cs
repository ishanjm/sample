using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Model.SystemModel
{
    public static class UserIdentity
    {
        public static string UserID { get; set; }
        public static string FirstName { get; set; }
        public static string LastName { get; set; }
        public static UserGroup UserGroup { get; set; }
        public static string Organization { get; set; }
        public static string ContactPerson { get; set; }
        public static string Country { get; set; }
        public static string Address { get; set; }
        public static string Telephone { get; set; }
        public static string Email { get; set; }
        public static DateTime ServiceStartDate { get; set; }
        public static Status Status { get; set; }

        public static string ProfileImage { get; set; }
    }
    public enum UserGroup
    {
        Admin,
        StaffMember,
        Sponsor,
        Supervisor
    }
    public enum Status
    {
        Active,
        Deleted,
        Approve,
        [Description("Not Approve")]
        NotApprove,
        Pending
    }
}
