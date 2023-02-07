using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20T1020433.DataLayers
{
    /// <summary>
    /// Định nghĩa các phép xử lý chung cho các dữ liệu
    /// đơn giản trên các bảng
    /// </summary>
    public interface ICommonDAL<T> where T: class
    {
        /// <summary>
        /// Tìm kiếm và lấy danh sách dữ liệu dưới dạng phân trang
        /// </summary>
        /// <param name="page">Trang cần hiển thị</param>
        /// <param name="pagesize">Số dòng được hiển thị trên mỗi trang (0 tức là không yêu cầu phân trang)</param>
        /// <param name="searchValue">Giá trị cần tìm kiếm (chuỗi rỗng nếu không tìm kiếm)</param>
        /// <returns></returns>
        IList<T> List(int page = 1, int pagesize = 0, string searchValue = "");
        /// <summary>
        /// Đếm số dòng dữ liệu cấp tìm được
        /// </summary>
        /// <param name="searchValue">Giá trị cần tìm kiếm (chuỗi rỗng nếu không tìm kiếm)</param>
        /// <returns></returns>
        int Count(string searchValue = "");
        /// <summary>
        /// Bổ sung 
        /// </summary>
        /// <param name="data"></param>
        /// <returns>ID của dữ liệu được tạo mới</returns>
        int Add(T data);
        /// <summary>
        /// Cập nhật thông tin của dữ liệu
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(T data);
        /// <summary>
        /// Xóa dựa vào mã của dữ liệu
        /// </summary>
        /// <param name="id">Mã của dữ liệu cần xóa</param>
        /// <returns></returns>
        bool Delete(int id);
        /// <summary>
        /// Lấy thông tin của nhà cung cấp
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Get(int id);
        /// <summary>
        /// Kiểm tra xem dữ liệu hiện có dữ liệu liên quan hay không?
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool InUsed(int id);
    }
}
