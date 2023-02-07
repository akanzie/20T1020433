using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _20T1020433.DataLayers;
using _20T1020433.DataLayers.SQLServer;
using _20T1020433.DomainModels;
namespace _20T1020433.BusinessLayers
{
    /// <summary>
    /// Các chức năng nghiệp vụ liên quan đến: nhà cung cấp, khách hàng,
    /// người giao hàng, nhân viên, loại hàng
    /// </summary>
    public static class CommonDataService
    {
        private static ICountryDAL countryDB;
        private static ICommonDAL<Supplier> supplierDB;
        private static ICommonDAL<Shipper> shipperDB;
        /// <summary>
        /// Ctor
        /// </summary>
        static CommonDataService()
        {
            string connectionString = @"server=DESKTOP-BIAF6GR;user id=sa; password=1; database=LiteComerceDB";
            countryDB = new DataLayers.SQLServer.CountryDAL(connectionString);
            supplierDB = new DataLayers.SQLServer.SupplierDAL(connectionString);
            shipperDB = new DataLayers.SQLServer.ShipperDAL(connectionString);
        }
        /// <summary>
        /// Lấy danh sách các quốc gia
        /// </summary>
        /// <returns></returns>
        public static List<Country> ListOfCountries()
        {
            return countryDB.List().ToList();
        }
    }
}
