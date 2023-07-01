using _20T1020433.BusinessLayers;
using _20T1020433.DomainModels;
using _20T1020433.Web.Areas.ShipperArea.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _20T1020433.Web.Areas.ShipperArea.Controllers
{
    
    [RoutePrefix("Order")]
    public class OrderController : Controller
    {
        private const string ORDER_SEARCH = "SearchOrderCondition";
        private const string SHOPPING_CART = "ShoppingCart";
        private const string ERROR_MESSAGE = "ErrorMessage";
        private const string SUCCESS_MESSAGE = "SuccessMessage";
        private const int PAGE_SIZE = 15;

        /// <summary>
        /// Tìm kiếm, phân trang
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (userAccount == null)
            {
                return RedirectToAction("Login", "Account");
            }
            //TODO: Code chức năng tìm kiếm, phân trang cho đơn hàng
            OrderSearchInput condition = Session[ORDER_SEARCH] as OrderSearchInput;
            ViewBag.SuccessMessage = TempData[SUCCESS_MESSAGE] ?? "";
            if (condition == null)
            {
                condition = new OrderSearchInput()
                {
                    ShipperID = 0,
                    Status = 2,
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = ""
                };
            }
            return View(condition);
        }
        public ActionResult Search(OrderSearchInput condition)
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (userAccount == null)
            {
                return RedirectToAction("Login", "Account");
            }
            int rowCount = 0;
            var data = OrderService.ListOrders(condition.Page,
                condition.PageSize, condition.Status, condition.ShipperID, 
                condition.SearchValue,
                out rowCount);
            var result = new OrderSearchOutput()
            {
                ShipperID = condition.ShipperID,
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue,
                Status = condition.Status,
                RowCount = rowCount,
                Data = data
            };
            Session[ORDER_SEARCH] = condition;
            return View(result);
        }
        /// <summary>
        /// Xem thông tin và chi tiết của đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id = 0)
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (userAccount == null)
            {
                return RedirectToAction("Login", "Account");
            }
            //TODO: Code chức năng lấy và hiển thị thông tin của đơn hàng và chi tiết của đơn hàng
            ViewBag.ErrorMessage = TempData[ERROR_MESSAGE] ?? "";
            ViewBag.SuccessMessage = TempData[SUCCESS_MESSAGE] ?? "";
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

        /// <summary>
        /// Chấp nhận đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Get(int id = 0)
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (userAccount == null)
            {
                return RedirectToAction("Login", "Account");
            }
            //TODO: Code chức năng chấp nhận đơn hàng (nếu được phép)
            if (id <= 0)
                return RedirectToAction("Index");  
            var data = OrderService.GetOrder(id);
            if (data == null)
            {
                return RedirectToAction("Index");
            }          
            var userId = Convert.ToInt32(userAccount.UserId);
            OrderService.ShipOrder(id, int.Parse(userAccount.UserId));
            TempData[SUCCESS_MESSAGE] = "Đơn hàng đã được nhận!";
            return RedirectToAction($"Details/{id}");            
        }        

    }
}