using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using _20T1020433.DomainModels;

namespace _20T1020433.Web
{
    public static class Converter
    {
        public static decimal? StringToDecimal(string s)
        {
            try
            {
                return decimal.Parse(s, CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static UserAccount CookieToUserAccount(string cookie)
        {
            // Json -> obj
            return Newtonsoft.Json.JsonConvert.DeserializeObject<UserAccount>(cookie);
        }
        
        //public 
    }
}