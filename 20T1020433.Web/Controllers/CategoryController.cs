using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _20T1020433.Web.Controllers
{
    public class CategoryController : Controller
    {   
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            ViewBag.Title = "Bổ sung loại hàng";
            return View("Edit");
        }

        public ActionResult Edit()
        {
            ViewBag.Title = "Cập nhật loại hàng";
            return View();
        }

        public ActionResult Delete()
        {
            ViewBag.Title = "Xóa loại hàng";
            return View();
        }
    }
}