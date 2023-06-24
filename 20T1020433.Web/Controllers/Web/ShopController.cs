﻿using _20T1020433.BusinessLayers;
using _20T1020433.DomainModels;
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
        private const string SHOPPING_CART = "ShoppingCart";
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
        public ActionResult Create()
        {
            ViewBag.ErrorMessage = TempData[ERROR_MESSAGE] ?? "";
            return View(GetShoppingCart());
        }
        [HttpPost]
        public ActionResult AddToCart(OrderDetail data)
        {
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
    }
}