using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NoovinData.Models.Domin;
using NoovinData.Models.Plugin;
using System.IO;
namespace NoovinData.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        DB db = new DB();
        Email email = new Email();
        public ActionResult UserIdentity()
        {
            if(Session["user"]==null)

                return RedirectToAction("Login","Register");

            string username=Session["user"].ToString();
            string rolename=Session["role"].ToString();
            if (rolename != "Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
               return RedirectToAction("Profile","Profile");
            }
            
                
            
            var user=db.Users.Where(a=>a.User_UserName==username && a.Role.Role_Name==rolename).SingleOrDefault();
             
           if (user==null)
           {
               TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
               return RedirectToAction("Profile", "Profile");
           }

           var userid = db.Identities.Where(a => a.Identity_Confirm == false).ToList();
           if (userid!=null)
           {
               ViewBag.Error= TempData["Error"];
               return View(userid);
           }
           else
           {
               ViewBag.Error = TempData["Error"];
              // ViewBag.Error = "کاربری برای تایید هویت وجود ندارد";
               return View();
           }
        }

        public ActionResult DetailsIdentity(int id = 0)
        {
            if (id == 0)
            {
                TempData["Error"] = "هیچ کاربری انتخاب نشده";
                return RedirectToAction("UserIdentity");
            }

            if (Session["user"] == null)
                return RedirectToAction("Login", "Register");

            string username = Session["user"].ToString();

            string rolename = Session["role"].ToString();

            if (rolename != "Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("UserIdentity");
            }

            var quser = db.Users.Where(a => a.User_UserName == username && a.Role.Role_Name == rolename).SingleOrDefault();

            if (quser == null)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("UserIdentity");
            }

            var qd = db.Identities.Where(a => a.Identity_ID == id).FirstOrDefault();
            if (qd != null)
            {
                return View(qd);
            }
            else
            {
                TempData["Error"] = "هیچ کاربری انتخاب نشده";
                return RedirectToAction("UserIdentity");
            }

        }

        public ActionResult ConfirmIdentityUser(int Identity=0, int IdentityUser=0)
        {
            if (IdentityUser == 0 && Identity == 0)
            {
                TempData["Error"] = "هیچ کاربری برای تائید انتخاب نشده";
                return RedirectToAction("UserIdentity");
            }

            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename != "Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("UserIdentity");
            }
            var quser = db.Users.Where(a => a.User_UserName == username && a.Role.Role_Name == rolename).SingleOrDefault();
            if (quser == null)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("UserIdentity");
            }

            var quserid = db.Identities.Where(a => a.Identity_ID == Identity && a.Identity_UserID == IdentityUser).SingleOrDefault();

            if (quserid == null)
            {
                TempData["Error"] = "تایید ناموفق بود";
                return RedirectToAction("DetailsIdentity", new { id = Identity });
            }
            else
            {
                quserid.Identity_Confirm = true;
                quserid.User.User_RoleID = 3;
                db.Identities.Attach(quserid);
                db.Entry(quserid).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                // TempData["Error"] = "تایید ناموفق بود";

                //ارسال ایمیل یا پیام به کاربر برای اطلاع از تایید شدن

                Massage m = new Massage();
                m.Massage_Body = "کاربر گرامی : " + quserid.User.User_NameFamily + " اطلاعات هویت کارت ملی شما مورد تایید قرار گرفت.اکنون میتوانید محصول برای فروش ثبت کنید.";
                m.Massage_Date = DateTime.Now;
                m.Massage_Read = false;
                m.Massage_Title = "تایید هویت شما";
                m.Massage_UserGet = quserid.Identity_UserID;
                db.Massages.Add(m);
                db.SaveChanges();

                TempData["Error"] = "کاربر محترم شما تایید شدید";
                return RedirectToAction("UserIdentity");

            }
        }

        public ActionResult MgProduct()
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Register");
           string user = Session["user"].ToString();
           string role = Session["role"].ToString();

             if(role!="Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("UserIdentity");
            }
            var quser=db.Users.Where(a=>a.User_UserName==user && a.Role.Role_Name==role).SingleOrDefault();
            
            if(quser==null)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("UserIdentity");
            }
            var qpro = db.Products.OrderByDescending(a => a.Product_Date).ToList();
           
            ViewBag.Error = TempData["Error"];
            return View(qpro);
        }

        public ActionResult ConfirmProduct(int id) 
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename != "Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("UserIdentity");
            }
            var quser = db.Users.Where(a => a.User_UserName == username && a.Role.Role_Name == rolename).SingleOrDefault();
            if (quser == null)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("UserIdentity");
            }


            var q = db.Products.Where(a => a.Product_ID == id && a.Product_Active == false).SingleOrDefault();

            if (q != null)//محصول تایید نشده
            {
                q.Product_Active = true;

                db.Products.Attach(q);
                db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                if (Convert.ToBoolean(db.SaveChanges() > 0))
                {
                    TempData["Error"] = "عملیات با موفقیت انجام شد";
                 
                    return RedirectToAction("MgProduct");

                }
                else
                {
                    TempData["Error"] = "عملیات با موفقیت انجام نشد";
                  
                    return RedirectToAction("MgProduct");
                }

            }
            else
            {
                var qd = db.Products.Where(a => a.Product_ID == id && a.Product_Active == true).SingleOrDefault();
                if (qd != null)
                {
                    qd.Product_Active = false;

                    db.Products.Attach(qd);
                    db.Entry(qd).State = System.Data.Entity.EntityState.Modified;
                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["Error"] = "عملیات با موفقیت انجام شد";
                       
                        return RedirectToAction("MgProduct");

                    }
                    else
                    {
                        TempData["Error"] = "عملیات با موفقیت انجام نشد";
                       
                        return RedirectToAction("MgProduct");
                    }
                }
                else
                {
                    TempData["Error"] = "عملیات با موفقیت انجام نشد";
                  
                    return RedirectToAction("MgProduct");
                }
            }
        }

        public ActionResult ShowProduct(int id = 0)
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename != "Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("UserIdentity");
            }
            var quser = db.Users.Where(a => a.User_UserName == username && a.Role.Role_Name == rolename).SingleOrDefault();
            if (quser == null)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("UserIdentity");
            }

            if (id == 0)
            {
                TempData["Error"] = "چنین محصولی وجود ندارد";
                return RedirectToAction("Page404");
            }
            else
            {
                var q = db.Products.Where(a => a.Product_ID == id).SingleOrDefault();

                if (q == null)
                {
                    TempData["Error"] = "چنین محصولی وجود ندارد";
                    return RedirectToAction("Page404");
                }
                else
                {

                    return View(q);


                }
            }


        }

        public ActionResult ListRequest()
        {

            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename != "Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("UserIdentity");
            }
            var quser = db.Users.Where(a => a.User_UserName == username && a.Role.Role_Name == rolename).SingleOrDefault();
            if (quser == null)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("UserIdentity");
            }

            var qreq = db.Withdrawals.Where(a => a.Withdrawal_Request == true && a.User.User_Active == true).ToList();
            if (qreq == null)
            {
              
                ViewBag.Error = TempData["Error"];
                return View();
            }
            else
            {
               
                ViewBag.Error = TempData["Error"];
                return View(qreq);
            }
        }

        public ActionResult ShowRequest(int id)
        {

            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename != "Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("UserIdentity");
            }
            var quser = db.Users.Where(a => a.User_UserName == username && a.Role.Role_Name == rolename).SingleOrDefault();
            if (quser == null)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("UserIdentity");
            }

            var qreq = db.Withdrawals.Where(a => a.Withdrawal_Request == true && a.User.User_Active == true && a.Withdrawal_ID == id).SingleOrDefault();
            if (qreq == null)
            {
              
                ViewBag.Error = TempData["Error"];
                return RedirectToAction("ListRequest");
            }
            else
            {
             
                ViewBag.Error = TempData["Error"];
                return View(qreq);

            }

        }

        [HttpPost]
        public ActionResult PayPrice(int id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename != "Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("UserIdentity");
            }
            var quser = db.Users.Where(a => a.User_UserName == username && a.Role.Role_Name == rolename).SingleOrDefault();
            if (quser == null)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("UserIdentity");
            }

            var qreq = db.Withdrawals.Where(a => a.Withdrawal_Request == true && a.User.User_Active == true && a.Withdrawal_ID == id).SingleOrDefault();
            if (qreq == null)
            {
                TempData["Error"] = "درخواست ناموفق بود";
               
                return RedirectToAction("ListRequest");
            }
            else
            {
                ViewBag.amount = qreq.Withdrawal_AmountRequested;


                qreq.Withdrawal_Stock += qreq.Withdrawal_AmountRequested;
                qreq.Withdrawal_Request = false;
                qreq.Withdrawal_LastReceivd = qreq.Withdrawal_AmountRequested;
                qreq.Withdrawal_TimeLastReceived = DateTime.Now;
                qreq.Withdrawal_AmountRequested = 0;


                db.Withdrawals.Attach(qreq);
                db.Entry(qreq).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                var qemail = db.Users.Where(a => a.User_ID == qreq.Withdrawal_UserID).FirstOrDefault();
                var qsmtp = db.Settings.FirstOrDefault().Smtp;
                var qemails = db.Settings.FirstOrDefault().Email;
                var qpassword = db.Settings.FirstOrDefault().Password;
                email.SendEmail(qsmtp, qemails, qpassword, qemail.User_Email, "انجام تسویه حساب ", " <span  style='direction:rtl;'>  آقا / خانم : " + qemail.User_NameFamily + " <br /> درخواست تسویه حساب شما انجام شد. <br /> مبلغ واریزی : " + ViewBag.amount + " تومان   </span>");


           
                TempData["Error"] = "عملیات با موفقیت انجام شد";
                return RedirectToAction("ListRequest");


            }
        }

        [HttpGet]
        public ActionResult MgGroup()
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename != "Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }
            var quser = db.Users.Where(a => a.User_UserName == username && a.Role.Role_Name == rolename).SingleOrDefault();
            if (quser == null)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }


            var q = db.Groups.Where(A => A.Group_ID != 4).OrderBy(a => a.Group_Name).ToList();

            if (q == null)
            {

                return View();
            }
            else
            {

                ViewBag.Error = TempData["Error"];
             
                return View(q);

            }

        }

        [HttpGet]
        public ActionResult AddGroup()
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename != "Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }
            var quser = db.Users.Where(a => a.User_UserName == username && a.Role.Role_Name == rolename).SingleOrDefault();
            if (quser == null)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }


            else
            {
                return View();

            }
        }

        [HttpPost]
        public ActionResult AddGroup(HttpPostedFileBase PicGroup, string txtname = "")
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            if (txtname == "")
            {
              
                ViewBag.Error = "نام گروه وارد نشده";
                return View();
            }

            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename != "Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }
            var quser = db.Users.Where(a => a.User_UserName == username && a.Role.Role_Name == rolename).SingleOrDefault();
            if (quser == null)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }
            else
            {


                Random rnd = new Random();

                if (PicGroup != null)
                {
                    string rndname = rnd.Next().ToString() + "-" + PicGroup.FileName;

                    if (PicGroup.ContentLength <= 512000)
                    {
                        PicGroup.SaveAs(Path.Combine(Server.MapPath("~") + "/Content/_images/Group/" + rndname));
                    }
                    else
                    {
                       /// ViewBag.State = "Error";
                        ViewBag.Error = " حجم تصویر بیش از 500 کیلو بایت است";
                        return View();
                    }

                    Group tg = new Group();
                    tg.Group_Name = txtname;
                    tg.Group_Pic = rndname;

                    db.Groups.Add(tg);
                    db.SaveChanges();
                 
                    return RedirectToAction("MgGroup");
                }
                else
                {
                    string picname = "PicGroup.png";

                    // PicGroup.SaveAs(Path.Combine(Server.MapPath("~") + "/Content/_images/Group/" + rndname));
                    Group tg = new Group();
                    tg.Group_Name = txtname;
                    tg.Group_Pic = picname;

                    db.Groups.Add(tg);
                    db.SaveChanges();
                  
                    return RedirectToAction("MgGroup");
                }
            }
        }

        public ActionResult RemoveGroup(int id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename != "Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }
            var quser = db.Users.Where(a => a.User_UserName == username && a.Role.Role_Name == rolename).SingleOrDefault();
            if (quser == null)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }

            else
            {

                var qremove = db.Groups.Where(a => a.Group_ID == id).FirstOrDefault();

                if (qremove == null)
                {
                    TempData["Error"] = "حذف با موفقیت انجام نشد!!!";
                  
                    return RedirectToAction("MgGroup");
                }

                db.Groups.Remove(qremove);
                if (Convert.ToBoolean(db.SaveChanges() > 0))
                {
                    TempData["Error"] = "حذف با موفقیت انجام شد";
                  
                    return RedirectToAction("MgGroup");
                }
                else
                {
                    TempData["Error"] = "حذف با موفقیت انجام نشد!!!";
                 
                    return RedirectToAction("MgGroup");
                }

            }
        }

        [HttpGet]
        public ActionResult EditGroup(int id = 0)
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");
            if (id == 0)
            {
                TempData["Error"] = "گروهی را برای ویرایش انتخاب کنید";
                return RedirectToAction("MgGroup");
            }
            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename != "Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("MgGroup");
            }
            var quser = db.Users.Where(a => a.User_UserName == username && a.Role.Role_Name == rolename).SingleOrDefault();
            if (quser == null)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("MgGroup");
            }


            var qedit = db.Groups.Where(a => a.Group_ID == id).FirstOrDefault();


            if (qedit == null)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("MgGroup");
            }
            else
            {

                ViewBag.Error = TempData["Error"];
             
                return View(qedit);

            }
        }

        [HttpPost]
        public ActionResult EditGroup(Group g, HttpPostedFileBase PicGroup)
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename != "Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }
            var quser = db.Users.Where(a => a.User_UserName == username && a.Role.Role_Name == rolename).SingleOrDefault();
            if (quser == null)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }


            var qedit = db.Groups.Where(a => a.Group_ID == g.Group_ID).FirstOrDefault();

            if (qedit == null)
            {
                TempData["Error"] = "گروهی را برای ویرایش انتخاب کنید";
                return RedirectToAction("Profile", "Profile");
            }
            else
            {

                if (PicGroup != null)
                {
                    Random rnd = new Random();
                    string rndname = rnd.Next().ToString() + "-" + PicGroup.FileName;

                    if (PicGroup.ContentLength <= 512000)
                    {
                        PicGroup.SaveAs(Path.Combine(Server.MapPath("~") + "/Content/_images/Group/" + rndname));
                        //برای حذف تصویر قبلی گروه که در حافظه قرار گرفته
                        System.IO.File.Delete(Server.MapPath("/Content/_images/Group/") + qedit.Group_Pic);
                    }
                    else
                    {
                       // ViewBag.State = "Error";
                        ViewBag.Error = " حجم تصویر بیش از 500 کیلو بایت است";
                        return RedirectToAction("EditGroup", new { id = qedit.Group_ID });
                    }


                    qedit.Group_Pic = rndname;
                }

                qedit.Group_Name = g.Group_Name;

                db.Groups.Attach(qedit);
                db.Entry(qedit).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                TempData["Error"] = "ویرایش با موفقیت انجام شد";
               
                return RedirectToAction("MgGroup");

            }
        }

        public ActionResult MgPostStatus()
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename != "Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }
            var quser = db.Users.Where(a => a.User_UserName == username && a.Role.Role_Name == rolename).SingleOrDefault();
            if (quser == null)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }


            var q = db.PostStatus.Where(a => a.St_ID != 1).OrderBy(a => a.St_ID).ToList();

            if (q == null)
            {
                //TempData["Error"] = "وضعیتی برای نمایش موجود نیست";
                return View();
            }
            else
            {

                ViewBag.Error = TempData["Error"];
               
                return View(q);

            }
        }

        [HttpGet]
        public ActionResult AddPostStatus()
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename != "Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }
            var quser = db. Users.Where(a => a.User_UserName == username && a.Role.Role_Name == rolename).SingleOrDefault();
            if (quser == null)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }

            return View();

        }

        [HttpPost]
        public ActionResult AddPostStatus(PostStatu ps)
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename != "Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }
            var quser = db.Users.Where(a => a.User_UserName == username && a.Role.Role_Name == rolename).SingleOrDefault();
            if (quser == null)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }

            PostStatu t = new PostStatu();
            t.St_Name = ps.St_Name;
            db.PostStatus.Add(t);
            db.SaveChanges();
            TempData["Error"] = "اضافه شدن وضعیت با موفقیت انجام شد";
            
            return RedirectToAction("MgPostStatus");

        }

        public ActionResult EditPostStatus(int id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename != "Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }
            var quser = db.Users.Where(a => a.User_UserName == username && a.Role.Role_Name == rolename).SingleOrDefault();
            if (quser == null)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }

            var q = db.PostStatus.Where(a => a.St_ID == id).FirstOrDefault();
            if (q == null)
            {
                TempData["Error"] = "شما مجوز ویرایش را ندارید";
               
                return RedirectToAction("MgPostStatus");
            }
            else
            {
                return View(q);
            }

        }

        [HttpPost]
        public ActionResult EditPostStatus(PostStatu ps)
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename != "Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }
            var quser = db.Users.Where(a => a.User_UserName == username && a.Role.Role_Name == rolename).SingleOrDefault();
            if (quser == null)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }

            var q = db.PostStatus.Where(a => a.St_ID == ps.St_ID && a.St_ID != 1).FirstOrDefault();
            if (q == null)
            {
                TempData["Error"] = "شما مجوز ویرایش را ندارید";
                
                return RedirectToAction("MgPostStatus");
            }
            else
            {
                q.St_Name = ps.St_Name;
                db.PostStatus.Attach(q);
                db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["Error"] = "ویرایش با موفقیت انجام شد";
               
                return RedirectToAction("MgPostStatus");
            }

        }

        public ActionResult RemovePostStatus(int id)
        {
            if (Session["User"]==null)
            {
                return RedirectToAction("Login", "Register");
            }
            string username=Session["User"].ToString();
            string rolename=Session["Role"].ToString();
            if(rolename!="Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }
            var quser=db.Users.Where(a=>a.User_UserName==username && a.Role.Role_Name==rolename ).SingleOrDefault();
            
            if (quser==null)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }
            var qdel=db.PostStatus.Where(a=>a.St_ID==id && a.St_ID!=1 && id!=1).FirstOrDefault();

            if (qdel==null)
            {
                TempData["Error"] = "عملیات حذف با موفقیت انجام نشد!!!";
             
                return RedirectToAction("MgPostStatus");

            }
            else
            {
                db.PostStatus.Remove(qdel);
                db.SaveChanges();
                TempData["Error"]="عملیات حذف با موفقیت انجام شد";
             
                return RedirectToAction("MgPostStatus");
            }
        }

        public ActionResult MgSetting()
        {
            if (Session["User"]==null)

            {
                return RedirectToAction("Login","Register");
            }
            string username=Session["User"].ToString();
            string rolename=Session["Role"].ToString();
            if(rolename!="Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile","Profile");
            }
            var quser = db.Users.Where(a => a.User_UserName == username && a.Role.Role_Name == rolename).SingleOrDefault();

            if(quser==null)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }
            var qset = (from a in db.Settings select a).FirstOrDefault();
            if(qset==null)
            {
                return View();
            }
            else
            {
                ViewBag.Error = TempData["Error"];
                return View(qset);
            }
        }

        public ActionResult EditSetting(int id=0)
        {
            if ( Session["User"]==null)
            {
                return RedirectToAction("Login", "Register");

            }
            if(id==0)
            {
            
                return RedirectToAction("Profile", "Profile");
            }
            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename != "Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }
            var quser = (from user in db.Users where (username == user.User_UserName && rolename == user.Role.Role_Name) select user).SingleOrDefault();
             
            if(quser==null)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }
            var qset = db.Settings.OrderBy(a => a.Setting_ID==id).FirstOrDefault();

            if(qset==null)
            {
                return View();
            }
            else
            {
                return View(qset);
            }
        }

        [HttpPost]
        public ActionResult EditSetting(Setting st)
        {

            try
            {
                if (Session["User"] == null)
                {
                    return RedirectToAction("Login", "Register");
                }
                string username = Session["User"].ToString();
                string rolename = Session["Role"].ToString();

                if (rolename != "Admin")
                {
                    TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                    return RedirectToAction("Profile", "Profile");
                }

                var quser = db.Users.Where(a => a.User_UserName == username && a.Role.Role_Name == rolename).SingleOrDefault();

                if (quser == null)
                {
                    TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                    return RedirectToAction("Profile", "Profile");
                }
                var set = db.Settings.Where(a => a.Setting_ID == st.Setting_ID).FirstOrDefault();

                if (set == null)
                {
                    TempData["Error"] = "ویرایش انجام نشد";
                    return View();
                }
                else
                {
                    //set.DateDeleteBill = st.DateDeleteBill;
                    set.Descrption = st.Descrption;
                    set.DescSite = st.DescSite;
                    set.Discount = st.Discount;
                    set.Email = st.Email;
                    set.Keyword = st.Keyword;
                    set.PageCount = st.PageCount;
                    set.Password = st.Password;
                    set.Smtp = set.Smtp;
                    set.Title = st.Title;
                    set.TitleSite = st.TitleSite;

                    db.Settings.Attach(set);
                    db.Entry(set).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["Error"] = "ویرایش با موفقیت انجام شد";
                    return RedirectToAction("MgSetting");

                }
            }
            catch 
            {

                TempData["Error"] = "ویرایش انجام نشد";
                return View();
            }
           
                

            }

        public ActionResult MgUser()
        {
            if (Session["User"]==null)
            {
                return RedirectToAction("Login", "Register");
            }
            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename!="Admin")

            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }

            var qusre = db.Users.Where(a => a.User_UserName==username && a.Role.Role_Name ==rolename).SingleOrDefault();

            if (qusre==null)
            {

                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }
            
                var quseractive = db.Users.Where(a => a.User_ID!=1016).OrderBy(a=>a.User_NameFamily).ToList();
           
                 if(quseractive==null)
                 {
                  
                     ViewBag.Error= TempData["Error"];
                     return View();
                 }
                 else
                 {
                    
                     ViewBag.Error = TempData["Error"];
                     return View(quseractive);
                 }
            }

        public ActionResult ConfirmUser(int id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Register");
            }

            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename != "Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }

            var quser = db.Users.Where(a => a.User_UserName == username && a.Role.Role_Name == rolename).SingleOrDefault();

            if (quser == null)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }

            var qconfirm = db.Users.Where(a => a.User_ID == id && a.User_Active == false).FirstOrDefault();

            if (qconfirm != null)
            {
                qconfirm.User_Active = true;
                db.Users.Attach(qconfirm);
                db.Entry(qconfirm).State = System.Data.Entity.EntityState.Modified;

                if (Convert.ToBoolean(db.SaveChanges() > 0))
                {
                    TempData["Error"] = "فعالسازی انجام شد";
                    return RedirectToAction("MgUser");
                }
                else
                {
                    TempData["Error"] = "فعالسازی انجام نشد";
                    return RedirectToAction("MgUser");
                }
            }
            else
            {
                var qconf = db.Users.Where(a => a.User_ID == id && a.User_Active == true).FirstOrDefault();
                if (qconf!=null)
                {
                    qconf.User_Active = false;
                    db.Users.Attach(qconf);
                    db.Entry(qconf).State = System.Data.Entity.EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["Error"] = "غیرفعالسازی انجام شد";
                        return RedirectToAction("MgUser");
                    }
                    else
                    {
                        TempData["Error"] = "غیرفعالسازی انجام نشد";
                        return RedirectToAction("MgUser");
                    }
                }
                else
                {
                    TempData["Error"] = "غیرفعالسازی انجام نشد";
                    return RedirectToAction("MgUser");
                }
                
            }
        }

        public ActionResult ShowUserName(int id)
        {
            if (Session["User"]==null)
            {
                return RedirectToAction("Login", "Register");
                
            }

            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename!="Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }

            var quser = db.Users.Where(a => a.User_UserName == username && a.Role.Role_Name == rolename ).SingleOrDefault();

            if (quser==null)
            {

                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }
            var quse = db.Users.Where(a => a.User_ID == id && id != 1009 && a.User_Active == true).FirstOrDefault();
            
            if(quse==null)
            {
                
                TempData["Error"] = "در نمایش اطلاعات مشکلی رخ داده";
                return RedirectToAction("MgUser");
                
            }
            ViewBag.Error = TempData["Error"];

            return View(quse);
        }

        public ActionResult UpRole(int roleid=0,int iduser=0)
        {
            if (Session["User"]==null)
            {
                return RedirectToAction("Login", "Register");
            }

            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename!="Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }

            if (roleid==0 || iduser==0)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }

            var quser = db.Users.Where(a => a.User_UserName == username && a.Role.Role_Name == rolename).SingleOrDefault();

            if (quser==null)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }

            var qrol = db.Users.Where(a => a.User_ID == iduser).FirstOrDefault();

            if (qrol == null)
            {
                TempData["Error"] = "هیچ کابری پیدا نشد";
                return RedirectToAction("ShowUserName", new { id = qrol.User_ID });
            }
            else
            {
                var qrole = db.Roles.Where(a => a.Role_ID == roleid).FirstOrDefault();
                if (qrole == null)
                {
                    TempData["Error"] = "چنین دسترسی وجود ندارد";
                    return RedirectToAction("ShowUserName", new { id = qrol.User_ID });
                }
                qrol.User_RoleID = roleid;
                db.Users.Attach(qrol);
                db.Entry(qrol).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["Error"] = "تغیر دسترسی با موفقیت انجام شد";
                return RedirectToAction("ShowUserName", new { id = qrol.User_ID });
            }
        }
        [HttpGet]
        public ActionResult ReportingDate()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Register");
            }

            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename != "Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }
            
            var quser = db.Users.Where(a => a.User_UserName == username && a.Role.Role_Name == rolename).SingleOrDefault();

            if (quser == null)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }
            return View();
        }

        [HttpPost]
        public ActionResult ReportingDate(string dt1="", string dt2="")
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Register");
            }

            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename != "Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }

            var quser = db.Users.Where(a => a.User_UserName == username && a.Role.Role_Name == rolename).SingleOrDefault();

            if (quser == null)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }
            if (dt1=="" || dt2=="")
            {
                ViewBag.Error = "لطفا تاریخ را برای جستجو وارد کنید";
                return View("ReportingDate");
            }

            DateTime dbegin = Convert.ToDateTime(dt1);
            DateTime dend = Convert.ToDateTime(dt2);

            var qdate=db.Bills.Where(a=>a.Bill_Date>=dbegin && a.Bill_Date<=dend && a.Bill_Status==true ).ToList();

            if (qdate.Count()<=0)
            {
                ViewBag.Error = "هیچ فروشی در این تاریخ انجام نشده";
                return View();
            }
            else
            {
                ViewBag.Day=(dend-dbegin).TotalDays;
                ViewBag.sumprice = qdate.Sum(a => a.Bill_PayPrice);
                ViewBag.SumPostage =qdate.Sum(a=>a.Bill_Postage);
                ViewBag.Error = "لیست فروشها در این تاریخ";
                return View(qdate);
            }
        
        }

        public ActionResult ListVisit(int page=1)
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename != "Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }
            var quser = db.Users.Where(a => a.User_UserName == username && a.Role.Role_Name == rolename).SingleOrDefault();
            if (quser == null)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }

            var q = (from a in db.Visits orderby a.Visit_Date select a).ToList();


            if (q == null)
            {
                ViewBag.Error = TempData["Error"];
               
                return View();
            }
            else
            {
                int Take = 6;
                int Skip = (Take * page) - Take;

                ViewBag.Take = Take;
                ViewBag.Skip = Skip;
                ViewBag.page = page;

                ViewBag.CountVisit = q.Count();
                ViewBag.Error = TempData["Error"];
              
                return View(q.Skip(Skip).Take(Take));
            }

        }

        public ActionResult MgSlider()
        {
            if (Session["User"]==null)
            {
                return RedirectToAction("Login", "Register");
            }
            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename!="Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }

            var user = db.Users.Where(a => a.User_UserName == username && a.Role.Role_Name == rolename).SingleOrDefault();

            if (user!=null)
            {
                var slide = db.Sliders.OrderBy(a => a.Sort).ToList();

                if (slide!=null)
                {
                    ViewBag.Error = TempData["Error"];
                    return View(slide);
                }
                else
                {
                    ViewBag.Error = TempData["Error"];
                    return View();
                }
            }
            else
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }
        }

        public ActionResult ConfirmSlider(int  id )
        
        {
            if (Session["User"]==null)
            
                return RedirectToAction("Login", "Register");
            
            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename!="Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }

            var user = db.Users.Where(a => a.User_UserName == username && a.Role.Role_Name == rolename).SingleOrDefault();

            if (user == null)
            
                {
                    TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                    return RedirectToAction("Profile", "Profile");
                }
           

                var access = db.Sliders.Where(a => a.ID == id && a.Enable == false).FirstOrDefault();

                if (access != null)
                {
                    access.Enable = true;
                    db.Sliders.Attach(access);
                    db.Entry(access).State = System.Data.Entity.EntityState.Modified;
                    
                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["Error"] = "فعالسازی انجام شد";
                        return RedirectToAction("MgSlider");
                    }
                    else
                    {
                        TempData["Error"] = "فعالسازی انجام نشد";
                        return RedirectToAction("MgSlider");
                    }
                }
                else
                {
                    var acess = db.Sliders.Where(a => a.ID == id && a.Enable == true).FirstOrDefault();

                    if (acess!=null)
                    {
                        acess.Enable = false;
                        db.Sliders.Attach(acess);
                        db.Entry(acess).State = System.Data.Entity.EntityState.Modified;
                        if (Convert.ToBoolean(db.SaveChanges() > 0))
                        {
                            TempData["Error"] = "غیرفعالسازی انجام شد";
                            return RedirectToAction("MgSlider");
                        }
                        else
                        {
                            TempData["Error"] = "غیرفعالسازی انجام نشد";
                            return RedirectToAction("MgSlier");
                        }
                    }
                    {
                        TempData["Error"] = "غیرفعالسازی انجام نشد";
                        return RedirectToAction("MgSlider");
                    }
                }
            
           
            
        }

        public ActionResult AddSlider()
        {
            if (Session["User"]==null)
            {
                return RedirectToAction("Login", "Register");
            }

           string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename!="Admin")
            {
               TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }

            var user = db.Users.Where(a => a.Role.Role_Name == rolename && a.User_UserName == username).SingleOrDefault();

            if (user==null)
            {
                 TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult AddSlider(string title, string URL, int sort, HttpPostedFileBase Pic)
        {
            
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Register");
            }

            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename != "Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }

            var user = db.Users.Where(a => a.Role.Role_Name == rolename && a.User_UserName == username).SingleOrDefault();

            if (user == null)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }
            else
            {

               
                Slider slid = new Slider();

                Random rnd = new Random();
                if (Pic != null)
                {
                    string namepic = rnd.Next(1, 999).ToString() + Pic.FileName;

                    if (Pic.ContentLength <= 512000)
                    {
                        Pic.SaveAs(Path.Combine(Server.MapPath("~") + "/Content/_images/file/" + namepic));
                    }
                    else
                    {
                        ViewBag.Error = " حجم تصویر بیش از 500 کیلو بایت است";
                        return View();
                    }

                    slid.Sort = sort;
                    slid.Title = title;
                    slid.Url = URL;
                    slid.Enable = false;
                    slid.Image = namepic;
                    db.Sliders.Add(slid);
                    db.SaveChanges();
                    TempData["Error"] = "با موفقیت اضافه شد";
                    return RedirectToAction("MgSlider");
                }
                else
                {
                    string Picture = "pic.jpg";
                    slid.Sort = sort;
                    slid.Title = title;
                    slid.Url = URL;
                    slid.Enable = false;
                    slid.Image = Picture;
                    db.Sliders.Add(slid);
                    db.SaveChanges();
                    TempData["Error"] = "با موفقیت اضافه شد";
                    return RedirectToAction("MgSlider");
                }
            }
        }

        public ActionResult RemoveSlider(int id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Register");
            }

            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename != "Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }

            var user = db.Users.Where(a => a.Role.Role_Name == rolename && a.User_UserName == username).SingleOrDefault();

            if (user == null)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }
            else
            {
                var del = db.Sliders.Where(a => a.ID == id).FirstOrDefault();
                if (del==null)
                {
                    TempData["Error"] = "اسلایدر حذف نشد";
                    return RedirectToAction("MgSlider");
                }
                else
                {
                    db.Sliders.Remove(del);
                    db.SaveChanges();
                    TempData["Error"] = "اسلایدر حذف شد";
                    return RedirectToAction("MgSlider");
                }
            }
        }

        public ActionResult EditSlider(int id)
        {
            if (Session["User"]==null)
            {
                return RedirectToAction("Login", "Register");
            }

            string username = Session["User"].ToString();
            string role = Session["Role"].ToString();
            if (role != "Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }

            var user = db.Users.Where(a => a.Role.Role_Name == role && a.User_UserName == username).SingleOrDefault();

            if (user == null)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }
            else
            {
                var slider = db.Sliders.Where(a => a.ID == id).FirstOrDefault();

                if (slider==null)
                {
                    TempData["Error"] = "اسلایدری برای ویرایش انتخاب نشده";
                    return RedirectToAction("MgSlider");
                }
                else
                {
                    return View(slider);
                }
            }
        }

        [HttpPost]
        public ActionResult EditSlider(Slider slid , HttpPostedFileBase pic)
        {
            if (Session["User"]==null)
            {
                return RedirectToAction("Login", "Register");
            }

            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename!="Admin")
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile"); 
            }

            var user = db.Users.Where(a => a.User_UserName == username && a.Role.Role_Name == rolename).SingleOrDefault();

            if (user==null)
            {
                TempData["Error"] = "لطفا با کاربری ادمین وارد شوید";
                return RedirectToAction("Profile", "Profile");
            }

                var slidedit=db.Sliders.Where(a=>a.ID==slid.ID).FirstOrDefault();
                 
            if (slidedit==null)
	        {
                TempData["Error"] = "اسلایدری را برای ویرایش انتخاب کنید";
                return RedirectToAction("Profile", "Profile");		 
	        }
            
	        else
            {
                if (pic!=null)
                {
                    Random rnd = new Random();
                    var picname = rnd.Next(0, 999).ToString() + pic.FileName;
                    if (pic.ContentLength<=512000)
                    {
                        pic.SaveAs(Path.Combine(Server.MapPath("~")+"/Content/_images/file/" +picname));
                        //برای حذف عکس قبلی 
                        System.IO.File.Delete(Server.MapPath("/Content/_images/file/")+slidedit.Image);
                    }
                    else
                    {
                        ViewBag.Error = " حجم تصویر بیش از 500 کیلو بایت است";
                        return RedirectToAction("EditSlider", new { id = slidedit.ID });
                    }
                    slid.Image = picname;
                }
                slidedit.Image = db.Sliders.Where(a => a.ID == slid.ID).Select(a => a.Image).FirstOrDefault();
                db.Sliders.Attach(slidedit);
                db.Entry(slidedit).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["Error"] = "ویرایش با موفقیت انجام شد";
                return RedirectToAction("MgSlider");

            }
        }

     
	}//end
}