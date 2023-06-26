using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _20T1020433.DomainModels;

namespace _20T1020433.Web.Areas.Admin.Models
{
    public class CustomerSearchOutput : PaginationSearchOutput
    {
        /// <summary>
    /// Danh sách khách hàng
    /// </summary>
    public List<Customer> Data { get; set; }

    }
}