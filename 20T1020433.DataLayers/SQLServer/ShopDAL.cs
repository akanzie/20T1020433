using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20T1020433.DataLayers.SQLServer
{
    public class ShopDAL : _BaseDAL, IShopDAL
    {
        public ShopDAL(string connectionString) : base(connectionString)
        {
        }
        public int CountProductSold(int productID)
        {
            int count = 0;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT	COUNT(OrderDetailID)
                                    FROM Products AS p LEFT JOIN OrderDetails AS od ON p.ProductID = od.ProductID		                                               
                                    Where p.ProductID = @ProductID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@ProductID", productID);

                count = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
            }
            return count;
        }
    }
}
