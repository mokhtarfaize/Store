using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NoovinData.Models.Domin;
using System.IO;
using NoovinData.Models.Plugin;

namespace NoovinData.Controllers
{
    public class ProfileController : Controller
    {
        DB db = new DB();
        //
        // GET: /Profile/
        public ActionResult Profile()
        {
            if (Session["user"] == null)

                return RedirectToAction("Login", "Register");

            ViewBag.Error = TempData["Error"];
            return View();
        }

        public ActionResult Identity(int id = 0)
        {
            if (Session["user"] == null)

                return RedirectToAction("Login", "Register");

            var username = Session["user"].ToString();
            var user = db.Users.Where(a => a.User_UserName == username).FirstOrDefault();

            if (user != null)
            {
                var userid = db.Users.Where(a => a.User_ID == id).FirstOrDefault();
                return View(userid);
            }
            else
            {
                TempData["Error"] = "هیچ کاربری برای تایید انتخاب نشده";
                return RedirectToAction("Profile");
            }
        }
        [HttpPost]
        public ActionResult ConfirmIdentity(int iduser, string fathername, string year, string month, string day, HttpPostedFileBase imagecart)
        {
            if (Session["user"] == null)

                return RedirectToAction("Login", "Register");
            string username = Session["user"].ToString();
            var userid = db.Users.Where(a => a.User_UserName == username).FirstOrDefault();
            if (userid != null)
            {
                Identity identity = new Identity();
                identity.Identity_Birth = year + "/" + month + "/" + day;
                identity.Identity_Confirm = false;
                identity.Identity_FathersName = fathername;
                identity.Identity_UserID = userid.User_ID;

                Random rnd = new Random();
                string rndname = imagecart.FileName + "-" + rnd.Next(1, 999).ToString() + ".jpg";
                if (imagecart != null)
                {
                    if (imagecart.ContentLength <= 512000)
                    {
                        imagecart.SaveAs(Path.Combine(Server.MapPath("~") + "/Content/_images/cart/" + rndname));

                    }
                    else
                    {
                        ViewBag.Errro = "حجم تصویر بیش از 500 کیلو بایت است";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Error = "تصویر باید انتخاب گردد";
                    return View();
                }
                identity.Identity_Pic = rndname;
                db.Identities.Add(identity);
                db.SaveChanges();

                TempData["Error"] = "مدارک برای تایید مدیر ارشسال شد";

                return RedirectToAction("Profile");
            }
            else
            {
                TempData["Error"] = "کاربری یافت نشد";
                return RedirectToAction("Profile");
            }

        }

        public ActionResult AddProduct()
        {

            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename == "Seller" || rolename == "Admin")
            {

                var quser = db.Users.Where(a => a.User_UserName == username && (a.Role.Role_Name == rolename)).SingleOrDefault();

                if (quser != null)
                {
                    var quser1 = db.Identities.Where(a => a.Identity_UserID == quser.User_ID && a.Identity_Confirm == true).SingleOrDefault();

                    if (quser1 != null)
                    {
                        return View();
                    }
                    else
                    {

                        return RedirectToAction("Profile", "Profile");
                    }
                }
                else
                {
                    TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                    return RedirectToAction("Profile", "Profile");
                }


            }
            else
            {
                TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";

                return RedirectToAction("Profile", "Profile");
            }


        }
        [HttpPost]
        public ActionResult AddProduct(Product p, HttpPostedFileBase imageindex, HttpPostedFileBase[] picgallery, string TagProduct, string cat1_id)
        {

            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();


            if (rolename == "Seller" || rolename == "Admin")
            {

                var quser = db.Users.Where(a => a.User_UserName == username && (a.Role.Role_Name == rolename)).SingleOrDefault();

                if (quser != null)
                {
                    var quser1 = db.Identities.Where(a => a.Identity_UserID == quser.User_ID && a.Identity_Confirm == true).SingleOrDefault();

                    if (quser1 != null)
                    {
                        Random rnd = new Random();

                        if (imageindex == null)
                        {
                            ViewBag.Error = "تصویر شاخص باید انتخاب گردد";
                            return View();
                        }

                        //-------------------------------------------
                        //  Check the image mime types
                        //-------------------------------------------
                        if (imageindex.ContentType.ToLower() != "image/jpg" &&
                                    imageindex.ContentType.ToLower() != "image/jpeg" &&
                                    imageindex.ContentType.ToLower() != "image/pjpeg" &&
                                    imageindex.ContentType.ToLower() != "image/gif" &&
                                    imageindex.ContentType.ToLower() != "image/x-png" &&
                                    imageindex.ContentType.ToLower() != "image/png")
                        {
                            ViewBag.Error = " نوع فایل انتخابی غیر مجاز است ";
                            return View();
                        }

                        //-------------------------------------------
                        //  Check the image extension
                        //-------------------------------------------
                        if (Path.GetExtension(imageindex.FileName).ToLower() != ".jpg"
                            && Path.GetExtension(imageindex.FileName).ToLower() != ".png"
                            && Path.GetExtension(imageindex.FileName).ToLower() != ".gif"
                            && Path.GetExtension(imageindex.FileName).ToLower() != ".jpeg")
                        {
                            ViewBag.Error = "  فایل انتخابی غیر مجاز است ";
                            return View();
                        }



                        if (imageindex.ContentLength >= 512000)
                        {

                            ViewBag.Error = " حجم تصویر بیش از 500 کیلو بایت است";
                            return View();
                        }


                        string rndname = rnd.Next(1, 999).ToString() + "-" + imageindex.FileName;
                        imageindex.SaveAs(Path.Combine(Server.MapPath("~") + "/Content/_images/product/" + rndname));
                        Product t = new Product();
                        t.Product_PicIndex = rndname;
                        t.Product_AllOff = p.Product_AllOff;
                        t.Product_Date = DateTime.Now;
                        t.Product_ExitCount = p.Product_ExitCount;
                        t.Product_GroupID = Convert.ToInt32(cat1_id);
                        t.Product_IsDownload = p.Product_IsDownload;
                        t.Product_Name = p.Product_Name;
                        t.Product_Off = p.Product_Off;
                        t.Product_Price = p.Product_Price;
                        t.Product_Text = p.Product_Text;
                        t.Product_UserID = quser.User_ID;
                        t.Product_Visit = 0;
                        t.Product_Weight = p.Product_Weight;
                        t.Product_Active = false;

                        db.Products.Add(t);
                        if (Convert.ToBoolean(db.SaveChanges() > 0))
                        {

                            //بخش اضافه شدن تگ
                            var tagname = TagProduct.Trim().Split('-');

                            foreach (var item1 in tagname)
                            {
                                Tag tg = new Tag()

                                {
                                    Tag_ProductID = db.Products.OrderByDescending(a => a.Product_ID).FirstOrDefault().Product_ID,
                                    Tag_Name = item1.Trim()

                                };

                                db.Tags.Add(tg);
                                db.SaveChanges();
                            }


                            //تصاویر گالری



                            if (picgallery != null)
                            {
                                List<Pic> lstpic = new List<Pic>();
                                foreach (var item in picgallery)
                                {
                                    string namepic = rnd.Next().ToString() + "-" + item.FileName;


                                    if (item == null)
                                    {
                                        var qproid = db.Products.OrderByDescending(a => a.Product_ID).FirstOrDefault();
                                        var qtagid = db.Tags.Where(a => a.Tag_ProductID == qproid.Product_ID).ToList();

                                        db.Tags.RemoveRange(qtagid);

                                        db.Products.Remove(qproid);

                                        db.SaveChanges();

                                        ViewBag.Error = "تصویر گالری باید انتخاب گردد";
                                        return View();
                                    }

                                    //-------------------------------------------
                                    //  Check the image mime types
                                    //-------------------------------------------
                                    if (imageindex.ContentType.ToLower() != "image/jpg" &&
                                                imageindex.ContentType.ToLower() != "image/jpeg" &&
                                                imageindex.ContentType.ToLower() != "image/pjpeg" &&
                                                imageindex.ContentType.ToLower() != "image/gif" &&
                                                imageindex.ContentType.ToLower() != "image/x-png" &&
                                                imageindex.ContentType.ToLower() != "image/png")
                                    {
                                        var qproid = db.Products.OrderByDescending(a => a.Product_ID).FirstOrDefault();
                                        var qtagid = db.Tags.Where(a => a.Tag_ProductID == qproid.Product_ID).ToList();

                                        db.Tags.RemoveRange(qtagid);

                                        db.Products.Remove(qproid);

                                        db.SaveChanges();
                                        ViewBag.Error = " نوع فایل انتخابی غیر مجاز است ";
                                        return View();
                                    }

                                    //-------------------------------------------
                                    //  Check the image extension
                                    //-------------------------------------------
                                    if (Path.GetExtension(imageindex.FileName).ToLower() != ".jpg"
                                        && Path.GetExtension(imageindex.FileName).ToLower() != ".png"
                                        && Path.GetExtension(imageindex.FileName).ToLower() != ".gif"
                                        && Path.GetExtension(imageindex.FileName).ToLower() != ".jpeg")
                                    {
                                        var qproid = db.Products.OrderByDescending(a => a.Product_ID).FirstOrDefault();
                                        var qtagid = db.Tags.Where(a => a.Tag_ProductID == qproid.Product_ID).ToList();

                                        db.Tags.RemoveRange(qtagid);

                                        db.Products.Remove(qproid);

                                        db.SaveChanges();

                                        ViewBag.Error = "  فایل انتخابی غیر مجاز است ";
                                        return View();
                                    }


                                    if (item.ContentLength <= 512000)
                                    {
                                        Pic pic = new Pic();
                                        pic.Pic_ProductID = db.Products.OrderByDescending(a => a.Product_ID).FirstOrDefault().Product_ID;
                                        item.SaveAs(Path.Combine(Server.MapPath("~") + "/Content/_images/product/Gallery/" + namepic));
                                        pic.Pic_Name = namepic;
                                        lstpic.Add(pic);
                                    }
                                    else
                                    {

                                        var qproid = db.Products.OrderByDescending(a => a.Product_ID).FirstOrDefault();
                                        var qtagid = db.Tags.Where(a => a.Tag_ProductID == qproid.Product_ID).ToList();

                                        db.Tags.RemoveRange(qtagid);

                                        db.Products.Remove(qproid);

                                        db.SaveChanges();


                                        ViewBag.Error = " حجم تصویر گالری بیش از 500 کیلو بایت است";
                                        return View();
                                    }
                                }
                                //دلیل استفاده از AddRange این است که بیش از یک عکس فرستاده میشود باید کل عکسها ذخیره شود
                                db.Pics.AddRange(lstpic);
                                db.SaveChanges();

                            }
                            else
                            {
                                var qproid = db.Products.OrderByDescending(a => a.Product_ID).FirstOrDefault();
                                var qtagid = db.Tags.Where(a => a.Tag_ProductID == qproid.Product_ID).ToList();

                                db.Tags.RemoveRange(qtagid);

                                db.Products.Remove(qproid);

                                db.SaveChanges();

                                ViewBag.Error = "تصویر گالری باید انتخاب گردد";
                                return View();
                            }

                            var qprorat = db.Products.OrderByDescending(a => a.Product_ID).FirstOrDefault();

                            var iduser = db.Users.Where(a => a.User_ID == qprorat.Product_UserID).SingleOrDefault();
                            if (iduser != null)
                            {
                                iduser.User_Rating = iduser.User_Rating + 10;

                                db.Users.Attach(iduser);
                                db.Entry(iduser).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                            }


                            var qsmtp = db.Settings.FirstOrDefault().Smtp;
                            var qemail = db.Settings.FirstOrDefault().Email;
                            var qpassword = db.Settings.FirstOrDefault().Password;

                            Email email = new Email();
                            email.SendEmail(qsmtp, qemail, qpassword, quser.User_Email, "ثبت محصول جدید در فروشگاه", "کاربر گرامی  آقا / خانم : " + quser.User_NameFamily + " <br /> محصول شما با عنوان " + p.Product_Name + " در سایت ما ثبت گردید . <br /> لطفا جهت نمایش بروی سایت برای فروش تا تایید مدیریت فروشگاه شکیبا باشیدبا سپاس از شما گرامی.");
                            Massage m = new Massage();
                            m.Massage_Body = "کاربر گرامی  آقا / خانم : " + quser.User_NameFamily + " <br /> محصول شما با عنوان " + p.Product_Name + " در سایت ما ثبت گردید . <br /> لطفا جهت نمایش بروی سایت برای فروش تا تایید مدیریت فروشگاه شکیبا باشیدبا سپاس از شما گرامی.";
                            m.Massage_Date = DateTime.Now;
                            m.Massage_Read = false;
                            m.Massage_Title = "ثبت محصول جدید در فروشگاه";
                            m.Massage_UserGet = quser.User_ID;
                            db.Massages.Add(m);
                            db.SaveChanges();

                            TempData["Error"] = "محصول با موفقیت ثبت شده لطفا منتظر تائید مدیر باشید ";
                            return RedirectToAction("ListProduct");
                        }//end savchange
                        else
                        {
                            TempData["Error"] = "محصول ثبت نشده";
                            return RedirectToAction("ListProduct");
                        }

                    }//end quser1
                    else
                    {
                        TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                        return RedirectToAction("ListProduct");
                    }
                }//end quser
                else
                {
                    TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                    return RedirectToAction("Profile", "Profile");
                }
            }//end role admin or seller
            else
            {
                TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                return RedirectToAction("Profile", "Profile");
            }


        }

