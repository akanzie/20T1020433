using _20T1020433.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _20T1020433.Web.Models
{
    public class OrderModel 
    {
        public Order Order { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public decimal Total { get; set; } = 0;
    }
}