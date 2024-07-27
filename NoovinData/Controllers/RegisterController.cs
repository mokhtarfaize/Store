using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NoovinData.Models.Domin;
using NoovinData.Models.Plugin;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;
namespace NoovinData.Controllers
{
    public class RegisterController : Controller
    {
        DB db = new DB();
        //
        // GET: /Register/
        public ActionResult Register()
        {
            if (Session["user"]==null)
            {
              return View();
            }
            else
            {
                return RedirectToAction("Profile", "Profile");
            }
            
        }
        [HttpPost]
        public ActionResult Register(User use , int State)
        {
            if (Session["user"] == null)
            {

                User user= new User();

                user.User_Active = false;
                user.User_Address = use.User_Address;
                user.User_City = use.User_City;
                user.User_Date = DateTime.Now;
                user.User_Email = use.User_Email;
                user.User_Mobile = use.User_Mobile;
                user.User_NameFamily = use.User_NameFamily;
                user.User_NationalCode = use.User_NationalCode;
                user.User_Password = use.User_Password;
                user.User_PostelCode = use.User_PostelCode;
                user.User_Rating ='0';
                user.User_RoleID = 1;
                user.User_StateID = Convert.ToInt32(State);
                user.User_Tel = use.User_Tel;
                user.User_UserName = use.User_UserName;

                db.Users.Add(user);
             if (Convert.ToBoolean(db.SaveChanges()>0))
             {
                 TempData["Error"] = "ثبت نام شما با وفقیت انجام شد لطفا به ایمیمل خود مراجعه کنید و روی لینک تایید کلیک نمایید .";
                 
                 //ایمیل فعال سازی را ارسال میکنیم

                 ConfirmEmail conemail = new ConfirmEmail();

                 conemail.ConfirmEmail_Date = DateTime.Today;
                 conemail.ConfirmEmail_Status = false; 
                 conemail.ConfirmEmail_UserID = db.Users.OrderByDescending(a => a.User_ID).FirstOrDefault().User_ID;

                 db.ConfirmEmails.Add(conemail);
                 db.SaveChanges();

                 Email sendemail = new Email();

                 var emailuser=db.Users.OrderByDescending(a=>a.User_ID).FirstOrDefault().User_Email;

                 sendemail.SendEmail(db.Settings.FirstOrDefault().Smtp,
                     db.Settings.FirstOrDefault().Email,
                     db.Settings.FirstOrDefault().Password,
                     emailuser,
                     "فعال سازی اکانت کاربری در سایت " + db.Settings.FirstOrDefault().Title + "",
                     "<br/>کاربر گرامی :" + use.User_NameFamily +
                     "<br/>برای فعال سازی اکانت کاربری خود در سایت " + db.Settings.FirstOrDefault().Title +
                     "روی لینک زیر کلیک نماید .<br/><a href='http://localhost:4918/Register/ConfirmEmail?Code=" + db.ConfirmEmails.OrderByDescending(a => a.ConfirmEmail_ID).FirstOrDefault().ConfirmEmail_ID + "'>لینک فعال سازی شما</a>");
                 return RedirectToAction("Message");
             }
             else
             {
                
                 return RedirectToAction("Message");
             }
            }
            else
            {
                return RedirectToAction("Profile", "Profile");
            }
            
        }

        public ActionResult Message()
        {
            ViewBag.Error = TempData["Error"];
            
         
            return View();
        }

        public ActionResult ConfirmEmail(int Code=0) 

