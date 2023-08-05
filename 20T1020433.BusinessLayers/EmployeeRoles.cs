using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20T1020433.BusinessLayers
{
    public class EmployeeRoles
    {
        /// <summary>
        /// Quản lý loại hàng
        /// </summary>
        public const int MANAGE_CATEGORY = 1;
        /// <summary>
        /// Quản lý nhân viên
        /// </summary>
        public const int MANAGE_EMPLOYEE = 2;
        /// <summary>
        /// Quản lý hóa đơn
        /// </summary>
        public const int MANAGE_ORDER = 6;
        /// <summary>
        /// Quản lý mặt hàng
        /// </summary>
        public const int MANAGE_PRODUCT = 4;
        /// <summary>
        /// Quản lý khách hàng
        /// </summary>
        public const int MANAGE_CUSTOMER = 3;
        /// <summary>
        /// Quản lý nhà cung cấp
        /// </summary>
        public const int MANAGE_SUPPLIER = 5;
        /// <summary>
        /// Quản lý người giao hàng
        /// </summary>
        public const int MANAGE_SHIPPER = 7;
        /// <summary>
        /// Lập hóa đơn
        /// </summary>
        public const int CREATE_ORDER = 14;
    }
}
