using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Member
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        [DisplayName("Date of Birth")]
        public DateTime Dob { get; set; }
    }
}
