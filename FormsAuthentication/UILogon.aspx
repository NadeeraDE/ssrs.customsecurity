<%@ Page Language="c#" CodeBehind="UILogon.aspx.cs" AutoEventWireup="false" Inherits="Microsoft.Samples.ReportingServices.CustomSecurity.UILogon" Culture="auto" UICulture="auto" %>

<%@ Import Namespace="Microsoft.Samples.ReportingServices.CustomSecurity" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Login</title>
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
                        <table class="msrs-header" cellpadding="0" cellspacing="0" border="0" width="100%">
                            <tbody>
                                <tr>
                                    <td class="msrs-logo" width="36">
                                        <img src="http://www.monumentconsulting.com/site/wp-content/themes/monument/images/logo.png" alt="Settings" style="height: 50px; width: 200px; border-width: 0px; display: inline;"></td>
                                    <td>
                                        <p class="msrs-site_title">MONUMENT Reporting Server</p>
                                        <p class="msrs-page_title">Login</p>
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
        <form id="Form1" method="post" runat="server" class="form-signin">
            <h2 class="form-signin-heading" style="font-size: 20px;">Please sign in</h2>
            <label class="sr-only" for="TxtUser">Username</label>
            <asp:TextBox ID="TxtUser" runat="server" CssClass="form-control" TabIndex="1" placeholder="Username"></asp:TextBox>
            <label class="sr-only" for="TxtPwd">Password</label>
            <asp:TextBox ID="TxtPwd" runat="server" CssClass="form-control" TabIndex="2" TextMode="Password" placeholder="Password"></asp:TextBox>
            <asp:Button ID="BtnLogon" runat="server" Text="Sign in" TabIndex="3" CssClass="btn btn-lg btn-primary btn-block"></asp:Button>
            <a href="<%=string.Format("/{0}/pages/forgotpassword.aspx",ConfigHelper.ReportManagerRootName) %>" class="btn btn-lg btn-primary btn-block">Forgot Password ?</a>
            <div style="float: left; width: 600px;">
                <asp:Label ID="lblMessage" ForeColor="Red"
                    runat="server"></asp:Label>
            </div>
            <asp:Button ID="BtnRegister" runat="server" Text="Register" Visible="False" TabIndex="3" CssClass="btn btn-lg btn-primary btn-block"></asp:Button>

        </form>
    </div>
</body>
</html>
