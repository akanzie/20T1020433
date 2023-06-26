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
        // GET: Home
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {            
            return View();
        }
    }
}