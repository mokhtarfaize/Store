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
    
    public partial class Withdrawal
    {
        public int Withdrawal_ID { get; set; }
        public System.DateTime Withdrawal_TimeLastReceived { get; set; }
        public int Withdrawal_Stock { get; set; }
        public int Withdrawal_LastReceivd { get; set; }
        public bool Withdrawal_Request { get; set; }
        public int Withdrawal_AmountRequested { get; set; }
        public int Withdrawal_UserID { get; set; }
    
        public virtual User User { get; set; }
    }
}
