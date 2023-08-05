using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
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
    public class EmployeeController : Controller
    {
        private const int PAGE_SIZE = 6;
        private const string EMPLOYEE_SEARCH = "SearchEmployeeCondition";
        private const string SUCCESS_MESSAGE = "SuccessMessage";
        public ActionResult Index()
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (!UserAccountService.IsInRole(Convert.ToInt32(userAccount.UserId), EmployeeRoles.MANAGE_EMPLOYEE))
            {
                return RedirectToAction("NotFound", "Error");
            }
            PaginationSearchInput condition = Session[EMPLOYEE_SEARCH] as PaginationSearchInput;
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
            if (!UserAccountService.IsInRole(Convert.ToInt32(userAccount.UserId), EmployeeRoles.MANAGE_EMPLOYEE))
            {
                return RedirectToAction("NotFound", "Error");
            }
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
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (!UserAccountService.IsInRole(Convert.ToInt32(userAccount.UserId), EmployeeRoles.MANAGE_EMPLOYEE))
            {
                return RedirectToAction("NotFound", "Error");
            }
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
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (!UserAccountService.IsInRole(Convert.ToInt32(userAccount.UserId), EmployeeRoles.MANAGE_EMPLOYEE))
            {
                return RedirectToAction("NotFound", "Error");
            }
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
        public ActionResult Save(Employee data, string birthday, HttpPostedFileBase uploadPhoto)
        {
            //try
            //{
            DateTime? d = Converter.DMYStringToDateTime(birthday);
            if (d == null)
                ModelState.AddModelError("BirthDate", $"Ngày { birthday}  không hợp lệ. Vui lòng nhập theo định dạng dd/MM/yyyy");
            else
                data.BirthDate = d.Value;
            if (string.IsNullOrWhiteSpace(data.FirstName))
                ModelState.AddModelError("FirstName", "Tên không được để trống");
            if (string.IsNullOrWhiteSpace(data.LastName))
                ModelState.AddModelError("LastName", "Họ không được để trống");
            DateTime minDate = DateTime.ParseExact("2/1/1753", "d/M/yyyy", CultureInfo.InvariantCulture);
            DateTime maxDate = DateTime.ParseExact("30/12/9999", "d/M/yyyy", CultureInfo.InvariantCulture);
            if (data.BirthDate < minDate || data.BirthDate > maxDate)
            {
                ModelState.AddModelError("BirthDate", $"Ngày {birthday} không hợp lệ!");
            }
            if (string.IsNullOrWhiteSpace(data.Photo))
            {
                data.Photo = "";
            }
            if (string.IsNullOrWhiteSpace(data.Notes))
                ModelState.AddModelError("Notes", "Ghi chú không được để trống");
            if (string.IsNullOrWhiteSpace(data.Email))
                ModelState.AddModelError("Email", "Email không được để trống");
            else
            {
                foreach (var item in CommonDataService.ListOfEmployees(""))
                {
                    if (item.Email == data.Email && item.EmployeeID != data.EmployeeID)
                    {
                        ModelState.AddModelError("Email", "Email không hợp lệ!");
                    }
                }
            }
            if (uploadPhoto != null)
            {
                //        WebImage photo = WebImage.GetImageFromRequest();
                //        if (photo != null)
                //        {
                //            string newFileName = Guid.NewGuid().ToString() + "_" +
                //Path.GetFileName(photo.FileName);
                //            string imagePath = @"Photo/" + newFileName;

                //            photo.Save(@"~/" + imagePath);
                //            data.Photo = imagePath;
                //        }

                string path = Server.MapPath("~/Photo");
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                string filePath = System.IO.Path.Combine(path, fileName);
                uploadPhoto.SaveAs(filePath);
                data.Photo = fileName;
            }
            //else
            //{
            //    ModelState.AddModelError("Photo", "Vui lòng chọn ảnh");
            //}
            if (!ModelState.IsValid)
            {
                ViewBag.Title = data.EmployeeID == 0 ? "Bổ sung nhà cung cấp" : "Cập nhật nhà cung cấp";
                return View("Edit", data);
            }
            if (data.EmployeeID == 0)
            {
                CommonDataService.AddEmployee(data);
                TempData[SUCCESS_MESSAGE] = $"Bổ sung nhân viên: {data.FirstName} {data.LastName} thành công!";
            }
            else
            {
                CommonDataService.UpdateEmployee(data);
                TempData[SUCCESS_MESSAGE] = $"Cập nhật nhân viên: {data.FirstName} {data.LastName} thành công!";
            }
            return RedirectToAction("Index");
            //}
            //catch (Exception ex)
            //{
            //    //Ghi lại log lỗi
            //    return Content("Có lỗi xảy ra. Vui lòng thử lại sau!");
            //}

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete(int id = 0)
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (!UserAccountService.IsInRole(Convert.ToInt32(userAccount.UserId), EmployeeRoles.MANAGE_EMPLOYEE))
            {
                return RedirectToAction("NotFound", "Error");
            }
            if (id <= 0)
                return RedirectToAction("Index");
            if (Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteEmployee(id);
                TempData[SUCCESS_MESSAGE] = $"Xóa nhân viên thành công!";
                return RedirectToAction("Index");

            }
            var data = CommonDataService.GetEmployee(id);
            if (data == null)
                return RedirectToAction("Index");
            return View(data);
        }
    }
}