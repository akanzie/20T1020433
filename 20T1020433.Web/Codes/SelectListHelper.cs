using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _20T1020433.BusinessLayers;
using _20T1020433.DomainModels;
namespace _20T1020433.Web
{
    /// <summary>
    /// Cung cấp hàm tiện ích liên quan đến SelectList
    /// </summary>
    public static class SelectListHelper
    {
        /// <summary>
        /// Danh sách các quốc gia
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> Countries()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Chọn quốc gia --"
            });
            foreach (var item in CommonDataService.ListOfCountries())
            {
                list.Add(new SelectListItem() {
                    Value = item.CountryName,
                    Text = item.CountryName
                });
            }
            return list;
        }
    }
}