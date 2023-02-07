using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20T1020433.DomainModels
{
    /// <summary>
    /// 
    /// </summary>
    public class Category
    {
        /// <summary>
        /// 
        /// </summary>
        public int CategoryID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ParentCategoryId { get; set; }
    }
}
