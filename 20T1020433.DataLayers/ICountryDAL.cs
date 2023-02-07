using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _20T1020433.DomainModels;
namespace _20T1020433.DataLayers
{
    /// <summary>
    /// Các chức năng xử lý dữ liệu cho quốc gia
    /// </summary>
    public interface ICountryDAL
    {
        /// <summary>
        /// Lấy danh sách các quốc gia
        /// </summary>
        /// <returns></returns>
        IList<Country> List();
    }
}
