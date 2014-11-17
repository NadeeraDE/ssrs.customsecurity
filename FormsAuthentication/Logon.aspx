<%@ Page language="c#" Codebehind="Logon.aspx.cs" AutoEventWireup="false" Inherits="Microsoft.Samples.ReportingServices.CustomSecurity.Logon, Microsoft.Samples.ReportingServices.CustomSecurity" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
   <HEAD>
      <title>Monument BI Server</title>
      <meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
      <meta name="CODE_LANGUAGE" Content="C#">
      <meta name="vs_defaultClientScript" content="JavaScript">
      <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
   </HEAD>
   <body MS_POSITIONING="GridLayout">
      <form id="Form1" method="post" runat="server">
         <asp:Label id="Label1" style="Z-INDEX: 108; LEFT: 83px; POSITION: absolute; TOP: 110px; height: 33px;" runat="server"
            Width="416px" Font-Size="Medium" Font-Names="Verdana" meta:resourcekey="Label1Resource1" Text="MONUMENT Reporting Server" Font-Bold="True"></asp:Label>
        <asp:Image ID="img" runat="server" imageurl="http://www.monumentconsulting.com/site/wp-content/themes/monument/images/logo.png" />
         <asp:Label id="LblUser" style="Z-INDEX: 101; LEFT: 253px; POSITION: absolute; TOP: 152px; width: 133px; height: 23px;" runat="server" Font-Size="Small" Font-Names="Verdana" Font-Bold="True" meta:resourcekey="LblUserResource1">User Name:</asp:Label>
         <asp:Button id="BtnLogon" style="Z-INDEX: 106; LEFT: 532px; POSITION: absolute; TOP: 245px; width: 132px;"
            runat="server" Text="Logon" tabIndex="3" meta:resourcekey="BtnLogonResource1"></asp:Button>
         <asp:TextBox id="TxtPwd" style="Z-INDEX: 104; LEFT: 392px; POSITION: absolute; TOP: 184px; width: 265px;" runat="server"
            tabIndex="2" TextMode="Password" meta:resourcekey="TxtPwdResource1"></asp:TextBox>
         <asp:Label id="LblPwd" style="Z-INDEX: 102; LEFT: 255px; POSITION: absolute; TOP: 184px; width: 133px;" runat="server" Font-Size="Small" Font-Names="Verdana" Font-Bold="True" meta:resourcekey="LblPwdResource1">Password:</asp:Label>&nbsp;
         <asp:TextBox id="TxtUser" style="Z-INDEX: 104; LEFT: 392px; POSITION: absolute; TOP: 152px; width: 268px;" runat="server"
            tabIndex="1" meta:resourcekey="TxtUserResource1"></asp:TextBox>
         <asp:Button Visible="False" id="BtnRegister" style="Z-INDEX: 105; LEFT: 388px; POSITION: absolute; TOP: 246px; width: 131px; right: 891px;"
            runat="server" Text="Register User" tabIndex="4" meta:resourcekey="BtnRegisterResource1"></asp:Button>
         <asp:Label id="lblMessage" style="Z-INDEX: 107; LEFT: 324px; POSITION: absolute; TOP: 311px; height: 25px; width: 443px;"
            runat="server" meta:resourcekey="lblMessageResource1"></asp:Label>
      </form>
   </body>
</HTML>
