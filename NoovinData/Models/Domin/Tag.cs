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
    
    public partial class Tag
    {
        public int Tag_ID { get; set; }
        public string Tag_Name { get; set; }
        public int Tag_ProductID { get; set; }
    
        public virtual Product Product { get; set; }
    }
}
