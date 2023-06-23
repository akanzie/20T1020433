using _20T1020433.BusinessLayers;
using System;
using System.Web;
using System.Web.Mvc;
using _20T1020433.DomainModels;
using _20T1020433.Web.Models;

namespace _20T1020433.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [RoutePrefix("product")]
    public class ProductController : Controller
    {
        private const int PAGE_SIZE = 10;
        private const string PRODUCT_SEARCH = "SearchProductCondition";
        private const string SUCCESS_MESSAGE = "SuccessMessage";
        private const string ERROR_MESSAGE = "ErrorMessage";
        /// <summary>
        /// Tìm kiếm, hiển thị mặt hàng dưới dạng phân trang
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
        /// <summary>
        /// Tạo mặt hàng mới
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var data = new Product()
            {
                ProductID = 0
            };
            return View(data);
        }
        /// <summary>
        /// Cập nhật thông tin mặt hàng, 
        /// Hiển thị danh sách ảnh và thuộc tính của mặt hàng, điều hướng đến các chức năng
        /// quản lý ảnh và thuộc tính của mặt hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public ActionResult Edit(int id = 0)
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Product data, string unPrice, HttpPostedFileBase uploadPhoto)
        {
            //try
            //{
                decimal? d = Converter.StringToDecimal(unPrice);
                if (d == null)
                    ModelState.AddModelError("Price", $"Giá { unPrice}  không hợp lệ.");
                else
                    data.Price = d.Value;
                if (string.IsNullOrWhiteSpace(data.ProductName))
                    ModelState.AddModelError("ProductName", "Tên mặt hàng không được để trống");
                if (data.SupplierID <= 0)
                    ModelState.AddModelError("SupplierID", "Vui lòng chọn nhà cung cấp");
                if (data.CategoryID <= 0)
                    ModelState.AddModelError("CategoryID", "Vui lòng chọn loại hàng");
                if (string.IsNullOrWhiteSpace(data.Unit))
                    ModelState.AddModelError("Unit", "Đơn vị tính không được để trống");
                if (string.IsNullOrWhiteSpace(data.Photo))
                {
                    data.Photo = "";
                }
                if (uploadPhoto != null)
                {
                    string path = Server.MapPath("~/Photo/Product");
                    string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                    string filePath = System.IO.Path.Combine(path, fileName);
                    uploadPhoto.SaveAs(filePath);
                    data.Photo = fileName;
                }
                if (data.Photo == "")
                {
                    ModelState.AddModelError("Photo", "Vui lòng chọn ảnh");
                }
                ProductModel model = new ProductModel()
                {
                    ProductID = data.ProductID,
                    ProductName = data.ProductName,
                    CategoryID = data.CategoryID,
                    SupplierID = data.SupplierID,
                    Unit = data.Unit,
                    Price = data.Price,
                    Photo = data.Photo,
                    Attributes = ProductDataService.ListAttributes(data.ProductID),
                    Photos = ProductDataService.ListPhotos(data.ProductID)
                };
                if (!ModelState.IsValid)
                {
                    if (data.ProductID == 0)
                        return View("Create", data);
                    else
                    {
                        return View("Edit", model);
                    }
                }
                if (data.ProductID == 0)
                {
                    ProductDataService.AddProduct(data);
                    TempData[SUCCESS_MESSAGE] = $"Bổ sung mặt hàng: {data.ProductName} thành công!";
                }
                else
                {
                    ProductDataService.UpdateProduct(data);
                    TempData[SUCCESS_MESSAGE] = $"Cập nhật mặt hàng: {data.ProductName} thành công!";
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
        /// Xóa mặt hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public ActionResult Delete(int id = 0)
        {
            ViewBag.ErrorMessage = TempData[ERROR_MESSAGE] ?? "";
            if (id <= 0)
                return RedirectToAction("Index");
            var data = ProductDataService.GetProduct(id);
            if (data == null)
                return RedirectToAction("Index");
            if (Request.HttpMethod == "POST")
            {
                if (ProductDataService.InUsedProduct(id))
                {
                    TempData[ERROR_MESSAGE] = "Không thể xóa mặt hàng này!";
                    return View(data);
                }
                ProductDataService.DeleteProduct(id);
                TempData[SUCCESS_MESSAGE] = $"Xóa mặt hàng: {data.ProductName} thành công!";
                return RedirectToAction("Index");
            }

            return View(data);
        }

        /// <summary>
        /// Các chức năng quản lý ảnh của mặt hàng
        /// </summary>
        /// <param name="method"></param>
        /// <param name="productID"></param>
        /// <param name="photoID"></param>
        /// <returns></returns>
        [Route("photo/{method?}/{productID?}/{photoID?}")]
        public ActionResult Photo(string method = "add", int productID = 0, long photoID = 0)
        {
            if (productID <= 0)
            {
                return RedirectToAction("Index");
            }
            var product = ProductDataService.GetProduct(productID);
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            switch (method)
            {
                case "add":
                    var data = new ProductPhoto()
                    {
                        PhotoID = 0,
                        ProductID = productID
                    };
                    ViewBag.Title = "Bổ sung ảnh";
                    return View(data);
                case "edit":
                    if (photoID <= 0)
                        return RedirectToAction($"Edit/{productID}");
                    data = ProductDataService.GetPhoto(photoID);
                    if (data == null)
                        return RedirectToAction($"Edit/{productID}");
                    ViewBag.Title = "Thay đổi ảnh";
                    return View(data);
                case "delete":
                    if (photoID <= 0)
                        return RedirectToAction($"Edit/{productID}");
                    data = ProductDataService.GetPhoto(photoID);
                    if (data == null)
                        return RedirectToAction($"Edit/{productID}");
                    ProductDataService.DeletePhoto(photoID);
                    TempData[SUCCESS_MESSAGE] = $"Xóa ảnh thành công!";
                    return RedirectToAction($"Edit/{productID}"); //return RedirectToAction("Edit", new { productID = productID });
                default:
                    return RedirectToAction($"Edit/{productID}");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SavePhoto(ProductPhoto data, HttpPostedFileBase uploadPhoto)
        {
            //try
            //{
                if (string.IsNullOrWhiteSpace(data.Photo))
                    data.Photo = "";
                if (uploadPhoto != null)
                {
                    string path = Server.MapPath("~/Photo/Product");
                    string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                    string filePath = System.IO.Path.Combine(path, fileName);
                    uploadPhoto.SaveAs(filePath);
                    data.Photo = fileName;
                }
                if (string.IsNullOrWhiteSpace(data.Photo))
                {
                    ModelState.AddModelError("Photo", "Vui lòng chọn ảnh");
                }
                if (string.IsNullOrWhiteSpace(data.Description))
                    ModelState.AddModelError("Description", "Giá trị thuộc tính không được để trống");                
                if (data.DisplayOrder <= 0)
                {
                    ModelState.AddModelError("DisplayOrder", "Thứ tự hiển thị không hợp lệ");
                }
                if (!ModelState.IsValid)
                {
                    ViewBag.Title = data.PhotoID == 0 ? "Bổ sung ảnh" : "Cập nhật ảnh";
                    return View("Photo", data);
                }
                if (data.PhotoID == 0)
                {
                    ProductDataService.AddPhoto(data);
                    TempData[SUCCESS_MESSAGE] = $"Thêm ảnh thành công!";
                }
                else
                {
                    ProductDataService.UpdatePhoto(data);
                    TempData[SUCCESS_MESSAGE] = $"Cập nhật ảnh thành công!";
                }
                return RedirectToAction($"Edit/{data.ProductID}");
            //}
            //catch (Exception ex)
            //{
            //    //Ghi lại log lỗi
            //    return Content("Có lỗi xảy ra. Vui lòng thử lại sau!");
            //}
        }
        /// <summary>
        /// Các chức năng quản lý thuộc tính của mặt hàng
        /// </summary>
        /// <param name="method"></param>
        /// <param name="productID"></param>
        /// <param name="attributeID"></param>
        /// <returns></returns>
        [Route("attribute/{method?}/{productID?}/{attributeID?}")]
        public ActionResult Attribute(string method = "add", int productID = 0, long attributeID = 0)
        {
            if (productID <= 0)
            {
                return RedirectToAction("Index");
            }
            var product = ProductDataService.GetProduct(productID);
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            switch (method)
            {
                case "add":
                    var data = new ProductAttribute()
                    {
                        AttributeID = 0,
                        ProductID = productID
                    };
                    ViewBag.Title = "Bổ sung thuộc tính";
                    return View(data);
                case "edit":
                    if (attributeID <= 0)
                        return RedirectToAction($"Edit/{productID}");
                    data = ProductDataService.GetAttribute(attributeID);
                    if (data == null)
                        return RedirectToAction($"Edit/{productID}");
                    ViewBag.Title = "Thay đổi thuộc tính";
                    return View(data);
                case "delete":
                    if (attributeID <= 0)
                        return RedirectToAction($"Edit/{productID}");
                    data = ProductDataService.GetAttribute(attributeID);
                    if (data == null)
                        return RedirectToAction($"Edit/{productID}");
                    ProductDataService.DeleteAttribute(attributeID);
                    TempData[SUCCESS_MESSAGE] = $"Xóa thuộc tính thành công!";
                    return RedirectToAction($"Edit/{productID}"); //return RedirectToAction("Edit", new { productID = productID });
                default:
                    return RedirectToAction($"Edit/{productID}");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAttribute(ProductAttribute data)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data.AttributeName))
                    ModelState.AddModelError("AttributeName", "Tên thuộc tính không được để trống");
                if (string.IsNullOrWhiteSpace(data.AttributeValue))
                    ModelState.AddModelError("AttributeValue", "Giá trị thuộc tính không được để trống");
                if (data.DisplayOrder <= 0)
                    ModelState.AddModelError("DisplayOrder", "Thứ tự thuộc tính không hợp lệ");
                if (!ModelState.IsValid)
                {
                    ViewBag.Title = data.AttributeID == 0 ? "Bổ sung thuộc tính" : "Cập nhật thuộc tính";
                    return View("Attribute", data);
                }
                if (data.AttributeID == 0)
                {
                    ProductDataService.AddAttribute(data);
                    TempData[SUCCESS_MESSAGE] = $"Thêm thuộc tính thành công!";
                }
                else
                {
                    ProductDataService.UpdateAttribute(data);
                    TempData[SUCCESS_MESSAGE] = $"Cập nhật thuộc tính thành công!";
                }
                return RedirectToAction($"Edit/{data.ProductID}");
            }
            catch (Exception ex)
            {
                //Ghi lại log lỗi
                return Content("Có lỗi xảy ra. Vui lòng thử lại sau!");
            }
        }
    }
}