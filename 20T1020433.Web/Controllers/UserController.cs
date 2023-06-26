using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;
using System.Web.UI;
using _20T1020433.BusinessLayers;
using _20T1020433.DomainModels;
using _20T1020433.Web.Areas.Admin.Models;
using _20T1020433.Web.Models;

namespace _20T1020433.Web.Controllers
{
    public class UserController : Controller
    {
        private const string ORDER_SEARCH = "SearchOrderCondition";
        private const string MESSAGE = "Message";
        private const int PAGE_SIZE = 10;
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>        
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            var cookie = Converter.CookieToUserAccount(User.Identity.Name);
            if (cookie != null)
                return RedirectToAction("Index", "Home");
            ViewBag.Message = TempData[MESSAGE] ?? "";
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        //tranh chay lap vi co authorize
        [AllowAnonymous]
        //chi dinh
        [HttpPost]
        public ActionResult Login(string userName = "", string password = "")
        {
            ViewBag.Message = TempData[MESSAGE] ?? "";
            ViewBag.UserName = userName;
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin!");
                return View();
            }

            var userAccount = UserAccountService.Authorize(AccountTypes.Customer, userName, password);

            if (userAccount == null || userAccount.RoleNames != "Customer")
            {
                ModelState.AddModelError("", "Đăng nhập thất bại!");
                return View();
            }

            // -> JSON
            string cookieValue = Newtonsoft.Json.JsonConvert.SerializeObject(userAccount);
            FormsAuthentication.SetAuthCookie(cookieValue, false);
            return RedirectToAction("Index", "Home");

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (userAccount == null)
            {
                return RedirectToAction("Login");
            }
            //Xoa thong tin dang nhap cua nguoi dung
            Session.Clear();

            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ChangePassword()
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (userAccount == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangePassword(string userName = "", string oldPassword = "", string newPassword = "", string newPass = "")
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (userAccount == null)
            {
                return RedirectToAction("Login");
            }
            if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword))
            {
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin!");
                return View();
            }
            if (newPass != newPassword)
            {
                ModelState.AddModelError("", "Mật khẩu không khớp");
                return View();
            }
            if (oldPassword == newPassword)
            {
                ModelState.AddModelError("", "Mật khẩu mới đã trùng với mật khẩu cũ!");
                return View();
            }
            var check = UserAccountService.ChangePassword(AccountTypes.Customer, userName, oldPassword, newPassword);
            if (check == false)
            {
                ModelState.AddModelError("", "Mật khẩu cũ không đúng");
                return View();
            }
            //Response.Write("<script>alert('Đổi mật khẩu thành công! Vui lòng đăng nhập lại!')</script>");
            TempData[MESSAGE] = "Đổi mật khẩu thành công! Vui lòng đăng nhập lại!";
            //return View("Login");
            return RedirectToAction("Logout");
        }
        public ActionResult Profile()
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (userAccount == null)
            {
                return RedirectToAction("Login");
            }
            var data = CommonDataService.GetCustomer(int.Parse(userAccount.UserId));
            if (data == null)
                return RedirectToAction("Index", "Home");
            return View(data);
        }
        public ActionResult Purchase()
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (userAccount == null)
            {
                return RedirectToAction("Login");
            }
            Models.OrderSearchInput condition = Session[ORDER_SEARCH] as Models.OrderSearchInput;
            //ViewBag.SuccessMessage = TempData[SUCCESS_MESSAGE] ?? "";
            if (condition == null)
            {
                condition = new Models.OrderSearchInput()
                {
                    CustomerID = int.Parse(userAccount.UserId),
                    Status = 0,
                    Page = 1,
                    PageSize = PAGE_SIZE
                };
            }
            return View(condition);
        }
        public ActionResult Search(Models.OrderSearchInput condition)
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (userAccount == null)
            {
                return RedirectToAction("Login");
            }
            int rowCount = 0;
            var data = OrderService.ListOrders(condition.Page,
                condition.PageSize, condition.Status, condition.CustomerID,
                out rowCount);
            var result = new Models.OrderSearchOutput()
            {
                CustomerID = condition.CustomerID,
                Page = condition.Page,
                PageSize = condition.PageSize,
                Status = condition.Status,
                RowCount = rowCount,
                Data = data
            };
            Session[ORDER_SEARCH] = condition;
            return View(result);
        }
        public ActionResult OrderDetail(int id = 0)
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (userAccount == null)
            {
                return RedirectToAction("Login", "User");
            }
            //TODO: Code chức năng lấy và hiển thị thông tin của đơn hàng và chi tiết của đơn hàng
            //ViewBag.ErrorMessage = TempData[ERROR_MESSAGE] ?? "";
            //ViewBag.SuccessMessage = TempData[SUCCESS_MESSAGE] ?? "";
            if (id <= 0)
                return RedirectToAction("Index");
            var order = OrderService.GetOrder(id);
            if (order == null)
            {
                return RedirectToAction("Index");
            }
            var data = new OrderModel()
            {
                Order = order,
                OrderDetails = OrderService.ListOrderDetails(id)
            };
            if (data == null)
                return RedirectToAction("Index");
            return View(data);
        }
        public ActionResult Cancel(int id = 0)
        {
            //TODO: Code chức năng hủy đơn hàng (nếu được phép)
            if (id <= 0)
                return RedirectToAction("Index");
            var data = OrderService.GetOrder(id);
            if (data == null)
                return RedirectToAction("Index");            

            OrderService.CancelOrder(id);
            //TempData[SUCCESS_MESSAGE] = "Đơn hàng đã chuyển sang trạng thái bị hủy!";
            return RedirectToAction($"Details/{id}");

        }
    }
}