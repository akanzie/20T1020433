using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _20T1020433.BusinessLayers;
using _20T1020433.DomainModels;

namespace _20T1020433.Web.Controllers
{
    public class ShipperController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // GET: Shipper
        public ActionResult Index(int page = 1, int pageSize = 5, string searchValue = "")
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfShippers(page, pageSize, searchValue, out rowCount);

            int pageCount = rowCount / pageSize;
            if (rowCount % pageSize > 0)
                pageCount += 1;

            ViewBag.Page = page;
            ViewBag.PageCount = pageCount;
            ViewBag.RowCount = rowCount;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchValue = searchValue;
            return View(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var data = new Shipper()
            {
                ShipperID = 0
            };
            ViewBag.Title = "Bổ sung người giao hàng";
            return View("Edit", data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            int shipperId = Convert.ToInt32(id);

            var data = CommonDataService.GetShipper(shipperId);
            ViewBag.Title = "Cập nhật người giao hàng";
            return View(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(Shipper data)
        {
            if (data.ShipperID == 0)
            {
                CommonDataService.AddShipper(data);
            }
            else
            {
                CommonDataService.UpdateShipper(data);
            }
            return RedirectToAction("Index");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete(string id)
        {
            int shipperId = Convert.ToInt32(id);
            if (Request.HttpMethod == "GET")
            {
                var data = CommonDataService.GetShipper(shipperId);
                return View(data);
            }
            else
            {
                CommonDataService.DeleteShipper(shipperId);
                return RedirectToAction("Index");
            }
        }
    }
}