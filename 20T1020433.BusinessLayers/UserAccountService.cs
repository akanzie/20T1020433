using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _20T1020433.DataLayers;
using _20T1020433.DomainModels;

namespace _20T1020433.BusinessLayers
{
    /// <summary>
    /// Các chức năng liên quan đến tài khoản
    /// </summary>
    public class UserAccountService
    {

        private static IUserAccountDAL employeeAccountDB;
        private static IUserAccountDAL customerAccountDB;
        private static IUserAccountDAL shipperAccountDB;

        /// <summary>
        /// 
        /// </summary>
        static UserAccountService()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
            employeeAccountDB = new DataLayers.SQLServer.EmployeeAccountDAL(connectionString);
            customerAccountDB = new DataLayers.SQLServer.CustomerAccountDAL(connectionString);
            shipperAccountDB = new DataLayers.SQLServer.ShipperAccountDAL(connectionString);

        }

        public static UserAccount Authorize(AccountTypes accountType, string userName, string password)
        {
            if (accountType == AccountTypes.Employee)
            {
                return employeeAccountDB.Authorize(userName, password);
            } 
            else if (accountType == AccountTypes.Customer) 
            {
                return customerAccountDB.Authorize(userName, password);
            }
            else
            {
                return shipperAccountDB.Authorize(userName, password);
            }
        }

        public static bool ChangePassword(AccountTypes accountType, string userName, string password, string oldPassword)
        {
            if (accountType == AccountTypes.Employee)

            {
                return employeeAccountDB.ChangePassword(userName, password, oldPassword);
            }
            else if (accountType == AccountTypes.Customer)
            {
                return customerAccountDB.ChangePassword(userName, password, oldPassword);
            }
            else
            {
                return shipperAccountDB.ChangePassword(userName, password, oldPassword);
            }
        }
        
    }
}
