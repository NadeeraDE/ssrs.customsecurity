<%@ Page Language="c#" CodeBehind="RefManagement.aspx.cs" AutoEventWireup="true" Inherits="Microsoft.Samples.ReportingServices.CustomSecurity.RefManagement, Microsoft.Samples.ReportingServices.CustomSecurity" Culture="auto" UICulture="auto" %>

<%@ Import Namespace="Microsoft.Samples.ReportingServices.CustomSecurity" %>
<%@ Import Namespace="Microsoft.Samples.ReportingServices.CustomSecurity.helpers" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Ref Management</title>
    <link href="/<%=ConfigHelper.ReportManagerRootName %>/styles/ReportingServices.css" type="text/css" rel="stylesheet">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.1/css/bootstrap.min.css">
    <script language="JavaScript">
<!--
    function autoResize(id) {
        var newheight;
        var newwidth;

        if (document.getElementById) {
            newheight = document.getElementById(id).contentWindow.document.body.scrollHeight;
            newwidth = document.getElementById(id).contentWindow.document.body.scrollWidth;
        }

        document.getElementById(id).height = (newheight) + "px";
        //document.getElementById(id).width = (newwidth) + "px";
    }
    //-->
    </script>
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
                                            <p class="msrs-page_title">Reference Management</p>
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
                    <td style="padding-left: 10px; padding-top: 20px;">

                        <label for="ddlReferenceList">Select a Reference : </label>
                        <select id="ddlReferenceList">
                            <option value="/<%=ConfigHelper.ReportManagerRootName %>/pages/DivisionManagement.aspx">Division-BU Reference</option>
                            <option value="/<%=ConfigHelper.ReportManagerRootName %>/pages/OfficeManagement.aspx">Office-New Office Reference</option>
                            <option value="/<%=ConfigHelper.ReportManagerRootName %>/pages/BidStatusManagement.aspx">Bid Status Reference</option>
                            <option value="/<%=ConfigHelper.ReportManagerRootName %>/pages/AwardStatusManagement.aspx">Award Status Reference</option>
                            <option value="/<%=ConfigHelper.ReportManagerRootName %>/pages/ProjectStatusManagement.aspx">Project Status Reference</option>
                            <option value="/<%=ConfigHelper.ReportManagerRootName %>/pages/DisEngagementStatusManagement.aspx">Dis-engagement Status Reference</option>
                        </select>
                        <br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td></td>
                </tr>
            </tbody>
        </table>

        <iframe id="refernceIframe" frameBorder="0" style="width: 50%; border: none;" onload="autoResize('refernceIframe')"></iframe>
    </form>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <script type="text/javascript">
        $(function () {
            var iframeUrl = $('#ddlReferenceList').val();
            $('#refernceIframe').attr("src", iframeUrl);
            $("#ddlReferenceList").change(function () {
                var iframeUrl = $('#ddlReferenceList').val();
                $('#refernceIframe').attr("src", iframeUrl);
            });
        });
    </script>
</body>
</html>
