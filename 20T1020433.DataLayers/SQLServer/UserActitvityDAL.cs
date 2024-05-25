using System;
using System.Data.SqlClient;
using Dapper;
namespace _20T1020433.DataLayers.SQLServer
{
    public class UserActitvityDAL : _BaseDAL, IUserActivityDAL
    {
        public UserActitvityDAL(string connectionString) : base(connectionString)
        {
        }

        public int LogSearch(int userId, string searchQuery)
        {
            try
            {

                var query = "INSERT INTO UserSearches (UserId, SearchQuery, SearchDate) VALUES (@UserId, @SearchQuery, @SearchDate)";
                using (var connection = OpenConnection())
                {
                    var parameters = new
                    {
                        UserId = userId,
                        SearchQuery = searchQuery,
                        SearchDate = DateTime.Now
                    };
                    connection.Execute(query,
                        parameters);
                }
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Đã xảy ra lỗi khi thêm lich su tim kiem: " + ex.Message);
                throw;
            }

        }

        public int LogView(int userId, int productId)
        {
            try
            {

                var query = "INSERT INTO UserViews (UserId, ProductId, ViewDate) VALUES (@UserId, @ProductId, @ViewDate)";
                using (var connection = OpenConnection())
                {
                    var parameters = new
                    {
                        UserId = userId,
                        ProductId = productId,
                        ViewDate = DateTime.Now
                    };
                    connection.Execute(query,
                        parameters);
                }
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Đã xảy ra lỗi khi thêm lich su xem mat hang: " + ex.Message);
                throw;
            }            
        }
    }
}
