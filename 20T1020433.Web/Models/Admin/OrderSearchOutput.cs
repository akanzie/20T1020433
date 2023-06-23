using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _20T1020433.DomainModels;

namespace _20T1020433.Web.Models
{
    public class OrderSearchOutput : PaginationSearchOutput
    {
        public List<Order> Data { get; set; }

        public int Status { get; set; } = 0;
    }
}