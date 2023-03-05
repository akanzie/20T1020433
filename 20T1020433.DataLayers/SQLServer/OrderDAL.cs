using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _20T1020433.DomainModels;

namespace _20T1020433.DataLayers.SQLServer
{
    public class OrderDAL : _BaseDAL, IOrderDAL
    {
        public OrderDAL(string connectionString) : base(connectionString)
        {
        }

        public IList<Order> List(int page = 1, int pageSize = 0, int status = 0, string searchValue = "")
        {
            throw new NotImplementedException();
        }

        public int Count(int status = -99, string searchValue = "")
        {
            throw new NotImplementedException();
        }

        public Order Get(int orderID)
        {
            throw new NotImplementedException();
        }

        public int Add(Order data, IEnumerable<OrderDetail> details)
        {
            throw new NotImplementedException();
        }

        public bool Update(Order data)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int orderID)
        {
            throw new NotImplementedException();
        }

        public IList<OrderDetail> ListDetails(int orderID)
        {
            throw new NotImplementedException();
        }

        public OrderDetail GetDetail(int orderID, int productID)
        {
            throw new NotImplementedException();
        }

        public int SaveDetail(int orderID, int productID, int quantity, decimal salePrice)
        {
            throw new NotImplementedException();
        }

        public bool DeleteDetail(int orderID, int productID)
        {
            throw new NotImplementedException();
        }
    }
}
