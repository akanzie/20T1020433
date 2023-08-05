using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Microsoft.Ajax.Utilities;
using _20T1020433.DomainModels;
using _20T1020433.BusinessLayers;
using _20T1020433.Web.Areas.Admin.Models;

namespace _20T1020433.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class SupplierController : Controller
    {
        private const int PAGE_SIZE = 5;
        private const string SUPPLIER_SEARCH = "SearchSupplierCondition";
        private const string SUCCESS_MESSAGE = "SuccessMessage";
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
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (!UserAccountService.IsInRole(Convert.ToInt32(userAccount.UserId), EmployeeRoles.MANAGE_SUPPLIER))
            {
                return RedirectToAction("NotFound", "Error");
            }
            PaginationSearchInput condition = Session[SUPPLIER_SEARCH] as PaginationSearchInput;
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
            if (!UserAccountService.IsInRole(Convert.ToInt32(userAccount.UserId), EmployeeRoles.MANAGE_SUPPLIER))
            {
                return RedirectToAction("NotFound", "Error");
            }
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
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (!UserAccountService.IsInRole(Convert.ToInt32(userAccount.UserId), EmployeeRoles.MANAGE_SUPPLIER))
            {
                return RedirectToAction("NotFound", "Error");
            }
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
        public ActionResult Edit(int id = 0)
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (!UserAccountService.IsInRole(Convert.ToInt32(userAccount.UserId), EmployeeRoles.MANAGE_SUPPLIER))
            {
                return RedirectToAction("NotFound", "Error");
            }
            if (id <= 0)
                return RedirectToAction("Index");
            var data = CommonDataService.GetSupplier(id);
            if (data == null)
                return RedirectToAction("Index");
            ViewBag.Title = "Cập nhật nhà cung cấp";
            return View(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Supplier data)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data.SupplierName))
                    ModelState.AddModelError("SupplierName", "Tên không được để trống");
                if (string.IsNullOrWhiteSpace(data.ContactName))
                    ModelState.AddModelError("ContactName", "Tên giao dịch không được để trống");
                if (string.IsNullOrWhiteSpace(data.Country))
                    ModelState.AddModelError("Country", "Vui lòng chọn quốc gia");
                if (string.IsNullOrWhiteSpace(data.City))
                    ModelState.AddModelError("City", "Thành phố không được để trống");
                if (string.IsNullOrWhiteSpace(data.Address))
                    ModelState.AddModelError("Address", "Địa chỉ không được để trống");
                if (string.IsNullOrWhiteSpace(data.Phone))
                    ModelState.AddModelError("Phone", "Điện thoại không được để trống");
                if (string.IsNullOrWhiteSpace(data.PostalCode))
                    ModelState.AddModelError("PostalCode", "Mã bưu chính không được để trống");

                if (!ModelState.IsValid)
                {
                    ViewBag.Title = data.SupplierID == 0 ? "Bổ sung nhà cung cấp" : "Cập nhật nhà cung cấp";
                    return View("Edit", data);
                }
                if (data.SupplierID == 0)
                {
                    CommonDataService.AddSupplier(data);
                    TempData[SUCCESS_MESSAGE] = $"Bổ sung nhà cung cấp: {data.SupplierName} thành công!";
                }
                else
                {
                    CommonDataService.UpdateSupplier(data);
                    TempData[SUCCESS_MESSAGE] = $"Cập nhật nhà cung cấp: {data.SupplierName} thành công!";
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
            if (!UserAccountService.IsInRole(Convert.ToInt32(userAccount.UserId), EmployeeRoles.MANAGE_SUPPLIER))
            {
                return RedirectToAction("NotFound", "Error");
            }
            if (id <= 0)
                return RedirectToAction("Index");

            if (Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteSupplier(id);
                TempData[SUCCESS_MESSAGE] = "Xóa nhà cung cấp thành công!";
                return RedirectToAction("Index");
            }
                var data = CommonDataService.GetSupplier(id);
                if (data == null)
                    return RedirectToAction("Index");
                return View(data);
            
        }
    }
}