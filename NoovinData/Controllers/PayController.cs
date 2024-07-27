using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NoovinData.Models.Domin;
using NoovinData.Models.Plugin;
namespace NoovinData.Controllers
{
    public class PayController : Controller
    {
        //
        // GET: /Pay/
        Email email = new Email();
        DB db = new DB();
        [HttpPost]
        public ActionResult PayPrice(int idinterimbill = 0, int idpro = 0)
        {
            try
            {
                if (idinterimbill == 0)
                    return RedirectToAction("Profile");


                if (Session["User"] == null)
                    return RedirectToAction("Login", "Register");

                string username = Session["User"].ToString();

                var quser = db.Users.Where(a => a.User_UserName == username).SingleOrDefault();

                if (quser != null)
                {

                    try
                    {
                        var qid = db.InterimBills.Where(a => a.InterimBill_ID == idinterimbill && a.InterimBill_Buyer == quser.User_ID).SingleOrDefault();
                        //اتصال به درگاه پرداخت
                        if (qid == null)
                            return RedirectToAction("ShowProduct", "Home", new { id = idpro });



                        double amount = qid.InterimBill_PayPrice;
                        if (amount < 1000)
                        {
                           
                            TempData["Error"] = "درعملیات پرداخت خطایی رخ داد";
                            return RedirectToAction("ListInBill", "Profile");
                        }



                        System.Net.ServicePointManager.Expect100Continue = false;
                        Zarinpal.PaymentGatewayImplementationServicePortTypeClient zp = new Zarinpal.PaymentGatewayImplementationServicePortTypeClient();
                        string txt = "پرداخت فاکتور";
                        string Authority;

                        int Status = zp.PaymentRequest("YOUR-ZARINPAL-MERCHANT-CODE", Convert.ToInt32(amount), txt.ToString(), "", "", "http://localhost:4918/Pay/CompletePay", out Authority);

                        if (Status == 100)
                        {

                            Bill b = new Bill();

                            b.Bill_Buyer = qid.InterimBill_Buyer;
                            b.Bill_Count = Convert.ToInt32(qid.InterimBill_Count);
                            b.Bill_Date = DateTime.Today;
                            b.Bill_ExpDate = Convert.ToDateTime(qid.InterimBill_ExpDate).AddDays(10);
                            b.Bill_PayPrice = qid.InterimBill_PayPrice;
                            b.Bill_Postage = Convert.ToInt32(qid.InterimBill_Postage);
                            b.Bill_Price = qid.Product.Product_Price;
                            b.Bill_ProductID = qid.InterimBill_ProductID;
                            b.Bill_RefNo = Authority;
                            b.Bill_Seller = qid.Product.Product_UserID;
                            b.Bill_Status = false;
                            b.Bill_TracingCode = "";
                            b.Bill_TransNo = "";
                            b.InvoiceNumber = qid.InvoiceNumber;

                            db.Bills.Add(b);
                            db.SaveChanges();

                            if (Status == 100)
                            {
                                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + Authority);
                            }
                            else
                            {
                                
                                TempData["Error"] = "درعملیات پرداخت خطایی رخ داد";
                                return RedirectToAction("ListInBill", "Profile");

                            }

                        }
                        else
                        {


                            //کاربر را به یک صفحه ی پیام ارسال کنید تا متن خطا را ببیند
                          
                            TempData["Error"] = "درعملیات پرداخت خطایی رخ داد";
                            return RedirectToAction("ListInBill", "Profile");
                        }
                    }
                    catch
                    {
                        //کاربر را به یک صفحه ی پیام ارسال کنید تا متن خطا را ببیند

                        TempData["Error"] = "درعملیات پرداخت خطایی رخ داد";
                        return RedirectToAction("ListInBill", "Profile");

                    }


                }
                else
                {
                   
                    TempData["Error"] = "درعملیات پرداخت خطایی رخ داد";
                    return RedirectToAction("ShowProduct", "Home", new { id = idpro });
                }

            }
            catch
            {

                TempData["Error"] = "درعملیات پرداخت خطایی رخ داد";
                return RedirectToAction("ListInBill", "Profile");
            }
        }

