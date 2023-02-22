using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace _20T1020433.Web
{
    public static class Converter
    {
        /// <summary>
        /// Chuyen một chuỗi DMY sang DateTime
        /// </summary>
        /// <param name="s"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime? DMYStringToDateTime(string s, string format = "d/M/yyyy")
        {
            try
            {
                return DateTime.ParseExact(s, format, CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime? YMDStringToDateTime(string s, string format = "yyyy-MM-dd")
        {
            try
            {
                return DateTime.ParseExact(s, format, CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }
    }
}