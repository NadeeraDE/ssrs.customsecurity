<%@ Page Language="c#" CodeBehind="Customizations.aspx.cs" AutoEventWireup="false" Inherits="Microsoft.Samples.ReportingServices.CustomSecurity.Customizations" Culture="auto" UICulture="auto" %>
<%@ Import Namespace="Microsoft.Samples.ReportingServices.CustomSecurity.helpers" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    </form>
    <%if (SessionHelper.IsAdminUser)
      { %>
    <script type="text/Javascript">
        window.parent.SsrsGlobal.EnableUserManagement();
    </script>
    <%} %>
</body>
</html>
