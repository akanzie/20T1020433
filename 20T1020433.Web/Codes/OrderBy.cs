using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _20T1020433.DomainModels;

namespace _20T1020433.Web
{
    public static class OrderBy
    {
        public static List<Product> OrderByPriceAsc(List<Product> data)
        {
            for (int i = 0; i < data.Count - 1; i++)
            for (int j = 0; j < data.Count - i - 1; j++)
                if (data[j].Price > data[j + 1].Price)
                {
                    (data[j], data[j + 1]) = (data[j + 1], data[j]);
                }
            return data;
        }
        public static List<Product> OrderByPriceDesc(List<Product> data)
        {
            for (int i = 0; i < data.Count - 1; i++)
            for (int j = 0; j < data.Count - i - 1; j++)
                if (data[j].Price < data[j + 1].Price)
                {
                    (data[j], data[j + 1]) = (data[j + 1], data[j]);
                }
            return data;
        }
    }
}