        public ActionResult ListProduct()
        {

            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();


            if (rolename == "Seller" || rolename == "Admin")
            {
                var quser = db.Users.Where(a => a.User_UserName == username && (a.Role.Role_Name == rolename)).SingleOrDefault();

                if (quser != null)
                {
                    var quser1 = db.Identities.Where(a => a.Identity_UserID == quser.User_ID && a.Identity_Confirm == true).SingleOrDefault();

                    if (quser1 != null)
                    {
                        var qpro = db.Products.Where(a => a.Product_UserID == quser.User_ID).ToList();

                        if (qpro != null)
                        {

                            ViewBag.Error = TempData["Error"];

                            return View(qpro);
                        }
                        else
                        {
                            ViewBag.Error = "شما تاکنون محصولی را برای فروش ثبت نکرده اید!!";
                            return View();
                        }
                    }
                    else
                    {
                        TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                        return RedirectToAction("Profile");
                    }

                }
                else
                {
                    TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                    return RedirectToAction("Profile");
                }

            }
            else
            {
                TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                return RedirectToAction("Profile");
            }
        }

        public ActionResult EditProduct(int id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            if (id == 0)
            {
                TempData["Error"] = "محصولی برای ویرایش انتخاب کنید ";
                return RedirectToAction("ListProduct");
            }


            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();


            if (rolename == "Seller" || rolename == "Admin")
            {
                var quser = db.Users.Where(a => a.User_UserName == username && (a.Role.Role_Name == rolename)).SingleOrDefault();

                if (quser != null)
                {
                    var quser1 = db.Identities.Where(a => a.Identity_UserID == quser.User_ID && a.Identity_Confirm == true).SingleOrDefault();

                    if (quser1 != null)
                    {
                        var qpro = db.Products.Where(a => a.Product_UserID == quser.User_ID && a.Product_ID == id).SingleOrDefault();

                        if (qpro != null)
                        {

                            ViewBag.Result = TempData["Error"];
                            return View(qpro);
                        }
                        else
                        {
                            TempData["Error"] = "امکان ویرایش نیست";
                            return RedirectToAction("ListProduct");
                        }
                    }
                    else
                    {
                        TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                        return RedirectToAction("Profile");
                    }

                }
                else
                {
                    TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                    return RedirectToAction("Profile");
                }

            }
            else
            {
                TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                return RedirectToAction("Profile");
            }
        }

