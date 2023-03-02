using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _20T1020433.DomainModels;

namespace _20T1020433.Web.Models
{
    public class ProductSearchOutput : PaginationSearchOutput
    {
        /// <summary>
        /// Danh sách mặt hàng
        /// </summary>
        public List<Product> Data { get; set; }

        public int OrderByPrice { get; set; } = 0;
        public int CategoryID { get; set; } = 0;
        public int SupplierID { get; set; } = 0;
    }
}