        public ActionResult CompletePay(string Status, string Authority)
        {
            try
            {

                if (Session["User"] == null)
                    return RedirectToAction("Login", "Register");

                string username = Session["User"].ToString();

                var quser = db.Users.Where(a => a.User_UserName == username).SingleOrDefault();

                if (quser != null)
                {
                    if (Status != "" && Status != null && Authority != "" && Authority != null)
                    {
                        if (Status.ToString().Equals("OK"))
                        {

                            var qrefno = db.Bills.Where(a => a.Bill_RefNo == Authority).SingleOrDefault();

                            if (qrefno == null)
                                return View();

                            int Amount = qrefno.Bill_PayPrice;
                            long RefID;
                            System.Net.ServicePointManager.Expect100Continue = false;
                            Zarinpal.PaymentGatewayImplementationServicePortTypeClient zp = new Zarinpal.PaymentGatewayImplementationServicePortTypeClient();

                            int Statuss = zp.PaymentVerification("YOUR-ZARINPAL-MERCHANT-CODE", Authority, Amount, out RefID);

                            if (Statuss == 100)
                            {

                                qrefno.Bill_TransNo = RefID.ToString();
                                qrefno.Bill_Status = true;
                                qrefno.Bill_PostStatsID = 1;
                                db.Bills.Attach(qrefno);
                                db.Entry(qrefno).State = System.Data.Entity.EntityState.Modified;
                                if (Convert.ToBoolean(db.SaveChanges() > 0))
                                {
                                    //اگر در سمت بانک و در سمت ما پرداخت موفقیت امیز بود این قسمت اجرا شود

                                    //بررسی دانلودی بودن یا پستی بودن

                                    if (qrefno. Product.Product_IsDownload == true)
                                    {
                                        Random rnd = new Random();
                                        //دانلودی
                                        TempDL td = new TempDL();
                                        td.TempDL_BillNo = qrefno.Bill_ID;
                                        td.TempDL_Date = DateTime.Now;
                                        td.TempDL_ExpDate = td.TempDL_Date.AddDays(2);
                                        td.TempDL_UnigNo = ((rnd.Next(1, 999) * DateTime.Now.Year) + DateTime.Now.Second).ToString();
                                        db.TempDLs.Add(td);
                                        db.SaveChanges();

                                        //حذف پیش فاکتور
                                        var qdel = db.InterimBills.Where(a => a.InvoiceNumber == qrefno.InvoiceNumber.ToString()).SingleOrDefault();
                                        db.InterimBills.Remove(qdel);
                                        db.SaveChanges();
                                        //باید برای اطمینان بیشتر این قسمت حذف شرطی باشد

                                       

                                        //کاهش موجودی محصول
                                        var qproid = db. Products.Where(a => a.Product_ID == qrefno.Bill_ProductID).FirstOrDefault();
                                        qproid.Product_ExitCount = (qproid.Product_ExitCount - qrefno.Bill_Count);
                                        db.Products.Attach(qproid);
                                        db.Entry(qproid).State = System.Data.Entity.EntityState.Modified;
                                        db.SaveChanges();




                                        //ارسال ایمیل در صورت اتمام موجودی
                                        if (qproid.Product_ExitCount < 1)
                                        {
                                            email.SendEmail(db.Settings.FirstOrDefault().Smtp, db.Settings.FirstOrDefault().Email, db.Settings.FirstOrDefault().Password, qproid.User.User_Email, "اتمام موجودی محصول شما", "فروشنده گرامی محصول " + qproid.Product_Name + "  <br /> در سایت ما مقدار موجود اش صفر شد.جهت فروش دوباره برای فعالسازی و افزایش موجودی اقدام نمایید.");
                                        }


                                        Plugin_Download pd = new Plugin_Download();
                                        pd.Transid = RefID.ToString();
                                        pd.LinkDl = "/Pay/DownloadLink?ID=" + db.TempDLs.OrderByDescending(a => a.TempDL_ID).FirstOrDefault().TempDL_ID + "&Uniqid=" + td.TempDL_UnigNo + "&IDBill=" + qrefno.Bill_ID;
                                        // return View(pd);
                                        TempData["link"] = pd.LinkDl;
                                        TempData["trans"] = pd.Transid;
                                        TempData["Billid"] = qrefno.Bill_ID;

                                        // اجازه دادن برای دانلود فایل
                                        var access = db.Downloads.Where(a => a.Download_ProductID == qrefno.Bill_ProductID && a.Download_Allow==false).FirstOrDefault();

                                        if (access!=null)
                                        {
                                            access.Download_Allow=true;
                                            db.Downloads.Attach(access);
                                            db.Entry(access).State = System.Data.Entity.EntityState.Modified;
                                            db.SaveChanges();
                                        }
                                        return RedirectToAction("CompleteDownolad", "Pay");

                                    }
                                    else
                                    {    //کاهش موجودی محصول
                                        var qproid = db.Products.Where(a => a.Product_ID == qrefno.Bill_ProductID).FirstOrDefault();
                                        qproid.Product_ExitCount = (qproid.Product_ExitCount - qrefno.Bill_Count);
                                        db.Products.Attach(qproid);
                                        db.Entry(qproid).State = System.Data.Entity.EntityState.Modified;
                                        db.SaveChanges();

                                        //ارسال ایمیل در صورت اتمام موجودی
                                        if (qproid.Product_ExitCount < 1)
                                        {
                                            email.SendEmail(db.Settings.FirstOrDefault().Smtp, db.Settings.FirstOrDefault().Email, db.Settings.FirstOrDefault().Password, qproid.User.User_Email, "اتمام موجودی محصول شما", "فروشنده گرامی محصول " + qproid.Product_Name + "  <br /> در سایت ما مقدار موجود اش صفر شد.جهت فروش دوباره برای فعالسازی و افزایش موجودی اقدام نمایید.");
                                        }

                                        //ارسال پیام به خریدار و فروشنده و مدیر سایت

                                        var idseller = db.Users.Where(a => a.User_ID == qrefno.Bill_Seller).FirstOrDefault();
                                        if (idseller != null)
                                        {

                                            idseller.User_Rating = idseller.User_Rating + 15;

                                            db.Users.Attach(idseller);
                                            db.Entry(idseller).State = System.Data.Entity.EntityState.Modified;
                                            db.SaveChanges();
                                        }
                                        if (idseller != null)
                                        {
                                            email.SendEmail(db.Settings.FirstOrDefault().Smtp, db.Settings.FirstOrDefault().Email, db.Settings.FirstOrDefault().Password, idseller.User_Email, " : خرید محصول شما در سایت " + db.Settings.FirstOrDefault().Title + "", "فروشنده گرامی محصول " + qproid.Product_Name + "  <br /> فروشنده محترم : محصول شما در  تاریخ " + PersianDateExtensionMethods.ToPeString(qrefno.Bill_Date, "yyyy/MM/dd") + " از سایت ما خریداری شد . جزئیات خرید : <br /> 1- شماره تراکنش : " + qrefno.Bill_TransNo + " <br /> 2-شماره رهگیری بانکی : " + qrefno.Bill_RefNo + " <br /> 3- مبلغ پرداخت شده : " + qrefno.Bill_PayPrice + " <br / > خریدار محصول شما کاربر : " + db.Users.Where(a => a.User_ID == qrefno.Bill_Buyer).FirstOrDefault().User_NameFamily + "");
                                        }

                                        var idbuyer = db.Users.Where(a => a.User_ID == qrefno.Bill_Buyer).FirstOrDefault();

                                        if (idbuyer != null)
                                        {


                                            idbuyer.User_Rating = idbuyer.User_Rating + 25;

                                            db.Users.Attach(idbuyer);
                                            db.Entry(idbuyer).State = System.Data.Entity.EntityState.Modified;
                                            db.SaveChanges();
                                        }


                                        if (idbuyer != null)
                                        {
                                            email.SendEmail(db.Settings.FirstOrDefault().Smtp, db.Settings.FirstOrDefault().Email, db.Settings.FirstOrDefault().Password, idbuyer.User_Email, " : خرید موفق شما در سایت " + db.Settings.FirstOrDefault().Title + "", "خریدار گرامی    <br />   :  شما در  تاریخ " + PersianDateExtensionMethods.ToPeString(qrefno.Bill_Date, "yyyy/MM/dd") + " یک خرید موفق داشتید . جزئیات خرید : <br /> 1- شماره تراکنش : " + qrefno.Bill_TransNo + " <br /> 2-شماره رهگیری بانکی : " + qrefno.Bill_RefNo + " <br /> 3- مبلغ پرداخت شده : " + qrefno.Bill_PayPrice + " <br / > فروشنده محصول   : " + idseller.User_NameFamily + "");
                                        }



                                        //پستی
                                        var qdel = db.InterimBills.Where(a => a.InvoiceNumber == qrefno.InvoiceNumber.ToString()).SingleOrDefault();
                                        if (qdel != null)
                                        {
                                            db.InterimBills.Remove(qdel);
                                            db.SaveChanges();
                                        }




                                        return View(qrefno);

                                    }
                                }
                                else
                                {
                                    ViewBag.Error = "خرید شما ناموفق بود.هزینه پرداختی تا 48 الی 72 ساعت دیگر به حساب شما برگشت داده میشود";
                                   
                                    return View();
                                    //اگر پرداخت در سمت بانک تایید شود ولی در سمت ما کامل نگردد این قسمت اجرا شود
                                }



                               
                            }
                            else
                            {
                                ViewBag.Error = "تکمیل عملیات پرداخت ناموفق بود";
                             
                                return View();

                            }

                        }
                        else
                        {
                            ViewBag.Error = "تکمیل عملیات پرداخت ناموفق بود";
                            return View();
                        }

                    }
                    else
                    {
                        ViewBag.Error = "تراکنش عملیات پرداخت شما ناموفق بود";
                                               //تراکنش نامعتبر
                        return View();
                    }


                }
                else
                {
                    ViewBag.Error = "خطایی در پرداخت رخ داد";
               
                    //خطای دسترسی کاربر
                    return View();
                }

            }
            catch
            {
                ViewBag.Error = "خطایی در پرداخت رخ داد";
               
                return View();
            }
        }

