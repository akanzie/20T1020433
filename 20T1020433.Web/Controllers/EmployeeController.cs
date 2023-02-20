using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _20T1020433.BusinessLayers;
using _20T1020433.DomainModels;
using _20T1020433.Web.Models;

namespace _20T1020433.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private const int PAGE_SIZE = 5;
        private const string EMPLOYEE_SEARCH = "SearchEmployeeCondition";
        public ActionResult Index()
        {
            PaginationSearchInput condition = Session[EMPLOYEE_SEARCH] as PaginationSearchInput;
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
            var data = CommonDataService.ListOfEmployees(condition.Page,
                condition.PageSize,
                condition.SearchValue,
                out rowCount);
            var result = new EmployeeSearchOutput()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue,
                RowCount = rowCount,
                Data = data
            };
            Session[EMPLOYEE_SEARCH] = condition;
            return View(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var data = new Employee()
            {
                EmployeeID = 0
            };
            ViewBag.Title = "Bổ sung nhân viên";
            return View("Edit", data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(int id = 0)
        {
            if (id <= 0)
                return RedirectToAction("Index");

            var data = CommonDataService.GetEmployee(id);
            if (data == null)
                return RedirectToAction("Index");
            ViewBag.Title = "Cập nhật nhân viên";
            return View(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Employee data)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data.FirstName))
                    ModelState.AddModelError("FirstName", "Tên không được để trống");
                if (string.IsNullOrWhiteSpace(data.LastName))
                    ModelState.AddModelError("LastName", "Họ không được để trống");
                if (string.IsNullOrWhiteSpace(data.BirthDate.ToString(CultureInfo.InvariantCulture)))
                    ModelState.AddModelError("BirthDate", "Vui lòng chọn ngày sinh");
                if (string.IsNullOrWhiteSpace(data.Photo))
                    ModelState.AddModelError("Photo", "Vui lòng chọn ảnh");
                if (string.IsNullOrWhiteSpace(data.Notes))
                    ModelState.AddModelError("Notes", "Ghi chú không được để trống");
                if (string.IsNullOrWhiteSpace(data.Email))
                    ModelState.AddModelError("Email", "Email không được để trống");
                

                if (!ModelState.IsValid)
                {
                    ViewBag.Title = data.EmployeeID == 0 ? "Bổ sung nhà cung cấp" : "Cập nhật nhà cung cấp";
                    return View("Edit", data);
                }
                if (data.EmployeeID == 0)
                {
                    CommonDataService.AddEmployee(data);
                }
                else
                {
                    CommonDataService.UpdateEmployee(data);
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
        if (id <= 0)
            return RedirectToAction("Index");
        if (Request.HttpMethod == "GET")
            {
                var data = CommonDataService.GetEmployee(id);
                if (data == null)
                    return RedirectToAction("Index");
                return View(data);
            }
            else
            {
                CommonDataService.DeleteEmployee(id);
                return RedirectToAction("Index");
            }
        }
    }
}