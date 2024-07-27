using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoovinData.Models.Domin
{
    internal class MetaDataProduct
    {
        public int Product_ID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا مقدار را وارد نمایید")]
        [Display(Name = "عنوان ")]
        [StringLength(100, ErrorMessage = "مقدار وارد شده بیش 100 کارکتراست")]
        public string Product_Name { get; set; }


        [AllowHtml]
        [Display(Name = "توضیحات ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string Product_Text { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا مقدار را وارد نمایید")]

        [Display(Name = "قیمت - تومان ")]

        public int Product_Price { get; set; }

        [Display(Name = "تخفیف محصول")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا حداقل  مقدار 0 را وارد نمایید")]

        public Nullable<int> Product_Off { get; set; }

        [Display(Name = "تصویر ")]

        public string Product_PicIndex { get; set; }


        [Display(Name = "دانلودی بودن ")]

        public bool Product_IsDownload { get; set; }


        [Display(Name = "زمان ثبت ")]

        public System.DateTime Product_Date { get; set; }


        [Display(Name = "دسته ")]

        public int Product_GroupID { get; set; }



        [Display(Name = "تخفیف مناسبتی ")]

        public bool Product_AllOff { get; set; }



        [Display(Name = "بازدید ")]

        public int Product_Visit { get; set; }


        [Display(Name = "موجودی ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا حداقل  مقدار 1 را وارد نمایید")]
        public Nullable<int> Product_ExitCount { get; set; }

        //[MinLength(10 , ErrorMessage="حداقل وزن 10 گرم")]
        [Display(Name = "وزن ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا حداقل مقدار  10 گرم را وارد نمایید")]
        public Nullable<int> Product_Weight { get; set; }


        [Display(Name = "فروشنده محصول")]

        public int Product_UserID { get; set; }

        [Display(Name = "نمایش محصول")]
        public bool Product_Active { get; set; }
    }

    [MetadataType(typeof(MetaDataProduct))]
    public partial class Product
    {


    }
}