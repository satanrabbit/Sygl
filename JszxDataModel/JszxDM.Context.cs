﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class jszxEntities : DbContext
    {
        public jszxEntities()
            : base("name=jszxEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<admins_tb> admins_tb { get; set; }
        public DbSet<classtimes_tb> classtimes_tb { get; set; }
        public DbSet<courses_tb> courses_tb { get; set; }
        public DbSet<exprecords_tb> exprecords_tb { get; set; }
        public DbSet<labs_tb> labs_tb { get; set; }
        public DbSet<schedule_tb> schedule_tb { get; set; }
        public DbSet<terms_tb> terms_tb { get; set; }
    }
}
