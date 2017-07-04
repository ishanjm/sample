using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Member
    {
        public string ID { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is Required")]
        public string Address { get; set; }


        [DisplayName("Date of Birth")]
        [Required(ErrorMessage = "Date of Birth is Required")]
        public DateTime Dob { get; set; }
    }
}
