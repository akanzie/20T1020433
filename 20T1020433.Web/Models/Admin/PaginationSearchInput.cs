using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _20T1020433.Web.Models
{
    public class PaginationSearchInput
    {
        /// <summary>
        /// Trang cần hiển thị
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// Số dòng cần hiển thị mỗi trang
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Giá trị cần tìm
        /// </summary>
        public string SearchValue { get; set; }
    }
    public class ProductSearchInput : PaginationSearchInput
    {
        public int SortByPrice { get; set; } 
        public int CategoryID { get; set; } = 0;
        public int SupplierID { get; set; } = 0;
    }

    public class OrderSearchInput : PaginationSearchInput
    {
        public int Status { get; set; } = 0;
    }
}