        public FilePathResult DownloadLink(int ID, string Uniqid, int IDBill)
        {
            var qbill = db.Bills.Where(a => a.Bill_ID == IDBill).FirstOrDefault().Bill_ProductID;

            var qdown = db.Downloads.Where(a => a.Download_ProductID == qbill && a.Download_Allow == true).FirstOrDefault();

            var qtemp = db.TempDLs.Where(a => a.TempDL_ID == ID && a.TempDL_UnigNo == Uniqid && a.TempDL_ExpDate > DateTime.Now).SingleOrDefault();

            if (qtemp == null)
                return null;

            if (qdown == null)
                return null;

            string strPathName = Server.MapPath("~/Content/Uploder/" + qdown.Download_Url);


            if (System.IO.File.Exists(strPathName) == false)
            {
                return null;
            }
            // **************************************************

            System.IO.Stream oStream = null;

            try
            {
                // بازکردن فایل
                oStream =
                    new System.IO.FileStream
                        (path: strPathName,
                        mode: System.IO.FileMode.Open,
                        share: System.IO.FileShare.Read,
                        access: System.IO.FileAccess.Read);

                // **************************************************
                Response.Buffer = false;

                // تنظیم نوع محتوا که ناشناخته است
                Response.ContentType = "application/octet-stream";

                // تنظیم نام فایل روی دیالوگ دانلود یا دانلود منیجر برای نمایش به کاربر
                Response.AddHeader("Content-Disposition", "attachment; filename=" + strPathName);

                long lngFileLength = oStream.Length;

                // ارسال حجم فایل دانلودی به مرورگر کاربر
                Response.AddHeader("Content-Length", lngFileLength.ToString());
                // **************************************************

                // پر کردن متغییر از حجم فایل برای خواندن بایت های فایل
                long lngDataToRead = lngFileLength;

                // برسی اینکه خواندن فایل هنوز به پایان نرسیده
                while (lngDataToRead > 0)
                {
                    // کد زیر را فقط برای تست گذاشتیم که یک وقفه ی 2 میلی ثانیه ای ایجاد میکنه
                    //System.Threading.Thread.Sleep(200);

                    // برسی اینکه آیا کاربر هنوز متصل هست و می خواد دانلود ادامه داشته باشه
                    if (Response.IsClientConnected)
                    {
                        // هربار چه مقدار از بایت ها خونده بشه
                        int intBufferSize = 1024 * 1024;

                        // ایجاد بافر برای خواندن فایل مقداری از فایل که در متغییر بالا تعیین شده
                        byte[] bytBuffers =
                            new System.Byte[intBufferSize];

                        // خواندن داده ها و گذاشتن در بافر ایجاد شده در مرحله ی قبل
                        int intTheBytesThatReallyHasBeenReadFromTheStream =
                            oStream.Read(buffer: bytBuffers, offset: 0, count: intBufferSize);

                        // خواندن داده ها از بافر و نوشتن داده ها روی مخزن جاری برای ارسال به کاربر
                        Response.OutputStream.Write
                            (buffer: bytBuffers, offset: 0,
                            count: intTheBytesThatReallyHasBeenReadFromTheStream);

                        // ارسال داده های که در مرحله ی قبل در مخزن گذاشته شدند برای مرور گر کاربر
                        // خیلی کم از حافظه ی رم استفاده می کند
                        Response.Flush();

                        // کم کردن داده های ارسال شده از داده های اصلی
                        lngDataToRead =
                            lngDataToRead - intTheBytesThatReallyHasBeenReadFromTheStream;
                    }
                    else
                    {
                        // در صورتی که کاربر قطع اتصال کرده باشد توسط دستور زیر از حلقه جلوگیری میی کند
                        lngDataToRead = -1;
                    }
                }

                return null;

            }
            catch
            {
                return null;
            }
            finally
            {
                if (oStream != null)
                {
                    // بستن فایل
                    oStream.Close();
                    oStream.Dispose();
                    oStream = null;
                }
                Response.Close();
            }


        }

