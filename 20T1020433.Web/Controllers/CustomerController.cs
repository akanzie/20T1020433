using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _20T1020433.BusinessLayers;
using _20T1020433.DomainModels;

namespace _20T1020433.Web.Controllers
{
    public class CustomerController : Controller
    {
        private const int PAGE_SIZE = 5;
        private const string CUSTOMER_SEARCH = "SearchSupplierCondition";
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // GET: Customer
        public ActionResult Index(int page = 1, int pageSize = 10, string searchValue = "")
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfCustomers(page, pageSize, searchValue, out rowCount);

            int pageCount = rowCount / pageSize;
            if (rowCount % pageSize > 0)
                pageCount += 1;

            ViewBag.Page = page;
            ViewBag.PageCount = pageCount;
            ViewBag.RowCount = rowCount;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchValue = searchValue;
            return View(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var data = new Customer()
            {
                CustomerID = 0
            };
            ViewBag.Title = "Bổ sung nhà cung cấp";
            return View("Edit", data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            int customerId = Convert.ToInt32(id);

            var data = CommonDataService.GetCustomer(customerId);
            ViewBag.Title = "Cập nhật nhà cung cấp";
            return View(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(Customer data)
        {
            if (data.CustomerID == 0)
            {
                CommonDataService.AddCustomer(data);
            }
            else
            {
                CommonDataService.UpdateCustomer(data);
            }
            return RedirectToAction("Index");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete(string id)
        {
            int customerId = Convert.ToInt32(id);
            if (Request.HttpMethod == "GET")
            {
                var data = CommonDataService.GetCustomer(customerId);
                return View(data);
            }
            else
            {
                CommonDataService.DeleteCustomer(customerId);
                return RedirectToAction("Index");
            }
        }
    }
}