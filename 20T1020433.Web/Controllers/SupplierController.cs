using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Microsoft.Ajax.Utilities;
using _20T1020433.DomainModels;
using _20T1020433.BusinessLayers;
using _20T1020433.Web.Models;

namespace _20T1020433.Web.Controllers
{
    public class SupplierController : Controller
    {
        private const int PAGE_SIZE = 5;
        private const string SUPPLIER_SEARCH = "SearchSupplierCondition";
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // GET: Supplier
        //public ActionResult Index(int page = 1, int pageSize = 5, string searchValue = "")
        //{
        //    int rowCount = 0;
        //    var data = CommonDataService.ListOfSuppliers(page, pageSize, searchValue, out rowCount);

        //    int pageCount = rowCount / pageSize;
        //    if (rowCount % pageSize > 0)
        //        pageCount += 1;

        //    ViewBag.Page = page;
        //    ViewBag.PageCount = pageCount;
        //    ViewBag.RowCount = rowCount;
        //    ViewBag.PageSize = pageSize;
        //    ViewBag.SearchValue = searchValue;
        //    return View(data);
        //}
        public ActionResult Index()
        {
            PaginationSearchInput condition = Session[SUPPLIER_SEARCH] as PaginationSearchInput;
            if (condition == null)
            {
                condition = new PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = ""
                };
            }
            
            return View(condition);
        }

        public ActionResult Search(PaginationSearchInput condition)
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfSuppliers(condition.Page, 
                condition.PageSize, 
                condition.SearchValue,
                out rowCount);
            var result = new SupplierSearchOutput()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue,
                RowCount = rowCount,
                Data = data
            };
            Session[SUPPLIER_SEARCH] = condition;
            return View(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var data = new Supplier()
            {
                SupplierID = 0
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
            int supplierId = Convert.ToInt32(id);

            var data = CommonDataService.GetSupplier(supplierId);
            ViewBag.Title = "Cập nhật nhà cung cấp";
            return View(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(Supplier data)
        {
            if (data.SupplierID == 0)
            {
                CommonDataService.AddSupplier(data);
            }
            else
            {
                CommonDataService.UpdateSupplier(data);
            }
            return RedirectToAction("Index");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete(string id)
        {
            int supplierID = Convert.ToInt32(id);
            if (Request.HttpMethod == "GET")
            {
                var data = CommonDataService.GetSupplier(supplierID);
                return View(data);
            }
            else
            {
                CommonDataService.DeleteSupplier(supplierID);
                return RedirectToAction("Index");
            }
        }
    }
}