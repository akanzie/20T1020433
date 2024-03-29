﻿using System;
using System.Collections.Generic;
using System.Linq;
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
            if (!UserAccountService.IsInRole(Convert.ToInt32(userAccount.UserId), EmployeeRoles.MANAGE_ORDER))
            {
                return RedirectToAction("NotFound", "Error");
            }
            //TODO: Code chức năng tìm kiếm, phân trang cho đơn hàng
            OrderSearchInput condition = Session[ORDER_SEARCH] as OrderSearchInput;
            ViewBag.SuccessMessage = TempData[SUCCESS_MESSAGE] ?? "";
            if (condition == null)
            {
                condition = new OrderSearchInput()
                {                    
                    Status = 0,
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
            if (!UserAccountService.IsInRole(Convert.ToInt32(userAccount.UserId), EmployeeRoles.MANAGE_ORDER))
            {
                return RedirectToAction("NotFound", "Error");
            }
            int rowCount = 0;
            var data = OrderService.ListOrders(condition.Page,
                condition.PageSize, condition.Status,
                condition.SearchValue,
                out rowCount);
            var result = new OrderSearchOutput()
            {                
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
            if (!UserAccountService.IsInRole(Convert.ToInt32(userAccount.UserId), EmployeeRoles.MANAGE_ORDER))
            {
                return RedirectToAction("NotFound", "Error");
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
        /// Giao diện Thay đổi thông tin chi tiết đơn hàng
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        [Route("EditDetail/{orderID?}/{productID?}")]
        public ActionResult EditDetail(int orderID = 0, int productID = 0)
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (!UserAccountService.IsInRole(Convert.ToInt32(userAccount.UserId), EmployeeRoles.MANAGE_ORDER))
            {
                return RedirectToAction("NotFound", "Error");
            }
            //TODO: Code chức năng để lấy chi tiết đơn hàng cần edit            
            if (orderID <= 0 || productID <= 0)
                return RedirectToAction("Index");
            var data = OrderService.GetOrder(orderID);
            if (data == null)
                return RedirectToAction("Index");
            if (Convert.ToInt32(userAccount.UserId) == data.EmployeeID)
            {
                var orderDetail = OrderService.GetOrderDetail(orderID, productID);
                if (orderDetail == null)
                    return RedirectToAction("Index");
                return View(orderDetail);
            }
            TempData[ERROR_MESSAGE] = "Bạn không được phép thay đổi hóa đơn này!";
            return RedirectToAction($"Details/{orderID}");
        }
        /// <summary>
        /// Thay đổi thông tin chi tiết đơn hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string UpdateDetail(OrderDetail data)
        {            
            //TODO: Code chức năng để cập nhật chi tiết đơn hàng
            if (data.Quantity < 1)
            {
                return $"Số lượng {data.Quantity} không hợp lệ";
            }
            if (data.SalePrice < 1)
            {
                return $"Giá {data.SalePrice} không hợp lệ";
            }
            OrderService.SaveOrderDetail(data.OrderID, data.ProductID, data.Quantity, data.SalePrice);
            TempData[SUCCESS_MESSAGE] = "Chỉnh sửa hóa đơn thành công";
            return "";
        }
        /// <summary>
        /// Xóa 1 chi tiết trong đơn hàng
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        [Route("DeleteDetail/{orderID?}/{productID?}")]
        public ActionResult DeleteDetail(int orderID = 0, int productID = 0)
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (!UserAccountService.IsInRole(Convert.ToInt32(userAccount.UserId), EmployeeRoles.MANAGE_ORDER))
            {
                return RedirectToAction("NotFound", "Error");
            }
            //TODO: Code chức năng xóa 1 chi tiết trong đơn hàng
            if (orderID <= 0 || productID <= 0)
                return RedirectToAction("Index");
            var data = OrderService.GetOrder(orderID);
            if (data == null)
                return RedirectToAction("Index");
            if (Convert.ToInt32(userAccount.UserId) == data.EmployeeID)
            {
                OrderService.DeleteOrderDetail(orderID, productID);
                TempData[SUCCESS_MESSAGE] = "Xóa thành công!";
                return RedirectToAction($"Details/{orderID}");
            }
            TempData[ERROR_MESSAGE] = "Bạn không được phép thay đổi hóa đơn này!";
            return RedirectToAction($"Details/{orderID}");
        }
        /// <summary>
        /// Xóa đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id = 0)
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (!UserAccountService.IsInRole(Convert.ToInt32(userAccount.UserId), EmployeeRoles.MANAGE_ORDER))
            {
                return RedirectToAction("NotFound", "Error");
            }
            //TODO: Code chức năng để xóa đơn hàng (nếu được phép xóa)
            if (id <= 0)
                return RedirectToAction("Index");
            var data = OrderService.GetOrder(id);
            if (data == null)
                return RedirectToAction("Index");
            if (Convert.ToInt32(userAccount.UserId) == data.EmployeeID)
            {
                
                if (data.Status > 1)
                {
                    TempData[ERROR_MESSAGE] = "Không thể xóa đơn hàng này!";
                    return RedirectToAction($"Details/{id}");
                }
                else
                {
                    OrderService.DeleteOrder(id);
                    TempData[SUCCESS_MESSAGE] = "Xóa đơn hàng thành công!";
                }
            }
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Chấp nhận đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Accept(int id = 0)
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (!UserAccountService.IsInRole(Convert.ToInt32(userAccount.UserId), EmployeeRoles.MANAGE_ORDER))
            {
                return RedirectToAction("NotFound", "Error");
            }
            //TODO: Code chức năng chấp nhận đơn hàng (nếu được phép)
            if (id <= 0)
                return RedirectToAction("Index");
            var userId = Convert.ToInt32(userAccount.UserId);
            var data = OrderService.GetOrder(id);
            if (data == null)
            {
                return RedirectToAction("Index");
            }
            if (userId == data.EmployeeID)
            {
                OrderService.AcceptOrder(id);
                TempData[SUCCESS_MESSAGE] = "Đơn hàng đã được chấp nhận!";
                return RedirectToAction($"Details/{id}");
            }
            TempData[ERROR_MESSAGE] = "Bạn không được phép thay đổi hóa đơn này!";
            return RedirectToAction($"Details/{id}");
        }
        public ActionResult SelectShipper(int id = 0)
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (!UserAccountService.IsInRole(Convert.ToInt32(userAccount.UserId), EmployeeRoles.MANAGE_ORDER))
            {
                return RedirectToAction("NotFound", "Error");
            }
            //TODO: Code chức năng chuyển đơn hàng sang trạng thái đang giao hàng (nếu được phép)
            if (id <= 0)
                return RedirectToAction("Index");
            ViewBag.OrderID = id;
            return View();
        }
        /// <summary>
        /// Xác nhận chuyển đơn hàng cho người giao hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public string Shipping(int id = 0, int shipperID = 0)
        {
                      
            //TODO: Code chức năng chuyển đơn hàng sang trạng thái đang giao hàng (nếu được phép)
            if (id <= 0)
                return "Có lỗi xảy ra!";
            if (shipperID <= 0)
            {
                return "Vui lòng chọn người giao hàng";
            }
            var data = OrderService.GetOrder(id);
            if (data == null)
                return "Có lỗi xảy ra!";
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name); 
            if (Convert.ToInt32(userAccount.UserId) == data.EmployeeID)
            {
                OrderService.ShipOrder(id, shipperID);
                TempData[SUCCESS_MESSAGE] = "Đơn hàng đã được chuyển sang trạng thái đang giao hàng!";
            }
            else
            {
                TempData[ERROR_MESSAGE] = "Bạn không được phép thay đổi hóa đơn này!";
            }
            return "";
        }

        /// <summary>
        /// Ghi nhận hoàn tất thành công đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Finish(int id = 0)
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (!UserAccountService.IsInRole(Convert.ToInt32(userAccount.UserId), EmployeeRoles.MANAGE_ORDER))
            {
                return RedirectToAction("NotFound", "Error");
            }
            //TODO: Code chức năng ghi nhận hoàn tất đơn hàng (nếu được phép)
            if (id <= 0)
                return RedirectToAction("Index");
            var data = OrderService.GetOrder(id);
            if (data == null)
                return RedirectToAction("Index");
            if (Convert.ToInt32(userAccount.UserId) == data.EmployeeID)
            {
                OrderService.FinishOrder(id);
                TempData[SUCCESS_MESSAGE] = "Đơn hàng đã chuyển sang trạng thái hoàn tất!";
                return RedirectToAction($"Details/{id}");
            }
            TempData[ERROR_MESSAGE] = "Bạn không được phép thay đổi hóa đơn này!";
            return RedirectToAction($"Details/{id}");
        }
        /// <summary>
        /// Hủy bỏ đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Cancel(int id = 0)
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (!UserAccountService.IsInRole(Convert.ToInt32(userAccount.UserId), EmployeeRoles.MANAGE_ORDER))
            {
                return RedirectToAction("NotFound", "Error");
            }
            //TODO: Code chức năng hủy đơn hàng (nếu được phép)
            if (id <= 0)
                return RedirectToAction("Index");
            var data = OrderService.GetOrder(id);
            if (data == null)
                return RedirectToAction("Index");
            if (Convert.ToInt32(userAccount.UserId) == data.EmployeeID)
            {
                OrderService.CancelOrder(id);
                TempData[SUCCESS_MESSAGE] = "Đơn hàng đã chuyển sang trạng thái bị hủy!";
                return RedirectToAction($"Details/{id}");
            }
            TempData[ERROR_MESSAGE] = "Bạn không được phép thay đổi hóa đơn này!";
            return RedirectToAction($"Details/{id}");
        }
        /// <summary>
        /// Từ chối đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Reject(int id = 0)
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (!UserAccountService.IsInRole(Convert.ToInt32(userAccount.UserId), EmployeeRoles.MANAGE_ORDER))
            {
                return RedirectToAction("NotFound", "Error");
            }
            //TODO: Code chức năng từ chối đơn hàng (nếu được phép)
            if (id <= 0)
                return RedirectToAction("Index");
            var data = OrderService.GetOrder(id);
            if (data == null)
                return RedirectToAction("Index");
            if (Convert.ToInt32(userAccount.UserId) == data.EmployeeID)
            {
                OrderService.RejectOrder(id);
                TempData[SUCCESS_MESSAGE] = "Đơn hàng đã chuyển sang trạng thái bị từ chối!";
                return RedirectToAction($"Details/{id}");
            }
            TempData[ERROR_MESSAGE] = "Bạn không được phép thay đổi hóa đơn này!";
            return RedirectToAction($"Details/{id}");
        }

        /// <summary>
        /// Sử dụng 1 biến session để lưu tạm giỏ hàng (danh sách các chi tiết của đơn hàng) trong quá trình xử lý.
        /// Hàm này lấy giỏ hàng hiện đang có trong session (nếu chưa có thì tạo mới giỏ hàng rỗng)
        /// </summary>
        /// <returns></returns>
        private List<OrderDetail> GetShoppingCart()
        {            
            List<OrderDetail> shoppingCart = Session[SHOPPING_CART] as List<OrderDetail>;
            if (shoppingCart == null)
            {
                shoppingCart = new List<OrderDetail>();
                Session[SHOPPING_CART] = shoppingCart;
            }
            return shoppingCart;
        }
        /// <summary>
        /// Giao diện lập đơn hàng mới
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (!UserAccountService.IsInRole(Convert.ToInt32(userAccount.UserId), EmployeeRoles.CREATE_ORDER))
            {
                return RedirectToAction("NotFound", "Error");
            }
            ViewBag.ErrorMessage = TempData[ERROR_MESSAGE] ?? "";
            return View(GetShoppingCart());
        }
        /// <summary>
        /// Tìm kiếm mặt hàng để bổ sung vào giỏ hàng
        /// </summary>
        /// <param name="page"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public ActionResult SearchProducts(int page = 1, string searchValue = "")
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (!UserAccountService.IsInRole(Convert.ToInt32(userAccount.UserId), EmployeeRoles.MANAGE_ORDER))
            {
                return RedirectToAction("NotFound", "Error");
            }
            int rowCount = 0;
            var data = ProductDataService.ListProducts(page, PAGE_SIZE, searchValue, 0, 0, 0, out rowCount);
            ViewBag.Page = page;
            return View(data);
        }
        /// <summary>
        /// Bổ sung thêm hàng vào giỏ hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddToCart(OrderDetail data)
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (!UserAccountService.IsInRole(Convert.ToInt32(userAccount.UserId), EmployeeRoles.CREATE_ORDER))
            {
                return RedirectToAction("NotFound", "Error");
            }
            if (data == null)
            {
                TempData[ERROR_MESSAGE] = "Dữ liệu không hợp lệ";
                return RedirectToAction("Create");
            }
            if (data.SalePrice <= 0 || data.Quantity <= 0)
            {
                TempData[ERROR_MESSAGE] = "Giá bán và số lượng không hợp lệ";
                return RedirectToAction("Create");
            }

            List<OrderDetail> shoppingCart = GetShoppingCart();
            var existsProduct = shoppingCart.FirstOrDefault(m => m.ProductID == data.ProductID);

            if (existsProduct == null) //Nếu mặt hàng cần được bổ sung chưa có trong giỏ hàng thì bổ sung vào giỏ
            {

                shoppingCart.Add(data);
            }
            else //Trường hợp mặt hàng cần bổ sung đã có thì tăng số lượng và thay đổi đơn giá
            {
                existsProduct.Quantity += data.Quantity;
                existsProduct.SalePrice = data.SalePrice;
            }
            Session[SHOPPING_CART] = shoppingCart;
            return RedirectToAction("Create");
        }
        /// <summary>
        /// Xóa 1 mặt hàng khỏi giỏ hàng
        /// </summary>
        /// <param name="id">Mã mặt hàng</param>
        /// <returns></returns>
        public ActionResult RemoveFromCart(int id = 0)
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (!UserAccountService.IsInRole(Convert.ToInt32(userAccount.UserId), EmployeeRoles.CREATE_ORDER))
            {
                return RedirectToAction("NotFound", "Error");
            }
            List<OrderDetail> shoppingCart = GetShoppingCart();
            int index = shoppingCart.FindIndex(m => m.ProductID == id);
            if (index >= 0)
                shoppingCart.RemoveAt(index);
            Session[SHOPPING_CART] = shoppingCart;
            return RedirectToAction("Create");
        }
        /// <summary>
        /// Xóa toàn bộ giỏ hàng
        /// </summary>
        /// <returns></returns>
        public ActionResult ClearCart()
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (!UserAccountService.IsInRole(Convert.ToInt32(userAccount.UserId), EmployeeRoles.CREATE_ORDER))
            {
                return RedirectToAction("NotFound", "Error");
            }
            List<OrderDetail> shoppingCart = GetShoppingCart();
            shoppingCart.Clear();
            Session[SHOPPING_CART] = shoppingCart;
            return RedirectToAction("Create");
        }
        /// <summary>
        /// Khởi tạo đơn hàng (với phần thông tin chi tiết của đơn hàng là giỏ hàng đang có trong session)
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Init(int customerID = 0, int employeeID = 0)
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (!UserAccountService.IsInRole(Convert.ToInt32(userAccount.UserId), EmployeeRoles.CREATE_ORDER))
            {
                return RedirectToAction("NotFound", "Error");
            }
            List<OrderDetail> shoppingCart = GetShoppingCart();
            if (shoppingCart == null || shoppingCart.Count == 0)
            {
                TempData[ERROR_MESSAGE] = "Không thể tạo đơn hàng với giỏ hàng trống";
                return RedirectToAction("Create");
            }

            if (customerID == 0 || employeeID == 0)
            {
                TempData[ERROR_MESSAGE] = "Vui lòng chọn khách hàng và nhân viên phụ trách";
                return RedirectToAction("Create");
            }

            int orderID = OrderService.InitOrder(customerID, employeeID, DateTime.Now, shoppingCart);

            Session.Remove(SHOPPING_CART); //Xóa giỏ hàng 

            return RedirectToAction($"Details/{orderID}");
        }
    }
}