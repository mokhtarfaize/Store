using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NoovinData.Models.Domin;
namespace NoovinData.Controllers
{
    public class TeatAjaxBeginformController : Controller
    {
        //
        // GET: /TeatAjaxBeginform/
        DB db = new DB();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        
            public JsonResult Getpro(string Product_Name)
            {
            var patients = from c in db.Products
                           select c.Product_Name;
            var listname = patients.Where(n => n.ToLower().Contains(Product_Name.ToLower()));
                return Json(listname,JsonRequestBehavior.AllowGet);
            }
        
	}
}