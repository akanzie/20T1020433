﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _20T1020433.DomainModels;

namespace _20T1020433.DataLayers
{
    /// <summary>
    /// Định nghĩa các phép xữ lý dữ liệu trên nhà cung cấp
    /// Sử dụng cách này dânx dến viết lặp đi lặp lại các kiểu code giống nhau
    /// cho các đối tượng
    /// sử dụng generate
    /// </summary>
    public interface ISupplierDAL
    {
        /// <summary>
        /// Tìm kiếm và lấy danh sách các nhà cung cấp dưới dạng phân trang
        /// </summary>
        /// <param name="page">Trang cần hiển thị</param>
        /// <param name="pagesize">Số dòng được hiển thị trên mỗi trang (0 tức là không yêu cầu phân trang)</param>
        /// <param name="searchValue">Tên cần tìm kiếm (chuỗi rỗng nếu không tìm kiếm theo tên)</param>
        /// <returns></returns>
        IList<Supplier> List(int page = 1, int pagesize = 0, string searchValue = "");
        /// <summary>
        /// Đếm số nhà cung cấp tìm được
        /// </summary>
        /// <param name="searchValue">Tên cần tìm kiếm (chuỗi rỗng nếu không tìm kiếm theo tên)</param>
        /// <returns></returns>
        int Count(string searchValue = "");
        /// <summary>
        /// Bổ sung thêm một nhà cung cấp
        /// </summary>
        /// <param name="data"></param>
        /// <returns>ID của nhà cung cấp được tạo mới</returns>
        int Add(Supplier data);
        /// <summary>
        /// Cập nhật thông tin của nhà cung cấp
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Supplier data);
        /// <summary>
        /// Xóa một nhà cung cấp dựa vào mã của nhà cung cấp
        /// </summary>
        /// <param name="supplierID">Mã của nhà cung cấp cần xóa</param>
        /// <returns></returns>
        bool Delete(int supplierID);
        /// <summary>
        /// Lấy thông tin của nhà cung cấp
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        Supplier Get(int supplierID);
        /// <summary>
        /// Kiểm tra xem nhà cung cấp hiện có dữ liệu liên quan hay không?
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        bool InUsed(int supplierID);
    }
}