        [HttpPost]
        public ActionResult EditProduct(Product p, HttpPostedFileBase imageindex, HttpPostedFileBase[] picgallery, string TagProduct, string cat1_id, int Productid)
        {

            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");


            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();


            if (rolename == "Seller" || rolename == "Admin")
            {
                var quser = db.Users.Where(a => a.User_UserName == username && (a.Role.Role_Name == rolename)).SingleOrDefault();

                if (quser != null)
                {
                    var quser1 = db.Identities.Where(a => a.Identity_UserID == quser.User_ID && a.Identity_Confirm == true).SingleOrDefault();

                    if (quser1 != null)
                    {
                        var qproid = db.Products.Where(a => a.Product_UserID == quser.User_ID && a.Product_ID == Productid).SingleOrDefault();

                        if (qproid != null)
                        {

                            //عملیات ویرایش


                            Random rnd = new Random();

                            if (imageindex != null)
                            {
                                string rndname = rnd.Next().ToString() + "-" + imageindex.FileName;

                                if (imageindex.ContentLength <= 512000)
                                {

                                    imageindex.SaveAs(Path.Combine(Server.MapPath("~") + "/Content/_images/product/" + rndname));

                                    //حذف تصویر قبلی
                                    try
                                    {
                                        System.IO.File.Delete(Server.MapPath("/Content/_images/product/") + qproid.Product_PicIndex);

                                    }
                                    catch
                                    {

                                    }

                                    qproid.Product_PicIndex = rndname;
                                    qproid.Product_AllOff = p.Product_AllOff;
                                    qproid.Product_ExitCount = p.Product_ExitCount;
                                    qproid.Product_GroupID = Convert.ToInt32(cat1_id);
                                    qproid.Product_IsDownload = p.Product_IsDownload;
                                    qproid.Product_Name = p.Product_Name;
                                    qproid.Product_Off = p.Product_Off;
                                    qproid.Product_Price = p.Product_Price;
                                    qproid.Product_Text = p.Product_Text;
                                    qproid.Product_Weight = p.Product_Weight;

                                    db.Products.Attach(qproid);
                                    db.Entry(qproid).State = System.Data.Entity.EntityState.Modified;

                                    //حذف تگ ها
                                    foreach (var item in db.Tags.Where(a => a.Tag_ProductID == qproid.Product_ID))
                                    {
                                        db.Tags.Remove(item);
                                    }
                                    db.SaveChanges();


                                    var tagname = TagProduct.Trim().Split('-');

                                    foreach (var item1 in tagname)
                                    {
                                        Tag tg = new Tag()
                                        {
                                            Tag_ProductID = qproid.Product_ID,
                                            Tag_Name = item1.Trim()

                                        };

                                        db.Tags.Add(tg);
                                        db.SaveChanges();
                                    }


                                    var qg = db.Pics.Where(a => a.Pic_ProductID == qproid.Product_ID).ToList();

                                    if (qg.Count() == 0)
                                    {
                                        if (picgallery[0] == null)
                                        {
                                            TempData["Error"] = "حداقل یک تصویر برای بخش گالری باید انتخاب شود";
                                            return RedirectToAction("EditProduct", new { id = qproid.Product_ID });
                                        }
                                        else
                                        {

                                            List<Pic> lstpic = new List<Pic>();
                                            foreach (var item in picgallery)
                                            {
                                                string namepic = rnd.Next().ToString() + item.FileName;

                                                if (item.ContentLength <= 512000)
                                                {
                                                    Pic pic = new Pic();
                                                    pic.Pic_ProductID = qproid.Product_ID;
                                                    item.SaveAs(Path.Combine(Server.MapPath("~") + "/Content/_images/product/Gallery/" + namepic));
                                                    pic.Pic_Name = namepic;
                                                    lstpic.Add(pic);
                                                }
                                                else
                                                {

                                                    TempData["Error"] = "حجم تصاویر انتخاب شده برای گالری بیش از 500 کیلوبایت می باشد";
                                                    return RedirectToAction("EditProduct", new { id = qproid.Product_ID });
                                                }
                                            }
                                            //از Addrange استفاده شده چون بیش از یک عکس اضافه میکنیم
                                            db.Pics.AddRange(lstpic);
                                            db.SaveChanges();

                                            TempData["Error"] = "ویرایش با موفقیت انجام شد";


                                            return RedirectToAction("ListProduct");
                                        }
                                    }
                                    else
                                    {

                                        if (picgallery[0] != null)
                                        {

                                            List<Pic> lstpic = new List<Pic>();
                                            foreach (var item in picgallery)
                                            {
                                                string namepic = rnd.Next().ToString() + item.FileName;
                                                if (item.ContentLength <= 512000)
                                                {
                                                    Pic pic = new Pic();
                                                    pic.Pic_ProductID = qproid.Product_ID;
                                                    item.SaveAs(Path.Combine(Server.MapPath("~") + "/Content/_images/product/Gallery/" + namepic));
                                                    pic.Pic_Name = namepic;
                                                    lstpic.Add(pic);
                                                }
                                                else
                                                {

                                                    TempData["Error"] = "حجم تصاویر انتخاب شده برای گالری بیش از 500 کیلوبایت می باشد";
                                                    return RedirectToAction("EditProduct", new { id = qproid.Product_ID });
                                                }
                                            }

                                            db.Pics.AddRange(lstpic);
                                            db.SaveChanges();

                                            TempData["Error"] = "ویرایش با موفقیت انجام شد";
                                            return RedirectToAction("ListProduct");
                                        }

                                        db.SaveChanges();

                                        TempData["Error"] = "ویرایش با موفقیت انجام شد";
                                        return RedirectToAction("ListProduct");


                                    }

                                }
                                else
                                {
                                    TempData["Error"] = "حجم تصاویر انتخاب شده برای گالری بیش از 500 کیلوبایت می باشد";
                                    return RedirectToAction("EditProduct", new { id = qproid.Product_ID });
                                }


                            }
                            else
                            {

                                qproid.Product_AllOff = p.Product_AllOff;
                                qproid.Product_ExitCount = p.Product_ExitCount;
                                qproid.Product_GroupID = Convert.ToInt32(cat1_id);
                                qproid.Product_IsDownload = p.Product_IsDownload;
                                qproid.Product_Name = p.Product_Name;
                                qproid.Product_Off = p.Product_Off;
                                qproid.Product_Price = p.Product_Price;
                                qproid.Product_Text = p.Product_Text;
                                qproid.Product_Weight = p.Product_Weight;


                                db.Products.Attach(qproid);
                                db.Entry(qproid).State = System.Data.Entity.EntityState.Modified;

                                foreach (var item in db.Tags.Where(a => a.Tag_ProductID == qproid.Product_ID))
                                {
                                    db.Tags.Remove(item);
                                }
                                db.SaveChanges();


                                var tagname = TagProduct.Trim().Split('-');

                                foreach (var item1 in tagname)
                                {
                                    Tag tg = new Tag()
                                    {
                                        Tag_ProductID = qproid.Product_ID,
                                        Tag_Name = item1.Trim()

                                    };

                                    db.Tags.Add(tg);
                                    db.SaveChanges();
                                }

                                var qg = db.Pics.Where(a => a.Pic_ProductID == qproid.Product_ID).ToList();

                                if (qg.Count() == 0)
                                {
                                    if (picgallery[0] == null)
                                    {
                                        TempData["Error"] = "حداقل یک تصویر برای بخش گالری باید انتخاب شود";
                                        return RedirectToAction("EditProduct", new { id = qproid.Product_ID });
                                    }
                                    else
                                    {

                                        List<Pic> lstpic = new List<Pic>();
                                        foreach (var item in picgallery)
                                        {
                                            string namepic = rnd.Next().ToString() + item.FileName;
                                            if (item.ContentLength <= 512000)
                                            {
                                                Pic pic = new Pic();
                                                pic.Pic_ProductID = qproid.Product_ID;
                                                item.SaveAs(Path.Combine(Server.MapPath("~") + "/Content/_images/product/Gallery/" + namepic));
                                                pic.Pic_Name = namepic;
                                                lstpic.Add(pic);
                                            }
                                            else
                                            {

                                                TempData["Error"] = "حجم تصاویر انتخاب شده برای گالری بیش از 500 کیلوبایت می باشد";
                                                return RedirectToAction("EditProduct", new { id = qproid.Product_ID });
                                            }
                                        }

                                        db.Pics.AddRange(lstpic);
                                        db.SaveChanges();

                                        TempData["Error"] = "ویرایش با موفقیت انجام شد";
                                        return RedirectToAction("ListProduct");
                                    }
                                }
                                else
                                {

                                    if (picgallery[0] != null)
                                    {

                                        List<Pic> lstpic = new List<Pic>();
                                        foreach (var item in picgallery)
                                        {
                                            string namepic = rnd.Next().ToString() + item.FileName;
                                            if (item.ContentLength <= 512000)
                                            {
                                                Pic pic = new Pic();
                                                pic.Pic_ProductID = qproid.Product_ID;
                                                item.SaveAs(Path.Combine(Server.MapPath("~") + "/Content/_images/product/Gallery/" + namepic));
                                                pic.Pic_Name = namepic;
                                                lstpic.Add(pic);
                                            }
                                            else
                                            {

                                                TempData["Error"] = "حجم تصاویر انتخاب شده برای گالری بیش از 500 کیلوبایت می باشد";
                                                return RedirectToAction("EditProduct", new { id = qproid.Product_ID });
                                            }
                                        }

                                        db.Pics.AddRange(lstpic);
                                        db.SaveChanges();

                                        TempData["Error"] = "ویرایش با موفقیت انجام شد";

                                        return RedirectToAction("ListProduct");
                                    }

                                    db.SaveChanges();
                                    TempData["Error"] = "ویرایش با موفقیت انجام شد";
                                    return RedirectToAction("ListProduct");
                                }

                            }

                        }
                        else
                        {
                            TempData["Error"] = "امکان ویرایش نیست";
                            return RedirectToAction("ListProduct");
                        }
                    }//end if user1
                    else
                    {
                        TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                        return RedirectToAction("Profile");
                    }

                }//end if user
                else
                {
                    TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                    return RedirectToAction("Profile");
                }

            }//end if role
            else
            {
                TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                return RedirectToAction("Profile");
            }

        }

