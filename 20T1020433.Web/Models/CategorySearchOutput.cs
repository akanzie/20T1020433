using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _20T1020433.DomainModels;

namespace _20T1020433.Web.Models
{
    public class CategorySearchOutput : PaginationSearchOutput
    {
        /// <summary>
        /// Danh sách nhà cung cấp
        /// </summary>
        public List<Category> Data { get; set; }
    }
}