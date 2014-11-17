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
    public class AwardStatusManagement : System.Web.UI.Page
    {

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!SessionHelper.IsAdminUser)
                {
                    Response.Redirect(string.Format("/{0}/pages/folder.aspx", ConfigHelper.ReportManagerRootName));
                }
                BindDataAwardStatus();

               
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


        
        private void BindDataAwardStatus()
        {

            gvAwardStatus.DataSource = MonumentDal.GetAwardStatus();
            gvAwardStatus.DataBind();
        }
        
        protected void OnPagingAwardStatus(object sender, GridViewPageEventArgs e)
        {
            BindDataAwardStatus();
            gvAwardStatus.PageIndex = e.NewPageIndex;
            gvAwardStatus.DataBind();
        }

        protected void EditAwardStatus(object sender, GridViewEditEventArgs e)
        {
            lblAwardStatusError.Text = string.Empty;
            gvAwardStatus.EditIndex = e.NewEditIndex;
         //   var clientId = int.Parse(ddlClients.SelectedValue);
            BindDataAwardStatus();
        }

        protected void UpdateAwardStatus(object sender, GridViewUpdateEventArgs e)
        {
            lblAwardStatusError.Text = string.Empty;
            try
            {
                int onceEngaged = int.Parse(((DropDownList)gvAwardStatus.Rows[e.RowIndex].FindControl("ddOnceEngaged")).SelectedValue);
                int positive = int.Parse(((DropDownList)gvAwardStatus.Rows[e.RowIndex].FindControl("ddPositive")).SelectedValue);
                int Awardstatusid = int.Parse(((HiddenField)gvAwardStatus.Rows[e.RowIndex].FindControl("hdnAwardStatusIdEdit")).Value);

                MonumentDal.UpdateAwardStatus(onceEngaged, positive, Awardstatusid);
                gvAwardStatus.EditIndex = -1;
                BindDataAwardStatus();
                 }
            catch (Exception ex)
            {
                lblAwardStatusError.Text = "Error occured while updating Award status, please try again. or inform system administrator.<br/>Error details: " + ex.Message;
            }
        }

        protected void CancelEditAwardStatus(object sender, GridViewCancelEditEventArgs e)
        {
            lblAwardStatusError.Text = string.Empty;
            gvAwardStatus.EditIndex = -1;
          //  var clientId = int.Parse(ddlClients.SelectedValue);
            BindDataAwardStatus();

        }

     
        

        /// <summary>
        /// GridView1 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected System.Web.UI.WebControls.Label lblError;
        protected System.Web.UI.WebControls.GridView gvAwardStatus;
     //   protected System.Web.UI.WebControls.DropDownList ddOnceEngaged;
     //   protected System.Web.UI.WebControls.DropDownList ddPositive;        
        protected System.Web.UI.WebControls.Label lblAwardStatusError;

    
    }
}
