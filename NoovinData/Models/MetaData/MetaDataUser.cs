using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoovinData.Models.Domin
{
    internal class MetaDataUser
    {
        [Display(Name = "شناسه کاربری")]
        public int User_ID { get; set; }


        [StringLength(100, ErrorMessage = "مقدار وارد شده بیش 100 کارکتراست")]
        [Display(Name = "نام و نام خانوادگی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string User_NameFamily { get; set; }

        [StringLength(100, ErrorMessage = "مقدار وارد شده بیش 100 کارکتراست")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا مقدار را وارد نمایید")]
        //از طریق جیسون بررسی میکنیم تکراری ثبت نشه
        [Remote("UsernameValid", "Register", HttpMethod = "Post",ErrorMessage="قبلا استفاده شده لطفا مقدار دیگری را  وارد کنید")]

        [Display(Name = "نام کاربری")]
        public string User_UserName { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا مقدار را وارد نمایید")]
        [Display(Name = "پست الکترونیکی")]
        [StringLength(200, ErrorMessage = "مقدار وارد شده بیش 200 کارکتراست")]
        [Remote("EamilValid", "Register", HttpMethod = "Post",ErrorMessage="قبلا استفاده شده لطفا مقدار دیگری را  وارد کنید")]
        [RegularExpression(@"^\w+[\w-\.]*\@\w+((-\w+)|(\w*))\.[a-z]{2,3}$", ErrorMessage = "ایمیل را به درستی وارد نمایید")]
        public string User_Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا مقدار را وارد نمایید")]
        [Display(Name = "کلمه عبور")]
        [StringLength(100, ErrorMessage = "مقدار وارد شده بیش 100 کارکتراست")]
        public string User_Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا مقدار را وارد نمایید")]
        [Display(Name = "آدرس پستی")]
        [StringLength(100, ErrorMessage = "مقدار وارد شده بیش 100 کارکتراست")]
        public string User_Address { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا مقدار را وارد نمایید")]
        [Display(Name = "شهر محل سکونت")]
        [StringLength(100, ErrorMessage = "مقدار وارد شده بیش 100 کارکتراست")]
        public string User_City { get; set; }

        [Display(Name = "کد پستی")]
        [StringLength(10, ErrorMessage = "مقدار وارد شده بیش 10 کارکتراست")]
        public string User_PostelCode { get; set; }


        [Display(Name = "تلفن منزل")]
        [StringLength(11, ErrorMessage = "مقدار وارد شده بیش 11 کارکتراست")]
        public string User_Tel { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا مقدار را وارد نمایید")]
        [Display(Name = "شماره همراه")]
        [Remote("MobileValid", "Register", HttpMethod = "Post",ErrorMessage="قبلا استفاده شده لطفا مقدار دیگری را  وارد کنید")]
        [StringLength(11, ErrorMessage = "مقدار وارد شده بیش 11 کارکتراست")]
        public string User_Mobile { get; set; }


        [Display(Name = "تاریخ ثبت نام")]
        public System.DateTime User_Date { get; set; }

        [Display(Name = "وضعیت دسترسی")]
        public bool User_Active { get; set; }

        [Display(Name = "امتیاز کاربری")]
        public string User_Rating { get; set; }

        [Display(Name = "وضعیت کاربری")]
        public int User_RoleID { get; set; }

        [Display(Name = "کد ملی")]
        [StringLength(10, ErrorMessage = "مقدار وارد شده بیش 10 کارکتراست")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا مقدار را وارد نمایید")]
        [Remote("NationalCodeValid", "Register", HttpMethod = "Post",ErrorMessage="قبلا استفاده شده لطفا مقدار دیگری را  وارد کنید")]
        public string User_NationalCode { get; set; }

        [Display(Name = "استان")]
        public int User_StateID { get; set; }
    }

    [MetadataType(typeof(MetaDataUser))]
    public partial class User
    { 
    }

}