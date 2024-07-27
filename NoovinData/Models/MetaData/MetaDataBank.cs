using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NoovinData.Models.Domin
{
    internal class MetaDataBank
    {
        [Display(Name = "شناسه بانکی")]
        public int Banks_ID { get; set; }

        [StringLength(16, ErrorMessage = "مقدار وارد شده بیش از 16 کاراکتر است")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا مقدار را وارد کنید")]
        [Display(Name = "شماره کارت")]
        public string Banks_NoCart { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا مقدار را وارد کنید")]
        [Display(Name = "شماره شبا")]
        public string Banks_NoIR { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا مقدار را وارد کنید")]
        [Display(Name = "شماره حساب")]
        public string Banks_NoBank { get; set; }

         [Display(Name = "نام بانک")]
        public int Banks_NameID { get; set; }

         [Display(Name = "نام کاربر ")]
        public int Banks_UserID { get; set; }

         [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا مقدار را وارد کنید")]
         [Display(Name = "صاحب حساب")]
        public string Banks_NameUser { get; set; }

        public virtual BankName BankName { get; set; }
        public virtual User User { get; set; }

    }
    [MetadataType(typeof(MetaDataBank))]
      public partial class NoBank
      {

      }
}