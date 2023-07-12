using _20T1020433.DataLayers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20T1020433.BusinessLayers
{
    public static class ShopService
    {
        private static IShopDAL shopDB;

        static ShopService()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
            shopDB = new DataLayers.SQLServer.ShopDAL(connectionString);
        }
        public static int CountProductSold (int productID)
        {
            return shopDB.CountProductSold(productID);
        }
    }
}