        [HttpPost]
        public string DelPic(int ids, int ProductIDs)
        {
            try
            {
                if (Session["User"] == null)//مبحث امنیتی
                    return "";

                string username = Session["User"].ToString();

                var q = (from a in db.Products
                         where a.User.User_UserName.Equals(username) && a.Product_ID.Equals(ProductIDs)
                         select a).SingleOrDefault();

                if (q != null)
                {
                    var q1 = q.Pics.Where(a => a.Pic_ID.Equals(ids)).SingleOrDefault();
                    try
                    {
                        System.IO.File.Delete(Server.MapPath("/Content/_images/product/Gallery/") + q1.Pic_Name);

                    }
                    catch
                    {

                    }
                    db.Pics.Remove(q1);
                    db.SaveChanges();


                    string Temp = "<ul class='list'>";

                    foreach (var item in q.Pics)
                    {
                        Temp += "<li class='item full'> <span><img src='/Content/_images/product/Gallery/" + item.Pic_Name + "' width='80' height='80' alt='' /></span><span style='color: red; font-size: 20px;margin-right: 10px;'><a data-ajax='true' data-ajax-confirm='میخواهید حذف شود؟ ' data-ajax-method='Post' data-ajax-mode='replace' data-ajax-update='#PicDelet' href='/Profile/DelPic?ids=" + item.Pic_ID + "&ProductIDs=" + item.Pic_ProductID + "'>X</a></span></li>";
                    }

                    Temp += "</ul>";

                    return Temp;

                }
                else
                {
                    return "خطایی رخ داد!!!";
                }



            }
            catch
            {
                return "خطایی رخ داد!!!";

            }
        }

        public ActionResult ListLink()
        {

            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();


            if (rolename == "Seller" || rolename == "Admin")
            {
                var quser = db.Users.Where(a => a.User_UserName == username && (a.Role.Role_Name == rolename)).SingleOrDefault();

                if (quser != null)
                {
                    var quser1 = db.Identities.Where(a => a.Identity_UserID == quser.User_ID && a.Identity_Confirm == true).SingleOrDefault();

                    if (quser1 != null)
                    {

                        var qlist = (from a in db.Products
                                     where a.Product_IsDownload == true && a.Product_UserID == quser.User_ID
                                     orderby a.Product_Date descending
                                     select a).ToList();

                        if (qlist == null)
                        {
                            ViewBag.Error = "شما تاکنون محصولی  دانلودی را برای فروش ثبت نکرده اید!!";
                            return View();
                        }
                        else
                        {
                            //نمایش محصولات دانلودی
                            ViewBag.Error = TempData["Error"];

                            return View(qlist);
                        }


                    }
                    else
                    {
                        TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                        return RedirectToAction("Profile");
                    }

                }
                else
                {
                    TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                    return RedirectToAction("Profile");
                }

            }
            else
            {
                TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                return RedirectToAction("Profile");
            }




        }

        [HttpGet]
        public ActionResult AddLink(int id = 0)
        {

            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();


            if (rolename == "Seller" || rolename == "Admin")
            {
                var quser = db.Users.Where(a => a.User_UserName == username && (a.Role.Role_Name == rolename)).SingleOrDefault();

                if (quser != null)
                {
                    var quser1 = db.Identities.Where(a => a.Identity_UserID == quser.User_ID && a.Identity_Confirm == true).SingleOrDefault();

                    if (quser1 != null)
                    {

                        var qid = db.Products.Where(a => a.Product_ID == id).SingleOrDefault();

                        if (qid == null)
                        {
                            return RedirectToAction("ListLink");

                        }
                        else
                        {
                            return View(qid);
                        }


                    }
                    else
                    {
                        TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                        return RedirectToAction("Profile");
                    }

                }
                else
                {
                    TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                    return RedirectToAction("Profile");
                }

            }
            else
            {
                TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                return RedirectToAction("Profile");
            }


        }

        [HttpPost]
        public JsonResult UploadFiles()
        {
            if (Session["User"] == null)
                return Json(false);

            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();

            if (rolename == "Seller" || rolename == "Admin")
            {
                var quser = db.Users.Where(a => a.User_UserName == username && (a.Role.Role_Name == rolename)).SingleOrDefault();

                if (quser != null)
                {
                    var quser1 = db.Identities.Where(a => a.Identity_UserID == quser.User_ID && a.Identity_Confirm == true).SingleOrDefault();

                    if (quser1 != null)
                    {
                        var r = new List<UploadFilesResult>();

                        foreach (string file in Request.Files)
                        {
                            HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                            if (hpf.ContentLength == 0)
                                continue;
                            Random rnd = new Random();
                            string name = rnd.Next(1, 1000).ToString() + hpf.FileName;
                            string savedFileName = Path.Combine(Server.MapPath("~/Content/Uploder"), Path.GetFileName(name));
                            hpf.SaveAs(savedFileName);

                            r.Add(new UploadFilesResult()
                            {
                                Name = hpf.FileName,
                                Length = hpf.ContentLength / 1024,
                                Type = hpf.ContentType
                            });
                            Download d = new Download();
                            d.Download_Allow = false;
                            d.Download_Date = DateTime.Now;
                            d.Download_Lenght = hpf.ContentLength / 1024;
                            d.Download_Url = name;
                            d.Download_UserID = quser.User_ID;
                            db.Downloads.Add(d);
                            db.SaveChanges();
                        }

                        return Json(true, JsonRequestBehavior.AllowGet);                      

                    }
                    else
                    {
                        TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                        return Json(false);
                    }

                }
                else
                {
                    TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                    return Json(false);
                }

            }
            else
            {
                TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                return Json(false);
            }



        }

        [HttpPost]
        public ActionResult upload(int proid)
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Register");

            string username = Session["User"].ToString();

            var quser = db.Users.Where(a => a.User_UserName == username).SingleOrDefault();

            if (quser != null)
            {

                var qid = db.Products.Where(a => a.Product_ID == proid).SingleOrDefault();

                if (qid == null)
                {
                    return RedirectToAction("ListLink");
                }
                else
                {
                    var qiddownlaod = db.Downloads.OrderByDescending(a => a.Download_ID).FirstOrDefault();

                    qiddownlaod.Download_ProductID = proid;
                    db.Downloads.Attach(qiddownlaod);
                    db.Entry(qiddownlaod).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ListLink");
                }
            }
            else
            {
                TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                return RedirectToAction("ListLink");
            }

        }

        public ActionResult InterimBill()
        {
            return RedirectToAction("Profile");
        }


