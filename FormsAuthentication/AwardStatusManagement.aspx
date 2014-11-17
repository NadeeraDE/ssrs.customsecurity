<%@ Page Language="c#" CodeBehind="AwardStatusManagement.aspx.cs" AutoEventWireup="true" Inherits="Microsoft.Samples.ReportingServices.CustomSecurity.AwardStatusManagement, Microsoft.Samples.ReportingServices.CustomSecurity" Culture="auto" UICulture="auto" %>

<%@ Import Namespace="Microsoft.Samples.ReportingServices.CustomSecurity" %>
<%@ Import Namespace="Microsoft.Samples.ReportingServices.CustomSecurity.helpers" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Award Status Management</title>
    <link href="/<%=ConfigHelper.ReportManagerRootName %>/styles/ReportingServices.css" type="text/css" rel="stylesheet">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.1/css/bootstrap.min.css">
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <table style="width: 100%;">
            <tbody>
                <tr>
                    <td valign="top">
                        <div style="display:none;">
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
                                                <a href="/<%=ConfigHelper.ReportManagerRootName %>/pages/UserManagement.aspx">User Management</a>
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
                        <table width="100%" class="msrs-normal" cellpadding="0" cellspacing="0" style="display:none;">
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
                </tr>
                <tr>
                    <td>
                        <div id="dvGridAwardStatus" style="padding: 10px; width: 100%;">
                            <asp:GridView ID="gvAwardStatus" runat="server" CssClass="table" UseAccessibleHeader="True" GridLines="None"
                                AutoGenerateColumns="false" AllowPaging="true" ShowFooter="true"
                                OnPageIndexChanging="OnPagingAwardStatus" OnRowEditing="EditAwardStatus"
                                OnRowUpdating="UpdateAwardStatus" OnRowCancelingEdit="CancelEditAwardStatus"
                                PageSize="50">
                                <Columns>
                                    <asp:TemplateField HeaderText="Award Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAwardStatus" runat="server"
                                                Text='<%# Eval("AwardStatus")%>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblAwardStatusEdit" runat="server"
                                                Text='<%# Eval("AwardStatus")%>'></asp:Label>
                                            <asp:HiddenField runat="server" ID="hdnAwardStatusIdEdit" Value='<%# Eval("AwardStatusId")%>' />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Once Engaged">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOnceEngaged" runat="server"
                                                Text='<%# Eval("OnceEngaged")%>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList Visible="True" Width="100px" ID="ddOnceEngaged" runat="server">
                                                 <asp:ListItem Text="True" Value="1"></asp:ListItem>
                                                 <asp:ListItem Text="False" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Positive">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPositive" runat="server"
                                                Text='<%# Eval("Positive")%>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList Visible="True" Width="100px" ID="ddPositive" runat="server">
                                                 <asp:ListItem Text="True" Value="1"></asp:ListItem>
                                                 <asp:ListItem Text="False" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Actions">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="imgEdit" ToolTip="Edit" CssClass="glyphicon glyphicon-edit"
                                                CommandName="Edit" AlternateText="Edit" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton runat="server" ID="imgUpdate" ToolTip="Update" CssClass="glyphicon glyphicon-floppy-save"
                                                CommandName="Update" AlternateText="Update" />
                                            <asp:LinkButton runat="server" ID="imgCancel" ToolTip="Cancel" CssClass="glyphicon glyphicon-floppy-remove"
                                                CommandName="Cancel" AlternateText="Cancel" />

                                        </EditItemTemplate>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <AlternatingRowStyle BackColor="#EBF3FF" />
                            </asp:GridView>
                        </div>
                        <div>
                            <asp:Label Visible="True" Style="width: 100%" ID="lblAwardStatusError" runat="server" ForeColor="Red" Font-Size="Small" Font-Names="Verdana" Font-Bold="True">
                            </asp:Label>
                        </div>
                        <br />
                        <br />
                    </td>
                </tr>
            </tbody>
        </table>
    </form>
</body>
</html>
