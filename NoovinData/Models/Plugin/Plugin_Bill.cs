using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoovinData.Models.Plugin
{
    
        public class Plugin_Bill
        {
            [Display(Name = "موجودی")]
            public int Stock { get; set; }// کل موجودی

            [Display(Name = "موجودی قابل برداشت")]
            public int RealStock { get; set; } // موجودی واقعی

            [Display(Name = "اخرین دریافتی")]
            public int EndRecive { get; set; } // اخرین دریافتی

            [Display(Name = "کل دریافتی")]
            public int AllRecive { get; set; } // تمامی دریافتی ها

            [Display(Name = "زمان اخرین دریافتی")]
            public string EndDateRecive { get; set; } // اخرین زمان دریافت

        
    }
}