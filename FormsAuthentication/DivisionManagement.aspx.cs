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
    public class DivisionManagement : System.Web.UI.Page
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
                BindDataDivision(clientId);

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

        private void BindDataDivision(int clientId)
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
            gvDivision.DataSource = MonumentDal.GetDivisions(cList);
            gvDivision.DataBind();
        }

        protected void OnPagingDivision(object sender, GridViewPageEventArgs e)
        {
            var clientId = int.Parse(ddlClients.SelectedValue);
            BindDataDivision(clientId);
            gvDivision.PageIndex = e.NewPageIndex;
            gvDivision.DataBind();
        }

        protected void EditDivision(object sender, GridViewEditEventArgs e)
        {
            lblDivisionError.Text = string.Empty;
            gvDivision.EditIndex = e.NewEditIndex;
            var clientId = int.Parse(ddlClients.SelectedValue);
            BindDataDivision(clientId);
        }

        protected void UpdateDivision(object sender, GridViewUpdateEventArgs e)
        {
            lblDivisionError.Text = string.Empty;
            try
            {
                string buName = ((TextBox)gvDivision.Rows[e.RowIndex].FindControl("txtBuNameEdit")).Text;
                int divisionId = int.Parse(((HiddenField)gvDivision.Rows[e.RowIndex].FindControl("hdnDivisionIdEdit")).Value);

                if (string.IsNullOrEmpty(buName))
                {
                    lblDivisionError.Text = "BU name is requireed.";
                    return;
                }

                MonumentDal.UpdateDivisionBu(buName, divisionId);
                gvDivision.EditIndex = -1;
                var clientId = int.Parse(ddlClients.SelectedValue);
                BindDataDivision(clientId);
            }
            catch (Exception ex)
            {
                lblDivisionError.Text = "Error occured while updating division, please try again. or inform system administrator.<br/>Error details: " + ex.Message;
            }
        }

        protected void CancelEditDivision(object sender, GridViewCancelEditEventArgs e)
        {
            lblDivisionError.Text = string.Empty;
            gvDivision.EditIndex = -1;
            var clientId = int.Parse(ddlClients.SelectedValue);
            BindDataDivision(clientId);
        }

        protected void ddlClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            var clientId = int.Parse(ddlClients.SelectedValue);
            BindDataDivision(clientId);

        }


        /// <summary>
        /// GridView1 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected System.Web.UI.WebControls.Label lblError;
        protected System.Web.UI.WebControls.GridView gvDivision;
        protected System.Web.UI.WebControls.DropDownList ddlClients;
        protected System.Web.UI.WebControls.Label lblDivisionError;
    }
}
