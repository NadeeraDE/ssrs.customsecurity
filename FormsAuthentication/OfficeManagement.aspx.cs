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
    public class OfficeManagement : System.Web.UI.Page
    {

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!SessionHelper.IsAdminUser)
                {
                    Response.Redirect(string.Format("/{0}/pages/folder.aspx", ConfigHelper.ReportManagerRootName));
                }
                BindClients();
                var clientId = int.Parse(ddlClients.SelectedValue);
                BindDataOffice(clientId);

               
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


        private void BindClients()
        {
            ddlClients.DataSource = MonumentDal.GetAllClientNames();
            ddlClients.DataBind();
        }
        
        private void BindDataOffice(int clientId)
        {
            var cList = new List<int>();
            if (clientId == -1)
            {
                foreach (var item in ddlClients.Items)
                {
                    var lstItem = (ListItem)item;
                    if (lstItem.Value != "-1")
                        cList.Add(int.Parse(lstItem.Value));
                }
            }
            else
            {
                cList.Add(clientId);
            }

            gvOffice.DataSource = MonumentDal.GetOfficeLocations(cList);
            gvOffice.DataBind();
        }
        
        protected void OnPagingOffice(object sender, GridViewPageEventArgs e)
        {
            var clientId = int.Parse(ddlClients.SelectedValue);
            BindDataOffice(clientId);
            gvOffice.PageIndex = e.NewPageIndex;
            gvOffice.DataBind();
        }

        protected void EditOffice(object sender, GridViewEditEventArgs e)
        {
            lblOfficeLocationError.Text = string.Empty;
            gvOffice.EditIndex = e.NewEditIndex;
            var clientId = int.Parse(ddlClients.SelectedValue);
            BindDataOffice(clientId);
        }

        protected void UpdateOffice(object sender, GridViewUpdateEventArgs e)
        {
            lblOfficeLocationError.Text = string.Empty;
            try
            {
                string newOfficeLocation = ((TextBox)gvOffice.Rows[e.RowIndex].FindControl("txtNewOfficeLocationEdit")).Text;
                int officeLocationId = int.Parse(((HiddenField)gvOffice.Rows[e.RowIndex].FindControl("hdnOfficeLocationIdEdit")).Value);

                if (string.IsNullOrEmpty(newOfficeLocation))
                {
                    lblOfficeLocationError.Text = "New Office Location is requireed.";
                    return;
                }

                MonumentDal.UpdateOfficeLocation(newOfficeLocation, officeLocationId);
                gvOffice.EditIndex = -1;
                var clientId = int.Parse(ddlClients.SelectedValue);
                BindDataOffice(clientId);
            }
            catch (Exception ex)
            {
                lblOfficeLocationError.Text = "Error occured while updating office location, please try again. or inform system administrator.<br/>Error details: " + ex.Message;
            }
        }

        protected void CancelEditOffice(object sender, GridViewCancelEditEventArgs e)
        {
            lblOfficeLocationError.Text = string.Empty;
            gvOffice.EditIndex = -1;
            var clientId = int.Parse(ddlClients.SelectedValue);
            BindDataOffice(clientId);

        }

        protected void ddlClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            var clientId = int.Parse(ddlClients.SelectedValue);
            BindDataOffice(clientId);


        }
        

        /// <summary>
        /// GridView1 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected System.Web.UI.WebControls.Label lblError;
        protected System.Web.UI.WebControls.GridView gvOffice;
        protected System.Web.UI.WebControls.DropDownList ddlClients;
        protected System.Web.UI.WebControls.Label lblOfficeLocationError;

    
    }
}
