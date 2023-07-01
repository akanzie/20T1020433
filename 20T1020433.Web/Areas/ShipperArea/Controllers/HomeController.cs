using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _20T1020433.Web.Areas.ShipperArea.Controllers{
    
    public class HomeController : Controller
    {
        // GET: Shipper/Home
        public ActionResult Index()
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (userAccount == null)
            {
                return RedirectToAction("Login", "Account" , new { area = "ShipperArea" });
            }
            return View();
        }
    }
}