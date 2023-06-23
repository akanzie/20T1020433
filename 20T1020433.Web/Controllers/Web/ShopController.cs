using _20T1020433.BusinessLayers;
using _20T1020433.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _20T1020433.Web.Controllers.Web
{
    public class ShopController : Controller
    {
        private const int PAGE_SIZE = 10;
        private const string PRODUCT_SEARCH = "SearchProductCondition";
        private const string SUCCESS_MESSAGE = "SuccessMessage";
        private const string ERROR_MESSAGE = "ErrorMessage";
        // GET: Home
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ProductSearchInput condition = Session[PRODUCT_SEARCH] as ProductSearchInput;
            ViewBag.SuccessMessage = TempData[SUCCESS_MESSAGE] ?? "";
            if (condition == null)
            {
                condition = new ProductSearchInput()
                {
                    SortByPrice = 0,
                    CategoryID = 0,
                    SupplierID = 0,
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = ""
                };
            }
            return View(condition);
        }
        public ActionResult Search(ProductSearchInput condition)
        {
            int rowCount = 0;
            var data = ProductDataService.ListProducts(condition.Page,
                condition.PageSize,
                condition.SearchValue, condition.CategoryID, condition.SupplierID, condition.SortByPrice,
                out rowCount);
            var result = new ProductSearchOutput()
            {
                SortByPrice = condition.SortByPrice,
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue,
                CategoryID = condition.CategoryID,
                SupplierID = condition.SupplierID,
                RowCount = rowCount,
                Data = data
            };
            Session[PRODUCT_SEARCH] = condition;
            return View(result);
        }
    }
}