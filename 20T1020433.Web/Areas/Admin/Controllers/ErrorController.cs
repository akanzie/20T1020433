using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _20T1020433.Web.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }

        // Xử lý lỗi máy chủ (500 Internal Server Error)
        public ActionResult ServerError()
        {
            Response.StatusCode = 500;
            return View();
        }
    }
}