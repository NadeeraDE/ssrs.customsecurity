<%@ Page Language="c#" CodeBehind="UserManagement.aspx.cs" AutoEventWireup="true" Inherits="Microsoft.Samples.ReportingServices.CustomSecurity.UserManagement, Microsoft.Samples.ReportingServices.CustomSecurity" Culture="auto" UICulture="auto" %>

<%@ Import Namespace="Microsoft.Samples.ReportingServices.CustomSecurity" %>
<%@ Import Namespace="Microsoft.Samples.ReportingServices.CustomSecurity.helpers" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>User Management</title>
    <link href="/<%=ConfigHelper.ReportManagerRootName %>/styles/ReportingServices.css" type="text/css" rel="stylesheet">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.1/css/bootstrap.min.css">
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <table style="width: 100%;">
            <tbody>
                <tr>
                    <td valign="top">
                        <div>
                            <table class="msrs-topBreadcrumb" cellpadding="0" cellspacing="0" border="0" width="100%">
                                <tbody>
                                    <tr>
                                        <td><span>
                                            <a href="<%=string.Format("/{0}/pages/folder.aspx",ConfigHelper.ReportManagerRootName) %>">Home</a>
                                        </span></td>
                                        <td align="right">
                                            <span>
                                                <a href="/<%=ConfigHelper.ReportManagerRootName %>/Pages/Folder.aspx">Home</a>&nbsp;| 
                                                <a href="/<%=ConfigHelper.ReportManagerRootName %>/Pages/Subscriptions.aspx">My&nbsp;Subscriptions</a>&nbsp;| 
                                                <a href="/<%=ConfigHelper.ReportManagerRootName %>/Pages/Settings.aspx">Site&nbsp;Settings</a>&nbsp;| 
                                                <a href="/<%=ConfigHelper.ReportManagerRootName %>/pages/resetpassword.aspx">Reset Password</a>&nbsp;| 
                                                <a href="/<%=ConfigHelper.ReportManagerRootName %>/pages/uilogon.aspx?Logout=1">Logout</a>
                                                <%if (SessionHelper.IsAdminUser)
                                                  { %>
                                                &nbsp;| 
                                                <a href="/<%=ConfigHelper.ReportManagerRootName %>/pages/UserManagement.aspx">User Management</a>
                                                &nbsp;| 
                                                <a href="/<%=ConfigHelper.ReportManagerRootName %>/pages/RefManagement.aspx">Ref Management</a>
                                                <%} %>
                                            </span>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <table class="msrs-header" cellpadding="0" cellspacing="0" border="0" width="100%">
                                <tbody>
                                    <tr>
                                        <td class="msrs-logo" width="36">
                                            <img src="http://www.monumentconsulting.com/site/wp-content/themes/monument/images/logo.png" alt="Settings" style="height: 50px; width: 200px; border-width: 0px; display: inline;"></td>
                                        <td>
                                            <p class="msrs-site_title">MONUMENT Reporting Server</p>
                                            <p class="msrs-page_title">User Management</p>
                                        </td>
                                        <td class="msrs-searchContainer" align="right" valign="bottom"></td>
                                    </tr>
                                </tbody>
                            </table>

                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" class="msrs-normal" cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr class="msrs-toolbar_top" height="6">
                                    <td valign="top"></td>
                                </tr>
                                <tr class="msrs-tool">
                                    <td valign="top">
                                        <table width="100%" cellpadding="0" cellspacing="0">
                                            <tbody>
                                                <tr>
                                                    <td valign="top" width="5"></td>
                                                    <td valign="top" width="3"></td>
                                                    <td width="100%"></td>
                                                    <td valign="top">&nbsp;</td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                                <tr class="msrs-toolbar_bottom" height="6">
                                    <td valign="top"></td>
                                </tr>
                                <tr>
                                    <td valign="top"></td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <h3 style="padding-left: 10px;">Roles</h3>
                        <div>
                            <div Visible="False" Style="width: 50%" ID="lblRoleError" runat="server" class="alert alert-danger">
                            </div>
                        </div>
                        <div id="dvGridRole" style="padding: 10px; width: 50%;">
                            <asp:GridView ID="gvRoles" runat="server" Width="100%" CssClass="table" UseAccessibleHeader="True" GridLines="None"
                                AutoGenerateColumns="false"  AllowPaging="true" ShowFooter="true"
                                OnPageIndexChanging="OnPaging" OnRowEditing="EditRole"
                                OnRowUpdating="UpdateRole" OnRowCancelingEdit="CancelEditRole"
                                PageSize="10">
                                <Columns>
                                    <asp:TemplateField  HeaderText="Role Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRoleName" runat="server"
                                                Text='<%# Eval("RoleName")%>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtRoleNameEdit" runat="server"
                                                Text='<%# Eval("RoleName")%>'></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdnRoleIdEdit" Value='<%# Eval("RoleId")%>'/>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtRoleNameNew"
                                                MaxLength="50" runat="server"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField  HeaderText="Is Administrator">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkIsAdmin" Enabled="False" Checked='<%# Eval("IsAdministrator")%>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkIsAdminEdit"  Checked='<%# Eval("IsAdministrator")%>'/>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:CheckBox runat="server" ID="chkIsAdminNew"/>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField  HeaderText="Actions">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="imgEdit" ToolTip="Edit" CssClass="glyphicon glyphicon-edit"
                                                        CommandName="Edit" AlternateText="Edit"
                                            ImageUrl="/reports_monumentbi/images/edit_item.gif" />
                                            <asp:LinkButton runat="server"  ID="imgDelete" ToolTip="Delete" CssClass="glyphicon glyphicon-remove"
                                                OnClientClick="return confirm('Do you want to delete?')"
                                                CommandArgument='<%# Eval("RoleId")%>' OnClick="DeleteRole"></asp:LinkButton>
                                     </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton runat="server" ID="imgUpdate" ToolTip="Update" CssClass="glyphicon glyphicon-floppy-save"
                                                        CommandName="Update" AlternateText="Update"
                                            ImageUrl="/reports_monumentbi/images/save_item.png" />
                                            <asp:LinkButton runat="server" ID="imgCancel" ToolTip="Cancel" CssClass="glyphicon glyphicon-floppy-remove"
                                                        CommandName="Cancel" AlternateText="Cancel"
                                            ImageUrl="/reports_monumentbi/images/cancel_item.png" />

                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:Button ID="btnAdd" runat="server" Text="Add"
                                                OnClick="AddNewRole" CssClass="btn btn-primary btn-small" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <AlternatingRowStyle BackColor="#EBF3FF" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <h3 style="padding-left: 10px;">Users</h3>
                        <div>
                            <div Visible="False" Style="width: 50%" ID="lblError" runat="server" class="alert alert-danger">
                            </div>
                        </div>
                        <div id="dvGrid" style="padding: 10px; width: 60%;">
                            <asp:GridView ID="GridView1" runat="server" Width="100%" CssClass="table" UseAccessibleHeader="True" GridLines="None"
                                AutoGenerateColumns="false"  OnRowDataBound="OnRowDataBound"  OnDataBound="OnDataBound" AllowPaging="true" ShowFooter="true"
                                OnPageIndexChanging="OnPaging" OnRowEditing="EditUser"
                                OnRowUpdating="UpdateUser" OnRowCancelingEdit="CancelEdit"
                                PageSize="10">
                                <Columns>
                                    <asp:TemplateField  HeaderText="UserName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUserName" runat="server"
                                                Text='<%# Eval("UserName")%>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblUserNameEdit" runat="server"
                                                Text='<%# Eval("UserName")%>'></asp:Label>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtUserNameNew"
                                                MaxLength="50" runat="server"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField  HeaderText="Password">
                                        <ItemTemplate>
                                            <span>******</span>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"  Width="70px" MaxLength="15"
                                                Text=''></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox Width="70px" TextMode="Password" MaxLength="15" ID="txtPasswordNew" runat="server"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField  HeaderText="Group">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGroupName" runat="server"
                                                Text='<%# Eval("RoleNames")%>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList Visible="False" Width="100px" ID="ddlGroupName" runat="server" ></asp:DropDownList>
                                            <asp:CheckBoxList CssClass="tblchkboxlst"  runat="server" ID="chkGroupNameEdit"/>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList Visible="False" Width="100px" ID="ddlGroupNameNew" runat="server" ></asp:DropDownList>
                                            <asp:CheckBoxList CssClass="tblchkboxlst" runat="server" ID="chkGroupNameNew"/>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField  HeaderText="Email">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmail" runat="server"
                                                Text='<%# Eval("Email")%>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEmail" runat="server" Width="300px"
                                                Text='<%# Eval("Email")%>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtEmailNew" Width="300px" runat="server"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField  HeaderText="Actions">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="imgEdit" ToolTip="Edit" CssClass="glyphicon glyphicon-edit"
                                                        CommandName="Edit" AlternateText="Edit" />
                                            <asp:LinkButton runat="server" ID="imgDelete" ToolTip="Delete" CssClass="glyphicon glyphicon-remove"
                                                OnClientClick="return confirm('Do you want to delete?')"
                                                CommandArgument='<%# Eval("UserName")%>' OnClick="DeleteUser" />
                                     </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton runat="server" ID="imgUpdate" ToolTip="Update" CssClass="glyphicon glyphicon-floppy-save"
                                                        CommandName="Update" AlternateText="Update" />
                                            <asp:LinkButton runat="server" ID="imgCancel" ToolTip="Cancel"
                                                        CommandName="Cancel" AlternateText="Cancel" CssClass="glyphicon glyphicon-floppy-remove" />

                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:Button ID="btnAdd" runat="server" Text="Add"  CssClass="btn btn-primary btn-small" 
                                                OnClick="AddNewUser" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <AlternatingRowStyle BackColor="#EBF3FF" />
                            </asp:GridView>
                        </div>

                    </td>
                </tr>
            </tbody>
        </table>
    </form>
</body>
</html>
