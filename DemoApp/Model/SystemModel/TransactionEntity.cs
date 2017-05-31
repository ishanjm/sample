using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SystemModel
{
    public class TransactionEntity
    {
        public string ID { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime DeletedOn { get; set; }
        public DateTime EditedOn { get; set; }
        public static Status Status { get; set; }
    }
}
