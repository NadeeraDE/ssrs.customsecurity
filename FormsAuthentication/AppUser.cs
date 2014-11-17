using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Samples.ReportingServices.CustomSecurity
{
    public class AppUser
    {
        public string UserName { get; set; }
        public string GroupName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
    }
}
