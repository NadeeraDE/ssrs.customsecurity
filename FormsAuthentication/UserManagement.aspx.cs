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
    public class UserManagement : System.Web.UI.Page
    {

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!SessionHelper.IsAdminUser)
                {
                    Response.Redirect(string.Format("/{0}/pages/folder.aspx", ConfigHelper.ReportManagerRootName));
                }
                BindData();
                BindDataRoles();
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

        private void BindData()
        {
            GridView1.DataSource = UserDbDal.GetAllUsers();
            GridView1.DataBind();
        }

        private void BindDataRoles()
        {
            gvRoles.DataSource = UserDbDal.GetAllRoles();
            gvRoles.DataBind();
        }

        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            BindData();
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataBind();
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    var roles = UserDbDal.GetAllRoles();
                    var ddList = (DropDownList)e.Row.FindControl("ddlGroupName");
                    //bind dropdownlist
                    ddList.DataTextField = "RoleName";
                    ddList.DataValueField = "RoleId";
                    ddList.DataSource = roles;
                    ddList.DataBind();

                    var dr = e.Row.DataItem as DataRowView;
                    //ddList.SelectedItem.Text = dr["category_name"].ToString();
                    ddList.SelectedValue = dr["GroupName"].ToString();


                    var chkGroupNameEdit = e.Row.FindControl("chkGroupNameEdit") as CheckBoxList;
                    chkGroupNameEdit.DataTextField = "RoleName";
                    chkGroupNameEdit.DataValueField = "RoleId";
                    chkGroupNameEdit.DataSource = roles;
                    chkGroupNameEdit.DataBind();

                    var userName = (e.Row.FindControl("lblUserNameEdit") as Label).Text;
                    var userRoles = UserDbDal.GetRoleIdsForUser(userName);

                    if (userRoles != null && userRoles.Count > 0)
                    {
                        foreach (ListItem item in chkGroupNameEdit.Items)
                        {
                            if (userRoles.Contains(int.Parse(item.Value)))
                            {
                                item.Selected = true;
                            }
                        } 
                    }
                }
            }
        }

        protected void OnDataBound(object sender, EventArgs e)
        {
            var roles = UserDbDal.GetAllRoles();
            var ddlCountries = GridView1.FooterRow.FindControl("ddlGroupNameNew") as DropDownList;
            ddlCountries.DataTextField = "RoleName";
            ddlCountries.DataValueField = "RoleId";
            ddlCountries.DataSource = roles;
            ddlCountries.DataBind();

            var chkGroupNameNew = GridView1.FooterRow.FindControl("chkGroupNameNew") as CheckBoxList;
            chkGroupNameNew.DataTextField = "RoleName";
            chkGroupNameNew.DataValueField = "RoleId";
            chkGroupNameNew.DataSource = roles;
            chkGroupNameNew.DataBind();
        }

        protected void AddNewUser(object sender, EventArgs e)
        {
            lblError.InnerText = string.Empty;
            lblError.Visible = false;
            try
            {
                string userName = ((TextBox)GridView1.FooterRow.FindControl("txtUserNameNew")).Text;
                string groupName = ((DropDownList)GridView1.FooterRow.FindControl("ddlGroupNameNew")).SelectedValue;
                string password = ((TextBox)GridView1.FooterRow.FindControl("txtPasswordNew")).Text;
                string email = ((TextBox)GridView1.FooterRow.FindControl("txtEmailNew")).Text;

                if (string.IsNullOrEmpty(userName))
                {
                    lblError.InnerText = "Username is required.";
                    lblError.Visible = true;
                    return;
                }
                if (string.IsNullOrEmpty(groupName))
                {
                    lblError.InnerText = "Group name is required.";
                    lblError.Visible = true;
                    return;
                }
                if (string.IsNullOrEmpty(password))
                {
                    lblError.InnerText = "Password is required.";
                    lblError.Visible = true;
                    return;
                }
                if (string.IsNullOrEmpty(email))
                {
                    lblError.InnerText = "Email is required.";
                    lblError.Visible = true;
                    return;
                }
                if (!AuthenticationUtilities.ValidateUserName(userName))
                {
                    lblError.InnerText = "Username is not valid. Please enter a valid username.";
                    lblError.Visible = true;
                    return;
                }

                var roles = new List<int>();
                var chkGroupNameNew = ((CheckBoxList)GridView1.FooterRow.FindControl("chkGroupNameNew"));
                foreach (ListItem item in chkGroupNameNew.Items)
                {
                    if (item.Selected)
                    {
                        roles.Add(int.Parse(item.Value));
                    }
                }


                if (roles.Count == 0)
                {
                    lblError.InnerText = "Role name is required.";
                    lblError.Visible = true;
                    return;
                }

                string salt = AuthenticationUtilities.CreateSalt(5);
                string passwordHash =
                   AuthenticationUtilities.CreatePasswordHash(password, salt);
                if (!UserDbDal.UserNameExists(userName))
                {
                    AuthenticationUtilities.StoreAccountDetails(
                      userName, passwordHash, salt, groupName, email,roles);
                    GridView1.DataSource = UserDbDal.GetAllUsers();
                    GridView1.DataBind();
                }
                else
                {
                    lblError.InnerText = "Username already used by another user, please change the username and retry.";
                    lblError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblError.InnerText = "Error occured while adding new user, please try again. or inform system administrator.<br/>Error details: " + ex.Message;
                    lblError.Visible = true;
            }
        }

        protected void DeleteUser(object sender, EventArgs e)
        {
            lblError.InnerText = string.Empty;
            lblError.Visible = true;
            try
            {
                var lnkRemove = (LinkButton)sender;
                UserDbDal.DeleteUser(lnkRemove.CommandArgument);
                BindData();
            }
            catch (Exception ex)
            {
                lblError.InnerText = "Error occured while deleting user, please try again. or inform system administrator. <br/>Error details: " + ex.Message;
                lblError.Visible = true;
            }
        }
        protected void EditUser(object sender, GridViewEditEventArgs e)
        {
            lblError.InnerText = string.Empty;
            lblError.Visible = false;
            GridView1.EditIndex = e.NewEditIndex;
            BindData();
        }
        protected void CancelEdit(object sender, GridViewCancelEditEventArgs e)
        {
            lblError.InnerText = string.Empty;
            lblError.Visible = false;
            GridView1.EditIndex = -1;
            BindData();
        }
        protected void UpdateUser(object sender, GridViewUpdateEventArgs e)
        {
            lblError.InnerText = string.Empty;
            lblError.Visible = false;
            try
            {
                string userName = ((Label)GridView1.Rows[e.RowIndex].FindControl("lblUserNameEdit")).Text;
                string groupName = ((DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlGroupName")).SelectedValue;
                string password = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("txtPassword")).Text;
                string email = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("txtEmail")).Text;

                var roles = new List<int>();
                var chkGroupNameEdit = ((CheckBoxList)GridView1.Rows[e.RowIndex].FindControl("chkGroupNameEdit"));
                foreach (ListItem item in chkGroupNameEdit.Items)
                {
                    if (item.Selected)
                    {
                        roles.Add(int.Parse(item.Value));
                    }
                }


                if (roles.Count == 0)
                {
                    lblError.InnerText = "Role name is required.";
                    lblError.Visible = true;
                    return;
                }
                if (string.IsNullOrEmpty(password))
                {
                    lblError.InnerText = "Password is required.";
                    lblError.Visible = true;
                    return;
                }
                if (string.IsNullOrEmpty(email))
                {
                    lblError.InnerText = "Email is required.";
                    lblError.Visible = true;
                    return;
                }

                string salt = AuthenticationUtilities.CreateSalt(5);
                string passwordHash = string.IsNullOrEmpty(password) ? string.Empty :
                   AuthenticationUtilities.CreatePasswordHash(password, salt);
                UserDbDal.UpdateUser(userName, groupName, password, passwordHash, salt, email, roles);
                GridView1.EditIndex = -1;
                BindData();
            }
            catch (Exception ex)
            {
                lblError.InnerText = "Error occured while updating user, please try again. or inform system administrator.<br/>Error details: " + ex.Message;
                lblError.Visible = true;
            }
        }

        protected void OnPagingRoles(object sender, GridViewPageEventArgs e)
        {
            BindDataRoles();
            gvRoles.PageIndex = e.NewPageIndex;
            gvRoles.DataBind();
        }

        protected void EditRole(object sender, GridViewEditEventArgs e)
        {
            lblRoleError.InnerText = string.Empty;
            lblRoleError.Visible = false;
            gvRoles.EditIndex = e.NewEditIndex;
            BindDataRoles();
        }

        protected void UpdateRole(object sender, GridViewUpdateEventArgs e)
        {
            lblRoleError.InnerText = string.Empty;
            lblRoleError.Visible = false;
            try
            {
                string roleName = ((TextBox)gvRoles.Rows[e.RowIndex].FindControl("txtRoleNameEdit")).Text;
                bool isAdmin = ((CheckBox) gvRoles.Rows[e.RowIndex].FindControl("chkIsAdminEdit")).Checked;
                int roleId = int.Parse(((HiddenField)gvRoles.Rows[e.RowIndex].FindControl("hdnRoleIdEdit")).Value);

                if (string.IsNullOrEmpty(roleName))
                {
                    lblRoleError.InnerText = "Role name is required.";
                    lblRoleError.Visible = true;
                    return;
                }

                UserDbDal.UpdateRole(roleId,roleName,isAdmin);
                gvRoles.EditIndex = -1;
                BindDataRoles();
                BindData();
            }
            catch (Exception ex)
            {
                lblRoleError.InnerText = "Error occured while updating role, please try again. or inform system administrator.<br/>Error details: " + ex.Message;
                lblRoleError.Visible = true;
            }
        }

        protected void CancelEditRole(object sender, GridViewCancelEditEventArgs e)
        {
            lblRoleError.InnerText = string.Empty;
            lblRoleError.Visible = false;
            gvRoles.EditIndex = -1;
            BindDataRoles();
        }

        protected void DeleteRole(object sender, EventArgs e)
        {
            lblRoleError.InnerText = string.Empty;
            lblRoleError.Visible = false;
            try
            {
                var lnkRemove = (LinkButton)sender;
                UserDbDal.DeleteRole(int.Parse(lnkRemove.CommandArgument));
                BindDataRoles();
                BindData();
            }
            catch (Exception ex)
            {
                lblRoleError.InnerText = "Error occured while deleting user, please try again. or inform system administrator. <br/>Error details: " + ex;
                lblRoleError.Visible = true;
            }
        }

        protected void AddNewRole(object sender, EventArgs e)
        {
            lblRoleError.InnerText = string.Empty;
            lblRoleError.Visible = false;
            try
            {
                string roleName = ((TextBox)gvRoles.FooterRow.FindControl("txtRoleNameNew")).Text;
                bool isAdmin = ((CheckBox)gvRoles.FooterRow.FindControl("chkIsAdminNew")).Checked;

                if (string.IsNullOrEmpty(roleName))
                {
                    lblRoleError.InnerText = "Role name is required.";
                    lblRoleError.Visible = true;
                    return;
                }

                if (!UserDbDal.RoleNameExists(roleName))
                {
                    UserDbDal.AddRole(roleName,isAdmin);
                    BindDataRoles();
                    BindData();
                }
                else
                {
                    lblRoleError.InnerText = "Username already used by another user, please change the username and retry.";
                    lblRoleError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblRoleError.InnerText = "Error occured while adding new user, please try again. or inform system administrator.<br/>Error details: " + ex.Message;
                lblRoleError.Visible = true;
            }
        }


        /// <summary>
        /// GridView1 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected System.Web.UI.WebControls.GridView GridView1;
        protected System.Web.UI.HtmlControls.HtmlGenericControl lblError;
        protected System.Web.UI.WebControls.GridView gvRoles;
        protected System.Web.UI.HtmlControls.HtmlGenericControl lblRoleError;
    }
}
