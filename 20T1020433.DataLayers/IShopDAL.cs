using _20T1020433.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20T1020433.DataLayers
{
    public interface IShopDAL
    {
        int CountProductSold(int productID);
        List<ProductRating> GetRatings();
        List<ProductRecommendation> GetRecommendations(int userId, int numberOfRecommendations);
        List<int> GetUserProductIds(int userId);
        List<int> GetAllProductIds();
        List<Product> GetRecommendedProducts(List<int> recommendedProductIds);
    }
}
