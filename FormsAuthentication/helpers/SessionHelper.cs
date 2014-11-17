using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Microsoft.Samples.ReportingServices.CustomSecurity.helpers
{
    public static class SessionHelper
    {
        public static bool IsAdminUser {
            get { return (bool) HttpContext.Current.Session["IsAdminUser"]; }
            set { HttpContext.Current.Session["IsAdminUser"] = value; }
        }

        public static List<UserRole> UserRoles {
            get { return (List<UserRole>) HttpContext.Current.Session["UserRoles"]; }
            set { HttpContext.Current.Session["UserRoles"] = value; }
        }

        public static string UserName
        {
            get
            {
                return (string)HttpContext.Current.Session["UserName"];
            }
            set { HttpContext.Current.Session["UserName"] = value; }
        }

        public static void LogOut()
        {
            UserName = string.Empty;
            IsAdminUser = false;
            UserRoles = new List<UserRole>();
        }
    }

    public class UserRole
    {
        public string RoleName { get; set; }
        public bool IsAdmin { get; set; }
        public int RoleId { get; set; }
    }
}
