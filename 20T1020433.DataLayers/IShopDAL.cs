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
    }
}
