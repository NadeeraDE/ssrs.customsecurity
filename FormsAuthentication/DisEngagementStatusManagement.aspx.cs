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
    public class DisEngagementStatusManagement : System.Web.UI.Page
    {

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!SessionHelper.IsAdminUser)
                {
                    Response.Redirect(string.Format("/{0}/pages/folder.aspx", ConfigHelper.ReportManagerRootName));
                }
                BindDataDisEngagementStatus();

               
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


        
        private void BindDataDisEngagementStatus()
        {

            gvDisEngagementStatus.DataSource = MonumentDal.GetDisEngagementStatus();
            gvDisEngagementStatus.DataBind();
        }
        
        protected void OnPagingDisEngagementStatus(object sender, GridViewPageEventArgs e)
        {
            BindDataDisEngagementStatus();
            gvDisEngagementStatus.PageIndex = e.NewPageIndex;
            gvDisEngagementStatus.DataBind();
        }

        protected void EditDisEngagementStatus(object sender, GridViewEditEventArgs e)
        {
            lblDisEngagementStatusError.Text = string.Empty;
            gvDisEngagementStatus.EditIndex = e.NewEditIndex;
         //   var clientId = int.Parse(ddlClients.SelectedValue);
            BindDataDisEngagementStatus();
        }

        protected void UpdateDisEngagementStatus(object sender, GridViewUpdateEventArgs e)
        {
            lblDisEngagementStatusError.Text = string.Empty;
            try
            {
                int positiveReason = int.Parse(((DropDownList)gvDisEngagementStatus.Rows[e.RowIndex].FindControl("ddPositiveReason")).SelectedValue);
                int DisEngagementstatusid = int.Parse(((HiddenField)gvDisEngagementStatus.Rows[e.RowIndex].FindControl("hdnDisEngagementStatusIdEdit")).Value);
                MonumentDal.UpdateDisEngagementStatus(DisEngagementstatusid, positiveReason);
                gvDisEngagementStatus.EditIndex = -1;
                BindDataDisEngagementStatus();
                 }
            catch (Exception ex)
            {
                lblDisEngagementStatusError.Text = "Error occured while updating Dis Engagement status, please try again. or inform system administrator.<br/>Error details: " + ex.Message;
            }
        }

        protected void CancelEditDisEngagementStatus(object sender, GridViewCancelEditEventArgs e)
        {
            lblDisEngagementStatusError.Text = string.Empty;
            gvDisEngagementStatus.EditIndex = -1;
          //  var clientId = int.Parse(ddlClients.SelectedValue);
            BindDataDisEngagementStatus();

        }

     
        

        /// <summary>
        /// GridView1 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected System.Web.UI.WebControls.Label lblError;
        protected System.Web.UI.WebControls.GridView gvDisEngagementStatus;
     //   protected System.Web.UI.WebControls.DropDownList ddlClients;
        protected System.Web.UI.WebControls.Label lblDisEngagementStatusError;

    
    }
}
