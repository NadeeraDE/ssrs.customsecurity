#region Copyright © Microsoft Corporation. All rights reserved.
/*============================================================================
  File:     UILogon.aspx.cs
  Summary:  The code-behind for a logon page that supports Forms
            Authentication in a custom security extension    
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
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Net;
using System.Web.Services;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web.Security;
using System.Xml;
using Microsoft.SqlServer.ReportingServices2010;
using Microsoft.Samples.ReportingServices.CustomSecurity.App_LocalResources;
using System.Globalization;
using System.Net.Mail;

namespace Microsoft.Samples.ReportingServices.CustomSecurity
{
    /// <summary>
    /// Summary description for WebForm1.
    /// </summary>
    public class ResetPassword : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.Label LblUser;
        protected System.Web.UI.WebControls.TextBox txtCurrentPassword;
        protected System.Web.UI.WebControls.TextBox txtNewPassword1;
        protected System.Web.UI.WebControls.TextBox txtNewPassword2;
        protected System.Web.UI.WebControls.Button btnReset;
        protected System.Web.UI.WebControls.Label lblEmail;
        protected System.Web.UI.WebControls.Label Label1;
        protected System.Web.UI.WebControls.Label lblError;

        private void Page_Load(object sender, System.EventArgs e)
        {
            // Put user code to initialize the page here
            if (Request.QueryString["Logout"] != null)
            {
            }
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void btnReset_Click(object sender, System.EventArgs e)
        {
            lblError.Text = string.Empty;
            if (!txtNewPassword1.Text.Equals(txtNewPassword2.Text))
            {
                lblError.Text = "New password does not match.";
                return;
            }

            if (string.IsNullOrEmpty(txtCurrentPassword.Text))
            {
                lblError.Text = "Please enter current password.";
                return;
            }


            var password = txtNewPassword1.Text;
            var userName = (string)Session["UserName"];
            /*var cookie = Request.Cookies["ssrscookie"];
            var tU = cookie.Values["U"];
            if (!string.IsNullOrEmpty(tU))
            {
                var bU = Convert.FromBase64String(tU);
                userName = EncriptionHelper.Decrypt(System.Text.Encoding.UTF8.GetString(bU));
            }*/
            if (!string.IsNullOrEmpty(userName) &&
                AuthenticationUtilities.VerifyPassword(userName, txtCurrentPassword.Text))
            {
                try
                {
                    string salt = AuthenticationUtilities.CreateSalt(5);
                    string passwordHash = string.IsNullOrEmpty(password)
                        ? string.Empty
                        : AuthenticationUtilities.CreatePasswordHash(password, salt);
                    UserDbDal.ResetPassword(userName, passwordHash, salt);
                    Response.Redirect(string.Format("/{0}/pages/folder.aspx", ConfigHelper.ReportManagerRootName));
                }
                catch (Exception ex)
                {
                    lblError.Text =
                        "Error occured while resetting password, please try again. or inform system administrator.<br/>Error details: " + ex.Message;
                }
            }
            else
            {
                lblError.Text = "Current password is wrong, please enter the correct password.";
            }

        }

    }
}
