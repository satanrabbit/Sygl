//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace JszxDataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class admins_tb
    {
        public admins_tb()
        {
            this.labs_tb = new HashSet<labs_tb>();
        }
    
        public int AdminID { get; set; }
        public string AdminName { get; set; }
        public string AdminAccount { get; set; }
        public string AdminPWD { get; set; }
        public sbyte AdminAuthority { get; set; }
        public sbyte AdminStatus { get; set; }
        public string AdminBackup { get; set; }
    
        public virtual ICollection<labs_tb> labs_tb { get; set; }
    }
}