        [HttpPost]
        public ActionResult InterimBill(int idpro = 0, int nopro = 0)
        {
            try
            {

                if (idpro == 0)
                {
                    return RedirectToAction("Profile");
                }

                if (Session["User"] == null)
                    return RedirectToAction("Profile");

                string username = Session["User"].ToString();
                string rolename = Session["Role"].ToString();


                if (rolename == "Seller" || rolename == "Admin")
                {
                    var quser = db.Users.Where(a => a.User_UserName == username && (a.Role.Role_Name == rolename)).SingleOrDefault();

                    if (quser != null)
                    {
                        var quser1 = db.Identities.Where(a => a.Identity_UserID == quser.User_ID && a.Identity_Confirm == true).SingleOrDefault();

                        if (quser1 != null)
                        {

                            var qid = db.Products.Where(a => a.Product_ID == idpro && a.Product_ExitCount >= nopro && a.Product_Active == true).SingleOrDefault();
                            if (qid == null)
                            {
                                TempData["Error"] = "تعداد انتخابی بیشتر از موجودی است";
                                return RedirectToAction("ShowProduct", "Home", new { id = idpro });
                            }
                            else
                            {
                                //اگر کاربر این محصول را یکبار پیش فاکتور کرده و پرداخت نشده اجازه ندارد دوباره ثبت سفارش بکند
                                var qidib = db.InterimBills.Where(a => a.InterimBill_ProductID == idpro && a.InterimBill_Buyer == quser.User_ID).SingleOrDefault();
                                if (qidib != null)
                                {
                                    //برای اینکه اگر کاربر دوباره تعداد محصول را تغیر داد برای محاسبه داشته باشیم
                                    //Boolean tru=false;
                                    var count = qidib.InterimBill_Count;
                                    //if (nopro != 0) 
                                    //{ 
                                    //    count=qidib.InterimBill_Count = nopro;

                                    //    tru = true;
                                    //}


                                    ViewBag.Count = count;
                                    if (qid.Product_IsDownload == false)
                                    {

                                        var weightpro = db.Weights.Where(a => a.Weight_Min <= qid.Product_Weight && a.Weight_Max >= qid.Product_Weight).SingleOrDefault();
                                        double discount = Convert.ToDouble(db.Settings.OrderBy(a => a.Setting_ID).FirstOrDefault().Discount);
                                        double price = Convert.ToDouble(qid.Product_Price);
                                        double dic_pro = Convert.ToDouble(qid.Product_Off);

                                        if (qid.Product_Off > 0 && qid.Product_AllOff == true)
                                        {
                                            double percent1 = Math.Ceiling(price - (((discount + dic_pro) / 100) * price));
                                            percent1 *= Convert.ToDouble(count);
                                            double Maliat = Math.Ceiling((0.09) * percent1);
                                            double prices = (Math.Ceiling((percent1 + Maliat) / 100) * 100);
                                            int pr = Convert.ToInt32(weightpro.Weight_Price) + Convert.ToInt32(prices);

                                            ViewBag.Price = prices;
                                            ViewBag.Postage = Convert.ToInt32(weightpro.Weight_Price);
                                            ViewBag.Pay = pr;
                                        }
                                        else if (qid.Product_AllOff == true && qid.Product_Off == 0)
                                        {
                                            double percent2 = Math.Ceiling(price - ((discount / 100) * price));
                                            percent2 *= Convert.ToDouble(count);
                                            double Maliat = Math.Ceiling((0.09) * percent2);
                                            double prices = (Math.Ceiling((percent2 + Maliat) / 100) * 100);
                                            int pr = Convert.ToInt32(weightpro.Weight_Price) + Convert.ToInt32(prices);

                                            ViewBag.Price = prices;
                                            ViewBag.Postage = Convert.ToInt32(weightpro.Weight_Price);
                                            ViewBag.Pay = pr;
                                        }
                                        else if (qid.Product_AllOff == false && qid.Product_Off > 0)
                                        {
                                            double percent3 = (price - ((dic_pro / 100) * price));
                                            percent3 *= Convert.ToDouble(count);
                                            double Maliat = Math.Ceiling((0.09) * percent3);
                                            double prices = (Math.Ceiling((percent3 + Maliat) / 100) * 100);
                                            int pr = Convert.ToInt32(weightpro.Weight_Price) + Convert.ToInt32(prices);

                                            ViewBag.Price = prices;
                                            ViewBag.Postage = Convert.ToInt32(weightpro.Weight_Price);
                                            ViewBag.Pay = pr;
                                        }
                                        else if (qid.Product_Off == 0 && qid.Product_AllOff == false)
                                        {
                                            double percent4 = qid.Product_Price;
                                            percent4 *= Convert.ToDouble(count);
                                            double Maliat = Math.Ceiling((0.09) * percent4);
                                            double prices = (Math.Ceiling((percent4 + Maliat) / 100) * 100);
                                            int pr = Convert.ToInt32(weightpro.Weight_Price) + Convert.ToInt32(prices);

                                            ViewBag.Price = prices;
                                            ViewBag.Postage = Convert.ToInt32(weightpro.Weight_Price);
                                            ViewBag.Pay = pr;
                                        }



                                        ViewBag.Date = qidib.InterimBill_Date;
                                        ViewBag.IdInBill = qidib.InterimBill_ID;
                                        //برای ذخیره کردن تعداد محصول بالا اگر کاربر تغیر داده بود
                                        //if(tru)
                                        //{
                                        //   db.InterimBills.Attach(qidib);
                                        //   db.Entry(qidib).State = System.Data.Entity.EntityState.Modified;
                                        //   db.SaveChanges();
                                        //}

                                        return View(qid);
                                    }
                                    else
                                    {

                                        ViewBag.Pay = qidib.InterimBill_PayPrice;
                                        ViewBag.Date = qidib.InterimBill_Date;
                                        ViewBag.IdInBill = qidib.InterimBill_ID;


                                        double discount = Convert.ToDouble(db.Settings.OrderBy(a => a.Setting_ID).FirstOrDefault().Discount);
                                        double price = Convert.ToDouble(qid.Product_Price);
                                        double dic_pro = Convert.ToDouble(qid.Product_Off);

                                        if (qid.Product_Off > 0 && qid.Product_AllOff == true)
                                        {
                                            double percent1 = Math.Ceiling(price - (((discount + dic_pro) / 100) * price));
                                            percent1 *= Convert.ToDouble(count);
                                            double Maliat = Math.Ceiling((0.09) * percent1);
                                            double prices = Math.Ceiling(percent1 + Maliat);
                                            double ps = Math.Ceiling(prices / 100) * 100;
                                            int pr = Convert.ToInt32(ps);

                                            ViewBag.Price = ps;
                                            ViewBag.Postage = 0;
                                            ViewBag.Pay = pr;
                                        }
                                        else if (qid.Product_AllOff == true && qid.Product_Off == 0)
                                        {
                                            double percent2 = Math.Ceiling(price - ((discount / 100) * price));
                                            percent2 *= Convert.ToDouble(count);
                                            double Maliat = Math.Ceiling((0.09) * percent2);
                                            double prices = (percent2 + Maliat);
                                            double ps = Math.Ceiling(prices / 100) * 100;
                                            int pr = Convert.ToInt32(ps);

                                            ViewBag.Price = ps;
                                            ViewBag.Postage = 0;
                                            ViewBag.Pay = pr;
                                        }
                                        else if (qid.Product_AllOff == false && qid.Product_Off > 0)
                                        {
                                            double percent3 = Math.Ceiling(price - ((dic_pro / 100) * price));
                                            percent3 *= Convert.ToDouble(count);
                                            double Maliat = Math.Ceiling((0.09) * percent3);
                                            double prices = Math.Ceiling(percent3 + Maliat);
                                            double ps = Math.Ceiling(prices / 100) * 100;
                                            int pr = Convert.ToInt32(ps);

                                            ViewBag.Price = ps;
                                            ViewBag.Postage = 0;
                                            ViewBag.Pay = pr;
                                        }
                                        else if (qid.Product_Off == 0 && qid.Product_AllOff == false)
                                        {
                                            double percent4 = qid.Product_Price;
                                            percent4 *= Convert.ToDouble(count);
                                            double Maliat = Math.Ceiling((0.09) * percent4);
                                            double prices = Math.Ceiling(percent4 + Maliat);
                                            double ps = Math.Ceiling(prices / 100) * 100;
                                            int pr = Convert.ToInt32(ps);

                                            ViewBag.Price = ps;
                                            ViewBag.Postage = 0;
                                            ViewBag.Pay = pr;
                                        }
                                        //if (tru)
                                        //{
                                        //    db.InterimBills.Attach(qidib);
                                        //    db.Entry(qidib).State = System.Data.Entity.EntityState.Modified;
                                        //    db.SaveChanges();

                                        //}


                                        return View(qid);
                                    }


                                }
                                //برای بار اول که میخواهیم محصولی را خرید کنیم چک میکند که تعداد محصول خالی نباشد
                                if (nopro == 0)
                                {
                                    TempData["Error"] = "لطفا تعداد محصول را انتخاب کنید";
                                    return RedirectToAction("ShowProduct", "Home", new { id = idpro });
                                }
                                Random rnd = new Random();

                                InterimBill ib = new InterimBill();

                                ib.InterimBill_Buyer = quser.User_ID;
                                ib.InterimBill_Date = DateTime.Today;
                                ib.InterimBill_ExpDate = ib.InterimBill_Date.AddDays(10);
                                ib.InterimBill_Count = nopro;
                                if (qid.Product_IsDownload == false)
                                {
                                    int count = nopro;
                                    ViewBag.Count = count;
                                    double discount = Convert.ToDouble(db.Settings.OrderBy(a => a.Setting_ID).FirstOrDefault().Discount);
                                    double price = Convert.ToDouble(qid.Product_Price);
                                    double dic_pro = Convert.ToDouble(qid.Product_Off);

                                    // وزن  
                                    // از 0 تا 100       101 تا 200  201 تا 500      501 تا 700    701 تا 1000   1001 تا 2000   2001 تا  بینهایت 
                                    var weightpro = db.Weights.Where(a => a.Weight_Min <= qid.Product_Weight && a.Weight_Max >= qid.Product_Weight).SingleOrDefault();



                                    if (qid.Product_Off > 0 && qid.Product_AllOff == true)
                                    {
                                        double percent1 = Math.Ceiling(price - (((discount + dic_pro) / 100) * price));
                                        percent1 *= count;
                                        double Maliat = Math.Ceiling((0.09) * percent1);
                                        double prices = (Math.Ceiling((percent1 + Maliat) / 100) * 100);
                                        int pr = Convert.ToInt32(weightpro.Weight_Price) + Convert.ToInt32(prices);

                                        ib.InterimBill_PayPrice = pr;

                                        ViewBag.Price = prices;

                                        ib.InterimBill_Postage = Convert.ToInt32(weightpro.Weight_Price);

                                        ViewBag.Postage = Convert.ToInt32(weightpro.Weight_Price);
                                        ViewBag.Pay = pr;
                                    }
                                    else if (qid.Product_AllOff == true && qid.Product_Off == 0)
                                    {
                                        double percent2 = Math.Ceiling(price - ((discount / 100) * price));
                                        percent2 *= count;
                                        double Maliat = Math.Ceiling((0.09) * percent2);
                                        double prices = (Math.Ceiling((percent2 + Maliat) / 100) * 100);
                                        int pr = Convert.ToInt32(weightpro.Weight_Price) + Convert.ToInt32(prices);

                                        ib.InterimBill_PayPrice = pr;

                                        ViewBag.Price = prices;

                                        ib.InterimBill_Postage = Convert.ToInt32(weightpro.Weight_Price);

                                        ViewBag.Postage = Convert.ToInt32(weightpro.Weight_Price);
                                        ViewBag.Pay = pr;
                                    }
                                    else if (qid.Product_AllOff == false && qid.Product_Off > 0)
                                    {
                                        double percent3 = Math.Ceiling(price - ((dic_pro / 100) * price));
                                        percent3 *= count;
                                        double Maliat = Math.Ceiling((0.09) * percent3);
                                        double prices = (Math.Ceiling((percent3 + Maliat) / 100) * 100);
                                        int pr = Convert.ToInt32(weightpro.Weight_Price) + Convert.ToInt32(prices);

                                        ib.InterimBill_PayPrice = pr;

                                        ViewBag.Price = prices;

                                        ib.InterimBill_Postage = Convert.ToInt32(weightpro.Weight_Price);

                                        ViewBag.Postage = Convert.ToInt32(weightpro.Weight_Price);
                                        ViewBag.Pay = pr;
                                    }
                                    else if (qid.Product_Off == 0 && qid.Product_AllOff == false)
                                    {
                                        double percent4 = qid.Product_Price;
                                        percent4 *= count;
                                        double Maliat = Math.Ceiling((0.09) * percent4);
                                        double prices = (Math.Ceiling((percent4 + Maliat) / 100) * 100);
                                        int pr = Convert.ToInt32(weightpro.Weight_Price) + Convert.ToInt32(prices);

                                        ib.InterimBill_PayPrice = pr;

                                        ViewBag.Price = prices;

                                        ib.InterimBill_Postage = Convert.ToInt32(weightpro.Weight_Price);

                                        ViewBag.Postage = Convert.ToInt32(weightpro.Weight_Price);
                                        ViewBag.Pay = pr;
                                    }


                                }
                                else
                                {
                                    int count1 = nopro;
                                    ViewBag.Count = count1;
                                    double discount = Convert.ToDouble(db.Settings.OrderBy(a => a.Setting_ID).FirstOrDefault().Discount);
                                    double price = Convert.ToDouble(qid.Product_Price);
                                    double dic_pro = Convert.ToDouble(qid.Product_Off);

                                    if (qid.Product_Off > 0 && qid.Product_AllOff == true)
                                    {
                                        double percent1 = Math.Ceiling(price - (((discount + dic_pro) / 100) * price));
                                        percent1 *= count1;
                                        double Maliat = Math.Ceiling((0.09) * percent1);
                                        double prices = (Math.Ceiling((percent1 + Maliat) / 100) * 100);
                                        int pr = Convert.ToInt32(prices);

                                        ib.InterimBill_PayPrice = pr;

                                        ViewBag.Price = prices;

                                        ib.InterimBill_Postage = 0;

                                        ViewBag.Postage = 0;
                                        ViewBag.Pay = pr;
                                    }
                                    else if (qid.Product_AllOff == true && qid.Product_Off == 0)
                                    {
                                        double percent2 = Math.Ceiling(price - ((discount / 100) * price));
                                        percent2 *= count1;
                                        double Maliat = Math.Ceiling((0.09) * percent2);
                                        double prices = (Math.Ceiling((percent2 + Maliat) / 100) * 100);
                                        int pr = Convert.ToInt32(prices);

                                        ib.InterimBill_PayPrice = pr;

                                        ViewBag.Price = prices;

                                        ib.InterimBill_Postage = 0;

                                        ViewBag.Postage = 0;
                                        ViewBag.Pay = pr;
                                    }
                                    else if (qid.Product_AllOff == false && qid.Product_Off > 0)
                                    {
                                        double percent3 = Math.Ceiling(price - ((dic_pro / 100) * price));
                                        percent3 *= count1;
                                        double Maliat = Math.Ceiling((0.09) * percent3);
                                        double prices = (Math.Ceiling((percent3 + Maliat) / 100) * 100);
                                        int pr = Convert.ToInt32(prices);

                                        ib.InterimBill_PayPrice = pr;

                                        ViewBag.Price = prices;

                                        ib.InterimBill_Postage = 0;

                                        ViewBag.Postage = 0;
                                        ViewBag.Pay = pr;
                                    }
                                    else if (qid.Product_Off == 0 && qid.Product_AllOff == false)
                                    {
                                        double percent4 = qid.Product_Price;
                                        percent4 *= count1;
                                        double Maliat = Math.Ceiling((0.09) * percent4);
                                        double prices = (Math.Ceiling((percent4 + Maliat) / 100) * 100);
                                        int pr = Convert.ToInt32(prices);

                                        ib.InterimBill_PayPrice = pr;

                                        ViewBag.Price = prices;

                                        ib.InterimBill_Postage = 0;

                                        ViewBag.Postage = 0;
                                        ViewBag.Pay = pr;
                                    }


                                }

                                ib.InterimBill_ProductID = qid.Product_ID;
                                ib.InterimBill_Seller = qid.User.User_NameFamily;
                                ib.InvoiceNumber = ((rnd.Next(1, 1000) * DateTime.Now.Year) + DateTime.Now.Second).ToString();

                                db.InterimBills.Add(ib);
                                db.SaveChanges();//می توان خطای ایجاد موفقیت امیز نبودن ثبت پیش فاکتور اعلام شود

                                ViewBag.IdInBill = db.InterimBills.OrderByDescending(a => a.InterimBill_ID).FirstOrDefault().InterimBill_ID;
                                return View(qid);
                            }

                        }
                        else
                        {
                            TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                            return RedirectToAction("Profile");
                        }

                    }
                    else
                    {
                        TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                        return RedirectToAction("Profile");
                    }

                }
                else
                {
                    TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                    return RedirectToAction("Profile");
                }


            }//try end
            catch
            {
                return RedirectToAction("Profile");
            }
        }

