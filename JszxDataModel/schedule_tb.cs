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
    
    public partial class schedule_tb
    {
        public int ScdID { get; set; }
        public int ScdCrs { get; set; }
        public sbyte ScdWeek { get; set; }
        public sbyte ScdWeekDay { get; set; }
        public int ScdClass { get; set; }
        public int ScdLab { get; set; }
        public int ScdTerm { get; set; }
    
        public virtual classtimes_tb classtimes_tb { get; set; }
        public virtual courses_tb courses_tb { get; set; }
        public virtual labs_tb labs_tb { get; set; }
        public virtual terms_tb terms_tb { get; set; }
    }
}
