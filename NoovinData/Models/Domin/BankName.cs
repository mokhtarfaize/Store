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
    
    public partial class BankName
    {
        public BankName()
        {
            this.NoBanks = new HashSet<NoBank>();
        }
    
        public int Bank_ID { get; set; }
        public string Bank_Name { get; set; }
    
        public virtual ICollection<NoBank> NoBanks { get; set; }
    }
}