using _20T1020433.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _20T1020433.Web.Models
{
    /// <summary>
    /// Kết quả tìm kiếm nhân viên dưới dạng phân trang
    /// </summary>
    public class EmployeeSearchOutput : PaginationSearchOutput
    {
        /// <summary>
        /// Danh sách nhân viên
        /// </summary>
        public List<Employee> Data { get; set; }
    }
}