        public ActionResult ListInBill()
        {
            try
            {

                if (Session["User"] == null)
                    return RedirectToAction("Profile");

                string username = Session["User"].ToString();
                string rolename = Session["Role"].ToString();


                if (rolename == "Seller" || rolename == "Admin")
                {
                    var quser = db.Users.Where(a => a.User_UserName == username && (a.Role.Role_Name == rolename)).SingleOrDefault();

                    if (quser != null)
                    {
                        var quser1 = db.Identities.Where(a => a.Identity_UserID == quser.User_ID && a.Identity_Confirm == true).SingleOrDefault();

                        if (quser1 != null)
                        {
                            //باید بررسی بشود سفارشی که ثبت موقت شده زمان انقضای ان معتبر است یا خیر و همچنین پیام پرداخت ارسال شود

                            var q = db.InterimBills.Where(a => a.InterimBill_Buyer == quser.User_ID).ToList();

                            if (q == null)
                            {
                                ViewBag.Error = TempData["Error"];
                                return View();

                            }
                            else
                            {
                                ViewBag.Error = TempData["Error"];
                                return View(q);
                            }

                        }
                        else
                        {
                            TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                            return RedirectToAction("Profile");
                        }

                    }
                    else
                    {
                        TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                        return RedirectToAction("Profile");
                    }

                }
                else
                {
                    TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                    return RedirectToAction("Profile");
                }
            }
            catch
            {

                return RedirectToAction("Profile");
            }
        }

