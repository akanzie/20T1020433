using _20T1020433.DomainModels;
using _20T1020433.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _20T1020433.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class HomeAdminController : Controller
    {               
        public ActionResult Index()
        {            
            return View();
        }
    }
}