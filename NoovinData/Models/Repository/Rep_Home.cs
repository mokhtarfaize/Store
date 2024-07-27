using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NoovinData.Models.Domin;
namespace NoovinData.Models.Repository
{
    public class Rep_Home
    {
        DB db = new DB();
        public IEnumerable<Slider> Get_slid()
        {
            var slid = db.Sliders.Where(b=>b.Enable==true).OrderBy(a => a.Sort).ToList();
            return slid.AsEnumerable();
        }

        public IEnumerable<Product> Get_NewProduct()
        {
            var newproduct=db.Products.Where(a=>a.Product_Active==true && a.User.User_Active==true).OrderByDescending(a=>a.Product_Date).ToList().Take(15);
            return newproduct.AsEnumerable();
        }
        public IEnumerable<Product> Get_AmarProduct()
        {
            var amarpro=db.Products.Where(a=>a.Product_Active==true && a.User.User_Active==true).ToList();
            return amarpro.AsEnumerable();
        }
        public  IEnumerable<User> Get_User()
        {
            var user=db.Users.Where(a=>a.User_Active==true).ToList();
            return user.AsEnumerable();
        }
        public IEnumerable<Product> Get_Product()
        {
            var proq=db.Products.OrderBy(a=>a.Product_Date).ToList();
            return proq.AsEnumerable();
        }
        public IEnumerable<Setting> Get_Off()
        {
            var offq = from off in db.Settings select off;
            return offq.AsEnumerable();
        }
        public IEnumerable<Pic> Get_Pic()
        {
            var picq=db.Pics.OrderBy(a=>a.Pic_Name ).ToList();
            return picq.AsEnumerable();
        }
        public IEnumerable<Tag>Get_Tag()
        {
            var tagq = db.Tags.OrderBy(a => a.Tag_Name).ToList();
            return tagq.AsEnumerable();
        }
        public IEnumerable<Group>Get_Group()
        {
            var grpq = from gro in db.Groups select gro;
            return grpq.AsEnumerable();
        }
        public IEnumerable<InterimBill> Get_InteerimBill()
        {
            var inter = from iter in db.InterimBills select iter;

            return inter.AsEnumerable();
        }

         public IEnumerable<Visit> Get_Visit()
        {
            var qvis = db.Visits.OrderBy(a => a.Visit_Date).ToList();
            return qvis.AsEnumerable();
        }
    }
}