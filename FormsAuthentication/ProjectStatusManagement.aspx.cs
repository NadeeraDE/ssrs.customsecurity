#region Copyright © Microsoft Corporation. All rights reserved.
/*============================================================================
  File:     Logon.aspx.cs
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
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web.Security;
using Microsoft.ReportingServices.Interfaces;
using Microsoft.Samples.ReportingServices.CustomSecurity.App_LocalResources;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.Samples.ReportingServices.CustomSecurity.helpers;

namespace Microsoft.Samples.ReportingServices.CustomSecurity
{
    public class ProjectStatusManagement : System.Web.UI.Page
    {

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!SessionHelper.IsAdminUser)
                {
                    Response.Redirect(string.Format("/{0}/pages/folder.aspx", ConfigHelper.ReportManagerRootName));
                }
                BindDataProjectStatus();

               
            }

        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            //this.BtnLogon.Click += new System.EventHandler(this.ServerBtnLogon_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion


        
        private void BindDataProjectStatus()
        {

            gvProjectStatus.DataSource = MonumentDal.GetProjectStatus();
            gvProjectStatus.DataBind();
        }
        
        protected void OnPagingProjectStatus(object sender, GridViewPageEventArgs e)
        {
            BindDataProjectStatus();
            gvProjectStatus.PageIndex = e.NewPageIndex;
            gvProjectStatus.DataBind();
        }

        protected void EditProjectStatus(object sender, GridViewEditEventArgs e)
        {
            lblProjectStatusError.Text = string.Empty;
            gvProjectStatus.EditIndex = e.NewEditIndex;
         //   var clientId = int.Parse(ddlClients.SelectedValue);
            BindDataProjectStatus();
        }

        protected void UpdateProjectStatus(object sender, GridViewUpdateEventArgs e)
        {
            lblProjectStatusError.Text = string.Empty;
            try
            {
                string approved = ((DropDownList)gvProjectStatus.Rows[e.RowIndex].FindControl("ddApproved")).SelectedValue;
                int Projectstatusid = int.Parse(((HiddenField)gvProjectStatus.Rows[e.RowIndex].FindControl("hdnProjectStatusIdEdit")).Value);
                MonumentDal.UpdateProjectStatus(approved, Projectstatusid);
                gvProjectStatus.EditIndex = -1;
                BindDataProjectStatus();
                 }
            catch (Exception ex)
            {
                lblProjectStatusError.Text = "Error occured while updating Project status, please try again. or inform system administrator.<br/>Error details: " + ex.Message;
            }
        }

        protected void CancelEditProjectStatus(object sender, GridViewCancelEditEventArgs e)
        {
            lblProjectStatusError.Text = string.Empty;
            gvProjectStatus.EditIndex = -1;
          //  var clientId = int.Parse(ddlClients.SelectedValue);
            BindDataProjectStatus();

        }

     
        

        /// <summary>
        /// GridView1 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected System.Web.UI.WebControls.Label lblError;
        protected System.Web.UI.WebControls.GridView gvProjectStatus;
        protected System.Web.UI.WebControls.DropDownList ddlClients;
        protected System.Web.UI.WebControls.Label lblProjectStatusError;

    
    }
}
