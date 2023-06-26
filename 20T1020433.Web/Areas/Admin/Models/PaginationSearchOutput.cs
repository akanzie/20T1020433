using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _20T1020433.Web.Areas.Admin.Models
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class PaginationSearchOutput
    {
        /// <summary>
        /// Trang hiển thị
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// Số dòng trên mỗi trang
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Giá trị đang được tìm kiếm
        /// </summary>
        public string SearchValue { get; set; }
        /// <summary>
        /// Số dòng tìm được
        /// </summary>
        public int RowCount { get; set; }
        /// <summary>
        /// Tổng số trang
        /// </summary>
        public int PageCount
        {
            get
            {
                if (PageSize == 0)
                    return 1;
                int p = RowCount / PageSize;
                if (RowCount % PageSize > 0)
                    p += 1;
                return p;
            }
        }
    }
}