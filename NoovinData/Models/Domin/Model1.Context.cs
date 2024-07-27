﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NoovinData.Models.Domin
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DB : DbContext
    {
        public DB()
            : base("name=DB")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BankName> BankNames { get; set; }
        public virtual DbSet<Bill> Bills { get; set; }
        public virtual DbSet<ConfirmEmail> ConfirmEmails { get; set; }
        public virtual DbSet<Download> Downloads { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Identity> Identities { get; set; }
        public virtual DbSet<InterimBill> InterimBills { get; set; }
        public virtual DbSet<Massage> Massages { get; set; }
        public virtual DbSet<NoBank> NoBanks { get; set; }
        public virtual DbSet<Pic> Pics { get; set; }
        public virtual DbSet<PostStatu> PostStatus { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<Slider> Sliders { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<TempDL> TempDLs { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Visit> Visits { get; set; }
        public virtual DbSet<Weight> Weights { get; set; }
        public virtual DbSet<Withdrawal> Withdrawals { get; set; }
    }
}