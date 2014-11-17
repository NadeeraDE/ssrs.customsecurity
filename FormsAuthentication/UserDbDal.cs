using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.Configuration;
using Microsoft.Samples.ReportingServices.CustomSecurity.helpers;

namespace Microsoft.Samples.ReportingServices.CustomSecurity
{
    public static class UserDbDal
    {
        public static DataTable GetAllUsers()
        {
            var dt = new DataTable();
            using (var conn = new SqlConnection(ConfigHelper.ConnectionString))
            {
                var cmd = new SqlCommand("select u.UserName,u.GroupName,u.Email," + @"substring(
                                        (
                                            Select ','+r.RoleName  AS [text()]
                                            From dbo.UserRoles ur inner join Roles r on r.RoleId=ur.FkRoleId
                                            Where ur.FkUserName = u.UserName
                                            For XML PATH ('')
                                        ), 2, 1000) [RoleNames] FROM Users u");
                var sda = new SqlDataAdapter();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                conn.Open();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
            }
            return dt;
        }

        public static void DeleteUser(string userName)
        {
            using (var conn = new SqlConnection(ConfigHelper.ConnectionString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "delete from UserRoles where FkUserName=@UserName; delete from  Users where UserName=@UserName;";
                cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userName;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdateUser(string userName, string group, string password, string passwordHash, string salt, string email, List<int> roles )
        {
            using (var conn = new SqlConnection(ConfigHelper.ConnectionString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update Users set GroupName=@GroupName,Email=@Email" +
                                  (string.IsNullOrEmpty(password) ? " " : ",PasswordHash=@PasswordHash,salt=@salt ") +
                                  "where UserName=@UserName; " +
                                  "delete from UserRoles where FkUserName=@UserName";
                cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userName;
                cmd.Parameters.Add("@GroupName", SqlDbType.VarChar).Value = group;
                cmd.Parameters.Add("@PasswordHash", SqlDbType.VarChar).Value = passwordHash;
                cmd.Parameters.Add("@salt", SqlDbType.VarChar).Value = salt;
                cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = email;
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            foreach (var role in roles)
            {
                AddUserRole(userName, role);
            }

            HttpContext.Current.Cache.Remove(ADMIN_USERROLES_CACHEKEY);
        }

        public static void AddUserRole(string userName, int roleId)
        {
            using (var conn = new SqlConnection(ConfigHelper.ConnectionString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert into UserRoles(FkRoleId,FkUserName) values(@FkRoleId,@FkUserName)";
                cmd.Parameters.Add("@FkUserName", SqlDbType.VarChar).Value = userName;
                cmd.Parameters.Add("@FkRoleId", SqlDbType.Int).Value = roleId;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static void ResetPassword(string userName, string passwordHash, string salt)
        {
            using (var conn = new SqlConnection(ConfigHelper.ConnectionString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "update Users set PasswordHash=@PasswordHash,salt=@Salt where UserName=@UserName";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@PasswordHash", SqlDbType.VarChar).Value = passwordHash;
                cmd.Parameters.Add("@Salt", SqlDbType.VarChar).Value = salt;
                cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userName;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static AppUser HasUser(string userName, string email)
        {
            AppUser user = null;
            using (var conn = new SqlConnection(ConfigHelper.ConnectionString))
            {
                string whereCl = string.Empty;
                string whereClVal = string.Empty;
                if (!string.IsNullOrEmpty(userName))
                {
                    whereCl = "UserName=@WhereCl";
                    whereClVal = userName;
                }
                else if (!string.IsNullOrEmpty(email))
                {
                    whereCl = "Email=@WhereCl";
                    whereClVal = email;
                }
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select top 1 Email,UserName,PasswordHash,salt,GroupName from Users where " + whereCl;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@WhereCl", SqlDbType.VarChar).Value = whereClVal;
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read(); // Advance to the one and only row
                    // Return output parameters from returned data stream
                    user = new AppUser
                           {
                               Email = reader.GetString(0),
                               UserName = reader.GetString(1),
                               PasswordHash = reader.GetString(2),
                               Salt = reader.GetString(3),
                               GroupName = reader.GetString(4)
                           };
                }
            }
            return user;
        }

        public static bool UserNameExists(string userName)
        {
            var result = false;
            using (var conn = new SqlConnection(ConfigHelper.ConnectionString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select top 1 UserName from Users where UserName=@UserName";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userName;
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    result = reader.HasRows;
                }
            }
            return result;
        }

        /*public static string GetGroupName(string userName)
        {
            string groupName = string.Empty;
            var user = HasUser(userName, string.Empty);
            if (user != null)
            {
                groupName = user.GroupName;
            }
            return groupName;
        }*/

        public static void AddNewUser(string userName, string passwordHash, string salt, string groupName, string email, List<int> roles )
        {
            using (var conn = new SqlConnection(ConfigHelper.ConnectionString))
            {
                var cmd = new SqlCommand("RegisterUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@userName", SqlDbType.VarChar, 40).Value = userName;
                cmd.Parameters.Add("@passwordHash", SqlDbType.VarChar, 50).Value = passwordHash;
                cmd.Parameters.Add("@salt", SqlDbType.VarChar, 10).Value = salt;
                cmd.Parameters.Add("@groupName", SqlDbType.VarChar, 50).Value = groupName;
                cmd.Parameters.Add("@email", SqlDbType.VarChar, 50).Value = email;

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            foreach (var role in roles)
            {
                AddUserRole(userName, role);
            }
            HttpContext.Current.Cache.Remove(ADMIN_USERROLES_CACHEKEY);
        }

        public static bool HasUserOrRole(string name)
        {
            var result = false;
            /*using (var conn = new SqlConnection(ConfigHelper.ConnectionString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select top 1 UserName from Users where UserName=@Name OR GroupName=@Name";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = name;
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    result = reader.HasRows;
                }
            }*/

            using (var conn = new SqlConnection(ConfigHelper.ConnectionString))
            {
                var cmd = new SqlCommand("HasUserOrRole", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@name", SqlDbType.VarChar, 40).Value = name;
                var outParam = cmd.Parameters.Add("@hasuserOrRole", SqlDbType.Bit, 50);
                outParam.Direction = ParameterDirection.Output;
                conn.Open();
                cmd.ExecuteNonQuery();
                result = (bool)outParam.Value;
            }
            return result;
        }

        public static DataTable GetAllRoles()
        {
            var dt = new DataTable();
            if (HttpContext.Current == null || HttpContext.Current.Cache[ALL_ROLES_CACHEKEY] == null)
            {
                using (var conn = new SqlConnection(ConfigHelper.ConnectionString))
                {
                    var cmd = new SqlCommand("select RoleId,RoleName,IsAdministrator from Roles");
                    var sda = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;
                    conn.Open();
                    sda.SelectCommand = cmd;
                    sda.Fill(dt);
                }
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Cache[ALL_ROLES_CACHEKEY] = dt; 
                }
            }
            else
            {
                dt = (DataTable)HttpContext.Current.Cache[ALL_ROLES_CACHEKEY];
            }
            return dt;
            
        }

        public static List<string> GetAdminUsersAndRoles()
        {
            var dt = new List<string>();
            if (HttpContext.Current == null || HttpContext.Current.Cache[ADMIN_USERROLES_CACHEKEY] == null)
            {
                using (var conn = new SqlConnection(ConfigHelper.ConnectionString))
                {
                    var cmd = new SqlCommand("GetAdminUsersAndRoles");
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var roleName = reader.GetString(0);
                            var userName = reader.GetString(1);
                            if (!dt.Contains(roleName))
                            {
                                dt.Add(roleName);
                            }
                            if (!dt.Contains(userName))
                            {
                                dt.Add(userName);
                            }
                        }
                    }
                }
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Cache[ADMIN_USERROLES_CACHEKEY] = dt;
                }
            }
            else
            {
                dt = (List<string>)HttpContext.Current.Cache[ADMIN_USERROLES_CACHEKEY];
            }
            return dt;

        }
        
        public static void UpdateRole(int roleId, string roleName, bool isAdmin)
        {
            using (var conn = new SqlConnection(ConfigHelper.ConnectionString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update Roles set RoleName=@RoleName,IsAdministrator=@IsAdministrator " +
                                  "where RoleId=@RoleId;";
                cmd.Parameters.Add("@RoleName", SqlDbType.VarChar).Value = roleName;
                cmd.Parameters.Add("@IsAdministrator", SqlDbType.Bit).Value = isAdmin;
                cmd.Parameters.Add("@RoleId", SqlDbType.Int).Value = roleId;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            HttpContext.Current.Cache.Remove(ALL_ROLES_CACHEKEY);
        }

        public static void DeleteRole(int roleId)
        {
            using (var conn = new SqlConnection(ConfigHelper.ConnectionString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "delete from  Roles where RoleId=@RoleId;";
                cmd.Parameters.Add("@RoleId", SqlDbType.Int).Value = roleId;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            HttpContext.Current.Cache.Remove(ALL_ROLES_CACHEKEY);
        }

        public static bool RoleNameExists(string roleName)
        {
            var result = false;
            using (var conn = new SqlConnection(ConfigHelper.ConnectionString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select top 1 RoleName from Roles where RoleName=@RoleName";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@RoleName", SqlDbType.VarChar).Value = roleName;
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    result = reader.HasRows;
                }
            }
            return result;
        }

        public static void AddRole(string roleName, bool isAdmin)
        {
            using (var conn = new SqlConnection(ConfigHelper.ConnectionString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert into Roles(RoleName,IsAdministrator)" +
                                    " values(@RoleName,@IsAdministrator)";
                cmd.Parameters.Add("@RoleName", SqlDbType.VarChar).Value = roleName;
                cmd.Parameters.Add("@IsAdministrator", SqlDbType.Bit).Value = isAdmin;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            HttpContext.Current.Cache.Remove(ALL_ROLES_CACHEKEY);

        }

        public static List<int> GetRoleIdsForUser(string userName)
        {
            var result = new List<int>();
            using (var conn = new SqlConnection(ConfigHelper.ConnectionString))
            {
                var cmd = new SqlCommand("select FkRoleId,FkUserName from UserRoles where FkUserName=@FkUserName");
                var sda = new SqlDataAdapter();
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@FkUserName", SqlDbType.VarChar).Value = userName;
                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add((reader.GetInt32(0)));
                    }
                }
            }
            return result;
            
        }

        public static List<UserRole> GetUserRoles(string userName)
        {
            var result = new List<UserRole>();
            using (var conn = new SqlConnection(ConfigHelper.ConnectionString))
            {
                var cmd = new SqlCommand("GetUserRoles");
                var sda = new SqlDataAdapter();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FkUserName", SqlDbType.VarChar).Value = userName;
                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new UserRole
                                   {
                                       IsAdmin = reader.GetBoolean(3),
                                       RoleId = reader.GetInt32(0),
                                       RoleName = reader.GetString(2)
                                   });
                    }
                }
            }
            return result;

        }

        public static List<string> GetRolesWithUserNameLowerCase(string userName)
        {
            var result = new List<string>();
            result.Add(userName.ToLower());
            foreach (var userRole in GetUserRoles(userName))
            {
                result.Add(userRole.RoleName.ToLower());
            }
            return result;
        }

        private const string ALL_ROLES_CACHEKEY = "ALL_ROLES";
        private const string ADMIN_USERROLES_CACHEKEY = "ADMIN_USERROLES_CACHEKEY";
    }

}
