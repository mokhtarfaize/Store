//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.Bills = new HashSet<Bill>();
            this.ConfirmEmails = new HashSet<ConfirmEmail>();
            this.Downloads = new HashSet<Download>();
            this.Identities = new HashSet<Identity>();
            this.InterimBills = new HashSet<InterimBill>();
            this.Massages = new HashSet<Massage>();
            this.NoBanks = new HashSet<NoBank>();
            this.Products = new HashSet<Product>();
            this.Withdrawals = new HashSet<Withdrawal>();
        }
    
        public int User_ID { get; set; }
        public string User_NameFamily { get; set; }
        public string User_UserName { get; set; }
        public string User_Email { get; set; }
        public string User_Password { get; set; }
        public string User_Address { get; set; }
        public string User_City { get; set; }
        public string User_PostelCode { get; set; }
        public string User_Tel { get; set; }
        public string User_Mobile { get; set; }
        public System.DateTime User_Date { get; set; }
        public bool User_Active { get; set; }
        public int User_Rating { get; set; }
        public int User_RoleID { get; set; }
        public string User_NationalCode { get; set; }
        public int User_StateID { get; set; }
    
        public virtual ICollection<Bill> Bills { get; set; }
        public virtual ICollection<ConfirmEmail> ConfirmEmails { get; set; }
        public virtual ICollection<Download> Downloads { get; set; }
        public virtual ICollection<Identity> Identities { get; set; }
        public virtual ICollection<InterimBill> InterimBills { get; set; }
        public virtual ICollection<Massage> Massages { get; set; }
        public virtual ICollection<NoBank> NoBanks { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual State State { get; set; }
        public virtual ICollection<Withdrawal> Withdrawals { get; set; }
        public virtual Role Role { get; set; }
    }
}