        {
            if (Code!=0)
            {
                var id = db.ConfirmEmails.Where(a => a.ConfirmEmail_ID.Equals(Code)).SingleOrDefault();
                if (id == null)
                {
              
                TempData["Error"] = "خطای در فعال شازی اکانت شا رخ داده است ";
                return RedirectToAction("Message");

                }

                if (id.ConfirmEmail_Date.AddDays(3) < DateTime.Today)
                {
                    TempData["Errors"] = "زمان انقضای تایید اکانت شما به پایان رسیده";
                    return RedirectToAction("Message");
                }
                if(id.ConfirmEmail_Status == false)
                {

                    //ایمیل دفعه اول تایید شده
                    id.ConfirmEmail_Status = true;
                    db.ConfirmEmails.Attach(id);
                    db.Entry(id).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    TempData["Error"] = "فعال ساز ی اکنت شما یک بار انجام شده";
                    
                    return RedirectToAction("Message");
                }

                var userid = id.ConfirmEmail_UserID;
                var conuserid = db.Users.Where(a => a.User_ID.Equals(userid)).SingleOrDefault();

                if (conuserid==null)
                {
                   // TempData["Info"] = "Error";
                    return RedirectToAction("Message");

                }
                else
                {
                    if (conuserid.User_Active == false)
                    {
                        //اکانت دفعه اول است تایید میشود
                        conuserid.User_Active = true;
                        db.Users.Attach(conuserid);
                        db.Entry(conuserid).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        TempData["Error"] = "اکنون میتوانید وارد سایت شوید";
                        
                        return RedirectToAction("Login");

                    }
                    else
                    {
                        TempData["Error"] = "اکانت یک بار تایید شده";
                      
                        return RedirectToAction("Messgae");
                    }
                }
            }
            else
            {
               
                TempData["Error"] = "خطای در فعال شازی اکانت شا رخ داده است ";
                return RedirectToAction("Message");
            }
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string emailusername, string passwordlogin)
        {
            try
            {
                if (Session["user"] != null)
                {
                    return RedirectToAction("Profile", "Profile");
                }
                if(emailusername == "" || passwordlogin == "")
                {
                    ViewBag.Error = "شما مجاز به ورود نیستید ";
                   
                    return View();
                }

                var userlog = (from log in db.Users where (log.User_UserName == emailusername || log.User_Email == emailusername) && log.User_Password == passwordlogin select log).SingleOrDefault();
                if (userlog != null)
                {
                    if (userlog.User_Active != true)
                    {
                        ViewBag.Error = "شما هنوز ایمیل خود را تایید نکردید";
                       
                        return View();
                    }
                    Session["user"] = userlog.User_UserName;
                    Session["role"] = userlog.Role.Role_Name;
                    ViewBag.Error = TempData["Error"];
                    return RedirectToAction("Profile", "Profile");
                }
                else
                {
                    ViewBag.Error = "کلمه عبور یا نام کاربری اشتباه است";
                   
                    return View();
                }
            }
                
            catch
            {

                ViewBag.Error = "شما هنوز ایمیل خود را تایید نکردید";
               
                return View();
            }
           
        }
        public ActionResult Logout()
        {
            try
            {
                if(Session["user"]!=null)
                {
                    Session["user"] = null;
                    Session["role"] = null;
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    return RedirectToAction("Index", "Home");

                }
            }
            catch 
            {

                return RedirectToAction("Profile", "Profile");
            }
        }
        [HttpPost]
        //برای جلوگیری از تکرار بودن مقادیر داخل جدول از این جیسونها استفاده میکنیم 
        public JsonResult UsernameValid(string User_Username)
        {
            try
            {
                var q = db.Users.Where(a => a.User_UserName == User_Username).SingleOrDefault();
                if (q==null)
                {
                    return Json(true, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.DenyGet);
                }
            }
            catch 
            {

                return Json(false,JsonRequestBehavior.DenyGet) ;
            }
        }
        [HttpPost]
        public JsonResult EamilValid(string User_Email)
        {
            try
            {
                var q = db.Users.Where(a => a.User_Email == User_Email).SingleOrDefault();
                if (q == null)
                {
                    return Json(true, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.DenyGet);
                }
            }
            catch
            {

                return Json(false, JsonRequestBehavior.DenyGet);
            }
        }
        [HttpPost]
        public JsonResult MobileValid(string User_Mobile)
        {
            try
            {
                var q = db.Users.Where(a => a.User_Mobile == User_Mobile).SingleOrDefault();
                if (q == null)
                {
                    return Json(true, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.DenyGet);
                }
            }
            catch
            {

                return Json(false, JsonRequestBehavior.DenyGet);
            }
        }
        [HttpPost]
        public JsonResult NationalCodeValid(string User_NationalCode)
        {
            try
            {
                var q = db.Users.Where(a => a.User_NationalCode == User_NationalCode).SingleOrDefault();
                if (q == null)
                {
                    return Json(true, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.DenyGet);
                }
            }
            catch
            {

                return Json(false, JsonRequestBehavior.DenyGet);
            }
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string emailusername)
        {
            try
            {

                if (!this.IsCaptchaValid("Error"))
                {
                    ViewBag.Error = "کد امنیتی وارد شده نادرست است";
                    return View();
                }
                var qrecover = db.Users.Where(a => a.User_Email == emailusername || a.User_UserName == emailusername).SingleOrDefault();

                if (qrecover != null)
                {
                    Email sendemail = new Email();

                    sendemail.SendEmail(db.Settings.FirstOrDefault().Smtp,
                    db.Settings.FirstOrDefault().Email,
                    db.Settings.FirstOrDefault().Password,
                    qrecover.User_Email,
                   "<br/> بازیابی رمز عبور شما در " + db.Settings.FirstOrDefault().Title,
                    "<br/>کاربر گرامی :" + emailusername + "<br/>رمز شما در سایت ما" + qrecover.User_Password);

                    TempData["Error"] = "رمز شما با موفقیت بازیابی شد لطفا ایمیل خود را برای دریافت رمز خود چک کنید";
                    return RedirectToAction("Message");
                }
                else
                {
                    TempData["Error"] = "نام کاربری یا ایمیلی با این مشخصات هنوز در سایت ما ثبت نام نکرده ";
                    return RedirectToAction("Message");
                }

            }
            catch
            {
                TempData["Error"] = " در بازیابی رمز مشکلی پیش آمده ";
                return RedirectToAction("Message");
            }
        }
    }//end 
}  