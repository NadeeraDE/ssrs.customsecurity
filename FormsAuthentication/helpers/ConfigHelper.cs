using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Configuration;
using System.Xml;

namespace Microsoft.Samples.ReportingServices.CustomSecurity
{
    public static class ConfigHelper
    {
        private static string _smtpHost = null;
        private static string _smtpUserName = null;
        private static string _smtpPassword = null;
        private static int? _smtpPort = null;
        private static bool? _smtpEnableSsl = null;
        private static string _connectionString = null;
        private static string _monumentConnectionString = null;
        private static string _reportManagerRootName = null;
        private static string _reportServiceUrl = null;
        private static List<string> _groupNames = null;

        public static void Initiatize(string configuration)
        {
            if (!string.IsNullOrEmpty(configuration))
            {
                var doc = new XmlDocument();
                doc.LoadXml(configuration);
                if (doc.DocumentElement.Name == "AdminConfiguration")
                {
                    foreach (XmlNode child in doc.DocumentElement.ChildNodes)
                    {
                        if (child.Name.Equals("SmtpHost",StringComparison.CurrentCultureIgnoreCase))
                        {
                            SmtpHost = child.InnerText;
                        }
                        else if (child.Name.Equals("SmtpUserName", StringComparison.CurrentCultureIgnoreCase))
                        {
                            SmtpUserName = child.InnerText;
                        }
                        else if (child.Name.Equals("SmtpPassword", StringComparison.CurrentCultureIgnoreCase))
                        {
                            SmtpPassword = child.InnerText;
                        }
                        else if (child.Name.Equals("SmtpPort", StringComparison.CurrentCultureIgnoreCase))
                        {
                            SmtpPort = int.Parse(child.InnerText);
                        }
                        else if (child.Name.Equals("SmtpEnableSsl", StringComparison.CurrentCultureIgnoreCase))
                        {
                            SmtpEnableSsl = bool.Parse(child.InnerText);
                        }
                        else if (child.Name.Equals("UserDbConnectionString", StringComparison.CurrentCultureIgnoreCase))
                        {
                            ConnectionString = child.InnerText;
                        }
                        else if (child.Name.Equals("ReportManagerRootName", StringComparison.CurrentCultureIgnoreCase))
                        {
                            ReportManagerRootName = child.InnerText;
                        }
                        else if (child.Name.Equals("GroupNames", StringComparison.CurrentCultureIgnoreCase))
                        {

                            var result = new List<string>();
                            foreach (var groupName in child.InnerText.Split(','))
                            {
                                result.Add(groupName);
                            }
                            _groupNames = result;
                            GroupNames = _groupNames;
                        }

                    }
                } 
            }
        }

        public static string SmtpHost
        {
            get
            {//smtp.gmail.com
                if (string.IsNullOrEmpty(_smtpHost))
                    _smtpHost = WebConfigurationManager.AppSettings["SmtpHost"];

                return _smtpHost;
            }
            set { _smtpHost = value; }
        }

        public static string SmtpUserName
        {
            get
            {//nadeera.ekanayake@rtslabs.com
                if (string.IsNullOrEmpty(_smtpUserName))
                _smtpUserName = WebConfigurationManager.AppSettings["SmtpUserName"];

                return _smtpUserName;
            }
            set { _smtpUserName = value; }
        }

        public static string SmtpPassword
        {
            get
            {
                if (string.IsNullOrEmpty(_smtpPassword))
                _smtpPassword = WebConfigurationManager.AppSettings["SmtpPassword"];

                return _smtpPassword;
            }
            set { _smtpPassword = value; }
        }

        public static int SmtpPort
        {
            get
            {
                if (!_smtpPort.HasValue)
                _smtpPort = Convert.ToInt32(WebConfigurationManager.AppSettings["SmtpPort"] ?? "25");

                return _smtpPort.Value;
            }
            set { _smtpPort = value; }
        }

        public static bool SmtpEnableSsl
        {
            get
            {
                if (!_smtpEnableSsl.HasValue)
                _smtpEnableSsl = Convert.ToBoolean(WebConfigurationManager.AppSettings["SmtpEnableSsl"] ?? "false");

                return _smtpEnableSsl.Value;
            }
            set { _smtpEnableSsl = value; }
        }

        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                _connectionString= WebConfigurationManager.AppSettings["UserDbConnectionString"];

                return _connectionString;
            }
            set { _connectionString = value; }
        }

        public static string MonumentConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_monumentConnectionString))
                    _monumentConnectionString = WebConfigurationManager.AppSettings["MonumentDbConnectionString"];

                return _monumentConnectionString;
            }
            set { _monumentConnectionString = value; }
        }

        public static string ReportManagerRootName
        {
            get
            {//reports_sqlnadeera
                if (string.IsNullOrEmpty(_reportManagerRootName))
                _reportManagerRootName = WebConfigurationManager.AppSettings["ReportManagerRootName"];

                return _reportManagerRootName;
            }
            set { _reportManagerRootName = value; }
        }

        public static string ReportServiceUrl
        {
            get
            {//http://rtslabs22/ReportServer_SQLNADEERA/ReportService2010.asmx

                if (string.IsNullOrEmpty(_reportServiceUrl))
                return WebConfigurationManager.AppSettings["ReportServiceUrl"];

                return _reportServiceUrl;
            }
            set { _reportServiceUrl = value; }
        }

        public static List<string> GroupNames
        {
            get
            {//
                if (_groupNames == null)
                {
                    var result = new List<string>();
                    foreach (var groupName in (WebConfigurationManager.AppSettings["GroupNames"] ?? string.Empty).Split(','))
                    {
                        result.Add(groupName);
                    }
                    _groupNames = result; 
                }

                return _groupNames;
            }
            set { _groupNames = value; }
        }
    }
}