        [HttpPost]
        public ActionResult DeleteInBill(int idinterimbill)
        {
            try
            {

                if (Session["User"] == null)
                    return RedirectToAction("Profile");

                string username = Session["User"].ToString();
                string rolename = Session["Role"].ToString();


                if (rolename == "Seller" || rolename == "Admin")
                {
                    var quser = db.Users.Where(a => a.User_UserName == username && (a.Role.Role_Name == rolename)).SingleOrDefault();

                    if (quser != null)
                    {
                        var quser1 = db.Identities.Where(a => a.Identity_UserID == quser.User_ID && a.Identity_Confirm == true).SingleOrDefault();

                        if (quser1 != null)
                        {

                            var qidib = db.InterimBills.Where(a => a.InterimBill_ID == idinterimbill && a.InterimBill_Buyer == quser.User_ID).SingleOrDefault();
                            if (qidib == null)
                            {

                                return RedirectToAction("Profile", "Profile");

                            }
                            else
                            {
                                db.InterimBills.Remove(qidib);
                                if (Convert.ToBoolean(db.SaveChanges() > 0))
                                {
                                    //نمایش پیام مناسب روی پروفایل
                                    TempData["Error"] = "پیش فاکتور مورد نظر با موفقیت حذف شد";
                                    return RedirectToAction("Profile", "Profile");
                                }
                                else
                                {

                                    //نمایش پیام مناسب روی پروفایل
                                    TempData["Error"] = "پیش فاکتور مورد نظر با موفقیت حذف نشد";
                                    return RedirectToAction("Profile", "Profile");
                                }
                            }
                        }
                        else
                        {
                            TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                            return RedirectToAction("Profile");
                        }

                    }
                    else
                    {
                        TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                        return RedirectToAction("Profile");
                    }

                }
                else
                {
                    TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                    return RedirectToAction("Profile");
                }

            }
            catch
            {

                TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                return RedirectToAction("Profile", "Profile");
            }
        }

        [HttpGet]
        public ActionResult BankAccount()
        {

            if (Session["User"] == null)
                return RedirectToAction("Profile");

            string username = Session["User"].ToString();
            string rolename = Session["Role"].ToString();


            if (rolename == "Seller" || rolename == "Admin")
            {
                var quser = db.Users.Where(a => a.User_UserName == username && (a.Role.Role_Name == rolename)).SingleOrDefault();

                if (quser != null)
                {
                    var quser1 = db.Identities.Where(a => a.Identity_UserID == quser.User_ID && a.Identity_Confirm == true).SingleOrDefault();

                    if (quser1 != null)
                    {

                        var qbank = db.NoBanks.Where(a => a.Banks_UserID == quser.User_ID).FirstOrDefault();


                        if (qbank == null)
                        {
                            return View();
                        }
                        else
                        {
                            ViewBag.Error = TempData["Error"];

                            return View(qbank);
                        }


                    }

                    else
                    {
                        TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                        return RedirectToAction("Profile");
                    }

                }
                else
                {
                    TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                    return RedirectToAction("Profile");
                }

            }

            else
            {
                TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                return RedirectToAction("Profile");
            }
        }

        [HttpPost]
        public ActionResult AddBankAccount(string AccountCode, string CartCode, string ShebaCode, string bank_id, string NameUser)
        {
            try
            {
                if (Session["User"] == null)
                    return RedirectToAction("Profile");

                string username = Session["User"].ToString();
                string rolename = Session["Role"].ToString();


                if (rolename == "Seller" || rolename == "Admin")
                {
                    var quser = db.Users.Where(a => a.User_UserName == username && (a.Role.Role_Name == rolename)).SingleOrDefault();

                    if (quser != null)
                    {
                        var quser1 = db.Identities.Where(a => a.Identity_UserID == quser.User_ID && a.Identity_Confirm == true).SingleOrDefault();

                        if (quser1 != null)
                        {
                            NoBank t = new NoBank();
                            t.Banks_NameID = Convert.ToInt32(bank_id);
                            t.Banks_NameUser = NameUser;
                            t.Banks_NoBank = AccountCode;
                            t.Banks_NoCart = CartCode;
                            t.Banks_NoIR = ShebaCode;
                            t.Banks_UserID = quser.User_ID;

                            db.NoBanks.Add(t);
                            db.SaveChanges();

                            TempData["Error"] = "شماره حساب ثبت شد";

                            return RedirectToAction("BankAccount");

                        }

                        else
                        {
                            TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                            return RedirectToAction("Profile");
                        }

                    }
                    else
                    {
                        TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                        return RedirectToAction("Profile");
                    }

                }

                else
                {
                    TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                    return RedirectToAction("Profile");
                }
            }
            catch
            {
                TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                return RedirectToAction("Profile");

            }
        }

        public ActionResult EditBank(int id)
        {
            try
            {
                if (Session["User"] == null)
                    return RedirectToAction("Profile");

                string username = Session["User"].ToString();
                string rolename = Session["Role"].ToString();


                if (rolename == "Seller" || rolename == "Admin")
                {
                    var quser = db.Users.Where(a => a.User_UserName == username && (a.Role.Role_Name == rolename)).SingleOrDefault();

                    if (quser != null)
                    {
                        var quser1 = db.Identities.Where(a => a.Identity_UserID == quser.User_ID && a.Identity_Confirm == true).SingleOrDefault();

                        if (quser1 != null)
                        {


                            var qedit = db.NoBanks.Where(a => a.Banks_UserID == quser.User_ID && a.Banks_ID == id).SingleOrDefault();


                            if (qedit == null)
                            {
                                TempData["Error"] = "در عملیات ویرایش مشکلی رخ داد";

                                return RedirectToAction("BankAccount");
                            }
                            else
                            {
                                return View(qedit);
                            }

                        }

                        else
                        {
                            TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                            return RedirectToAction("Profile");
                        }

                    }
                    else
                    {
                        TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                        return RedirectToAction("Profile");
                    }

                }

                else
                {
                    TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                    return RedirectToAction("Profile");
                }
            }
            catch
            {
                TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                return RedirectToAction("Profile");

            }
        }

        [HttpPost]
        public ActionResult EditBankAccount(string AccountCode, string CartCode, string ShebaCode, string bank_id, string NameUser, int IDBank = 0)
        {
            try
            {
                if (Session["User"] == null)
                    return RedirectToAction("Profile");

                string username = Session["User"].ToString();
                string rolename = Session["Role"].ToString();


                if (rolename == "Seller" || rolename == "Admin")
                {
                    var quser = db.Users.Where(a => a.User_UserName == username && (a.Role.Role_Name == rolename)).SingleOrDefault();

                    if (quser != null)
                    {
                        var quser1 = db.Identities.Where(a => a.Identity_UserID == quser.User_ID && a.Identity_Confirm == true).SingleOrDefault();

                        if (quser1 != null)
                        {

                            var qid = db.NoBanks.Where(a => a.Banks_ID == IDBank && a.Banks_UserID == quser.User_ID).SingleOrDefault();
                            if (qid == null)
                            {
                                TempData["Error"] = "ویرایش  انجام نشد";
                                return RedirectToAction("BankAccount");
                            }
                            else
                            {
                                qid.Banks_NameID = Convert.ToInt32(bank_id);
                                qid.Banks_NameUser = NameUser;
                                qid.Banks_NoBank = AccountCode;
                                qid.Banks_NoCart = CartCode;
                                qid.Banks_NoIR = ShebaCode;

                                db.NoBanks.Attach(qid);
                                db.Entry(qid).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();

                                TempData["Error"] = "ویرایش انجام شد";
                                ;
                                return RedirectToAction("BankAccount");

                            }
                        }

                        else
                        {
                            TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                            return RedirectToAction("Profile");
                        }

                    }
                    else
                    {
                        TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                        return RedirectToAction("Profile");
                    }

                }

                else
                {
                    TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                    return RedirectToAction("Profile");
                }
            }
            catch
            {
                TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                return RedirectToAction("Profile");

            }
        }

