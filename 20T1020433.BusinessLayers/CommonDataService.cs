using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _20T1020433.DataLayers;
using _20T1020433.DataLayers.SQLServer;
using _20T1020433.DomainModels;
using System.Configuration;
using System.Globalization;

namespace _20T1020433.BusinessLayers
{
    /// <summary>
    /// Các chức năng nghiệp vụ liên quan đến: nhà cung cấp, nhân vie,
    /// người giao hàng, khách hàng, loại hàng
    /// </summary>
    public static class CommonDataService
    {
        private static ICountryDAL countryDB;
        private static ICommonDAL<Supplier> supplierDB;
        private static ICommonDAL<Shipper> shipperDB;
        private static ICommonDAL<Customer> customerDB;
        private static ICommonDAL<Employee> employeeDB;
        private static ICommonDAL<Category> categoryDB;
        /// <summary>
        /// Ctor
        /// </summary>
        static CommonDataService()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
            countryDB = new DataLayers.SQLServer.CountryDAL(connectionString);
            supplierDB = new DataLayers.SQLServer.SupplierDAL(connectionString);
            shipperDB = new DataLayers.SQLServer.ShipperDAL(connectionString);
            customerDB = new DataLayers.SQLServer.CustomerDAL(connectionString);
            categoryDB = new DataLayers.SQLServer.CategoryDAL(connectionString);
            employeeDB = new DataLayers.SQLServer.EmployeeDAL(connectionString);
        }

        #region Các nghiệp vụ liên quan đến quốc gia
        /// <summary>
        /// Lấy danh sách các quốc gia
        /// </summary>
        /// <returns></returns>
        public static List<Country> ListOfCountries()
        {
            return countryDB.List().ToList();
        }
        #endregion

        #region Các nghiệp vụ liên quan đến nhà cung cấp

