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
    
    public partial class Role
    {
        public Role()
        {
            this.Users = new HashSet<User>();
        }
    
        public int Role_ID { get; set; }
        public string Role_Name { get; set; }
        public string Role_Title { get; set; }
    
        public virtual ICollection<User> Users { get; set; }
    }
}
