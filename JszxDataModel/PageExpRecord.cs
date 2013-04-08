using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JszxDataModel
{
    public class PageExpRecord
    {
        public int Pages { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public List<exprecords_tb> ExpRecordList { get; set; }
    }
}