        public ActionResult LsitMesage()
        {
            if (Session["User"] == null)
                return RedirectToAction("Profile");

            string username = Session["User"].ToString();

            var quser = db.Users.Where(a => a.User_UserName == username).SingleOrDefault();

            if (quser != null)
            {
                var quser1 = db.Identities.Where(a => a.Identity_UserID == quser.User_ID && a.Identity_Confirm == true).SingleOrDefault();

                if (quser1 != null)
                {
                    var qmsg = db.Massages.Where(a => a.Massage_UserGet == quser.User_ID).OrderByDescending(a => a.Massage_Date).ToList();

                    if (qmsg == null)
                    {
                        ViewBag.Error = TempData["Error"];
                        return View();
                    }
                    else
                    {
                        ViewBag.Error = TempData["Error"];
                        return View(qmsg.OrderByDescending(a => a.Massage_Read == false));
                    }
                }
                else
                {
                    TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                    return RedirectToAction("Profile");
                }
            }
            else
            {
                TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                return RedirectToAction("Profile");
            }

        }

        public ActionResult ShowMsg(int id = 0)
        {
            if (Session["User"] == null)
                return RedirectToAction("Profile");

            if (id == 0)
            {
                TempData["Error"] = "مشاهده ی  پیام با مشکل رو به رو شد!";
                return RedirectToAction("LsitMesage");
            }

            string username = Session["User"].ToString();

            var quser = db.Users.Where(a => a.User_UserName == username).SingleOrDefault();

            if (quser != null)
            {
                var quser1 = db.Identities.Where(a => a.Identity_UserID == quser.User_ID && a.Identity_Confirm == true).SingleOrDefault();

                if (quser1 != null)
                {
                    var qmsgshow = db.Massages.Where(a => a.Massage_UserGet == quser.User_ID && a.Massage_ID == id).FirstOrDefault();

                    if (qmsgshow == null)
                    {
                        TempData["Error"] = "مشاهده ی  پیام با مشکل رو به رو شد!";
                        return RedirectToAction("LsitMesage");
                    }
                    else
                    {
                        qmsgshow.Massage_Read = true;
                        db.Massages.Attach(qmsgshow);
                        db.Entry(qmsgshow).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        ViewBag.Error = TempData["Error"];


                        return View(qmsgshow);

                    }
                }
                else
                {
                    TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                    return RedirectToAction("Profile");
                }
            }
            else
            {
                TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                return RedirectToAction("Profile");
            }
        }

        public ActionResult DeleteMsg(int id)
        {

            if (Session["User"] == null)
                return RedirectToAction("Profile");


            string username = Session["User"].ToString();

            var quser = db.Users.Where(a => a.User_UserName == username).SingleOrDefault();

            if (quser != null)
            {
                var quser1 = db.Identities.Where(a => a.Identity_UserID == quser.User_ID && a.Identity_Confirm == true).SingleOrDefault();

                if (quser1 != null)
                {

                    var qdelete = db.Massages.Where(a => a.Massage_ID == id).FirstOrDefault();

                    if (qdelete == null)
                    {
                        TempData["Error"] = "حذف پیام با مشکل رو به رو شد!";

                        return RedirectToAction("LsitMesage");
                    }
                    else
                    {
                        db.Massages.Remove(qdelete);

                        if (Convert.ToBoolean(db.SaveChanges() > 0))
                        {
                            TempData["Error"] = "حذف پیام با موفقیت انجام شد!";
                            return RedirectToAction("LsitMesage");
                        }
                        else
                        {
                            TempData["Error"] = "حذف پیام با مشکل رو به رو شد!";
                            return RedirectToAction("LsitMesage");
                        }

                    }


                }
                else
                {
                    TempData["Error"] = "حذف پیام با مشکل رو به رو شد!";
                    return RedirectToAction("LsitMesage");
                }
            }
            else
            {
                TempData["Error"] = "حذف پیام با مشکل رو به رو شد!";
                return RedirectToAction("LsitMesage");
            }


        }

        [HttpPost]
        public ActionResult SendMsg(string BMesg, string TMesg, string UserGet)
        {
            if (Session["User"] == null)
                return RedirectToAction("Profile");


            string username = Session["User"].ToString();

            var quser = db.Users.Where(a => a.User_UserName == username).SingleOrDefault();

            if (quser != null)
            {
                var quser1 = db.Identities.Where(a => a.Identity_UserID == quser.User_ID && a.Identity_Confirm == true).SingleOrDefault();

                if (quser1 != null)
                {
                    var quserget = db.Users.Where(a => a.User_UserName == UserGet).FirstOrDefault();
                    if (quserget == null)
                    {
                        TempData["Error"] = "کاربر مورد نظر یافت نشد!";
                        return RedirectToAction("LsitMesage");
                    }

                    Massage m = new Massage();
                    m.Massage_Body = BMesg;
                    m.Massage_Date = DateTime.Now;
                    m.Massage_Read = false;
                    m.Massage_Title = TMesg;
                    m.Massage_UserGet = quserget.User_ID;
                    m.Massage_UserSend = quser.User_ID;
                    db.Massages.Add(m);
                    db.SaveChanges();
                    TempData["Error"] = "پیام ارسال شد";
                    return RedirectToAction("LsitMesage");
                }
                else
                {
                    TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                    return RedirectToAction("Profile");
                }
            }
            else
            {
                TempData["Error"] = "کاربر گرامی هویت شما هنوز تایید نشده ";
                return RedirectToAction("profile");
            }
        }

        public ActionResult AddMsg()
        {
            if (Session["User"] == null)
                return RedirectToAction("Profile");


            string username = Session["User"].ToString();

            var quser = db.Users.Where(a => a.User_UserName == username).SingleOrDefault();

            if (quser != null)
            {
                var quser1 = db.Identities.Where(a => a.Identity_UserID == quser.User_ID && a.Identity_Confirm == true).SingleOrDefault();

                if (quser1 != null)
                {
                    return View();
                }
                else
                {

                    return RedirectToAction("LsitMesage");
                }
            }
            else
            {
                return RedirectToAction("LsitMesage");
            }
        }

        public ActionResult ChangePassword()
        {

            return View();

        }

        [HttpPost]
        public ActionResult ChangePassword(string passwordold, string passwordnew)
        {
            

                if (Session["User"] == null)
                    return RedirectToAction("Login", "Register");

                string username = Session["User"].ToString();

                var user = db.Users.Where(a => a.User_UserName == username).SingleOrDefault();

                if (user != null)
                {
                    var useridentity = db.Identities.Where(a => a.Identity_UserID == user.User_ID && a.Identity_Confirm == true).SingleOrDefault();

                    if (useridentity != null)
                    {
                       if( user.User_Password==passwordold)
                       {
                           user.User_Password = passwordnew;

                           db.Users.Attach(user);
                           db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                           db.SaveChanges();
                         // TempData["Error"] = "رمز شما با موفقیت تغیر یافت";
                           Session["User"] = null;
                           Session["Role"] = null;
                          return RedirectToAction("Index","Home");
                       }
                       else
                       {
                           ViewBag.Error = "رمز فعلی درست نمی باشد";
                           return View();
                       }

                    }
                    else
                    {
                        TempData["Error"] = "کاربری گرامی هویت شما هنوز تائید نشده ";
                        return RedirectToAction("Profile");
                    }
                }
                else
                {
                    TempData["Error"] = "کاربری گرامی هویت شما هنوز تائید نشده ";
                    return RedirectToAction("Profile");
                }
            
            }
           
        }
    }//end