        /// <summary>
        /// Tìm kiếm, lấy danh sách các nhà cung cấp dưới dạng phân trang
        /// </summary>
        /// <param name="page">Trang cần xem</param>
        /// <param name="pagesize">Số dòng trên mỗi trang (0 tức là không phân trang)</param>
        /// <param name="searchValue">Giá trị tìm kiếm (rỗng tức là không tìm kiếm</param>
        /// <param name="rowCount">Output: Tổng số dòng tìm được</param>
        /// <returns></returns>
        public static List<Supplier> ListOfSuppliers(int page, int pageSize, string searchValue, out int rowCount)
        {
            rowCount = supplierDB.Count(searchValue);
            return supplierDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// Tìm kiếm và lấy danh sách các nhà cung cấp (không phân trang)
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Supplier> ListOfSuppliers(string searchValue)
        {
            return supplierDB.List(1, 0, searchValue).ToList();
        }
        /// <summary>
        /// Bổ sung nhà cung cấp
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Mã của nhà cung cấp được bổ sung</returns>
        public static int AddSupplier(Supplier data)
        {
            return supplierDB.Add(data);
        }
        /// <summary>
        /// Cập nhật thông tin của nhà cung cấp
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateSupplier(Supplier data)
        {
            return supplierDB.Update(data);
        }
        /// <summary>
        /// Xóa một nhà cung cấp
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        public static bool DeleteSupplier(int supplierID)
        {
            return supplierDB.Delete(supplierID);
        }
        /// <summary>
        /// Lấy thông tin của 1 nhà cung cấp
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        public static Supplier GetSupplier(int supplierID)
        {
            return supplierDB.Get(supplierID);
        }
        /// <summary>
        /// Kiểm tra xem 1 nhà cung cấp hiện có dữ liệu liên quan hay không?
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        public static bool InUsedSupplier(int supplierID)
        {
            return supplierDB.InUsed(supplierID);
        }
       
        #endregion

        #region Các nghiệp vụ liên quan đến người giao hàng

        /// <summary>
        /// Tìm kiếm, lấy danh sách các người giao hàng dưới dạng phân trang
        /// </summary>
        /// <param name="page">Trang cần xem</param>
        /// <param name="pageSize">Số dòng trên mỗi trang (0 tức là không phân trang)</param>
        /// <param name="searchValue">Giá trị tìm kiếm (rỗng tức là không tìm kiếm</param>
        /// <param name="rowCount">Output: Tổng số dòng tìm được</param>
        /// <returns></returns>
        public static List<Shipper> ListOfShippers(int page, int pageSize, string searchValue, out int rowCount)
        {
            rowCount = shipperDB.Count(searchValue);
            return shipperDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// Tìm kiếm và lấy danh sách các người giao hàng (không phân trang)
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Shipper> ListOfShippers(string searchValue)
        {
            return shipperDB.List(1, 0, searchValue).ToList();
        }
        /// <summary>
        /// Bổ sung người giao hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Mã của người giao hàng được bổ sung</returns>
        public static int AddShipper(Shipper data)
        {
            return shipperDB.Add(data);
        }
        /// <summary>
        /// Cập nhật thông tin của người giao hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateShipper(Shipper data)
        {
            return shipperDB.Update(data);
        }
        /// <summary>
        /// Xóa một người giao hàng
        /// </summary>
        /// <param name="shipperID"></param>
        /// <returns></returns>
        public static bool DeleteShipper(int shipperID)
        {
            return shipperDB.Delete(shipperID);
        }
        /// <summary>
        /// Lấy thông tin của 1 người giao hàng
        /// </summary>
        /// <param name="shipperID"></param>
        /// <returns></returns>
        public static Shipper GetShipper(int shipperID)
        {
            return shipperDB.Get(shipperID);
        }
        /// <summary>
        /// Kiểm tra xem 1 người giao hàng hiện có dữ liệu liên quan hay không?
        /// </summary>
        /// <param name="shipperID"></param>
        /// <returns></returns>
        public static bool InUsedShipper(int shipperID)
        {
            return shipperDB.InUsed(shipperID);
        }

        #endregion

        #region Các nghiệp vụ liên quan đến nhân viên

        /// <summary>
        /// Tìm kiếm, lấy danh sách các nhân viên dưới dạng phân trang
        /// </summary>
        /// <param name="page">Trang cần xem</param>
        /// <param name="pageSize">Số dòng trên mỗi trang (0 tức là không phân trang)</param>
        /// <param name="searchValue">Giá trị tìm kiếm (rỗng tức là không tìm kiếm</param>
        /// <param name="rowCount">Output: Tổng số dòng tìm được</param>
        /// <returns></returns>
        public static List<Employee> ListOfEmployees(int page, int pageSize, string searchValue, out int rowCount)
        {
            rowCount = employeeDB.Count(searchValue);
            return employeeDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// Tìm kiếm và lấy danh sách các nhân viên (không phân trang)
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Employee> ListOfEmployees(string searchValue)
        {
            return employeeDB.List(1, 0, searchValue).ToList();
        }
        /// <summary>
        /// Bổ sung nhân viên
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Mã của nhân viên được bổ sung</returns>
        public static int AddEmployee(Employee data)
        {
           return employeeDB.Add(data);
        }
        /// <summary>
        /// Cập nhật thông tin của nhân viên
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateEmployee(Employee data)
        {
            return employeeDB.Update(data);
        }
        /// <summary>
        /// Xóa một nhân viên
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public static bool DeleteEmployee(int employeeID)
        {
            return employeeDB.Delete(employeeID);
        }
        /// <summary>
        /// Lấy thông tin của 1 nhân viên
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public static Employee GetEmployee(int employeeID)
        {
            return employeeDB.Get(employeeID);
        }
        /// <summary>
        /// Kiểm tra xem 1 nhân viên hiện có dữ liệu liên quan hay không?
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public static bool InUsedEmployee(int employeeID)
        {
            return employeeDB.InUsed(employeeID);
        }
        #endregion

        #region Các nghiệp vụ liên quan đến khách hàng

        /// <summary>
        /// Tìm kiếm, lấy danh sách các khách hàng dưới dạng phân trang
        /// </summary>
        /// <param name="page">Trang cần xem</param>
        /// <param name="pageSize">Số dòng trên mỗi trang (0 tức là không phân trang)</param>
        /// <param name="searchValue">Giá trị tìm kiếm (rỗng tức là không tìm kiếm</param>
        /// <param name="rowCount">Output: Tổng số dòng tìm được</param>
        /// <returns></returns>
        public static List<Customer> ListOfCustomers(int page, int pageSize, string searchValue, out int rowCount)
        {
            rowCount = customerDB.Count(searchValue);
            return customerDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// Tìm kiếm và lấy danh sách các khách hàng (không phân trang)
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Customer> ListOfCustomers(string searchValue)
        {
            return customerDB.List(1, 0, searchValue).ToList();
        }
        /// <summary>
        /// Bổ sung khách hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Mã của khách hàng được bổ sung</returns>
        public static int AddCustomer(Customer data)
        {
            return customerDB.Add(data);
        }
        /// <summary>
        /// Cập nhật thông tin của khách hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateCustomer(Customer data)
        {
            return customerDB.Update(data);
        }
        /// <summary>
        /// Xóa một khách hàng
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public static bool DeleteCustomer(int customerID)
        {
            return customerDB.Delete(customerID);
        }
        /// <summary>
        /// Lấy thông tin của 1 khách hàng
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public static Customer GetCustomer(int customerID)
        {
            return customerDB.Get(customerID);
        }
        /// <summary>
        /// Kiểm tra xem 1 khách hàng hiện có dữ liệu liên quan hay không?
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public static bool InUsedCustomer(int customerID)
        {
            return customerDB.InUsed(customerID);
        }
        #endregion

        #region Các nghiệp vụ liên quan đến loại hàng

        /// <summary>
        /// Tìm kiếm, lấy danh sách các loại hàng dưới dạng phân trang
        /// </summary>
        /// <param name="page">Trang cần xem</param>
        /// <param name="pageSize">Số dòng trên mỗi trang (0 tức là không phân trang)</param>
        /// <param name="searchValue">Giá trị tìm kiếm (rỗng tức là không tìm kiếm</param>
        /// <param name="rowCount">Output: Tổng số dòng tìm được</param>
        /// <returns></returns>
        public static List<Category> ListOfCategories(int page, int pageSize, string searchValue, out int rowCount)
        {
            rowCount = categoryDB.Count(searchValue);
            return categoryDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// Tìm kiếm và lấy danh sách các loại hàng (không phân trang)
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Category> ListOfCategories(string searchValue)
        {
            return categoryDB.List(1, 0, searchValue).ToList();
        }
        /// <summary>
        /// Bổ sung loại hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Mã của loại hàng được bổ sung</returns>
        public static int AddCategory(Category data)
        {
            return categoryDB.Add(data);
        }
        /// <summary>
        /// Cập nhật thông tin của loại hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateCategory(Category data)
        {
            return categoryDB.Update(data);
        }
        /// <summary>
        /// Xóa một loại hàng
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public static bool DeleteCategory(int categoryID)
        {
            return categoryDB.Delete(categoryID);
        }
        /// <summary>
        /// Lấy thông tin của 1 loại hàng
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public static Category GetCategory(int categoryID)
        {
            return categoryDB.Get(categoryID);
        }
        /// <summary>
        /// Kiểm tra xem 1 loại hàng hiện có dữ liệu liên quan hay không?
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public static bool InUsedCategory(int categoryID)
        {
            return categoryDB.InUsed(categoryID);
        }
        
        #endregion
    }
}
