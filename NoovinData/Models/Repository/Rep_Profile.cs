using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NoovinData.Models.Domin;
namespace NoovinData.Models.Repository
{
    public class Rep_Profile
    {

        DB db = new DB();
         public IEnumerable<User> Get_User()
        {
            var userq = from user in db.Users select user;
            return userq.AsEnumerable();
        }
         public IEnumerable<Group> Get_Group()
         {
             var groupq = from grop in db.Groups select grop;
             return groupq.AsEnumerable();
                         
         }
        public IEnumerable<Bill> Get_Bill()
         {
             var billq = from bill in db.Bills select bill;
             return billq.AsEnumerable();
                         
         }
        public IEnumerable<Identity> Get_Confirm()
        {
            var conq = from con in db.Identities select con;
            return conq.AsEnumerable();

        }
        public IEnumerable<Product> Get_Product()
        {
            var proq=from pro in db.Products select pro;
            return proq.AsEnumerable();
        }

        public IEnumerable<Massage> Get_Message()
        {
            var msgq = from msg in db.Massages select msg;
            return msgq.AsEnumerable();
        }

        public IEnumerable<Pic> Get_Gallery()
        {
            var picq=from pic in db.Pics select pic;
            return picq.AsEnumerable();
        }
        public IEnumerable<Tag> Get_Tag()
        {
            var tagq = db.Tags.ToList();
            return tagq.AsEnumerable();
        }
        public IEnumerable<Role> Get_Role()
        {
            var rolq = from rol in db.Roles select rol;
            return rolq.AsEnumerable();
        }
        public IEnumerable<Download>Get_Download()
        {
            var donq = db.Downloads.ToList();
            return donq.AsEnumerable();
        }
         
        public IEnumerable<PostStatu> Get_Post()
        {
            var po = from pos in db.PostStatus select pos;
            return po.AsEnumerable();
        }

        public IEnumerable<Bill> Get_Sale()
        {
            var q = from a in db.Bills.OrderByDescending(a => a.Bill_Date).ToList() select a;
            return q.AsEnumerable();
        }

        public IEnumerable<Withdrawal>Get_Withdrawal()
        {
            var q = db.Withdrawals.OrderBy(a => a.Withdrawal_UserID).ToList();
            return q.AsEnumerable();

        }

        public IEnumerable<NoBank> Get_NBank()
        {
            var qs = from a in db.NoBanks
                     join b in db.BankNames on a.Banks_NameID equals b.Bank_ID
                     select a;

            // var q = db.Tbl_NoBank.OrderBy(a => a.Banks_ID).ToList();
            return qs.AsEnumerable();
        }

        public IEnumerable<BankName> Get_NameBank()
        {
            var q = db.BankNames.OrderBy(a => a.Bank_Name).ToList();
            return q.AsEnumerable();
        }

        public IEnumerable<Setting> Get_Setting()
        {
            var qse = (from set in db.Settings select set);
            return qse.AsEnumerable();
        }

       
    }
}