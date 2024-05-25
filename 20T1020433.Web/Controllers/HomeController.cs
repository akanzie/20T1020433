using _20T1020433.BusinessLayers;
using _20T1020433.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _20T1020433.Web.Controllers
{
    public class HomeController : Controller
    {      

        public ActionResult Index()
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (userAccount == null)
            {
                return RedirectToAction("Login", "User");
            } 
            var recommendedProducts = ShopService.GetRecommendations(int.Parse(userAccount.UserId), 10);
            return View(recommendedProducts);
        }
    }
}