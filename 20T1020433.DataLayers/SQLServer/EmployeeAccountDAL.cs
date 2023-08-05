using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _20T1020433.DomainModels;

namespace _20T1020433.DataLayers.SQLServer
{
    public class EmployeeAccountDAL : _BaseDAL, IUserAccountDAL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public EmployeeAccountDAL(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public UserAccount Authorize(string userName, string password)
        {
            UserAccount data = null;
            using (SqlConnection connection = OpenConnection())
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"SELECT EmployeeID, FirstName, LastName, Email, Photo, Role
                                    FROM   Employees
                                    WHERE  Email = @Email AND Password = @Password";

                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@Email", userName);
                cmd.Parameters.AddWithValue("@Password", password);

                using (var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new UserAccount()
                        {
                            UserId = Convert.ToString(dbReader["EmployeeID"]),
                            UserName = Convert.ToString(dbReader["Email"]),
                            FullName = $"{dbReader["FirstName"]} {dbReader["LastName"]}",
                            Email = Convert.ToString(dbReader["Email"]),
                            Photo = Convert.ToString(dbReader["Photo"]),
                            Password = "",
                            RoleNames = Convert.ToString(dbReader["Role"])

                        };

                    }
                    dbReader.Close();
                }
                connection.Close();
            }
            return data;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var cmd = connection.CreateCommand();
                cmd.CommandText = "UPDATE Employees SET Password=@NewPassword WHERE Email=@Email AND Password=@OldPassword";
                cmd.Parameters.AddWithValue("@Email", userName);
                cmd.Parameters.AddWithValue("@NewPassword", newPassword);
                cmd.Parameters.AddWithValue("@OldPassword", oldPassword);

                result = cmd.ExecuteNonQuery() > 0;
                connection.Close();
            }
            return result;
        }
        public IList<Role> GetRoles(int userID)
        {
            List<Role> data = new List<Role>();           

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT EmployeeID, e.RoleID, RoleName
                                    FROM Employee_Roles  e inner join Roles r on e.RoleID = r.RoleID
                                    WHERE EmployeeID = @EmployeeID;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@EmployeeID", userID);
                var dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dbReader.Read())
                {
                    data.Add(new Role()
                    {
                        UserID = Convert.ToInt32(dbReader["EmployeeID"]),
                        RoleID = Convert.ToInt32(dbReader["RoleID"]),
                        RoleName = Convert.ToString(dbReader["RoleName"])
                    });
                }
                dbReader.Close();
                cn.Close();
            }

            return data;
        }
        public bool IsInRole(int roleID, int employeeID)
        {
            using (var connection = OpenConnection())
            {
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"SELECT 1
                                    FROM Employee_Roles  e
                                    WHERE RoleID = @RoleID and EmployeeID = @EmployeeID;";
                cmd.Parameters.AddWithValue("@RoleID", roleID);
                cmd.Parameters.AddWithValue("@EmployeeID", employeeID);

                object result = cmd.ExecuteScalar();                
                connection.Close();
                if (result != null && result != DBNull.Value)
                {
                    return true;
                }
            }
            return false;
        }        
    }
}
