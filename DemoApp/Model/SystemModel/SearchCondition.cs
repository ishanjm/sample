using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SystemModel
{
    public class SearchCondition
    {
        public string SortExpression { get; set; }

        public int RecordStart { get; set; }

        public int RecordEnd { get; set; }

        public string searchCond { get; set; }

        public string searchCondColFilter { get; set; }

        public int TotalCount { get; set; }
    }
}
