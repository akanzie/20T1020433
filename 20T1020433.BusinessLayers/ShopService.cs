using _20T1020433.DataLayers;
using _20T1020433.DataLayers.SQLServer;
using _20T1020433.DomainModels;
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
        private static IProductDAL productDB;
        private static IUserActivityDAL userActivityDB;
        static ShopService()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
            shopDB = new ShopDAL(connectionString);
            productDB = new ProductDAL(connectionString);
            userActivityDB = new UserActitvityDAL(connectionString);
        }
        public static int CountProductSold(int productID)
        {
            return shopDB.CountProductSold(productID);
        }
        public static List<ProductRating> GetRatings()
        {
            return shopDB.GetRatings();

        }
        public static List<ProductRecommendation> GetRecommendations(int userId, int numberOfRecommendations)
        {
            return shopDB.GetRecommendations(userId, numberOfRecommendations);
        }
        public static List<int> GetUserProductIds(int userId)
        {
            return shopDB.GetUserProductIds(userId);

        }
        public static List<int> GetAllProductIds()
        {
            return shopDB.GetAllProductIds();

        }
        public static List<Product> GetRecommendedProducts(List<int> recommendedProductIds)
        {
            return shopDB.GetRecommendedProducts(recommendedProductIds);
        }
        public static List<Product> ListProducts(int userId, int page, int pageSize, string searchValue, int categoryID, int supplierID, int sortByPrice, out int rowCount)
        {
            rowCount = productDB.Count(searchValue, categoryID, supplierID);
            if (searchValue != null || searchValue != "")
                userActivityDB.LogSearch(userId, searchValue);
            return productDB.List(page, pageSize, searchValue, categoryID, supplierID, sortByPrice).ToList();
        }
        public static Product GetProduct(int userId, int productID)
        {
            userActivityDB.LogView(userId, productID);
            return productDB.Get(productID);

        }
    }
}
