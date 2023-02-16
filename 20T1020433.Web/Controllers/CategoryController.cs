using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _20T1020433.BusinessLayers;
using _20T1020433.DomainModels;
using _20T1020433.Web.Models;

namespace _20T1020433.Web.Controllers
{
    public class CategoryController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // GET: Category
        //public ActionResult Index(int page = 1, int pageSize = 5, string searchValue = "")
        //{
        //    var rowCount = 0;
        //    var data = CommonDataService.ListOfCategories(page, pageSize, searchValue, out rowCount);
        //    var data2 = CommonDataService.ListOfCategories(searchValue);
        //    string[] parentCategoryNames = new string[data2.Count+1];
        //    int parentCategoryId;
        //    for (var i = 0; i < data2.Count; i++)
        //    {
        //        parentCategoryId = data2[i].ParentCategoryId;
        //        if (parentCategoryId != 0)
        //        {
        //            Category category = new Category();
        //            category = CommonDataService.GetCategory(parentCategoryId);
        //            parentCategoryNames[i+1] = category.CategoryName;
        //        }
        //        else parentCategoryNames[i+1] = "Không có";
        //    }

        //    ViewBag.parentCategoryNames = parentCategoryNames;
        //    var pageCount = rowCount / pageSize;
        //    if (rowCount % pageSize > 0)
        //        pageCount += 1;

        //    ViewBag.Page = page;
        //    ViewBag.PageCount = pageCount;
        //    ViewBag.RowCount = rowCount;
        //    ViewBag.PageSize = pageSize;
        //    ViewBag.SearchValue = searchValue;
        //    return View(data);
        //}
        private const int PAGE_SIZE = 5;
        private const string CATEGORY_SEARCH = "SearchCustomerCondition";
        public ActionResult Index()
        {
            PaginationSearchInput condition = Session[CATEGORY_SEARCH] as PaginationSearchInput;
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
            var data = CommonDataService.ListOfCategories(condition.Page,
                condition.PageSize,
                condition.SearchValue,
                out rowCount);
            var result = new CategorySearchOutput()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue,
                RowCount = rowCount,
                Data = data
            };
            Session[CATEGORY_SEARCH] = condition;
            return View(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var data = new Category()
            {
                CategoryID = 0
            };
            ViewBag.Title = "Bổ sung loại hàng";
            return View("Edit", data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            int categoryId = Convert.ToInt32(id);

            var data = CommonDataService.GetCategory(categoryId);
            ViewBag.Title = "Cập nhật loại hàng";
            return View(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(Category data)
        {
            if (data.CategoryID == 0)
            {
                CommonDataService.AddCategory(data);
            }
            else
            {
                CommonDataService.UpdateCategory(data);
            }
            return RedirectToAction("Index");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete(string id)
        {
            int categoryId = Convert.ToInt32(id);
            if (Request.HttpMethod == "GET")
            {
                var data = CommonDataService.GetCategory(categoryId);
                return View(data);
            }
            else
            {
                CommonDataService.DeleteCategory(categoryId);
                return RedirectToAction("Index");
            }
        }
    }
}