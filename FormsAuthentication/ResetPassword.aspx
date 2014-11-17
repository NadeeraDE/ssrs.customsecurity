<%@ Page Language="c#" CodeBehind="ResetPassword.aspx.cs" AutoEventWireup="false" Inherits="Microsoft.Samples.ReportingServices.CustomSecurity.ResetPassword" Culture="auto" UICulture="auto" %>

<%@ Import Namespace="Microsoft.Samples.ReportingServices.CustomSecurity" %>
<%@ Import Namespace="Microsoft.Samples.ReportingServices.CustomSecurity.helpers" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Reset Password</title>
    <link href="/<%=ConfigHelper.ReportManagerRootName %>/styles/ReportingServices.css" type="text/css" rel="stylesheet">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.1/css/bootstrap.min.css">
</head>
<body ms_positioning="GridLayout">
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
                                        <p class="msrs-page_title">Reset Password</p>
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
                <td></td>
            </tr>
        </tbody>
    </table>
    <form id="Form1" method="post" runat="server">
        <div style="width: 700px;">
            <div>
                <br />
                <asp:Label AssociatedControlID="txtCurrentPassword" Style="float: left; width: 300px;" ID="LblUser" runat="server" >Current Password:</asp:Label>
                <asp:TextBox  ID="txtCurrentPassword" TextMode="Password" runat="server" style="margin-bottom: 5px;width: 200px;"></asp:TextBox>
                <br />
                <asp:Label AssociatedControlID="txtNewPassword1" Style="float: left; width: 300px;" ID="lblEmail" runat="server" >New Password:</asp:Label>
                <asp:TextBox ID="txtNewPassword1" TextMode="Password" runat="server" style="margin-bottom: 5px;width: 200px;"></asp:TextBox>
                <br />
                <asp:Label AssociatedControlID="txtNewPassword2" Style="float: left; width: 300px;" ID="Label1" runat="server" >Confirm New Password:</asp:Label>
                <asp:TextBox ID="txtNewPassword2" TextMode="Password" runat="server" style="margin-bottom: 5px;width: 200px;"></asp:TextBox>
                <br />
                <div style="float: left; width: 500px; text-align: right;margin-top: 10px;">
                    <asp:Button ID="btnReset" CssClass="btn btn-lg btn-primary btn-block" style="width: 100px;float: right;"
                        runat="server" Text="Reset"></asp:Button>

                </div>
            </div>
            <br />
            <div style="float: left; width: 500px; margin-top: 10px;">
                <asp:Label Style="width: 100%" ID="lblError" runat="server" ForeColor="Red" Font-Size="Small" Font-Names="Verdana" Font-Bold="True"></asp:Label>

            </div>
        </div>
    </form>
</body>
</html>
