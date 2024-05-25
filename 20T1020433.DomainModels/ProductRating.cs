using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20T1020433.DomainModels
{
    public class ProductRating
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public double Rating { get; set; }
    }
}
