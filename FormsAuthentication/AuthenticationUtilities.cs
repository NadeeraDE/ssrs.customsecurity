#region Copyright © Microsoft Corporation. All rights reserved.
/*============================================================================
   File:      AuthenticationStore.cs

  Summary:  Demonstrates how to create and maintain a user store for
            a security extension. 
--------------------------------------------------------------------
  This file is part of Microsoft SQL Server Code Samples.
    
 This source code is intended only as a supplement to Microsoft
 Development Tools and/or on-line documentation. See these other
 materials for detailed information regarding Microsoft code 
 samples.

 THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF 
 ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO 
 THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
 PARTICULAR PURPOSE.
===========================================================================*/
#endregion

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Management;
using System.Globalization;

namespace Microsoft.Samples.ReportingServices.CustomSecurity
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal sealed class AuthenticationUtilities
    {
        // The path of any item in the report server database 
        // has a maximum character length of 260
        private const int MaxItemPathLength = 260;
        private const string wmiNamespace = @"\root\Microsoft\SqlServer\ReportServer\{0}\v11";
        private const string rsAsmx = @"/ReportService2010.asmx";

        // Method used to create a cryptographic random number that is used to
        // salt the user's password for added security
        internal static string CreateSalt(int size)
        {
            // Generate a cryptographic random number using the cryptographic
            // service provider
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);
            // Return a Base64 string representation of the random number
            return Convert.ToBase64String(buff);
        }

        // Returns a hash of the combined password and salt value
        internal static string CreatePasswordHash(string pwd, string salt)
        {
            // Concat the raw password and salt value
            string saltAndPwd = String.Concat(pwd, salt);
            // Hash the salted password
            string hashedPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(
             saltAndPwd, "SHA1");

            return hashedPwd;
        }


        // Stores the account details in a SQL table named UserAccounts
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
        internal static void StoreAccountDetails(string userName,
           string passwordHash,
           string salt, string groupName, string email, List<int> roles )
        {
            // See "How To Use DPAPI (Machine Store) from ASP.NET" for 
            // information about securely storing connection strings.
            try
            {
                UserDbDal.AddNewUser(userName, passwordHash, salt, groupName, email, roles);
            }
            catch (Exception ex)
            {
                // Code to check for primary key violation (duplicate account 
                // name) or other database errors omitted for clarity
                throw new Exception(string.Format(CultureInfo.InvariantCulture,
                  CustomSecurity.AddAccountError + ex.Message));
            }
        }

        // Method that indicates whether 
        // the supplied username and password are valid
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
        internal static bool VerifyPassword(string suppliedUserName,
           string suppliedPassword)
        {
            bool passwordMatch = false;
            // Get the salt and pwd from the database based on the user name.
            // See "How To: Use DPAPI (Machine Store) from ASP.NET," "How To:
            // Use DPAPI (User Store) from Enterprise Services," and "How To:
            // Create a DPAPI Library" on MSDN for more information about 
            // how to use DPAPI to securely store connection strings.
            try
            {
                var user = UserDbDal.HasUser(suppliedUserName, string.Empty);
                if (user != null)
                {
                    string passwordAndSalt = String.Concat(suppliedPassword, user.Salt);
                    // Now hash them
                    string hashedPasswordAndSalt =
                    FormsAuthentication.HashPasswordForStoringInConfigFile(
                      passwordAndSalt, "SHA1");
                    // Now verify them. Returns true if they are equal
                    passwordMatch = hashedPasswordAndSalt.Equals(user.PasswordHash);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(CultureInfo.InvariantCulture,
                      CustomSecurity.VerifyUserException + ex.Message));
            }
            return passwordMatch;
        }

        // Method to verify that the user name does not contain any
        // illegal characters. If My Reports is enabled, illegal characters
        // will invalidate the paths created in the \Users folder. Usernames
        // should not contain the characters captured by this method.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Int32.ToString")]
        internal static bool ValidateUserName(string input)
        {
            Regex r = new Regex(@"\A(\..*)?[^\. ](.*[^ ])?\z(?<=\A[^/;\?:@&=\+\$,\\\*<>\|""]{0," +
               MaxItemPathLength.ToString() + @"}\z)",
               RegexOptions.Compiled | RegexOptions.ExplicitCapture);
            bool isValid = r.IsMatch(input);
            return isValid;
        }

        //Method to get the report server url using WMI
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        internal static string GetReportServerUrl(string machineName, string instanceName)
        {
            string reportServerVirtualDirectory = String.Empty;
            string fullWmiNamespace = @"\\" + machineName + string.Format(wmiNamespace, instanceName);

            ManagementScope scope = null;

            ConnectionOptions connOptions = new ConnectionOptions();
            connOptions.Authentication = AuthenticationLevel.PacketPrivacy;

            //Get management scope
            try
            {
                scope = new ManagementScope(fullWmiNamespace, connOptions);
                scope.Connect();

                //Get management class
                ManagementPath path = new ManagementPath("MSReportServer_Instance");
                ObjectGetOptions options = new ObjectGetOptions();
                ManagementClass serverClass = new ManagementClass(scope, path, options);

                serverClass.Get();

                if (serverClass == null)
                    throw new Exception(string.Format(CultureInfo.InvariantCulture,
                      CustomSecurity.WMIClassError));

                //Get instances
                ManagementObjectCollection instances = serverClass.GetInstances();

                foreach (ManagementObject instance in instances)
                {
                    instance.Get();

                    ManagementBaseObject outParams = (ManagementBaseObject)instance.InvokeMethod("GetReportServerUrls",
                       null, null);

                    string[] appNames = (string[])outParams["ApplicationName"];
                    string[] urls = (string[])outParams["URLs"];

                    for (int i = 0; i < appNames.Length; i++)
                    {
                        if (appNames[i] == "ReportServerWebService")
                            reportServerVirtualDirectory = urls[i];
                    }

                    if (reportServerVirtualDirectory == string.Empty)
                        throw new Exception(string.Format(CultureInfo.InvariantCulture,
                           CustomSecurity.MissingUrlReservation));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(CultureInfo.InvariantCulture,
                    CustomSecurity.RSUrlError + ex.Message), ex);
            }

            return reportServerVirtualDirectory + rsAsmx;
        }
    }
}
