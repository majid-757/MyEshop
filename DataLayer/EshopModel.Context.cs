﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataLayer
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MyEshop_DBEntities : DbContext
    {
        public MyEshop_DBEntities()
            : base("name=MyEshop_DBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Product_Groups> Product_Groups { get; set; }
        public virtual DbSet<Product_Galleries> Product_Galleries { get; set; }
        public virtual DbSet<Product_Selected_Groups> Product_Selected_Groups { get; set; }
        public virtual DbSet<Product_Tags> Product_Tags { get; set; }
        public virtual DbSet<Products> Products { get; set; }
    }
}
