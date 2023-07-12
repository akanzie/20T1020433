using _20T1020433.BusinessLayers;
using _20T1020433.DomainModels;
using _20T1020433.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _20T1020433.Web.Controllers.Web
{
    [RoutePrefix("Shop")]
    public class ShopController : Controller
    {
        private const int PAGE_SIZE = 10;
        private const string SHOP_SEARCH = "SearchShopProductCondition";
        private const string SHOPPING_CART = "ShoppingCartCustomer";
        private const string SUCCESS_MESSAGE = "SuccessMessage";
        private const string ERROR_MESSAGE = "ErrorMessage";

        // GET: Home
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ProductSearchInput condition = Session[SHOP_SEARCH] as ProductSearchInput;
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
            Session[SHOP_SEARCH] = condition;
            return View(result);
        }        
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

        public ActionResult Detail(int id = 0)
        {
            if (id <= 0)
                return RedirectToAction("Index");
            var product = ProductDataService.GetProduct(id);
            if (product == null)
                return RedirectToAction("Index");
            var data = new ProductModel()
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                CategoryID = product.CategoryID,
                SupplierID = product.SupplierID,
                Unit = product.Unit,
                Price = product.Price,
                Photo = product.Photo,
                Attributes = ProductDataService.ListAttributes(id),
                Photos = ProductDataService.ListPhotos(id)
            };
            if (data == null)
                return RedirectToAction("Index");
            return View(data);
        }
       
        public ActionResult ShoppingCart()
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (userAccount == null)
            {
                return RedirectToAction("Login", "User");
            }

            ViewBag.ErrorMessage = TempData[ERROR_MESSAGE] ?? "";
            return View(GetShoppingCart());
        }
        [HttpPost]        
        public ActionResult AddToCart(OrderDetail data)
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (userAccount == null)
            {
                return RedirectToAction("Login", "User");
            }
            if (data == null)
            {
                TempData[ERROR_MESSAGE] = "Dữ liệu không hợp lệ";
                return RedirectToAction("Index"); ;
            }
            if (data.Quantity <= 0)
                data.Quantity = 1;
            if (data.SalePrice <= 0 || data.Quantity <= 0)
            {
                TempData[ERROR_MESSAGE] = "Giá bán và số lượng không hợp lệ";
                return RedirectToAction("Index");
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
            return RedirectToAction("ShoppingCart");
        }
        
        public ActionResult RemoveFromCart(int id = 0)
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (userAccount == null)
            {
                return RedirectToAction("Login", "User");
            }
            List<OrderDetail> shoppingCart = GetShoppingCart();
            int index = shoppingCart.FindIndex(m => m.ProductID == id);
            if (index >= 0)
                shoppingCart.RemoveAt(index);
            Session[SHOPPING_CART] = shoppingCart;
            return RedirectToAction("ShoppingCart");
        }
        [HttpPost]        
        public ActionResult Init(int customerID = 0)
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (userAccount == null)
            {
                return RedirectToAction("Login", "User");
            }
            List<OrderDetail> shoppingCart = GetShoppingCart();
            if (shoppingCart == null || shoppingCart.Count == 0)
            {
                TempData[ERROR_MESSAGE] = "Không thể tạo đơn hàng với giỏ hàng trống";
                return RedirectToAction("ShoppingCart");
            }

            int orderID = OrderService.InitOrder(customerID, 1, DateTime.Now, shoppingCart);

            Session.Remove(SHOPPING_CART); //Xóa giỏ hàng 

            return RedirectToAction("Purchase", "User");
        }       
              
    }

}