//check_domains

$(document).ready(function(e) {

$.validator.addMethod("regex", function(value, element, regexpr) {          
    return regexpr.test(value);
}, "Provide a valid regex");

/*$.validator.addMethod('imgMinWidth', function (value, element, param) {
    alert(element.files[0].Width);
    return this.optional(element) || (element.files[0].Width >= param);
});

$.validator.addMethod('imgMinHeight', function (value, element, param) {
    return this.optional(element) || (element.files[0].height >= param);
});*/


////////////////////////////////////////////////////////////////////
//Banner Form
$("#frm_banner").validate({

	errorClass: "error",

	rules: {
		tariff_id: {
			required: true,
		},
		time_id: {
			required: true,
		},
		name: {
			required: true,
		},
		phone: {
			required: true,
		},
		email: {
			required: true,
			email:true
		},
		accounter_name: {
			required: true,
		},
		tracking_code: {
			required: true,
		},
		card_code: {
			required: true,
		},
		bank_source_id: {
			required: true,
		},
		bank_account_id: {
			required: true,
		},
		date_pay: {
			required: true,
		},
	},

	messages: {
		tariff_id: {
			required: "پلن را انتخاب نمایید",
		},
		time_id: {
			required: "مدت زمان را انتخاب نمایید",
		},
		name: {
			required: "نام و نام خانوادگی را وارد نمایید",
		},
		phone: {
			required: "تلفن را وارد نمایید",
		},
		email: {
			required: "ایمیل را وارد نمایید",
			email:"ایمیل وارد شده معتبر نیست"
		},
		accounter_name: {
			required: "صاحب حساب را وارد نمایید",
		},
		tracking_code: {
			required: "کد پیگیری را وارد نمایید",
		},
		card_code: {
			required: "شماره کارت/حساب را وارد نمایید",
		},
		bank_source_id: {
			required: "بانک مبدا را انتخاب نمایید",
		},
		bank_account_id: {
			required: "بانک مقصد را انتخاب نمایید",
		},
		date_pay: {
			required: "تاریخ پرداخت را انتخاب نمایید",
		},
	}

});//Banner Form


/////////////////////////////////////////////////////////////////////////////
//frm_checkout_request
$("#frm_checkout_request").validate({
	errorClass: "error",
	ignore:'',
	rules: {
		pay_type_id: {
			required:true,
		},
		bank_account_id: {
			required:true,
		},
		price:{
			required:true,
			regex:/^[0-9,]+$/	
		},
	},
	
	messages: {
		pay_type_id: {
			required:"",
		},
		bank_account_id: {
			required:"",
		},
		price:{
			required:"",
			regex:"کاراکترهای وارد شده مجاز نمی باشد"	
		},	
	}	
});//frm_checkout_request




/////////////////////////////////////////////////////////////////////////////
//frm_bank_account
$("#frm_edit_bank_account").validate({
	errorClass: "error",
	ignore:'',
	rules: {
		bank_id: {
			required:true,
		},
		name: {
			required:true,
		},
		account_code:{
			required:true,	
		},
		card_code:{
			required:true,
		},
		sheba_code: {
			required:true,	
		},
		card_pic:{
			extension: "gif|png|jpg|jpeg"
		}
	},
	
	messages: {
		bank_id: {
			required:"",
		},
		name: {
			required:"",
		},
		account_code:{
			required:"",	
		},
		card_code:{
			required:"",
		},
		sheba_code: {
			required:"",	
		},
		card_pic:{
			extension: "فرمت های مجاز gif | png | jpg | jpeg می باشد"
		},	
	}	
});//frm_edit_bank_account




/////////////////////////////////////////////////////////////////////////////
//frm_bank_account
$("#frm_bank_account").validate({
	errorClass: "error",
	ignore:'',
	rules: {
		bank_id: {
			required:true,
		},
		name: {
			required:true,
		},
		account_code:{
			required:true,	
		},
		card_code:{
			required:true,
		},
		sheba_code: {
			required:true,	
		},
		card_pic:{
			required:true,
			extension: "gif|png|jpg|jpeg"
		}
	},
	
	messages: {
		bank_id: {
			required:"",
		},
		name: {
			required:"",
		},
		account_code:{
			required:"",	
		},
		card_code:{
			required:"",
		},
		sheba_code: {
			required:"",	
		},
		card_pic:{
			required:"تصویر کارت خود را انتخاب نمایید",
			extension: "فرمت های مجاز gif | png | jpg | jpeg می باشد"
		},	
	}	
});//frm_bank_account




/////////////////////////////////////////////////////////////////////////////
//frm_add_product
$("#frm_add_product").validate({
	errorClass: "error",
	ignore:'',
	rules: {
		cat1_id: {
			required:true,
		},
		name: {
			required:true,
		},
		file:{
			required:true,	
			extension: "rar|zip"
		},
		image:{
			extension: "gif|png|jpg|jpeg"
		},
		price: {
			required:true,	
			regex:/^[0-9,]+$/
		}
	},
	
	messages: {
		cat1_id: {
			required:"",
		},
		name: {
			required:"",
		},
		file:{
			required:"فایل خود را انتخاب نمایید",
			extension: "فرمت های مجاز zip | rar می باشد"
		},
		image:{
			extension: "فرمت های مجاز gif | png | jpg | jpeg می باشد"
		},
		price: {
			required:"",	
			regex:"کاراکترهای وارد شده مجاز نمی باشد"
		}	
	}	
});//frm_add_product

/////////////////////////////////////////////////////////////////////////////
//frm_edit_product
$("#frm_edit_product").validate({
	errorClass: "error",
	ignore:'',
	rules: {
		cat1_id: {
			required:true,
		},
		name: {
			required:true,
		},
		file:{	
			extension: "rar|zip"
		},
		image:{
			extension: "gif|png|jpg|jpeg"
		},
		price: {
			required:true,	
			regex:/^[0-9,]+$/
		}
	},
	
	messages: {
		cat1_id: {
			required:"",
		},
		name: {
			required:"",
		},
		file:{
			extension: "فرمت های مجاز zip | rar می باشد"
		},
		image:{
			extension: "فرمت های مجاز gif | png | jpg | jpeg می باشد"
		},
		price: {
			required:"",	
			regex:"کاراکترهای وارد شده مجاز نمی باشد"
		}	
	}	
});//frm_edit_product


/////////////////////////////////////////////////////////////////////////////
//frm_cat_store
$("#frm_cat_store").validate({
	errorClass: "error",
	rules: {
		name: {
			required:true,
		},
	},
	
	messages: {
		name: {
			required:"",
		},	
	}	
});//frm_cat_store



//////////////////////////////////////////////////////////////
// frm_store
$("#frm_store").validate({
	errorClass: "error",

	rules: {
		name:{
			required:true,
			maxlength:50
		},
		subtitle: {
			required: true,
			maxlength:100
		},
		keyword: {
			maxlength:255
		},
		url: {
			maxlength:25,
			regex:/^[a-zA-Z0-9_-]+$|^$/
		}	
	},//rules	
	messages: {
		name:{
			required:'',
			maxlength:'حداکثر تعداد کاراکتر مجاز 50 می باشد'
		},
		subtitle: {
			required: '',
			maxlength:'حداکثر تعداد کاراکتر مجاز 100 می باشد'
		},
		keyword: {
			maxlength:'حداکثر تعداد کاراکتر مجاز 255 می باشد'
		},
		url: {
			maxlength:'حداکثر تعداد کاراکتر مجاز 25 می باشد',
			regex:'کاراکترهای وارد شده مجاز نمی باشد'
		}		
	}//messages	
});//frm_store


/////////////////////////////////////////////////////////////////////////////
//frm_profile
$("#frm_profile").validate({
	errorClass: "error",
	ignore:'',
	rules: {
		nationalid: {
			required:true,
            number: true,
            minlength: 10,
            maxlength: 10	
		},
		father_name: {
			required:true,
		},
		year: {
			required:true,
		},
		month: {
			required:true,
		},
		day: {
			required:true,
		},
		phone: {
			required:true,
		},
		mobile: {
			required:true,
			number:true,
		},
		zipcode: {
			required:true,
            number: true,
            minlength: 10,
            maxlength: 10	
		},
		address: {
			required:true,
		},
		image:{
			extension: "gif|png|jpg|jpeg"
		},
		national_pic:{
			required:true,
		},
	},
	
	messages: {
		nationalid: {
			required:"",
            number: "کد ملی وارد شده معتبر نمی باشد",
            minlength: "کد ملی وارد شده معتبر نمی باشد",
            maxlength: "کد ملی وارد شده معتبر نمی باشد"	
		},
		father_name: {
			required:"",
		},
		year: {
			required:"",
		},
		month: {
			required:"",
		},
		day: {
			required:"",
		},
		phone: {
			required:"",
		},
		mobile: {
			required:"",
			number:"",
		},
		zipcode: {
			required:"",
            number: "کد پستی وارد شده معتبر نمی باشد",
            minlength: "کد پستی وارد شده معتبر نمی باشد",
            maxlength: "کد پستی وارد شده معتبر نمی باشد"	
		},
		address: {
			required:"",
		},
		image:{
			extension: "نوع فایل وارد شده مجاز نمی باشد..."
		},
		national_pic:{
			required:"تصویر کارت ملی خود را وارد نمایید.",
		},	
	}	
});//frm_profile


/////////////////////////////////////////////////////////////////////////////
//frm_newsletter
$("#frm_newsletter").validate({
	errorClass: "error",
	rules: {
		email: {
			required:true,
			email:true,
		},
	},
	
	messages: {
		email: {
			required:"",
			email:"",
		},	
	}	
});//frm_newsletter


//////////////////////////////////////////////////////////////
// frm_login
$("#frm_login").validate({
	errorClass: "error",

	rules: {
		email:{
			required:true,
			email: true			
		},
		pass: {
			required: true
		}	
	},//rules	
	messages: {
		email:{
			required:"",
			email: "ایمیل وارد شده معتبر نیست"		
		},
		pass: {
			required: ""
		}		
	}//messages	
});//frm_login

// frm_login2
$("#frm_login2").validate({
	errorClass: "error",

	rules: {
		email:{
			required:true,
			email: true			
		},
		pass: {
			required: true
		}	
	},//rules	
	messages: {
		email:{
			required:"",
			email: "ایمیل وارد شده معتبر نیست"		
		},
		pass: {
			required: ""
		}		
	}//messages	
});//frm_login


//////////////////////////////////////////////////////////////
// frm_register
$("#frm_register").validate({
	errorClass: "error",

	rules: {
		firstname:{
			required:true,
		},
		lastname:{ 
			required:true,
		},
		email:{
			required:true,
			email: true			
		},
		mobile:{
			required:true,
			number: true			
		},
		nationalid:{
			required:true,
            number: true,
            minlength: 10,
            maxlength: 10
		},
		captcha:{
			required:true
		},
		pass: {
			required: true
		},
		re_pass: {
			required: true,
			equalTo: "#pass"
		}	
	},//rules	
	messages: {
		firstname:{
			required:""
		},
		lastname:{ 
			required:""
		},
		email:{
			required:"",
			email: ""			
		},
		mobile:{
			required:'',
			number: ''			
		},
		nationalid:{
			required:'',
            number: "کد ملی وارد شده معتبر نمی باشد",
            minlength: "کد ملی وارد شده معتبر نمی باشد",
            maxlength: "کد ملی وارد شده معتبر نمی باشد"		
		},
		captcha:{
			required:""
		},
		pass: {
			required: ""
		},
		re_pass: {
			required: "",
			equalTo: "کلمه عبور و تکرار آن یکسان نمی باشند"
		}		
	}//messages	
});// frm_register1


//////////////////////////////////////////////////////////////
// frm_changepass
$("#frm_changepass").validate({
	errorClass: "error",

	rules: {
		cur_pass:{
			required:true,
		},
		pass: {
			required: true
		},
		re_pass: {
			required: true,
			equalTo: "#pass"
		}	
	},//rules	
	messages: {
		cur_pass:{
			required:"",
		},
		pass:{
			required:"",
		},
		re_pass: {
			required: "",
			equalTo: "کلمه عبور و تکرار آن مغایرت دارند"
		}		
	}//messages	
});//frm_changepass



//////////////////////////////////////////////////////////////
// frm_forgotpass
$("#frm_forgotpass").validate({
	errorClass: "error",

	rules: {
		email:{
			required:true,
			email: true			
		},
	},//rules	
	messages: {
		email:{
			required:"",
			email: ""		
		},
	}//messages	
});//frm_forgotpass



//////////////////////////////////////////////////////////////
// frm_resetpass
$("#frm_resetpass").validate({
	errorClass: "error",

	rules: {
		pass: {
			required: true
		},
		re_pass: {
			required: true,
			equalTo: "#pass"
		}	
	},//rules	
	messages: {
		pass: {
			required: "",
		},
		re_pass: {
			required: "",
			equalTo: ""
		}		
	}//messages	
});// frm_resetpass

//////////////////////////////////////////////////////////////
$('#contact_form').validate({
	/*errorPlacement: function(error, element) {
    	error.prependTo(element.parent('div'));
  	},*/
	errorClass: "error",	
	rules: {
		username:{
			required:true,
		},
		email:{ 
			email: true			
		},
		text:{
			required:true,
		},
	},//rules	
	messages: {
		username:{
			required:"",
		},
		email:{ 
			email: "ایمیل وارد شده معتبر نیست"			
		},
		text:{
			required:"پیام خود را وارد نمائید",
		},		
	}//messages	
});


});//document ready


























