using _20T1020433.DomainModels;
using Dapper;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

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
                cmd.CommandText = @"SELECT COUNT(OrderDetailID)
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
        public List<ProductRating> GetRatings()
        {
            using (var connection = OpenConnection())
            {
                var query = @"
            SELECT UserId, ProductId, 1.0 AS Rating FROM UserViews
            UNION ALL
            SELECT o.CustomerId, od.ProductId, 2.0 AS Rating FROM Orders o
            JOIN OrderDetails od ON o.OrderId = od.OrderId
            UNION ALL
            SELECT UserId, Products.ProductId, 0.5 AS Rating
            FROM UserSearches
            JOIN Products ON UserSearches.SearchQuery LIKE '%' + Products.ProductName + '%'";
                return connection.Query<ProductRating>(query).ToList();
            }
        }

        public List<ProductRecommendation> GetRecommendations(int userId, int numberOfRecommendations)
        {
            var ratings = GetRatings();
            var userProductRatings = ratings
             .GroupBy(r => r.UserId)
             .ToDictionary(
                 g => g.Key,
                 g => g.GroupBy(r => r.ProductId)
                       .ToDictionary(
                           rg => rg.Key,
                           rg => rg.Average(r => r.Rating)
                       )
             );

            if (!userProductRatings.ContainsKey(userId))
                return new List<ProductRecommendation>();

            var userRatings = userProductRatings[userId];
            var allProductIds = GetAllProductIds();

            // Tạo vector từ dictionary
            var userVectors = userProductRatings.ToDictionary(
                u => u.Key,
                u => CreateUserVector(u.Value, allProductIds)
            );

            var similarities = userVectors
                .Where(u => u.Key != userId)
                .Select(u => new
                {
                    UserId = u.Key,
                    Similarity = CosineSimilarity(userVectors[userId], u.Value)
                })
                .OrderByDescending(u => u.Similarity)
                .ToList();

            // Tính điểm gợi ý sản phẩm
            var productScores = similarities
                .SelectMany(s => userProductRatings[s.UserId])
                .GroupBy(p => p.Key)
                .Select(g => new
                {
                    ProductId = g.Key,
                    Score = g.Sum(p => p.Value)
                })
                .OrderByDescending(p => p.Score)
                .Select(p => p.ProductId)
                .Except(userRatings.Keys)
                .Take(numberOfRecommendations)
                .ToList();

            return GetRecommendedProducts(productScores)
                .Select(p => new ProductRecommendation
                {
                    Product = p,
                    Score = userRatings.ContainsKey(p.ProductID) ? userRatings[p.ProductID] : 0
                })
                .ToList();
        }
        private Vector<double> CreateUserVector(Dictionary<int, double> ratings, List<int> allProductIds)
        {
            var vector = Vector<double>.Build.Dense(allProductIds.Count);

            for (int i = 0; i < allProductIds.Count; i++)
            {
                var productId = allProductIds[i];
                vector[i] = ratings.ContainsKey(productId) ? ratings[productId] : 0.0;
            }

            return vector;
        }
        public List<int> GetUserProductIds(int userId)
        {
            using (var connection = OpenConnection())
            {
                var query = @"
            SELECT ProductId FROM UserViews WHERE UserId = @UserId
            UNION
            SELECT od.ProductId FROM Orders o
            JOIN OrderDetails od ON o.OrderId = od.OrderId
            WHERE o.CustomerId = @UserId";
                return connection.Query<int>(query, new { UserId = userId }).ToList();
            }
        }

        public List<int> GetAllProductIds()
        {
            using (var connection = OpenConnection())
            {
                var query = "SELECT ProductId FROM Products";
                return connection.Query<int>(query).ToList();
            }
        }

        public List<Product> GetRecommendedProducts(List<int> recommendedProductIds)
        {
            using (var connection = OpenConnection())
            {
                var query = "SELECT * FROM Products WHERE ProductId IN @ProductIds";
                return connection.Query<Product>(query, new { ProductIds = recommendedProductIds }).ToList();
            }
        }

        private double CosineSimilarity(Vector<double> vectorA, Vector<double> vectorB)
        {
            return vectorA.DotProduct(vectorB) / (vectorA.L2Norm() * vectorB.L2Norm());
        }
    }
}
