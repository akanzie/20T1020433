using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _20T1020433.Web.Controllers
{
    [RoutePrefix("thu-nghiem")]
    public class TestController : Controller
    {
        [HttpGet]
       public ActionResult Input()
        {
            Person p = new Person()
            {

            };
            return View(p);
        }
        [HttpPost]
        public ActionResult Input(Person p)
        {
            var data = new
            {
                Name = p.Name,
                BirthDate = string.Format("{0:dd/MM/yyyy}", p.BirthDate),
                Salary = p.Salary
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public string TestDate (DateTime value)
        {
            DateTime d = value;
            return string.Format("{0:dd/MM/yyyy}", d);
        }
        public decimal? TestDecimal(string s)
        {
            decimal? d = decimal.Parse("aa", CultureInfo.InvariantCulture); ;
            return d;
        }
        
    }
    public class Person
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public float Salary { get; set; }
    }
    
}