using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NoovinData.Models.Domin
{
    internal class MetaDataSetting
    {
       
        public int Setting_ID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "مقدار را وارد نمایید")]
        [Display(Name = "Smpt")]
        public string Smtp { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "مقدار را وارد نمایید")]
        [Display(Name = "کلمه عبور ایمیل")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "مقدار را وارد نمایید")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

          [Display(Name = "تخفیف مناسبتی")]
        public Nullable<int> Discount { get; set; }

         [Display(Name = "کلمات کلیدی")]
        public string Keyword { get; set; }

         [Display(Name = "توضیح مختصر سایت")]
        public string Descrption { get; set; }

          [Display(Name = "توضیح مختصر صفحه")]
        public string DescSite { get; set; }

         [Display(Name = "نام صفحه")]
        public string TitleSite { get; set; }

         [Display(Name = "نام سایت")]
        public string Title { get; set; }

         [Display(Name = "تعداد در صفحه")]
        public Nullable<int> PageCount { get; set; }

        [Display(Name = "پاکسازی سبدخرید")]
        public System.DateTime DateDeleteBill { get; set; }
    }

    [MetadataType(typeof(MetaDataSetting))]
    public partial class Setting
    {

    }

}