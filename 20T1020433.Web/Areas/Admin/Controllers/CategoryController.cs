﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _20T1020433.BusinessLayers;
using _20T1020433.DomainModels;
using _20T1020433.Web.Areas.Admin.Models;

namespace _20T1020433.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
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
        private const string SUCCESS_MESSAGE = "SuccessMessage";
        public ActionResult Index()
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (!UserAccountService.IsInRole(Convert.ToInt32(userAccount.UserId), EmployeeRoles.MANAGE_CATEGORY))
            {
                return RedirectToAction("NotFound", "Error");
            }
            PaginationSearchInput condition = Session[CATEGORY_SEARCH] as PaginationSearchInput;
            ViewBag.SuccessMessage = TempData[SUCCESS_MESSAGE] ?? "";
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
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (!UserAccountService.IsInRole(Convert.ToInt32(userAccount.UserId), EmployeeRoles.MANAGE_CATEGORY))
            {
                return RedirectToAction("NotFound", "Error");
            }
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
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (!UserAccountService.IsInRole(Convert.ToInt32(userAccount.UserId), EmployeeRoles.MANAGE_CATEGORY))
            {
                return RedirectToAction("NotFound", "Error");
            }
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
        public ActionResult Edit(int id = 0)
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (!UserAccountService.IsInRole(Convert.ToInt32(userAccount.UserId), EmployeeRoles.MANAGE_CATEGORY))
            {
                return RedirectToAction("NotFound", "Error");
            }
            if (id <= 0)
                return RedirectToAction("Index");

            var data = CommonDataService.GetCategory(id);
            if (data == null)
                return RedirectToAction("Index");
            ViewBag.Title = "Cập nhật loại hàng";

            return View(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Category data)
        {
            try
            {                
                if (string.IsNullOrWhiteSpace(data.CategoryName))
                    ModelState.AddModelError("CategoryName", "Tên không được để trống");
                if (string.IsNullOrWhiteSpace(data.Description))
                    ModelState.AddModelError("Description", "Mô tả không được để trống");

                if (!ModelState.IsValid)
                {
                    ViewBag.Title = data.CategoryID == 0 ? "Bổ sung loại hàng" : "Cập nhật loại hàng";
                    return View("Edit", data);
                }
                if (data.CategoryID == 0)
                {
                    CommonDataService.AddCategory(data);
                    TempData[SUCCESS_MESSAGE] = $"Bổ sung loại hàng: {data.CategoryName} thành công!";
                }
                else
                {
                    CommonDataService.UpdateCategory(data);
                    TempData[SUCCESS_MESSAGE] = $"Cập nhật loại hàng: {data.CategoryName} thành công!";
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //Ghi lại log lỗi
                return Content("Có lỗi xảy ra. Vui lòng thử lại sau!");
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete(int id = 0)
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (!UserAccountService.IsInRole(Convert.ToInt32(userAccount.UserId), EmployeeRoles.MANAGE_CATEGORY))
            {
                return RedirectToAction("NotFound", "Error");
            }
            if (id <= 0)
                return RedirectToAction("Index");
            var data = CommonDataService.GetCategory(id);
            if (data == null)
                return RedirectToAction("Index");
            if (Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteCategory(id);
                TempData[SUCCESS_MESSAGE] = $"Xóa loại hàng thành công!";
                return RedirectToAction("Index");
            }
            return View(data);
        }
    }
}