        public ActionResult CompleteDownolad()
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            ViewBag.link = TempData["link"];
           
            ViewBag.idbill = TempData["Billid"];

            if (ViewBag.idbill != null)
            {
                int idbill = ViewBag.idbill;
                var qbill = db.Bills.Where(a => a.Bill_ID == idbill).FirstOrDefault();

                var qidpro = db.Products.Where(a => a.Product_ID == qbill.Bill_ProductID).FirstOrDefault();

                var idseller = db.Users.Where(a => a.User_ID == qbill.Bill_Seller).FirstOrDefault();
                if (idseller != null)
                {
                    email.SendEmail(db.Settings.FirstOrDefault().Smtp, db.Settings.FirstOrDefault().Email, db.Settings.FirstOrDefault().Password, idseller.User_Email, " : خرید محصول شما در سایت " + db.Settings.FirstOrDefault().Title + "", "فروشنده گرامی محصول " + qidpro.Product_Name + "  <br /> فروشنده محترم : محصول شما در  تاریخ " + PersianDateExtensionMethods.ToPeString(qbill.Bill_Date, "yyyy/MM/dd") + " از سایت ما خریداری شد . جزئیات خرید : <br /> 1- شماره تراکنش : " + qbill.Bill_TransNo + " <br /> 2-شماره رهگیری بانکی : " + qbill.Bill_RefNo + " <br /> 3- مبلغ پرداخت شده : " + qbill.Bill_PayPrice + " <br / > خریدار محصول شما کاربر : " + db.Users.Where(a => a.User_ID == qbill.Bill_Buyer).FirstOrDefault().User_NameFamily + "");
                }

                var idbuyer = db.Users.Where(a => a.User_ID == qbill.Bill_Buyer).FirstOrDefault();
                if (idbuyer != null)
                {
                    email.SendEmail(db.Settings.FirstOrDefault().Smtp, db.Settings.FirstOrDefault().Email, db.Settings.FirstOrDefault().Password, idbuyer.User_Email, " : خرید موفق شما در سایت " + db.Settings.FirstOrDefault().Title + "", "خریدار گرامی    <br />   :  شما در  تاریخ " + PersianDateExtensionMethods.ToPeString(qbill.Bill_Date, "yyyy/MM/dd") + " یک خرید موفق داشتید . جزئیات خرید : <br /> 1- شماره تراکنش : " + qbill.Bill_TransNo + " <br /> 2-شماره رهگیری بانکی : " + qbill.Bill_RefNo + " <br /> 3- مبلغ پرداخت شده : " + qbill.Bill_PayPrice + " <br / > فروشنده محصول   : " + idseller.User_NameFamily + " <br /> <a href='http://localhost:4918/" + ViewBag.link + "' style='color:green;'>لینک دانلود محصول</a>");
                }

                return View(qbill);
            }
            else
            {
                ViewBag.Error = "فاکتوری موجود نیست";
                return View();
            }


        }

        public ActionResult HistoryBuy(int id = 0)
        {

            try
            {
                if (Session["User"] == null)
                    return RedirectToAction("Login", "Register");

                string username = Session["User"].ToString();

                var quser = db.Users.Where(a => a.User_UserName == username).SingleOrDefault();

                if (quser != null)
                {

                    ViewBag.Error = TempData["Error"];
                  
                    if (id != 0)
                    {
                        ViewBag.Day = TempData["Day"];
                        ViewBag.sumprice = TempData["sumprice"];
                        ViewBag.model = TempData["qdt"];
                        return View(ViewBag.model);
                    }
                    var qbuy = db.Bills.Where(a => a.Bill_Buyer == quser.User_ID && a.Bill_Status == true).OrderByDescending(a => a.Bill_Date).ToList();
                    return View(qbuy);
                }
                else
                {
                    ViewBag.Error = TempData["State"];
                    return View();
                }

            }
            catch
            {
              
                ViewBag.Error = TempData["State"];
                return View();
            }
        }

        public ActionResult ShowBuy(int id = 0)

        {
            try
            {
                if (id == 0)
                    return RedirectToAction("Profile", "Profile");


                if (Session["User"] == null)
                    return RedirectToAction("Login", "Register");

                string username = Session["User"].ToString();

                var quser = db.Users.Where(a => a.User_UserName == username).SingleOrDefault();

                if (quser != null)
                {
                   
                    var qbuy = db.Bills.Where(a => a.Bill_Buyer == quser.User_ID && a.Bill_Status == true && a.Bill_ID == id).SingleOrDefault();

                    Plugin_Download pd = new Plugin_Download();
                    pd.LinkDl = "/Pay/DownloadLink?ID=" + db.TempDLs.OrderByDescending(a => a.TempDL_ID).FirstOrDefault().TempDL_ID + "&Uniqid=" + db.TempDLs.OrderByDescending(a => a.TempDL_ID).FirstOrDefault().TempDL_UnigNo + "&IDBill=" + qbuy.Bill_ID;                   
                    ViewBag.link =pd.LinkDl;
                    return View(qbuy);
                }
                else
                {
                    TempData["Error"] = "در عملیات درخواستی مشکلی رخ داد";
                    
                    return RedirectToAction("HistoryBuy");
                }

            }
            catch
            {

                TempData["Error"] = "در عملیات درخواستی مشکلی رخ داد";
            
                return RedirectToAction("HistoryBuy");
            }
        }

        public ActionResult HistorySale()
        {
            try
            {
                if (Session["User"] == null)
                    return RedirectToAction("Login", "Register");

                string username = Session["User"].ToString();

                var quser = db.Users.Where(a => a.User_UserName == username).SingleOrDefault();

                if (quser != null)
                {
                    ViewBag.Error = TempData["Error"];
                   
                    var qsale = db.Bills.Where(a => a.Bill_Seller == quser.User_ID && a.Bill_Status == true).OrderByDescending(a => a.Bill_Date).ToList();
                    return View(qsale);
                }
                else
                {
                    ViewBag.Error = "در عملیات درخواستی مشکلی رخ داد";
                  
                    return View();
                }

            }
            catch
            {

                ViewBag.Error = "در عملیات درخواستی مشکلی رخ داد";

                return View();
            }
        }

        public ActionResult ShowSale(int id = 0)
        {
            try
            {
                if (id == 0)
                    return RedirectToAction("Login", "Register");


                if (Session["User"] == null)
                    return RedirectToAction("Login", "Register");

                string username = Session["User"].ToString();

                var quser = db.Users.Where(a => a.User_UserName == username).SingleOrDefault();

                if (quser != null)
                {

                    var qsale = db.Bills.Where(a => a.Bill_Seller == quser.User_ID && a.Bill_Status == true && a.Bill_ID == id).SingleOrDefault();
                    return View(qsale);
                }
                else
                {
                    TempData["Error"] = "در عملیات درخواستی مشکلی رخ داد";
                   
                    return RedirectToAction("HistorySale");
                }

            }
            catch
            {

                TempData["Error"] = "در عملیات درخواستی مشکلی رخ داد";
               
                return RedirectToAction("HistorySale");
            }
        }

        [HttpPost]
        public ActionResult Posts(int id, string PostCode, int selectid)
        {
            try
            {
                if (id == 0)
                    return RedirectToAction("Login", "Register");


                if (Session["User"] == null)
                    return RedirectToAction("Login", "Register");

                string username = Session["User"].ToString();

                var quser = db.Users.Where(a => a.User_UserName == username).SingleOrDefault();

                if (quser != null)
                {

                    var qsale = db.Bills.Where(a => a.Bill_Seller == quser.User_ID && a.Bill_Status == true && a.Bill_ID == id).SingleOrDefault();

                    qsale.Bill_TracingCode = PostCode;
                    qsale.Bill_PostStatsID = selectid;
                    db.Bills.Attach(qsale);
                    db.Entry(qsale).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    
                    return RedirectToAction("HistorySale");
                }
                else
                {
                    TempData["Error"] = "در عملیات درخواستی مشکلی رخ داد";
                  
                    return RedirectToAction("HistorySale");
                }

            }
            catch
            {

                TempData["Error"] = "در عملیات درخواستی مشکلی رخ داد";
                
                return RedirectToAction("HistorySale");
            }
        }


        public ActionResult Withdrawal()
        {
            try
            {

                if (Session["User"] == null)
                    return RedirectToAction("Login", "Register");

                string username = Session["User"].ToString();

                var quser = db.Users.Where(a => a.User_UserName == username).SingleOrDefault();

                if (quser != null)
                {
                    var qbill = db.Bills.Where(a => a.Bill_Seller.Equals(quser.User_ID) && a.Bill_Status == true);

                    List<Bill> lstbill = new List<Bill>();
                    List<double> lstprice = new List<double>();
                    foreach (var item in qbill)
                    {
                        double p = (item.Bill_PayPrice * 0.05);

                        double po = (Math.Ceiling((item.Bill_PayPrice - p) / 100) * 100);

                        lstprice.Add(po);//هزینه قابل برداشت
                        lstbill.Add(item);
                     }


                    if (lstbill.Count() > 0)
                    {
                        //صورتحساب برداشت از قبل موجود میباشد

                        var qw = db.Withdrawals.Where(a => a.Withdrawal_UserID.Equals(quser.User_ID)).SingleOrDefault();

                        Plugin_Bill pbill = new Plugin_Bill();

                        if (qw != null)
                        {
                            pbill.AllRecive = qw.Withdrawal_Stock;
                            pbill.EndRecive = qw.Withdrawal_LastReceivd;
                            pbill.EndDateRecive = PersianDateExtensionMethods.ToPeString(qw.Withdrawal_TimeLastReceived, "yyyy/MM/dd");
                            pbill.Stock = (lstbill.Sum(a => a.Bill_PayPrice)) - (qw.Withdrawal_Stock);
                            pbill.RealStock = Convert.ToInt32(lstprice.Sum());

                            return View(pbill);

                        }
                        else
                        {
                            pbill.AllRecive = qw != null ? qw.Withdrawal_Stock : 0;
                            pbill.EndRecive = qw != null ? qw.Withdrawal_LastReceivd : 0;
                            pbill.EndDateRecive = qw != null ? PersianDateExtensionMethods.ToPeString(qw.Withdrawal_TimeLastReceived, "yyyy/MM/dd") : "پرداختی صورت نگرفته است";
                            pbill.Stock = (lstbill.Sum(a => a.Bill_PayPrice)) - (qw != null ? qw.Withdrawal_Stock : 0);
                            pbill.RealStock = pbill.Stock.ToString() != null ? Convert.ToInt32(lstprice.Sum()) : 0;

                            return View(pbill);
                        }

                    }
                    else
                    {
                        //این کاربر تا کنون صورتحساب برداشت نداشته است
                        ViewBag.Error = "صورتحسابی تاکنون ثبت نشده";
                        return View();
                    }


                }
                else
                {
                    ViewBag.Error = "صورتحسابی تاکنون ثبت نشده";
                    return View();
                }

            }
            catch
            {

                ViewBag.Error = "صورتحسابی تاکنون ثبت نشده";
                return View();
            }

        }

        [HttpPost]
        public ActionResult RequestPrice(int txtprice)
        {
            try
            {

                if (Session["User"] == null)
                    return RedirectToAction("Login", "Register");

                string username = Session["User"].ToString();

                var quser = db.Users.Where(a => a.User_UserName == username).SingleOrDefault();

                if (quser != null)
                {

                    var qws = db.Withdrawals.Where(a => a.Withdrawal_UserID.Equals(quser.User_ID) && a.Withdrawal_Request == true).SingleOrDefault();


                    if (qws != null)
                    {
                      
                        TempData["Error"] = "شما درخواست تسویه یکبار داده اید منتظر بمانید تا به ان رسیدیگی شود.";
                        return RedirectToAction("Profile", "Profile");
                    }


                    var qw = db.Withdrawals.Where(a => a.Withdrawal_UserID.Equals(quser.User_ID)).SingleOrDefault();


                    if (qw != null)
                    {
                        var qbill = db.Bills.Where(a => a.Bill_Seller.Equals(quser.User_ID) && a.Bill_Status == true).ToList();


                        if (qbill == null)
                        {
                           
                            TempData["Error"] = "شما هیچ موجودی برای برداشت ندارید";
                            return RedirectToAction("Profile", "Profile");
                        }

                        List<Bill> lstbill = new List<Bill>();
                        List<double> lstprice = new List<double>();
                        foreach (var item in qbill)
                        {
                            double p = (item.Bill_PayPrice * 0.05);

                            double po = (Math.Ceiling((item.Bill_PayPrice - p) / 100) * 100);

                            lstprice.Add(po);//هزینه قابل برداشت
                            lstbill.Add(item);
                        }

                        if (lstbill.Count() > 0)
                        {
                            //صورتحساب برداشت از قبل موجود میباشد

                            Plugin_Bill pbill = new Plugin_Bill();

                            pbill.Stock = (lstbill.Sum(a => a.Bill_PayPrice)) - (qw != null ? qw.Withdrawal_Stock : 0);
                            int f = pbill.RealStock = pbill.Stock.ToString() != null ? Convert.ToInt32(lstprice.Sum()) : 0;

                            if (f >= txtprice && txtprice >= 5000)
                            {
                                qw.Withdrawal_AmountRequested = txtprice;
                            }
                            else
                            {
                              
                                TempData["Error"] = "مبلغ درخواستی بیش از سقف برداشت است یا کمتر از 5000 هزار تومان است";
                                return RedirectToAction("Profile", "Profile");
                            }
                            qw.Withdrawal_Request = true;
                            db.Withdrawals.Attach(qw);
                            db.Entry(qw).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();

                           
                            return RedirectToAction("Profile", "Profile");

                        }
                        else
                        {
                            //این کاربر تا کنون صورتحساب برداشت نداشته است
                            TempData["Error"] = "خطایی در عملیات شما رخ داد";
                           
                            return RedirectToAction("Profile", "Profile");
                        }

                    }
                    else
                    {
                        //تسویه حساب از قبل موجود نمی باشد
                        var qbill = db.Bills.Where(a => a.Bill_Seller.Equals(quser.User_ID) && a.Bill_Status == true);


                        if (qbill == null)
                        {
                            TempData["Error"] = "شما هیچ موجودی برای برداشت ندارید";
                            return RedirectToAction("Profile", "Profile");
                        }

                        List<Bill> lstbill = new List<Bill>();
                        List<double> lstprice = new List<double>();
                        foreach (var item in qbill)
                        {
                            double p = (item.Bill_PayPrice * 0.05);

                            double po = (Math.Ceiling((item.Bill_PayPrice - p) / 100) * 100);

                            lstprice.Add(po);//هزینه قابل برداشت
                            lstbill.Add(item);
                        }


                        Withdrawal tw = new Withdrawal();

                        if (lstbill.Count() > 0)
                        {
                            if (txtprice <= lstprice.Sum() && txtprice >= 5000)
                            {
                                tw.Withdrawal_AmountRequested = txtprice;
                            }
                            else
                            {
                            
                                TempData["Error"] = "مبلغ درخواستی بیش از سقف برداشت است";
                                return RedirectToAction("Profile", "Profile");
                            }

                            tw.Withdrawal_LastReceivd = 0;
                            tw.Withdrawal_Request = true;
                            tw.Withdrawal_Stock = 0;
                            tw.Withdrawal_TimeLastReceived = DateTime.Now;
                            tw.Withdrawal_UserID = quser.User_ID;
                            db.Withdrawals.Add(tw);
                            db.SaveChanges();
                           
                            return RedirectToAction("Profile", "Profile");

                        }
                        else
                        {
                            
                            TempData["Error"] = "خطایی در عملیات شما رخ داد";
                            return RedirectToAction("Profile", "Profile");
                        }


                    }



                }
                else
                {
                   
                    TempData["Error"] = "شما مجاز به مشاهده صورتحساب نیستید";
                    return RedirectToAction("Profile", "Profile");
                }

            }
            catch
            {
             
                TempData["Error"] = "صورتحسابی تاکنون ثبت نشده";
                return RedirectToAction("Profile", "Profile");
            }
        }

      
	}
}