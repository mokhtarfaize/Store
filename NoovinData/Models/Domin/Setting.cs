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
    
    public partial class Setting
    {
        public int Setting_ID { get; set; }
        public string Smtp { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Nullable<int> Discount { get; set; }
        public string Keyword { get; set; }
        public string Descrption { get; set; }
        public string DescSite { get; set; }
        public string TitleSite { get; set; }
        public string Title { get; set; }
        public Nullable<int> PageCount { get; set; }
        public System.DateTime DateDeleteBill { get; set; }
    }
}
