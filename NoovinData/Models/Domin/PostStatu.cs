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
    
    public partial class PostStatu
    {
        public PostStatu()
        {
            this.Bills = new HashSet<Bill>();
        }
    
        public int St_ID { get; set; }
        public string St_Name { get; set; }
    
        public virtual ICollection<Bill> Bills { get; set; }
    }
}
