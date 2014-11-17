<%@ Page Language="c#" CodeBehind="ForgotPassword.aspx.cs" AutoEventWireup="false" Inherits="Microsoft.Samples.ReportingServices.CustomSecurity.ForgotPassword" Culture="auto" UICulture="auto" %>

<%@ Import Namespace="Microsoft.Samples.ReportingServices.CustomSecurity" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Forgot Password</title>
    <link href="/<%=ConfigHelper.ReportManagerRootName %>/styles/ReportingServices.css" type="text/css" rel="stylesheet">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.1/css/bootstrap.min.css">
    <style type="text/css">
        .form-signin {
            max-width: 330px;
            padding: 15px;
            margin: 0 auto;
        }

            .form-signin .form-signin-heading,
            .form-signin .checkbox {
                margin-bottom: 10px;
            }

            .form-signin .checkbox {
                font-weight: normal;
            }

            .form-signin .form-control {
                position: relative;
                height: auto;
                -webkit-box-sizing: border-box;
                -moz-box-sizing: border-box;
                box-sizing: border-box;
                padding: 10px;
                font-size: 16px;
            }

                .form-signin .form-control:focus {
                    z-index: 2;
                }

            .form-signin input[type="email"] {
                margin-bottom: -1px;
                border-bottom-right-radius: 0;
                border-bottom-left-radius: 0;
            }

            .form-signin input[type="password"] {
                margin-bottom: 10px;
                border-top-left-radius: 0;
                border-top-right-radius: 0;
            }
    </style>
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
                                        <a href="<%=string.Format("/{0}/pages/uilogon.aspx",ConfigHelper.ReportManagerRootName) %>">Login</a>
                                    </span></td>
                                    <td align="right"></td>
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
                                        <p class="msrs-page_title">Forgot Password</p>
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
    <div style="width: 100%;" class="container">
        <h3>Enter Username to reset the password. Once reset, you will receive an email with the new password.</h3>
        <form id="Form1" method="post" runat="server" class="form-signin">
            <div style="float: left; width: 100%;">
                <br />
                <br />
                <label class="sr-only" for="txtUserName">Username</label>
                <asp:TextBox Style="float: left;" ID="txtUserName" TextMode="SingleLine" runat="server" CssClass="form-control" placeholder="Username"></asp:TextBox>
                <br />
                <span style="float: left; width: 400px; display: none;">OR</span>
                <br />
                <asp:Label Visible="false" Style="float: left; width: 200px;" ID="lblEmail" runat="server" Font-Size="Small" Font-Names="Verdana" Font-Bold="True">Email:</asp:Label>
                <asp:TextBox Visible="false" Style="float: left; width: 200px;" ID="txtEmail" TextMode="SingleLine" runat="server"></asp:TextBox>
                <asp:Button ID="btnReset" style="padding-top: 5px;"
                    runat="server" Text="Reset" CssClass="btn btn-lg btn-primary btn-block"></asp:Button>
            </div>
            <div style="float: left; width: 100%; margin-top: 10px;" class="">
                <div Visible="False" Style="width: 100%;" ID="lblError" runat="server" class="alert alert-danger"></div>

            </div>
            <div style="float: left; width: 100%; margin-top: 10px;" class="">
                <div Visible="False" Style="width: 100%" ID="lblSuccess" runat="server" class="alert alert-success">
                    Your password was reset successfully, an email was sent out to the email address associated with your account.
                    <br />Click <a href="<%=string.Format("/{0}/pages/uilogon.aspx",ConfigHelper.ReportManagerRootName) %>">here</a> to go back to Login.
                </div>

            </div>
        </form>
    </div>
</body>
</html>
