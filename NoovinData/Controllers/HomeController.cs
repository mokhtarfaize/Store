using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NoovinData.Models.Domin;
using NoovinData.Models.Plugin;
using CaptchaMvc;
using CaptchaMvc.HtmlHelpers;
namespace NoovinData.Controllers
{
    public class HomeController : Controller
    {
        DB db = new DB();
        Email email = new Email();
        //
        // GET: /Home/
        public ActionResult Index()
        {

            try
            {
                var qdate = (from a in db.Settings select a).FirstOrDefault();

                if (DateTime.Today > qdate.DateDeleteBill)
                {
                    var qib = (from a in db.InterimBills orderby a.InterimBill_Date select a).ToList();

                    DateTime d1 = DateTime.Today.AddDays(3);
                    DateTime d2 = DateTime.Today.AddDays(6);
                    DateTime d3 = DateTime.Today.AddDays(9);
                    DateTime d4 = DateTime.Today.AddDays(10);

                    // فاکتور شما 1 روز دیگر انتقضای آن به تمام میرسد
                    var qsmtp = db.Settings.FirstOrDefault().Smtp;
                    var qemail = db.Settings.FirstOrDefault().Email;
                    var qpassword = db.Settings.FirstOrDefault().Password;

                    foreach (var item in qib)
                    {
                        var qemailuser = db.Users.Where(a => a.User_ID == item.InterimBill_Buyer).FirstOrDefault();


                        if (item.InterimBill_ExpDate == d1)
                        {

                            email.SendEmail(qsmtp, qemail, qpassword, qemailuser.User_Email, "پرداخت فاکتور رسید شده", "<div style='direction:rtl;'> آقا / خانم <span style='font-weight:700'>" + qemailuser.User_NameFamily + "</span> فاکتور رسید شده به شماره <span style='font-weight:700'>" + item.InvoiceNumber + "</span> تنها فقط <span style='font-weight:700; color:blue;'>7 روز</span> دیگر انقضا برای پرداخت دارد. <br /><span style='color:red;'> جهت پرداخت آن سریعا اقدام نمایید.</span><br /> با تشکر مدیریت سایت.</div>");
                        }
                        else if (item.InterimBill_ExpDate == d2)
                        {

                            email.SendEmail(qsmtp, qemail, qpassword, qemailuser.User_Email, "پرداخت فاکتور رسید شده", "<div style='direction:rtl;'> آقا / خانم <span style='font-weight:700'>" + qemailuser.User_NameFamily + "</span> فاکتور رسید شده به شماره <span style='font-weight:700'>" + item.InvoiceNumber + "</span> تنها فقط <span style='font-weight:700; color:blue;'>4 روز</span> دیگر انقضا برای پرداخت دارد. <br /><span style='color:red;'> جهت پرداخت آن سریعا اقدام نمایید.</span><br /> با تشکر مدیریت سایت.</div>");
                        }
                        else if (item.InterimBill_ExpDate == d3)
                        {

                            email.SendEmail(qsmtp, qemail, qpassword, qemailuser.User_Email, "پرداخت فاکتور رسید شده", "<div style='direction:rtl;'> آقا / خانم <span style='font-weight:700'>" + qemailuser.User_NameFamily + "</span> فاکتور رسید شده به شماره <span style='font-weight:700'>" + item.InvoiceNumber + "</span> تنها فقط <span style='font-weight:700; color:blue;'>1 روز</span> دیگر انقضا برای پرداخت دارد. <br /><span style='color:red;'> جهت پرداخت آن سریعا اقدام نمایید.</span><br /> با تشکر مدیریت سایت.</div>");
                        }
                        else if (item.InterimBill_ExpDate >= d4)
                        {
                            // فاکتور شما انقضای ان تمام شد
                            email.SendEmail(qsmtp, qemail, qpassword, qemailuser.User_Email, "حذف فاکتور رسید شده", "<div style='direction:rtl;'>  آقا / خانم  <span style='font-weight:700'>" + qemailuser.User_NameFamily + "</span>فاکتور رسید شده به شماره <span style='font-weight:700'>" + item.InvoiceNumber + "</span>  <br /><span style='color:red;'> به علت پرداخت نشدن و به اتمام رسیدن انقضا حذف شد </span>با تشکر مدیریت سایت <br /> </div>");
                            db.InterimBills.Remove(item);
                            db.SaveChanges();
                        }

                    }


                    qdate.DateDeleteBill = DateTime.Today;
                    db.Settings.Attach(qdate);
                    db.Entry(qdate).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch
            {

                return View();

            }


            var qvisit = db.Visits.Where(a => a.Visit_IP == Request.UserHostAddress && a.Visit_Date == DateTime.Today).FirstOrDefault();

            if (qvisit == null)
            {
                Visit v = new Visit();
                v.Visit_Date = DateTime.Today;
                if (Session["User"] == null)
                {
                    v.Visit_UserName = "مهمان";
                }
                else
                {
                    v.Visit_UserName = Session["User"].ToString();
                }

                v.Visit_IP = Request.UserHostAddress;
                v.Visit_Browser = Request.Browser.Type;
                db.Visits.Add(v);
                if (Convert.ToBoolean(db.SaveChanges() > 0))
                {
                    return View();
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }

        }

        public ActionResult page404()
        {
            ViewBag.Error=TempData["Error"];
            return View();
        }
        public ActionResult Search(int Searchid, string keywordSearch)

        {
            var gropq = db.Groups.Where(a => a.Group_ID == Searchid).FirstOrDefault();
            if (gropq == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                
                if (keywordSearch == "")
                {
                    var pro = db.Products.Where(a => a.Product_GroupID == Searchid).ToList();
                    
                    if(pro.Count()==0)
                    {
                        ///اگر همه گروهها انتخاب شده باشد
                        var all = db.Products.Where(a => a.Product_GroupID != Searchid).ToList();
                        if (all.Count() == 0)
                        {
                            ViewBag.Key = keywordSearch;
                            ViewBag.Groupid = Searchid;
                            return View();
                        }
                        else
                        {
                            ViewBag.Key = keywordSearch;
                            ViewBag.Groupid = Searchid;
                            return View(all);
                        }

                    }
                    //کاربر یک گروه انتخابی را میخواهد
                    else
                    {
                        ViewBag.Key = keywordSearch;
                        ViewBag.Groupid = Searchid;
                        return View(pro);
                    }

                    
                }
                else
                {
                    var search = (from ser in db.Products where ser.Product_GroupID == Searchid && ser.Product_Text.Contains(keywordSearch) select ser).ToList();
                    
                    
                    if (search.Count()==0)
                    {
                        //کاربر یک کلمه خاص را در همه گروها جستجو میکند
                        var searchall = (from ser in db.Products where ser.Product_GroupID != Searchid && ser.Product_Text.Contains(keywordSearch) select ser).ToList();
                        if (searchall.Count() ==0)
                        {
                            ViewBag.Key = keywordSearch;
                            ViewBag.Groupid = Searchid;
                            return View();
                        }
                        else
                        {
                            ViewBag.Key = keywordSearch;
                            ViewBag.Groupid = Searchid;
                            return View(searchall);

                        }
                    }
                        //کاربر یک کلمه خاص در گروه خاص را جستجو میکند
                    else
                    {
                        ViewBag.Key = keywordSearch;
                        ViewBag.Groupid = Searchid;
                        return View(search);
                    }
                }


            }
            
        }

        public ActionResult ShowProduct( int id=0)
        {
            if (id == 0)
            {
                TempData["Error"] = "چنین محصولی وجود ندارد";
                return RedirectToAction("Page404");
            }
            else
            {

                var q = db.Products.Where(a => a.Product_ID == id && a.Product_Active == true && a.User.User_Active == true).SingleOrDefault();


                if (q == null)
                {
                    TempData["Error"] = "چنین محصولی وجود ندارد";
                    return RedirectToAction("Page404");
                }
                else
                {
                    if (q.Product_IsDownload == true)
                    {
                        var qd = db.Downloads.Where(a => a.Download_ProductID == q.Product_ID).FirstOrDefault();

                        if (qd == null)
                        {
                            TempData["Error"] = "لینک محصولی وجود ندارد";
                            return RedirectToAction("Page404");

                        }
                            

                        q.Product_Visit = q.Product_Visit + 1;
                        db.Products.Attach(q);
                        db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                        if (Convert.ToBoolean(db.SaveChanges() > 0))
                        {
                            ViewBag.Error=TempData["Error"];
                            return View(q);
                        }
                        else
                        {
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        q.Product_Visit = q.Product_Visit + 1;
                        db.Products.Attach(q);
                        db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                        if (Convert.ToBoolean(db.SaveChanges() > 0))
                        {
                            ViewBag.Error=TempData["Error"];
                                return View(q);
                        }
                        else
                        {
                            return RedirectToAction("Index");
                        }
                    }



                }
            }
        }

        public ActionResult ShowGroup(int min_price = 0, int max_price = 0, int page = 1, string sort = "DESC", int id = 0)
        {

            if (id == 0)
            {
                TempData["Error"] = "هیچ گروه برای نمایش نیست";
                return RedirectToAction("Page404");
            }

            else
            {
                List<Product> lstproduct = new List<Product>();

                var q = db.Products.Where(a => a.Product_GroupID == id && a.Product_Active == true && a.User.User_Active == true).ToList();

                //if (q.Count()==0)
                //{
                //    TempData["Error"] = "هیچ گروه برای نمایش نیست";
                //    return RedirectToAction("Page404");
                //}

                if (q.Count() == 0 && id==4)
                {
                    
                    //نمایش تمام گروهها
                        var all = db.Products.Where(a => a.Product_Active == true && a.User.User_Active == true).ToList();

                        if (all != null)
                        {
                            var qg = db.Groups.Where(a => a.Group_ID == id).SingleOrDefault();
                            if (qg == null)
                            {
                                ViewBag.Gropname = "";
                                ViewBag.Topic = "";
                            }
                            else
                            {
                                ViewBag.Gropname = qg.Group_Name;
                                ViewBag.Topic = qg.Group_ID;
                            }

                            int Take = 3;
                            int Skip = (Take * page) - Take;
                            ViewBag.CountProduct = all.Count();
                            ViewBag.Sort = sort;
                            // 9 * 1 - 9 = 0   9 * 2 - 9 = 9  9 * 3 - 9 =18

                            ViewBag.Take = Take;
                            ViewBag.Skip = Skip;
                            ViewBag.page = page;

                            if (sort == "DESC")
                            {
                                return View(all.OrderByDescending(a => a.Product_Price).Skip(Skip).Take(Take));
                            }
                            else
                            {
                                return View(all.OrderBy(a => a.Product_Price).Skip(Skip).Take(Take));
                            }
                        }
                        else
                        {
                            var qg = db.Groups.Where(a => a.Group_ID == id).SingleOrDefault();
                            if (qg == null)
                            {
                                ViewBag.Gropname = "";
                                ViewBag.Topic = "";
                            }
                            else
                            {
                                ViewBag.Gropname = qg.Group_Name;
                                ViewBag.Topic = qg.Group_ID;
                            }
                            ViewBag.Message = "تاکنون در این گروه هیچ محصولی ثبت نشده";
                            return View();
                        }
                }//end if 1
                else //else 1
                {
                    // هیچ قیمتی برای جستجوی محصول وارد نشد
                    if (min_price <= 1000 && max_price <= 1000)
                    {
                        lstproduct.AddRange(q);

                        var qg = db.Groups.Where(a => a.Group_ID == id).SingleOrDefault();
                        if (qg == null)
                        {
                            ViewBag.Gropname = "";
                            ViewBag.Topic = "";
                        }
                        else
                        {
                            ViewBag.Gropname = qg.Group_Name;
                            ViewBag.Topic = qg.Group_ID;
                        }


                        ViewBag.Minprice = min_price <= 0 ? "" : min_price.ToString();
                        ViewBag.Maxprice = max_price <= 0 ? "" : max_price.ToString();
                        ViewBag.CountProduct = q.Count();
                        ViewBag.Sort = sort;

                        int Take = 3;
                        int Skip = (Take * page) - Take;
                        // 9 * 1 - 9 = 0   9 * 2 - 9 = 9  9 * 3 - 9 =18

                        ViewBag.Take = Take;
                        ViewBag.Skip = Skip;
                        ViewBag.page = page;

                        if (sort == "DESC")
                        {
                            return View(lstproduct.OrderByDescending(a => a.Product_Price).Skip(Skip).Take(Take));
                        }
                        else
                        {
                            return View(lstproduct.OrderBy(a => a.Product_Price).Skip(Skip).Take(Take));
                        }



                    } //end if 2
                    else //else 2
                    { //جستجوی برای زمانی که قیمت وارد میشود
                        lstproduct.AddRange(q);
                        //این لیست برای زمانی که محصولی در این بازه قیمتی پیدا شود استفاده میشود
                        List<Product> lstpro = new List<Product>();
                        var qlist = from a in lstproduct
                                    where a.Product_Price <= max_price && a.Product_Price >= min_price
                                    select a;

                        if (qlist.Count() == 0)
                        {
                            var qg = db.Groups.Where(a => a.Group_ID == id).SingleOrDefault();
                            if (qg == null)
                            {
                                ViewBag.Gropname = "";
                                ViewBag.Topic = "";
                            }
                            else
                            {
                                ViewBag.Gropname = qg.Group_Name;
                                ViewBag.Topic = qg.Group_ID;
                            }
                            ViewBag.Message = "محصولی در این محدوده قیمت پیدا نشد";
                            return View();
                        }
                        else
                        {
                            lstpro.AddRange(qlist);
                            lstproduct.Clear();
                            lstproduct.AddRange(lstpro);

                            var qg = db.Groups.Where(a => a.Group_ID == id).SingleOrDefault();
                            if (qg == null)
                            {
                                ViewBag.Gropname = "";
                                ViewBag.Topic = "";
                            }
                            else
                            {
                                ViewBag.Gropname = qg.Group_Name;
                                ViewBag.Topic = qg.Group_ID;
                            }


                            ViewBag.Minprice = min_price <= 0 ? "1000" : min_price.ToString();
                            ViewBag.Maxprice = max_price <= 0 ? "" : max_price.ToString();

                            ViewBag.CountProduct = q.Count();


                            int Take = 3;
                            int Skip = (Take * page) - Take;
                            // 9 * 1 - 9 = 0   9 * 2 - 9 = 9  9 * 3 - 9 =18

                            ViewBag.Take = Take;
                            ViewBag.Skip = Skip;
                            ViewBag.page = page;

                            if (sort == "DESC")
                            {
                                return View(lstproduct.OrderByDescending(a => a.Product_Price).Skip(Skip).Take(Take));
                            }
                            else
                            {
                                return View(lstproduct.OrderBy(a => a.Product_Price).Skip(Skip).Take(Take));
                            }

                        }
                    } //end else 2

                }//end else 1
            }
        }

        public ActionResult ProductUser(int page = 1, int id = 0)
        {
            if (id == 0)
            {
                TempData["Error"] = "کاربری با چنین مشخصاتی وجود ندارد";
                return RedirectToAction("Page404");
            }
            else
                
            {
               
                var quser = db.Users.Where(a => a.User_ID == id && a.User_Active == true).SingleOrDefault();

                if (quser == null)
                {
                    TempData["Error"] = "کاربری با چنین مشخصاتی وجود ندارد";
                    return RedirectToAction("Page404");
                }
                else
                {
                    var qpro = db.Products.Where(a => a.Product_UserID == quser.User_ID && a.User.User_Active == true).ToList();
                    if (qpro.Count() > 0)
                    {
                        ViewBag.namepro = qpro.OrderByDescending(a => a.Product_Date).Take(1);
                        ViewBag.UserName = quser.User_NameFamily;
                        ViewBag.User = quser.User_UserName;
                        ViewBag.Rating = quser.User_Rating;
                        ViewBag.Stateid = quser.State.State_Name;
                        ViewBag.Iduser = quser.User_ID;
                        ViewBag.CountProduct = qpro.Count();
                        //for paging
                        int Take = 2;
                        int Skip = (Take * page) - Take;

                        ViewBag.Take = Take;
                        ViewBag.Skip = Skip;
                        ViewBag.page = page;
                       
                        return View(qpro.OrderByDescending(a => a.Product_Price).Skip(Skip).Take(Take));
                        
                    }
                    else
                    {
                        ViewBag.Iduser = quser.User_ID;
                        ViewBag.UserName = quser.User_NameFamily;
                        ViewBag.User = quser.User_UserName;
                        ViewBag.Rating = quser.User_Rating;
                        ViewBag.Stateid = quser.State.State_Name;
                        ViewBag.Take = 2;
                        ViewBag.CountProduct = 0;
                      //  ViewBag.error = "برای این کاربر هیچ محصولی ثبت نشده";
                        return View();
                    }

                }

            }
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(string username, string email, string text, string title)
        {
            try
            {
                if (!this.IsCaptchaValid("Error"))
                {
                  
                    ViewBag.Error = "کد امنیتی وارد شده نادرست است";
                    return View();
                }

                var qmailadmin = db.Users.Where(a => a.Role.Role_Name == "Admin").FirstOrDefault();

                if (qmailadmin == null)
                {
                 
                    ViewBag.Error = "پیام ارسال نشد!!!";
                    return View();
                }

                Massage m = new  Massage();
                m.Massage_Body = text + " / " + email;
                m.Massage_Date = DateTime.Now;
                m.Massage_Read = false;
                m.Massage_Title = title;
                m.Massage_UserGet = qmailadmin.User_ID;
               // m.Massage_UserSend =Convert.ToInt32(username);
                db.Massages.Add(m);
                db.SaveChanges();

                Email emails = new Email();
                var qsmtp = db.Settings.FirstOrDefault().Smtp;
                var qemail = db.Settings.FirstOrDefault().Email;
                var qpassword = db.Settings.FirstOrDefault().Password;

                emails.SendEmail(qsmtp, qemail, qpassword, qmailadmin.User_Email, username, "شما یک پیام از کاربر: " + email+ "< br />" +"با عنوان : "+ title + "<br />" + " با محتوای زیر دریافت کردید : "+ text + "");

               // ViewBag.State = "OK";
                ViewBag.Error = "پیام شما با موفقیت ارسال شد.";
                return View();

            }
            catch
            {

                //ViewBag.State = "Error";
                ViewBag.Error = "پیام ارسال نشد!!!";
                return View();

            }
        }
	}
}